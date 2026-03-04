# Coding Standard — หัวข้อสำคัญ & เกณฑ์สำหรับทุกโปรเจค

> เอกสารฉบับนี้กำหนด **หัวข้อ** และ **เกณฑ์** ที่ทุกโปรเจค API ต้องปฏิบัติตาม  
> อ้างอิงจาก Architecture Pattern ของ ESTATEMENT_API พร้อมปรับปรุงข้อจำกัดที่พบ  
> **ใช้บังคับ:** ทุก ASP.NET Core Web API Project ใน TFS — ITGroup_Collection

---

## สารบัญ

| # | หัวข้อ | ระดับ |
|---|--------|-------|
| 1 | [Project Structure — โครงสร้างโปรเจค](#1-project-structure--โครงสร้างโปรเจค) | 🔴 บังคับ |
| 2 | [Naming Convention — หลักการตั้งชื่อ](#2-naming-convention--หลักการตั้งชื่อ) | 🔴 บังคับ |
| 3 | [DataAccess Layer — ชั้นเข้าถึงข้อมูล](#3-dataaccess-layer--ชั้นเข้าถึงข้อมูล) | 🔴 บังคับ |
| 4 | [Service Layer — ชั้น Business Logic](#4-service-layer--ชั้น-business-logic) | 🔴 บังคับ |
| 5 | [Controller Layer — ชั้น API Endpoint](#5-controller-layer--ชั้น-api-endpoint) | 🔴 บังคับ |
| 6 | [Dependency Injection — การลงทะเบียน DI](#6-dependency-injection--การลงทะเบียน-di) | 🔴 บังคับ |
| 7 | [Authentication & Authorization](#7-authentication--authorization) | 🔴 บังคับ |
| 8 | [Error Handling & Logging](#8-error-handling--logging) | 🔴 บังคับ |
| 9 | [Configuration Management](#9-configuration-management) | 🔴 บังคับ |
| 10 | [API Documentation (Swagger)](#10-api-documentation-swagger) | 🟡 แนะนำ |
| 11 | [Deployment & Containerization](#11-deployment--containerization) | 🟡 แนะนำ |
| 12 | [Code Quality & Review Checklist](#12-code-quality--review-checklist) | 🟡 แนะนำ |

> **ระดับ:** 🔴 บังคับ = ต้องปฏิบัติตามทุกโปรเจค | 🟡 แนะนำ = ควรปฏิบัติตาม

---

## 1. Project Structure — โครงสร้างโปรเจค

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 1.1 | ต้องแยกโฟลเดอร์เป็น **3 Layer** อย่างน้อย: `Controllers/`, `Services/`, `DataAccess/` | ☐ |
| 1.2 | โฟลเดอร์ `Models/` ต้องแยกออกจาก Layer อื่น (ไม่วาง Model ไว้ใน Controller หรือ Service) | ☐ |
| 1.3 | `Services/` ต้องมีโฟลเดอร์ `Interfaces/` สำหรับ Service Interface | ☐ |
| 1.4 | `DataAccess/` ต้องจัดกลุ่มตาม Database Context (1 Context = 1 โฟลเดอร์ย่อย) | ☐ |
| 1.5 | แต่ละกลุ่ม DataAccess ต้องมีโฟลเดอร์ `Interface/` สำหรับ IDataSets, IUoW | ☐ |
| 1.6 | `Filters/` ต้องแยกออกมาสำหรับ Action Filter (ห้ามวางใน Controller) | ☐ |

### โครงสร้างอ้างอิง

```
{ProjectName}/
├── Controllers/
│   ├── {Feature}Controller.cs
│   └── HealthCheckController.cs
├── Services/
│   ├── Interfaces/
│   │   ├── I{Feature}Service.cs
│   │   └── ...
│   ├── {Feature}Service.cs
│   └── ...
├── DataAccess/
│   ├── {DatabaseName}Db/
│   │   ├── Interface/
│   │   │   ├── I{DatabaseName}DataSets.cs
│   │   │   └── I{DatabaseName}UoW.cs
│   │   ├── {DatabaseName}DbContext.cs
│   │   ├── {DatabaseName}UoW.cs
│   │   └── {EntityName}Entity.cs
│   └── {AnotherDb}Db/
│       └── ... (โครงสร้างเดียวกัน)
├── Models/
│   ├── {Feature}Model.cs
│   └── ResultModel.cs
├── Filters/
│   ├── LogFilter.cs
│   └── AvailableFilter.cs
├── Startup.cs
├── Program.cs
├── appsettings.json
├── appsettings.{Environment}.json
├── DockerFile
└── deployment.yaml
```

---

## 2. Naming Convention — หลักการตั้งชื่อ

### 🔴 เกณฑ์บังคับ

| # | หมวด | รูปแบบ | ตัวอย่างถูก | ตัวอย่างผิด |
|---|------|--------|------------|------------|
| 2.1 | **Namespace** | `{ProjectName}.{Layer}.{SubGroup}` | `eStatementAPI.DataAccess.MainDb` | `eStatementAPI.DB` |
| 2.2 | **Class** | PascalCase | `RegisterService` | `registerService`, `register_service` |
| 2.3 | **Interface** | `I` + PascalCase | `IRegisterService` | `RegisterServiceInterface` |
| 2.4 | **Public Property** | PascalCase | `public string ContractNo { get; set; }` | `public string contractNo` |
| 2.5 | **Private Field** | `_` + camelCase | `private readonly IConfiguration _configuration;` | `private IConfiguration configuration;` |
| 2.6 | **Local Variable** | camelCase | `var resultModel = new ResultModel();` | `var ResultModel = ...` |
| 2.7 | **Method** | PascalCase + Verb | `GetCustomer()`, `ValidateInput()` | `customer()`, `validate()` |
| 2.8 | **Async Method** | PascalCase + Verb + `Async` | `GetCustomerAsync()` | `GetCustomer()` (ถ้า return Task) |
| 2.9 | **Controller** | `{Feature}Controller` | `RegisterController` | `RegCtrl`, `Register` |
| 2.10 | **Service** | `{Feature}Service` | `RegisterService` | `RegSvc`, `RegisterHelper` |
| 2.11 | **DbContext** | `{DatabaseName}DbContext` | `MainDbContext` | `MyContext`, `DB1` |
| 2.12 | **Entity** | `{EntityName}Entity` | `ActivateEntity` | `Activate`, `tbl_activate` |
| 2.13 | **Unit of Work** | `{DatabaseName}UoW` / `I{DatabaseName}UoW` | `MainUoW`, `IMainUoW` | `UnitOfWork1` |
| 2.14 | **Model (DTO)** | `{Purpose}Model` | `RequestRegisterModel`, `ResultModel` | `RegisterDTO`, `RegReq` |
| 2.15 | **Constant** | PascalCase หรือ UPPER_SNAKE_CASE | `public const string Key = "Main";` | `public const string key` |

### กฎเพิ่มเติม

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 2.16 | ห้ามใช้ **ตัวย่อ** ที่ไม่เป็นที่รู้จัก (เช่น `Reg`, `Svc`, `Mgr`) ยกเว้นคำมาตรฐาน (`Id`, `Url`, `Api`, `Db`) | ☐ |
| 2.17 | ชื่อไฟล์ต้องตรงกับชื่อ Class/Interface ภายในไฟล์ | ☐ |
| 2.18 | 1 ไฟล์ = 1 Class/Interface (ห้ามรวมหลาย Class ในไฟล์เดียว) | ☐ |

---

## 3. DataAccess Layer — ชั้นเข้าถึงข้อมูล

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.1 | ต้องใช้ **Entity Framework Core** เป็น ORM | ☐ |
| 3.2 | ทุก DbContext ต้อง implement `IDataSets` interface | ☐ |
| 3.3 | ต้องใช้ **Unit of Work Pattern** — ห้าม inject DbContext เข้า Service โดยตรง | ☐ |
| 3.4 | UoW ต้อง implement `IUoW` interface ที่สืบทอดจาก `IUnitOfWork<TContext>` และ `IDataSets` | ☐ |
| 3.5 | ถ้ามีมากกว่า 1 Database → ต้องใช้ **UnitOfWork Pool** (`IUnitOfWorkPool`) | ☐ |
| 3.6 | Entity Configuration ต้องใช้ **Fluent API** ใน `OnModelCreating()` | ☐ |
| 3.7 | ต้องกำหนด **Table Name** ด้วย `.ToTable("table_name")` ทุก Entity | ☐ |
| 3.8 | ต้องกำหนด **Primary Key** ด้วย `.HasKey()` ทุก Entity | ☐ |
| 3.9 | Entity Class ต้องมี **เฉพาะ Property** เท่านั้น — ห้ามมี Business Logic หรือ Mapping Attribute | ☐ |
| 3.10 | ใช้ NuGet Package: `Easybuy.WebApplication.CoreUnitOfWork` | ☐ |

### ตัวอย่างโครงสร้าง DataAccess ที่ถูกต้อง

```
DataAccess/
└── MainDb/
    ├── Interface/
    │   ├── IMainDataSets.cs       ← ประกาศ DbSet Properties
    │   └── IMainUoW.cs            ← สืบทอด IUnitOfWork<T> + IMainDataSets
    ├── MainDbContext.cs            ← implement IMainDataSets, OnModelCreating
    ├── MainUoW.cs                  ← implement IMainUoW, ใช้ IUnitOfWorkPool
    ├── CustomerEntity.cs
    └── TransactionEntity.cs
```

### ตัวอย่าง Code

**IDataSets:**
```csharp
public interface IMainDataSets
{
    DbSet<CustomerEntity> Customers { get; }          // ← PascalCase
    DbSet<TransactionEntity> Transactions { get; }
}
```

**IUoW:**
```csharp
public interface IMainUoW : IUnitOfWork<MainDbContext>, IMainDataSets
{
}
```

**DbContext:**
```csharp
public class MainDbContext : DbContext, IMainDataSets
{
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CustomerEntity>(entity =>
        {
            entity.ToTable("customer");
            entity.HasKey(x => x.CustomerId);
        });

        builder.Entity<TransactionEntity>(entity =>
        {
            entity.ToTable("transaction");
            entity.HasKey(x => x.TransactionId);
        });
    }
}
```

**UoW:**
```csharp
public class MainUoW : IMainUoW
{
    public const string KEY = "Main";
    private readonly IUnitOfWork _uow;

    public DbSet<CustomerEntity> Customers => _uow.Set<CustomerEntity>();
    public DbSet<TransactionEntity> Transactions => _uow.Set<TransactionEntity>();

    public MainUoW(IUnitOfWorkPool uowPool)
    {
        _uow = uowPool.Get(KEY);
    }

    public void CommitTransaction() => _uow.CommitTransaction();
    public void ForceBeginTransaction() => _uow.ForceBeginTransaction();
    public void RollbackTransaction() => _uow.RollbackTransaction();
    public int SaveChanges() => _uow.SaveChanges();
    public DbSet<T> Set<T>() where T : class => _uow.Set<T>();
    public void SetIsolationLevel(IsolationLevel isolationLevel) => _uow.SetIsolationLevel(isolationLevel);
}
```

> ⚠️ **ปรับปรุงจาก ESTATEMENT_API:** ตัด `try { } catch (Exception) { throw; }` ที่ไม่จำเป็นออก ใช้ Expression Body แทน

---

## 4. Service Layer — ชั้น Business Logic

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 4.1 | ทุก Service **ต้องมี Interface** อยู่ใน `Services/Interfaces/` | ☐ |
| 4.2 | Service ต้องรับ Dependency ผ่าน **Constructor Injection** เท่านั้น | ☐ |
| 4.3 | Service ต้องใช้ **IUoW** ในการเข้าถึง Database — ห้ามใช้ DbContext ตรง | ☐ |
| 4.4 | Method ที่มี I/O (Database, HTTP, File) ต้องเป็น **Async** และ return `Task<T>` | ☐ |
| 4.5 | ห้ามมี **Hard-coded Test Data** ใน Production Code | ☐ |
| 4.6 | Input Validation ต้องอยู่ **ต้น Method** ก่อน Business Logic | ☐ |
| 4.7 | ต้องมี **try-catch** พร้อม Logging ที่มีความหมาย (ไม่ใช่ `catch { throw; }` เปล่าๆ) | ☐ |
| 4.8 | Return Type ต้องเป็น **Model/DTO** — ห้าม return Entity ตรง | ☐ |

### ตัวอย่าง Code

**Interface:**
```csharp
// Services/Interfaces/ICustomerService.cs
public interface ICustomerService
{
    Task<ResultModel<CustomerInfoModel>> GetCustomerAsync(string customerId);
    Task<ResultModel> CreateCustomerAsync(RequestCreateCustomerModel request);
}
```

**Implementation:**
```csharp
// Services/CustomerService.cs
public class CustomerService : ICustomerService
{
    private readonly IMainUoW _mainUoW;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        IMainUoW mainUoW,
        IConfiguration configuration,
        ILogger<CustomerService> logger)
    {
        _mainUoW = mainUoW;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ResultModel<CustomerInfoModel>> GetCustomerAsync(string customerId)
    {
        var result = new ResultModel<CustomerInfoModel>();
        try
        {
            // ── Validation ──
            if (string.IsNullOrEmpty(customerId))
            {
                result.Status = 400;
                result.Success = false;
                result.Message = "CustomerId is required.";
                return result;
            }

            // ── Business Logic ──
            var entity = _mainUoW.Customers
                .FirstOrDefault(x => x.CustomerId == customerId);

            if (entity == null)
            {
                result.Status = 404;
                result.Success = false;
                result.Message = "Customer not found.";
                return result;
            }

            // ── Map Entity → Model ──
            result.Status = 200;
            result.Success = true;
            result.Data = new CustomerInfoModel
            {
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                Email = entity.Email
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCustomerAsync for {CustomerId}", customerId);
            result.Status = 500;
            result.Success = false;
            result.Message = "Internal server error.";
        }
        return result;
    }
}
```

---

## 5. Controller Layer — ชั้น API Endpoint

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 5.1 | Controller ต้องสืบทอดจาก `ControllerBase` | ☐ |
| 5.2 | ต้องมี Attribute: `[ApiController]`, `[Route("api/[controller]/[action]")]`, `[Produces("application/json")]` | ☐ |
| 5.3 | Controller ต้องรับ **IService** ผ่าน Constructor — ห้ามมี Business Logic ใน Controller | ☐ |
| 5.4 | ทุก Action ที่ต้อง Authentication ต้องมี `[Authorize]` | ☐ |
| 5.5 | Action Method ต้องระบุ HTTP Verb: `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` | ☐ |
| 5.6 | ต้องมี **HealthCheckController** อย่างน้อย 1 Endpoint ที่ไม่ต้อง Authorize | ☐ |
| 5.7 | Controller ทำหน้าที่แค่ **รับ Request → เรียก Service → ส่ง Response** เท่านั้น | ☐ |

### ตัวอย่าง Code

```csharp
[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [Authorize]
    [HttpGet("{customerId}")]
    public async Task<ResultModel<CustomerInfoModel>> GetCustomer(string customerId)
    {
        return await _customerService.GetCustomerAsync(customerId);
    }

    [Authorize]
    [HttpPost]
    public async Task<ResultModel> CreateCustomer(RequestCreateCustomerModel request)
    {
        return await _customerService.CreateCustomerAsync(request);
    }
}
```

---

## 6. Dependency Injection — การลงทะเบียน DI

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 6.1 | ลงทะเบียน DI ใน `Startup.cs` → `ConfigureServices()` | ☐ |
| 6.2 | ต้องจัดกลุ่มลำดับการลงทะเบียนตามนี้: | |
| | ① `AddDbContext` — Database Context | ☐ |
| | ② `AddUnitOfWork` — Unit of Work | ☐ |
| | ③ `AddUnitOfWorkPool` — UoW Pool (ถ้ามีหลาย DB) | ☐ |
| | ④ `AddScoped<IUoW, UoW>` — UoW Mapping | ☐ |
| | ⑤ `AddScoped<IService, Service>` — Service Mapping | ☐ |
| 6.3 | Service / UoW ต้องใช้ `AddScoped` (1 Instance ต่อ 1 Request) | ☐ |
| 6.4 | ห้ามใช้ `AddSingleton` กับ Service ที่เข้าถึง Database | ☐ |
| 6.5 | ทุก Interface → Implementation ต้องลงทะเบียนครบ (ไม่มี `new` ในโค้ด) | ☐ |

### ตัวอย่าง Code

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ─── ① Database Context ───
    services.AddDbContext<MainDbContext>(options =>
        options.UseMySQL(Configuration.GetConnectionString("MainDbContext")));
    services.AddDbContext<LogDbContext>(options =>
        options.UseMySQL(Configuration.GetConnectionString("LogDbContext")));

    // ─── ② Unit of Work ───
    services.AddUnitOfWork<MainDbContext>();
    services.AddUnitOfWork<LogDbContext>();

    // ─── ③ UoW Pool ───
    services.AddUnitOfWorkPool(builder =>
    {
        builder.AddUnitOfWork<MainDbContext>(MainUoW.KEY);
        builder.AddUnitOfWork<LogDbContext>(LogUoW.KEY);
    });

    // ─── ④ UoW Mapping ───
    services.AddScoped<IMainUoW, MainUoW>();
    services.AddScoped<ILogUoW, LogUoW>();

    // ─── ⑤ Service Mapping ───
    services.AddScoped<ICustomerService, CustomerService>();
    services.AddScoped<ITransactionService, TransactionService>();
    services.AddScoped<IMailService, MailService>();

    // ─── ⑥ Infrastructure ───
    services.AddControllers(options =>
    {
        options.Filters.Add(typeof(LogFilter));
        options.Filters.Add(typeof(AvailableFilter));
    });
}
```

---

## 7. Authentication & Authorization

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 7.1 | ต้องใช้ **JWT Bearer Token** เป็น Authentication Scheme | ☐ |
| 7.2 | JWT Configuration ต้องอยู่ใน `appsettings.json` (Issuer, Key) — ห้าม Hard-code | ☐ |
| 7.3 | ต้องตั้งค่า `ValidateIssuer`, `ValidateAudience`, `ValidateLifetime`, `ValidateIssuerSigningKey` = **true** ทั้งหมด | ☐ |
| 7.4 | ต้องตั้ง `RequireExpirationTime = true` และ `ClockSkew = TimeSpan.Zero` | ☐ |
| 7.5 | Middleware ต้องเรียงลำดับ: `UseAuthentication()` → `UseAuthorization()` | ☐ |
| 7.6 | ทุก Endpoint ที่ต้องการ Auth ต้องมี `[Authorize]` — HealthCheck ไม่ต้อง | ☐ |

---

## 8. Error Handling & Logging

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 8.1 | ทุก Service Method ต้องมี **try-catch** ครอบ Business Logic | ☐ |
| 8.2 | catch block ต้อง **Log Error** พร้อม Context (Method Name, Parameters) | ☐ |
| 8.3 | ห้ามมี `catch (Exception) { throw; }` เปล่าๆ — ต้อง Log ก่อน throw หรือ return Error | ☐ |
| 8.4 | ใช้ `ILogger<T>` ที่ Inject ผ่าน Constructor — ห้ามใช้ `Console.WriteLine` | ☐ |
| 8.5 | Error Response ต้องมีโครงสร้างเดียวกัน: `Status`, `Success`, `Message` | ☐ |
| 8.6 | ห้าม return Exception Message ตรงให้ Client ใน Production (เช่น Stack Trace) | ☐ |
| 8.7 | ใช้ **LogFilter** (Action Filter) สำหรับ Log ทุก Request/Response อัตโนมัติ | ☐ |

### ตัวอย่าง Error Response Structure

```csharp
public class ResultModel
{
    public int Status { get; set; }         // HTTP Status Code
    public bool Success { get; set; }       // true/false
    public string Message { get; set; }     // ข้อความสำหรับ Client
}

public class ResultModel<T> : ResultModel
{
    public T Data { get; set; }             // Payload (nullable)
}
```

---

## 9. Configuration Management

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 9.1 | ต้องมี `appsettings.json` เป็น Base Configuration | ☐ |
| 9.2 | ต้องมี **อย่างน้อย 3 Environment**: Development, UAT, Production | ☐ |
| 9.3 | Connection String ต้องอยู่ใน `appsettings.{Environment}.json` — ห้าม Hard-code ในโค้ด | ☐ |
| 9.4 | Secret/Password ห้ามอยู่ใน Source Code หรือ appsettings ที่ Commit ขึ้น Repository | ☐ |
| 9.5 | อ่าน Configuration ผ่าน `IConfiguration` ที่ Inject เข้ามา | ☐ |
| 9.6 | ข้อความ Message Response ต้องอยู่ใน Configuration — ห้าม Hard-code | ☐ |

### ไฟล์ Configuration ที่ต้องมี

```
appsettings.json                 ← Base (ค่าที่ใช้ร่วมกัน)
appsettings.Development.json     ← Dev Server
appsettings.UAT.json             ← UAT Server
appsettings.Production.json      ← Production Server
appsettings.Localhost.json       ← Local Development (optional)
```

---

## 10. API Documentation (Swagger)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 10.1 | ต้องติดตั้ง **Swagger (Swashbuckle)** และเปิดใช้งาน | ☐ |
| 10.2 | Swagger Doc ต้องมี Title และ Version | ☐ |
| 10.3 | ต้องกำหนด **Security Definition** สำหรับ Bearer Token | ☐ |
| 10.4 | ต้องเปิดใช้ **XML Comments** (`IncludeXmlComments`) | ☐ |
| 10.5 | ทุก Public Action ควรมี `/// <summary>` XML Comment อธิบาย | ☐ |

---

## 11. Deployment & Containerization

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 11.1 | ต้องมี **DockerFile** ที่ Build และ Run ได้ | ☐ |
| 11.2 | ต้องมี **deployment.yaml** (Kubernetes) หรือ Docker Compose | ☐ |
| 11.3 | DockerFile ต้องใช้ **Multi-Stage Build** (build stage + runtime stage) | ☐ |
| 11.4 | ต้องมี **HealthCheck Endpoint** (`/api/HealthCheck`) สำหรับ Container Orchestration | ☐ |
| 11.5 | Image ต้องไม่รวม Source Code — เฉพาะ Published Output | ☐ |

---

## 12. Code Quality & Review Checklist

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 12.1 | ห้ามมี **unused using statements** | ☐ |
| 12.2 | ห้ามมี **commented-out code** ที่ไม่ได้ใช้ (ลบออก ไม่ใช่ comment) | ☐ |
| 12.3 | ห้ามมี `TODO` / `HACK` / `FIXME` comment ค้างใน Production Branch | ☐ |
| 12.4 | ทุก Public Method ใน Service Interface ต้องมี **XML Comment** | ☐ |
| 12.5 | ไม่มีการ `new` Service หรือ UoW ในโค้ด — ต้องใช้ DI ทั้งหมด | ☐ |
| 12.6 | ไม่มี Magic Number / Magic String — ใช้ Constant หรือ Configuration | ☐ |
| 12.7 | ไม่มี Business Logic ใน Controller | ☐ |
| 12.8 | ไม่มี Database Access ใน Controller (ต้องผ่าน Service) | ☐ |
| 12.9 | ไม่ return Entity ออกจาก Service — ต้อง Map เป็น Model/DTO ก่อน | ☐ |
| 12.10 | Branch ที่ Merge เข้า Master/Main ต้องผ่าน Code Review อย่างน้อย 1 คน | ☐ |

---

## สรุปจำนวนเกณฑ์ทั้งหมด

| หัวข้อ | ระดับ | จำนวนเกณฑ์ |
|--------|:-----:|:----------:|
| 1. Project Structure | 🔴 | 6 |
| 2. Naming Convention | 🔴 | 18 |
| 3. DataAccess Layer | 🔴 | 10 |
| 4. Service Layer | 🔴 | 8 |
| 5. Controller Layer | 🔴 | 7 |
| 6. Dependency Injection | 🔴 | 5 |
| 7. Authentication | 🔴 | 6 |
| 8. Error Handling & Logging | 🔴 | 7 |
| 9. Configuration Management | 🔴 | 6 |
| 10. API Documentation | 🟡 | 5 |
| 11. Deployment | 🟡 | 5 |
| 12. Code Quality | 🟡 | 10 |
| **รวม** | | **93 เกณฑ์** |
| บังคับ (🔴) | | **73 เกณฑ์** |
| แนะนำ (🟡) | | **20 เกณฑ์** |

---

> **เอกสารอ้างอิง:** [Why_ESTATEMENT_API_As_Standard.md](./Why_ESTATEMENT_API_As_Standard.md)  
> **สร้างเมื่อ:** มีนาคม 2026  
> **อ้างอิงจาก:** ESTATEMENT_API — ESTATEMENT_CLOUD Project (TFS)
