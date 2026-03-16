# ทำไมถึงเลือก ESTATEMENT_API เป็น Coding Standard

> เอกสารนี้อธิบายเหตุผลว่าทำไม **ESTATEMENT_API** จาก Project `ESTATEMENT_CLOUD` บน TFS  
> ถูกเลือกให้เป็น Reference Standard สำหรับการพัฒนา API ในองค์กร

---

## 1. ที่มาของการคัดเลือก

### กระบวนการสำรวจ (Survey Process)

ทีมได้ทำการสแกนค้นหา Repository ทั้งหมดบน TFS Server (`ITGroup_Collection`) โดย:

| รายการ | จำนวน |
|--------|-------|
| จำนวน Project ที่สำรวจ | ~25 Projects |
| จำนวน Repository ที่ตรวจสอบ | ~40+ Repos |
| เกณฑ์ที่ใช้คัดกรอง | ใช้ EF Core + DbContext + มี Pattern ที่ชัดเจน |
| Repository ที่ผ่านเกณฑ์ขั้นสุดท้าย | **ESTATEMENT_API** |

### เกณฑ์การคัดเลือก (Selection Criteria)

1. ใช้ **Entity Framework Core** กับ DbContext อย่างถูกต้อง
2. มีการแยก Layer ชัดเจน (Controller → Service → DataAccess)
3. มี **Unit of Work Pattern** ที่ครบถ้วน
4. รองรับ **Multiple Database Contexts**
5. ใช้ **Dependency Injection** อย่างเป็นระบบ
6. มี **Interface Abstraction** ในทุก Layer
7. โครงสร้างโฟลเดอร์เป็นระเบียบ อ่านเข้าใจง่าย

---

## 2. เหตุผลหลัก 7 ข้อ ที่เลือก ESTATEMENT_API

### ✅ เหตุผลที่ 1: โครงสร้างโปรเจคแยก Layer ชัดเจน (Clean Layered Architecture)

```
eStatementAPI/
├── Controllers/          ← Presentation Layer (รับ Request / ส่ง Response)
├── Services/
│   ├── Interfaces/       ← Service Contracts (Interface)
│   ├── RegisterService.cs
│   ├── InquiryService.cs
│   └── ...               ← Business Logic Layer
├── DataAccess/
│   ├── eStatementDb/     ← Database Context #1
│   │   ├── Interface/
│   │   │   ├── IeStatementDataSets.cs
│   │   │   └── IeStatementUoW.cs
│   │   ├── eStatementContext.cs
│   │   ├── eStatementUoW.cs
│   │   ├── ActivateEntity.cs
│   │   └── ControlFlowEntity.cs
│   └── TermDb/           ← Database Context #2
│       ├── Interface/
│       │   ├── ITermDataSets.cs
│       │   └── ITermUoW.cs
│       ├── TermDbContext.cs
│       ├── TermUoW.cs
│       └── TermEntity.cs
├── Models/               ← Request/Response Models (DTO)
├── Filters/              ← Action Filters (Logging, Availability)
├── Startup.cs            ← DI Registration & Middleware Config
└── Program.cs            ← Entry Point
```

**ทำไมถึงดี:**
- แต่ละ Layer มีหน้าที่ชัดเจน ไม่ปนกัน
- Developer ใหม่สามารถเข้าใจโครงสร้างได้ภายในไม่กี่นาที
- เมื่อต้องแก้ไข Business Logic → ไปที่ `Services/`, ต้องแก้ DB → ไปที่ `DataAccess/`

**เทียบกับ Repo อื่นๆ ที่พบ:**
- หลาย Repo วาง Entity, DbContext, Service ปะปนกันในโฟลเดอร์เดียว
- บาง Repo ไม่มี Interface Layer → ทำให้ Unit Test ยากหรือทำไม่ได้

---

### ✅ เหตุผลที่ 2: Unit of Work Pattern ที่สมบูรณ์

ESTATEMENT_API ใช้ **Unit of Work (UoW) Pattern** ผ่าน NuGet Package `Easybuy.WebApplication.CoreUnitOfWork` ซึ่งให้ความสามารถ:

```csharp
// Interface: IeStatementUoW
public interface IeStatementUoW : IUnitOfWork<eStatementDbContext>, IeStatementDataSets
{
}
```

**ความสามารถของ UoW ที่มีครบ:**

