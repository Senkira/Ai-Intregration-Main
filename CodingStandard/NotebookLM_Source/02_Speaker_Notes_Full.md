# Speaker Notes — Coding Standard Slides

> สคริปต์พูดประกอบไฟล์ `Coding_Standard_Slides.html` (20 slides)  
> อธิบายเหตุผล ที่มา ผลกระทบ — ไม่ใช่แค่บอกกฎ แต่บอกว่า "ทำไม"

---

## Slide 1 — Title

สวัสดีครับ ก่อนเข้าเรื่อง ขอเล่าสั้น ๆ — เมื่อไม่นานมานี้ developer ใหม่เข้ามาในทีม เปิดโปรเจคแรก ใช้เวลา 3 สัปดาห์กว่าจะเข้าใจโครงสร้าง พอย้ายไปโปรเจคที่สอง ต้องเรียนรู้ใหม่หมด เพราะเขียนคนละแบบ ตั้งชื่อไม่เหมือนกัน จัดการ error ไม่เหมือนกัน

(pause)

นี่คือปัญหาที่เกิดซ้ำทุกครั้งที่มีคนเข้า-ออก และเราจ่ายต้นทุนนี้อยู่ทุกวัน

วันนี้จะนำเสนอ Coding Standard Criteria สำหรับ ASP.NET Core Web API + Dapper บน .NET 10 / C# 14 ที่จะแก้ปัญหานี้

เป้าหมายง่าย ๆ คือ — developer ทุกคน เปิดโปรเจคไหนก็เข้าใจทันที เหมือนไปทุก McDonald's สาขาไหนก็สั่งแบบเดียวกันได้

ถ้าถามว่าไม่ทำได้ไหม — ได้ แต่ต้นทุนที่จ่ายอยู่คือ onboarding developer ใหม่ใช้เวลา 2–4 สัปดาห์ต่อโปรเจค ถ้ามีมาตรฐาน ลดเหลือ 3–5 วัน คูณจำนวนคนที่หมุนเวียนต่อปี ประหยัดได้หลายร้อย man-day ยังไม่รวม production incident จาก code quality ต่ำ ซึ่งแต่ละครั้งกินเวลา 2–5 วันทำงานของทีม

---

## Slide 2 — Executive Summary

เพื่อแก้ปัญหาที่ว่า เราสร้างมาตรฐานชุดนี้ขึ้นมา มาดูภาพรวมกันก่อน

ตัวเลข — 295 เกณฑ์ แบ่งเป็น 246 บังคับ กับ 49 แนะนำ

(pause — ให้ผู้ฟังย่อยตัวเลข)

ฟังดูเยอะ แต่เหตุผลที่ต้องครอบคลุมขนาดนี้ เพราะมันกินตั้งแต่โครงสร้างโฟลเดอร์ ไปจนถึง security, testing, performance, deployment ถ้าไม่กำหนดรายละเอียด แต่ละคนจะตีความไม่เหมือนกัน ยิ่งเยอะคนยิ่งคลาดเคลื่อน

สิ่งสำคัญ — เกณฑ์ทุกข้อไม่ได้คิดขึ้นเอง ทุกข้ออ้างอิงจากแหล่งที่เป็นสากลและ auditor ยอมรับได้ ได้แก่ Microsoft .NET Official Docs, OWASP ASVS 5.0 ซึ่งเป็นมาตรฐาน security สำหรับ web application, NIST SP 800-53 ที่ภาครัฐสหรัฐใช้, ISO 27001:2022 ที่องค์กรต้องผ่าน certification และ CWE Top 25 ที่รวม vulnerability ที่พบบ่อยที่สุดในโลก ถ้า audit ถามว่าทำไมกำหนดแบบนี้ เราชี้ reference ได้ทุกข้อ

แน่นอนว่า 295 ข้อฟังดูเยอะ เรื่อง delivery speed ต้องพูดตรง ๆ — sprint แรก ๆ อาจช้าลง 15–20% เพราะต้อง set up โครงสร้างและเรียนรู้ pattern แต่ตั้งแต่ sprint ที่ 3 เป็นต้นไปจะเร็วขึ้นกว่าเดิม เพราะไม่ต้องคิดโครงสร้างเอง ไม่ต้อง debug ปัญหาที่มาตรฐานป้องกันไว้แล้ว ไม่ต้อง rework จาก code review ที่ reject ซ้ำ ๆ ข้อมูลจาก industry research (Capers Jones) ชี้ว่าโปรเจคที่มี coding standard ดี มี defect rate ต่ำกว่า 60% และ rework ลดลง 40% ลงทุนช่วงแรกแต่ได้คืนทุกวัน

(pause)

คำถามถัดมาคือ จะบังคับใช้ยังไงให้จริง — ไม่ได้พึ่งคน ใช้ 3 กลไกอัตโนมัติ หนึ่ง .editorconfig + `dotnet format` บังคับ formatting ตอน commit สอง CI Pipeline มี SonarQube gate ตรวจ naming, complexity, code smell ถ้าไม่ผ่าน merge ไม่ได้ สาม Code Review Checklist ที่ reviewer ใช้ตรวจเกณฑ์ที่ tool จับไม่ได้ วัดผลผ่าน SonarQube Dashboard รายสัปดาห์ — coverage, technical debt, vulnerability count ดู trend ได้

สำหรับโปรเจคเก่าหลายสิบตัวที่มีอยู่แล้ว ไม่ต้อง rewrite ใช้ progressive adoption — เมื่อเปิด feature ใหม่หรือ fix bug ให้โค้ดส่วนที่แก้ทำตามมาตรฐาน โค้ดเก่าที่ไม่ได้แตะไม่ต้องแก้ เรียกว่า Boy Scout Rule — ทำให้ดีขึ้นทุกครั้งที่เข้าไป ค่อย ๆ converge เข้าหามาตรฐาน โปรเจคใหม่ทั้งหมดเริ่มจาก Template

เรื่อง exception — บางข้ออาจมีเหตุผลที่ทำไม่ได้ในบาง context เรามี Waiver Process ชัดเจน developer เขียน request ระบุข้อที่ขอยกเว้นพร้อมเหตุผล ส่ง Tech Lead approve ถ้าเป็นเกณฑ์ security ต้อง Security Champion sign-off ด้วย waiver มีอายุ review ทุก quarter ไม่ใช่ยกเว้นถาวร

Rollout plan แบ่ง 3 Phase: Phase 1 (เดือน 1–2) Training + Pilot 2–3 โปรเจคใหม่ / Phase 2 (เดือน 3–4) โปรเจคใหม่ทั้งหมดบังคับ + โปรเจคเก่าเริ่ม progressive adoption / Phase 3 (เดือน 5+) Full enforcement ทุกโปรเจค CI gate เปิดเต็ม

