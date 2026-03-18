# Solution Analysis — TP001

> **รหัสเอกสาร:** SA-TP001  
> **วันที่:** 18 มีนาคม 2026  
> **ผู้จัดทำ:** AI Solution Architect (Role 01)  
> **Method:** TOT + COT + Block-Split-Chunk + 2-Table Validation + Audit State

---

## §1 Executive Summary

สร้าง **Reference Implementation API** ตาม Coding Standard ขององค์กร (295 เกณฑ์, 26 sections) เพื่อพิสูจน์ว่ามาตรฐานปฏิบัติตามได้จริง และเป็น template สำหรับ Developer ใหม่

**เป้าหมาย:** สร้าง ASP.NET Core 10 Web API + Dapper ที่ผ่านเกณฑ์ §1–§12 (Core Mandatory) พร้อมหลักฐาน Build + Unit Test

---

## §2 Requirement

| # | Requirement | Priority | Source |
|---|------------|:--------:|--------|
| R1 | โครงสร้าง 3-Layer ตาม §1 (Controllers, Services, DataAccess) | 🔴 High | Coding Standard §1 |
| R2 | Naming Convention ตาม §2 (PascalCase, _camelCase, suffix) | 🔴 High | Coding Standard §2 |
| R3 | DataAccess ใช้ Dapper + Repository Pattern ตาม §3 | 🔴 High | Coding Standard §3 |
| R4 | Service Layer ตาม §4 (Interface, Guard, try-catch, ResultModel) | 🔴 High | Coding Standard §4 |
| R5 | Controller Layer ตาม §5 (Thin, ControllerBase, CancellationToken) | 🔴 High | Coding Standard §5 |
| R6 | DI Registration ตาม §6 (Singleton/Scoped ถูกต้อง) | 🔴 High | Coding Standard §6 |
| R7 | JWT Authentication ตาม §7 | 🔴 High | Coding Standard §7 |
| R8 | Error Handling + Logging ตาม §8 | 🔴 High | Coding Standard §8 |
| R9 | Configuration Management ตาม §9 | 🔴 High | Coding Standard §9 |
| R10 | Unit Tests ตาม §12 (xUnit, Mock, AAA, naming) | 🔴 High | Coding Standard §12 |
| R11 | Swagger API Documentation ตาม §10 | 🟡 Medium | Coding Standard §10 |
| R12 | HealthCheck Endpoint ตาม §5.6 | 🔴 High | Coding Standard §5 |

---

## §3 Options Analysis

### Solution Table (Block-Split-Chunk)

| Block | Task | Method | Dependencies |
|:-----:|------|--------|:------------:|
| **B1** | Project Structure | .NET 10 CLI → solution + src/tests layout | None |
| **B2** | Models Layer | Entity, Request, Response, ResultModel | B1 |
| **B3** | DataAccess Layer | Dapper + BaseRepository + SqlQueries + IDbConnectionFactory | B1, B2 |
| **B4** | Service Layer | Interface + Implementation (Guard, try-catch, structured log) | B2, B3 |
| **B5** | Controller Layer | Thin Controller + [ApiController] + CancellationToken chain | B4 |
| **B6** | Infrastructure | Program.cs (DI, JWT, Swagger, Filters) + appsettings | B3, B4, B5 |
| **B7** | Unit Tests | xUnit + NSubstitute + AAA Pattern | B4 |
| **B8** | Documentation | Project Summary + AI Role Pipeline handoff documents | B1–B7 |

### Options Comparison

| # | Solution | Method | Pros | Cons | Score |
|---|----------|--------|------|------|:-----:|
| 1 | **Full Standard Implementation** | Dapper + 3-Layer + xUnit | ✅ พิสูจน์ Standard จริง ✅ Testable ✅ Copy ได้ | ⏱ ใช้เวลามาก | **9/10** |
| 2 | Minimal Skeleton — placeholder only | Empty classes + TODO | ⏱ เร็ว | ❌ ไม่พิสูจน์อะไร | 3/10 |
| 3 | Copy ESTATEMENT_API แก้ชื่อ | Fork + rename | ⏱ เร็ว | ❌ ผูกกับ business logic จริง | 5/10 |

---

## §4 Selected Solution & Rationale

### ✅ เลือก Option 1: Full Standard Implementation

| เหตุผล |
|--------|
| 1. พิสูจน์ได้จริง — Build + 18 tests = หลักฐานว่า Standard ใช้ได้ |
| 2. Self-contained — SQLite ไม่ต้อง setup DB → ทุกคน demo ได้ |
| 3. Business Domain ชัด — eStatement (Customer, Statement, Auth) ใกล้เคียงงานจริง |
| 4. Testable — ทุก layer มี Interface → mock ได้ → unit test ได้ |
| 5. Traceable — ทุกไฟล์ comment อ้างอิง § ของ Coding Standard |

### Verification Table

| Criteria | ✅/❌ | Evidence |
|----------|:----:|---------|
| Completeness | ✅ | ครอบคลุม §1–§12 ที่เป็น 🔴 บังคับ |
| Appropriateness | ✅ | Dapper (ไม่ใช่ EF Core) ตรงกับ §3.1 |
| Impact | ✅ | Self-contained — ไม่กระทบ production |
| Architecture Fit | ✅ | 3-Layer ตาม Microsoft reference |
| Simplicity | ✅ | 3 controllers, 3 services — ไม่ over-engineer |

### Audit State

| # | Audit Point | PASS/FAIL |
|---|------------|:---------:|
| 1 | Completeness — ครอบคลุมทุก requirement | ✅ PASS |
| 2 | Appropriateness — เทคโนโลยีถูกต้อง | ✅ PASS |
| 3 | Impact — ไม่กระทบระบบอื่น | ✅ PASS |
| 4 | Architecture Fit — ตาม pattern ขององค์กร | ✅ PASS |
| 5 | Simplicity — ไม่ซับซ้อนเกินจำเป็น | ✅ PASS |
