# Glossary — สารบัญคำศัพท์เฉพาะ

> รวมคำศัพท์เทคนิคที่ใช้ในเอกสาร Research และ Coding Standard ทั้งหมด  
> อ้างอิง: [Database_Access_Methods_NET10.md](./Database_Access_Methods_NET10.md) | [Dapper_Migration_Analysis.md](./Dapper_Migration_Analysis.md) | [Coding_Standard_Criteria.md](../Coding_Standard_Criteria.md)  
> **วันที่:** 5 มีนาคม 2026

---

## หมวดที่ 1 — ประเภท Data Access / Library

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **ADO.NET** | ActiveX Data Objects for .NET | Library พื้นฐานสุดของ .NET สำหรับเชื่อมต่อ DB — ทุก Library อื่นสร้างบนนี้ | Low-level DB Access |
| **ORM** | Object-Relational Mapper | เครื่องมือที่ Map ข้อมูลจาก Database Table ↔ C# Object อัตโนมัติ | EF Core, NHibernate |
| **Micro-ORM** | Micro Object-Relational Mapper | ORM ขนาดเล็ก — Map Object ได้ แต่ SQL ยังเขียนเอง | Dapper, RepoDB |
| **Full ORM** | Full Object-Relational Mapper | ORM เต็มรูปแบบ — สร้าง SQL ให้อัตโนมัติ มี Migration | EF Core, NHibernate |
| **EF Core** | Entity Framework Core | Full ORM ของ Microsoft สำหรับ .NET | ปัจจุบันใช้ใน RL Series |
| **Dapper** | — | Micro-ORM สร้างโดย Stack Overflow — SQL เขียนเอง แต่ Map Object อัตโนมัติ | เสนอให้ใช้แทน EF Core |
| **RepoDB** | — | Micro-ORM ที่อยู่กลางระหว่าง Dapper และ EF Core | ทางเลือก |
| **NHibernate** | — | Full ORM ที่ Port มาจาก Java Hibernate | ไม่แนะนำสำหรับ Project ใหม่ |
| **SqlKata** | — | Query Builder — สร้าง SQL จาก Fluent API แล้วรันผ่าน Dapper | ทางเลือก |
| **Dapper.Contrib** | — | Extension ของ Dapper ที่เพิ่ม CRUD อัตโนมัติ (ไม่ต้องเขียน SQL) | เสริม Dapper |
| **Dapper.FluentMap** | — | Extension ของ Dapper สำหรับ Map ชื่อ Column ↔ Property | เสริม Dapper |

---

## หมวดที่ 2 — Architecture Pattern

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **UoW** | Unit of Work | Pattern ที่รวม DB Operations ไว้ด้วยกัน แล้ว Commit/Rollback ทีเดียว | EF Core Pattern ปัจจุบัน |
| **UoW Pool** | Unit of Work Pool | UoW ที่รองรับหลาย DB Context ในโปรเจคเดียวกัน | ใช้กับ Multi-DB Project |
| **Repository Pattern** | — | Pattern ที่แยกชั้น Data Access ออกจาก Business Logic ผ่าน Interface | Dapper Pattern ที่เสนอ |
| **DAO** | Data Access Object | Object ที่รับผิดชอบการเข้าถึง DB โดยตรง | ADO.NET Pattern |
| **DI** | Dependency Injection | การส่ง Object ที่ต้องใช้เข้าไปทาง Constructor แทนการสร้างเอง | ทุก Pattern |
| **IoC** | Inversion of Control | หลักการที่ Framework จัดการ Object lifecycle แทน Code | ASP.NET Core DI Container |
| **3-Layer Architecture** | — | แบ่ง Code เป็น 3 ชั้น: Controller → Service → DataAccess | Coding Standard บังคับ |
| **Hybrid Approach** | — | ใช้ 2 Library ร่วมกัน เช่น EF Core + Dapper ในโปรเจคเดียว | ทางเลือก Option B |
| **Connection Factory** | — | Object ที่รับผิดชอบสร้าง DB Connection — ซ่อน Connection String | Dapper Pattern |
| **Base Repository** | — | Repository กลางที่มี Method ทั่วไป (Query, Execute, Transaction) ให้ Repository อื่น Inherit | Dapper Pattern |

---