---

## Slide 3 — 8 Pillars Overview

26 หัวข้อจัดกลุ่มเป็น 8 กลุ่ม เพื่อให้เห็นภาพรวมก่อนลงรายละเอียด

**Architecture** 88 เกณฑ์ — เป็นกลุ่มที่เยอะที่สุด เพราะถ้าโครงสร้างพื้นฐานผิดตั้งแต่ต้น ยิ่งเขียนต่อยิ่งซับซ้อน ยิ่งแก้ยาก เหมือนสร้างบ้านแล้วฐานรากเอียง ต่อให้ตกแต่งสวยแค่ไหนก็พัง ครอบคลุม §1 โครงสร้าง, §2 ตั้งชื่อ, §6 DI, §14 formatting

**Data & Business** 54 เกณฑ์ — กำหนดว่าข้อมูลเข้าออก DB ต้องผ่านทางเดียว คือ Dapper + Repository ทำไมไม่ใช้ EF Core — เพราะทีมเราทำ banking API ต้องควบคุม SQL เองทุกบรรทัด EF Core generate SQL ให้ ซึ่งบาง scenario มี performance overhead และ unpredictable query plan Dapper ให้เราเขียน SQL เองแต่ลดงานจาก raw ADO.NET ได้ 70% ครอบคลุม §3 DataAccess, §4 Service, §5 Controller

**Security** 30 เกณฑ์ — API ของเราเป็น financial service ถ้าถูกโจมตีสำเร็จ ผลกระทบรุนแรง ทั้ง SQL injection, token hijack, data leak เกณฑ์กลุ่มนี้ตั้งมาเพื่อปิดช่องโหว่ที่พบบ่อยตาม CWE Top 25 ครอบคลุม §7 Auth, §9 Config, §15 Validation, §19 NuGet

**Quality & Testing** 38 เกณฑ์ — โค้ดที่ไม่มี test เหมือนถนนที่ไม่มีราวกั้น วันดีคืนดีก็ตกหน้าผา เกณฑ์กลุ่มนี้บังคับให้มี safety net ทุกจุดเปลี่ยน ครอบคลุม §8 Error handling, §12 Testing, §20 Complexity, §11 Code Quality

**API Design** 21 เกณฑ์ — API เราเปิดให้ระบบอื่นเรียก ถ้าแต่ละโปรเจคออกแบบ URL, response format, pagination ไม่เหมือนกัน ฝั่ง consumer ต้องเรียนรู้ใหม่ทุกตัว ครอบคลุม §13 API Design, §10 OpenAPI, §21 Documentation

**Performance** 30 เกณฑ์ — user ไม่รอ response เกิน 3 วินาที เรากำหนด P95 ≤ 500ms เพราะ API ส่วนใหญ่ถูกเรียกเป็น chain ถ้าตัวหนึ่ง 500ms ก็กิน budget ไป 1 ใน 6 ของ chain ทั้งหมด ครอบคลุม §17 Performance, §16 Observability, §18 HTTP Client

**Advanced Patterns** 27 เกณฑ์ — กลุ่มนี้แนะนำ ไม่บังคับ เพราะไม่ใช่ทุกโปรเจคที่ต้องการ circuit breaker หรือ cache แต่เมื่อโปรเจคโตขึ้น complexity สูงขึ้น กลุ่มนี้จะเป็นตาข่ายนิรภัยชั้นถัดไป ครอบคลุม §22–§25

**DevOps** 7 เกณฑ์ — แยกขอบเขตชัดว่า developer ทำอะไรได้ ทำอะไรไม่ได้ เพราะเคยเกิด incident ที่ developer แก้ Jenkinsfile แล้ว pipeline ล่มทั้ง branch §26

เกณฑ์ทุกข้อ map กลับไป reference สากลได้หมด — CWE Top 25, OWASP ASVS, ISO 27001:2022 เอกสาร Criteria ฉบับเต็มมี mapping table ท้ายเอกสาร สำหรับท่านที่ต้องตอบ auditor ดูได้เลย

(pause)

ทั้ง 8 กลุ่มนี้คือภาพรวม ต่อจากนี้เราจะลงรายละเอียดทีละกลุ่ม เริ่มจากสิ่งที่ developer เจอวันแรกเลย — โครงสร้างโปรเจค

---

## Slide 4 — Project Structure (§1)

เปิดโปรเจคใหม่ สิ่งแรกที่ต้องถูกคือโครงสร้าง ถ้าโครงสร้างผิดตั้งแต่ต้น จะส่งผลกระทบเป็น snowball effect ไปทุกหัวข้อ

ใช้ 3-Layer Architecture — Controllers, Services, DataAccess — ตาม Microsoft N-Layer Architecture Guide ทำไมแยก 3 Layer? เพราะแต่ละ layer มีหน้าที่เดียว Controller รับ-ส่ง HTTP เท่านั้น Service ทำ business logic เท่านั้น DataAccess คุย DB เท่านั้น ถ้าไม่แยก เวลาต้องเปลี่ยน DB จาก SQL Server เป็น PostgreSQL จะต้องไล่แก้ทุกไฟล์ แต่ถ้าแยก layer แค่แก้ DataAccess

ทำไม Models ต้องแยก Entities กับ Requests? — Entity คือหน้าตาข้อมูลใน DB, Request คือสิ่งที่ user ส่งมา สองสิ่งนี้ต้องไม่ใช่ class เดียวกัน เพราะ DB schema อาจมี column ที่ไม่ควรให้ user เห็นหรือแก้ได้ เช่น CreatedDate, IsDeleted ถ้าใช้ class เดียวกัน user อาจส่ง IsDeleted = false มา overwrite ข้อมูลได้ เรียกว่า Mass Assignment Attack ตาม CWE-915

ทำไมต้องมี Interfaces? — เพื่อให้ unit test mock ได้ ถ้าไม่มี interface ตอน test Service ต้องต่อ DB จริง ซึ่งช้า ไม่เสถียร และไม่ควรทำ

ทำไม tests แยกจาก src? — เพราะตอน publish production ไม่ต้องการให้ test code ถูก compile ไปด้วย เปลือง size และเปิด attack surface โดยไม่จำเป็น

---

## Slide 5 — Naming Convention (§2)

พอโครงสร้างถูกแล้ว สิ่งถัดมาที่ developer เจอคือ — ตั้งชื่อ

62 เกณฑ์ ฟังดูเยอะ แต่จริง ๆ แค่จำหลักเดียว — PascalCase กับ _camelCase แล้วทุกอย่างจะเข้าที่

ทำไมเรื่องตั้งชื่อถึงสำคัญขนาดนี้? — เพราะ developer อ่านโค้ดมากกว่าเขียน 10 เท่า ชื่อที่สื่อความหมายช่วยให้เข้าใจโค้ดได้โดยไม่ต้องไล่อ่านทุกบรรทัด ชื่อที่ดีทำหน้าที่เป็น documentation ในตัว