| Feature | มีใน ESTATEMENT_API | อธิบาย |
|---------|:-------------------:|--------|
| `SaveChanges()` | ✅ | บันทึกทุก Change ใน Transaction เดียว |
| `ForceBeginTransaction()` | ✅ | เริ่ม Transaction แบบ Explicit |
| `CommitTransaction()` | ✅ | ยืนยัน Transaction |
| `RollbackTransaction()` | ✅ | ย้อนกลับเมื่อเกิด Error |
| `SetIsolationLevel()` | ✅ | กำหนด Isolation Level ได้ (Read Committed, Serializable, etc.) |
| `Set<T>()` | ✅ | เข้าถึง DbSet ของ Entity ใดก็ได้ |

**ทำไมถึงดี:**
- รับประกัน **Data Consistency** → ถ้า Operation ใดล้มเหลว Rollback ทั้งหมด
- ควบคุม Transaction ได้ละเอียด ไม่ต้องพึ่ง Auto-Commit
- Service Layer ไม่ต้องรู้จัก DbContext โดยตรง → Loose Coupling

**เทียบกับ Repo อื่นๆ:**
- ส่วนใหญ่ Inject `DbContext` ตรงเข้า Service → Tight Coupling
- ไม่มี Transaction Management → อาจเกิด Partial Save (บาง Record บันทึก บางอันไม่)

---

### ✅ เหตุผลที่ 3: รองรับ Multiple Database Contexts (UnitOfWork Pool)

โปรเจคนี้เชื่อมต่อ **2 ฐานข้อมูลพร้อมกัน** ผ่าน UoW Pool Pattern:

```csharp
// Startup.cs — การลงทะเบียน Multiple DbContext
services.AddDbContext<eStatementDbContext>(options =>
    options.UseMySQL(strConnectDb));
services.AddUnitOfWork<eStatementDbContext>();

services.AddDbContext<TermDbContext>(options =>
    options.UseMySQL(strTermConnectDb));
services.AddUnitOfWork<TermDbContext>();

// UoW Pool — รวม UoW หลายตัวไว้ในที่เดียว
services.AddUnitOfWorkPool(optionBuilder =>
{
    optionBuilder.AddUnitOfWork<eStatementDbContext>(eStatementUoW.KEY);  // KEY = "Activate"
    optionBuilder.AddUnitOfWork<TermDbContext>(TermUoW.KEY);              // KEY = "Term"
});
```

**ทำไมถึงดี:**
- ในงานจริง API มักต้องเชื่อมต่อหลาย Database (เช่น Main DB + Log DB + Config DB)
- Pattern นี้ทำให้เพิ่ม Database ใหม่ได้ง่าย โดย:
  1. สร้าง DbContext + Entity ใหม่
  2. สร้าง IDataSets + IUoW Interface
  3. สร้าง UoW Implementation
  4. ลงทะเบียนใน Startup.cs
- แต่ละ Database มี Context แยก ไม่ปนกัน

**เทียบกับ Repo อื่นๆ:**
- Repo ที่เจอส่วนใหญ่มี DbContext เดียว → ไม่มีตัวอย่าง Multi-DB
- บาง Repo ยัด Table ทุกอันเข้า DbContext เดียว แม้จะมาจากคนละ Database

---

### ✅ เหตุผลที่ 4: Interface Abstraction ครบทุก Layer

```
Controller  →  IService  →  IUoW  →  IDataSets
    ↓              ↓           ↓          ↓
(ไม่รู้จัก    (ไม่รู้จัก   (ไม่รู้จัก   (ไม่รู้จัก
 Service       DbContext    DbContext    Entity
 Implementation  ตรง)        ตรง)        ตรง)
 ตรง)
```

**ตัวอย่างจริงจาก Code:**

| Layer | Interface | Implementation |
|-------|-----------|----------------|
| Service | `IRegisterService` | `RegisterService` |
| Unit of Work | `IeStatementUoW` | `eStatementUoW` |
| DataSets | `IeStatementDataSets` | `eStatementDbContext` |

```csharp
// Controller รู้จักแค่ Interface ไม่รู้จัก Implementation
public class RegisterController : ControllerBase
{
    private readonly IRegisterService registerService;
    public RegisterController(IRegisterService _registerService)
    {
        registerService = _registerService;
    }
}

// Service รู้จักแค่ IeStatementUoW ไม่รู้จัก eStatementDbContext
public class RegisterService : IRegisterService
{
    readonly private IeStatementUoW _eStatementUoW;
    public RegisterService(..., IeStatementUoW eStatementUoW)
    {
        _eStatementUoW = eStatementUoW;
    }
}
```

**ทำไมถึงดี:**
- **Testability**: Mock `IeStatementUoW` ได้ง่ายใน Unit Test
- **Flexibility**: เปลี่ยน Implementation ได้โดยไม่กระทบ Layer อื่น
- **Maintainability**: แก้ไข Database Layer ไม่กระทบ Business Logic