## หมวดที่ 3 — .NET / C# Concepts

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **BCL** | Base Class Library | Library พื้นฐานของ .NET ที่มาพร้อมกับ Runtime (ไม่ต้องติดตั้งเพิ่ม) | ADO.NET อยู่ใน BCL |
| **AOT** | Ahead-of-Time Compilation | คอมไพล์ Code ก่อน Runtime — ลด Startup time, เหมาะกับ Container | EF Core 10, .NET 10 |
| **JIT** | Just-in-Time Compilation | คอมไพล์ Code ตอน Runtime (ค่าเริ่มต้น) | .NET ทั่วไป |
| **LINQ** | Language Integrated Query | Syntax ใน C# สำหรับ Query ข้อมูลแบบ Type-safe | EF Core |
| **Async/Await** | Asynchronous Programming | Pattern สำหรับเขียน Code แบบ Non-blocking — ไม่ Block Thread ขณะรอ DB | ทุก Library |
| **IDbConnection** | — | Interface มาตรฐานของ .NET สำหรับ DB Connection | Dapper ทำงานบน Interface นี้ |
| **DbContext** | Database Context | Class ของ EF Core ที่เป็นตัวแทนของ DB Session และ Change Tracker | EF Core |
| **DbSet** | — | Collection ที่แทน Table ใน EF Core — ใช้ Query ผ่าน LINQ | EF Core |
| **IL Emit** | Intermediate Language Emit | เทคนิคสร้าง Code ณ Runtime ด้วย IL — Dapper ใช้ทำ Mapping เร็วกว่า Reflection | Dapper internals |
| **Reflection** | — | กลไกของ .NET ที่ใช้อ่าน Type/Property ณ Runtime — ช้ากว่า IL Emit | ORM ทั่วไป |
| **Expression Tree** | — | โครงสร้างข้อมูลที่แทน LINQ Query — EF Core ใช้ Compile เป็น SQL | EF Core internals |
| **NuGet** | — | Package Manager ของ .NET — ใช้ติดตั้ง Library | ทุก Library |
| **Transitive Dependency** | — | Library ที่ถูก Install มาโดยอัตโนมัติ เพราะ Library ที่เราใช้ต้องการ | EF Core มี ~15 packages |
| **File-Scoped Namespace** | — | Syntax C# 10+ ที่ประกาศ Namespace โดยไม่ต้องใช้ `{}` — ลด Nesting 1 Level | ทุก `.cs` file |
| **Primary Constructor** | — | C# 12+ Feature — กำหนด Parameter ใน Class Declaration โดยไม่ต้อง Constructor Body | Service, Middleware |
| **ConfigureAwait(false)** | — | Method ที่บอก Async ว่าไม่ต้อง Capture Context กลับมา — ป้องกัน Deadlock | Library code |
| **Attribute** | — | Metadata ที่ติดบน Class/Method/Property ด้วย `[...]` เช่น `[Table]`, `[Key]`, `[Required]` | Validation, Dapper.Contrib |
| **ConcurrentDictionary** | — | Dictionary แบบ Thread-safe — Dapper ใช้เก็บ Cache ของ Query Mapping | Dapper internals, Concurrency |
| **Span\<T\>** | — | โครงสร้างข้อมูลใน .NET ที่อ้างอิง Memory โดยไม่ต้อง Allocate ใหม่ — ลด GC Pressure | .NET 8+ Performance |

---

## หมวดที่ 4 — Database Concepts

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **SP / Stored Procedure** | — | ชุดคำสั่ง SQL ที่บันทึกไว้ใน DB Server — เรียกใช้ด้วยชื่อ | ใช้ใน RL Series |
| **Migration** | DB Migration | กระบวนการ Version Control ของ DB Schema — EF Core สร้าง SQL Script ให้อัตโนมัติ | EF Core feature |
| **Schema** | Database Schema | โครงสร้างของ DB: Table, Column, Index, Constraint | |
| **Transaction** | — | กลุ่มคำสั่ง SQL ที่ต้อง Commit ทั้งหมดหรือ Rollback ทั้งหมด — ป้องกันข้อมูลเสียหาย | ทุก Library |
| **Commit** | — | ยืนยันการ Save ทุก Operation ใน Transaction ลง DB | Transaction |
| **Rollback** | — | ยกเลิกทุก Operation ใน Transaction — คืนค่า DB กลับสู่สถานะก่อนหน้า | Transaction |
| **Connection String** | — | String ที่ระบุ Server, Database, Authentication สำหรับเชื่อมต่อ DB | ทุก Library |
| **Connection Pool** | — | กลุ่ม DB Connection ที่ถูก Reuse — ลด Overhead การสร้าง Connection ใหม่ | SQL Server default |
| **Index** | — | โครงสร้างข้อมูลใน DB ที่เร่งความเร็วการ Query | Performance Tuning |
| **Primary Key** | PK | Column ที่ระบุ Row ได้ไม่ซ้ำกัน | Entity ทุกตัวต้องมี |
| **Foreign Key** | FK | Column ที่อ้างอิง Primary Key ของ Table อื่น | Relationship |
| **JOIN** | — | คำสั่ง SQL ที่รวมข้อมูลจากหลาย Table | Dapper ต้องเขียนเอง |
| **Parameterized Query** | — | Query ที่ใช้ `@parameter` แทน String Interpolation — ป้องกัน SQL Injection | Security Best Practice |
| **SQL Injection** | — | การโจมตีที่แทรก SQL Command เข้าไปใน Query — อันตรายถ้าใช้ String Interpolation | Security Risk |
| **Bulk Operation** | — | การ Insert/Update/Delete ข้อมูลจำนวนมากในครั้งเดียว — เร็วกว่า Loop | Performance |
| **DBA** | Database Administrator | ผู้ดูแลและออกแบบ Database, Index, Stored Procedure | |
| **TVP** | Table-Valued Parameter | Parameter ใน SQL Server ที่ส่ง Table Data เข้า SP/Query ได้ | Advanced SQL Server |
| **MARS** | Multiple Active Result Sets | Feature ของ SQL Server ที่รัน Query หลายตัวพร้อมกันบน Connection เดียว | SQL Server |
| **SqlBulkCopy** | — | Class ใน ADO.NET สำหรับ Bulk Insert ข้อมูลจำนวนมาก (ล้าน Row) เข้า SQL Server | Bulk Insert |
| **Transient Error** | — | Error ชั่วคราวจาก Network/DB ไม่เสถียร — สามารถ Retry แล้วสำเร็จได้ | Retry Policy, Deadlock |