ตารางด้านซ้ายคือ pattern ที่ต้องจำ ทุกตัวมีเหตุผล — Controller ลงท้ายด้วย Controller เพื่อให้เห็นทันทีว่าเป็น API endpoint ไม่ใช่ Service, Interface ขึ้นต้นด้วย I ตาม .NET convention ที่ใช้มาตั้งแต่ .NET Framework 1.0, async method ลงท้าย Async เพราะ caller ต้องรู้ว่า method นี้ต้อง await ถ้าไม่ลงท้ายอาจลืม await แล้วเกิด fire-and-forget bug

ด้านขวาคือ anti-pattern — ห้าม Hungarian notation อย่าง strName, intCount เพราะ C# เป็น strongly-typed compiler บอก type ให้อยู่แล้ว ไม่ต้องเขียนซ้ำในชื่อ ห้าม UPPER_SNAKE_CASE อย่าง MAX_RETRY_COUNT เพราะนี่คือ convention ของ Java/Python ไม่ใช่ C# ใน C# constant ใช้ PascalCase ห้ามย่อ อย่าง CustRepo เพราะคนใหม่จะไม่รู้ว่า Cust คือ Customer หรือ Custom หรือ Custody

---

## Slide 6 — DataAccess Layer (§3)

จากโครงสร้างกับชื่อ ต่อไปเราลงมาที่ชั้นล่างสุด — ชั้นที่คุยกับ database โดยตรง

38 เกณฑ์ เยอะสุดเป็นอันดับสอง เพราะ DataAccess คือจุดที่เสี่ยงที่สุดของระบบ ข้อมูลทุกบาทเข้าออกผ่านตรงนี้

ทำไมเลือก Dapper ไม่ใช้ EF Core? — Dapper เป็น micro-ORM พูดง่าย ๆ คือตัวช่วยแปลงผลลัพธ์ SQL เป็น object ให้ แต่ SQL เราเขียนเองทุกบรรทัด community benchmark ทั่วไปแสดงว่า Dapper ช้ากว่า raw ADO.NET แค่เล็กน้อย แต่ EF Core อาจช้ากว่า 2–4 เท่าในบาง query pattern โดยเฉพาะ query ที่ซับซ้อนหรือเปิด change tracking สำหรับ banking API ที่ต้อง optimize ทุก millisecond และต้อง audit SQL ทุก query การให้ framework สร้าง SQL ให้อัตโนมัติทำให้เราตรวจสอบลำบาก

ทำไมต้อง Repository Pattern? — เพราะ Service layer ไม่ควรรู้จัก IDbConnection ถ้า Service เรียก DB ตรง เท่ากับ layer บนรู้จัก implementation detail ของ layer ล่าง วันที่ต้องเปลี่ยนจาก SQL Server เป็น PostgreSQL จะต้องไล่แก้ Service ทุกตัว

ทำไม IDbConnectionFactory ห้าม new SqlConnection ตรง? — เพราะต้องการให้ connection string มาจากที่เดียว ถ้ากระจายอยู่หลายที่ วันที่ต้องเปลี่ยน connection string ต้องไล่หาทุกจุด พลาดจุดเดียวระบบเชื่อมผิด DB

เรื่อง Security สำคัญมาก — ห้ามเขียน `$"SELECT * FROM Users WHERE Id = {id}"` เด็ดขาด เพราะถ้า id มาจาก user input อาจถูก inject เป็น `1; DROP TABLE Users;` พูดง่าย ๆ คือ attacker ส่งคำสั่ง SQL แอบมากับข้อมูล ลบทั้งตารางได้เลย ต้องใช้ @parameter ให้ Dapper จัดการ parameterized query ตาม CWE-89 ซึ่งอยู่ใน Top 5 ของ CWE Top 25 ทุกปี ห้าม SELECT * เพราะอาจดึง column ที่เป็น sensitive data อย่าง password hash กลับมาโดยไม่ตั้งใจ ตาม CWE-200 connection ต้องปิดด้วย using เสมอ เพราะถ้าลืมปิด connection pool จะหมด เหมือนก๊อกน้ำเปิดทิ้งไว้ไม่ปิด น้ำในถังหมด DB ก็รับ request ไม่ได้ ตาม CWE-404

(pause — เรื่อง security จบ เปลี่ยนจังหวะ)

อีกเรื่องที่เสีย resource โดยไม่จำเป็นคือ CancellationToken — สมมติ user กดปุ่มค้นหาแล้วปิด browser ไปก่อน ถ้าไม่มี CancellationToken request นั้นจะยัง query DB อยู่จนเสร็จทั้งที่ไม่มีใครรอแล้ว เสีย resource ไปเปล่า ๆ CancellationToken เป็นสัญญาณบอกว่า "ไม่ต้องทำต่อแล้ว" ส่งต่อจาก Controller ไปจนถึง DB query ยกเลิกได้ทันทีที่ client disconnect Dapper รับ token ผ่าน CommandDefinition

เรื่อง audit trail — §3 กำหนดว่าทุก write operation ไม่ว่า INSERT, UPDATE, DELETE ต้องผ่าน Repository ที่มี logging ไม่มีทางเขียน DB ตรง ๆ จาก Service ได้ ร่วมกับ LogFilter ที่ §8 ทำให้ทุก API call ถูก log อัตโนมัติ พร้อม CorrelationId, UserId จาก JWT, timestamp, input parameters เทียบได้กับ audit trail ตาม ISO 27001 A.8.15 Logging

เรื่อง data encryption — Data in transit บังคับ HTTPS/TLS 1.2+ ตาม §7, JWT ใช้ HMAC-SHA256 หรือ RS256 signing Data at rest ขึ้นอยู่กับ DB configuration ซึ่งอยู่นอก scope ของ coding standard แต่ connection string ต้องใช้ Encrypt=true บังคับ encrypted connection ไปยัง SQL Server

---

## Slide 7 — Service & Controller (§4, §5)

พอเข้าใจชั้น DataAccess แล้ว เลื่อนขึ้นมาชั้นบน — Service กับ Controller

สองหัวข้อนี้คู่กัน — Service ทำงานหนัก Controller ทำแค่ส่งต่อ

Service ทุกตัวต้องมี Interface — Interface คือ "สัญญา" ที่บอกว่า Service ตัวนี้ทำอะไรได้บ้าง แต่ไม่บอกว่าทำยังไง ไม่ใช่แค่ convention แต่จำเป็นสำหรับ 2 เรื่อง หนึ่งคือ DI container (ระบบจัดการ dependency อัตโนมัติ) ต้อง register ด้วย interface เพื่อให้สลับ implementation ได้ สองคือ unit test ต้อง mock (จำลอง) service ได้ ถ้าไม่มี interface จะจำลองไม่ได้เลย

