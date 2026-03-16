# Template — Reference Implementation

> **ASP.NET Core Web API + Dapper** · .NET 10 / C# 14  
> Starter Template ที่ปฏิบัติตาม Coding Standard Criteria ครบทุกเกณฑ์

---

## 📁 โครงสร้างโปรเจค

```
Template/
├── .editorconfig                      ← §14 Code Formatting Rules
├── SampleAPI.sln                      ← Solution file
├── dbs/                               ← Database scripts (ใส่เมื่อมี)
├── docs/                              ← Documentation (ใส่เมื่อมี)
├── src/
│   └── SampleAPI/
│       ├── SampleAPI.csproj           ← NuGet: Dapper, FluentValidation, JWT, Polly
│       ├── Program.cs                 ← §6 DI Registration + §7 JWT + §10 OpenAPI
│       ├── appsettings.json           ← §9 Base Config + §9.6 Messages
│       ├── appsettings.Development.json
│       ├── Controllers/
│       │   ├── CustomerController.cs  ← §5 Thin Controller: รับ → เรียก → ส่ง
│       │   └── HealthCheckController.cs  ← §5.6 Liveness Probe
│       ├── Services/
│       │   ├── Interfaces/
│       │   │   └── ICustomerService.cs  ← §4.1 Interface + XML Comment
│       │   └── CustomerService.cs     ← §4 Business Logic + try-catch + Log
│       ├── DataAccess/
│       │   ├── Connections/
│       │   │   ├── IDbConnectionFactory.cs  ← §3.3 Factory Pattern
│       │   │   └── DbConnectionFactory.cs   ← §6.3 Singleton
│       │   └── Repositories/
│       │       ├── Interfaces/
│       │       │   ├── IBaseRepository.cs    ← §3.4 Generic Base Interface
│       │       │   └── ICustomerRepository.cs ← §1.5 Mockable Interface
│       │       ├── BaseRepository.cs  ← §3.4 Dapper wrapper + CommandDefinition CT
│       │       └── CustomerRepository.cs  ← §3.6 @parameter, §3.11 no SELECT *
│       ├── Models/
│       │   ├── Entities/
│       │   │   └── CustomerEntity.cs  ← §1.2 DB Mapping
│       │   ├── Requests/
│       │   │   └── CreateCustomerRequest.cs  ← §1.2 Input DTO
│       │   ├── CustomerResponse.cs    ← §4.7 Response DTO (ห้าม return Entity)
│       │   ├── PagedResponse.cs       ← §13.4 Pagination Structure
│       │   └── ResultModel.cs         ← §8.5 Unified Error Response
│       ├── Filters/
│       │   ├── LogFilter.cs           ← §8.7 Auto Log Request/Response
│       │   └── AvailableFilter.cs     ← §25 Feature Flag / Availability
│       ├── Middleware/
│       │   └── CorrelationIdMiddleware.cs  ← §16.2 X-Correlation-Id
│       └── Validators/
│           └── CreateCustomerRequestValidator.cs  ← §15 FluentValidation
└── tests/
    └── UnitTest/
        └── SampleAPI.UnitTest/
            ├── SampleAPI.UnitTest.csproj  ← xUnit + Moq
            └── Services/
                └── CustomerServiceTests.cs  ← §12 AAA Pattern, Mock, 100% coverage
```

---

## 🎯 เกณฑ์ที่ครอบคลุม (ตัวอย่าง)

| Section | ไฟล์ตัวอย่าง | เกณฑ์ที่ demo |
|---------|-------------|--------------|
| §1 Structure | ทุกไฟล์ | 3-Layer, Interfaces แยก, tests แยก src |
| §2 Naming | ทุกไฟล์ | PascalCase, _camelCase, {Feature}Controller |
| §3 DataAccess | BaseRepository, CustomerRepository | Dapper only, @parameter, CommandDefinition, async+CT |
| §4 Service | CustomerService | Interface, DI, Guard Clause, try-catch, DTO mapping |
| §5 Controller | CustomerController | ControllerBase, [Authorize], thin actions, CT chain |
| §6 DI | Program.cs | Singleton→Scoped ordering, ห้าม captive |
| §7 Auth | Program.cs | JWT Bearer, Validate all = true, ClockSkew = Zero |
| §8 Error | CustomerService, LogFilter | ILogger, ResultModel, ห้าม StackTrace leak |
| §9 Config | appsettings.json | Per-environment, Messages in config |
| §10 OpenAPI | Program.cs | AddOpenApi() + MapOpenApi() |
| §12 Testing | CustomerServiceTests | xUnit, Moq, AAA, Mock Repository |
| §13 API Design | CustomerController | /api/v1/resource, HTTP Verbs, Pagination |
| §14 Formatting | .editorconfig | Allman, 4 spaces, naming rules |
| §15 Validation | CreateCustomerRequestValidator | FluentValidation, Required, Email, Regex |
| §16 Observability | CorrelationIdMiddleware, LogFilter | CorrelationId, Structured Logging |

---

## 🚀 วิธีใช้

1. **Copy Template** ทั้งโฟลเดอร์ไปเป็นโปรเจคใหม่
2. **Rename** `SampleAPI` → ชื่อโปรเจคของคุณ (ทุกที่)
3. **แก้ Connection String** ใน `appsettings.{Env}.json`
4. **เพิ่ม Feature** ตามรูปแบบ Customer: Controller → Service → Repository → Entity → DTO → Validator → Test
5. **Run Tests**: `dotnet test`
6. **Format Code**: `dotnet format`

---

> 📖 เอกสารเกณฑ์ฉบับเต็ม: [Coding_Standard_Criteria.md](../Coding_Standard_Criteria.md)  
> 🌐 Presentation: [Coding_Standard_Presentation.html](../Coding_Standard_Presentation.html)  
> 📊 Overview Diagram: [coding_standard_Overview_Diagram.md](../coding_standard_Overview_Diagram.md)