---

## หมวดที่ 5 — Performance & Memory

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Performance** | — | ประสิทธิภาพการทำงาน — วัดจาก Response Time, Throughput | เหตุผลหลักที่เลือก Dapper |
| **Overhead** | — | ค่าใช้จ่ายเพิ่มเติมที่เกิดจาก Library/Framework — ไม่ใช่ Logic ของ Application | EF Core มี Overhead สูงกว่า |
| **Change Tracking** | — | กลไกของ EF Core ที่ติดตามว่า Property ไหนถูกแก้ไข เพื่อสร้าง UPDATE ที่ถูกต้อง | EF Core เท่านั้น |
| **Memory Overhead** | — | หน่วยความจำที่ Library ใช้เพิ่มจาก Application จริง — EF Core ใช้มากกว่าเพราะ Change Tracking | |
| **Hot Path** | — | ส่วน Code ที่ถูกเรียกบ่อยมาก — ต้องการ Optimize เป็นพิเศษ | Compiled Queries |
| **Compiled Query** | — | Query ที่ถูก Pre-compile ไว้ล่วงหน้า — ลด Overhead การ Parse LINQ ทุกครั้ง | EF Core 10 feature |
| **Benchmark** | — | การวัดและเปรียบเทียบ Performance ระหว่าง Option ต่างๆ | ตารางเปรียบเทียบในเอกสาร |
| **Throughput** | — | จำนวน Request ที่ระบบรองรับได้ต่อวินาที | Performance metric |
| **Latency** | — | เวลาที่ใช้ตอบกลับ Request หนึ่งครั้ง (ms) | Performance metric |
| **Read-heavy** | — | ระบบที่มีสัดส่วน Read (SELECT) มากกว่า Write | เหตุผลเลือก Dapper |
| **Write-heavy** | — | ระบบที่มีสัดส่วน Write (INSERT/UPDATE/DELETE) มากกว่า Read | EF Core เหมาะกว่า |
| **GC** | Garbage Collection | กลไกของ .NET ที่จัดการ Memory อัตโนมัติ — Release Object ที่ไม่ใช้งาน | .NET Runtime |
| **GC Pressure** | — | สถานการณ์ที่สร้าง Object มากเกินไป ทำให้ GC ทำงานบ่อย → กิน CPU | EF Core vs Dapper |
| **JIT PGO** | Profile-Guided Optimization | เทคนิค .NET Runtime ที่ Optimize Code ตาม Profile จริง ณ Runtime | .NET 8+ |
| **Cold Start** | — | การเรียกใช้ครั้งแรกที่ยังไม่มี Cache — ช้ากว่าครั้งถัดไป | Dapper IL Compilation |
| **Warm Cache** | — | การเรียกใช้ครั้งถัดไปหลัง Cache แล้ว — เร็วกว่า Cold Start มาก | Dapper Subsequent Calls |
| **Memory Allocation** | — | การจอง Memory สำหรับ Object ใหม่ — Dapper ~1-5 KB/Query, ADO.NET ~100-500 bytes | Performance Comparison |

---

## หมวดที่ 6 — Design & Code Quality

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Boilerplate** | — | Code ซ้ำๆ ที่ต้องเขียนทุกครั้งโดยไม่เพิ่ม Business Value | ADO.NET มี Boilerplate สูง |
| **Type Safety** | — | ระบบที่ Compiler ตรวจสอบ Type ให้ก่อน Run — ลด Runtime Error | LINQ ใน EF Core |
| **Fluent API** | — | รูปแบบ API ที่ Method เรียกต่อกันได้ (Method Chaining) อ่านง่าย | EF Core OnModelCreating, SqlKata |
| **Fluent NHibernate** | — | Extension ของ NHibernate ที่แทน XML Configuration ด้วย C# | NHibernate |
| **Interface** | — | สัญญาที่กำหนดว่า Class ต้อง Implement Method อะไรบ้าง — ไม่มี Implementation | `ICustomerRepository` |
| **Abstract Class** | — | Class ที่ไม่สามารถสร้าง Instance ได้โดยตรง — ต้อง Inherit และ Implement | `BaseRepository` |
| **Generic** | — | Code ที่ทำงานกับ Type หลายอย่างโดยใช้ `<T>` — ลด Code ซ้ำ | `BaseRepository<T>` |
| **PascalCase** | — | รูปแบบตั้งชื่อที่ทุกคำขึ้นต้นด้วยตัวพิมพ์ใหญ่ | `CustomerRepository` |
| **camelCase** | — | รูปแบบตั้งชื่อที่คำแรกพิมพ์เล็ก คำถัดไปขึ้นต้นพิมพ์ใหญ่ | `customerId` |
| **Namespace** | — | ชื่อกลุ่มของ Class — ใช้จัดระเบียบ Code และป้องกันชื่อชน | `eStatementAPI.DataAccess.MainDb` |
| **Separation of Concerns** | SoC | หลักการแยก Code ตามหน้าที่ — Controller ไม่ควรมี SQL, Service ไม่ควรมี HTTP Logic | 3-Layer Architecture |
| **Single Responsibility** | SRP | Class หนึ่งควรมีหน้าที่เดียว | SOLID Principle |
| **Coupling** | — | ระดับที่ Component ต้องพึ่งพากัน — ยิ่ง Loose Coupling ยิ่งดี | Repository Pattern ลด Coupling |
| **Refactor** | — | การปรับปรุง Code Structure โดยไม่เปลี่ยน Behavior | Migration Plan Phase 3 |
| **Migration (Code)** | — | กระบวนการเปลี่ยน Code จาก Pattern หนึ่งไปอีก Pattern | EF Core → Dapper |