สร้าง dependency ต้องใช้ Constructor Injection เท่านั้น ห้าม new — พูดง่าย ๆ คือห้าม Service ไปสร้าง Repository เอง แต่ให้ "ประกาศ" ว่าต้องการอะไร แล้วระบบจะ "ส่งให้" ตอนเริ่มทำงาน ถ้าเขียน `new CustomerRepository()` ตรง ๆ จะ test ไม่ได้เลย เพราะเปลี่ยนตัวจำลองไม่ได้

ทุก method ต้องเริ่มด้วย Guard Clause — คือการตรวจข้อมูลเข้าที่ต้น method ทันที ถ้าผิดก็ reject ทันที เหมือน รปภ. ตรวจบัตรก่อนเข้าตึก ถ้า id เป็น null แล้วปล่อยให้วิ่งไปจนถึง DB แล้วค่อยเจอ error จะ debug ยากว่า null มาจากไหน Guard clause ตรวจที่ต้น method throw ทันที stack trace ชัดเจน

ห้าม return Entity ออกไปตรง ๆ เด็ดขาด เพราะ Entity มี field ที่ map กับ DB ตรง ๆ เช่น PasswordHash, InternalNotes, IsDeleted ถ้า return Entity ตรง field เหล่านี้จะรั่วไปถึง client ต้อง map เป็น Response DTO ที่เลือกเฉพาะ field ที่ client ควรเห็น

Controller ต้อง thin — ทำแค่ "รับ → เรียก → ส่ง" เพราะ Controller ที่มี business logic จะ test ยาก ต้อง mock HttpContext, ModelState, ActionResult ถ้า logic อยู่ใน Service ที่รับ parameter ธรรมดา test ง่ายกว่า 10 เท่า

ที่กำหนดให้ใช้ ControllerBase แทน Controller ก็เพราะ Controller class มี View(), PartialView(), Json() ที่เป็นของ MVC ซึ่ง API ไม่ต้องการ ControllerBase มีแค่ Ok(), BadRequest(), NotFound() ที่ API ใช้จริง เบากว่า ไม่เปิด method ที่ไม่จำเป็น

Guard clause ใน Service กับ FluentValidation ที่ Controller ไม่ซ้ำกัน — FluentValidation ตรวจ format เช่น email ถูก pattern ไหม ส่วน Guard clause ตรวจ business rule เช่น customer id นี้มีอยู่ใน DB ไหม ทำงานคนละชั้นตาม OWASP Input Validation Cheat Sheet ที่แนะนำให้ validate ทุก layer

---

## Slide 8 — DI & Authentication (§6, §7)

ต่อมา 2 เรื่องที่เป็น infrastructure ของทั้งระบบ — DI กับ Authentication

DI registration ใน Program.cs ต้องเรียงลำดับ Singleton → Scoped → Infra เรียงแบบนี้ไม่ใช่แค่ให้สวย แต่เพื่อให้คนอ่านเห็นทันทีว่าตัวไหนอยู่ได้ตลอด (Singleton) ตัวไหนอยู่แค่ต่อ request (Scoped) เวลา debug หาปัญหาจะง่ายขึ้นมาก DbConnectionFactory เป็น Singleton เพราะมันแค่เป็น "โรงงานสร้าง connection" ตาม connection string ไม่มีข้อมูลสะสม ไม่ต้องสร้างใหม่ทุก request Repository เป็น Scoped เพราะต้องผูกกับ request lifecycle ถ้า request A กับ request B ใช้ Repository ตัวเดียวกัน ข้อมูลอาจปนกัน

เกณฑ์ที่ต้องจำคือ Captive Dependency หรือ "การจับตัวประกัน" — สมมติ Singleton A (อยู่ตลอดชีวิต app) inject Scoped B (ควรอยู่แค่ 1 request แล้วทิ้ง) ตัว B ถูกสร้างครั้งแรกแล้วถูก Singleton ยึดไว้ตลอด ไม่ถูกทำลาย ทำให้ B ใช้ข้อมูลเดิมข้าม request ทุกคน ถ้า B มี DbConnection ที่ควรใช้แค่ 1 request เท่ากับทุก request แย่ง connection เดียวกัน ข้อมูลปนกัน ที่อันตรายคือไม่ error ทันที แต่ข้อมูลผิดเงียบ ๆ ตรวจจับยากมาก

JWT Authentication กำหนดให้ validate ครบทั้ง 4 ค่า — Issuer ตรวจว่า token มาจากเซิร์ฟเวอร์ที่เราเชื่อถือ ไม่ใช่ใครมาแอบสร้างเอง Audience ตรวจว่า token นี้ออกให้ API ของเราจริง ไม่ใช่ token ของระบบอื่น Lifetime ตรวจว่ายังไม่หมดอายุ SigningKey ตรวจว่า token ไม่ถูก tamper ถ้าไม่ validate ตัวใดตัวหนึ่ง เหมือนประตูที่ล็อกแค่ 3 จาก 4 จุด โจรเข้าได้

ClockSkew ต้องตั้งเป็น Zero — ค่า default ของ .NET คือ 5 นาที หมายความว่า token หมดอายุไปแล้ว 5 นาที ยังใช้ได้อยู่ Microsoft ตั้งไว้เผื่อ server clock คลาดเคลื่อน แต่ระบบเราอยู่ใน cluster เดียวกัน clock sync ด้วย NTP อยู่แล้ว 5 นาทีเผื่อเกินไป ถ้า attacker ขโมย token ที่หมดอายุไปแล้ว ยังมีหน้าต่าง 5 นาทีใช้งานได้ ตั้ง Zero ปิดหน้าต่างนี้

---

## Slide 9 — Error Handling & Config (§8, §9)

ต่อไปเป็น 2 เรื่องที่ดูเหมือนง่ายแต่ทำผิดบ่อยที่สุด — Error Handling กับ Config

ทุก Service method ต้องมี try-catch เพราะถ้า exception หลุดออกไปถึง Controller โดยไม่จัดการ ASP.NET จะส่ง default error response ที่มี StackTrace กลับไปหา client StackTrace บอก attacker ว่าใช้ framework อะไร version อะไร โครงสร้าง namespace เป็นแบบไหน ช่วย attacker วางแผนโจมตีได้ ตาม CWE-209

Logging ต้องใช้ ILogger เท่านั้น ห้าม Console.WriteLine เพราะ Console.WriteLine ไม่มี log level ไม่มี timestamp ไม่มี structure ไม่ส่งไป log aggregator พอ deploy ขึ้น K8s ข้อมูลหายหมด ILogger ต่อเข้า Serilog, Application Insights ได้ กรอง level ได้ ค้นหาได้ alert ได้