**เทียบกับ Repo อื่นๆ:**
- หลาย Repo ให้ Controller เรียก DbContext โดยตรง (No Abstraction)
- บาง Repo มี Interface เฉพาะ Service แต่ไม่มีใน Data Layer

---

### ✅ เหตุผลที่ 5: Dependency Injection ลงทะเบียนเป็นระบบ

`Startup.cs` มีการลงทะเบียน DI อย่างเป็นระเบียบ แบ่งเป็นกลุ่มชัดเจน:

```csharp
// ─── กลุ่ม 1: Database Context ───
services.AddDbContext<eStatementDbContext>(...);
services.AddDbContext<TermDbContext>(...);

// ─── กลุ่ม 2: Unit of Work ───
services.AddUnitOfWork<eStatementDbContext>();
services.AddUnitOfWork<TermDbContext>();

// ─── กลุ่ม 3: UoW Pool ───
services.AddUnitOfWorkPool(optionBuilder => { ... });

// ─── กลุ่ม 4: UoW Interface → Implementation ───
services.AddScoped<IeStatementUoW, eStatementUoW>();
services.AddScoped<ITermUoW, TermUoW>();

// ─── กลุ่ม 5: Service Interface → Implementation ───
services.AddScoped<IRegisterService, RegisterService>();
services.AddScoped<IInquiryService, InquiryService>();
services.AddScoped<ICustomerService, CustomerService>();
// ... etc.
```

**ทำไมถึงดี:**
- ใช้ `AddScoped` → สร้าง Instance ใหม่ทุก Request (เหมาะสำหรับ Web API)
- เห็นภาพรวมการเชื่อมต่อระหว่าง Layer ได้ในที่เดียว
- เพิ่ม Service ใหม่ง่าย → เพิ่มบรรทัด `AddScoped<IXxx, Xxx>()` 1 บรรทัด

---