---

## หมวดที่ 7 — Infrastructure & DevOps

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Container** | — | Package ที่รวม Application + Dependencies ให้ Run ได้ทุกที่ | Docker |
| **Docker** | — | Platform สำหรับสร้างและรัน Container | Dockerfile ใน RL Series |
| **Startup Time** | — | เวลาที่ Application ใช้ในการเริ่มทำงาน — AOT ช่วยลดได้ | .NET 10, EF Core 10 |
| **Internal NuGet Server** | — | Server ภายในองค์กรสำหรับ Host NuGet Package | `Easybuy.CoreUnitOfWork` |
| **Build Risk** | — | ความเสี่ยงที่ Build จะล้มเหลว เช่น Package หาไม่เจอ | Internal Library Risk |
| **DbUp** | — | Tool สำหรับ Run SQL Script Migration แทน EF Migration | ใช้ถ้าเปลี่ยนไป Dapper |
| **Flyway** | — | Database Migration Tool จาก Redgate — รองรับหลาย DB | ทางเลือกแทน EF Migration |
| **Side-by-side** | — | การรัน 2 Implementation พร้อมกันชั่วคราว ระหว่าง Migration | Rollback Strategy |
| **Rollback Plan** | — | แผนสำรองเมื่อ Migration มีปัญหา — กลับไปใช้ Version เดิม | Migration Risk Management |
| **SonarQube** | — | Platform วิเคราะห์ Code Quality — ตรวจ Bug, Code Smell, Vulnerability, Complexity | Code Quality |
| **OpenTelemetry** | — | มาตรฐาน Open Source สำหรับ Observability: Tracing, Metrics, Logging | Monitoring |
| **CVE** | Common Vulnerabilities and Exposures | รหัสมาตรฐานสำหรับระบุ Security Vulnerability ที่ค้นพบ | Dependency Security |
| **.editorconfig** | — | ไฟล์ Configuration กำหนด Code Style Rules ให้ IDE บังคับใช้ร่วมกันทั้ง Team | Code Formatting |
| **TFS** | Team Foundation Server | ระบบ Version Control และ CI/CD ของ Microsoft | DevOps |

---

## หมวดที่ 8 — SQL & Query Terms

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **CRUD** | Create, Read, Update, Delete | 4 Operation พื้นฐานของการจัดการข้อมูล | ทุก Data Access |
| **SELECT** | — | SQL Command สำหรับ Read ข้อมูล | Query |
| **INSERT** | — | SQL Command สำหรับเพิ่มข้อมูลใหม่ | Create |
| **UPDATE** | — | SQL Command สำหรับแก้ไขข้อมูล | Update |
| **DELETE** | — | SQL Command สำหรับลบข้อมูล | Delete |
| **EXEC** | Execute | SQL Command สำหรับรัน Stored Procedure | SP Call |
| **Multi-mapping** | — | ความสามารถของ Dapper ที่ Map ผล JOIN หนึ่ง Row → หลาย Object | Dapper feature |
| **Multi-result** | — | ความสามารถของ Dapper ที่รับผล SELECT หลาย Statement ในครั้งเดียว | `QueryMultiple` |
| **Alias** | AS | การตั้งชื่อแทนให้ Column ใน SQL เพื่อให้ตรงกับ Property Name | Dapper Column Mapping |
| **Column Map** | — | การกำหนดความสัมพันธ์ ชื่อ Column ↔ Property Name | Dapper.FluentMap |
| **Eager Loading** | — | โหลด Related Data ทันทีพร้อมกับ Main Query ด้วย JOIN/Include | EF Core `.Include()` |
| **Lazy Loading** | — | โหลด Related Data เมื่อเข้าถึง Property นั้นครั้งแรก | EF Core |
| **N+1 Problem** | — | ปัญหา Performance ที่เกิดจากการ Query N ครั้งสำหรับ N Records — ใช้ Eager Loading แก้ | EF Core Risk |
| **Query Builder** | — | API สำหรับสร้าง SQL แบบ Programmatic โดยไม่ต้อง Concatenate String | SqlKata |
| **Raw SQL** | — | SQL ที่เขียนตรงๆ เป็น String ใน Code | `.FromSqlRaw()`, Dapper |
| **String Interpolation** | — | การใช้ `$"..."` แทนค่าตัวแปรใน String — ห้ามใช้ใน SQL (SQL Injection Risk) | Security Rule D.1 |

---