Response ต้องเป็นรูปแบบเดียวกันทั้ง API — ถ้า API หนึ่งส่ง `{ "error": "..." }` อีก API ส่ง `{ "message": "..." }` อีกตัวส่ง `{ "msg": "...", "code": 1 }` ฝั่ง consumer ต้องเขียน error handling แยกทุกตัว ResultModel กำหนดว่าทุก API ต้องส่ง `{ status, isSuccess, message }` เหมือนกันหมด consumer เขียน handler ทีเดียวใช้ได้กับทุก API

Config ต้องแยก environment เพราะ Dev ต่อ DB ตัวหนึ่ง UAT อีกตัว Production อีกตัว ถ้ายัดรวมไฟล์เดียว เสี่ยงที่ Dev จะใช้ Production DB โดยไม่ตั้งใจ ASP.NET มี built-in mechanism ที่โหลด appsettings.{Environment}.json override appsettings.json อัตโนมัติ ใช้ให้ถูก

Secrets ห้ามอยู่ใน source code เด็ดขาด เพราะ source code อยู่ใน TFS ที่ developer ทุกคนเข้าถึงได้ ถ้า connection string มี password ทุกคนเห็น ผิด ISO 27001:2022 A.8.9

หลักง่าย ๆ คือ — password, API key, signing key ห้ามอยู่ในไฟล์ที่ commit ขึ้น TFS

ตัว production ใช้ Kubernetes Secrets หรือ Vault ตัว local ใช้ `dotnet user-secrets` ที่เก็บนอก project ที่สำคัญคือ secret ทุกตัวต้องเปลี่ยนได้โดยไม่ต้อง redeploy — อ่านจาก config ที่ reload ได้ ตาม NIST SP 800-53 IA-5

Structured Logging ห้ามใช้ string concatenation — `_logger.LogError("Error " + method + " " + id)` สร้าง string ใหม่ทุกครั้งแม้ log level จะไม่ถึง เสีย memory แต่ `_logger.LogError("Error in {Method} id={Id}", method, id)` จะ evaluate เฉพาะเมื่อ log level ถึง และ log aggregator parse field ได้ ค้นหา `Method = "GetCustomer"` ได้เลย ไม่ต้อง regex

ที่ต้องระวังเป็นพิเศษคือเรื่อง PII ใน log — §16 กำหนดชัดว่าห้าม log ข้อมูลส่วนบุคคล เช่น เลขบัตรประชาชน, เบอร์โทร, email, เลขบัญชี ถ้าจำเป็นต้อง log เพื่อ debug ต้อง mask เช่น `"1-xxxx-xxxxx-xx-1"` เรื่องนี้ตรง PDPA มาตรา 37(1) ที่กำหนดให้มีมาตรการรักษาความมั่นคงปลอดภัยที่เหมาะสม และ ISO 27001 A.8.11 Data Masking ถ้าเกิด data leak ผ่าน log จะกลายเป็น data breach ที่ต้องแจ้ง สคส. ภายใน 72 ชั่วโมง โทษทางปกครองปรับสูงสุด 5 ล้านบาท และอาจมีโทษทางอาญาเพิ่มเติม

---

## Slide 10 — Input Validation (§15)

(ก่อนไปต่อ ถ้ามีคำถามเรื่อง Slide ที่ผ่านมายกมือได้เลยนะครับ)

"Never trust input" — กฎข้อเดียวของ Slide นี้ HTTP request มาจากไหนก็ได้ ไม่ใช่แค่ UI ที่เราเขียน attacker ใช้ Postman หรือ curl ยิงตรง ส่งอะไรมาก็ได้

กำหนดให้ใช้ FluentValidation แทน Data Annotation เพราะ Data Annotation อย่าง `[Required]` เขียนง่ายก็จริง แต่ validation logic ปนอยู่กับ model class ไม่มี unit test ได้ง่าย ไม่มี conditional validation ถ้า rule ซับซ้อนจะเริ่มเละ FluentValidation แยก validator เป็น class ต่างหาก test ได้อิสระ เขียน complex rule อย่าง "ถ้า field A มีค่า field B ต้องมีค่าด้วย" ได้ง่าย

string input ทุกตัวต้อง Trim ก่อนใช้ เพราะ user อาจส่ง " admin " มาแล้วพอเก็บ DB ไม่ตรงกับ "admin" ที่ใช้ค้นหา ทำให้ data inconsistency

ห้ามรับ dynamic หรือ object เป็น parameter เพราะ compiler ไม่สามารถตรวจ type ได้ตอน compile time ทุก error จะเกิดตอน runtime ซึ่ง test อาจจับไม่ได้หมด ใช้ strongly-typed DTO ให้ compiler ช่วยตรวจ

pageSize ต้องจำกัดสูงสุด 100 เพราะถ้าไม่จำกัด user อาจส่ง pageSize=1000000 มาทำให้ DB ดึงข้อมูลล้าน row ใน single query เกิด memory spike และ response timeout เป็น Denial of Service โดย client

---

## Slide 11 — Testing Standards (§12)

พอกฎเขียนโค้ดครบแล้ว คำถามคือ — จะมั่นใจได้ยังไงว่าโค้ดทำงานถูก?

ทุก Service class ต้องมี unit test เพราะ test คือ safety net ที่บอกเราว่าโค้ดที่แก้ไม่ทำให้ของเดิมพัง ไม่มี test เท่ากับแก้โค้ดแล้วลุ้นว่า production จะพังไหม

เลือก xUnit เป็น test framework เพราะ xUnit เป็น framework ที่ .NET team เองใช้ test .NET runtime ออกแบบมาสำหรับ modern .NET ไม่มี setup/teardown แบบ class-level ที่ทำให้ test มี shared state บังคับให้แต่ละ test เป็น isolated โดยธรรมชาติ

เขียนตาม AAA Pattern — Arrange-Act-Assert ทำให้ test อ่านง่าย เห็นทันทีว่า test นี้เตรียมอะไร ทำอะไร คาดหวังอะไร ถ้าเขียนมั่วปนกัน พอ test fail จะ debug ยากว่าผิดตรงไหน

ห้ามต่อ DB จริงใน unit test เพราะ test ที่ต่อ DB จริงช้า ไม่เสถียร depend on data state ถ้า data เปลี่ยน test fail ทั้งที่โค้ดไม่ผิด CI จะรัน test ทุก commit ถ้าต่อ DB จริงแต่ละ commit ใช้เวลา 5 นาที 20 commit ต่อวัน เสียเวลาเกือบ 2 ชั่วโมง mock ใช้เวลาไม่กี่วินาที

