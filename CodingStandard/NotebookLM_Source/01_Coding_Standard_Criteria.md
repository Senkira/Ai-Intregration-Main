# Coding Standard — หัวข้อสำคัญ & เกณฑ์สำหรับทุกโปรเจค

> เอกสารฉบับนี้กำหนด **หัวข้อ** และ **เกณฑ์** ที่ทุกโปรเจค API ต้องปฏิบัติตาม  
> **ใช้บังคับ:** ทุก ASP.NET Core Web API Project ใน TFS — ITGroup_Collection

---

## สารบัญ

| # | หัวข้อ | ระดับ |
|---|--------|-------|
| 1 | [Project Structure — โครงสร้างโปรเจค](#1-project-structure--โครงสร้างโปรเจค) | 🔴 บังคับ |
| 2 | [Naming Convention — หลักการตั้งชื่อ](#2-naming-convention--หลักการตั้งชื่อ) | 🔴 บังคับ |
| 3 | [DataAccess Layer — Dapper + Repository](#3-dataaccess-layer--ชั้นเข้าถึงข้อมูล) | 🔴 บังคับ |
| 4 | [Service Layer — ชั้น Business Logic](#4-service-layer--ชั้น-business-logic) | 🔴 บังคับ |
| 5 | [Controller Layer — ชั้น API Endpoint](#5-controller-layer--ชั้น-api-endpoint) | 🔴 บังคับ |
| 6 | [Dependency Injection — การลงทะเบียน DI](#6-dependency-injection--การลงทะเบียน-di) | 🔴 บังคับ |
| 7 | [Authentication & Authorization](#7-authentication--authorization) | 🔴 บังคับ |
| 8 | [Error Handling & Logging](#8-error-handling--logging) | 🔴 บังคับ |
| 9 | [Configuration Management](#9-configuration-management) | 🔴 บังคับ |
| 10 | [API Documentation (OpenAPI)](#10-api-documentation-openapi) | 🟡 แนะนำ |
| 11 | [Code Quality & Review Checklist](#11-code-quality--review-checklist) | 🟡 แนะนำ |
| 12 | [Testing Standards — มาตรฐานการทดสอบ](#12-testing-standards--มาตรฐานการทดสอบ) | 🔴 บังคับ |
| 13 | [API Design & Versioning — การออกแบบ API](#13-api-design--versioning--การออกแบบ-api) | 🔴 บังคับ |
| 14 | [Code Formatting & Style Rules — กฎการจัดรูปแบบโค้ด](#14-code-formatting--style-rules--กฎการจัดรูปแบบโค้ด) | 🔴 บังคับ |
| 15 | [Input Validation — การตรวจสอบข้อมูลขาเข้า](#15-input-validation--การตรวจสอบข้อมูลขาเข้า) | 🔴 บังคับ |
| 16 | [Observability & Monitoring — การเฝ้าระวังระบบ](#16-observability--monitoring--การเฝ้าระวังระบบ) | 🔴 บังคับ |
| 17 | [Performance Standards — มาตรฐานประสิทธิภาพ](#17-performance-standards--มาตรฐานประสิทธิภาพ) | 🔴 บังคับ |
| 18 | [HTTP Client Standards — การเรียก API ภายนอก](#18-http-client-standards--การเรียก-api-ภายนอก) | 🔴 บังคับ |
| 19 | [Dependency Management — การจัดการ NuGet Package](#19-dependency-management--การจัดการ-nuget-package) | 🔴 บังคับ |
| 20 | [Code Complexity Metrics — เกณฑ์ความซับซ้อนของโค้ด](#20-code-complexity-metrics--เกณฑ์ความซับซ้อนของโค้ด) | 🔴 บังคับ |
| 21 | [Documentation Standards — มาตรฐานเอกสาร](#21-documentation-standards--มาตรฐานเอกสาร) | 🟡 แนะนำ |
| 22 | [Resilience & Fault Tolerance — ความทนทานต่อความผิดพลาด](#22-resilience--fault-tolerance--ความทนทานต่อความผิดพลาด) | 🟡 แนะนำ |
| 23 | [Concurrency & Thread Safety — ความปลอดภัยของ Thread](#23-concurrency--thread-safety--ความปลอดภัยของ-thread) | 🟡 แนะนำ |
| 24 | [Caching Standards — มาตรฐานการ Cache](#24-caching-standards--มาตรฐานการ-cache) | 🟡 แนะนำ |
| 25 | [Feature Flags — การจัดการ Feature Toggle](#25-feature-flags--การจัดการ-feature-toggle) | 🟡 แนะนำ |
| 26 | [DevOps Files — ขอบเขตความรับผิดชอบ](#26-devops-files--ขอบเขตความรับผิดชอบ) | 🔴 บังคับ (อ่านอย่างเดียว) |

> **ระดับ:** 🔴 บังคับ = ต้องปฏิบัติตามทุกโปรเจค | 🟡 แนะนำ = ควรปฏิบัติตาม

---

## 1. Project Structure — โครงสร้างโปรเจค

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 1.1 | ต้องแยกโฟลเดอร์เป็น **3 Layer** อย่างน้อย: `Controllers/`, `Services/`, `DataAccess/` | ☐ |
| 1.2 | โฟลเดอร์ `Models/` ต้องแยกออกจาก Layer อื่น — ต้องมี `Entities/` สำหรับ DB Mapping และ `Requests/` สำหรับ Input Model | ☐ |
| 1.3 | `Services/` ต้องมีโฟลเดอร์ `Interfaces/` สำหรับ Service Interface | ☐ |
| 1.4 | `DataAccess/` ต้องมีโฟลเดอร์ย่อย: `Connections/`, `Repositories/` | ☐ |
| 1.5 | `Repositories/` ต้องมีโฟลเดอร์ `Interfaces/` สำหรับ Repository Interface | ☐ |
| 1.6 | `Filters/` ต้องแยกออกมาสำหรับ Action Filter (ห้ามวางใน Controller) | ☐ |
| 1.7 | Solution ต้องมีโฟลเดอร์ `tests/` แยกจาก `src/` สอดคล้องกับ [Section 12](#12-testing-standards--มาตรฐานการทดสอบ) | ☐ |

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 1.8 | ไฟล์ `.editorconfig` ที่ Solution Root เพื่อบังคับ Code Style ร่วมกัน — Microsoft แนะนำแต่ไม่บังคับ เหมาะสำหรับทีมที่มีหลายคน | ☐ |
| 1.9 | `DataAccess/SqlQueries/` แยก SQL Constants เป็นไฟล์ตาม Feature — ใช้เมื่อ Query มีจำนวนมาก (หาก Query น้อยให้เก็บเป็น `private const` ใน Repository ได้) | ☐ |
| 1.10 | `Middleware/` แยกออกมาสำหรับ Custom Middleware — ใช้เมื่อมี Custom Middleware (เช่น Error Handling, Request Logging) | ☐ |

### โครงสร้างอ้างอิง

> **อ้างอิง:** Microsoft [Common Web Application Architectures — N-Layer](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)  
> **หมายเหตุ:** โครงสร้างนี้ออกแบบสำหรับ **Single-Project Monolith API + Dapper** — หากโปรเจคมีขนาดใหญ่หรือซับซ้อนมาก ให้พิจารณา [Clean Architecture (Multi-Project)](https://github.com/ardalis/CleanArchitecture)

#### Solution Level

```
{SolutionName}/
├── .editorconfig                        ← 🟡 แนะนำ Code Style Rules
├── {SolutionName}.sln
├── src/
│   └── {ProjectName}/                   ← ดูโครงสร้างด้านล่าง
└── tests/
    ├── UnitTest/                        ← 🔴 บังคับ (ดู Section 12)
    └── IntegrationTest/                 ← 🔴 บังคับ (ดู Section 12)
```

#### Project Level

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
│   ├── Connections/                     ← Dapper Connection Factory
│   │   ├── IDbConnectionFactory.cs
│   │   └── DbConnectionFactory.cs
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   │   ├── IBaseRepository.cs
│   │   │   └── I{Feature}Repository.cs
│   │   ├── BaseRepository.cs
│   │   └── {Feature}Repository.cs
│   └── SqlQueries/                      ← 🟡 แนะนำ (ใช้เมื่อ Query มีจำนวนมาก)
│       └── {Feature}Queries.cs
├── Models/
│   ├── Entities/                        ← DB Mapping Models
│   │   └── {Name}Entity.cs
│   ├── Requests/                        ← Input DTOs
│   │   └── {Action}{Feature}Request.cs
│   ├── {Feature}Model.cs                ← Response / General Models
│   └── ResultModel.cs
├── Filters/
│   ├── LogFilter.cs
│   └── AvailableFilter.cs
├── Middleware/                           ← 🟡 แนะนำ (สร้างเมื่อมี Custom Middleware)
│   └── {Purpose}Middleware.cs
├── Program.cs
├── appsettings.json
├── appsettings.{Environment}.json
├── DockerFile
└── deployment.yaml
```

> **📝 หมายเหตุ `SqlQueries/`:**  
> มาตรฐานสากลเก็บ SQL query เป็น `private const string` ภายใน Repository  
> แต่สำหรับโปรเจคที่มี Query จำนวนมากต่อ Feature อนุญาตให้แยกเป็น `{Feature}Queries.cs` ใน `SqlQueries/` ได้  
> ทั้งสองวิธีเป็นที่ยอมรับ — เลือกแบบที่เหมาะกับขนาดโปรเจค

### ตารางอ้างอิงเกณฑ์และเหตุผล

| # | เกณฑ์ | ระดับ | มาตรฐานอ้างอิง | เหตุผล |
|---|-------|:-----:|----------------|--------|
| 1.1 | แยก 3 Layer | 🔴 | Microsoft N-Layer Architecture | แยก Presentation / Business / Data ไม่ให้ปนกัน — พื้นฐาน Separation of Concerns |
| 1.2 | `Models/` แยก + `Entities/` + `Requests/` | 🔴 | DTO Pattern, Over-posting Prevention | Entity ≠ API Contract — แยกเพื่อเปลี่ยน DB schema โดยไม่กระทบ client |
| 1.3 | `Services/Interfaces/` | 🔴 | SOLID — Dependency Inversion | Interface ทำให้ mock ได้ → ทดสอบ Controller แยกจาก Service จริง |
| 1.4 | `Connections/` + `Repositories/` | 🔴 | Repository Pattern + Connection Factory | Dapper ต้องจัดการ connection เอง — Factory ควบคุม pooling ได้ |
| 1.5 | `Repositories/Interfaces/` | 🔴 | SOLID — Dependency Inversion | Mock repository ได้ → ทดสอบ Service โดยไม่ต้องเชื่อม DB |
| 1.6 | `Filters/` แยก | 🔴 | ASP.NET Core Filters, SRP | Cross-cutting concern (log, auth check) ใช้ร่วมหลาย Controller |
| 1.7 | `tests/` แยกจาก `src/` | 🔴 | Industry standard, CI/CD best practice | สอดคล้องกับ Section 12 ที่บังคับเขียน test + build production ไม่ compile test |
| 1.8 | `.editorconfig` | 🟡 | Microsoft recommended (ไม่บังคับ) | บังคับ code style ผ่านไฟล์ — ดีสำหรับทีมหลายคน แต่ไม่จำเป็นทุกโปรเจค |
| 1.9 | `SqlQueries/` | 🟡 | ทั้ง inline และ separate class ยอมรับ | มาตรฐานสากลเก็บ SQL ใน Repository — แยกได้เมื่อ query มีจำนวนมาก |
| 1.10 | `Middleware/` | 🟡 | ASP.NET Core ไม่กำหนด folder — วางได้ยืดหยุ่น | API ส่วนใหญ่ไม่มี custom middleware — สร้างเมื่อจำเป็นเท่านั้น |

---

## 2. Naming Convention — หลักการตั้งชื่อ

> **อ้างอิงมาตรฐานสากล:**
> - Microsoft .NET [Framework Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines) & [C# Identifier Naming Rules](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)
> - Microsoft .NET Runtime [Coding Style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
> - [Google C# Style Guide](https://google.github.io/styleguide/csharp-style.html)
> - [Roslyn (C# Compiler) Coding Conventions](https://github.com/dotnet/roslyn/blob/main/CONTRIBUTING.md#csharp)
> - .NET 10 / C# 14 Best Practices

---

### 2A. Capitalization Rules — กฎการใช้ตัวพิมพ์ใหญ่-เล็ก

> 📖 **อ้างอิง:** Microsoft [Capitalization Conventions](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions)

| Identifier | Casing | ตัวอย่าง |
|------------|--------|----------|
| **Namespace** | PascalCase | `System.Security`, `CompanyName.DataAccess` |
| **Type** (class, struct, record) | PascalCase | `StreamReader`, `CustomerService` |
| **Interface** | `I` + PascalCase | `IEnumerable`, `ICustomerRepository` |
| **Method** | PascalCase | `ToString()`, `GetCustomerAsync()` |
| **Property** | PascalCase | `Length`, `ContractNo`, `IsActive` |
| **Event** | PascalCase | `Clicked`, `ValueChanged` |
| **Enum Type** | PascalCase (Singular) | `FileMode`, `TransactionStatus` |
| **Enum Value** | PascalCase | `FileMode.Append`, `TransactionStatus.Active` |
| **Delegate** | PascalCase | `EventHandler`, `Converter<TInput, TOutput>` |
| **Public Field** | PascalCase | `public static readonly TimeSpan InfiniteTimeout;` |
| **Constant** (`const`) | PascalCase | `public const string DefaultKey = "Main";` |
| **Private / Internal Field** | `_camelCase` | `private readonly ILogger _logger;` |
| **Static Private Field** | `_camelCase` | `private static IWorkerQueue _workerQueue;` |

> 📝 **หมายเหตุ Static Private Field:** ใช้ `_camelCase` เหมือน instance field — ง่าย สม่ำเสมอ ไม่ต้องจำหลาย prefix  
> หน้า [C# Identifier Naming Rules](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names) แนะนำ `s_` สำหรับ static field แต่ MS เอง note ว่า *"isn't part of the Framework Design Guidelines"*  
> เราเลือก `_camelCase` เพื่อความสม่ำเสมอ — developer ไม่ต้องจำว่า field ไหนใช้ prefix อะไร

| **Parameter** | camelCase | `string customerId`, `int pageSize` |
| **Local Variable** | camelCase | `var resultModel = new ResultModel();` |
| **Local Constant** | PascalCase | `const int MaxRetry = 3;` |
| **Type Parameter** (Generic) | `T` + PascalCase | `TSession`, `TInput`, `TOutput` |
| **Record Positional Parameter** | PascalCase | `public record Person(string FirstName, string LastName);` |
| **Primary Constructor Parameter** (class/struct) | camelCase | `public class DataService(ILogger logger)` |

> ⚠️ **สำคัญ:** ห้ามใช้ `UPPER_SNAKE_CASE` สำหรับ Constant ใน C#  
> Microsoft & Google กำหนดให้ใช้ **PascalCase** เท่านั้นสำหรับ const  
> `UPPER_SNAKE_CASE` เป็นแนวปฏิบัติของ Java/Python ไม่ใช่ .NET

---

### 2B. Acronym & Compound Word Rules — กฎคำย่อและคำประสม

> 📖 **อ้างอิง:** Microsoft [Capitalization Conventions — Compound Words](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions#capitalizing-compound-words-and-common-terms)

#### Acronym ที่มี 2 ตัวอักษร → พิมพ์ใหญ่ทั้ง 2 ตัว
| ✅ ถูก | ❌ ผิด |
|--------|--------|
| `IOStream` | `IoStream` |
| `DBConnection` | `DbConnection` ← *ข้อยกเว้น: .NET Runtime ใช้ PascalCase ปกติ* |

#### Acronym ที่มี 3+ ตัวอักษร → ใช้ PascalCase ปกติ
| ✅ ถูก | ❌ ผิด |
|--------|--------|
| `XmlReader` | `XMLReader` |
| `HtmlTag` | `HTMLTag` |
| `JsonSerializer` | `JSONSerializer` |
| `HttpClient` | `HTTPClient` |
| `ApiController` | `APIController` |

#### Compound Words — คำประสมที่มักเขียนผิด
| ✅ PascalCase | ✅ camelCase | ❌ ผิด |
|---------------|-------------|--------|
| `Callback` | `callback` | `CallBack` |
| `Email` | `email` | `EMail` |
| `Endpoint` | `endpoint` | `EndPoint` |
| `FileName` | `fileName` | `Filename` |
| `Hashtable` | `hashtable` | `HashTable` |
| `Id` | `id` | `ID` (ในกลาง/ท้ายคำ) |
| `Metadata` | `metadata` | `MetaData` |
| `Namespace` | `namespace` | `NameSpace` |
| `UserName` | `userName` | `Username` |
| `WhiteSpace` | `whiteSpace` | `Whitespace` |
| `SignIn` | `signIn` | `SignOn`, `LogIn` |
| `SignOut` | `signOut` | `SignOff`, `LogOut` |
| `Ok` | `ok` | `OK` (ยกเว้น HTTP 200 OK) |

---

### 2C. Identifier Naming — เกณฑ์การตั้งชื่อ

| # | หมวด | รูปแบบ | ตัวอย่างถูก | ตัวอย่างผิด |
|---|------|--------|------------|------------|
| 2.1 | **Namespace** | PascalCase: `{Company}.{Product}.{Layer}` | `CompanyName.DataAccess.Repositories` | `companyAPI.DB`, `COMPANYNAME.data_access` |
| 2.2 | **Class** | PascalCase (Noun / Noun Phrase) | `CustomerService`, `StreamReader` | `registerService`, `register_service`, `Data1` |
| 2.3 | **Interface** | `I` + PascalCase (Noun / Adjective) | `ICustomerService`, `IDisposable` | `CustomerServiceInterface`, `Iservice` |
| 2.4 | **Record** | PascalCase + Positional PascalCase | `record Person(string FirstName, string LastName)` | `record person(string first_name)` |
| 2.5 | **Struct** | PascalCase (Value Semantic Name) | `ValueCoordinate`, `DateRange` | `value_coordinate` |
| 2.6 | **Public Property** | PascalCase (Noun / Adjective) | `public string ContractNo { get; set; }` | `public string contractNo`, `public string contract_no` |
| 2.7 | **Boolean Property** | `Is`/`Has`/`Can` + Adjective/Verb | `IsActive`, `HasPermission`, `CanSeek` | `Active`, `Flag`, `Check` |
| 2.8 | **Collection Property** | Plural Noun (ไม่ต้องมี List/Collection) | `Customers`, `OrderItems` | `CustomerList`, `CustomerCollection` |
| 2.9 | **Private Field** | `_` + camelCase | `private readonly IConfiguration _configuration;` | `private IConfiguration configuration;`, `private IConfiguration m_config;` |
| 2.10 | **Static Private Field** | `_` + camelCase (เหมือน instance field) | `private static IWorkerQueue _workerQueue;` | `private static IWorkerQueue workerQueue;`, `s_workerQueue` |
| 2.11 | **Local Variable** | camelCase | `var resultModel = new ResultModel();` | `var ResultModel = ...`, `var result_model = ...` |
| 2.12 | **Parameter** | camelCase | `string customerId`, `CancellationToken cancellationToken` | `string CustomerId`, `string customer_id` |
| 2.13 | **Method** | PascalCase + **Verb / Verb Phrase** | `GetCustomer()`, `ValidateInput()`, `SendEmail()` | `customer()`, `validate()`, `data()` |
| 2.14 | **Async Method** | PascalCase + Verb + `Async` suffix | `GetCustomerAsync()`, `SaveOrderAsync()` | `GetCustomer()` (ถ้า return `Task`) |
| 2.15 | **Event** | PascalCase + **Verb Tense** (ไม่ใช้ Before/After) | `Closing`, `Closed`, `ValueChanged` | `BeforeClose`, `AfterClose`, `OnClose` |
| 2.16 | **Event Handler** | PascalCase + `EventHandler` suffix | `ClickedEventHandler`, `ValueChangedEventHandler` | `HandleClick`, `OnValueChange` |
| 2.17 | **Enum Type** | PascalCase **Singular** (ยกเว้น Flags = Plural) | `TransactionStatus`, `FileMode` | `TransactionStatuses`, `TRANSACTION_STATUS` |
| 2.18 | **Enum Value** | PascalCase | `TransactionStatus.Pending` | `TransactionStatus.PENDING`, `TransactionStatus.pending` |
| 2.19 | **Constant** (`const` / `static readonly`) | **PascalCase** | `public const string DefaultKey = "Main";` | `public const string DEFAULT_KEY`, `public const string key` |
| 2.20 | **Generic Type Parameter** | `T` + PascalCase (Descriptive) | `TSession`, `TEntity`, `TResult` | `T1`, `type`, `session` |
| 2.21 | **Delegate** | PascalCase + ตามการใช้งาน | `Converter<TInput, TOutput>`, `Predicate<T>` | `converter_func`, `CALLBACK` |
| 2.22 | **Attribute Class** | PascalCase + `Attribute` suffix | `ObsoleteAttribute`, `SerializableAttribute` | `Obsolete_Attr`, `MyAttr` |
| 2.23 | **Exception Class** | PascalCase + `Exception` suffix | `InvalidOperationException`, `CustomerNotFoundException` | `CustomerError`, `BadRequestEx` |

---

### 2D. Project-Specific Naming — การตั้งชื่อเฉพาะโปรเจค (ASP.NET Core API)

| # | หมวด | รูปแบบ | ตัวอย่างถูก | ตัวอย่างผิด |
|---|------|--------|------------|------------|
| 2.24 | **Controller** | `{Feature}Controller` : ControllerBase | `CustomerController`, `RegisterController` | `RegCtrl`, `Register`, `CustomerCtrl` |
| 2.25 | **Service** | `{Feature}Service` / `I{Feature}Service` | `CustomerService`, `ICustomerService` | `RegSvc`, `CustomerHelper`, `CustBL` |
| 2.26 | **Repository** | `{Feature}Repository` / `I{Feature}Repository` | `CustomerRepository`, `ICustomerRepository` | `CustRepo`, `DAL1`, `CustomerDAL` |
| 2.27 | **Entity** (DB Mapping) | `{EntityName}Entity` | `CustomerEntity`, `TransactionEntity` | `Customer`, `tbl_customer`, `TBL_CUST` |
| 2.28 | **Model / DTO** | `{Purpose}Model` หรือ `{Purpose}Dto` | `CreateCustomerRequest`, `CustomerResponse`, `ResultModel` | `RegReq`, `CustResp`, `Data1` |
| 2.29 | **Request Model** | `{Action}{Feature}Request` | `CreateCustomerRequest`, `UpdateOrderRequest` | `RequestRegisterModel`, `ReqReg` |
| 2.30 | **Response Model** | `{Feature}Response` หรือ `{Feature}Result` | `CustomerResponse`, `OrderResult` | `RespCust`, `ReturnData` |
| 2.31 | **Connection Factory** | `IDbConnectionFactory` / `DbConnectionFactory` | `DbConnectionFactory` | `DbHelper`, `ConnectionManager`, `ConnFactory` |
| 2.32 | **SQL Queries** | `{Feature}Queries` (static class) | `CustomerQueries`, `TransactionQueries` | `SQL`, `Queries`, `SqlConst` |
| 2.33 | **Filter** | `{Purpose}Filter` / `{Purpose}Attribute` | `LogActionFilter`, `ValidateModelAttribute` | `MyFilter`, `Filter1` |
| 2.34 | **Middleware** | `{Purpose}Middleware` | `ExceptionHandlingMiddleware`, `CorrelationIdMiddleware` | `MyMiddleware`, `MW1` |
| 2.35 | **Extension Method Class** | `{Type}Extensions` | `StringExtensions`, `ServiceCollectionExtensions` | `StringHelper`, `Utils`, `Ext` |
| 2.36 | **Options / Settings** | `{Feature}Options` / `{Feature}Settings` | `JwtOptions`, `SmtpSettings` | `Config`, `AppConfig`, `Settings1` |

---

### 2E. General Naming Rules — กฎทั่วไป

> 📖 **อ้างอิง:** Microsoft [General Naming Conventions](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/general-naming-conventions) & [Google C# Style Guide](https://google.github.io/styleguide/csharp-style.html)

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 2.37 | ✅ **Readability over Brevity** — ใช้ชื่อที่อ่านง่ายแม้จะยาวกว่า เช่น `CanScrollHorizontally` ดีกว่า `ScrollableX` | ☐ |
| 2.38 | ❌ ห้ามใช้ **Hungarian Notation** — เช่น `strName`, `iCount`, `bIsActive` | ☐ |
| 2.39 | ❌ ห้ามใช้ **Underscore** ในชื่อ (ยกเว้น Private Field prefix `_`) — เช่น `customer_name`, `Get_Data()` | ☐ |
| 2.40 | ❌ ห้ามใช้ **ตัวย่อ** ที่ไม่เป็นสากล (เช่น `Reg`, `Svc`, `Mgr`, `Impl`, `Proc`) ยกเว้นที่ยอมรับทั่วไป: `Id`, `Url`, `Html`, `Xml`, `Json`, `Http`, `Api`, `Db`, `IO`, `UI` | ☐ |
| 2.41 | ❌ ห้ามใช้ชื่อตัวเดียว — ยกเว้น loop counter (`i`, `j`, `k`) หรือ LINQ lambda (`x => x.Id`) | ☐ |
| 2.42 | ❌ ห้ามใช้ชื่อที่ซ้ำกับ **C# Keywords** — เช่น `class`, `event`, `string` เป็นชื่อตัวแปร | ☐ |
| 2.43 | ❌ ห้ามใช้ **2 undersscores ติดกัน** `__` (สงวนสำหรับ Compiler) | ☐ |
| 2.44 | ✅ ใช้ **Semantic Name** ไม่ใช่ Type Name — เช่น `GetLength()` ดีกว่า `GetInt()` | ☐ |
| 2.45 | ✅ Method ต้องขึ้นต้นด้วย **Verb** — `Get`, `Set`, `Create`, `Delete`, `Update`, `Validate`, `Send`, `Calculate`, `Find`, `Build`, `Initialize` | ☐ |
| 2.46 | ✅ Boolean **Property/Parameter** ต้องขึ้นต้นด้วย `is`, `has`, `can`, `should`, `was`, `will` — local variable ใช้ชื่อที่สื่อความหมายได้เลย เช่น `bool found = list.Any()` | ☐ |

---

### 2F. File & Organization Rules — กฎไฟล์และการจัดลำดับ

> 📖 **อ้างอิง:** [Google C# Style Guide — Organization](https://google.github.io/styleguide/csharp-style.html)

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 2.47 | ชื่อไฟล์ต้องเป็น **PascalCase** และตรงกับชื่อ Class/Interface หลักภายในไฟล์ — เช่น `CustomerService.cs` | ☐ |
| 2.48 | **1 ไฟล์ = 1 Type** (Class/Interface/Enum/Record) — ห้ามรวมหลาย Type ในไฟล์เดียว (ยกเว้น nested type, SQL Queries static class) | ☐ |
| 2.49 | ใช้ **File-Scoped Namespace** (C# 10+) — `namespace MyProject.Services;` แทน block `namespace { }` | ☐ |
| 2.50 | `using` directive ต้องอยู่ **นอก namespace** และเรียง **alphabetical** (`System.*` ก่อน) | ☐ |
| 2.51 | **Member Ordering** ภายใน Class: | ☐ |
| | ① Nested types, enums, delegates, events | |
| | ② `static`, `const`, `readonly` fields | |
| | ③ Fields and properties | |
| | ④ Constructors and finalizers | |
| | ⑤ Methods | |
| 2.52 | ภายในแต่ละกลุ่ม เรียง **Access Modifier**: `public` → `internal` → `protected internal` → `protected` → `private` | ☐ |
| 2.53 | **Modifier Order**: `public protected internal private new abstract virtual override sealed static readonly extern unsafe volatile async` | ☐ |

---

### 2G. ❌ Anti-Patterns — สิ่งที่ห้ามทำเด็ดขาด

| # | Anti-Pattern | ❌ ตัวอย่างผิด | ✅ ควรเป็น | อ้างอิง |
|---|-------------|---------------|-----------|---------|
| 2.54 | Hungarian Notation | `strName`, `intCount`, `bIsValid` | `name`, `count`, `isValid` | Microsoft General Naming |
| 2.55 | UPPER_SNAKE_CASE Constants | `MAX_RETRY_COUNT`, `DEFAULT_TIMEOUT` | `MaxRetryCount`, `DefaultTimeout` | Microsoft Capitalization |
| 2.56 | Prefix/Suffix ที่ไม่จำเป็น | `CCustomer`, `CustomerClass`, `enumStatus` | `Customer`, `Status` | Microsoft General Naming |
| 2.57 | Database Naming ใน Code | `tbl_customer`, `sp_GetData`, `fn_Calculate` | `CustomerEntity`, `GetDataAsync()` | Google C# Style |
| 2.58 | Type ในชื่อตัวแปร | `customerString`, `amountDecimal` | `customerName`, `totalAmount` | Microsoft General Naming |
| 2.59 | Numbered Names | `Data1`, `Model2`, `Service3`, `temp1` | ใช้ชื่อที่สื่อความหมาย | Google C# Style |
| 2.60 | ตัวย่อไม่เป็นสากล | `CustRepo`, `TxnSvc`, `AcctMgr`, `RegProc` | `CustomerRepository`, `TransactionService` | Microsoft General Naming |
| 2.61 | Inconsistent Casing | `getCustomer()`, `Get_Customer()` | `GetCustomer()` | Microsoft Capitalization |
| 2.62 | "Get" Prop + "Get" Method clash | `TextWriter` (prop) + `GetTextWriter()` (method) | เลือกอย่างใดอย่างหนึ่ง | Microsoft Type Members |

---

### 2H. Quick Reference Card — สรุปรวดเร็ว

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    .NET 10 NAMING CONVENTION QUICK REFERENCE            │
│         (Microsoft + Google + .NET Runtime + Roslyn Standards)          │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  PascalCase         camelCase           _camelCase                      │
│  ─────────          ─────────           ──────────                      │
│  Namespace           parameter           _privateField                  │
│  ClassName           localVariable       _staticField                   │
│  InterfaceName       lambdaParam         _logger                        │
│  MethodName                              _configuration                 │
│  PropertyName       T + PascalCase                                      │
│  EventName          ──────────────       File Names                    │
│  EnumType           TSession             ──────────                    │
│  EnumValue          TEntity              PascalCase.cs                 │
│  ConstName          TResult              = ClassName.cs                │
│  DelegateName                                                          │
│  PublicField                                                           │
│                                                                         │
│  ❌ NEVER: UPPER_SNAKE_CASE, Hungarian, m_prefix, s_prefix             │
│  ❌ NEVER: underscore_names, Abbreviations (Mgr, Svc, Impl, Proc)     │
│  ✅ ALWAYS: Readable > Short, Verb for Methods, Noun for Properties    │
│                                                                         │
│  2-letter Acronym: UPPERCASE → IO, DB, UI                              │
│  3+ letter Acronym: PascalCase → Xml, Json, Html, Http, Api            │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### 2I. ตัวอย่าง Code — Naming Convention ที่ถูกต้องตามมาตรฐานสากล

```csharp
// ✅ File: CustomerService.cs
// ✅ File-Scoped Namespace (C# 10+)
// ✅ using อยู่นอก namespace, เรียง alphabetical, System.* ก่อน

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyProject.DataAccess.Repositories.Interfaces;
using MyProject.Models;
using MyProject.Services.Interfaces;

namespace MyProject.Services;

/// <summary>
/// Service responsible for customer-related business logic.
/// </summary>
public class CustomerService : ICustomerService
{
    // ── Constants & Static Fields (① static, const, readonly) ──
    private const int MaxRetryCount = 3;
    private const string DefaultStatus = "Active";
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

    // ── Instance Fields (③ fields) ──
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    // ── Constructor ──
    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    // ── Method: Verb + PascalCase + Async suffix ──
    public async Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        // ✅ Local variables: camelCase
        var result = new ResultModel<CustomerResponse>();

        // ✅ Boolean variable: starts with 'is'
        bool isValidInput = !string.IsNullOrWhiteSpace(customerId);

        if (!isValidInput)
        {
            result.Status = 400;
            result.IsSuccess = false;
            result.Message = "CustomerId is required.";
            return result;
        }

        // ✅ Meaningful variable names (no abbreviations)
        var customerEntity = await _customerRepository.GetByIdAsync(
            customerId, cancellationToken);

        // ✅ Boolean: 'was' prefix for past tense
        bool wasCustomerFound = customerEntity is not null;

        if (!wasCustomerFound)
        {
            result.Status = 404;
            result.IsSuccess = false;
            result.Message = "Customer not found.";
            return result;
        }

        result.Status = 200;
        result.IsSuccess = true;
        result.Data = MapToResponse(customerEntity!);
        return result;
    }

    // ── Private Method: still PascalCase + Verb ──
    private static CustomerResponse MapToResponse(CustomerEntity entity) => new()
    {
        CustomerId = entity.CustomerId,
        CustomerName = entity.CustomerName,
        Email = entity.Email
    };
}

// ─── Enum: Singular PascalCase, Values PascalCase ───
public enum TransactionStatus
{
    Pending,
    Approved,
    Rejected,
    Canceled          // ✅ Microsoft Compound Words: "Canceled" (1 L) ไม่ใช่ "Cancelled" (2 L)
}

// ─── Flags Enum: Plural PascalCase ───
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4,
    ReadWrite = Read | Write
}

// ─── Record: PascalCase Positional Parameters ───
public record CustomerSummary(string CustomerId, string FullName, bool IsActive);

// ─── Generic Interface: T + Descriptive PascalCase ───
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
}
```

---

### 2J. Audit Report — ตรวจสอบความถูกต้องตามมาตรฐานสากล

> 📅 **วันที่ตรวจสอบ:** 9 มีนาคม 2026  
> 🔍 **วิธีการ:** เทียบกับ 5 แหล่งอ้างอิงหลัก

#### แหล่งอ้างอิงที่ใช้ตรวจสอบ

| รหัส | แหล่งอ้างอิง | URL |
|:----:|-------------|-----|
| **FDG** | Microsoft Framework Design Guidelines | [learn.microsoft.com/dotnet/standard/design-guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines) |
| **IDN** | Microsoft C# Identifier Naming Rules | [learn.microsoft.com/dotnet/csharp/.../identifier-names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names) |
| **RUN** | dotnet/runtime Coding Style | [github.com/dotnet/runtime/.../coding-style.md](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md) |
| **GOO** | Google C# Style Guide | google.github.io/styleguide/csharp-style.html |
| **COD** | Microsoft Common C# Code Conventions | [learn.microsoft.com/dotnet/csharp/.../coding-conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) |

#### สรุปผลการตรวจสอบ

| หมวด | จำนวนเกณฑ์ | สถานะ | หมายเหตุ |
|:----:|:----------:|:-----:|---------|
| **2A** Capitalization Rules | 19 | ✅ | ครบทุก identifier type ตาม FDG + IDN |
| **2B** Acronym & Compound Words | 25 | ✅ | ตรงกับ FDG compound words table ทุกคำ |
| **2C** Identifier Naming | 23 (2.1–2.23) | ✅ | ทุก identifier มี pattern ชัดเจน + ตัวอย่าง |
| **2D** ASP.NET Core Naming | 13 (2.24–2.36) | ✅ | Best practice ทั่วไป — practical |
| **2E** General Rules | 10 (2.37–2.46) | ✅ | ครอบคลุม FDG + GOO guidelines |
| **2F** File & Organization | 7 (2.47–2.53) | ✅ | File-scoped namespace, member ordering |
| **2G** Anti-Patterns | 9 (2.54–2.62) | ✅ | ครอบคลุมทุก common mistake |
| **2H** Quick Reference Card | 1 | ✅ | สรุปได้ครบ 1 หน้า |
| **2I** Code Example | 1 | ✅ | แสดง convention ครบทุกจุด |

#### ประเด็นสำคัญที่ตัดสินใจ

| # | ประเด็น | มาตรฐานนี้ | เหตุผล | อ้างอิง |
|:-:|---------|-----------|--------|---------|
| 1 | **Static Private Field** | `_camelCase` (เหมือน instance field) | FDG ไม่ได้กำหนด private fields — ใช้ `_` เดียวง่าย ไม่ต้องจำ prefix หลายแบบ | FDG "Names of Fields" ครอบคลุมเฉพาะ public/protected |
| 2 | **Local Constant** | `PascalCase` | IDN + RUN #12 ระบุชัดว่า **all** constant ใช้ PascalCase | IDN, RUN #12 |
| 3 | **Boolean local variable** | ไม่บังคับ prefix | FDG ระบุ "optionally... only where it adds value" เฉพาะ Property — local variable ชื่อธรรมดาอ่านง่ายแล้ว | FDG "Names of Properties" |
| 4 | **Member Ordering** | รวมในมาตรฐานเดียว | GOO + RUN แนะนำเพื่อ consistency — ไม่กระทบ compile/runtime แต่ช่วยให้โค้ดเป็นระเบียบ | GOO, RUN #15 |
| 5 | **C# Version** | C# 14 (ไม่ใช่ 13) | .NET 10 มาพร้อม C# 14 | [learn.microsoft.com/.../csharp-14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14) |

#### ผลสรุป

| ผลลัพธ์ | จำนวน |
|:--------|:-----:|
| ✅ ผ่านมาตรฐานสากล | **62 เกณฑ์** |
| ❌ ต้องแก้ไข | **0** |
| ⚠️ Over-engineer | **0** |

**✅ Section 2 ผ่านการ audit ครบทุกเกณฑ์** — เป็น standard เดียว ไม่แบ่งระดับ ทุกข้อมีแหล่งอ้างอิงรองรับจาก Microsoft, Google, dotnet/runtime และ Roslyn

---

## 3. DataAccess Layer — ชั้นเข้าถึงข้อมูล

> **มาตรฐาน: Dapper + Repository Pattern**  
> Dapper เป็น Micro-ORM มาตรฐานเดียวที่อนุญาตสำหรับทุกโปรเจค  
> เหตุผล: Performance ใกล้เคียง ADO.NET (≤5%), SQL Control เต็มที่, Code กระชับ  
> ดูรายละเอียดวิจัย: [Research/Database_Access_Methods_NET10.md](./Research/Database_Access_Methods_NET10.md)

### Data Access Pattern

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.1 | ต้องใช้ **Dapper** เป็น Data Access หลักเท่านั้น — ห้ามใช้ ORM อื่น (EF Core, NHibernate, etc.) | ☐ |
| 3.2 | ต้องใช้ **Repository Pattern** — ห้าม inject `IDbConnection` เข้า Service โดยตรง | ☐ |
| 3.3 | ต้องมี **`IDbConnectionFactory`** สำหรับสร้าง Connection — ห้าม `new SqlConnection()` ตรงใน Repository | ☐ |
| 3.4 | ต้องมี **`BaseRepository`** เป็น Generic Base Class ที่ wrap Dapper methods | ☐ |
| 3.5 | ถ้ามีมากกว่า 1 Database → `IDbConnectionFactory` ต้องมี method แยกต่อ DB (`CreateMainConnection()`, `CreateLogConnection()`) | ☐ |

### SQL Management

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.6 | **ห้ามใช้ String Interpolation ใน SQL** — ต้องใช้ `@parameter` เสมอ (ป้องกัน SQL Injection — **CWE-89**, OWASP ASVS v5.0-1.2.4, OWASP Top 10 A03:2021, ISO 27001 A.8.28) | ☐ |
| 3.7 | SQL ต้องแยกเป็น **`static class`** ใน `SqlQueries/` — ห้ามฝัง SQL string ใน Repository Method | ☐ |
| 3.8 | Column Alias ใน SQL ต้องตรงกับ **Property Name** ของ Model/Entity เสมอ (case-insensitive) | ☐ |
| 3.9 | ทุก Multi-operation ต้องใช้ **Transaction** — ห้าม Execute หลาย command แยกโดยไม่มี Transaction | ☐ |
| 3.10 | Connection ต้องปิดด้วย **`using`** เสมอ — ห้าม Leave Connection Open (**CWE-404**: Improper Resource Shutdown) | ☐ |
| 3.11 | **ห้ามใช้ `SELECT *`** — ต้องระบุ Column ที่ต้องการเท่านั้น (Performance + Least-Privilege data exposure — SQL Best Practice, **CWE-200**) | ☐ |

### Async & CancellationToken

> 📖 **อ้างอิง:** ASP.NET Core Best Practice, Enterprise Dapper Standard

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.12 | ทุก Repository method ที่เข้าถึง Database ต้องเป็น **`async Task<T>`** — ห้ามใช้ synchronous | ☐ |
| 3.13 | ทุก async method ต้องรับ **`CancellationToken cancellationToken = default`** เป็น parameter สุดท้าย | ☐ |
| 3.14 | ต้องส่ง `CancellationToken` ผ่านไปยัง Dapper ผ่าน **`CommandDefinition`** — เช่น `new CommandDefinition(sql, param, cancellationToken: ct)` (Dapper ไม่รับ CancellationToken โดยตรงใน QueryAsync) | ☐ |
| 3.15 | ห้ามใช้ **`.Result`** หรือ **`.Wait()`** บน async method — ต้อง `await` เสมอ (ป้องกัน deadlock) | ☐ |

### Timeout & Resilience

> 📖 **อ้างอิง:** ISO 25010 Reliability, Azure SQL Best Practice, Polly Library

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.16 | ต้องกำหนด **`commandTimeout`** ค่าเริ่มต้นใน `BaseRepository` — ค่าแนะนำ: **30 วินาที** (ปรับได้ต่อ operation) | ☐ |
| 3.17 | Query ที่คาดว่าจะใช้เวลานาน (Report, Export) ต้องกำหนด `commandTimeout` แยก — สูงสุดไม่เกิน **300 วินาที** | ☐ |
| 3.18 | ต้องมี **Retry Policy** (แนะนำ Polly) สำหรับ Transient SQL Error — Deadlock (1205), Azure Transient (40197, 40501, 49918) | ☐ |
| 3.19 | Retry ต้องใช้ **Exponential Backoff** — ไม่เกิน **3 ครั้ง**, delay 1s → 2s → 4s | ☐ |
| 3.20 | ห้าม Retry บน **Non-Transient Error** (เช่น SQL syntax error, constraint violation) | ☐ |

### Connection Security & Pool

> 📖 **อ้างอิง:** OWASP Database Security Cheat Sheet, OWASP ASVS 5.0 (V12.3, V13.2, V13.3), CWE-798/259/311/295, NIST SP 800-53 (AC-6, IA-5, SC-8, SC-28), ISO 27001:2022 A.8.3/A.8.24/A.8.28

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.21 | Connection String **ห้ามอยู่ใน Source Code** — ต้องอยู่ใน `appsettings.{Environment}.json` หรือ Secret Manager / Vault (**CWE-798**, OWASP ASVS v5.0-13.3.1, NIST IA-5) | ☐ |
| 3.22 | Production Connection String ต้องเข้ารหัสหรืออยู่ใน **Secret Vault** (Azure Key Vault, HashiCorp Vault, etc.) (**CWE-259**, OWASP ASVS v5.0-13.3.1) | ☐ |
| 3.23 | Connection String ต้องใช้ **`Encrypt=True;TrustServerCertificate=False`** สำหรับ Production (TLS 1.2+) (**CWE-311/295**, OWASP ASVS v5.0-12.3.2, NIST SC-8, ISO 27001 A.8.24) | ☐ |
| 3.24 | ต้องกำหนด **Connection Pool** ใน Connection String: `Max Pool Size`, `Min Pool Size`, `Connection Timeout` | ☐ |
| 3.25 | ค่าแนะนำ Connection Pool: `Max Pool Size=200;Min Pool Size=10;Connection Timeout=30;Connection Lifetime=600` | ☐ |
| 3.26 | Database Account ต้องใช้ **Least Privilege** — ห้ามใช้ `sa` หรือ `db_owner` ใน Production (**CWE-250**, NIST AC-6, OWASP ASVS v5.0-13.2.2) | ☐ |

### Audit Logging (Write Operations)

> 📖 **อ้างอิง:** ISO 27001:2022 A.8.15, NIST SP 800-53 AU-2/AU-3/AU-12, SOC 2 CC6.1, OWASP ASVS v5.0-16.3.3

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.27 | ทุก **INSERT / UPDATE / DELETE** operation ต้องมี Log — ระบุ Who, What, When | ☐ |

### Model/Entity

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 3.28 | Entity/Model Class ต้องมี **เฉพาะ Property** เท่านั้น — ห้ามมี Business Logic | ☐ |
| 3.29 | ใช้ NuGet Package: **`Dapper`** (และ `Dapper.Contrib` ถ้าต้องการ basic CRUD auto-generate) | ☐ |
| 3.30 | ห้ามใช้ NuGet ต่อไปนี้: `Easybuy.WebApplication.CoreUnitOfWork`, `Microsoft.EntityFrameworkCore.*` — ทุกโปรเจค | ☐ |

### ตัวอย่างโครงสร้าง DataAccess ที่ถูกต้อง

```
DataAccess/
├── Connections/
│   ├── IDbConnectionFactory.cs
│   └── DbConnectionFactory.cs
├── Repositories/
│   ├── Interfaces/
│   │   ├── IBaseRepository.cs
│   │   └── ICustomerRepository.cs
│   ├── BaseRepository.cs
│   └── CustomerRepository.cs
└── SqlQueries/
    ├── CustomerQueries.cs
    └── TransactionQueries.cs
```

### ตัวอย่าง Code

**IDbConnectionFactory:**
```csharp
// DataAccess/Connections/IDbConnectionFactory.cs
public interface IDbConnectionFactory
{
    DbConnection CreateMainConnection();
    DbConnection CreateLogConnection();   // ← เพิ่มตาม DB ที่มี
}
```

**DbConnectionFactory:**
```csharp
// DataAccess/Connections/DbConnectionFactory.cs
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
        => _configuration = configuration;

    public DbConnection CreateMainConnection()
        => new SqlConnection(_configuration.GetConnectionString("MainDb"));

    public DbConnection CreateLogConnection()
        => new SqlConnection(_configuration.GetConnectionString("LogDb"));
}
```

**BaseRepository:**
```csharp
// DataAccess/Repositories/BaseRepository.cs
public abstract class BaseRepository
{
    // ✅ I1: private — subclass ใช้ผ่าน protected method ไม่ต้องเข้าถึง field ตรง
    private readonly IDbConnectionFactory _connectionFactory;

    // ✅ G2: Default CommandTimeout (วินาที) — override ได้ต่อ method
    private const int DefaultCommandTimeout = 30;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    // ✅ G1: CancellationToken ทุก method | G2: commandTimeout
    // ✅ I2: ConfigureAwait(false) ป้องกัน deadlock
    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        object? param = null,
        string? dbKey = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = CreateConnection(dbKey);
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.QuerySingleOrDefaultAsync<T>(command)
            .ConfigureAwait(false);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        string? dbKey = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = CreateConnection(dbKey);
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.QueryAsync<T>(command)
            .ConfigureAwait(false);
    }

    protected async Task<int> ExecuteAsync(
        string sql,
        object? param = null,
        string? dbKey = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = CreateConnection(dbKey);
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.ExecuteAsync(command)
            .ConfigureAwait(false);
    }

    protected async Task<T> ExecuteInTransactionAsync<T>(
        Func<DbConnection, DbTransaction, Task<T>> action,
        string? dbKey = null,
        CancellationToken cancellationToken = default)
    {
        // ✅ ใช้ DbConnection (ไม่ใช่ IDbConnection) เพราะ OpenAsync/BeginTransactionAsync อยู่บน DbConnection
        await using var conn = CreateConnection(dbKey);
        await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await conn.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
        try
        {
            var result = await action(conn, transaction)
                .ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    // ✅ Return DbConnection (ไม่ใช่ IDbConnection) — รองรับ OpenAsync/BeginTransactionAsync
    private DbConnection CreateConnection(string? dbKey) => dbKey switch
    {
        "Log" => _connectionFactory.CreateLogConnection(),
        _     => _connectionFactory.CreateMainConnection()
    };
}
```

**SqlQueries (SQL Constants):**
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
}
```

**Repository Interface:**
```csharp
// DataAccess/Repositories/Interfaces/ICustomerRepository.cs
public interface ICustomerRepository
{
    // ✅ G1: CancellationToken ทุก method
    Task<CustomerEntity?> GetByIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerEntity>> GetActiveListAsync(CancellationToken cancellationToken = default);
    Task<int> InsertAsync(CustomerEntity entity, CancellationToken cancellationToken = default);
    Task<int> UpdateStatusAsync(string customerId, string status, CancellationToken cancellationToken = default);
}
```

**Repository Implementation:**
```csharp
// DataAccess/Repositories/CustomerRepository.cs
public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    // ✅ G1: ส่ง CancellationToken ถึง BaseRepository
    public async Task<CustomerEntity?> GetByIdAsync(
        string customerId, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<CustomerEntity>(
            CustomerQueries.GetById, new { customerId },
            cancellationToken: cancellationToken);

    public async Task<IEnumerable<CustomerEntity>> GetActiveListAsync(
        CancellationToken cancellationToken = default)
        => await QueryAsync<CustomerEntity>(
            CustomerQueries.GetActiveList,
            cancellationToken: cancellationToken);

    public async Task<int> InsertAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            CustomerQueries.Insert, entity,
            cancellationToken: cancellationToken);

    public async Task<int> UpdateStatusAsync(
        string customerId, string status, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            CustomerQueries.UpdateStatus, new { customerId, status },
            cancellationToken: cancellationToken);
}
```

**Stored Procedure Example (ผ่าน BaseRepository):**
```csharp
// DataAccess/Repositories/TransactionRepository.cs
// ✅ I3: SP ผ่าน BaseRepository — ใช้ QueryStoredProcedureAsync (เพิ่มใน BaseRepository)
public async Task<IEnumerable<TransactionModel>> GetByCustomerAsync(
    string customerId, CancellationToken cancellationToken = default)
    => await QueryStoredProcedureAsync<TransactionModel>(
        "sp_GetCustomerTransactions",
        new { CustomerId = customerId },
        cancellationToken: cancellationToken);
```

> 💡 **BaseRepository ควรมี method สำหรับ Stored Procedure:**
> ```csharp
> protected async Task<IEnumerable<T>> QueryStoredProcedureAsync<T>(
>     string storedProcedure, object? param = null,
>     string? dbKey = null, int? commandTimeout = null,
>     CancellationToken cancellationToken = default)
> {
>     using var conn = CreateConnection(dbKey);
>     var command = new CommandDefinition(
>         storedProcedure, param,
>         commandType: CommandType.StoredProcedure,
>         commandTimeout: commandTimeout ?? DefaultCommandTimeout,
>         cancellationToken: cancellationToken);
>     return await conn.QueryAsync<T>(command).ConfigureAwait(false);
> }
> ```

**Retry Policy Example (Polly — G3):**
```csharp
// Infrastructure/Resilience/SqlResiliencePolicy.cs
// ✅ G3: Retry Policy สำหรับ Transient SQL Error
public static class SqlResiliencePolicy
{
    private static readonly int[] TransientErrorNumbers =
    {
        1205,   // Deadlock
        -2,     // Timeout
        40197,  // Azure: service error
        40501,  // Azure: service busy
        40613,  // Azure: database unavailable
        49918,  // Azure: not enough resources
    };

    public static IAsyncPolicy CreateRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<SqlException>(ex =>
                ex.Errors.Cast<SqlError>()
                    .Any(e => TransientErrorNumbers.Contains(e.Number)))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)), // 1s → 2s → 4s
                onRetry: (exception, delay, attempt, _) =>
                    logger.LogWarning(exception,
                        "SQL transient error. Retry {Attempt} after {Delay}s",
                        attempt, delay.TotalSeconds));
    }
}
```

**Connection String ตัวอย่าง (G5/G6):**
```
// ✅ G5: Encrypt + TLS | G6: Pool Configuration
Server=myserver;Database=mydb;User Id=app_readonly;Password=***;
Encrypt=True;TrustServerCertificate=False;
Max Pool Size=200;Min Pool Size=10;Connection Timeout=30;Connection Lifetime=600;
```

> 📌 **หมายเหตุ:** ไม่จำเป็นต้องใช้ raw ADO.NET — Dapper เร็วกว่า ADO.NET เพียง ≤5%  
> แต่ code น้อยกว่า 5-10 เท่า และลด bug จาก manual mapping  
> ดูผลวิจัย: [Research/Database_Access_Methods_NET10.md](./Research/Database_Access_Methods_NET10.md#12-การตัดสินใจ--adonet-vs-dapper-ผลวิจัยเพิ่มเติม)

### ข้อยกเว้น — กรณีที่อนุญาตให้ใช้ Raw ADO.NET

> ⚠️ **ADO.NET ใช้ได้เฉพาะกรณีด้านล่างเท่านั้น** — ต้องผ่านเกณฑ์คัดเลือก **ทุกข้อ** ก่อนใช้  
> ถ้าไม่เข้าเกณฑ์ → **ต้องใช้ Dapper** เท่านั้น

#### Decision Tree — Dapper หรือ ADO.NET?

```
เริ่มต้น: ต้องเข้าถึง Database
│
├─ Q1: ต้องใช้ SqlBulkCopy (insert ≥10,000 แถวต่อครั้ง)?
│  ├─ ใช่ → ✅ ใช้ ADO.NET (Case A)
│  └─ ไม่ ↓
│
├─ Q2: ต้อง Stream forward-only + ต้องการ column-level SequentialAccess (BLOB/CLOB) หรือ custom CommandBehavior?
│  ├─ ใช่ → ✅ ใช้ ADO.NET (Case B)
│  ├─ แค่ Stream row-by-row (≥100,000 แถว) → ✅ ใช้ Dapper `QueryUnbufferedAsync<T>` (IAsyncEnumerable)
│  └─ ไม่ ↓
│
├─ Q3: ต้องใช้ Table-Valued Parameters (TVP) ส่ง DataTable เข้า Stored Procedure?
│  ├─ ใช่ → ✅ ใช้ ADO.NET (Case C)
│  └─ ไม่ ↓
│
├─ Q4: ต้องใช้ Connection-level Events (เช่น InfoMessage, StateChange)?
│  ├─ ใช่ → ✅ ใช้ ADO.NET (Case D)
│  └─ ไม่ ↓
│
└─ ❌ ไม่เข้าเกณฑ์ → ต้องใช้ Dapper
```

#### เกณฑ์คัดเลือก — ต้องผ่านทุกข้อก่อนใช้ ADO.NET

| # | เกณฑ์ที่ต้องผ่าน | ผ่าน / ไม่ผ่าน |
|---|-----------------|:--------------:|
| E.1 | ต้องเข้า **อย่างน้อย 1 ใน 4 Cases** (A/B/C/D) ข้างบน — ถ้าไม่เข้าเลย = ห้ามใช้ ADO.NET | ☐ |
| E.2 | ต้องมี **เอกสาร/Comment ใน Code** อธิบายว่าทำไมต้องใช้ ADO.NET แทน Dapper | ☐ |
| E.3 | Code ADO.NET ต้องอยู่ใน **Repository Layer เท่านั้น** — ห้ามมีใน Service/Controller | ☐ |
| E.4 | ต้อง **wrap ด้วย method ใน Repository** ที่มี Interface — ห้าม expose ADO.NET types ออกนอก Repository | ☐ |
| E.5 | ต้องผ่าน **Code Review อย่างน้อย 1 คน** ก่อน Merge — Reviewer ต้อง confirm ว่าเข้าเกณฑ์ | ☐ |
| E.6 | **ห้ามใช้ `AddWithValue()`** — ต้องใช้ `new SqlParameter(name, SqlDbType.xxx) { Value = ... }` เสมอ (ป้องกัน implicit type conversion → query plan ไม่ cache) | ☐ |
| E.7 | ต้องกำหนด **`CommandTimeout`** ทุก `SqlCommand` — ห้ามใช้ค่า default (30s) โดยไม่ระบุ explicit | ☐ |
| E.8 | ต้องส่ง **`CancellationToken`** ไปยังทุก Async ADO.NET method (`OpenAsync`, `ExecuteReaderAsync`, etc.) | ☐ |

#### Case A: SqlBulkCopy — Bulk Insert ข้อมูลจำนวนมาก

| เงื่อนไข | รายละเอียด |
|----------|------------|
| **เมื่อไหร่** | Insert **≥10,000 แถว** ต่อ operation (เช่น CSV import, data sync, batch processing) |
| **ทำไม Dapper ไม่ได้** | Dapper ทำ row-by-row execute — `SqlBulkCopy` ใช้ TDS protocol stream ตรง เร็วกว่า **10-100x** |
| **ห้ามใช้ถ้า** | Insert < 10,000 แถว → ใช้ Dapper `ExecuteAsync` กับ list ได้ |

```csharp
// ✅ อนุญาต — SqlBulkCopy สำหรับ Bulk Insert
public async Task BulkInsertAsync(
    DataTable dataTable, CancellationToken cancellationToken = default)
{
    // ADO.NET Exception: SqlBulkCopy — Dapper ไม่รองรับ bulk protocol
    using var conn = new SqlConnection(_connectionString);
    await conn.OpenAsync(cancellationToken);
    using var bulkCopy = new SqlBulkCopy(conn)
    {
        DestinationTableName = "staging_transactions",
        BatchSize = 5000,
        BulkCopyTimeout = 120  // ← E.7: explicit timeout
    };
    await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
}
```

#### Case B: Streaming DataReader — Column-Level SequentialAccess

> ⚠️ **Update:** Dapper 2.x มี **`QueryUnbufferedAsync<T>`** (บน `DbConnection`) ที่ return `IAsyncEnumerable<T>` — ใช้ stream row-by-row ได้โดยไม่ต้อง ADO.NET  
> ADO.NET DataReader จำเป็น **เฉพาะ** กรณีต้องการ column-level `SequentialAccess` (BLOB/CLOB) หรือ custom `CommandBehavior`

| เงื่อนไข | รายละเอียด |
|----------|------------|
| **เมื่อไหร่** | ต้องอ่านข้อมูลแบบ **column-level SequentialAccess** (เช่น stream BLOB/CLOB, `GetStream()`, `GetTextReader()`) หรือต้องกำหนด `CommandBehavior` เอง |
| **ทำไม Dapper ไม่ได้** | `QueryUnbufferedAsync<T>` stream row ได้ แต่ไม่รองรับ column-level SequentialAccess (`GetStream`, `GetBytes`) — ต้องใช้ `DbDataReader` โดยตรง |
| **ห้ามใช้ถ้า** | แค่ stream row-by-row → ใช้ **Dapper `QueryUnbufferedAsync<T>`** แทน |

> 💡 **Row-Level Streaming (ใช้ Dapper ได้):**
> ```csharp
> // ✅ Dapper QueryUnbufferedAsync — IAsyncEnumerable<T> streaming
> await foreach (var item in conn.QueryUnbufferedAsync<TransactionModel>(
>     TransactionQueries.ExportByDateRange,
>     new { FromDate = from, ToDate = to }).WithCancellation(ct))
> {
>     await writer.WriteLineAsync($"{item.TransactionId},{item.Amount}");
> }
> ```

```csharp
// ✅ อนุญาต — Streaming DataReader สำหรับ large dataset export
// ✅ G7: ใช้ explicit SqlParameter type — ห้ามใช้ AddWithValue
public async IAsyncEnumerable<TransactionExportModel> StreamTransactionsAsync(
    DateTime from, DateTime to,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    // ADO.NET Exception: Streaming — ห้ามใช้ Dapper เพราะ materialize ทั้ง List
    using var conn = new SqlConnection(_connectionString);
    await conn.OpenAsync(cancellationToken);
    using var cmd = new SqlCommand(TransactionQueries.ExportByDateRange, conn);
    cmd.CommandTimeout = 300; // ← Report/Export อนุญาตสูงสุด 300s
    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime2) { Value = from });
    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime2) { Value = to });
    
    using var reader = await cmd.ExecuteReaderAsync(
        CommandBehavior.SequentialAccess, cancellationToken);
    while (await reader.ReadAsync(cancellationToken))
    {
        yield return new TransactionExportModel
        {
            TransactionId = reader.GetString(0),
            Amount = reader.GetDecimal(1),
            CreatedDate = reader.GetDateTime(2)
        };
    }
}
```

#### Case C: Table-Valued Parameters (TVP)

| เงื่อนไข | รายละเอียด |
|----------|------------|
| **เมื่อไหร่** | ต้องส่ง **DataTable / structured data** เข้า Stored Procedure ผ่าน `SqlDbType.Structured` |
| **ทำไม Dapper ไม่ได้** | Dapper support TVP จำกัด ต้อง workaround ซับซ้อน — ADO.NET ส่งตรง native |
| **ห้ามใช้ถ้า** | สามารถส่ง parameter เป็น comma-separated string แล้ว split ใน SP ได้ หรือใช้ JSON parameter |

```csharp
// ✅ อนุญาต — TVP สำหรับ Stored Procedure ที่รับ DataTable
public async Task<int> ProcessBatchAsync(
    DataTable batchItems, CancellationToken cancellationToken = default)
{
    // ADO.NET Exception: TVP — Dapper ไม่รองรับ SqlDbType.Structured natively
    using var conn = new SqlConnection(_connectionString);
    await conn.OpenAsync(cancellationToken);
    using var cmd = new SqlCommand("sp_ProcessBatch", conn)
    {
        CommandType = CommandType.StoredProcedure,
        CommandTimeout = 60  // ← E.7: explicit timeout
    };
    cmd.Parameters.Add(new SqlParameter("@BatchItems", SqlDbType.Structured)
    {
        TypeName = "dbo.BatchItemType",
        Value = batchItems
    });
    return await cmd.ExecuteNonQueryAsync(cancellationToken);
}
```

#### Case D: Connection-Level Events

| เงื่อนไข | รายละเอียด |
|----------|------------|
| **เมื่อไหร่** | ต้อง subscribe **`InfoMessage`**, **`StateChange`** หรือ event อื่นบน `SqlConnection` |
| **ทำไม Dapper ไม่ได้** | Dapper ไม่ expose connection events — ต้องใช้ `SqlConnection` โดยตรง |
| **ห้ามใช้ถ้า** | ต้องการแค่ log query result → ใช้ Dapper + `ILogger` แทน |

```csharp
// ✅ อนุญาต — Connection Event สำหรับ capture SQL Print/RAISERROR messages
public async Task<IEnumerable<string>> ExecuteWithMessagesAsync(
    string storedProc, CancellationToken cancellationToken = default)
{
    // ADO.NET Exception: InfoMessage event — Dapper ไม่รองรับ
    var messages = new List<string>();
    using var conn = new SqlConnection(_connectionString);
    conn.InfoMessage += (sender, e) => messages.Add(e.Message);
    await conn.OpenAsync(cancellationToken);
    using var cmd = new SqlCommand(storedProc, conn)
    {
        CommandType = CommandType.StoredProcedure,
        CommandTimeout = 30  // ← E.7: explicit timeout
    };
    await cmd.ExecuteNonQueryAsync(cancellationToken);
    return messages;
}
```

#### ❌ กรณีที่ห้ามใช้ ADO.NET (แม้จะคิดว่าเร็วกว่า)

| สถานการณ์ | เหตุผลที่ห้าม | ใช้อะไรแทน |
|-----------|-------------|------------|
| "อยากให้เร็วขึ้น" สำหรับ query ทั่วไป | Gap แค่ ≤5% (34μs/query) — ไม่คุ้ม code 5-10x | **Dapper** |
| Insert ข้อมูล < 10,000 แถว | Dapper `ExecuteAsync` เพียงพอ ไม่ต้อง SqlBulkCopy | **Dapper** |
| อ่านข้อมูล < 100,000 แถว | Dapper `QueryAsync` load ได้สบายใน Memory | **Dapper** |
| Stream row-by-row (≥100,000 แถว) | Dapper `QueryUnbufferedAsync<T>` return `IAsyncEnumerable<T>` — ไม่ต้อง ADO.NET | **Dapper** |
| Stored Procedure ทั่วไป | Dapper รองรับ SP ได้ 100% + code สั้นกว่า | **Dapper** |
| "คุ้นเคย ADO.NET มากกว่า" | ไม่ใช่เหตุผลทางเทคนิค — ต้องเรียนรู้ Dapper | **Dapper** |
| JOIN / Complex Query | Dapper ส่ง SQL ตรง — เร็วเท่า ADO.NET | **Dapper** |
| Transaction (multi-operation) | `BaseRepository.ExecuteInTransactionAsync()` รองรับแล้ว | **Dapper** |

---

### Audit Report — ตรวจสอบความถูกต้อง Section 3

> 📅 **วันที่ตรวจสอบ:** 5 มีนาคม 2026 (Revision 4 — ASVS 5.0 ID Correction + NIST/ISO Accuracy Audit)  
> 🔍 **วิธีการ:**  
> 1. Dapper Source Code (SqlMapper.Async.cs) → ตรวจสอบ API จริง  
> 2. OWASP ASVS 5.0.0 (GitHub v5.0.0 tag) → Verify requirement IDs จาก source files จริง  
> 3. CWE Top 25 (2024) → Map vulnerabilities to criteria  
> 4. OWASP Cheat Sheets → Verify security claims  
> 5. ISO/IEC 27001:2022 Annex A → Cross-reference technological controls (A.8.x)  
> 6. NIST SP 800-53 Rev 5 → Verify control definitions จาก NIST catalog  

#### แหล่งอ้างอิงมาตรฐานสากล

| รหัส | มาตรฐาน | ประเภท | URL |
|:----:|---------|:------:|-----|
| **CWE** | MITRE CWE Top 25 (2024) | Vulnerability Classification | [cwe.mitre.org/top25](https://cwe.mitre.org/top25/archive/2024/2024_top25_list.html) |
| **ASVS** | OWASP ASVS 5.0.0 (May 2025) | Application Security Standard | [github.com/OWASP/ASVS v5.0.0](https://github.com/OWASP/ASVS/tree/v5.0.0) |
| **OWS** | OWASP SQL Injection Prevention | Cheat Sheet | [cheatsheetseries.owasp.org](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html) |
| **ODB** | OWASP Database Security | Cheat Sheet | [cheatsheetseries.owasp.org](https://cheatsheetseries.owasp.org/cheatsheets/Database_Security_Cheat_Sheet.html) |
| **OT10** | OWASP Top 10 (2021) | Risk Classification | [owasp.org/Top10](https://owasp.org/Top10/) |
| **NIST** | NIST SP 800-53 Rev 5 | Security Controls | [csrc.nist.gov](https://csrc.nist.gov/publications/detail/sp/800-53/rev-5/final) |
| **ISO27** | ISO/IEC 27001:2022 | InfoSec Management | Annex A Controls |
| **ISO55** | ISO/IEC 5055:2021 | Automated Source Code Quality | CISQ Standard |
| **DAP** | Dapper Source Code | Source Verification | [github.com/DapperLib/Dapper](https://github.com/DapperLib/Dapper/blob/main/Dapper/SqlMapper.Async.cs) |
| **POL** | Polly Resilience Library | Library Reference | [github.com/App-vNext/Polly](https://github.com/App-vNext/Polly) |
| **ADO** | Microsoft ADO.NET Docs | API Reference | [learn.microsoft.com](https://learn.microsoft.com/dotnet/api/system.data.common.dbconnection) |

#### Mapping เกณฑ์ → มาตรฐานสากล

| เกณฑ์ | คำอธิบาย | CWE ID | OWASP ASVS 5.0 | OWASP Top 10 | NIST 800-53 | ISO 27001:2022 | สถานะ |
|:------|:---------|:------:|:--------------:|:------------:|:-----------:|:--------------:|:-----:|
| **3.6** ห้าม String Interpolation SQL | ต้องใช้ @parameter | **CWE-89** (#3 Top 25) | **1.2.4** (parameterized queries) | **A03:2021** Injection | **SI-10** (Input Validation) | **A.8.28** (Secure coding) | ✅ |
| **3.9** Transaction ทุก multi-op | ACID compliance | — | — | — | — | — | ✅ |
| **3.10** using สำหรับ Connection | ปิด connection เสมอ | **CWE-404** (Improper Resource Shutdown) | — | — | — | — | ✅ |
| **3.11** ห้าม SELECT * | ระบุ column เท่านั้น | **CWE-200** (#17 Top 25) | — | **A01:2021** Broken Access Control | **AC-6** (Least Privilege) | **A.8.3** (Info access restriction) | ✅ |
| **3.15** ห้าม .Result/.Wait() | ป้องกัน deadlock | **CWE-833** (Deadlock) | — | — | — | — | ✅ |
| **3.18** Retry Policy (Polly) | Transient SQL Error | — | — | — | — | — | ✅ |
| **3.20** ห้าม retry non-transient | Filter by error type | — | — | — | — | — | ✅ |
| **3.21** Connection String ไม่อยู่ใน code | Secret Management | **CWE-798** (#22 Top 25) | **13.3.1** (secrets not in source) | **A07:2021** Identification Failures | **IA-5** (Authenticator Mgmt) | **A.5.17** (Authentication info) | ✅ |
| **3.22** Production ใน Secret Vault | Encrypted credential storage | **CWE-259** (Hard-coded Password) | **13.3.1** (use secret vault) | **A07:2021** | **SC-28** (Protection at Rest) | **A.8.24** (Cryptography) | ✅ |
| **3.23** Encrypt=True; TrustServerCertificate=False | TLS 1.2+ | **CWE-311** + **CWE-295** | **12.3.2** (TLS cert validation) | **A02:2021** Crypto Failures | **SC-8** (Transmission Confidentiality) | **A.8.24** (Cryptography) | ✅ |
| **3.26** Least Privilege | ห้าม sa/db_owner | **CWE-250** (Unnecessary Privileges) | **13.2.2** (least necessary privileges) | **A01:2021** Broken Access Control | **AC-6** (Least Privilege) | **A.8.3** (Info access restriction) | ✅ |
| **3.27** Audit Logging write ops | Who/What/When | — | **16.3.3** (log security events) | **A09:2021** Security Logging | **AU-2**, **AU-3**, **AU-12** | **A.8.15** (Logging) | ✅ |
| **E.6** ห้าม AddWithValue() | Explicit SqlDbType | **CWE-704** (Incorrect Type Conversion) | — | — | — | — | ✅ |
| **E.8** CancellationToken ทุก Async | Resource cleanup | **CWE-400** (#24 Top 25) | — | — | — | — | ✅ |

#### สรุปผลการตรวจสอบ (International Standards Mapping)

| หมวด | เกณฑ์ | สถานะ | มาตรฐานสากลที่ครอบคลุม |
|:-----|:------|:-----:|----------------------|
| **3.1** Dapper only | Micro-ORM เดียว | ✅ | Team Standard — DAP (18.2k stars, Stack Overflow production) |
| **3.2** Repository Pattern | ห้าม inject IDbConnection เข้า Service | ✅ | Architectural Best Practice — Separation of Concerns |
| **3.3** IDbConnectionFactory | Factory สร้าง Connection | ✅ | ADO — Factory Pattern for testability |
| **3.4** BaseRepository | Generic Base wrap Dapper | ✅ | Practical — ลด boilerplate |
| **3.5** Multi-DB method | แยก method ต่อ DB | ✅ | Practical |
| **3.6** ห้าม String Interpolation SQL | ต้องใช้ @parameter | ✅ | **CWE-89** · ASVS 5.0 §1.2.4 · OT10 A03 · OWS Defense #1 · NIST SI-10 · ISO27 A.8.28 |
| **3.7** SQL ใน static class | แยกจาก Repository | ✅ | Practical — maintainability · ISO55 maintainability metric |
| **3.8** Column Alias = Property | Dapper auto-map | ✅ | DAP source: "column-name===member-name mapping" |
| **3.9** Transaction ทุก multi-op | ACID | ✅ | ADO — ACID compliance · Data Integrity best practice |
| **3.10** using สำหรับ Connection | ปิด connection เสมอ | ✅ | **CWE-404** · ADO IDisposable contract |
| **3.11** ห้าม SELECT * | ระบุ column เท่านั้น | ✅ | **CWE-200** · OT10 A01 · NIST AC-6 · ISO27 A.8.3 · Least-privilege data |
| **3.12** Async ทุก method | async Task\<T\> | ✅ | ADO async I/O · Microsoft best practice |
| **3.13** CancellationToken parameter | default ทุก method | ✅ | Microsoft ASP.NET Core best practice |
| **3.14** CancellationToken → Dapper | ผ่าน CommandDefinition | ✅ | DAP source verified — `QueryAsync` ส่ง `default` ต้อง wrap เอง |
| **3.15** ห้าม .Result/.Wait() | ต้อง await | ✅ | **CWE-833** · Microsoft async deadlock prevention |
| **3.16** Default commandTimeout 30s | BaseRepository | ✅ | ADO SqlCommand.CommandTimeout default = 30 |
| **3.17** Report timeout 300s max | แยก commandTimeout | ✅ | Practical — bounded resource consumption |
| **3.18** Retry Policy (Polly) | Transient SQL Error | ✅ | POL recommended · Cloud Design Pattern (Retry + Backoff) |
| **3.19** Exponential Backoff | 3 ครั้ง, 1s→2s→4s | ✅ | POL WaitAndRetryAsync · Cloud design pattern |
| **3.20** ห้าม retry non-transient | Syntax/constraint error | ✅ | POL filter by error number |
| **3.21** Connection String ไม่อยู่ใน code | appsettings / Vault | ✅ | **CWE-798** · ASVS 5.0 §13.3.1 · OT10 A07 · NIST IA-5 · ISO27 A.5.17 |
| **3.22** Production ใน Secret Vault | Azure Key Vault etc. | ✅ | **CWE-259** · ASVS 5.0 §13.3.1 · ODB · NIST SC-28 · ISO27 A.8.24 |
| **3.23** Encrypt=True | TLS 1.2+ | ✅ | **CWE-311/295** · ASVS 5.0 §12.3.2 · OT10 A02 · NIST SC-8 · ISO27 A.8.24 |
| **3.24–3.25** Connection Pool config | Max/Min Pool Size | ✅ | ADO SqlConnection pool · **CWE-400** resource exhaustion prevention |
| **3.26** Least Privilege | ห้าม sa/db_owner | ✅ | **CWE-250** · ASVS 5.0 §13.2.2 · OT10 A01 · **NIST AC-6** · ISO27 A.8.3 |
| **3.27** Audit Logging write ops | Who/What/When | ✅ | ASVS 5.0 §16.3.3 · OT10 A09 · **NIST AU-2/AU-3/AU-12** · **ISO27 A.8.15** |
| **3.28** Entity = Property only | ไม่มี business logic | ✅ | Practical — anemic entity for Dapper mapping |
| **3.29** NuGet: Dapper (+Contrib) | | ✅ | DAP |
| **3.30** ห้าม EF Core / CoreUnitOfWork | | ✅ | Team standard |
| **E.1–E.5** ADO.NET exception criteria | 4 Cases + 5 guards | ✅ | ครอบคลุม SqlBulkCopy, DataReader, TVP, InfoMessage |
| **E.6** ห้าม AddWithValue() | Explicit SqlDbType | ✅ | **CWE-704** · SQL Server query plan cache issue |
| **E.7** Explicit CommandTimeout | ทุก SqlCommand | ✅ | **CWE-400** · Bounded resource consumption |
| **E.8** CancellationToken ทุก Async | Resource cleanup | ✅ | **CWE-400** · Cooperative cancellation pattern |
| **Code** | BaseRepository, Factory, Queries, Repository, Polly | ✅ | Pattern สอดคล้อง — verified against DAP source |

#### CWE Coverage Summary

| CWE ID | ชื่อ | CWE Top 25 (2024) | ครอบคลุมโดยเกณฑ์ | ระดับ |
|:------:|------|:-----------------:|-----------------|:-----:|
| **CWE-89** | SQL Injection | **#3** | 3.6 (parameterized queries) | 🔴 บังคับ |
| **CWE-200** | Exposure of Sensitive Information to an Unauthorized Actor | **#17** | 3.11 (ห้าม SELECT *) | 🔴 บังคับ |
| **CWE-250** | Execution with Unnecessary Privileges | — | 3.26 (Least Privilege) | 🔴 บังคับ |
| **CWE-259** | Use of Hard-Coded Password | — | 3.22 (Secret Vault) | 🔴 บังคับ |
| **CWE-295** | Improper Certificate Validation | — | 3.23 (TrustServerCertificate=False) | 🔴 บังคับ |
| **CWE-311** | Missing Encryption of Sensitive Data | — | 3.23 (Encrypt=True, TLS 1.2+) | 🔴 บังคับ |
| **CWE-400** | Uncontrolled Resource Consumption | **#24** | 3.24–3.25, E.7, E.8 | 🔴 บังคับ |
| **CWE-404** | Improper Resource Shutdown | — | 3.10 (using statement) | 🔴 บังคับ |
| **CWE-704** | Incorrect Type Conversion | — | E.6 (ห้าม AddWithValue) | 🔴 บังคับ |
| **CWE-798** | Use of Hard-coded Credentials | **#22** | 3.21 (Connection String ไม่อยู่ใน code) | 🔴 บังคับ |
| **CWE-833** | Deadlock | — | 3.15 (ห้าม .Result/.Wait()) | 🔴 บังคับ |

> **ครอบคลุม 11 CWE IDs** — รวม **4 รายการจาก CWE Top 25 (2024)**: CWE-89 (#3), CWE-200 (#17), CWE-798 (#22), CWE-400 (#24)

#### OWASP ASVS 5.0.0 Compliance Matrix

> ⚠️ **หมายเหตุ:** ASVS 5.0.0 (May 2025) ปรับโครงสร้าง chapter ใหม่ทั้งหมดจาก v4.0.3  
> V4 Access Control → V8 | V5 Validation → V1 | V6 Crypto → V11/V13 | V7 Logging → V16 | V9 Communication → V12 | V14 Configuration → V13

| ASVS 5.0 Requirement | คำอธิบาย | Section 3 เกณฑ์ | สถานะ |
|:---------------------:|----------|:----------------:|:-----:|
| **1.2.4** | Database queries use parameterized queries, ORMs, or are protected from injection attacks. Also relevant for stored procedures | 3.6 | ✅ |
| **12.3.2** | TLS clients validate certificates received before communicating with a TLS server | 3.23 | ✅ |
| **13.2.2** | Communications between backend components are performed with accounts assigned the least necessary privileges | 3.26 | ✅ |
| **13.3.1** | Secrets management solution is used; secrets must not be included in source code or build artifacts | 3.21, 3.22 | ✅ |
| **16.3.3** | Application logs security events defined in documentation and logs attempts to bypass security controls | 3.27 | ✅ |

> **ครอบคลุม 5 ASVS requirements** — Level 1 & 2 compliance สำหรับ Database Access Layer

#### NIST SP 800-53 Rev 5 Control Mapping

| Control | คำอธิบาย | Section 3 เกณฑ์ | สถานะ |
|:-------:|----------|:----------------:|:-----:|
| **AC-6** | Least Privilege | 3.11, 3.26 | ✅ |
| **AU-2** | Event Logging | 3.27 | ✅ |
| **AU-3** | Content of Audit Records | 3.27 (Who/What/When) | ✅ |
| **AU-12** | Audit Record Generation | 3.27 | ✅ |
| **IA-5** | Authenticator Management | 3.21, 3.22 | ✅ |
| **SC-8** | Transmission Confidentiality | 3.23 | ✅ |
| **SC-28** | Protection of Information at Rest | 3.22 | ✅ |
| **SI-10** | Information Input Validation | 3.6 | ✅ |

> **ครอบคลุม 8 NIST controls** — Moderate baseline สำหรับ Financial/Banking applications

#### ประเด็นสำคัญที่ตัดสินใจ

| # | ประเด็น | มาตรฐานนี้ | เหตุผล | แหล่ง Verified |
|:-:|---------|-----------|--------|:--------------:|
| 1 | **IDbConnectionFactory → DbConnection** | return `DbConnection` | `OpenAsync()` / `BeginTransactionAsync()` อยู่บน `DbConnection` เท่านั้น | DAP source: `TryOpenAsync` checks `is DbConnection` |
| 2 | **CancellationToken → CommandDefinition** | ต้องใช้ `CommandDefinition` | `QueryAsync<T>(sql, ...)` ส่ง `default` CT ภายใน — ต้อง wrap เอง | DAP source: `new CommandDefinition(sql, ..., default)` |
| 3 | **QueryUnbufferedAsync** | Dapper **CAN** async stream | Dapper 2.x มี `QueryUnbufferedAsync<T>` → `IAsyncEnumerable<T>` | DAP source verified |
| 4 | **QueryAsync buffered:false** | fallback sync read | unbuffered → `ExecuteReaderSync<T>` ใช้ sync `reader.Read()` | DAP source: `// can't use ReadAsync` |
| 5 | **SELECT * อ้างอิง** | SQL Best Practice + **CWE-200** | OWASP ไม่ได้กล่าว `SELECT *` ตรงๆ — map เป็น CWE-200 (least-privilege data) | OWS verified + CWE cross-ref |
| 6 | **ConfigureAwait(false)** | ใช้ในทุก await ใน BaseRepository | Library code — defensive practice | Microsoft best practice |

#### ผลสรุป

| ผลลัพธ์ | จำนวน |
|:--------|:-----:|
| ✅ ผ่านมาตรฐาน | **30 เกณฑ์ + 8 ADO.NET guards + 5 code examples** |
| 🌐 CWE IDs ครอบคลุม | **11 CWE** (รวม 4 จาก Top 25: #3, #17, #22, #24) |
| 📋 OWASP ASVS 5.0 | **5 requirements** (verified จาก v5.0.0 GitHub source) |
| 🏛️ NIST SP 800-53 | **8 controls** (Moderate baseline) |
| 📜 ISO 27001:2022 | **5 Annex A controls** (A.5.17, A.8.3, A.8.15, A.8.24, A.8.28) |
| 🔧 แก้ไขรอบนี้ (R4) | **11 จุด** — แก้ ASVS IDs ×5, NIST ×2, ISO ×3, CWE ×1 |
| 🔧 แก้ไขรอบก่อน | R1: 4 จุด · R2: 1 จุด · R3: เพิ่ม mapping |

#### Revision 4 Changelog — สิ่งที่แก้ไขรอบนี้

| # | ประเภท | ก่อนแก้ (ผิด) | หลังแก้ (ถูก) | เหตุผล |
|:-:|:------:|:-------------|:-------------|--------|
| 1 | ASVS ID | v5.0-4.3.3 | **13.2.2** | V4 Access Control → V13 ใน v5.0 |
| 2 | ASVS ID | v5.0-6.4.2 | **13.3.1** | V6 Cryptography → V13 ใน v5.0 |
| 3 | ASVS ID | v5.0-7.2.1 | **16.3.3** | V7 Logging → V16 ใน v5.0 |
| 4 | ASVS ID | v5.0-9.2.1 | **12.3.2** | V9 Communication → V12 ใน v5.0 |
| 5 | ASVS ID | v5.0-14.3.4 | **13.3.1** | V14 Configuration → V13 ใน v5.0 |
| 6 | NIST | AC-4 (Info Flow) | **AC-6** (Least Privilege) | AC-4 = network/cross-domain flow ไม่ใช่ SQL column |
| 7 | NIST | CP-10 (System Recovery) | **ลบออก** | CP-10 = disaster recovery ไม่ใช่ app retry |
| 8 | CWE | CWE-362 → Transaction | **ลบออก** | CWE-362 = code race condition ไม่ใช่ DB transaction |
| 9 | ISO | A.8.9 → Credentials | **A.5.17** (Authentication info) | A.8.9 = Configuration Management |
| 10 | ISO | A.8.11 → SELECT * | **A.8.3** (Info access restriction) | A.8.11 = Data Masking |
| 11 | ISO | A.8.14 → Polly retry | **ลบออก** | A.8.14 = Infrastructure redundancy |

> 💡 **Root Cause (R3→R4):** ASVS 5.0.0 ปรับโครงสร้าง chapter ใหม่ทั้งหมด — R3 ใช้หมายเลข v4.0.3 label เป็น v5.0 โดยไม่ได้ตรวจ [v4.0.3→v5.0.0 mapping](https://github.com/OWASP/ASVS/blob/main/5.0/mappings/)

**✅ Section 3 ผ่านการ audit ตามมาตรฐานสากล** — ครอบคลุม CWE Top 25 (2024), OWASP ASVS 5.0.0, OWASP Top 10 (2021), NIST SP 800-53 Rev 5, ISO/IEC 27001:2022

---

## 4. Service Layer — ชั้น Business Logic

> 📖 **อ้างอิง:**  
> • Microsoft — [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)  
> • Microsoft — [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) (Constructor Injection)  
> • OWASP — [Input Validation Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html) (Defense-in-depth)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 4.1 | ทุก Service **ต้องมี Interface** อยู่ใน `Services/Interfaces/` | ☐ |
| 4.2 | Service ต้องรับ Dependency ผ่าน **Constructor Injection** เท่านั้น — ห้าม `new` dependency เอง | ☐ |
| 4.3 | Service ต้องใช้ **IRepository** ในการเข้าถึง Database — ห้ามใช้ `IDbConnection` หรือ `IDbConnectionFactory` ตรง | ☐ |
| 4.4 | Method ที่มี I/O ต้องเป็น **`async Task<T>`** และรับ **`CancellationToken`** เป็น parameter สุดท้าย — ส่งต่อถึง Repository ทุกครั้ง | ☐ |
| 4.5 | Guard clause (null/empty check) ต้องอยู่ **ต้น Method** ก่อน Business Logic — ใช้ `string.IsNullOrWhiteSpace()` สำหรับ string | ☐ |
| 4.6 | ต้องมี **try-catch** พร้อม Structured Logging ที่มี context (ไม่ใช่ `catch { throw; }` เปล่าๆ) — return `ResultModel` กับ error message ที่ปลอดภัย (ห้าม leak exception detail ให้ client) | ☐ |
| 4.7 | Return Type ต้องเป็น **Response Model/DTO** — ห้าม return Entity ตรง | ☐ |
| 4.8 | ห้ามมี **Hard-coded value** ใน Production Code — ค่า config ต้องมาจาก `IConfiguration` หรือ `IOptions<T>` | ☐ |

> 💡 **หมายเหตุ Guard Clause vs. Primary Validation:**  
> เกณฑ์ 4.5 คือ **defensive check** (ป้องกัน null/crash ใน Service) — ไม่ใช่ primary validation  
> Primary validation อยู่ที่ **Section 15 (FluentValidation ที่ Controller Level)** ซึ่งทำงานก่อนเข้า Service  
> ทั้งสองชั้นทำงานร่วมกัน (**defense-in-depth** ตาม OWASP Input Validation Cheat Sheet)

### ตัวอย่าง Code

**Interface:**
```csharp
// Services/Interfaces/ICustomerService.cs
public interface ICustomerService
{
    Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default);
    Task<ResultModel> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken = default);
}
```

**Implementation:**
```csharp
// Services/CustomerService.cs
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<CustomerResponse>();
        try
        {
            // ── Guard Clause (4.5) — defensive check ──
            if (string.IsNullOrWhiteSpace(customerId))
            {
                result.Status = 400;
                result.IsSuccess = false;
                result.Message = "CustomerId is required.";
                return result;
            }

            // ── Business Logic ──
            var entity = await _customerRepository.GetByIdAsync(
                customerId, cancellationToken);  // ✅ 4.4: ส่ง CT ต่อ

            if (entity is null)
            {
                result.Status = 404;
                result.IsSuccess = false;
                result.Message = "Customer not found.";
                return result;
            }

            // ── Map Entity → Response DTO (4.7) ──
            result.Status = 200;
            result.IsSuccess = true;
            result.Data = MapToResponse(entity);
        }
        catch (Exception ex)
        {
            // ✅ 4.6: Structured logging พร้อม context
            _logger.LogError(ex, "Error in GetCustomerAsync for {CustomerId}", customerId);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";  // ✅ ห้าม leak ex.Message
        }
        return result;
    }

    // ── Private Mapping Method — แยก mapping logic ให้กระชับ ──
    private static CustomerResponse MapToResponse(CustomerEntity entity) => new()
    {
        CustomerId = entity.CustomerId,
        CustomerName = entity.CustomerName,
        Email = entity.Email
    };
}
```

> 💡 **ทำไม Service จัดการ Error เอง (try-catch + ResultModel) แทนที่จะ throw ไปให้ Middleware?**  
> Pattern นี้เลือกใช้เพราะ:  
> 1. **Consistent response** — Client ได้ `{ status, isSuccess, message }` ทุกกรณี ไม่ว่า success หรือ error  
> 2. **Context-aware logging** — Service รู้ว่า operation ไหนล้มเหลว log ได้ละเอียดกว่า global handler  
> 3. **Explicit control** — Developer เห็นชัดว่า error ถูก handle ที่ไหน ไม่ต้องตามหา middleware ที่อยู่คนละไฟล์  
>  
> ถ้าต้องการใช้ **Global Exception Middleware** แทน (throw + middleware catch) ก็สามารถทำได้ — แต่ต้องเลือก **อย่างใดอย่างหนึ่ง** ให้เป็นมาตรฐานเดียว ห้ามผสม

> 💡 **ทำไม Service ไม่ใช้ `ConfigureAwait(false)` แบบ BaseRepository?**  
> `ConfigureAwait(false)` จำเป็นเฉพาะ **library code** (เช่น BaseRepository) เพื่อป้องกัน deadlock  
> Service Layer เป็น **application code** — ASP.NET Core ไม่มี SynchronizationContext จึงไม่จำเป็น  
> (ดู [Microsoft: ConfigureAwait FAQ](https://devblogs.microsoft.com/dotnet/configureawait-faq/))

### Audit Report — Section 4

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices) — verify async, DI, error handling  
> 2. Microsoft — [DI in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) — verify Constructor Injection  
> 3. Cross-check กับ Section 3 (CancellationToken), Section 15 (FluentValidation)  
> 4. Code example source: [MS Docs — Web API Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices) (ดัดแปลง)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **4.1** | Service ต้องมี Interface | DI ทำไม่ได้ → unit test mock ไม่ได้ |
| **4.2** | Constructor Injection เท่านั้น | `new` dependency เอง → coupling แน่น → test ยาก |
| **4.3** | ใช้ IRepository เข้า DB | SQL กระจายทั่ว codebase → maintain ลำบาก |
| **4.4** | async + CancellationToken | thread block รอ DB → server รับ load ไม่ได้ |
| **4.5** | Guard clause ต้น method | NullReferenceException crash → debug ยาก |
| **4.6** | try-catch + structured log | error หลุดถึง client = **security risk** |
| **4.7** | Return DTO ห้าม Entity | client เห็น column ทั้งหมด = **data leak** |
| **4.8** | ห้าม hard-code config | deploy UAT/PROD ต้องแก้ code + build ใหม่ |

✅ Section 4 ผ่านการ audit

---

## 5. Controller Layer — ชั้น API Endpoint

> 📖 **อ้างอิง:**  
> • Microsoft — [Create web APIs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/) (ControllerBase + `[ApiController]`)  
> • Microsoft — [Controller action return types](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types) (IActionResult)  
> • Microsoft — [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices) (async controller actions)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 5.1 | Controller ต้องสืบทอดจาก **`ControllerBase`** | ☐ |
| 5.2 | ต้องมี Attribute: **`[ApiController]`**, **`[Route("api/[controller]/[action]")]`**, **`[Produces("application/json")]`** | ☐ |
| 5.3 | Controller ต้องรับ **IService** ผ่าน Constructor — ห้ามมี Business Logic ใน Controller | ☐ |
| 5.4 | ทุก Action ที่ต้อง Authentication ต้องมี **`[Authorize]`** | ☐ |
| 5.5 | Action Method ต้องระบุ HTTP Verb: **`[HttpGet]`**, **`[HttpPost]`**, **`[HttpPut]`**, **`[HttpDelete]`** | ☐ |
| 5.6 | ต้องมี **HealthCheckController** อย่างน้อย 1 Endpoint ที่ไม่ต้อง Authorize | ☐ |
| 5.7 | Controller ทำหน้าที่แค่ **รับ Request → เรียก Service → ส่ง Response** เท่านั้น | ☐ |
| 5.8 | ทุก async Action ต้องรับ **`CancellationToken`** และ **ส่งต่อให้ Service** ทุกครั้ง | ☐ |

> 💡 **หมายเหตุ Route Pattern (เกณฑ์ 5.2):**  
> องค์กรใช้ `[Route("api/[controller]/[action]")]` (Action-based routing) เพื่อให้ URL ชัดเจน สื่อสารง่ายกับทีม  
> ซึ่ง**แตกต่าง**จาก §13 RESTful pattern (`/api/v{ver}/{resource}`) — §13 เป็นแนวปฏิบัติสำหรับ API ใหม่ที่ออกแบบตั้งแต่ต้น  
> หากโปรเจคเริ่มใหม่และต้องการ RESTful ให้ใช้ `[Route("api/v1/[controller]")]` + HTTP Method routing แทน

> 💡 **ทำไม Controller ต้อง "บาง" (Thin Controller)?**  
> ถ้า Controller มี business logic → เกิดปัญหา:  
> 1. **Test ยาก** — ต้อง mock HttpContext + pipeline ทั้งชุดเพื่อ test logic  
> 2. **Reuse ไม่ได้** — logic ผูกกับ HTTP layer จะเรียกจาก background job/message queue ไม่ได้  
> 3. **Responsibility ปน** — ฝ่าฝืน Single Responsibility Principle → แก้ bug ยาก  
>  
> Controller ควรมีแค่ **1–3 บรรทัด** ต่อ action: รับ → เรียก → ส่ง

> 💡 **CancellationToken ใน Controller (เกณฑ์ 5.8):**  
> ASP.NET Core **bind อัตโนมัติ** จาก `HttpContext.RequestAborted` เมื่อประกาศ `CancellationToken` เป็น parameter  
> ถ้าไม่รับ CT → Service ได้ `default(CancellationToken)` → client disconnect แล้ว query ยังวิ่งอยู่เปลืองทรัพยากร  
> CT chain ต้องครบ: **Controller (§5) → Service (§4) → Repository (§3)**

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
    public async Task<IActionResult> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken)  // ✅ 5.8: ASP.NET Core bind CT อัตโนมัติ
    {
        var result = await _customerService.GetCustomerAsync(
            customerId, cancellationToken);  // ✅ ส่ง CT ต่อให้ Service
        return StatusCode(result.Status, result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateCustomerAsync(
            request, cancellationToken);
        return StatusCode(result.Status, result);
    }
}
```

> 💡 **ทำไมใช้ `IActionResult` + `StatusCode()` แทน return `ResultModel` ตรง?**  
> • `return StatusCode(result.Status, result)` — ทำให้ **HTTP Status Code** ตรงกับ business result (400, 404, 500)  
> • ถ้า return `ResultModel` ตรง → HTTP status จะเป็น **200 ทุกครั้ง** แม้ business logic จะ fail — Client/API Gateway อ่าน status ผิด  
> • Middleware, logging, monitoring tools ทำงานถูกต้องเมื่อ HTTP status ตรงกับ reality

### Audit Report — ตรวจสอบความถูกต้อง Section 5

> 📅 **วันที่ตรวจสอบ:** 9 มีนาคม 2026  
> 🔍 **วิธีการ:**  
> 1. Microsoft — [Create web APIs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/) — verify ControllerBase + `[ApiController]`  
> 2. Microsoft — [Controller action return types](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types) — verify IActionResult  
> 3. Microsoft — [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices) — verify async actions  
> 4. Cross-check กับ Section 3 (CT chain), Section 4 (Service interface signature)  
> 5. Code example source: [MS Docs — Web API tutorial](https://learn.microsoft.com/en-us/aspnet/core/web-api/) (ดัดแปลง)

#### เกณฑ์มีอะไร ทำไมต้องปรับใช้

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **5.1** | สืบทอด `ControllerBase` | ใช้ `Controller` → ได้ View support ที่ไม่จำเป็นสำหรับ API → เปลืองหน่วยความจำ |
| **5.2** | Attribute ครบ 3 ตัว | ไม่มี `[ApiController]` → ไม่ได้ auto model validation · ไม่มี Route → endpoint ไม่ work |
| **5.3** | รับ IService ผ่าน Constructor | inject ไม่ได้ → test mock ไม่ได้ → coupling แน่น |
| **5.4** | `[Authorize]` | endpoint เปิดให้คนนอกเรียกได้ = **security hole** |
| **5.5** | HTTP Verb attribute | Swagger สร้าง doc ไม่ถูก · routing ผิด → 405 Method Not Allowed |
| **5.6** | HealthCheck endpoint | K8s/Docker health probe ไม่มีทาง check → pod ถูก restart ไม่มีเหตุผล |
| **5.7** | Controller บาง | Business logic ใน Controller → test ยาก + reuse ไม่ได้ |
| **5.8** | CancellationToken | ไม่ส่ง CT → client disconnect แล้ว query ยังวิ่ง → เปลืองทรัพยากร DB/server |

**✅ Section 5 ผ่านการ audit**

---

## 6. Dependency Injection — การลงทะเบียน DI

> 📖 **อ้างอิง:**  
> • Microsoft — [Dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines) (Service lifetimes, Captive dependency anti-pattern)  
> • Microsoft — [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 6.1 | ลงทะเบียน DI ใน **`Program.cs`** (Minimal Hosting — .NET 10 standard) | ☐ |
| 6.2 | จัดกลุ่มลำดับ: ① Singleton (Factory) → ② Scoped (Repository) → ③ Scoped (Service) → ④ Infrastructure | ☐ |
| 6.3 | `IDbConnectionFactory` ต้องใช้ **`AddSingleton`** — stateless, thread-safe, ไม่ hold connection | ☐ |
| 6.4 | Repository / Service ต้องใช้ **`AddScoped`** — 1 Instance ต่อ 1 HTTP Request | ☐ |
| 6.5 | ห้าม inject **Scoped เข้า Singleton** (captive dependency) — ASP.NET Core จะ throw `InvalidOperationException` | ☐ |
| 6.6 | ทุก Interface → Implementation ต้องลงทะเบียนครบ — ห้ามมี `new` dependency ในโค้ด | ☐ |

> 💡 **Lifetime เลือกผิด → เกิดอะไร?**  
> | Lifetime | ใช้กับ | ถ้าใช้ผิด → |
> |:---------|:------|:-----------|
> | **Singleton** | Stateless factory (DbConnectionFactory) | hold state → data leak ข้าม request |
> | **Scoped** | Repository, Service | ถ้าเป็น Singleton → connection share ข้าม request = **race condition** |
> | **Transient** | Lightweight utility | ถ้า implement IDisposable → memory leak |

### ตัวอย่าง Code

```csharp
// Program.cs — .NET 10 Minimal Hosting

// ─── ① Connection Factory (Singleton — Stateless, Thread-safe) ───
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ─── ② Repository Mapping (Scoped — 1 per Request) ───
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// ─── ③ Service Mapping (Scoped — 1 per Request) ───
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IMailService, MailService>();

// ─── ④ Infrastructure ───
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(LogFilter));
    options.Filters.Add(typeof(AvailableFilter));
});
```

### Audit Report — ตรวจสอบความถูกต้อง Section 6

> 📅 **วันที่ตรวจสอบ:** 9 มีนาคม 2026  
> 🔍 **วิธีการ:**  
> 1. Microsoft DI guidelines — verify service lifetimes, captive dependency anti-pattern  
> 2. Microsoft ASP.NET Core DI docs — verify Singleton/Scoped/Transient behavior  
> 4. Cross-check กับ Section 3 (Repository = Scoped), Section 4 (Service = Scoped via Constructor Injection)

#### เกณฑ์มีอะไร ทำไมต้องปรับใช้

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **6.1** | ลงทะเบียนใน Program.cs | กระจายหลายที่ → developer ไม่รู้ว่า service ไหนถูก register |
| **6.2** | จัดกลุ่มตามลำดับ | ไม่ affect runtime แต่ code อ่านยาก → maintain ลำบาก |
| **6.3** | DbConnectionFactory = Singleton | ถ้า Scoped → สร้างใหม่ทุก request เปลืองหน่วยความจำ (factory ไม่มี state) |
| **6.4** | Repository/Service = Scoped | ถ้า Singleton → connection share ข้าม request = **race condition + data leak** |
| **6.5** | ห้าม Scoped เข้า Singleton | Captive dependency → Scoped service อยู่นานกว่าที่ควร → **stale data ข้าม request** |
| **6.6** | ลงทะเบียนครบทุก Interface | ไม่ register → runtime throw `InvalidOperationException` ตอน resolve |

**✅ Section 6 ผ่านการ audit**


---



## 7. Authentication & Authorization

> 📖 **อ้างอิง:**  
> • Microsoft — [Overview of ASP.NET Core authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)  
> • Microsoft — [JWT Bearer authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)  
> • Microsoft — [Middleware order](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/#middleware-order)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 7.1 | ต้องใช้ **JWT Bearer Token** เป็น Authentication Scheme | ☐ |
| 7.2 | JWT Configuration ต้องอยู่ใน `appsettings.json` (Issuer, Key) — ห้าม Hard-code *(ดู §9.4)* | ☐ |
| 7.3 | ต้องตั้งค่า `ValidateIssuer`, `ValidateAudience`, `ValidateLifetime`, `ValidateIssuerSigningKey` = **true** ทั้งหมด | ☐ |
| 7.4 | ต้องตั้ง `RequireExpirationTime = true` และ `ClockSkew = TimeSpan.Zero` | ☐ |
| 7.5 | Middleware ต้องเรียงลำดับ: `UseAuthentication()` → `UseAuthorization()` | ☐ |
| 7.6 | ทุก Endpoint ที่ต้องการ Auth ต้องมี `[Authorize]` — HealthCheck ไม่ต้อง *(ดู §5.4)* | ☐ |

> 💡 **ClockSkew = Zero:** MS default = 5 นาที — เราเลือก Zero เพื่อ security (token หมดอายุ = ใช้ไม่ได้ทันที) ถ้ามีปัญหา clock sync ให้ปรับเป็น `TimeSpan.FromSeconds(30)`

### Audit Report — Section 7

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [JWT Bearer authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn) — verify TokenValidationParameters  
> 2. Microsoft — [Middleware order](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/#middleware-order)  
> 3. Cross-check กับ Section 5 (5.4 Authorize), Section 9 (config)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **7.1** | JWT Bearer Token | endpoint เปิดรับทุกคน = **security hole** |
| **7.2** | Config ใน appsettings | hard-code key → secret leak ใน source |
| **7.3** | Validate 4 params = true | token ปลอมจาก issuer อื่นผ่านได้ |
| **7.4** | ClockSkew = Zero | token หมดอายุแล้วยังใช้ได้อีก 5 นาที |
| **7.5** | Middleware order | สลับลำดับ → authorize fail ทุก request |
| **7.6** | `[Authorize]` | endpoint เปิด public = unauthorized access |

✅ Section 7 ผ่านการ audit

---

## 8. Error Handling & Logging

> 📖 **อ้างอิง:**  
> • Microsoft — [Handle errors in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)  
> • Microsoft — [Logging in .NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 8.1 | ทุก Service Method ต้องมี **try-catch** ครอบ Business Logic *(ดู §4.6)* | ☐ |
| 8.2 | catch block ต้อง **Log Error** พร้อม Context (Method Name, Parameters) | ☐ |
| 8.3 | ห้ามมี `catch (Exception) { throw; }` เปล่าๆ — ต้อง Log ก่อน throw หรือ return Error | ☐ |
| 8.4 | ใช้ `ILogger<T>` ที่ Inject ผ่าน Constructor — ห้ามใช้ `Console.WriteLine` | ☐ |
| 8.5 | Error Response ต้องมีโครงสร้างเดียวกัน: `Status`, `IsSuccess`, `Message` | ☐ |
| 8.6 | ห้าม return Exception Message ตรงให้ Client ใน Production (เช่น Stack Trace) | ☐ |
| 8.7 | ใช้ **LogFilter** (Action Filter) สำหรับ Log ทุก Request/Response อัตโนมัติ | ☐ |

> 💡 **MS Alternative:** .NET 8+ มี `IExceptionHandler` + `AddProblemDetails()` (RFC 7807) สำหรับ centralized error handling — โปรเจคนี้เลือก try-catch + ResultModel เพื่อ consistent response (ดู §4 note)

### ตัวอย่าง Error Response Structure

> Code example source: [MS Docs — Handle errors](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling) (ดัดแปลง)

```csharp
public class ResultModel
{
    public int Status { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

public class ResultModel<T> : ResultModel
{
    public T? Data { get; set; }
}
```

### Audit Report — Section 8

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Handle errors](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling) — verify IExceptionHandler, ProblemDetails  
> 2. Microsoft — [Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) — verify ILogger  
> 3. Cross-check กับ Section 4 (4.6 try-catch + ResultModel)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **8.1** | try-catch ครอบ business logic | exception ไม่ถูก handle → 500 ไม่มี context *(ดู §4.6)* |
| **8.2** | Log Error + Context | debug ไม่ได้ — ไม่รู้ error เกิดที่ไหน |
| **8.3** | ห้าม catch+throw เปล่า | error ไม่ถูก log → สูญเสีย evidence |
| **8.4** | ILogger via DI | Console.WriteLine → production ไม่เห็น output |
| **8.5** | Uniform error response | response คนละ format → client parse ยาก |
| **8.6** | ห้าม return stack trace | leak internal structure = **security risk** |
| **8.7** | LogFilter auto log | ไม่มี request/response log → debug production ลำบาก |

✅ Section 8 ผ่านการ audit

---

## 9. Configuration Management

> 📖 **อ้างอิง:**  
> • Microsoft — [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)  
> • Microsoft — [Options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)  
> • Microsoft — [Safe storage of app secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 9.1 | ต้องมี `appsettings.json` เป็น Base Configuration | ☐ |
| 9.2 | ต้องมี **อย่างน้อย 3 Environment**: Development, UAT, Production | ☐ |
| 9.3 | Connection String ต้องอยู่ใน `appsettings.{Environment}.json` — ห้าม Hard-code ในโค้ด | ☐ |
| 9.4 | Secret/Password ห้ามอยู่ใน Source Code หรือ appsettings ที่ Commit ขึ้น Repository | ☐ |
| 9.5 | อ่าน Configuration ผ่าน `IConfiguration` หรือ **`IOptions<T>`** (MS recommended) ที่ Inject เข้ามา | ☐ |
| 9.6 | ข้อความ Message Response ต้องอยู่ใน Configuration — ห้าม Hard-code | ☐ |

> 💡 **IOptions\<T\> vs IConfiguration:** MS แนะนำ [Options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) สำหรับ strongly-typed config — type-safe + validate ได้  
> 💡 **Secret Management:** Development ใช้ [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) / Production ใช้ Azure Key Vault หรือ Environment Variables

### ไฟล์ Configuration ที่ต้องมี

```
appsettings.json                 ← Base (ค่าที่ใช้ร่วมกัน)
appsettings.Development.json     ← Dev Server
appsettings.UAT.json             ← UAT Server
appsettings.Production.json      ← Production Server
appsettings.Localhost.json       ← Local Development (optional)
```

### Audit Report — Section 9

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Configuration](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) — verify appsettings  
> 2. Microsoft — [Options pattern](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) — verify IOptions  
> 3. Cross-check กับ Section 7 (JWT config), Section 4 (IConfiguration)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **9.1** | appsettings.json base | ตั้งค่าซ้ำทุก environment |
| **9.2** | 3 Environment ขั้นต่ำ | dev ต่อ prod DB โดยไม่รู้ตัว = **data corruption** |
| **9.3** | Connection String ใน env file | เปลี่ยน server ต้อง rebuild |
| **9.4** | Secret ห้ามอยู่ใน source | commit secret = **credential leak** |
| **9.5** | IConfiguration / IOptions via DI | hard-code ค่า config → maintain ลำบาก |
| **9.6** | Message ใน config | เปลี่ยนข้อความต้อง rebuild + deploy ใหม่ |

✅ Section 9 ผ่านการ audit

---

## 10. API Documentation (OpenAPI)

> 📖 **อ้างอิง:**  
> • Microsoft — [OpenAPI support in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview)  
> • Microsoft — [Generate OpenAPI documents](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi)  
> ⚠️ **Swashbuckle ถูกลบออกจาก default template ตั้งแต่ .NET 9** — ใช้ `Microsoft.AspNetCore.OpenApi` แทน

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 10.1 | ต้องเปิดใช้ **OpenAPI** (`AddOpenApi()` + `MapOpenApi()`) — ~~Swashbuckle deprecated ตั้งแต่ .NET 9~~ | ☐ |
| 10.2 | OpenAPI Doc ต้องมี Title และ Version | ☐ |
| 10.3 | ต้องกำหนด **Security Definition** สำหรับ Bearer Token (ใช้ OpenAPI transformer) | ☐ |
| 10.4 | ต้องเปิดใช้ **XML Comments** สำหรับ API documentation | ☐ |
| 10.5 | ทุก Public Action ควรมี `/// <summary>` XML Comment อธิบาย | ☐ |

### Audit Report — Section 10

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [OpenAPI support](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview) — verify Swashbuckle removal in .NET 9+  
> 2. Microsoft — [Generate OpenAPI](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi) — verify AddOpenApi + MapOpenApi

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **10.1** | OpenAPI (built-in .NET 10) | ไม่มี API doc → developer ต้องอ่าน source code |
| **10.2** | Title + Version | metadata ไม่ครบ → versioning ไม่ชัด |
| **10.3** | Security Definition | Swagger UI ไม่มีปุ่ม Authorize → test ลำบาก |
| **10.4** | XML Comments | doc ไม่มีคำอธิบาย → developer อ่านไม่เข้าใจ |
| **10.5** | `/// <summary>` | doc แสดงแต่ชื่อ method ไม่มีรายละเอียด |

✅ Section 10 ผ่านการ audit (แก้ Swashbuckle → OpenAPI แล้ว)

---

## 11. Code Quality & Review Checklist

> 📖 **อ้างอิง:**  
> • Microsoft — [Code analysis in .NET](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 11.1 | ห้ามมี **unused using statements** | ☐ |
| 11.2 | ห้ามมี **commented-out code** ที่ไม่ได้ใช้ (ลบออก ไม่ใช่ comment) | ☐ |
| 11.3 | ห้ามมี `TODO` / `HACK` / `FIXME` comment ค้างใน Production Branch | ☐ |
| 11.4 | ทุก Public Method ใน Service Interface ต้องมี **XML Comment** | ☐ |
| 11.5 | ไม่มีการ `new` Service, Repository หรือ Connection ในโค้ด — ต้องใช้ DI ทั้งหมด *(ดู §4.2, §6.6)* | ☐ |
| 11.6 | ไม่มี Magic Number / Magic String — ใช้ Constant หรือ Configuration *(ดู §4.8)* | ☐ |
| 11.7 | ไม่มี Business Logic ใน Controller *(ดู §5.3, §5.7)* | ☐ |
| 11.8 | ไม่มี Database Access ใน Controller (ต้องผ่าน Service) *(ดู §5.3)* | ☐ |
| 11.9 | ไม่ return Entity ออกจาก Service — ต้อง Map เป็น Model/DTO ก่อน *(ดู §4.7)* | ☐ |
| 11.10 | Branch ที่ Merge เข้า Master/Main ต้องผ่าน Code Review อย่างน้อย 1 คน | ☐ |

### Audit Report — Section 11

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Code analysis](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview) — verify code quality rules  
> 2. Cross-check กับ §4 (4.2, 4.7, 4.8), §5 (5.3, 5.7), §6 (6.6) — ตรวจ duplication

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **11.1** | ลบ unused using | code สกปรก → compile warning |
| **11.2** | ลบ commented-out code | reviewer สับสน → maintain ยาก |
| **11.3** | ลบ TODO/HACK/FIXME | debt ค้าง production → อาจมี bug |
| **11.4** | XML Comment public method | developer อื่นไม่รู้ว่า method ทำอะไร |
| **11.5–11.9** | **Quick Review Checklist** | cross-ref §4–§6 เป็น checklist สำหรับ reviewer *(ดู section ต้นทาง)* |
| **11.10** | Code Review ก่อน merge | ไม่ review → bug เข้า main → rollback ลำบาก |

✅ Section 11 ผ่านการ audit

---

## 12. Testing Standards — มาตรฐานการทดสอบ

> **อ้างอิง:** Microsoft [Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices), [xUnit.net](https://xunit.net/)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 12.1 | ทุก Service Class ต้องมี **Unit Test** — อย่างน้อย 1 Test ต่อ 1 Public Method | ☐ |
| 12.2 | Test Framework: ใช้ **xUnit** (team standard) — ห้ามใช้ MSTest/NUnit ในโปรเจคใหม่ | ☐ |
| 12.3 | Mocking Framework: ใช้ **Moq** หรือ **NSubstitute** — ห้าม Mock ด้วยมือ | ☐ |
| 12.4 | Assertion Library: ใช้ **FluentAssertions** หรือ xUnit built-in Assert | ☐ |
| 12.5 | Test Project ต้องอยู่ใน Solution ภายใต้โฟลเดอร์ `tests/` — แยกจาก `src/` | ☐ |
| 12.6 | Test Project Naming: `{ProjectName}.Tests` (Unit), `{ProjectName}.IntegrationTests` (Integration) | ☐ |
| 12.7 | Test Class Naming: **`{ClassName}Tests`** — เช่น `CustomerServiceTests` | ☐ |
| 12.8 | Test Method Naming: **`{Method}_{Scenario}_{Expected}`** — เช่น `GetCustomerAsync_WhenNotFound_Returns404` | ☐ |
| 12.9 | ทุก Test ต้องใช้ **AAA Pattern**: Arrange → Act → Assert (แยก section ชัดเจน) | ☐ |
| 12.10 | ห้ามเข้าถึง **Database จริง** ใน Unit Test — ต้องใช้ Mock Repository | ☐ |
| 12.11 | **Code Coverage** ต้อง ≥ **80%** สำหรับ Service Layer, ≥ **60%** Overall | ☐ |
| 12.12 | Test ต้องรัน **อัตโนมัติ** ใน CI Pipeline ก่อน Merge — ห้าม Merge ถ้า Test Fail | ☐ |
| 12.13 | ห้าม **Skip/Ignore** Test โดยไม่มีเหตุผล — ต้องมี `// TODO:` ระบุวันที่จะแก้ | ☐ |
| 12.14 | ทุก **Bug Fix** ต้องมี Regression Test — เขียน Test ที่ reproduce bug ก่อนแก้ | ☐ |
| 12.15 | Integration Test สำหรับ DataAccess Layer ต้องใช้ **Testcontainers** หรือ In-Memory DB | ☐ |

### โครงสร้าง Test Project

```
{SolutionName}/
├── src/
│   └── {ProjectName}/
│       ├── Controllers/
│       ├── Services/
│       └── DataAccess/
└── tests/
    ├── {ProjectName}.Tests/                    ← Unit Tests
    │   ├── Services/
    │   │   └── CustomerServiceTests.cs
    │   └── Usings.cs
    └── {ProjectName}.IntegrationTests/         ← Integration Tests
        └── Repositories/
            └── CustomerRepositoryTests.cs
```

### ตัวอย่าง Unit Test

```csharp
// tests/{ProjectName}.Tests/Services/CustomerServiceTests.cs
public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepo;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly CustomerService _sut; // System Under Test

    public CustomerServiceTests()
    {
        _mockRepo = new Mock<ICustomerRepository>();
        _mockLogger = new Mock<ILogger<CustomerService>>();
        _sut = new CustomerService(
            _mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCustomerAsync_WhenNotFound_Returns404()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync("CUST001", default))
            .ReturnsAsync((CustomerEntity?)null);

        // Act
        var result = await _sut.GetCustomerAsync("CUST001");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Equal("Customer not found.", result.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetCustomerAsync_WhenInvalidId_Returns400(string? customerId)
    {
        // Act
        var result = await _sut.GetCustomerAsync(customerId!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }
}
```


### Audit Report — Section 12

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) — verify xUnit, AAA, naming  
> 2. [xUnit.net Documentation](https://xunit.net/) — verify Fact/Theory, Assert  
> 3. Cross-check กับ Section 1 (1.7 tests/ folder), Section 4 (Service testability)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **12.1** | Unit Test ทุก Service | ไม่มี test → refactor แล้วพัง ไม่มีใครรู้ |
| **12.2** | xUnit (team standard) | หลาย framework → config ยุ่ง, ทีมต้องเรียนรู้หลายตัว |
| **12.3** | Moq/NSubstitute | mock ด้วยมือ → boilerplate เยอะ, ผิดง่าย |
| **12.4** | FluentAssertions | Assert.Equal เฉยๆ อ่านยาก — `.Should().Be()` ชัดกว่า |
| **12.5–12.6** | tests/ แยกจาก src/ | test compile เข้า production → binary ใหญ่ขึ้น |
| **12.7–12.8** | Naming convention | test ชื่อไม่สื่อ → ดู test fail แล้วไม่รู้ว่าอะไรพัง |
| **12.9** | AAA Pattern | test อ่านยาก → maintain ลำบาก |
| **12.10** | ห้าม DB จริงใน Unit Test | test ช้า + flaky + ต้องมี DB server |
| **12.11** | Coverage ≥ 80% Service | coverage ต่ำ → ไม่มั่นใจว่าครอบคลุม edge case |
| **12.12** | CI auto-run test | merge code ไม่ผ่าน test → bug เข้า main |
| **12.13** | ห้าม Skip test | skip ค้าง → debt สะสม → ลืมแก้ |
| **12.14** | Regression test ทุก bug fix | fix bug แล้วกลับมาพังซ้ำ (regression) |
| **12.15** | Testcontainers for Integration | ใช้ DB จริง → test flaky + ต้อง setup |

✅ Section 12 ผ่านการ audit

---

## 13. API Design & Versioning — การออกแบบ API

> **อ้างอิง:** [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines), Microsoft [Web API design](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 13.1 | URL Pattern: **`/api/v{version}/{resource}`** — เช่น `/api/v1/customers` | ☐ |
| 13.2 | ใช้ **HTTP Method** ตามความหมาย: `GET` (อ่าน), `POST` (สร้าง), `PUT` (แก้ไขทั้งหมด), `PATCH` (แก้ไขบางส่วน), `DELETE` (ลบ) | ☐ |
| 13.3 | **HTTP Status Code** ต้องถูกต้องตาม RFC 9110: | ☐ |
| | `200` OK / `201` Created / `204` No Content | |
| | `400` Bad Request / `401` Unauthorized / `403` Forbidden | |
| | `404` Not Found / `409` Conflict / `422` Unprocessable Entity | |
| | `429` Too Many Requests / `500` Internal Server Error | |
| 13.4 | **Collection Endpoint** ต้องรองรับ Pagination: `pageNumber`, `pageSize` (default: 20, max: 100) | ☐ |
| 13.5 | **Pagination Response** ต้องมีโครงสร้าง: `{ data: [...], pagination: { page, pageSize, totalCount, totalPages } }` | ☐ |
| 13.6 | ต้องใช้ **API Versioning** (NuGet: `Asp.Versioning.Mvc`) — URL Segment เป็นค่าเริ่มต้น | ☐ |
| 13.7 | ห้ามทำ **Breaking Change** บน Version เดิม — ต้อง Deprecate แล้วสร้าง Version ใหม่ | ☐ |
| 13.8 | Request/Response Body ต้องเป็น **camelCase JSON** (`System.Text.Json` default ใน .NET 10) | ☐ |
| 13.9 | **Date/Time** ต้องเป็น ISO 8601 UTC format — `2026-03-05T10:30:00Z` | ☐ |
| 13.10 | ห้ามมี **Verb ใน URL** — ใช้ HTTP Method แทน (ยกเว้น Action ที่ไม่ใช่ CRUD เช่น `/api/v1/orders/{id}/cancel`) | ☐ |
| 13.11 | ทุก Endpoint ต้องมี **strongly-typed Request/Response DTO** — ห้ามรับ `string`/`int` เดี่ยวเป็น body | ☐ |
| 13.12 | **POST** ที่สร้างทรัพยากร ควรรองรับ **Idempotency Key** (Header: `Idempotency-Key`) สำหรับ FinTech/Payment | ☐ |

### ตัวอย่าง Pagination Response

```csharp
public class PagedResponse<T>
{
    public IReadOnlyList<T> Data { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
```


### Audit Report — Section 13

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines) — verify URL pattern, HTTP methods, status codes  
> 2. Microsoft — [Web API design](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design) — verify pagination, versioning  
> 3. Cross-check กับ Section 5 (Controller), Section 15 (Input Validation)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **13.1** | URL Pattern `/api/v{ver}/{resource}` | URL ไม่สม่ำเสมอ → client integrate ยาก |
| **13.2** | HTTP Method ตามความหมาย | POST ใช้ทุกอย่าง → RESTful ไม่เป็น |
| **13.3** | HTTP Status Code ตาม RFC | return 200 ทุกกรณี → client แยก success/fail ไม่ได้ |
| **13.4–13.5** | Pagination | return ทั้งหมด → response ใหญ่ → timeout |
| **13.6** | API Versioning | breaking change → client เดิมพัง |
| **13.7** | ห้าม breaking change | client ที่ deploy แล้วใช้ไม่ได้ |
| **13.8** | camelCase JSON | key ไม่สม่ำเสมอ → client parse ผิด |
| **13.9** | ISO 8601 UTC | timezone ปนกัน → data ผิดเวลา |
| **13.10** | ห้าม verb ใน URL | URL ไม่เป็น RESTful standard |
| **13.11** | Strongly-typed DTO | รับ raw type → ไม่มี validation + doc ไม่สมบูรณ์ |
| **13.12** | Idempotency Key | duplicate request → สร้างข้อมูลซ้ำ (FinTech critical) |

✅ Section 13 ผ่านการ audit

---

## 14. Code Formatting & Style Rules — กฎการจัดรูปแบบโค้ด

> **อ้างอิง:** [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions), [EditorConfig](https://editorconfig.org/)
> **หลักการ:** Formatting ที่สม่ำเสมอ = ลด Cognitive Load = Code Review เร็วขึ้น

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 14.1 | ต้องมีไฟล์ **`.editorconfig`** ใน Root ของ Solution — enforce formatting rules อัตโนมัติ | ☐ |
| 14.2 | **Indentation:** 4 spaces — ห้ามใช้ Tab (ตาม Microsoft .NET default) | ☐ |
| 14.3 | **Column Limit:** 120 characters (soft), 150 characters (hard) — ใช้ line wrapping ถ้าเกิน | ☐ |
| 14.4 | **Brace Style:** Allman Style (เปิด `{` บรรทัดใหม่) — ตาม Microsoft C# default | ☐ |
| 14.5 | ต้องมี **blank line** ระหว่าง methods, ระหว่าง logical blocks, ระหว่าง member groups | ☐ |
| 14.6 | ห้ามมี **trailing whitespace** และ ไฟล์ต้องลงท้ายด้วย **newline** (`\n`) | ☐ |
| 14.7 | ใช้ **`var`** เมื่อ type ชัดเจนจาก right-hand side — ห้ามใช้เมื่อ type ไม่ชัดเจน (เช่น `var x = GetResult()`) | ☐ |
| 14.8 | **Braces** ต้องใช้เสมอ แม้ `if`/`for`/`while` มี statement เดียว — ตาม Microsoft C# Conventions | ☐ |
| 14.9 | ใช้ **expression-bodied members** (`=>`) สำหรับ single-line methods/properties | ☐ |
| 14.10 | ต้อง Run **`dotnet format`** หรือ IDE auto-format ก่อนทุก Commit — ห้าม Commit code ที่ไม่ผ่าน format | ☐ |

### .editorconfig ขั้นต่ำที่ต้องมี

```ini
# .editorconfig — วางที่ Root ของ Solution
root = true

[*]
indent_style = space
indent_size = 4
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.cs]
# Namespace
csharp_style_namespace_declarations = file_scoped:warning

# var preferences
csharp_style_var_for_built_in_types = false:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
csharp_style_var_elsewhere = false:suggestion

# Braces
csharp_prefer_braces = true:warning

# Expression-bodied members
csharp_style_expression_bodied_methods = when_on_single_line:suggestion
csharp_style_expression_bodied_properties = true:suggestion
csharp_style_expression_bodied_constructors = false:suggestion

# using directives
dotnet_sort_system_directives_first = true
csharp_using_directive_placement = outside_namespace:warning

# New line preferences (Allman style)
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true

# Naming conventions
dotnet_naming_rule.private_fields_should_be_camel_case.severity = warning
dotnet_naming_rule.private_fields_should_be_camel_case.symbols = private_fields
dotnet_naming_rule.private_fields_should_be_camel_case.style = camel_case_underscore
dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private
dotnet_naming_style.camel_case_underscore.required_prefix = _
dotnet_naming_style.camel_case_underscore.capitalization = camel_case
```


### Audit Report — Section 14

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) — verify Allman braces, var usage  
> 2. [EditorConfig](https://editorconfig.org/) — verify settings  
> 3. Cross-check กับ Section 2 (Naming), Section 11 (Code Quality)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **14.1** | .editorconfig | ไม่มี auto-enforce → แต่ละคน format คนละแบบ |
| **14.2** | 4 spaces | tab vs space ปน → git diff เยอะเกินจริง |
| **14.3** | Column limit 120/150 | บรรทัดยาวเกิน → อ่านต้อง scroll → review ยาก |
| **14.4** | Allman brace style | ปน K&R → code ไม่สม่ำเสมอ |
| **14.5** | Blank line ระหว่าง method | อ่านยาก → ไม่เห็นขอบเขตของแต่ละ method |
| **14.6** | ห้าม trailing whitespace | git diff มี noise → review ลำบาก |
| **14.7** | var เมื่อ type ชัด | var x = GetResult() → อ่าน code แล้วไม่รู้ type |
| **14.8** | Braces เสมอ | if ไม่มี {} → เพิ่ม statement แล้วลืมใส่ → bug |
| **14.9** | Expression-bodied `=>` | single-line method เขียนเต็มจำนวน → verbose |
| **14.10** | dotnet format ก่อน commit | commit code ที่ format ไม่ตรง → PR full of noise |

✅ Section 14 ผ่านการ audit

---

## 15. Input Validation — การตรวจสอบข้อมูลขาเข้า

> **อ้างอิง:** [OWASP Input Validation Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html), [FluentValidation](https://docs.fluentvalidation.net/), Microsoft [Model Validation](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation)  
> **หลักการ:** "Never trust input" — ทุก input จาก Client เป็น Untrusted จนกว่าจะ Validate

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 15.1 | ทุก Request Model ต้องมี **Validation** — ใช้ **FluentValidation** (Primary) หรือ **DataAnnotations** (Simple cases) | ☐ |
| 15.2 | Validation ต้องทำงานที่ **Controller Level** (Model Binding + Filter) — ก่อนเข้า Service Layer | ☐ |
| 15.3 | ต้อง Validate ครบ: **Required**, **String Length** (min/max), **Numeric Range**, **Email Format**, **Date Range**, **Regex Pattern** | ☐ |
| 15.4 | Custom Validation ต้องสร้างเป็น **Validator Class แยก** — ห้ามเขียน if-else ยาวๆ ใน Service | ☐ |
| 15.5 | **Validation Error Response** ต้องมีโครงสร้างมาตรฐาน: `{ status: 400, errors: [{ field, message }] }` | ☐ |
| 15.6 | **String Input** ต้อง `Trim()` whitespace ก่อนใช้งาน | ☐ |
| 15.7 | ห้ามรับ **`dynamic`** หรือ **`object`** เป็น parameter — ต้องมี strongly-typed Model เสมอ | ☐ |
| 15.8 | **Pagination Parameters** ต้อง validate: `pageSize > 0 && pageSize ≤ 100`, `pageNumber ≥ 1` | ☐ |
| 15.9 | **Enum Parameters** ต้อง validate ว่าเป็นค่าที่ valid — ป้องกัน arbitrary integer | ☐ |
| 15.10 | **File Upload** ต้อง validate: extension whitelist (`.pdf`, `.jpg`, `.png`), content-type match, size limit | ☐ |

### ตัวอย่าง FluentValidation

```csharp
// Validators/CreateCustomerRequestValidator.cs
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("CustomerName is required.")
            .MaximumLength(200).WithMessage("CustomerName must not exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{9,10}$").WithMessage("PhoneNumber must be 9-10 digits.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}

// Program.cs — Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
```


### Audit Report — Section 15

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Model Validation in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation) — verify DataAnnotations, FluentValidation  
> 2. [OWASP Input Validation](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html) — verify validation rules  
> 3. Cross-check กับ Section 4 (Guard Clause), Section 13 (DTO)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **15.1** | FluentValidation / DataAnnotations | input ไม่ validate → SQL Injection, XSS, data corruption |
| **15.2** | Validate ที่ Controller Level | validate ใน Service → logic ปน validation |
| **15.3** | Validate ครบ (length, range, etc.) | ข้อมูลเกิน limit → DB error / buffer overflow |
| **15.4** | Validator Class แยก | if-else ยาวใน Service → อ่าน/test ยาก |
| **15.5** | Validation Error Response มาตรฐาน | error format คนละแบบ → client parse ไม่ได้ |
| **15.6** | Trim() whitespace | " admin " ≠ "admin" → duplicate data |
| **15.7** | ห้าม dynamic/object | ไม่มี type safety → runtime error |
| **15.8** | Validate pagination | pageSize = 999999 → query ใหญ่ → server crash |
| **15.9** | Validate enum | enum = 999 → undefined behavior |
| **15.10** | Validate file upload | upload .exe → **remote code execution** |

✅ Section 15 ผ่านการ audit

---

## 16. Observability & Monitoring — การเฝ้าระวังระบบ

> **อ้างอิง:** [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/dotnet/), Microsoft [Logging Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/), [Structured Logging](https://messagetemplates.org/)  
> **หลักการ:** "You can't fix what you can't see" — ระบบ Production ต้อง Observable ตลอดเวลา

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 16.1 | **Structured Logging:** ทุก Log ต้องมี `Timestamp`, `CorrelationId`, `Level`, `Message`, `Properties` | ☐ |
| | ห้ามใช้ String Concatenation ใน Log: `_logger.LogError("Error " + id)` ❌ → `_logger.LogError("Error for {CustomerId}", id)` ✅ | |
| 16.2 | ทุก HTTP Request ต้องมี **CorrelationId** — generate UUID ถ้าไม่มีใน Header `X-Correlation-Id` | ☐ |
| 16.3 | **Log Levels** ต้องใช้ถูกต้อง: | ☐ |
| | `Trace` = verbose debug / `Debug` = development only / `Information` = business event | |
| | `Warning` = unexpected but recoverable / `Error` = failure / `Critical` = system down | |
| 16.4 | ห้าม Log ข้อมูล **PII/Sensitive**: Password, Token, CardNumber, NationalId — ต้อง Mask ก่อน | ☐ |
| 16.5 | ต้องรองรับ **Distributed Tracing** — OpenTelemetry (OTLP) หรือ Application Insights SDK | ☐ |
| 16.6 | **Health Check** ต้องตรวจ: Self (liveness), Database Connectivity (readiness), External Dependencies | ☐ |
| 16.7 | ต้อง Log ทุก **Outbound HTTP Call**: URL, Method, StatusCode, Duration (ms) | ☐ |
| 16.8 | Log Format ต้องเป็น **JSON** สำหรับ Production — ให้ Log Aggregator (ELK, Seq, Loki) parse ได้ | ☐ |
| 16.9 | **Critical/Fatal Error** ต้องมี Alert mechanism — ไม่ใช่แค่เขียน Log แล้วจบ | ☐ |
| 16.10 | ต้องมี **Metrics Endpoint** (`/metrics`): Response Time (P50/P95/P99), Error Rate, Throughput, Active Connections | ☐ |

### ตัวอย่าง CorrelationId Middleware

```csharp
// Middleware/CorrelationIdMiddleware.cs
public class CorrelationIdMiddleware(
    RequestDelegate next,
    ILogger<CorrelationIdMiddleware> _logger)
{
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Append(CorrelationIdHeader, correlationId);

        using (_logger.BeginScope(new Dictionary<string, object>
            { ["CorrelationId"] = correlationId }))
        {
            await next(context);
        }
    }
}
```


### Audit Report — Section 16

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Logging Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) — verify structured logging, log levels  
> 2. [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/dotnet/) — verify distributed tracing  
> 3. Cross-check กับ Section 8 (Error Handling & Logging), Section 18 (HTTP Client logging)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **16.1** | Structured Logging | string concat → log aggregator parse ไม่ได้ |
| **16.2** | CorrelationId ทุก request | trace request ข้าม service ไม่ได้ |
| **16.3** | Log Level ถูกต้อง | Error ใช้เป็น Info → alert เยอะเกิน (noise) |
| **16.4** | ห้าม log PII/Sensitive | log password → **PDPA/GDPR violation** |
| **16.5** | Distributed Tracing | microservice debug ไม่ได้ → ไม่รู้ว่า bottleneck อยู่ที่ไหน |
| **16.6** | Health Check | K8s ไม่รู้ว่า pod healthy → ไม่ restart เมื่อควร |
| **16.7** | Log outbound HTTP | external call fail แต่ไม่มี log → debug ไม่ได้ |
| **16.8** | JSON log format | plain text → ELK/Seq parse ไม่ได้ |
| **16.9** | Critical alert | system down แต่ไม่มีคนรู้ → downtime ยาว |
| **16.10** | Metrics endpoint | ไม่มี P95/P99 → ไม่รู้ว่า performance เสื่อม |

✅ Section 16 ผ่านการ audit

---

## 17. Performance Standards — มาตรฐานประสิทธิภาพ

> **อ้างอิง:** [ISO 25010 Performance Efficiency](https://iso25000.com/index.php/en/iso-25000-standards/iso-25010), Microsoft [ASP.NET Core Performance Best Practices](https://learn.microsoft.com/en-us/aspnet/core/performance/performance-best-practices), [.NET Performance Tips](https://learn.microsoft.com/en-us/dotnet/framework/performance/performance-tips)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 17.1 | **API Response Time SLA**: P95 ≤ **500ms** (CRUD ทั่วไป), P95 ≤ **2s** (Complex Query/Report) | ☐ |
| 17.2 | ห้ามมี **N+1 Query Problem** — ต้องใช้ JOIN, Batch Query, หรือ Multi-Mapping | ☐ |
| 17.3 | ทุก Database Query ต้องมี **Execution Plan ที่ใช้ Index** — ห้าม Full Table Scan บน Production table ≥ 10,000 rows | ☐ |
| 17.4 | **Response Payload Size**: ไม่เกิน **1 MB** สำหรับ API ทั่วไป — ใช้ Pagination สำหรับ Collection | ☐ |
| 17.5 | **Async/Await** ต้องใช้ตลอด pipeline (Controller → Service → Repository) — ห้ามมี Sync-over-Async | ☐ |
| 17.6 | ห้ามใช้ **`.Result`**, **`.Wait()`**, **`.GetAwaiter().GetResult()`** — ต้อง `await` เสมอ (ป้องกัน Deadlock + ThreadPool Starvation) | ☐ |
| 17.7 | ต้อง **Dispose** `IDisposable` resources ด้วย `using` statement เสมอ — Connection, Stream, HttpClient | ☐ |
| 17.8 | **String** manipulation จำนวนมาก (> 5 concatenations ใน loop) ต้องใช้ `StringBuilder` | ☐ |
| 17.9 | Configuration / Static data ที่อ่านบ่อยต้อง **Cache** — ห้าม Read from DB ทุก Request | ☐ |
| 17.10 | ห้าม Return **`IQueryable<T>`** ออกจาก Repository — ต้อง Materialize เป็น `IEnumerable<T>` / `List<T>` ก่อน | ☐ |


### Audit Report — Section 17

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [ASP.NET Core Performance Best Practices](https://learn.microsoft.com/en-us/aspnet/core/performance/performance-best-practices) — verify async, dispose, sync-over-async  
> 2. Microsoft — [.NET Performance Tips](https://learn.microsoft.com/en-us/dotnet/framework/performance/performance-tips)  
> 3. Cross-check กับ Section 3 (async Repository), Section 4 (async Service), Section 24 (Caching)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **17.1** | Response Time SLA P95 ≤ 500ms | ช้าเกิน → user experience แย่ + timeout |
| **17.2** | ห้าม N+1 Query | query 100 รอบ แทน 1 รอบ → ช้า 100x |
| **17.3** | ต้องใช้ Index | Full Table Scan → query ช้าขึ้นตาม data size |
| **17.4** | Payload ≤ 1 MB | response ใหญ่ → bandwidth + memory spike |
| **17.5** | Async ตลอด pipeline | sync method block thread → server รับ load ไม่ได้ |
| **17.6** | ห้าม .Result/.Wait() | deadlock + **ThreadPool starvation** |
| **17.7** | Dispose IDisposable | connection/stream leak → **memory leak** |
| **17.8** | StringBuilder | string concat ใน loop → O(n²) memory allocation |
| **17.9** | Cache static data | อ่าน DB ทุก request → DB load สูงเกิน |
| **17.10** | ห้าม return IQueryable | *(Note: Dapper ไม่ใช้ IQueryable — เกณฑ์นี้เป็น safeguard กรณีใช้ EF Core ในอนาคต)* |

✅ Section 17 ผ่านการ audit

---

## 18. HTTP Client Standards — การเรียก API ภายนอก

> **อ้างอิง:** Microsoft [IHttpClientFactory](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests), [Polly](https://github.com/App-vNext/Polly), Microsoft [Cloud Design Patterns](https://learn.microsoft.com/en-us/azure/architecture/patterns/)  
> **หลักการ:** ทุก Outbound HTTP Call ต้อง Resilient — External Service อาจ Fail ได้ทุกเมื่อ

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 18.1 | ต้องใช้ **`IHttpClientFactory`** — ห้าม `new HttpClient()` ตรง (ป้องกัน Socket Exhaustion + DNS caching) | ☐ |
| 18.2 | ต้อง Configure **Named HttpClient** หรือ **Typed HttpClient** ใน DI Registration | ☐ |
| 18.3 | ต้องกำหนด **Timeout** ทุก HttpClient — default **30 วินาที** (ปรับตาม use case) | ☐ |
| 18.4 | ต้องมี **Retry Policy** (Polly / `Microsoft.Extensions.Http.Resilience`) สำหรับ Transient Error (408, 429, 5xx) | ☐ |
| 18.5 | Retry ต้องใช้ **Exponential Backoff**: max **3 ครั้ง**, delay 1s → 2s → 4s | ☐ |
| 18.6 | ต้องมี **Circuit Breaker** สำหรับ External Service ที่มี SLA — Break after 5 failures in 30s | ☐ |
| 18.7 | ต้อง **Log** ทุก Outbound Request: URL, Method, StatusCode, Duration (ms) | ☐ |
| 18.8 | ต้องส่ง **`X-Correlation-Id`** Header ไปกับทุก Outbound Request | ☐ |
| 18.9 | Response ต้อง Deserialize เป็น **strongly-typed Model** — ห้ามใช้ `dynamic` หรือ `JObject` | ☐ |
| 18.10 | ห้ามใช้ **`HttpClient`** ใน Singleton Service โดยไม่ผ่าน `IHttpClientFactory` | ☐ |

### ตัวอย่าง Typed HttpClient + Polly

```csharp
// Program.cs — HttpClient Registration with Resilience
builder.Services.AddHttpClient<IExternalPaymentService, ExternalPaymentService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PaymentApi:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddStandardResilienceHandler(options =>
{
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
    options.CircuitBreaker.FailureRatio = 0.5;
    options.CircuitBreaker.MinimumThroughput = 5;
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
});
```


### Audit Report — Section 18

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [IHttpClientFactory](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests) — verify Named/Typed HttpClient, socket exhaustion  
> 2. Microsoft — [Microsoft.Extensions.Http.Resilience](https://learn.microsoft.com/en-us/dotnet/core/resilience/) — verify retry, circuit breaker  
> 3. Cross-check กับ Section 16 (Log outbound), Section 22 (Resilience)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **18.1** | IHttpClientFactory | new HttpClient() → **Socket Exhaustion** + DNS cache stale |
| **18.2** | Named/Typed HttpClient | config กระจาย → maintain ยาก |
| **18.3** | Timeout ทุก client | ไม่มี timeout → request ค้าง → thread leak |
| **18.4** | Retry Policy | transient error → fail ทั้งที่ retry แล้วสำเร็จ |
| **18.5** | Exponential Backoff | retry ทันที → flood target service |
| **18.6** | Circuit Breaker | service ล่ม → ส่ง request ซ้ำไม่หยุด → **cascading failure** |
| **18.7** | Log outbound request | call fail แต่ไม่มี log → debug ไม่ได้ *(ดู §16.7)* |
| **18.8** | X-Correlation-Id | trace ข้าม service ไม่ได้ *(ดู §16.2)* |
| **18.9** | Strongly-typed response | dynamic → runtime error + ไม่มี IntelliSense |
| **18.10** | ห้าม HttpClient ใน Singleton | socket ไม่ถูก recycle → DNS stale + leak |

✅ Section 18 ผ่านการ audit

---

## 19. Dependency Management — การจัดการ NuGet Package

> **อ้างอิง:** Microsoft [NuGet Best Practices](https://learn.microsoft.com/en-us/nuget/concepts/best-practices-for-managing-packages), [OWASP Dependency-Check](https://owasp.org/www-project-dependency-check/), Microsoft [NuGet Best Practices](https://learn.microsoft.com/en-us/nuget/concepts/best-practices-for-managing-packages)

### 🔴 เกณฑ์บังคับ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 19.1 | NuGet Package Version ต้อง **Pin exact version** — ห้ามใช้ floating version (`*`, `1.*`) | ☐ |
| 19.2 | ต้องมี **Approved Package List** — ห้ามเพิ่ม Package ใหม่โดยไม่ผ่าน Review | ☐ |
| 19.3 | ห้ามใช้ Package ที่ **Deprecated**, **Archived**, หรือ **Unmaintained** (> 2 ปีไม่อัปเดต) | ☐ |
| 19.4 | ต้อง **สแกน CVE** ก่อนทุก Release — `dotnet list package --vulnerable` | ☐ |
| 19.5 | ห้ามมี **Critical/High CVE** ใน Production Dependencies | ☐ |
| 19.6 | **Package License** ต้อง Compatible กับองค์กร — อนุญาต: MIT, Apache 2.0, BSD — ห้าม: GPL ใน Commercial | ☐ |
| 19.7 | ต้อง **Update** Framework & Package อย่างน้อย **ทุก Quarter** (Security Patches) | ☐ |
| 19.8 | ห้ามใช้ Package ที่ **ทำหน้าที่ซ้ำกัน** — เช่น ทั้ง `Newtonsoft.Json` + `System.Text.Json` ในโปรเจคเดียว | ☐ |


### Audit Report — Section 19

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [NuGet Best Practices](https://learn.microsoft.com/en-us/nuget/concepts/best-practices-for-managing-packages) — verify version pinning, CVE scan  
> 2. [OWASP Dependency-Check](https://owasp.org/www-project-dependency-check/) — verify vulnerability scanning  
> 3. Cross-check กับ Section 26 (DevOps — Library Scanning stage)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **19.1** | Pin exact version | floating version → build ไม่ deterministic |
| **19.2** | Approved Package List | ใครก็เพิ่ม package ได้ → dependency bloat + security risk |
| **19.3** | ห้าม deprecated package | deprecated → ไม่มี security patch |
| **19.4** | สแกน CVE ก่อน release | deploy package ที่มี vulnerability → **security breach** |
| **19.5** | ห้าม Critical/High CVE | known exploit ใน production → **compliance violation** |
| **19.6** | License compatibility | GPL ใน commercial → **legal risk** |
| **19.7** | Update ทุก quarter | เก่าเกิน → CVE สะสม + ไม่ได้ performance improvement |
| **19.8** | ห้าม duplicate purpose | 2 JSON lib → binary ใหญ่ + maintain ยาก |

✅ Section 19 ผ่านการ audit

---

## 20. Code Complexity Metrics — เกณฑ์ความซับซ้อนของโค้ด

> **วัตถุประสงค์:** จำกัดความซับซ้อนเพื่อให้โค้ดอ่านง่าย ส่งต่องานได้ ลดโอกาสเกิด Bug  
> **อ้างอิง:** [McCabe — Cyclomatic Complexity (NIST SP 500-235)](https://www.mccabe.com/pdf/mccabe-nist235r.pdf), [SonarSource — Cognitive Complexity](https://www.sonarsource.com/resources/cognitive-complexity/), [Clean Code — Robert C. Martin](https://www.oreilly.com/library/view/clean-code-a/9780136083238/)

### 🔴 เกณฑ์บังคับ

| # | Metric | SonarQube Rule | ⚠️ Warning | ❌ Error (ต้อง Refactor) | ที่มา |
|---|--------|:--------------:|:----------:|:----------------------:|-------|
| 20.1 | **Cognitive Complexity** per Method | S3776 | ≤ 15 | > 20 | SonarSource — วัด "ความยากในการอ่านเข้าใจ" (default = 15) |
| 20.2 | **Cyclomatic Complexity** per Method | S1541 | ≤ 10 | > 15 | McCabe / NIST SP 500-235 — วัด "จำนวน test path" (default = 10) |
| 20.3 | **Method Length** (lines of body) | — | ≤ 30 | > 50 | Clean Code — ยิ่งสั้นยิ่งอ่านง่าย |
| 20.4 | **Parameters per Method** | S107 | ≤ 4 | > 6 | Clean Code — เกิน → สร้าง Request DTO |
| 20.5 | **Nesting Depth** (if/for/while ซ้อน) | S134 | ≤ 3 | > 4 | SonarQube default = 3 — ใช้ Guard Clause แทน |
| 20.6 | **Class Length** (total lines) | — | ≤ 300 | > 500 | Clean Code + SRP — เกิน → แยก Service |

> **💡 Grey Zone (ระหว่าง Warning กับ Error):** ถือว่า "ผ่านแบบมีเงื่อนไข" — Reviewer พิจารณาเป็นรายกรณี  
> หากมีเหตุผลสมควร (เช่น `switch-case` จำนวนมากที่หลีกเลี่ยงไม่ได้) ให้บันทึกเหตุผลใน Code Review Comment

### ทำไมต้องมี 2 Complexity Metrics?

| | Cyclomatic (McCabe 1976) | Cognitive (SonarSource 2016) |
|---|---|---|
| **วัดอะไร** | จำนวน linearly independent paths | ความยากในการอ่านเข้าใจ |
| **ใช้ทำอะไร** | ประเมินจำนวน Test Cases ที่ต้องเขียน | ประเมินว่า Developer คนถัดไปจะอ่านรู้เรื่องไหม |
| **switch/case** | +1 ต่อทุก case | +1 ครั้งเดียวทั้ง switch |
| **Nesting** | ไม่สนใจ | ยิ่ง nested ยิ่ง penalty สูง |

### วิธี Refactor เมื่อเกินเกณฑ์

| สถานการณ์ | วิธีแก้ |
|---|---|
| Method ยาวเกิน | **Extract Method** — แยกเป็น private method ที่ชื่อสื่อความหมาย |
| Nesting ลึกเกิน | **Guard Clause / Early Return** — ลด nesting level |
| Parameters เยอะเกิน | **Request DTO** — สร้าง Parameter Object |
| Cyclomatic สูง | **Strategy Pattern / Dictionary mapping** — แทน if/else chain |
| Class ใหญ่เกิน | **Extract Service** — แยกตาม Single Responsibility |


### Audit Report — Section 20

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. [McCabe — Cyclomatic Complexity (NIST SP 500-235)](https://www.mccabe.com/pdf/mccabe-nist235r.pdf) — verify thresholds  
> 2. [SonarSource — Cognitive Complexity](https://www.sonarsource.com/resources/cognitive-complexity/) — verify rules S3776, S1541  
> 3. Cross-check กับ Section 4 (Service design), Section 14 (formatting)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **20.1** | Cognitive Complexity ≤ 15 | method อ่านไม่เข้าใจ → maintain ลำบาก |
| **20.2** | Cyclomatic Complexity ≤ 10 | test path เยอะ → ต้องเขียน test มาก + bug ซ่อนง่าย |
| **20.3** | Method Length ≤ 30 lines | method ยาว → อ่านไม่จบ → ผิดพลาดง่าย |
| **20.4** | Parameters ≤ 4 | parameter เยอะ → เรียกผิดลำดับ → bug |
| **20.5** | Nesting Depth ≤ 3 | nested ลึก → อ่านยาก → cognitive overload |
| **20.6** | Class Length ≤ 300 lines | class ใหญ่ → ผิด SRP → แก้จุดหนึ่งกระทบอีกจุด |

✅ Section 20 ผ่านการ audit

---

## 21. Documentation Standards — มาตรฐานเอกสาร

> **อ้างอิง:** [OpenAPI Specification](https://swagger.io/specification/), Microsoft [OpenAPI support in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 21.1 | **API Specification** ต้องมี Swagger/OpenAPI spec ที่ตรงกับ Implementation — auto-generate จาก code | ☐ |
| 21.2 | ทุก Controller Action ต้องมี **XML Comment** (`/// <summary>`, `/// <param>`, `/// <returns>`) สำหรับ generate API documentation | ☐ |
| 21.3 | **Response Model** ต้องมี Example Value ใน OpenAPI UI — ใช้ OpenAPI document transformer หรือ `[SwaggerSchema]` (legacy) | ☐ |
| 21.4 | API Spec ต้อง Export เป็น `swagger.json` / `openapi.json` file สำหรับ API Testing Tools (Postman, REST Client) | ☐ |


### Audit Report — Section 21

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [OpenAPI support](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview) — verify OpenAPI spec generation  
> 2. Cross-check กับ Section 10 (OpenAPI setup), Section 11 (XML Comment)  
> ⚠️ **แก้ไข:** เปลี่ยนอ้างอิงจาก Swashbuckle → OpenAPI (สอดคล้องกับ §10)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **21.1** | API Spec auto-generate | spec กับ code ไม่ตรงกัน → client integrate ผิด |
| **21.2** | XML Comment ทุก Action | OpenAPI doc ไม่มีคำอธิบาย → developer อ่านไม่รู้เรื่อง |
| **21.3** | Example Value | ไม่มี example → developer ต้องเดา format |
| **21.4** | Export openapi.json | ไม่มี file → Postman/REST Client import ไม่ได้ |

✅ Section 21 ผ่านการ audit (แก้ Swashbuckle ref → OpenAPI แล้ว)

---

## 22. Resilience & Fault Tolerance — ความทนทานต่อความผิดพลาด

> **อ้างอิง:** Microsoft [Cloud Design Patterns](https://learn.microsoft.com/en-us/azure/architecture/patterns/), [Polly v8+](https://github.com/App-vNext/Polly), [Microsoft.Extensions.Http.Resilience](https://learn.microsoft.com/en-us/dotnet/core/resilience/), [Release It! — Michael Nygard](https://pragprog.com/titles/mnee2/release-it-second-edition/)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 22.1 | External Service call ต้องมี **Circuit Breaker** (Polly / `Microsoft.Extensions.Resilience`) | ☐ |
| 22.2 | Circuit Breaker Config: Break after **5 failures** in **30s**, Open Duration **30s** | ☐ |
| 22.3 | ต้องมี **Fallback Strategy** สำหรับ Non-critical External Dependencies (เช่น return cached data) | ☐ |
| 22.4 | **Timeout** ต้องกำหนดทุก External Call — ห้ามใช้ Infinite Timeout | ☐ |
| 22.5 | ต้องมี **Graceful Shutdown**: รอ In-flight Requests เสร็จก่อน Shutdown (`IHostApplicationLifetime`) | ☐ |
| 22.6 | ต้องรองรับ **Bulkhead Isolation**: จำกัด Concurrent Calls ต่อ External Service (ป้องกัน Cascading Failure) | ☐ |
| 22.7 | Error ต้อง **Propagate อย่างถูกต้อง** — ห้าม Swallow Exception โดยไม่ Log | ☐ |
| 22.8 | ต้อง **Design for Failure**: assume ทุก External Dependency จะ Fail — Service ต้อง Degrade gracefully | ☐ |


### Audit Report — Section 22

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Cloud Design Patterns](https://learn.microsoft.com/en-us/azure/architecture/patterns/) — verify Circuit Breaker, Bulkhead, Retry  
> 2. Microsoft — [Microsoft.Extensions.Http.Resilience](https://learn.microsoft.com/en-us/dotnet/core/resilience/) — verify Polly v8+  
> 3. Cross-check กับ Section 18 (HTTP Client resilience), Section 17 (Performance)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **22.1** | Circuit Breaker | external service ล่ม → ส่ง request ซ้ำ → **cascading failure** |
| **22.2** | CB Config (5 failures/30s) | threshold ต่ำเกิน → open บ่อย / สูงเกิน → ไม่ protect |
| **22.3** | Fallback Strategy | service ล่ม → user เห็น error แทนที่จะเห็น cached data |
| **22.4** | Timeout ทุก external call | ไม่มี timeout → request ค้าง → thread leak *(ดู §18.3)* |
| **22.5** | Graceful Shutdown | kill pod ทันที → in-flight request ถูก drop |
| **22.6** | Bulkhead Isolation | service หนึ่งช้า → กิน thread pool หมด → ทุก service ช้า |
| **22.7** | ห้าม swallow exception | catch แล้วไม่ log → error หายไป → debug ไม่ได้ *(ดู §8.3)* |
| **22.8** | Design for Failure | assume ทุกอย่างจะ fail → เตรียม graceful degradation |

✅ Section 22 ผ่านการ audit

---

## 23. Concurrency & Thread Safety — ความปลอดภัยของ Thread

> **อ้างอิง:** Microsoft [Threading Best Practices](https://learn.microsoft.com/en-us/dotnet/standard/threading/managed-threading-best-practices), [Concurrent Collections](https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/), [Stephen Cleary — Async Best Practices](https://blog.stephencleary.com/)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 23.1 | **Shared Mutable State** ต้องป้องกันด้วย `lock`, `SemaphoreSlim`, หรือ `ConcurrentCollection` | ☐ |
| 23.2 | ต้องใช้ **`ConcurrentDictionary<K,V>`** แทน `Dictionary<K,V>` + `lock` สำหรับ thread-safe scenarios | ☐ |
| 23.3 | ห้ามใช้ **`lock`** กับ `async` code — ต้องใช้ **`SemaphoreSlim(1,1)`** แทน | ☐ |
| 23.4 | **Singleton Service** ต้อง Thread-safe — ห้ามมี Mutable Instance State | ☐ |
| 23.5 | ห้ามใช้ **`static` mutable fields** ใน Web Application (ยกเว้น Thread-safe Collections / Immutable) | ☐ |
| 23.6 | ใช้ **`Channel<T>`** สำหรับ Producer-Consumer Pattern แทน `Queue<T>` + `lock` | ☐ |


### Audit Report — Section 23

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Threading Best Practices](https://learn.microsoft.com/en-us/dotnet/standard/threading/managed-threading-best-practices) — verify lock, SemaphoreSlim  
> 2. Microsoft — [Concurrent Collections](https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/) — verify ConcurrentDictionary  
> 3. Cross-check กับ Section 6 (DI lifetimes — Singleton), Section 17 (async)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **23.1** | ป้องกัน Shared Mutable State | **race condition** → data corruption (ไม่ deterministic) |
| **23.2** | ConcurrentDictionary | Dictionary + lock → code verbose + deadlock risk |
| **23.3** | ห้าม lock กับ async | lock + async → **deadlock** (lock ไม่ release ข้าม thread) |
| **23.4** | Singleton Thread-safe | Singleton มี mutable state → request ข้ามกันเห็น data ผิด |
| **23.5** | ห้าม static mutable | static field shared ทุก request → **concurrency bug** |
| **23.6** | Channel\<T\> | Queue + lock → boilerplate เยอะ + error-prone |

✅ Section 23 ผ่านการ audit

---

## 24. Caching Standards — มาตรฐานการ Cache

> **อ้างอิง:** Microsoft [Caching Best Practices](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview), [Redis Best Practices](https://redis.io/docs/management/optimization/), [Cache-Aside Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cache-aside)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 24.1 | **In-Memory Cache** ใช้ `IMemoryCache` — สำหรับ data ที่ใช้ภายใน instance เดียว | ☐ |
| 24.2 | **Distributed Cache** ใช้ `IDistributedCache` (Redis) — สำหรับ data ที่ share ข้าม instances | ☐ |
| 24.3 | **Cache Key Naming**: `{ServiceName}:{Entity}:{Identifier}` — เช่น `"CustomerService:Customer:12345"` | ☐ |
| 24.4 | ทุก Cache Entry ต้องมี **TTL (Time-To-Live)** — ห้าม Cache แบบไม่มีวันหมดอายุ | ☐ |
| 24.5 | **TTL แนะนำ**: Config/Static = 1 hour, Business Data = 5-15 min, Session = ตาม JWT Lifetime | ☐ |
| 24.6 | ต้องมี **Cache Invalidation Strategy**: Time-based (TTL) หรือ Event-based (Pub/Sub) | ☐ |
| 24.7 | ห้าม Cache **Sensitive Data** (PII, Token, Password) ใน Distributed Cache โดยไม่เข้ารหัส | ☐ |
| 24.8 | ต้อง Handle **Cache Miss** gracefully — Fallback ไป Database, ห้าม throw exception | ☐ |

### ตัวอย่าง Cache-Aside Pattern

```csharp
// Services/CustomerService.cs — Cache-Aside Pattern
public async Task<CustomerResponse?> GetCustomerCachedAsync(
    string customerId, CancellationToken cancellationToken = default)
{
    var cacheKey = $"CustomerService:Customer:{customerId}";

    if (!_memoryCache.TryGetValue(cacheKey, out CustomerResponse? cached))
    {
        var entity = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (entity is null) return null;

        cached = MapToResponse(entity);
        _memoryCache.Set(cacheKey, cached, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(3)
        });
    }

    return cached;
}
```


### Audit Report — Section 24

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Caching Best Practices](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview) — verify IMemoryCache, IDistributedCache  
> 2. Microsoft — [Cache-Aside Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cache-aside) — verify pattern  
> 3. Cross-check กับ Section 17 (Performance — cache static data), Section 16 (PII logging)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **24.1** | IMemoryCache (single instance) | cache library อื่น → ไม่ integrate กับ DI |
| **24.2** | IDistributedCache (Redis) | in-memory cache ข้าม instance ไม่ได้ → data ไม่ consistent |
| **24.3** | Cache Key Naming convention | key ชนกัน → cache wrong data |
| **24.4** | TTL ทุก entry | ไม่มี TTL → data เก่าค้าง → **stale data bug** |
| **24.5** | TTL แนะนำ | TTL สั้นเกิน → cache miss เยอะ / ยาวเกิน → stale data |
| **24.6** | Cache Invalidation Strategy | data เปลี่ยนแต่ cache ไม่ update → user เห็นค่าเก่า |
| **24.7** | ห้าม cache sensitive data | PII ใน Redis → **data breach risk** *(ดู §16.4)* |
| **24.8** | Handle Cache Miss | cache miss → throw exception → service down |

✅ Section 24 ผ่านการ audit

---

## 25. Feature Flags — การจัดการ Feature Toggle

> **อ้างอิง:** Microsoft [Feature Management](https://learn.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management), [Martin Fowler — Feature Toggles](https://martinfowler.com/articles/feature-toggles.html)

### 🟡 เกณฑ์แนะนำ

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 25.1 | Feature Flag ต้อง Manage ผ่าน **Configuration** (`appsettings` / Azure App Configuration) — ห้าม Hard-code `if/else` | ☐ |
| 25.2 | Flag Naming: **`FeatureFlags:{FeatureName}`** — เช่น `"FeatureFlags:NewPaymentFlow"` | ☐ |
| 25.3 | ใช้ `Microsoft.FeatureManagement` NuGet Package สำหรับ ASP.NET Core | ☐ |
| 25.4 | ต้อง **Clean up** Feature Flag ที่ Fully Rolled Out ภายใน **1 Sprint** — ห้ามค้างใน code | ☐ |
| 25.5 | ต้อง **Log** เมื่อ Feature Flag ถูก Evaluate: Flag Name, Result (On/Off), User/Context | ☐ |


### Audit Report — Section 25

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. Microsoft — [Feature Management](https://learn.microsoft.com/en-us/azure/azure-app-configuration/concept-feature-management) — verify FeatureManagement NuGet  
> 2. Cross-check กับ Section 9 (Configuration), Section 11 (Code Quality — cleanup)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **25.1** | Flag ใน Configuration | hard-code if/else → ต้อง redeploy เพื่อเปลี่ยน flag |
| **25.2** | Flag Naming convention | ชื่อไม่สม่ำเสมอ → สับสน + key ชนกัน |
| **25.3** | Microsoft.FeatureManagement | build เอง → boilerplate + ไม่มี percentage rollout |
| **25.4** | Clean up ภายใน 1 Sprint | flag ค้าง → code มี dead branch → complexity สูงขึ้น |
| **25.5** | Log flag evaluation | flag on/off แต่ไม่มี log → debug ลำบาก |

✅ Section 25 ผ่านการ audit

---

## 26. DevOps Files — ขอบเขตความรับผิดชอบ

> **หลักการ:** Separation of Concerns — แต่ละฝ่ายมีหน้าที่ชัดเจน, Developer ต้องเข้าใจแต่ไม่แก้ไข  
> **อ้างอิง:** Microsoft [DevOps Resource Center](https://learn.microsoft.com/en-us/devops/), [DORA Metrics](https://dora.dev/)

### 🚫 ไฟล์ที่ห้ามแก้ไข — DevOps/Infrastructure Responsibility

> **⚠️ สำคัญ:** ไฟล์เหล่านี้เป็นความรับผิดชอบของ **ฝ่าย DevOps/Infrastructure**  
> Developer **ห้ามแก้ไข** โดยเด็ดขาด แต่ **ควรอ่านเพื่อเข้าใจบริบท** ของ CI/CD Pipeline, Deployment Strategy, และ Infrastructure Configuration

#### A. Jenkins Pipeline Files — CI/CD Configuration

| ไฟล์ | หน้าที่ | เจ้าของ | Developer Action |
|------|--------|---------|-----------------|
| `Jenkinsfile` | CI/CD Pipeline สำหรับ Development | DevOps Team | 📖 อ่านอย่างเดียว |
| `Jenkinsfile-PROD` | CI/CD Pipeline สำหรับ Production | DevOps Team | 📖 อ่านอย่างเดียว |
| `Jenkinsfile-Tag` | Release Tagging Pipeline | DevOps Team | 📖 อ่านอย่างเดียว |
| `Jenkinsfile_DEV` | Development Environment Pipeline | DevOps Team | 📖 อ่านอย่างเดียว |
| `Jenkinsfile_UAT` | UAT Environment Pipeline | DevOps Team | 📖 อ่านอย่างเดียว |
| `Jenkinsfile_PRD` | Production Deployment Pipeline | DevOps Team | 📖 อ่านอย่างเดียว |

**ทำไมห้ามแก้ไข:**
- Pipeline กำหนดโดย **Security/Compliance Requirements**
- มี **Quality Gates** ที่ทีม DevOps/Security audit แล้ว (SonarQube threshold, Vulnerability scan, Secret detection)
- เชื่อมต่อกับ **Infrastructure Credentials** ที่ sensitive
- การแก้ไขอาจ **ข้าม Security Scanning** หรือ **Break Production Deployment**

**Developer ควรเข้าใจอะไร:**
```groovy
// ตัวอย่าง Jenkinsfile Stage ที่ Developer ควรรู้
stages {
    stage ('Restore Package')        // → NuGet restore from Private Registry
    stage ('Run UnitTest')           // → Test ต้องผ่าน 100%
    stage ('Run CodeCoverage')       // → Coverage threshold (เช่น 20%+)
    stage ('Run Sonarqube')          // → Code Quality gate ต้องผ่าน
    stage ('Secrets Scanning')       // → ห้ามมี credentials ใน code
    stage ('Library Scanning')       // → OWASP Dependency-Check
    stage ('Build Docker Image')     // → Container image for deployment
    stage ('Deploy to K8s')          // → Kubernetes deployment
}
```

#### B. Docker Files — Container Build Configuration

| ไฟล์ | หน้าที่ | เจ้าของ | Developer Action |
|------|--------|---------|-----------------|
| `Dockerfile` | Production Container Build | DevOps Team | 📖 อ่านอย่างเดียว |
| `DockerfileTest` | Test Container Build | DevOps Team | 📖 อ่านอย่างเดียว |
| `DockerfileSonarqube` | SonarQube Analysis Container | DevOps Team | 📖 อ่านอย่างเดียว |
| `DockerfileLibScan` | OWASP Dependency-Check Container | DevOps Team | 📖 อ่านอย่างเดียว |

**ทำไมห้ามแก้ไข:**
- Base Image version **ตรวจสอบ Vulnerability** แล้วโดย Security Team
- Certificate configuration กำหนดตาม **Corporate PKI**
- Build args และ environment **optimize สำหรับ Production**
- การแก้ไขอาจทำให้ Container **ไม่ผ่าน Security Scan** หรือ **ขนาดใหญ่เกินไป**

**Developer ควรเข้าใจอะไร:**
```dockerfile
# ตัวอย่าง Dockerfile Structure ที่ Developer ควรรู้
FROM mcr.microsoft.com/dotnet/aspnet:8.0    # → Base Image (DevOps เลือก)
COPY nexus-cert.crt /usr/local/share/...   # → Corporate Certificate
WORKDIR /app                                 # → Working directory
COPY published-output .                      # → Build output from dotnet publish
EXPOSE 5001                                  # → Application port (จาก appsettings)
ENTRYPOINT ["dotnet", "YourApp.dll"]        # → Start command
```

#### C. Kubernetes Deployment Files

| ไฟล์ | หน้าที่ | เจ้าของ | Developer Action |
|------|--------|---------|-----------------|
| `deployment.yaml` | K8s Deployment Manifest | DevOps Team | 📖 อ่านอย่างเดียว |
| `service.yaml` | K8s Service Configuration | DevOps Team | 📖 อ่านอย่างเดียว |
| `configmap.yaml` | K8s ConfigMap | DevOps Team | 📖 อ่านอย่างเดียว |

#### D. Build Configuration Files

| ไฟล์ | หน้าที่ | เจ้าของ | Developer Action |
|------|--------|---------|-----------------|
| `settings.xml` (Maven) | Private Nexus Registry Config | DevOps Team | 📖 อ่านอย่างเดียว |
| `Nuget.Config` | NuGet Private Feed Config | DevOps Team | 📖 อ่านอย่างเดียว |
| `.dockerignore` | Docker build exclude pattern | DevOps Team | 📖 อ่านอย่างเดียว |
| `hadolint.yaml` | Dockerfile linting rules | DevOps Team | 📖 อ่านอย่างเดียว |

---

### 🔴 เกณฑ์บังคับ — Developer Responsibility

| # | เกณฑ์ | ผ่าน / ไม่ผ่าน |
|---|-------|:--------------:|
| 26.1 | **ห้ามแก้ไข** Jenkinsfile, Dockerfile* โดยเด็ดขาด — ต้องผ่าน DevOps Team อนุมัติเท่านั้น | ☐ |
| 26.2 | **ห้ามแก้ไข** deployment.yaml, service.yaml, configmap.yaml — Infrastructure Team เป็นเจ้าของ | ☐ |
| 26.3 | **ห้ามแก้ไข** settings.xml, Nuget.Config — Private registry credentials ควบคุมโดย DevOps | ☐ |
| 26.4 | **ห้าม Commit** ไฟล์เหล่านี้ใน PR ของ Developer — จะถูก Reject อัตโนมัติ | ☐ |
| 26.5 | หาก**จำเป็นต้องเปลี่ยนแปลง** (เช่น เพิ่ม Port, Environment Variable) → **สร้าง Ticket ให้ DevOps Team** | ☐ |
| 26.6 | Developer ต้อง**อ่านและเข้าใจ** Pipeline Stages ใน Jenkinsfile — รู้ว่า Code จะผ่าน Gates อะไรบ้าง | ☐ |
| 26.7 | Developer ต้อง**รัน Local Test** ให้ผ่านก่อน Push — ห้ามพึ่ง Jenkins เป็นตัว Validate | ☐ |


### Audit Report — Section 26

> 📅 9 มีนาคม 2026  
> 🔍 วิธีการ:  
> 1. ตรวจสอบ Jenkinsfile, Dockerfile ตัวจริงในโปรเจคองค์กร  
> 2. Cross-check กับ Section 12 (Test ก่อน push), Section 19 (Dependency scan)

#### เกณฑ์มีอะไร ทำไมต้องมี

| # | เกณฑ์ | ถ้าไม่ทำ → เกิดอะไร |
|:-:|:------|:--------------------|
| **26.1** | ห้ามแก้ Jenkinsfile/Dockerfile | แก้ pipeline → ข้าม security scan → **vulnerability เข้า production** |
| **26.2** | ห้ามแก้ K8s manifests | แก้ deployment → pod config ผิด → service down |
| **26.3** | ห้ามแก้ settings.xml/Nuget.Config | แก้ registry → build fail ทั้งทีม |
| **26.4** | ห้าม commit DevOps files | PR มี DevOps file → auto-reject *(ตั้ง CODEOWNERS)* |
| **26.5** | สร้าง Ticket ให้ DevOps | แก้เอง → ไม่ผ่าน review → break compliance |
| **26.6** | อ่าน + เข้าใจ Pipeline | ไม่รู้ว่ามี Quality Gates → push code ที่ fail gate |
| **26.7** | Local test ก่อน push | push ไม่ผ่าน test → CI fail → block ทีม |

✅ Section 26 ผ่านการ audit

---

## สรุปจำนวนเกณฑ์ทั้งหมด

| หัวข้อ | ระดับ | จำนวนเกณฑ์ |
|--------|:-----:|:----------:|
| 1. Project Structure | 🔴 | 10 (7 บังคับ + 3 แนะนำ) |
| 2. Naming Convention | 🔴 | 62 (2A-2I: Capitalization, Acronym, Identifier, Project-Specific, General, File/Organization, Anti-Patterns, Audit) |
| 3. DataAccess Layer (Dapper) | 🔴 | 30 + 8 เกณฑ์ข้อยกเว้น ADO.NET |
| 4. Service Layer | 🔴 | 8 |
| 5. Controller Layer | 🔴 | 8 |
| 6. Dependency Injection | 🔴 | 6 |
| 7. Authentication | 🔴 | 6 |
| 8. Error Handling & Logging | 🔴 | 7 |
| 9. Configuration Management | 🔴 | 6 |
| 10. API Documentation (OpenAPI) | 🟡 | 5 |
| 11. Code Quality | 🟡 | 10 |
| **12. Testing Standards** | 🔴 | **15** |
| **13. API Design & Versioning** | 🔴 | **12** |
| **14. Code Formatting & Style** | 🔴 | **10** |
| **15. Input Validation** | 🔴 | **10** |
| **16. Observability & Monitoring** | 🔴 | **10** |
| **17. Performance Standards** | 🔴 | **10** |
| **18. HTTP Client Standards** | 🔴 | **10** |
| **19. Dependency Management** | 🔴 | **8** |
| **20. Code Complexity Metrics** | 🔴 | **6** |
| **21. Documentation Standards** | 🟡 | **4** |
| **22. Resilience & Fault Tolerance** | 🟡 | **8** |
| **23. Concurrency & Thread Safety** | 🟡 | **6** |
| **24. Caching Standards** | 🟡 | **8** |
| **25. Feature Flags** | 🟡 | **5** |
| **26. DevOps Files (Read-Only)** | 🔴 | **7** |
| **รวม** | | **295 เกณฑ์** (รวม 8 ADO.NET) |
| บังคับ (🔴) | | **246 เกณฑ์** |
| แนะนำ (🟡) | | **49 เกณฑ์** |

---

> **สร้างเมื่อ:** มีนาคม 2026  
> **อัปเดตล่าสุด:** 9 มีนาคม 2026 — **Audit §1–§26 ครบทุก section**:  
> • ลบ InternalPerplex + Google Eng Practices (MS ครบ) — เก็บ Google C# Style ใน §2 เท่านั้น  
> • แก้เลข # §12–§26 (เพี้ยนจากการลบ section เก่า)  
> • เพิ่ม 📖 อ้างอิง MS Docs §7–§11 | เพิ่ม Audit Report §3–§26 (24 sections)  
> • แก้ §10+§21 Swashbuckle → OpenAPI (.NET 9+) | แก้ §19 เพิ่ม NuGet Best Practices ref  
> • แก้ §26 DevOps Institute → MS DevOps + DORA | เพิ่ม cross-ref §11 ลด duplication  
> • จำนวนเกณฑ์: **295 เกณฑ์** รวม 8 ADO.NET (🔴 246 บังคับ, 🟡 49 แนะนำ)  
> **เอกสารวิจัยประกอบ:** [Research/Database_Access_Methods_NET10.md](./Research/Database_Access_Methods_NET10.md) | [Research/Dapper_Migration_Analysis.md](./Research/Dapper_Migration_Analysis.md)