## หมวดที่ 9 — ASP.NET Core & Web API

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **ASP.NET Core** | — | Framework ของ Microsoft สำหรับสร้าง Web Application / API บน .NET | โปรเจค RL Series ทั้งหมด |
| **Web API** | Web Application Programming Interface | API ที่รับ/ส่งข้อมูลผ่าน HTTP — มักใช้ JSON เป็น Format | โปรเจค RL Series ทั้งหมด |
| **Controller** | — | Class ที่รับ HTTP Request แล้วส่งต่อไปยัง Service — ชั้นบนสุดของ 3-Layer | `CustomerController` |
| **Service** | — | Class ที่ประมวลผล Business Logic — อยู่กลางระหว่าง Controller และ DataAccess | `CustomerService` |
| **Endpoint** | — | URL ที่รับ HTTP Request แต่ละ Request | `GET /api/customer/{id}` |
| **HTTP Method** | — | ประเภทคำสั่ง HTTP: GET, POST, PUT, DELETE, PATCH | RESTful API |
| **Middleware** | — | Code ที่รันระหว่าง Request เข้า → Response ออก — ใช้ทำ Logging, Auth, Error Handling | ASP.NET Core Pipeline |
| **Action Filter** | — | Middleware ระดับ Controller/Method — รันก่อน/หลัง Action Method | `LogFilter`, `AvailableFilter` |
| **Swagger / OpenAPI** | — | เครื่องมือสร้าง API Documentation อัตโนมัติจาก Code — มี UI ทดสอบ API ได้ | `Swashbuckle.AspNetCore` |
| **appsettings.json** | — | ไฟล์ Configuration ของ ASP.NET Core — เก็บ Connection String, JWT Secret ฯลฯ | ทุกโปรเจค |
| **Program.cs** | — | Entry point ของ ASP.NET Core App — ลงทะเบียน DI, Middleware ทั้งหมด | ทุกโปรเจค |
| **Startup.cs** | — | ไฟล์ Configuration เก่า (ก่อน .NET 6) — ปัจจุบันรวมเข้า Program.cs แล้ว | โปรเจค .NET เก่า |
| **Health Check** | — | Endpoint พิเศษที่ตอบสถานะ "ระบบทำงานปกติ" — ใช้กับ Kubernetes Liveness Probe | `HealthCheckController` |
| **Route** | — | Pattern URL ที่ Map ไปยัง Controller/Action | `[Route("api/[controller]")]` |
| **Model Binding** | — | กระบวนการ Map HTTP Request Body/Query String → C# Object อัตโนมัติ | ASP.NET Core |
| **Validation** | — | ตรวจสอบความถูกต้องของข้อมูลก่อนประมวลผล | `[Required]`, `[MaxLength]` |

---

## หมวดที่ 10 — Security & Authentication

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Authentication** | AuthN | กระบวนการพิสูจน์ตัวตน — "คุณเป็นใคร?" | JWT, API Key |
| **Authorization** | AuthZ | กระบวนการตรวจสอบสิทธิ์ — "คุณทำอะไรได้บ้าง?" | Role, Policy |
| **JWT** | JSON Web Token | Token มาตรฐานสำหรับส่งข้อมูล Authentication — มี 3 ส่วน: Header.Payload.Signature | API Security |
| **Bearer Token** | — | JWT ที่ส่งใน HTTP Header `Authorization: Bearer {token}` | API Security |
| **API Key** | — | Key ลับที่ส่งใน Header เพื่อยืนยันตัวตน Application | Service-to-Service |
| **HTTPS** | HyperText Transfer Protocol Secure | HTTP ที่เข้ารหัสด้วย TLS/SSL — ป้องกันการดักข้อมูล | Production Environment |
| **Parameterized Query** | — | วิธีป้องกัน SQL Injection โดยใช้ `@param` แทนการต่อ String | Dapper, ADO.NET |
| **SQL Injection** | — | การโจมตีระบบโดยแทรก SQL Command ผ่าน Input ที่ไม่ได้ Sanitize | Security Risk |

---

## หมวดที่ 11 — Scoped / Lifecycle ของ DI

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Singleton** | — | DI Lifetime ที่สร้าง Object ครั้งเดียวตลอด Application lifetime — ใช้ร่วมกันทุก Request | `IDbConnectionFactory` |
| **Scoped** | — | DI Lifetime ที่สร้าง Object ใหม่ 1 ครั้งต่อ 1 HTTP Request — ทำลายเมื่อ Request จบ | Repository, Service |
| **Transient** | — | DI Lifetime ที่สร้าง Object ใหม่ทุกครั้งที่ถูก Inject | Utility Class |
| **Object Lifecycle** | — | ช่วงอายุของ Object ตั้งแต่สร้างจนถึงทำลาย | DI Container จัดการ |
| **Dispose** | — | กระบวนการ Release Resource (Memory, Connection) เมื่อไม่ใช้งาน | `using` statement |
| **using statement** | — | Syntax ใน C# ที่เรียก `Dispose()` อัตโนมัติเมื่อออกจาก Block | Connection Management |
| **IDisposable** | — | Interface ที่กำหนดว่า Object นั้น Disposable — ต้องมี `Dispose()` Method | DB Connection |

---