Coverage กำหนดอย่างต่ำ 80% ที่ Service layer เพราะ Service คือ business logic ที่สำคัญที่สุด ถ้า logic ผิด ข้อมูลผิด Controller แค่ส่งต่อ DataAccess แค่ query DB แต่ Service คือคนตัดสินใจ ที่ไม่ตั้ง 100% เพราะ 100% มักบังคับให้เขียน test ที่ไม่มีความหมายเพิ่มแค่ cover getter/setter 80% คือจุดที่คุ้มค่าที่สุด

ทุก bug fix ต้องมี regression test เพราะ bug เกิดซ้ำได้ ถ้ามี test ล็อกไว้ว่า bug นี้จะต้องไม่กลับมา ถ้าใครแก้โค้ดแล้ว bug กลับมา test จะ fail ทันที จับได้ก่อน deploy

แต่ test อย่างเดียวยังไม่พอ ต้องมั่นใจว่าทุกโปรเจคทำตามจริง — อย่างที่พูดไปใน Slide 2 เรามี 3 ชั้นตรวจสอบ: unit test, SonarQube gate, และ Code Review checklist สามชั้นนี้ทำให้เกณฑ์ไม่ใช่แค่เอกสาร แต่เป็นกลไกที่ enforce ได้จริง

---

## Slide 12 — API Design (§13)

ต่อมาเรื่อง "หน้าตา" ที่ระบบภายนอกเห็น — API Design

URL ต้องมี version เสมอ `/api/v{version}/{resource}` เพราะ API ที่ออกไปแล้วมี consumer ใช้อยู่ ถ้าต้องแก้ structure ของ response จะ break consumer ทุกตัว version ทำให้เพิ่ม v2 ได้โดย v1 ยังทำงานปกติ consumer ค่อย ๆ migrate

ห้ามใส่ verb ใน URL — `/api/v1/getCustomer` ผิดเพราะ GET method บอกอยู่แล้วว่าจะ "get" ใส่ verb ซ้ำทำให้ URL ไม่เป็น REST พอมี POST ก็กลายเป็น `/api/v1/createCustomer` ถ้าทำถูกคือ `GET /api/v1/customers/{id}` กับ `POST /api/v1/customers` ใช้ HTTP method บอก action ใช้ URL บอก resource

JSON response ใช้ camelCase เพราะ frontend ส่วนใหญ่ใช้ JavaScript ซึ่ง convention คือ camelCase ถ้า API ส่ง PascalCase frontend ต้อง map ทุก field ถ้า API ส่ง camelCase ใช้ได้เลย ASP.NET ตั้งแต่ .NET 3.0 ก็ default เป็น camelCase แล้ว

DateTime ใช้ ISO 8601 UTC เพราะ server อาจอยู่ timezone หนึ่ง client อยู่อีก timezone ถ้าส่ง local time ไม่มี offset ไม่รู้ว่าเป็นเวลาของใคร ISO 8601 format `2026-03-12T10:00:00Z` ชัดเจนว่าเป็น UTC แปลงเป็น local time ได้ถูกต้องทุกที่ในโลก

Collection endpoint ต้องมี pagination เพราะตาราง customer อาจมี 10 ล้าน row ถ้า GET /customers ไม่มี pagination ดึงทั้งหมดมา response ขนาด GB memory explode ทั้ง server และ client

---

## Slide 13 — Performance & HTTP Client (§17, §18)

เข้าสู่ช่วงท้ายแล้ว — อีก 3 กลุ่มสุดท้ายเริ่มจาก Performance

เป้าหมาย performance คือ P95 ≤ 500ms — หมายความว่า 95% ของ request ต้องตอบใน 500ms เราไม่ดู average เพราะ average ซ่อน outlier ได้ average 100ms แต่ 5% ของ request ใช้ 10 วินาที user ยังเจอปัญหาอยู่ดี P95 จับ tail latency ได้ดีกว่า

ตั้งที่ 500ms เพราะ API ส่วนใหญ่ถูกเรียกเป็น chain เช่น Frontend → API A → API B → API C ถ้าแต่ละตัวใช้ 500ms chain 3 ตัว = 1.5 วินาที ยังพอรับได้ ถ้าแต่ละตัวใช้ 2 วินาที chain = 6 วินาที user ทนไม่ได้

N+1 Query เป็น performance killer ที่ต้องระวังเป็นพิเศษ สมมติดึง order 100 รายการ แล้วแต่ละ order ไป query customer อีกที = 1 + 100 = 101 queries แค่ JOIN ก็เหลือ 1 query Dapper ไม่ช่วยป้องกันให้ เพราะ SQL เราเขียนเอง ต้องระวังเอง

ห้ามเรียก .Result กับ .Wait() บน async method เด็ดขาด เพราะใน ASP.NET thread ที่รอจะถูก block หยุดทำงานอื่นไม่ได้ เหมือนพนักงานยืนรอลูกค้าคนเดียวจนกว่าจะเสร็จ ไม่ไปรับคนอื่น ถ้า request เยอะ thread pool หมด server ก็ค้าง เรียกว่า thread starvation ใช้ await แทน thread ถูกปล่อยไปทำงานอื่นระหว่างรอ

ห้ามสร้าง new HttpClient() ตรง ๆ เพราะ HttpClient ถ้าสร้างใหม่ทุก request จะสร้าง TCP connection ใหม่ทุกครั้ง พอ dispose แล้ว socket ยังค้างอยู่ในสถานะ TIME_WAIT อีกราว 2 นาที (ขึ้นกับ OS) เหมือนโทรศัพท์ที่วางสายแล้วแต่สายยังไม่ตัด request เยอะ ๆ port หมด server เชื่อมต่อไม่ได้เลย เรียกว่า socket exhaustion ต้องใช้ IHttpClientFactory จัดการ connection pool ให้ reuse connection

Retry ต้องใช้ Exponential Backoff ห้าม retry ทันที เพราะถ้า server ปลายทาง overload แล้วเรา retry ทันที 3 ครั้ง เท่ากับเพิ่ม load 3 เท่า ยิ่งทำให้ server ไม่ฟื้น Exponential Backoff 1s → 2s → 4s ให้เวลา server หายใจ

ถ้า dependency fail ซ้ำ ๆ ต้องใช้ Circuit Breaker ตัดวงจร — ถ้าเรียก service ปลายทางแล้ว fail 5 ครั้งติด แปลว่ามันน่าจะล่ม ถ้ายังเรียกต่อแต่ละ request รอ timeout 30 วินาที user รอ 30 วินาทีได้ error Circuit breaker return error ทันทีไม่ต้องรอ พอ 30 วินาทีผ่านค่อยลองใหม่

---

## Slide 14 — Observability (§16)

Production ที่ไม่มี observability เหมือนขับรถตอนกลางคืนโดยไม่เปิดไฟ ไม่รู้ว่ามีปัญหาจนกว่า user จะโทรมาบอก

