# Coding Standard Checklist — __TASK_ID__

> **วันที่:** __DATE__  
> **วัตถุประสงค์:** Cross-check โค้ดจริงกับเกณฑ์ Coding Standard §1–§26

---

## สรุปผลรวม

| Category | ผ่าน | N/A | ไม่ผ่าน |
|----------|:----:|:---:|:------:|
| 🔴 Core §1–§9 | | | |
| 🟡 Recommended §10–§11 | | | |
| 🔴 Testing §12 | | | |
| **Total** | | | |

---

## §1 Project Structure

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 1.1 | 3 Layer: Controllers/, Services/, DataAccess/ | ⬜ |
| 1.2 | Models/ + Entities/ + Requests/ | ⬜ |
| 1.3 | Services/Interfaces/ | ⬜ |
| 1.4 | DataAccess/ → Connections/ + Repositories/ | ⬜ |
| 1.5 | Repositories/Interfaces/ | ⬜ |
| 1.6 | Filters/ | ⬜ |
| 1.7 | tests/ แยกจาก src/ | ⬜ |

## §2 Naming Convention

| Rule | ✅ |
|------|:--:|
| PascalCase Classes/Methods | ⬜ |
| _camelCase Private fields | ⬜ |
| Entity suffix | ⬜ |
| Request prefix: Action+Feature | ⬜ |
| Response suffix | ⬜ |
| Interface I prefix | ⬜ |
| Async suffix | ⬜ |

## §3 DataAccess

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 3.1 | Dapper only | ⬜ |
| 3.3 | IDbConnectionFactory | ⬜ |
| 3.4 | BaseRepository + ConfigureAwait(false) | ⬜ |
| 3.6 | Parameterized queries | ⬜ |
| 3.11 | ห้าม SELECT * | ⬜ |

## §4 Service

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 4.1 | Interface ทุก Service | ⬜ |
| 4.5 | Guard Clause | ⬜ |
| 4.6 | try-catch + structured log | ⬜ |
| 4.7 | Return DTO ไม่ใช่ Entity | ⬜ |
| 4.8 | ResultModel wrap | ⬜ |

## §5 Controller

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 5.1 | ControllerBase | ⬜ |
| 5.2 | [ApiController] + [Route] | ⬜ |
| 5.3 | Thin Controller | ⬜ |
| 5.6 | HealthCheckController | ⬜ |
| 5.8 | CancellationToken | ⬜ |

## §6 DI

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 6.3 | Singleton: stateless only | ⬜ |
| 6.4 | Scoped: Repo + Service | ⬜ |

## §7 Authentication

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 7.1 | JWT Bearer | ⬜ |
| 7.3 | ValidateAll 4 params | ⬜ |
| 7.4 | ClockSkew = Zero | ⬜ |

## §8 Error Handling

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 8.1 | try-catch ทุก method | ⬜ |
| 8.2 | Structured logging | ⬜ |
| 8.5 | ResultModel response | ⬜ |

## §9 Configuration

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 9.1 | appsettings.json base | ⬜ |
| 9.2 | 3+ Environment files | ⬜ |

## §12 Testing

| # | เกณฑ์ | ✅ |
|---|-------|:--:|
| 12.1 | Unit Test ทุก Service | ⬜ |
| 12.2 | xUnit | ⬜ |
| 12.3 | NSubstitute | ⬜ |
| 12.8 | Method_Scenario_Expected naming | ⬜ |
| 12.9 | AAA Pattern | ⬜ |
| 12.10 | ห้าม DB จริง | ⬜ |