## หมวดที่ 12 — Code Mapping & Migration Terms

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **As-Is** | — | สถานะปัจจุบัน — สภาพที่เป็นอยู่ก่อนเปลี่ยนแปลง | Migration Analysis |
| **To-Be** | — | สถานะเป้าหมาย — สภาพที่ต้องการหลังเปลี่ยนแปลง | Migration Analysis |
| **Navigation Property** | — | Property ใน EF Core Entity ที่ชี้ไปยัง Related Entity อื่น | `Customer.Transactions` |
| **Entity** | — | C# Class ที่ Map ตรงกับ DB Table ใน EF Core | `CustomerEntity` |
| **DTO** | Data Transfer Object | Object ที่ใช้รับ/ส่งข้อมูลระหว่าง Layer — ไม่ใช่ DB Entity | `RequestModel`, `ResponseModel` |
| **Model** | — | C# Class ที่ใช้รับ/ส่งข้อมูลใน API — ทั้ง Request/Response | `CustomerModel` |
| **Mapping** | — | การแปลงข้อมูลจาก Format หนึ่งไปอีก Format เช่น DB Row → C# Object | ORM Core feature |
| **Rewrite** | — | การเขียน Code ใหม่ทั้งหมด เปลี่ยน Implementation แต่ผล Output เหมือนเดิม | Migration Phase |
| **Implement** | — | การเขียน Code ให้เป็นไปตาม Interface ที่กำหนด | Repository Implementation |
| **Inject** | — | การส่ง Dependency เข้าไปใน Class ผ่าน DI Container | Constructor Injection |
| **Register** | — | การแจ้งให้ DI Container รู้จัก Interface กับ Implementation ที่ใช้ | `Program.cs` |
| **Scope** | — | ขอบเขตของ Variable หรือ Object — กำหนดว่าใช้ได้ที่ไหน | DI Lifetime |
| **PackageReference** | — | XML Element ใน `.csproj` สำหรับระบุ NuGet Package ที่ต้องการ | `.csproj` file |
| **Down Script** | — | SQL Script สำหรับ Undo Migration — กลับ Schema ไปเวอร์ชันก่อนหน้า | Migration Rollback |
| **Integration Test** | — | การทดสอบที่ครอบคลุมหลาย Component รวมถึง DB จริง | Testing Phase |
| **Unit Test** | — | การทดสอบ Component เดียวแบบ Isolated — Mock Dependency อื่น | Testing Phase |

---

## หมวดที่ 13 — JSON & Data Format

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **JSON** | JavaScript Object Notation | Format ข้อมูลที่ใช้แพร่หลายใน API — `{ "key": "value" }` | HTTP Request/Response |
| **JSON Column** | — | Column ใน DB ที่เก็บข้อมูล JSON — EF Core 10 Map เป็น C# Object ได้ | EF Core 10 feature |
| **Serialization** | — | แปลง C# Object → JSON String สำหรับส่งผ่าน HTTP | Response |
| **Deserialization** | — | แปลง JSON String → C# Object เมื่อรับ HTTP Request | Request Body |
| **appsettings.{Environment}.json** | — | ไฟล์ Config แยกตาม Environment เช่น Development, UAT, Production | Config Management |

---

## หมวดที่ 14 — Keyword เฉพาะใน Code

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **AsNoTracking()** | — | Method ของ EF Core ที่ปิด Change Tracking — Query เร็วขึ้น ใช้สำหรับ Read-only | EF Core Optimization |
| **SaveChangesAsync()** | — | Method ของ EF Core ที่ Commit การเปลี่ยนแปลงทั้งหมดลง DB | EF Core Write |
| **FindAsync()** | — | Method ของ EF Core ที่ค้นหา Entity ด้วย Primary Key | EF Core |
| **FirstOrDefaultAsync()** | — | Method ของ LINQ ที่คืน Element แรก หรือ null ถ้าไม่พบ | EF Core / LINQ |
| **QuerySingleOrDefaultAsync()** | — | Method ของ Dapper ที่คืน Object เดียว หรือ null ถ้าไม่พบ | Dapper |
| **QueryAsync()** | — | Method ของ Dapper ที่คืน Collection ของ Object | Dapper |
| **ExecuteAsync()** | — | Method ของ Dapper สำหรับ INSERT/UPDATE/DELETE — คืนจำนวน Row ที่ถูกกระทบ | Dapper |
| **QueryMultipleAsync()** | — | Method ของ Dapper ที่รัน SQL หลาย Statement พร้อมกัน | Dapper Multi-result |
| **CommandType.StoredProcedure** | — | Enum ที่บอก Dapper ว่าชื่อที่ส่งไปคือ Stored Procedure ไม่ใช่ SQL String | Dapper SP Call |
| **ExecuteUpdate()** | — | Method ใหม่ใน EF Core 8+ สำหรับ UPDATE โดยไม่ต้อง Load Entity ก่อน | EF Core Bulk Update |
| **ExecuteDelete()** | — | Method ใหม่ใน EF Core 8+ สำหรับ DELETE โดยไม่ต้อง Load Entity ก่อน | EF Core Bulk Delete |
| **SqlQuery\<T\>()** | — | Method ใน EF Core 8+ ที่รัน Raw SQL แล้ว Map ผลไปยัง Arbitrary Type (ไม่ต้องเป็น Entity) | EF Core Raw SQL |
| **FromSqlRaw()** | — | Method ของ EF Core สำหรับรัน Raw SQL บน DbSet — ต้อง Return Entity Type | EF Core Raw SQL (เก่า) |
| **OnModelCreating()** | — | Method ของ EF Core DbContext ที่ใช้กำหนด Entity Configuration ด้วย Fluent API | EF Core Configuration |
| **ToTable()** | — | Fluent API Method สำหรับกำหนดชื่อ Table ใน DB ที่ Entity นี้ Map ไปยัง | EF Core Entity Config |
| **HasKey()** | — | Fluent API Method สำหรับกำหนด Primary Key ของ Entity | EF Core Entity Config |
| **static class** | — | Class ที่ไม่สามารถสร้าง Instance ได้ — ใช้เก็บ Constant หรือ Utility Method | `CustomerQueries` SQL Class |
| **const** | — | ตัวแปรที่ค่าไม่เปลี่ยนแปลง กำหนด ณ Compile Time | SQL String Constants |
| **readonly** | — | Field ที่กำหนดค่าได้เฉพาะใน Constructor — ไม่เปลี่ยนหลังจากนั้น | `private readonly IMainUoW _uow` |
| **abstract** | — | Method/Class ที่ไม่มี Implementation — บังคับให้ Subclass Implement | `BaseRepository` |
| **override** | — | การ Implement Method ที่ถูก Define ไว้ใน Base Class หรือ Interface | |
| **switch expression** | — | Syntax C# 8+ สำหรับ Switch แบบสั้น — ใช้ใน Connection selector | `dbKey switch { ... }` |