(pause — ให้อุปมาจมลง)

เราเปิดไฟให้ production ด้วย 3 อย่าง — Structured Logging, CorrelationId, Health Check

กำหนดให้ใช้ Structured Logging ทุกที่ — plain text log อย่าง `"Error processing customer 123"` ค้นหาได้แค่ text search แต่ structured log เก็บเป็น field `{ Method: "GetCustomer", CustomerId: "123", Duration: 450 }` query ได้ เช่น "หา request ทั้งหมดที่ Duration > 1000ms" หรือ "หา error ของ customer 123" ทำ dashboard, alert ได้หมด

ทุก request ต้องมี CorrelationId เพราะ request เดียวอาจวิ่งผ่าน 5 service ถ้าไม่มี CorrelationId เวลาเกิด error จะไม่รู้ว่า log ของ service A กับ service B เป็น request เดียวกัน CorrelationId คือ ID ที่ติดไปกับ request ตั้งแต่ต้นจนจบ ค้นหา ID เดียวได้ทั้ง chain

ทุก API ต้องเปิด Health Check endpoint เพราะ K8s ใช้ liveness probe ตรวจว่า container ยังตอบ ถ้าไม่ตอบ K8s restart ให้ readiness probe ตรวจว่า service พร้อมรับ traffic ถ้า DB ยังเชื่อมไม่ได้ K8s จะยังไม่ส่ง traffic มา ป้องกัน user เจอ error

ห้าม silent failure เด็ดขาด เพราะ error ที่ไม่มีใครรู้คือ error ที่อันตรายที่สุด data อาจผิดไปหลายชั่วโมงกว่าจะมีคนสังเกต critical error ต้องมี alert ทันที ไม่ว่าจะเป็น email, Slack, PagerDuty

alert เป็นแค่จุดเริ่มต้น สิ่งที่ coding standard เตรียมให้จริง ๆ คือข้อมูลสำหรับ incident response — CorrelationId ทำให้ trace ได้ทั้ง chain ว่า request นี้ผ่านอะไรบ้าง, structured log ทำให้ query ได้ว่าช่วงเวลานั้นเกิดอะไร, health check ทำให้ตรวจสอบสถานะ dependency ได้ทันที เวลาเกิด incident ทีมมีข้อมูลครบตั้งแต่วินาทีแรก ไม่ต้องมานั่ง ssh เข้า server ไล่ grep log

เรื่อง log retention — retention policy อยู่ที่ infrastructure level เช่น ELK หรือ Splunk ไม่ใช่ application level แต่ coding standard รับรองว่าทุก API call ถูก log พร้อม timestamp, userId, correlationId, input/output summary ส่วนเรื่อง tamper-proof — log ถูกส่ง streaming ไป centralized log server แบบ append-only application ไม่มีสิทธิ์ delete หรือ modify log ที่ส่งไปแล้ว

---

## Slide 15 — Code Quality Gates (§20, §14)

Complexity ต้องจำกัด — พูดง่าย ๆ Complexity คือจำนวนทางแยกในโค้ด ยิ่ง if-else ซ้อนกันเยอะ ยิ่งเข้าใจยาก research จาก Carnegie Mellon SEI พบว่า bug rate เพิ่มขึ้นแบบทวีคูณเมื่อ cyclomatic complexity (จำนวนเส้นทางที่โค้ดวิ่งได้) เกิน 10 method ที่ complexity 20 มี bug rate สูงกว่า complexity 5 ถึง 4 เท่า ไม่ใช่เรื่อง preference แต่เป็นสถิติ

Cognitive complexity กำหนดไว้ ≤ 15 — ตัวเลขนี้วัดว่าคนอ่านต้องคิดหนักแค่ไหน ≤ 15 หมายความว่า developer คนอื่นอ่านเข้าใจได้ใน 1 รอบ ไม่ต้องอ่านซ้ำ

Method ≤ 30 บรรทัด เพราะจอ monitor ปกติแสดงได้ 40–50 บรรทัด method ที่อ่านจบได้ใน 1 หน้าจอ เข้าใจง่ายกว่า method ที่ต้อง scroll

Parameter ≤ 4 ตัว เพราะจาก cognitive psychology มนุษย์จำ working memory ได้ 4 ± 1 items parameter 7 ตัว developer จะลืมว่า parameter ที่ 3 คืออะไร

ทุกโปรเจคต้องมี .editorconfig เพราะ developer แต่ละคนตั้ง IDE ไม่เหมือนกัน คนหนึ่ง tab คนหนึ่ง space พอ commit diff เต็มไปด้วย whitespace change ที่ไม่เกี่ยวกับ logic เลย code review ดูไม่ออกว่าแก้อะไรจริง .editorconfig บังคับให้ IDE ทุกตัวใช้ setting เดียวกัน

Brace style ใช้ Allman ตาม Microsoft Official C# Coding Convention ซึ่งเป็นค่า default ของ Visual Studio ไม่ต้องเถียงกันว่าจะเปิดปีกกาบรรทัดเดียวกับ if หรือบรรทัดใหม่ ใช้ Allman ตาม official convention

---

## Slide 16 — Advanced Patterns (§22–§25)

กลุ่มนี้เป็นแนะนำ ไม่บังคับ เพราะแต่ละโปรเจคมี scope ต่างกัน API ง่าย ๆ CRUD ธรรมดา ไม่จำเป็นต้องมี Circuit breaker แต่ API ที่เรียก service ภายนอก หรือรับ load สูง ควรพิจารณา

§22 Resilience — Circuit Breaker ที่พูดไปแล้วใน Slide 13 อยู่ในกลุ่มนี้ เมื่อ dependency ล่ม ถ้าไม่ตัดวงจร service เราล่มตาม เรียกว่า cascading failure เหมือนเบรกเกอร์ไฟฟ้าที่ตัดวงจรเมื่อ overload ไม่ให้ไฟไหม้ทั้งบ้าน

§23 Concurrency — ห้ามใช้ lock กับ async เพราะ lock บังคับให้ thread หยุดรอ แต่ async ออกแบบมาให้ thread ไปทำงานอื่นระหว่างรอ สองอย่างขัดกัน ถ้าใช้คู่กัน thread อาจรอกันวนไม่จบ เกิด deadlock ระบบค้าง ใช้ SemaphoreSlim แทนเพราะรองรับ async ได้

§24 Caching — ทุก entry ต้องมี TTL เพราะ cache ที่ไม่หมดอายุจะกลายเป็น stale data เงียบ ๆ user เห็นข้อมูลเก่าโดยไม่รู้ ในระบบ financial data เก่า 1 ชั่วโมงอาจหมายถึงราคาที่ผิดไปแล้ว TTL บังคับให้ cache refresh ตามรอบ

