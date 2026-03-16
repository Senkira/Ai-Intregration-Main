# Analysis: การเปลี่ยนไปใช้ Dapper ทั้งหมด

> **วัตถุประสงค์:** วิเคราะห์ความเหมาะสมของการเปลี่ยนจาก EF Core + UoW Pattern  
> ไปเป็น **Dapper** ทั้งหมดในโปรเจค OmniChannel / RL Series  
> **วันที่:** 4 มีนาคม 2026

---

## สารบัญ

| # | หัวข้อ |
|---|--------|
| 1 | [สถานะปัจจุบัน (As-Is)](#1-สถานะปัจจุบัน-as-is) |
| 2 | [ข้อดีของการเปลี่ยนไป Dapper ทั้งหมด](#2-ข้อดีของการเปลี่ยนไป-dapper-ทั้งหมด) |
| 3 | [ข้อเสียและความเสี่ยง](#3-ข้อเสียและความเสี่ยง) |
| 4 | [Dapper Architecture Pattern ที่เสนอ](#4-dapper-architecture-pattern-ที่เสนอ) |
| 5 | [ตัวอย่าง Code Mapping: EF Core → Dapper](#5-ตัวอย่าง-code-mapping-ef-core--dapper) |
| 6 | [Migration Plan (Step-by-Step)](#6-migration-plan-step-by-step) |
| 7 | [ทางเลือกที่แนะนำ](#7-ทางเลือกที่แนะนำ) |
| 8 | [สรุปการตัดสินใจ](#8-สรุปการตัดสินใจ) |

---

## 1. สถานะปัจจุบัน (As-Is)

### Stack ปัจจุบัน

```
ASP.NET Core Web API (.NET 8 / .NET 10)
│
├── Controllers/
├── Services/          ← Business Logic
├── DataAccess/
│   └── {DbName}Db/
│       ├── Interface/
│       │   ├── I{DbName}DataSets.cs   ← DbSet declarations
│       │   └── I{DbName}UoW.cs        ← UoW interface
│       ├── {DbName}DbContext.cs        ← EF Core DbContext
│       ├── {DbName}UoW.cs             ← Unit of Work impl
│       └── {Entity}Entity.cs          ← EF Core Entity
└── Models/
```

### NuGet Dependencies ปัจจุบัน

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.*" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.*" />
<PackageReference Include="Easybuy.WebApplication.CoreUnitOfWork" Version="*" />
```

### Pattern ปัจจุบัน: EF Core + Unit of Work Pool

```csharp
// Service ปัจจุบัน
public class CustomerService : ICustomerService
{
    private readonly IMainUoW _uow;
    
    public async Task<CustomerEntity?> GetByIdAsync(string id)
        => await _uow.Customers.FindAsync(id);

    public async Task<IEnumerable<CustomerEntity>> GetActiveAsync()
        => await _uow.Customers
            .Where(x => x.Status == "ACTIVE")
            .AsNoTracking()
            .ToListAsync();

    public async Task CreateAsync(CustomerEntity entity)
    {
        _uow.Customers.Add(entity);
        await _uow.SaveChangesAsync();
    }
}
```

---

## 2. ข้อดีของการเปลี่ยนไป Dapper ทั้งหมด

### 2.1 Performance

| Scenario | EF Core (Tracking) | EF Core (NoTracking) | Dapper |
|----------|:-----------------:|:--------------------:|:------:|
| Single row lookup | ~1.2ms | ~0.9ms | **~0.4ms** |
| 100 rows query | ~8ms | ~5ms | **~2ms** |
| 1000 rows query | ~60ms | ~35ms | **~12ms** |
| Complex JOIN (5 tables) | ~150ms | ~80ms | **~25ms** |
| Stored Procedure | ~15ms | ~12ms | **~5ms** |

> 📌 ตัวเลขเป็นการประมาณการ — ผลจริงขึ้นอยู่กับ Hardware, Query Complexity, Index

**เหตุที่ Dapper เร็วกว่า:**
- ❌ ไม่มี Change Tracking (Memory overhead หาย)
- ❌ ไม่มี Expression Tree compilation ทุก request
- ❌ ไม่มี Navigation Property resolution
- ✅ ส่ง SQL ตรงไปยัง DB โดยตรง
- ✅ IL Emit ใน Mapping เร็วกว่า Reflection

### 2.2 Simplicity — SQL ชัดเจน อ่านง่าย

```csharp
// EF Core — ไม่รู้ว่า SQL ที่ Generate ออกมาเป็นอะไร
var result = await _uow.Transactions
    .Include(t => t.Customer)
    .Where(t => t.Status == "P" && t.Amount > 1000)
    .OrderByDescending(t => t.CreatedDate)
    .Take(50)
    .ToListAsync();

// Dapper — รู้ชัดว่า SQL เป็นอะไร ปรับ Optimize ได้ทันที
const string sql = @"
    SELECT TOP 50
           t.transaction_id   AS TransactionId,
           t.amount           AS Amount,
           t.status           AS Status,
           t.created_date     AS CreatedDate,
           c.customer_name    AS CustomerName
    FROM   transaction t
    INNER JOIN customer c ON c.customer_id = t.customer_id
    WHERE  t.status = 'P'
      AND  t.amount > 1000
    ORDER BY t.created_date DESC";
```

### 2.3 ลด Dependency

```
ปัจจุบัน (EF Core):
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer  
- Microsoft.EntityFrameworkCore.Tools
- Easybuy.WebApplication.CoreUnitOfWork     ← Internal Library
- (รวม ~15 NuGet Transitive)

เปลี่ยนเป็น Dapper:
- Dapper                                    ← เดียว (~200KB)
- Microsoft.Data.SqlClient                  ← ต้องมีอยู่แล้ว
- (รวม ~3 NuGet)
```

### 2.4 หลุดพ้นจาก Internal Library Dependency

> ปัจจุบัน `Easybuy.WebApplication.CoreUnitOfWork` เป็น **Internal Package**  
> ที่ต้อง Feed จาก Internal NuGet Server  
> ถ้าเซิร์ฟเวอร์ล่ม หรือ Package หยุด Maintain → Build ล้มเหลวทุกโปรเจค

### 2.5 Stored Procedure Support ดีกว่า

```csharp
// Dapper — SP ใช้งานได้ Natural
var result = await _db.QueryAsync<TransactionModel>(
    "sp_GetCustomerStatement",
    new 
    { 
        CustomerId  = customerId,
        FromDate    = from,
        ToDate      = to,
        PageNo      = page,
        PageSize    = size
    },
    commandType: CommandType.StoredProcedure
);

// EF Core — SP ใช้งานได้ แต่ Awkward
var result = await _context.Database
    .SqlQueryRaw<TransactionModel>(
        "EXEC sp_GetCustomerStatement @CustomerId, @FromDate, @ToDate, @PageNo, @PageSize",
        new SqlParameter("@CustomerId", customerId),
        new SqlParameter("@FromDate", from),
        new SqlParameter("@ToDate", to),
        new SqlParameter("@PageNo", page),
        new SqlParameter("@PageSize", size)
    ).ToListAsync();
```

---

## 3. ข้อเสียและความเสี่ยง

### 3.1 ❌ ไม่มี Database Migration

| ด้าน | EF Core | Dapper |
|------|---------|--------|
| Schema Version Control | `dotnet ef migrations add` | ❌ ต้องทำเอง |
| Schema Apply | `dotnet ef database update` | ❌ ต้องใช้ SQL Script |
| Rollback | Migration Rollback | ❌ ต้องเขียน Down Script |

**แก้ไขได้ด้วย:**
- [DbUp](https://dbup.readthedocs.io/) — SQL Script Migration Tool
- [Flyway](https://flywaydb.org/) — Database Migration (External Tool)
- Manual SQL Script + Version control ใน `/dbs/migrations/`

### 3.2 ❌ SQL String — Risk Typo & SQL Injection

```csharp
// ❌ อันตราย — ถ้าทำแบบนี้
string sql = $"SELECT * FROM customer WHERE name = '{name}'"; // SQL Injection!

// ✅ ปลอดภัย — ต้องใช้ Parameterized เสมอ
string sql = "SELECT * FROM customer WHERE name = @name";
await _db.QueryAsync<Customer>(sql, new { name });
```

**กฎที่ต้องบังคับ:** ห้ามใช้ String Interpolation ใน SQL — ต้องใช้ `@parameter` เสมอ

### 3.3 ❌ ไม่มี Change Tracking → ต้อง Manage State เอง

```csharp
// EF Core — แก้ง่าย
var entity = await _uow.Customers.FindAsync(id);
entity.Status = "INACTIVE";
await _uow.SaveChangesAsync(); // EF รู้ว่า UPDATE อะไร

// Dapper — ต้องเขียน UPDATE เอง
const string sql = "UPDATE customer SET status = @status WHERE customer_id = @id";
await _db.ExecuteAsync(sql, new { status = "INACTIVE", id });
```

### 3.4 ❌ Migration Cost จาก EF Core → Dapper

| Component | จำนวนที่ต้อง Rewrite |
|-----------|:--------------------:|
| DbContext files | เปลี่ยนเป็น Connection Setup |
| Entity files | เปลี่ยนเป็น Model/DTO |
| UoW files | เปลี่ยนเป็น Repository |
| Service files | แก้ Method calls |
| Registration (DI) | แก้ใน Program.cs |

> สำหรับโปรเจค RL Series (~15 repos) — ต้องใช้เวลา **2-4 สัปดาห์** เพื่อ Migrate ทั้งหมด

### 3.5 ❌ ไม่มี Relationship Management

```csharp
// EF Core — ดึง related data ง่าย
var customer = await _uow.Customers
    .Include(c => c.Transactions)
    .Include(c => c.Addresses)
    .FirstOrDefaultAsync(c => c.CustomerId == id);

// Dapper — ต้องเขียน JOIN หรือ Multi-query เอง
const string sql = @"
    SELECT c.*, t.*, a.*
    FROM customer c
    LEFT JOIN transaction t ON t.customer_id = c.customer_id
    LEFT JOIN address a     ON a.customer_id = c.customer_id
    WHERE c.customer_id = @id";

// หรือใช้ Multi-mapping
await _db.QueryAsync<Customer, Transaction, Address, Customer>(
    sql, 
    (customer, transaction, address) => { /* manual mapping */ },
    new { id }
);
```

---

## 4. Dapper Architecture Pattern ที่เสนอ

### โครงสร้าง Folder

```
{ProjectName}/
├── Controllers/
├── Services/
│   ├── Interfaces/
│   │   └── I{Feature}Service.cs
│   └── {Feature}Service.cs
├── DataAccess/                          ← เปลี่ยน Pattern
│   ├── Connections/
│   │   ├── IDbConnectionFactory.cs      ← NEW: Connection Factory
│   │   └── DbConnectionFactory.cs
│   ├── Repositories/                    ← แทน UoW
│   │   ├── Interfaces/
│   │   │   ├── I{Feature}Repository.cs
│   │   │   └── IBaseRepository.cs
│   │   └── {Feature}Repository.cs
│   └── SqlQueries/                      ← NEW: SQL Constants
│       └── {Feature}Queries.cs
├── Models/
│   ├── Entities/                        ← DB Mapping Models
│   │   └── {Name}Entity.cs
│   └── {Feature}Model.cs
└── ...
```

### IDbConnectionFactory

```csharp
// DataAccess/Connections/IDbConnectionFactory.cs
public interface IDbConnectionFactory
{
    IDbConnection CreateMainConnection();
    IDbConnection CreateLogConnection();
}

// DataAccess/Connections/DbConnectionFactory.cs
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
        => _configuration = configuration;

    public IDbConnection CreateMainConnection()
        => new SqlConnection(_configuration.GetConnectionString("MainDb"));

    public IDbConnection CreateLogConnection()
        => new SqlConnection(_configuration.GetConnectionString("LogDb"));
}
```

### Base Repository (Generic)

```csharp
// DataAccess/Repositories/BaseRepository.cs
public abstract class BaseRepository
{
    protected readonly IDbConnectionFactory _connectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql, 
        object? param = null,
        string? dbKey = null)
    {
        using var conn = CreateConnection(dbKey);
        return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(
        string sql, 
        object? param = null,
        string? dbKey = null)
    {
        using var conn = CreateConnection(dbKey);
        return await conn.QueryAsync<T>(sql, param);
    }

    protected async Task<int> ExecuteAsync(
        string sql, 
        object? param = null,
        string? dbKey = null)
    {
        using var conn = CreateConnection(dbKey);
        return await conn.ExecuteAsync(sql, param);
    }

    protected async Task<T> ExecuteInTransactionAsync<T>(
        Func<IDbConnection, IDbTransaction, Task<T>> action,
        string? dbKey = null)
    {
        using var conn = CreateConnection(dbKey);
        await conn.OpenAsync();
        using var transaction = conn.BeginTransaction();
        try
        {
            var result = await action(conn, transaction);
            transaction.Commit();
            return result;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private IDbConnection CreateConnection(string? dbKey) => dbKey switch
    {
        "Log"  => _connectionFactory.CreateLogConnection(),
        _      => _connectionFactory.CreateMainConnection()
    };
}
```

### SQL Constants (แยก SQL ออกจาก Logic)

```csharp
// DataAccess/SqlQueries/CustomerQueries.cs
public static class CustomerQueries
{
    public const string GetById = @"
        SELECT customer_id   AS CustomerId,
               customer_name AS CustomerName,
               status        AS Status,
               created_date  AS CreatedDate
        FROM   customer
        WHERE  customer_id = @customerId";

    public const string GetActiveList = @"
        SELECT customer_id   AS CustomerId,
               customer_name AS CustomerName,
               status        AS Status
        FROM   customer
        WHERE  status = 'ACTIVE'
        ORDER BY customer_name";

    public const string Insert = @"
        INSERT INTO customer (customer_id, customer_name, status, created_date)
        VALUES (@CustomerId, @CustomerName, @Status, @CreatedDate)";

    public const string UpdateStatus = @"
        UPDATE customer
        SET    status       = @Status,
               updated_date = GETDATE()
        WHERE  customer_id  = @CustomerId";

    public const string Delete = @"
        DELETE FROM customer WHERE customer_id = @CustomerId";
}
```

### Repository Implementation

```csharp
// DataAccess/Repositories/Interfaces/ICustomerRepository.cs
public interface ICustomerRepository
{
    Task<CustomerEntity?> GetByIdAsync(string customerId);
    Task<IEnumerable<CustomerEntity>> GetActiveListAsync();
    Task<int> InsertAsync(CustomerEntity entity);
    Task<int> UpdateStatusAsync(string customerId, string status);
    Task<int> DeleteAsync(string customerId);
}

// DataAccess/Repositories/CustomerRepository.cs
public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory) { }

    public async Task<CustomerEntity?> GetByIdAsync(string customerId)
        => await QuerySingleOrDefaultAsync<CustomerEntity>(
            CustomerQueries.GetById, new { customerId });

    public async Task<IEnumerable<CustomerEntity>> GetActiveListAsync()
        => await QueryAsync<CustomerEntity>(CustomerQueries.GetActiveList);

    public async Task<int> InsertAsync(CustomerEntity entity)
        => await ExecuteAsync(CustomerQueries.Insert, entity);

    public async Task<int> UpdateStatusAsync(string customerId, string status)
        => await ExecuteAsync(CustomerQueries.UpdateStatus, new { customerId, status });

    public async Task<int> DeleteAsync(string customerId)
        => await ExecuteAsync(CustomerQueries.Delete, new { customerId });
}
```

### DI Registration

```csharp
// Program.cs
// ลบ EF Core Registration
// builder.Services.AddDbContext<MainDbContext>(...); ← ลบ
// builder.Services.AddScoped<IMainUoW, MainUoW>();  ← ลบ

// เพิ่ม Dapper Registration
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
```

---

## 5. ตัวอย่าง Code Mapping: EF Core → Dapper

### Simple Query

```csharp
// ── EF Core (Before) ──────────────────────────────────────
var customer = await _uow.Customers
    .AsNoTracking()
    .FirstOrDefaultAsync(c => c.CustomerId == id);

// ── Dapper (After) ────────────────────────────────────────
var customer = await _customerRepository.GetByIdAsync(id);
```

### Complex Query + JOIN

```csharp
// ── EF Core (Before) ──────────────────────────────────────
var transactions = await _uow.Transactions
    .Include(t => t.Customer)
    .Where(t => t.CustomerId == customerId 
             && t.Status == "SUCCESS"
             && t.CreatedDate >= fromDate)
    .OrderByDescending(t => t.CreatedDate)
    .AsNoTracking()
    .ToListAsync();

// ── Dapper (After) ────────────────────────────────────────
const string sql = @"
    SELECT t.transaction_id  AS TransactionId,
           t.amount          AS Amount,
           t.status          AS Status,
           t.created_date    AS CreatedDate,
           c.customer_name   AS CustomerName
    FROM   transaction t
    INNER JOIN customer c ON c.customer_id = t.customer_id
    WHERE  t.customer_id = @customerId
      AND  t.status      = 'SUCCESS'
      AND  t.created_date >= @fromDate
    ORDER BY t.created_date DESC";

var transactions = await _db.QueryAsync<TransactionModel>(
    sql, new { customerId, fromDate });
```

### INSERT with Transaction

```csharp
// ── EF Core (Before) ──────────────────────────────────────
_uow.Customers.Add(entity);
_uow.Logs.Add(logEntity);
await _uow.SaveChangesAsync(); // EF จัดการ Transaction ให้

// ── Dapper (After) ────────────────────────────────────────
await ExecuteInTransactionAsync(async (conn, tx) =>
{
    await conn.ExecuteAsync(CustomerQueries.Insert, entity, tx);
    await conn.ExecuteAsync(LogQueries.Insert, logEntity, tx);
    return true;
});
```

### Stored Procedure

```csharp
// ── EF Core (Before) ──────────────────────────────────────
var result = await _uow.Context.Database
    .SqlQueryRaw<StatementModel>(
        "EXEC sp_GetStatement @customerId, @fromDate, @toDate",
        new SqlParameter("@customerId", id),
        new SqlParameter("@fromDate", from),
        new SqlParameter("@toDate", to))
    .ToListAsync();

// ── Dapper (After) ────────────────────────────────────────
var result = await _db.QueryAsync<StatementModel>(
    "sp_GetStatement",
    new { customerId = id, fromDate = from, toDate = to },
    commandType: CommandType.StoredProcedure);
```

---

## 6. Migration Plan (Step-by-Step)

### Phase 1: Setup & Foundation (สัปดาห์ที่ 1)

```
[ ] 1. เพิ่ม NuGet: Dapper, Dapper.Contrib (optional)
[ ] 2. สร้าง IDbConnectionFactory + DbConnectionFactory
[ ] 3. สร้าง BaseRepository (Generic)
[ ] 4. สร้าง Folder Structure ใหม่: Repositories/, SqlQueries/
[ ] 5. ทดสอบ Connection ทุก DB ที่ใช้ใน Project
```

### Phase 2: Repository Layer (สัปดาห์ที่ 1-2)

```
[ ] 1. สร้าง SQL Constants ใน SqlQueries/
[ ] 2. สร้าง Repository Interface สำหรับแต่ละ Feature
[ ] 3. Implement Repository ทีละ Feature
[ ] 4. เขียน Unit Test สำหรับ Repository
```

### Phase 3: Service Layer Refactor (สัปดาห์ที่ 2-3)

```
[ ] 1. เปลี่ยน IMainUoW → ICustomerRepository, ITransactionRepository
[ ] 2. แก้ Service Method ให้เรียก Repository แทน UoW
[ ] 3. ย้าย Business Logic ที่ Leak เข้า Query ออกมาใน Service
[ ] 4. ทดสอบ Service ทุก Method
```

### Phase 4: DI & Cleanup (สัปดาห์ที่ 3-4)

```
[ ] 1. แก้ Program.cs / DI Registration
[ ] 2. ลบ EF Core Packages ที่ไม่ใช้แล้ว
[ ] 3. ลบ Entity Files (ถ้าไม่ใช้ Migration)
[ ] 4. ลบ DbContext, UoW Files
[ ] 5. Integration Test ทุก Endpoint
[ ] 6. Performance Test เปรียบเทียบ Before/After
```

### Rollback Plan

> ถ้า Migration มีปัญหา ให้ทำแบบ **Side-by-side** ก่อน  
> (EF Core + Dapper อยู่ด้วยกัน) แล้วค่อยตัด EF Core ออกทีหลัง

---

## 7. ทางเลือกที่แนะนำ

### Option A: เปลี่ยนเป็น Dapper ทั้งหมด ✅ Recommended สำหรับ RL Series

```
เหมาะกับ: RLSR061, RLSR250, RLSR251, RLSR261 ฯลฯ
เหตุผล:
- Query ซับซ้อน (Multi-table JOIN)
- ใช้ Stored Procedure อยู่แล้วหลายจุด
- Read-heavy (Transaction log, Statement)
- ต้องการ Performance สูง
- ไม่ต้องการ Migration (Schema ถูก Manage โดย DBA)
```

**Pros:** Performance ↑↑, Code ชัดเจน, ลด Dependency  
**Cons:** SQL เขียนเอง, ไม่มี Migration  

### Option B: Hybrid — Dapper + EF Core ✅ Recommended สำหรับ Project ใหม่

```
ใช้ EF Core สำหรับ:
- Migration management
- Simple CRUD (INSERT/UPDATE/DELETE)

ใช้ Dapper สำหรับ:
- Complex Query (Report, Statement, Dashboard)
- Stored Procedure calls
- Bulk read operations
```

**Pros:** ได้ทั้ง Performance และ Migration  
**Cons:** 2 Pattern ใน Project เดียว → ต้อง Document ชัด  

### Option C: คง EF Core ไว้ + ปรับ Optimize

```
ใช้:
- AsNoTracking() ทุก Query ที่ไม่ต้องการ Update
- Compiled Queries สำหรับ Hot Path
- ExecuteUpdate / ExecuteDelete (EF Core 8+)
- SqlQuery<T>() สำหรับ Complex Query
```

**Pros:** ไม่ต้อง Rewrite, มี Migration  
**Cons:** Performance ยังต่ำกว่า Dapper  

---

## 8. สรุปการตัดสินใจ

### Matrix ประเมิน

| เกณฑ์ | น้ำหนัก | EF Core Only | Dapper Only | Hybrid |
|-------|:-------:|:------------:|:-----------:|:------:|
| Performance | 30% | 6/10 | 10/10 | 8/10 |
| Code Maintainability | 20% | 8/10 | 7/10 | 7/10 |
| Migration Risk | 20% | 9/10 | 6/10 | 8/10 |
| SQL Control | 15% | 5/10 | 10/10 | 8/10 |
| Team Learning | 10% | 8/10 | 7/10 | 6/10 |
| Dependency Reduction | 5% | 4/10 | 10/10 | 7/10 |
| **รวม** | 100% | **6.9** | **8.5** | **7.6** |

### 🏆 การตัดสินใจสุดท้าย (Finalized)

> **📌 ตัดสินใจเมื่อ:** 4 มีนาคม 2026

> **สำหรับโปรเจค RL Series ที่มีอยู่แล้ว → ✅ Option A: เปลี่ยน Dapper ทั้งหมด**
>
> **เหตุผล:**
> 1. Schema ถูก Manage โดย DBA → ไม่ต้องการ EF Migration
> 2. ระบบ Read-heavy (Transaction, Statement, Report) → Performance สำคัญมาก
> 3. มี Stored Procedure อยู่แล้วจำนวนมาก → Dapper รองรับได้ Natural กว่า
> 4. ลด Dependency จาก Internal Library (`Easybuy.CoreUnitOfWork`) → ลด Build Risk
> 5. SQL ชัดเจน → DBA ร่วม Review และ Optimize ได้ง่ายกว่า

> **สำหรับโปรเจคใหม่ → ✅ Option A เช่นกัน (Dapper เป็นหลัก)**
>
> จากผลวิจัยเพิ่มเติม ADO.NET vs Dapper พบว่า:
> - Dapper ช้ากว่า ADO.NET เพียง **≤5%** (34μs ต่อ query) ใน .NET 8/9/10
> - **ไม่จำเป็นต้องใช้ ADO.NET ดิบ** — Dapper เพียงพอสำหรับทุก use case ของเรา
> - Stack Overflow รองรับ 2-3 พันล้าน page views/เดือน ด้วย Dapper โดยไม่ต้อง fall back ไป ADO.NET
> - ถ้าต้องการ SqlBulkCopy หรือ Streaming (ซึ่งไม่มีในโปรเจคเรา) สามารถ drop ลง ADO.NET เฉพาะจุดได้

### Coding Standard ที่บังคับใช้ (อัปเดตใน Coding_Standard_Criteria.md แล้ว)

| # | กฎ | ระดับ |
|---|-----|-------|
| D.1 | ห้ามใช้ String Interpolation ใน SQL — ต้องใช้ `@parameter` เสมอ | 🔴 บังคับ |
| D.2 | SQL ต้องแยกเป็น `static class` ใน `SqlQueries/` — ห้ามฝังใน Method | 🔴 บังคับ |
| D.3 | Connection ต้องปิดด้วย `using` เสมอ — ห้าม Leave Connection Open | 🔴 บังคับ |
| D.4 | ทุก Multi-operation ต้องใช้ Transaction — ห้าม Execute แยกโดยไม่มี TX | 🔴 บังคับ |
| D.5 | Column Alias ใน SQL ต้องตรงกับ Property Name ของ Model เสมอ | 🔴 บังคับ |
| D.6 | ต้องใช้ Repository Pattern — ห้าม inject `IDbConnection` เข้า Service โดยตรง | 🔴 บังคับ |
| D.7 | ต้องใช้ `IDbConnectionFactory` สำหรับสร้าง Connection — ห้าม `new SqlConnection()` ตรงใน Repository | 🔴 บังคับ |

---

*เอกสารนี้จัดทำเพื่อประกอบการตัดสินใจ Migration Strategy*  
*วันที่: 4 มีนาคม 2026 | อ้างอิง: [Database_Access_Methods_NET10.md](./Database_Access_Methods_NET10.md)*  
*อัปเดตล่าสุด: 4 มีนาคม 2026 — Finalized Decision*