---

## หมวดที่ 15 — ADO.NET Core Classes

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **SqlConnection** | — | Class ใน ADO.NET สำหรับเปิด Connection ไปยัง SQL Server — ทุก Library ใช้เบื้องหลัง | Dapper, EF Core |
| **SqlCommand** | — | Class ใน ADO.NET สำหรับสร้างและรัน SQL Command บน Connection | ADO.NET Pattern |
| **SqlDataReader** | — | Class ใน ADO.NET สำหรับอ่านข้อมูลแบบ Forward-only, Read-only — เร็วสุด | ADO.NET Pattern |
| **SqlDataAdapter** | — | Class ใน ADO.NET สำหรับเติมข้อมูลลง DataSet/DataTable — ช้ากว่า DataReader | Legacy ADO.NET |
| **SqlParameter** | — | Class ใน ADO.NET สำหรับส่ง Parameter ไปกับ SQL Command — ป้องกัน SQL Injection | ADO.NET, EF Core SP |
| **Microsoft.Data.SqlClient** | — | NuGet Package ใหม่ที่แทน `System.Data.SqlClient` — ทุกโปรเจคต้องใช้ | ทุก Library |

---

## หมวดที่ 16 — Resilience & Fault Tolerance

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Polly** | — | .NET Library สำหรับ Resilience Pattern: Retry, Circuit Breaker, Timeout, Bulkhead | HTTP Client, DB Access |
| **IHttpClientFactory** | — | Interface ของ ASP.NET Core สำหรับสร้าง HttpClient อย่างถูกต้อง — จัดการ Connection Pool ให้ | HTTP Client Standard |
| **Retry Policy** | — | นโยบายที่ลองทำ Operation ซ้ำเมื่อเกิด Transient Error — กำหนดจำนวนครั้งและ Delay | Polly, DB Access |
| **Exponential Backoff** | — | Strategy ที่เพิ่ม Delay เป็นเท่าตัวทุกรอบ Retry (1s → 2s → 4s) — ลดภาระ Server | Retry Policy |
| **Circuit Breaker** | — | Pattern ที่หยุดเรียก Service ชั่วคราวเมื่อ Error เกินเกณฑ์ — ป้องกัน Cascading Failure | External API Call |
| **Timeout** | — | ระยะเวลาสูงสุดที่รอ Response — ถ้าเกินจะยกเลิก Operation | HTTP Client, DB Query |
| **Idempotency** | — | คุณสมบัติที่เรียก Operation ซ้ำหลายครั้งแล้วผลลัพธ์เหมือนเดิม | POST API, Retry |
| **Idempotency Key** | — | Key ที่ Client ส่งมาเพื่อให้ Server ตรวจสอบว่า Request นี้ซ้ำหรือไม่ | POST API Design |
| **Cascading Failure** | — | สถานการณ์ที่ Failure ของ Service หนึ่งลามไป Service อื่นเป็นลูกโซ่ | Circuit Breaker แก้ |
| **Bulkhead** | — | Pattern ที่แบ่ง Resource เป็นส่วนๆ — ป้องกันไม่ให้ Component หนึ่งใช้ Resource หมด | Polly Pattern |

---

## หมวดที่ 17 — Logging & Observability

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Structured Logging** | — | รูปแบบ Log ที่มีโครงสร้างชัดเจน (JSON) — ค้นหาและวิเคราะห์ได้ง่ายกว่า Plain Text | Coding Standard บังคับ |
| **ILogger\<T\>** | — | Interface มาตรฐานของ .NET สำหรับ Logging — Inject ผ่าน Constructor | ทุก Service, Repository |
| **CorrelationId** | — | UUID ที่ติดไปกับทุก Request ตลอด Flow — ใช้ Trace Request ข้าม Service | Middleware, HTTP Header |
| **Log Level** | — | ระดับความสำคัญของ Log: Trace → Debug → Information → Warning → Error → Critical | Logging Configuration |
| **Serilog** | — | Library Logging ยอดนิยมใน .NET — รองรับ Structured Logging และ Sink หลากหลาย | Logging Option |
| **Tracing** | — | การติดตาม Request ตลอด Flow ข้าม Service — ดูว่าผ่าน Service ไหนบ้างใช้เวลาเท่าไร | OpenTelemetry |
| **Metrics** | — | ข้อมูลเชิงตัวเลขที่วัดได้ต่อเนื่อง เช่น Request Count, Response Time, Error Rate | Monitoring Dashboard |

---