§25 Feature Flags — flag ที่เปิดใช้แล้ว stable ต้อง cleanup ใน 1 sprint เพราะ flag ที่ค้างนาน ๆ กลายเป็น tech debt โค้ดเต็มไปด้วย `if (featureEnabled)` ซ้อนกัน ทุก branch ต้อง test ทั้ง on และ off ยิ่งค้างนานยิ่ง test case ทวีคูณ ต้องลบออกเมื่อ feature stable

---

## Slide 17 — Dependencies & DevOps (§19, §26)

NuGet package ต้อง pin version เสมอ เพราะ package อาจ publish version ใหม่ที่มี breaking change ถ้าใช้ wildcard อย่าง `6.*` วันดีคืนดี restore แล้วได้ version ใหม่ที่ทำให้ build แตก pin version ทำให้ทุก developer ได้ version เดียวกัน upgrade เมื่อพร้อม

ก่อน release ทุกครั้งต้องสแกน CVE ตาม Sonatype State of Software Supply Chain report 60% ของ security breach มาจาก known vulnerability ใน third-party library ไม่ใช่จากโค้ดที่เราเขียนเอง package ที่เราใช้อาจมีช่องโหว่ที่ถูกค้นพบหลัง install สแกนก่อน release จับได้ก่อนขึ้น production

License ห้ามใช้ GPL — เพราะ GPL กำหนดว่าถ้าใช้ library ที่เป็น GPL โค้ดของเราก็ต้องเปิดเป็น open source ด้วย ซึ่งไม่เหมาะกับ commercial software ของบริษัท MIT กับ Apache 2.0 อนุญาตให้ใช้ในโค้ด commercial ได้โดยไม่มีเงื่อนไข copyleft

Developer ห้ามแก้ Jenkinsfile เพราะ CI/CD pipeline เป็น shared infrastructure ที่ทุกโปรเจคใช้ร่วมกัน เคยเกิดจริง — developer แก้ Docker stage ผิดบรรทัดเดียว ทำให้ deploy ขึ้น production ไม่ได้ 2 วัน กระทบ 3 ทีม ตั้งแต่แยกขอบเขตให้ DevOps team ดูแล ไม่เกิดอีกเลย

CI Pipeline 8 gates เรียงกัน — ถ้า unit test ไม่ผ่าน 100% ก็ไม่ถึงขั้น build Docker ถ้า OWASP scan เจอ critical vulnerability ก็ไม่ deploy developer ต้องเข้าใจว่า pipeline ทำอะไรบ้าง เพราะถ้า push แล้ว pipeline fail ต้องรู้ว่าแก้ตรงไหน

pipeline ครอบคลุมทั้ง 3 มิติของ application security testing:

หนึ่ง SAST หรือ Static Analysis — SonarQube scan source code ทุก commit ตรวจ SQL injection pattern, hardcoded credential, insecure deserialization

สอง SCA หรือ Software Composition Analysis — OWASP Dependency-Check ร่วมกับ `dotnet list package --vulnerable` scan NuGet packages เทียบกับ NVD database ทุก build ถ้าเจอ Critical/High CVE build fail ทันที นอกจากนี้ยังมี scheduled scan สัปดาห์ละครั้ง เพราะ CVE ใหม่อาจถูกค้นพบหลังจาก build ล่าสุดผ่านไปแล้ว

สาม DAST หรือ Dynamic Testing — ทำเป็น periodic penetration test โดย external vendor ปีละ 2 ครั้ง ก่อน major release และ annual review finding ที่เจอ remediate ตาม severity — Critical ภายใน 7 วัน, High ภายใน 30 วัน, Medium ภายใน 90 วัน

coding standard ช่วยให้ pentest finding น้อยลงเรื่อย ๆ เพราะ common vulnerability ถูกป้องกันตั้งแต่เขียนโค้ด

---

## Slide 18 — Starter Template (Divider)

(เปลี่ยนจังหวะ — เบาลง ยิ้ม)

ผมรู้ว่าฟังมา 17 slides อาจรู้สึกว่า "295 ข้อ จำไม่ไหว" — ข่าวดีคือ ไม่ต้องจำ

(pause)

เราเตรียม Template ให้แล้ว เป็นโปรเจคจริงที่ทำตามทุกเกณฑ์ครบ 295 ข้อ compile ได้ test ผ่าน run ได้จริง ไม่ต้องจำกฎ เปิด Template เทียบได้เลย

---

## Slide 19 — Template Details

มาดูว่า Template หน้าตาเป็นยังไง — ตารางด้านซ้ายแสดงว่าแต่ละ layer ครอบคลุมเกณฑ์อะไรบ้าง

จุดเด่นคือทุกไฟล์มี comment `// §X.Y` บอกว่าบรรทัดนี้ตรงกับเกณฑ์ข้อไหนและทำไม เช่น `// §3.6 — always use @parameter to prevent SQL injection` ไม่ต้องเปิดเอกสารเทียบ ดูในโค้ดได้เลย

วิธีใช้ — copy Template เป็นโปรเจคใหม่ rename SampleAPI เป็นชื่อโปรเจค แก้ connection string ตาม environment เพิ่ม feature ตาม pattern ที่ Customer demo ไว้ ทุกครั้งที่เพิ่ม feature ใหม่ให้สร้างครบ 7 ชิ้น — Controller, Service, Repository, Entity, DTO, Validator, Test ขาดชิ้นไหน code review จะ reject

---

## Slide 20 — Thank You

กลับมาที่ปัญหาตอนต้น — developer ใหม่เข้ามา เปิดโปรเจค ใช้เวลา 3 สัปดาห์กว่าจะเข้าใจ ถ้าเรามีมาตรฐานนี้ วันแรกเปิด Template เห็นโครงสร้างเดียวกัน ชื่อเดียวกัน pattern เดียวกัน 3–5 วันเริ่มเขียน feature ได้

(pause)

295 เกณฑ์ แก่นจริง ๆ คือ 4 เรื่อง — โครงสร้างเป็นมาตรฐาน, ข้อมูลปลอดภัย, โค้ดมี test, performance รับประกัน ทุกเกณฑ์มีเหตุผลรองรับ ไม่มีข้อไหนตั้งเพราะชอบ แต่ตั้งเพราะเคยเจอปัญหาจริงหรือ reference สากลกำหนดไว้

สิ่งที่อยากให้ทุกคนทำหลังออกจากห้องนี้ — bookmark เอกสาร 3 ชิ้น: Criteria ฉบับเต็ม, Presentation สำหรับอ่านทบทวน, Template สำหรับเริ่มโปรเจคใหม่ โปรเจคถัดไปที่เปิดใหม่ ใช้ Template ได้เลย

ขอบคุณครับ มีคำถามเชิญได้เลย
