# Research: วิธีการเชื่อมต่อฐานข้อมูลใน .NET 10

> **วัตถุประสงค์:** วิจัยและเปรียบเทียบวิธีการ Data Access ที่มีอยู่ใน .NET 10  
> เพื่อประกอบการตัดสินใจ ว่าจะ **คง EF Core**, **เปลี่ยนไป Dapper**, หรือใช้ **ผสมกัน**  
> **วันที่วิจัย:** 4 มีนาคม 2026  
> **Context:** ASP.NET Core Web API — ระบบ OmniChannel / RL Series

---

## สารบัญ

| # | หัวข้อ |
|---|--------|
| 1 | [ภาพรวม Data Access Options ใน .NET 10](#1-ภาพรวม-data-access-options-ใน-net-10) |
| 2 | [ADO.NET (Data Object Access)](#2-adonet--raw-sql--dao-pattern) |
| 3 | [Entity Framework Core 10](#3-entity-framework-core-10) |
| 4 | [Dapper](#4-dapper) |
| 5 | [Dapper + Extensions](#5-dapper--extensions) |
| 6 | [RepoDB](#6-repodb) |
| 7 | [NHibernate](#7-nhibernate) |
| 8 | [SqlKata](#8-sqlkata) |
| 9 | [Stored Procedure Only Pattern](#9-stored-procedure-only-pattern) |
| 10 | [ตารางเปรียบเทียบรวม](#10-ตารางเปรียบเทียบรวม) |
| 11 | [สรุปข้อเสนอแนะ](#11-สรุปข้อเสนอแนะ) |

---

## 1. ภาพรวม Data Access Options ใน .NET 10

```
.NET 10 Data Access Landscape
│
├── Low-Level (Full Control)
│   ├── ADO.NET (SqlConnection, SqlCommand, SqlDataReader)
│   └── Stored Procedure + ADO.NET
│
├── Micro-ORM (กลาง — SQL + Mapping)
│   ├── Dapper ⭐ (Stack Overflow)
│   ├── Dapper.Contrib / Dapper.FluentMap
│   ├── RepoDB
│   └── SqlKata (Query Builder)
│
└── Full ORM (High Abstraction)
    ├── Entity Framework Core 10 ⭐ (Microsoft)
    └── NHibernate
```

**ใน Context ของโปรเจคเรา** ใช้:
- ✅ EF Core + Unit of Work Pattern (ปัจจุบัน)
- ✅ Stored Procedure ผ่าน EF Core `.FromSqlRaw()` และ `.ExecuteSqlRaw()`
- ❌ Dapper (ยังไม่ได้ใช้)

---

## 2. ADO.NET / Raw SQL / DAO Pattern

### คืออะไร
`ADO.NET` คือ Foundation ของทุก Data Access ใน .NET — ทุก Library ข้างบนล้วน Build บน ADO.NET

### วิธีใช้งาน

```csharp
// Program.cs / DI Registration
builder.Services.AddTransient<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("MainDb")));

// DAO Class
public class CustomerDao
{
    private readonly IDbConnection _connection;
    public CustomerDao(IDbConnection connection) => _connection = connection;

    public async Task<Customer?> GetByIdAsync(string customerId)
    {
        const string sql = "SELECT * FROM customer WHERE customer_id = @customerId";
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@customerId", customerId);
        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Customer
            {
                CustomerId = reader.GetString(0),
                Name = reader.GetString(1)
            };
        }
        return null;
    }
}
```

### ข้อดี
- ✅ Performance สูงสุด — Zero overhead
- ✅ Full control ทุก query
- ✅ ไม่มี dependency นอก BCL
- ✅ เหมาะกับ Stored Procedure heavy systems

### ข้อเสีย
- ❌ Code verbose มาก (Boilerplate สูง)
- ❌ ต้อง Map Column → Property เอง ทุก Property
- ❌ Error-prone — ผิด column name = Runtime error
- ❌ ยาก Maintain เมื่อ Schema เปลี่ยน
- ❌ ไม่มี Type Safety

### เหมาะกับ
- ระบบที่ต้องการ Performance สุดขีด
- ระบบที่ใช้ Stored Procedure 100%
- Legacy system ที่มี DBA ดูแล Schema

---

## 3. Entity Framework Core 10

### คืออะไร
Full ORM จาก Microsoft — Map C# Class ↔ Database Table โดยอัตโนมัติ  
ปัจจุบัน **EF Core 10** (Release พร้อม .NET 10) มีฟีเจอร์ใหม่ดังนี้:

### ฟีเจอร์ใหม่ใน EF Core 10
- **AOT (Ahead-of-Time) Compilation** — ลด startup time, เหมาะ Container
- **Complex Type Improvements** — Map Nested Object ได้ดีขึ้น
- **ExecuteUpdate / ExecuteDelete** — Bulk operations ไม่ต้อง Load entity
- **JSON Column Support** — Map JSON column เป็น C# object ได้
- **Raw SQL Improvement** — `SqlQuery<T>()` supports arbitrary types (ไม่ต้องเป็น Entity)
- **Compiled Queries** — Pre-compile LINQ → SQL สำหรับ Hot Path

### วิธีใช้งาน (Current Pattern ของโปรเจค)

```csharp
// DI Registration
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MainDb")));

// Interface
public interface IMainDataSets
{
    DbSet<CustomerEntity> Customers { get; }
}

public interface IMainUoW : IUnitOfWork<MainDbContext>, IMainDataSets { }

// DbContext
public class MainDbContext : DbContext, IMainDataSets
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CustomerEntity>(entity =>
        {
            entity.ToTable("customer");
            entity.HasKey(x => x.CustomerId);
        });
    }
}

// Service ใช้งานผ่าน UoW
public class CustomerService : ICustomerService
{
    private readonly IMainUoW _uow;
    public CustomerService(IMainUoW uow) => _uow = uow;

    public async Task<CustomerEntity?> GetByIdAsync(string id)
        => await _uow.Customers.FindAsync(id);
}
```

### EF Core 10 — Raw SQL / SP Support

```csharp
// ใช้ Stored Procedure
var result = await _uow.Context.Database
    .ExecuteSqlRawAsync("EXEC sp_GetCustomer @customerId", 
        new SqlParameter("@customerId", id));

// SqlQuery สำหรับ Arbitrary Type (ใหม่ใน EF8+)
var result = await _uow.Context.Database
    .SqlQuery<CustomerSummaryModel>($"SELECT ...")
    .ToListAsync();
```

### ข้อดี
- ✅ **ลด Boilerplate** — ไม่ต้อง Map Column เอง
- ✅ **LINQ** — Type-safe query
- ✅ **Migration** — Schema versioning อัตโนมัติ
- ✅ **Change Tracking** — Track object state
- ✅ **Lazy / Eager Loading** — Relationship management
- ✅ **Microsoft Support** — Long-term support
- ✅ **AOT Ready** ใน EF Core 10

### ข้อเสีย
- ❌ **Performance ต่ำกว่า Dapper / ADO.NET** บน Complex query
- ❌ **Generated SQL ไม่สวย** — อาจไม่ Optimize
- ❌ **Startup time** (แก้ได้ด้วย AOT ใน EF Core 10)
- ❌ **Memory overhead** — Change Tracking
- ❌ **Learning curve** สูงกว่า Dapper
- ❌ **Debugging ยาก** เมื่อ SQL ถูก Generate ผิด

### เหมาะกับ
- CRUD-heavy systems ที่ Business logic ซับซ้อน
- ระบบที่ต้องการ Migration management
- Team ที่ไม่ถนัด SQL

---

## 4. Dapper

### คืออะไร
**Micro-ORM** สร้างโดย Stack Overflow — ทำงานบน `IDbConnection` ตรง  
เพิ่มความสามารถ **Object Mapping** ให้ ADO.NET — แต่ SQL ยังเขียนเอง

### Installation

```xml
<!-- .csproj -->
<PackageReference Include="Dapper" Version="2.1.*" />
```

### วิธีใช้งานพื้นฐาน

```csharp
// DI Registration
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(configuration.GetConnectionString("MainDb")));

// Repository / DAO
public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnection _db;
    public CustomerRepository(IDbConnection db) => _db = db;

    // Query — Map Result → Object อัตโนมัติ
    public async Task<CustomerModel?> GetByIdAsync(string customerId)
    {
        const string sql = @"
            SELECT customer_id   AS CustomerId,
                   customer_name AS CustomerName,
                   status        AS Status
            FROM   customer
            WHERE  customer_id = @customerId";

        return await _db.QuerySingleOrDefaultAsync<CustomerModel>(sql, 
            new { customerId });
    }

    // Execute — INSERT / UPDATE / DELETE
    public async Task<int> CreateAsync(CustomerModel model)
    {
        const string sql = @"
            INSERT INTO customer (customer_id, customer_name, status)
            VALUES (@CustomerId, @CustomerName, @Status)";

        return await _db.ExecuteAsync(sql, model);
    }

    // Stored Procedure
    public async Task<IEnumerable<TransactionModel>> GetTransactionsAsync(string customerId)
    {
        return await _db.QueryAsync<TransactionModel>(
            "sp_GetCustomerTransactions",
            new { customerId },
            commandType: CommandType.StoredProcedure);
    }

    // Multi-result (Multiple SELECT)
    public async Task<(CustomerModel? Customer, IEnumerable<AddressModel> Addresses)> 
        GetCustomerWithAddressesAsync(string customerId)
    {
        const string sql = @"
            SELECT * FROM customer WHERE customer_id = @customerId;
            SELECT * FROM address WHERE customer_id = @customerId;";

        using var multi = await _db.QueryMultipleAsync(sql, new { customerId });
        var customer = await multi.ReadSingleOrDefaultAsync<CustomerModel>();
        var addresses = await multi.ReadAsync<AddressModel>();
        return (customer, addresses);
    }
}
```

### Transaction Management กับ Dapper

```csharp
public async Task<bool> TransferAsync(TransferModel model)
{
    using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();
    using var transaction = connection.BeginTransaction();

    try
    {
        await connection.ExecuteAsync(
            "UPDATE account SET balance = balance - @Amount WHERE account_id = @FromId",
            new { model.Amount, model.FromId }, transaction);

        await connection.ExecuteAsync(
            "UPDATE account SET balance = balance + @Amount WHERE account_id = @ToId",
            new { model.Amount, model.ToId }, transaction);

        transaction.Commit();
        return true;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

### ข้อดี
- ✅ **Performance ใกล้เคียง ADO.NET** — Overhead ต่ำมาก
- ✅ **SQL เขียนเอง** — Full control, Optimize ได้
- ✅ **Simple API** — เรียนรู้ง่าย
- ✅ **Stored Procedure รองรับ 100%**
- ✅ **Multi-mapping** — Join หลาย Table → หลาย Object
- ✅ **Lightweight** — NuGet เดียว, ไม่ซับซ้อน
- ✅ **ทำงานกับ `IDbConnection`** — เปลี่ยน DB ง่าย
- ✅ **Async Support** ครบถ้วน
- ✅ **ไม่มี Change Tracking** — Memory ต่ำกว่า EF Core

### ข้อเสีย
- ❌ **SQL เขียนเอง** — โอกาส Typo / SQL Injection ถ้าไม่ระวัง
- ❌ **ไม่มี Migration** — ต้องจัดการ Schema เอง
- ❌ **ไม่มี Change Tracking** — ต้อง Manage State เอง
- ❌ **ไม่มี Relationship Management** — Join เขียนเองทุกครั้ง
- ❌ **Column Name ต้อง Match** Property name (แก้ด้วย AS alias หรือ ColumnMap)
- ❌ **ไม่มี LINQ** — ไม่ Type-safe Query

### เหมาะกับ
- ระบบ Read-heavy ที่ต้องการ Performance สูง
- ระบบที่ DBA เขียน Stored Procedure ให้
- ระบบที่ SQL Complex และต้องการ Optimize เอง
- ทีมที่ถนัด SQL

---

## 5. Dapper + Extensions

### Dapper.Contrib — CRUD อัตโนมัติ

```csharp
// NuGet: Dapper.Contrib
// ไม่ต้องเขียน SQL สำหรับ Basic CRUD

[Table("customer")]
public class CustomerEntity
{
    [Key]
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
}

// CRUD อัตโนมัติ
await _db.InsertAsync(new CustomerEntity { ... });
var customer = await _db.GetAsync<CustomerEntity>("C001");
await _db.UpdateAsync(customer);
await _db.DeleteAsync(customer);
await _db.GetAllAsync<CustomerEntity>();
```

### Dapper.FluentMap — Column Mapping

```csharp
// Map ชื่อ Column → Property โดยไม่ต้องใช้ Alias
public class CustomerMap : EntityMap<CustomerEntity>
{
    public CustomerMap()
    {
        Map(x => x.CustomerId).ToColumn("customer_id");
        Map(x => x.CustomerName).ToColumn("customer_name");
    }
}

// Register ใน DI
FluentMapper.Initialize(config =>
{
    config.AddMap(new CustomerMap());
});
```

### Dapper + Repository Pattern

```csharp
public interface ICustomerRepository
{
    Task<CustomerModel?> GetByIdAsync(string customerId);
    Task<IEnumerable<CustomerModel>> GetAllAsync();
    Task<int> CreateAsync(CustomerModel model);
    Task<int> UpdateAsync(CustomerModel model);
    Task<int> DeleteAsync(string customerId);
}

// Generic Base Repository
public abstract class BaseRepository<T> where T : class
{
    protected readonly IDbConnection _db;
    protected BaseRepository(IDbConnection db) => _db = db;

    public async Task<T?> GetByIdAsync(object id)
        => await _db.GetAsync<T>(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _db.GetAllAsync<T>();

    public async Task<long> InsertAsync(T entity)
        => await _db.InsertAsync(entity);

    public async Task<bool> UpdateAsync(T entity)
        => await _db.UpdateAsync(entity);

    public async Task<bool> DeleteAsync(T entity)
        => await _db.DeleteAsync(entity);
}
```

---

## 6. RepoDB

### คืออะไร
Micro-ORM ที่อยู่กลางระหว่าง Dapper และ EF Core  
มี **Fluent API**, **CRUD อัตโนมัติ** และ **Batch Operations**

```csharp
// NuGet: RepoDb ; RepoDb.SqlServer
SqlServerBootstrap.Initialize();

// CRUD
using var connection = new SqlConnection(connectionString);
var customer = await connection.QueryAsync<CustomerEntity>(e => 
    e.CustomerId == "C001");

await connection.InsertAsync(new CustomerEntity { ... });
await connection.UpdateAsync<CustomerEntity>(
    new { CustomerName = "New Name" },
    e => e.CustomerId == "C001");
```

### ข้อดี
- ✅ CRUD อัตโนมัติ + LINQ-style filter
- ✅ Batch Insert / Update / Delete
- ✅ Second-level Cache Built-in
- ✅ Trace / Profiling Built-in

### ข้อเสีย
- ❌ Community เล็กกว่า Dapper และ EF Core
- ❌ Documentation น้อยกว่า
- ❌ Risk ถ้า Library หยุด Maintain

---

## 7. NHibernate

### คืออะไร
Full ORM ที่ Port มาจาก Java Hibernate — มีมาก่อน EF Core  
ปัจจุบันยังใช้งานได้แต่ **Community ลดลงมาก** หลัง EF Core โตขึ้น

### ข้อดี
- ✅ Mature และ Battle-tested
- ✅ รองรับ Multiple DB (Oracle, MySQL, MSSQL, etc.)
- ✅ Second-level Cache
- ✅ เหมาะ Domain-Driven Design

### ข้อเสีย
- ❌ **XML Configuration ซับซ้อน** (แม้จะมี Fluent NHibernate)
- ❌ Community เล็กมากใน .NET ecosystem ปัจจุบัน
- ❌ Microsoft ไม่ Support
- ❌ ไม่แนะนำสำหรับ Project ใหม่

---

## 8. SqlKata

### คืออะไร
**Query Builder** — ไม่ใช่ ORM สมบูรณ์  
สร้าง SQL จาก Fluent API แล้วรันผ่าน Dapper หรือ ADO.NET

```csharp
// NuGet: SqlKata ; SqlKata.Execution
var db = new QueryFactory(connection, new SqlServerCompiler());

var customers = await db.Query("customer")
    .Where("status", "ACTIVE")
    .WhereDate("created_date", ">", DateTime.Now.AddDays(-30))
    .OrderBy("customer_name")
    .GetAsync<CustomerModel>();
```

### ข้อดี
- ✅ SQL Builder — ไม่ต้อง Concatenate string
- ✅ Database Agnostic (เปลี่ยน DB = เปลี่ยน Compiler)
- ✅ ใช้ร่วมกับ Dapper ได้

### ข้อเสีย
- ❌ ยังต้องเขียน SQL สำหรับ Complex query
- ❌ Community เล็ก
- ❌ ไม่มี Migration

---

## 9. Stored Procedure Only Pattern

### คืออะไร
ทุก Data Access ทำผ่าน Stored Procedure เท่านั้น  
สามารถใช้ได้กับ **ทุก Library** ข้างต้น

```csharp
// Dapper + Stored Procedure
public async Task<IEnumerable<TransactionModel>> GetByCustomerAsync(string customerId)
{
    return await _db.QueryAsync<TransactionModel>(
        "sp_GetCustomerTransactions",
        new { CustomerId = customerId },
        commandType: CommandType.StoredProcedure
    );
}

// EF Core + Stored Procedure
var result = await _context.Database
    .SqlQueryRaw<TransactionModel>("EXEC sp_GetCustomerTransactions @customerId",
        new SqlParameter("@customerId", customerId))
    .ToListAsync();
```

---

## 10. ตารางเปรียบเทียบรวม

### Performance Benchmark (Relative)

| Library | Single Row | 100 Rows | 1000 Rows | Bulk Insert |
|---------|:----------:|:--------:|:---------:|:-----------:|
| ADO.NET | ⚡⚡⚡⚡⚡ | ⚡⚡⚡⚡⚡ | ⚡⚡⚡⚡⚡ | ⚡⚡⚡⚡⚡ |
| Dapper | ⚡⚡⚡⚡⚡ | ⚡⚡⚡⚡⚡ | ⚡⚡⚡⚡ | ⚡⚡⚡⚡ |
| RepoDB | ⚡⚡⚡⚡ | ⚡⚡⚡⚡ | ⚡⚡⚡⚡ | ⚡⚡⚡⚡⚡ |
| EF Core 10 (No Tracking) | ⚡⚡⚡⚡ | ⚡⚡⚡ | ⚡⚡⚡ | ⚡⚡ |
| EF Core 10 (Tracking) | ⚡⚡⚡ | ⚡⚡ | ⚡⚡ | ⚡ |
| NHibernate | ⚡⚡ | ⚡⚡ | ⚡⚡ | ⚡⚡ |

> 📌 ค่าข้างต้นเป็นการประมาณการเชิงเปรียบเทียบ  
> Dapper มักเร็วกว่า EF Core (Tracking) ประมาณ **2-5x** บน Complex Query  
> Dapper ช้ากว่า ADO.NET ดิบๆ ไม่เกิน **5%**

### Feature Comparison

| Feature | ADO.NET | Dapper | EF Core 10 | RepoDB | NHibernate |
|---------|:-------:|:------:|:----------:|:------:|:----------:|
| Auto Object Mapping | ❌ | ✅ | ✅ | ✅ | ✅ |
| LINQ Query | ❌ | ❌ | ✅ | ⚡ (บางส่วน) | ✅ |
| Migration | ❌ | ❌ | ✅ | ❌ | ✅ |
| Change Tracking | ❌ | ❌ | ✅ | ❌ | ✅ |
| Stored Procedure | ✅ | ✅ | ✅ | ✅ | ✅ |
| Bulk Operations | ❌ | ⚡ (Contrib) | ✅ (EF Core 8+) | ✅ | ✅ |
| Transaction | ✅ | ✅ | ✅ | ✅ | ✅ |
| Multi-DB Support | ✅ | ✅ | ✅ | ✅ | ✅ |
| Async Support | ✅ | ✅ | ✅ | ✅ | ✅ |
| AOT .NET 10 | ✅ | ✅ | ✅ | ⚡ | ❌ |
| NuGet Size | Tiny | Tiny | Large | Medium | Large |
| Community | ✅✅✅ | ✅✅✅ | ✅✅✅ | ✅✅ | ✅ |
| MS Support | ✅ | ❌ | ✅ | ❌ | ❌ |

### Complexity vs Performance

```
Performance (เร็ว)
        ↑
        │  ADO.NET ●
        │
        │  Dapper ●      RepoDB ●
        │
        │                           EF Core (NoTracking) ●
        │
        │                                        EF Core (Tracking) ●
        │
        │                                                    NHibernate ●
        └──────────────────────────────────────────────────────────────→
            ง่ายใช้                                             ยากใช้
```

---

## 11. สรุปข้อเสนอแนะ

### สำหรับโปรเจค OmniChannel / RL Series

| สถานการณ์ | คำแนะนำ |
|-----------|---------|
| ต้องการ **Query ซับซ้อน + Join หลาย Table** | Dapper + SP |
| ต้องการ **CRUD เร็ว + Transaction** | Dapper + Dapper.Contrib |
| ต้องการ **Migration Management** | EF Core (คงไว้) |
| ระบบ **Read-heavy** (e.g. Report, Transaction Log) | Dapper |
| ระบบ **Write-heavy** ที่ Logic ซับซ้อน | EF Core + ExecuteUpdate |
| ต้องการ **Stored Procedure 100%** | Dapper หรือ ADO.NET |

### คำแนะนำสุดท้าย

> **แนะนำ: Hybrid Approach — Dapper + EF Core ร่วมกัน**
>
> - ใช้ **EF Core** สำหรับ: Migration, Simple CRUD, Entity Relationship  
> - ใช้ **Dapper** สำหรับ: Complex Query, Report, Stored Procedure, Performance-critical Path  
>
> ดูรายละเอียดเพิ่มเติมใน [Dapper_Migration_Analysis.md](./Dapper_Migration_Analysis.md)

---

*เอกสารนี้จัดทำเพื่อประกอบการตัดสินใจ Data Access Strategy สำหรับโปรเจค OmniChannel*  
*วันที่: 4 มีนาคม 2026*