## หมวดที่ 18 — Code Quality & Metrics

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Cyclomatic Complexity** | — | ค่าวัดจำนวน Independent Path ใน Method — ยิ่งสูงยิ่งต้องเขียน Test มาก | SonarQube S1541 |
| **Cognitive Complexity** | — | ค่าวัดความยากในการอ่านเข้าใจ Code — คิดค่า Nesting, Break in Flow | SonarQube S3776 |
| **Code Smell** | — | Code ที่ไม่ผิดแต่มีปัญหาเชิง Design — บ่งบอกว่าควร Refactor | SonarQube, Code Review |
| **Guard Clause** | Early Return | Pattern ที่ตรวจสอบเงื่อนไข Invalid แล้ว Return ทันที — ลด Nesting | Refactoring Technique |
| **Extract Method** | — | Refactoring Technique ที่แยก Code ก้อนหนึ่งออกมาเป็น Method ใหม่ — ลด Method Length | Clean Code |
| **Strategy Pattern** | — | Design Pattern ที่แยก Algorithm ออกเป็น Class — ใช้แทน if/else chain | Refactoring Technique |
| **Magic Number** | — | ตัวเลขที่ใช้ตรงๆ ใน Code โดยไม่มีชื่อ — ควรแทนด้วย Named Constant | Code Smell |
| **Dead Code** | — | Code ที่ไม่ถูกเรียกใช้งาน — ควรลบออกเพื่อลดความสับสน | Code Smell |

---

## หมวดที่ 19 — Configuration & Options

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **IOptions\<T\>** | — | Interface สำหรับอ่าน Configuration แบบ Strongly-typed — Inject ผ่าน DI | Configuration Pattern |
| **IConfiguration** | — | Interface สำหรับอ่าน Configuration จาก appsettings.json, Environment Variable ฯลฯ | Program.cs |
| **Secret Manager** | — | เครื่องมือของ .NET สำหรับเก็บ Secret (Connection String, API Key) นอก Source Code | Development Security |
| **Feature Flag** | Feature Toggle | เทคนิคเปิด/ปิด Feature โดยไม่ต้อง Deploy Code ใหม่ — ใช้ Config หรือ Remote Service | Release Management |
| **Environment Variable** | — | ตัวแปรระดับ OS ที่ ASP.NET Core อ่านได้ — ใช้ Override appsettings ใน Production | Config Management |
| **User Secrets** | — | ไฟล์ `secrets.json` ที่เก็บนอก Project — ใช้ใน Development แทนการใส่ Secret ใน Code | Development Security |

---

## หมวดที่ 20 — Concurrency & Thread Safety

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **Thread Safety** | — | คุณสมบัติที่ Code ทำงานถูกต้องเมื่อหลาย Thread เข้าถึงพร้อมกัน | Singleton Service |
| **Race Condition** | — | Bug ที่เกิดจาก 2+ Thread แข่งกัน Read/Write ข้อมูลเดียวกัน — ผลลัพธ์ไม่แน่นอน | Concurrency Bug |
| **Deadlock** | — | สถานการณ์ที่ 2+ Thread รอ Resource ซึ่งกันและกันไม่มีที่สิ้นสุด — ระบบค้าง | DB Transaction, Async |
| **lock statement** | — | Keyword ใน C# สำหรับ Lock Resource ให้ Thread เดียวเข้าถึง — ป้องกัน Race Condition | Shared Resource |
| **SemaphoreSlim** | — | Synchronization Primitive สำหรับจำกัดจำนวน Thread ที่เข้าถึง Resource พร้อมกัน | Rate Limiting, Async |
| **Async I/O** | — | การทำ I/O (DB, File, HTTP) แบบ Non-blocking — Thread ไม่ถูก Block ขณะรอ | ทุก Async Method |

---

## หมวดที่ 21 — Caching

| คำศัพท์ | ย่อ / Full Name | ความหมาย | บริบทที่ใช้ |
|---------|----------------|-----------|------------|
| **IMemoryCache** | — | Interface ของ .NET สำหรับ In-Memory Cache — เก็บใน RAM ของ Process เดียว | Single Instance App |
| **IDistributedCache** | — | Interface ของ .NET สำหรับ Distributed Cache (Redis, SQL Server) — Share ข้าม Instance | Multi-Instance App |
| **Cache Invalidation** | — | กระบวนการลบ/อัปเดต Cache เมื่อข้อมูลต้นทางเปลี่ยน — หนึ่งใน Hard Problems ของ CS | Cache Management |
| **Cache-Aside Pattern** | — | Pattern ที่ Application ตรวจ Cache ก่อน → ถ้า Miss → Query DB → เก็บ Cache | Caching Strategy |
| **Cache Hit / Cache Miss** | — | Hit = พบข้อมูลใน Cache (เร็ว) / Miss = ไม่พบต้อง Query DB (ช้า) | Cache Performance |
| **TTL** | Time to Live | ระยะเวลาที่ Cache Item มีอายุ — หมดแล้วจะถูกลบอัตโนมัติ | Cache Expiration |

---

*อัปเดตล่าสุด: 5 มีนาคม 2026*  
*อ้างอิง: [Database_Access_Methods_NET10.md](./Database_Access_Methods_NET10.md) | [Dapper_Migration_Analysis.md](./Dapper_Migration_Analysis.md) | [Coding_Standard_Criteria.md](../Coding_Standard_Criteria.md)*
