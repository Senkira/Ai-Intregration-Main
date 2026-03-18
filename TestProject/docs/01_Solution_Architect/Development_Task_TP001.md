# Development Task — TP001

> **รหัสเอกสาร:** DT-TP001  
> **วันที่:** 18 มีนาคม 2026  
> **ผู้รับผิดชอบ:** AI Developer (Role 03)  
> **อ้างอิง:** Solution_Analysis_TP001.md

---

## §1 Objective

สร้าง **SmartBankStatement API** — Reference Implementation ตาม Coding Standard §1–§12 พร้อม Unit Test ที่ Build + Test ผ่าน เป็นตัวอย่างให้ Developer ใช้อ้างอิง

---

## §2 Scope of Work

### ✅ In Scope

| # | งาน | Coding Standard § |
|---|-----|:-----------------:|
| 1 | สร้าง Solution + Project Structure (src/tests) | §1 |
| 2 | สร้าง Models: Entity, Request, Response, ResultModel | §2 |
| 3 | สร้าง DataAccess Layer: IDbConnectionFactory, BaseRepository, Repositories, SqlQueries | §3 |
| 4 | สร้าง Services: Interface + Implementation (Guard, try-catch, ResultModel, DTO mapping) | §4 |
| 5 | สร้าง Controllers: Thin Controller, [ApiController], CancellationToken chain | §5 |
| 6 | สร้าง Program.cs: DI (Singleton/Scoped), JWT, Swagger, Filters | §6, §7, §10 |
| 7 | สร้าง appsettings 4 environments | §9 |
| 8 | สร้าง Filters: LogFilter, AvailableFilter | §1.6, §8 |
| 9 | สร้าง HealthCheckController | §5.6 |
| 10 | สร้าง Unit Tests: xUnit + NSubstitute, AAA, ≥1 test/method | §12 |
| 11 | SQLite DB initialization + seed admin user | Demo support |

### 🚫 Out of Scope

| # | สิ่งที่ห้ามทำ | เหตุผล |
|---|-------------|--------|
| OS-1 | ห้ามสร้าง FluentValidation | §15 แยก scope |
| OS-2 | ห้ามเพิ่ม CorrelationId Middleware | §16 Out of scope |
| OS-3 | ห้ามเพิ่ม API Versioning | §13 ใช้ action-based routing |
| OS-4 | ห้ามสร้าง Integration Test | §12.15 แยก task |
| OS-5 | ห้ามสร้าง Dockerfile / Jenkinsfile | §26 DevOps responsibility |
| OS-6 | ห้ามเพิ่ม Polly / Resilience | §18 ไม่มี external call |
| OS-7 | ห้ามใช้ EF Core / DbContext | §3.1 Dapper only |
| OS-8 | ห้ามใช้ BCrypt/Argon2 | Demo — SHA256 เพียงพอ |

---

## §3 Implementation Steps

### Block 1: Project Structure (§1) — Steps 1–5
1. สร้าง Solution `SmartBankStatement.slnx`
2. สร้าง Web API project `src/SmartBankStatementAPI/`
3. สร้าง xUnit project `tests/UnitTest/`
4. สร้างโฟลเดอร์ตาม §1.1–§1.7
5. เพิ่ม NuGet: Dapper, Sqlite, JWT Bearer, Swashbuckle

### Block 2: Models (§2) — Steps 6–9
6. สร้าง `ResultModel.cs` + `ResultModel<T>`
7. สร้าง Entities: `StatementEntity`, `CustomerEntity`, `UserEntity` (§2.27 suffix)
8. สร้าง Requests: `GetStatementRequest`, `CreateCustomerRequest`, `LoginRequest` (§2.29 prefix)
9. สร้าง Responses: `StatementResponse`, `CustomerResponse`, `LoginResponse`, `HealthCheckModel`

### Block 3: DataAccess (§3) — Steps 10–14
10. สร้าง `IDbConnectionFactory` + `DbConnectionFactory` (§3.3)
11. สร้าง `BaseRepository` wrap Dapper + CancellationToken + ConfigureAwait(false) (§3.4)
12. สร้าง Repository Interfaces (§3.5)
13. สร้าง SqlQueries constants — ห้าม SELECT * (§3.11)
14. สร้าง Repository Implementations extend BaseRepository

### Block 4: Services (§4) — Steps 15–16
15. สร้าง Service Interfaces
16. สร้าง Service Implementations: Guard (§4.5) → try-catch (§4.6) → DTO mapping (§4.7) → ResultModel (§8.5)

### Block 5: Controllers (§5) — Steps 17–18
17. สร้าง Controllers: ControllerBase (§5.1), Thin (§5.7), CancellationToken (§5.8), [Authorize] (§5.4)
18. สร้าง HealthCheckController — ไม่ต้อง [Authorize] (§5.6)

### Block 6: Infrastructure (§6, §7, §9, §10) — Steps 19–23
19. Program.cs: DI Singleton → Scoped (§6.3, §6.4)
20. JWT Bearer + ValidateAll + ClockSkew=Zero (§7)
21. Swagger + SecurityDefinition (§10)
22. LogFilter, AvailableFilter
23. appsettings 4 environments + SQLite init

### Block 7: Unit Tests (§12) — Steps 24–28
24. เพิ่ม NSubstitute
25. CustomerServiceTests — 7 tests (Guard, NotFound, Duplicate, Success, Exception)
26. StatementServiceTests — 5 tests
27. AuthServiceTests — 5 tests (Guard, WrongPw, NotFound, Inactive, Success)
28. Verify AAA (§12.9), Naming (§12.8), No DB (§12.10)

### Block 8: Verify — Steps 29–30
29. `dotnet build` → 0 errors
30. `dotnet test` → 0 failures

---

## §4 Acceptance Criteria

| AC# | Criteria | Testable Condition |
|:---:|---------|-------------------|
| AC1 | Build สำเร็จ | `Build succeeded` |
| AC2 | Test ผ่าน 100% | `failed: 0` |
| AC3 | Folder structure ตาม §1 | ทุกโฟลเดอร์ตาม standard มีอยู่จริง |
| AC4 | Dapper only (ไม่มี EF Core) | grep `DbContext` = 0 |
| AC5 | ทุก Service มี try-catch + ResultModel | ทุก public method return ResultModel |
| AC6 | Controller สืบทอด ControllerBase | grep ControllerBase = match |
| AC7 | HealthCheck ไม่ต้อง [Authorize] | HealthCheckController ไม่มี [Authorize] |
| AC8 | Unit Tests ≥ 15 | Test count ≥ 15 |
| AC9 | appsettings 4 files | File count = 4 |
| AC10 | CancellationToken chain ครบ | ทุก async method มี CT parameter |

---

> **หมายเหตุ:** เอกสารนี้กำหนด **หลักการ** เท่านั้น — ห้ามมี code (ตาม Role 01 restriction)
