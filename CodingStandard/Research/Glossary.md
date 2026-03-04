# Glossary — สารบัญคำศัพท์เฉพาะ

> รวมคำศัพท์เทคนิคที่ใช้ในเอกสาร Research ทั้งหมด  
> อ้างอิง: [Database_Access_Methods_NET10.md](./Database_Access_Methods_NET10.md) | [Dapper_Migration_Analysis.md](./Dapper_Migration_Analysis.md)  
> **วันที่:** 4 มีนาคม 2026

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

*อัปเดตล่าสุด: 4 มีนาคม 2026*  
*อ้างอิง: [Database_Access_Methods_NET10.md](./Database_Access_Methods_NET10.md) | [Dapper_Migration_Analysis.md](./Dapper_Migration_Analysis.md)*