### ✅ เหตุผลที่ 6: Entity Configuration ใช้ Fluent API อย่างถูกต้อง

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    var activateBuilder = builder.Entity<ActivateEntity>();
    activateBuilder.ToTable("estm_activate");            // ← กำหนดชื่อ Table
    activateBuilder.HasKey(x => new { x.key });          // ← กำหนด Primary Key

    var controlFlowBuilder = builder.Entity<ControlFlowEntity>();
    controlFlowBuilder.ToTable("estm_control_flow");
    controlFlowBuilder.HasKey(x => new { x.key });
}
```

**ทำไมถึงดี:**
- **ชื่อ Table กำหนดใน Code** → ไม่ต้องพึ่ง Convention (ป้องกันชื่อ Table ผิด)
- **Primary Key กำหนดใน Fluent API** → ชัดเจนกว่า Data Annotation `[Key]`
- Entity Class สะอาด → มีแค่ Property ไม่มี Mapping Logic ปนอยู่
- รองรับ Composite Key ได้ง่าย (`HasKey(x => new { x.key1, x.key2 })`)

**เทียบกับ Repo อื่นๆ:**
- บาง Repo ใช้ Data Annotation ปนกับ Fluent API → สับสน
- บาง Repo ไม่กำหนด Table Name → พึ่ง Convention ที่อาจผิดพลาด

---

### ✅ เหตุผลที่ 7: มี Cross-Cutting Concerns ครบ

| Feature | Implementation | ไฟล์ |
|---------|---------------|------|
| **Authentication** | JWT Bearer Token | `Startup.cs` |
| **API Documentation** | Swagger (OpenAPI) | `Startup.cs` |
| **Logging** | `LogFilter` (Action Filter) | `Filters/` |
| **Health Check** | `HealthCheckController` | `Controllers/` |
| **Multiple Environments** | appsettings.{env}.json | Root |
| **Containerization** | DockerFile + deployment.yaml | Root |
| **Availability Check** | `AvailableFilter` | `Filters/` |

```csharp
// Startup.cs — Filter Registration
services.AddControllers(options =>
{
    options.Filters.Add(typeof(AvailableFilter));  // ← เช็คว่า API พร้อมให้บริการ
    options.Filters.Add(typeof(LogFilter));         // ← Log ทุก Request/Response
});
```

**ทำไมถึงดี:**
- มี Pattern สำหรับงานจริงครบ (Auth, Logging, Health Check, Docker)
- แยก Cross-Cutting Concerns ออกจาก Business Logic ผ่าน Filter
- มี Configuration สำหรับทุก Environment (Localhost, Dev, UAT, Production)

---

## 3. สรุปเปรียบเทียบกับ Repo อื่นที่สำรวจ

| เกณฑ์ | ESTATEMENT_API | Repo ทั่วไปที่พบ |
|-------|:--------------:|:----------------:|
| Layered Architecture | ✅ ชัดเจน 3 Layer | ❌ ปนกัน / ไม่แยก |
| Interface Abstraction | ✅ ครบทุก Layer | ⚠️ มีบ้างเฉพาะ Service |
| Unit of Work Pattern | ✅ สมบูรณ์ | ❌ ส่วนใหญ่ไม่มี |
| Multiple DbContext | ✅ มี 2 DbContext + Pool | ❌ มีแค่ 1 DbContext |
| Transaction Management | ✅ Begin/Commit/Rollback/IsolationLevel | ❌ ไม่มี / ใช้ Auto |
| Fluent API Configuration | ✅ ใช้ OnModelCreating | ⚠️ ใช้ Data Annotation ปน |
| DI Registration | ✅ เป็นระบบ กลุ่มชัดเจน | ⚠️ กระจัดกระจาย |
| JWT Authentication | ✅ | ⚠️ มีบ้าง |
| Swagger Documentation | ✅ พร้อม Security Def | ⚠️ มีบ้าง |
| Docker/K8s Ready | ✅ | ❌ ส่วนใหญ่ไม่มี |
| Multi-Environment Config | ✅ 4 Environments | ⚠️ มี 1-2 |
| Action Filter (Log/Avail) | ✅ | ❌ ส่วนใหญ่ไม่มี |

---

## 4. ข้อจำกัดที่ทราบ (Known Limitations)

แม้จะถูกเลือกเป็น Standard แต่ ESTATEMENT_API ก็มีจุดที่ควรปรับปรุง:

| ข้อจำกัด | รายละเอียด | แนวทางปรับปรุงใน Standard |
|-----------|-----------|--------------------------|
| Naming Convention | Property ใช้ camelCase (`public string key`) แทน PascalCase | Standard จะกำหนดให้ใช้ PascalCase |
| Empty try-catch | UoW มี `catch (Exception) { throw; }` ซ้ำทุก Method | Standard จะตัดออกหรือเพิ่ม Logging |
| Test Cases in Code | มี Hard-coded test data ใน Service (เช่น `if request.csnNumber == "11111111"`) | Standard จะกำหนดให้แยก Test ออกจาก Production Code |
| No Repository Pattern | ใช้ DbSet ตรงผ่าน UoW ไม่มี Repository Layer | พิจารณาเพิ่ม Generic Repository ใน Standard |
| No Async/Await | Service Method ส่วนใหญ่เป็น Synchronous | Standard จะกำหนดให้ใช้ Async เป็นค่าเริ่มต้น |
| ไม่มี Validation Layer | Validation อยู่ใน Service Method โดยตรง | พิจารณาใช้ FluentValidation |

---

## 5. บทสรุป

> **ESTATEMENT_API ถูกเลือกเพราะเป็น Repo เดียวที่ผ่านเกณฑ์ทั้ง 7 ข้อ**  
> จากการสำรวจ ~40+ Repos ใน ~25 Projects บน TFS Server

แม้จะมีข้อจำกัดบางจุด แต่ **Architecture Pattern ที่ใช้ถือว่าดีที่สุด** ในบรรดา Repo ทั้งหมดที่สำรวจ ได้แก่:

1. **Layered Architecture** → แยก Concern ชัดเจน
2. **Unit of Work** → จัดการ Transaction ได้สมบูรณ์
3. **Multiple DbContext** → รองรับหลาย Database
4. **Interface Abstraction** → Testable & Maintainable
5. **Organized DI** → เข้าใจง่าย เพิ่มเติมง่าย
6. **Fluent API** → Entity Configuration ที่ชัดเจน
7. **Production-Ready** → JWT, Swagger, Docker, Multi-Environment

ข้อจำกัดที่พบจะถูกแก้ไขใน Coding Standard โดยนำ Pattern ที่ดีมาใช้ พร้อมปรับปรุง Naming Convention และ Best Practices เพิ่มเติม

---

## ภาคผนวก: ข้อมูล Repository

| รายการ | ค่า |
|--------|-----|
| TFS Server | `http://siam-tfd-03:8080/tfs18/ITGroup_Collection` |
| Project | `ESTATEMENT_CLOUD` |
| Repository | `ESTATEMENT_API` |
| Branch | `master` |
| Framework | ASP.NET Core (.NET Core) |
| ORM | Entity Framework Core |
| Database | MySQL |
| UoW Package | `Easybuy.WebApplication.CoreUnitOfWork` |
| วันที่สำรวจ | กุมภาพันธ์ 2026 |
