# Speaker Notes — Coding Standard Slides (ฉบับ 20 นาที)

> สคริปต์พูดประกอบไฟล์ `Coding_Standard_Slides.html` (20 slides)  
> ⏱ ออกแบบให้พรีเซนจบใน **20 นาที** — เฉลี่ย ~1 นาที/slide  
> เน้น key message + เหตุผลหลัก — ตัดรายละเอียดเชิงลึกออก ถ้าผู้ฟังสนใจให้เปิด Q&A

---

## Slide 1 — Title ⏱ 1 นาที

สวัสดีครับ เล่าสั้น ๆ — developer ใหม่เข้ามาในทีม ใช้เวลา 3 สัปดาห์กว่าจะเข้าใจโปรเจค พอย้ายโปรเจคที่สอง ต้องเรียนรู้ใหม่หมด เพราะเขียนคนละแบบ ตั้งชื่อไม่เหมือนกัน จัดการ error ไม่เหมือนกัน

วันนี้จะนำเสนอ Coding Standard สำหรับ ASP.NET Core Web API + Dapper บน .NET 10 / C# 14 ที่จะแก้ปัญหานี้

เป้าหมาย — developer ทุกคน เปิดโปรเจคไหนก็เข้าใจทันที เหมือน McDonald's สาขาไหนก็สั่งแบบเดียวกันได้

ถ้าถามว่าไม่ทำได้ไหม — ได้ แต่ต้นทุนที่จ่ายอยู่คือ onboarding 2–4 สัปดาห์ต่อโปรเจค ถ้ามีมาตรฐาน ลดเหลือ 3–5 วัน คูณจำนวนคนที่หมุนเวียนต่อปี ประหยัดได้หลายร้อย man-day ยังไม่รวม production incident จาก code quality ต่ำ

---

## Slide 2 — Executive Summary ⏱ 2.5 นาที

ภาพรวม — 295 เกณฑ์ แบ่งเป็น 246 บังคับ กับ 49 แนะนำ

ฟังดูเยอะ แต่ครอบคลุมตั้งแต่โครงสร้างโฟลเดอร์ ไปจนถึง security, testing, performance, deployment ทุกข้ออ้างอิงจาก Microsoft .NET Docs, OWASP ASVS 5.0, NIST SP 800-53, ISO 27001:2022 และ CWE Top 25 — audit ถามได้ทุกข้อ

เรื่อง speed — sprint แรก ๆ อาจช้าลง 15–20% แต่ตั้งแต่ sprint ที่ 3 เร็วขึ้นกว่าเดิม เพราะไม่ต้องคิดโครงสร้างเอง ไม่ต้อง rework จาก code review ที่ reject ซ้ำ

บังคับใช้ด้วย 3 กลไกอัตโนมัติ — .editorconfig + `dotnet format`, CI/SonarQube gate, และ Code Review Checklist วัดผลผ่าน **SonarQube Dashboard รายสัปดาห์** ดู coverage, technical debt, vulnerability count เป็น trend ได้

ข้อมูลจาก industry research (Capers Jones) ชี้ว่าโปรเจคที่มี coding standard ดี defect rate ต่ำกว่า 60% และ rework ลดลง 40%

โปรเจคเก่าใช้ progressive adoption — แก้ตรงไหน ทำตามมาตรฐานตรงนั้น โปรเจคใหม่เริ่มจาก Template ทันที

เรื่อง exception — บางข้อทำไม่ได้ในบาง context เรามี **Waiver Process** ชัดเจน developer เขียน request ระบุข้อที่ขอยกเว้นพร้อมเหตุผล Tech Lead approve ถ้าเป็นเกณฑ์ security ต้อง Security Champion sign-off ด้วย waiver review ทุก quarter ไม่ยกเว้นถาวร

**Rollout 3 Phase:** Phase 1 (เดือน 1–2) Training + Pilot 2–3 โปรเจค / Phase 2 (เดือน 3–4) โปรเจคใหม่บังคับ + เก่าเริ่ม progressive adoption / Phase 3 (เดือน 5+) Full enforcement CI gate เปิดเต็ม

---

## Slide 3 — 8 Pillars Overview ⏱ 1 นาที

26 หัวข้อจัดเป็น 8 กลุ่ม ผมจะ highlight ตัวเลขสำคัญ

- **Architecture** 88 เกณฑ์ — ถ้าโครงสร้างผิดตั้งแต่ต้น ยิ่งเขียนต่อยิ่งแก้ยาก
- **Data & Business** 54 เกณฑ์ — ข้อมูลเข้าออก DB ต้องผ่าน Dapper + Repository ทางเดียว
- **Security** 30 เกณฑ์ — ปิดช่องโหว่ตาม CWE Top 25
- **Quality & Testing** 38 เกณฑ์ — safety net ทุกจุดเปลี่ยน
- **API Design** 21 เกณฑ์ — consumer เรียนรู้ API ตัวเดียว ใช้ได้ทุกตัว
- **Performance** 30 เกณฑ์ — P95 ≤ 500ms
- **Advanced Patterns** 27 เกณฑ์ — แนะนำ ไม่บังคับ ใช้เมื่อโปรเจคโตขึ้น
- **DevOps** 7 เกณฑ์ — แยกขอบเขต developer กับ DevOps ชัดเจน

ทุกข้อ map กลับ reference สากลได้ — ดู mapping table ท้ายเอกสาร Criteria ฉบับเต็ม

---

## Slide 4 — Project Structure (§1) ⏱ 1 นาที

สิ่งแรกที่ต้องถูกคือโครงสร้าง — ใช้ 3-Layer Architecture ตาม Microsoft Guide

- **Controllers** — รับ-ส่ง HTTP เท่านั้น
- **Services** — business logic เท่านั้น
- **DataAccess** — คุย DB เท่านั้น

Models แยก Entities กับ Requests ป้องกัน Mass Assignment Attack (CWE-915) — user ส่ง field อย่าง IsDeleted มา overwrite ไม่ได้

tests แยกจาก src เพราะ production ไม่ต้องการ test code

---

## Slide 5 — Naming Convention (§2) ⏱ 1 นาที

62 เกณฑ์ แต่จำหลักเดียว — PascalCase กับ _camelCase

หลักสำคัญ — Controller ลงท้าย Controller, Interface ขึ้นต้น I, async method ลงท้าย Async เพราะ caller ต้องรู้ว่าต้อง await

Anti-pattern ที่ห้าม — ห้าม Hungarian notation (strName), ห้าม UPPER_SNAKE_CASE (นั่นคือ Java/Python), ห้ามย่อ (CustRepo — Customer หรือ Custom?)

ชื่อที่ดีคือ documentation ในตัว — developer อ่านโค้ดมากกว่าเขียน 10 เท่า

---

## Slide 6 — DataAccess Layer (§3) ⏱ 1.5 นาที

ชั้นที่เสี่ยงที่สุด — ข้อมูลทุกบาทเข้าออกผ่านตรงนี้

**ทำไม Dapper ไม่ใช่ EF Core?** — banking API ต้องควบคุม SQL เองทุกบรรทัด EF Core generate SQL ให้ซึ่ง audit ลำบาก Dapper ให้เราเขียน SQL เองแต่ลดงานจาก raw ADO.NET ได้มาก

**Security ที่ต้องจำ:**
- ห้าม string interpolation ใน SQL — ใช้ @parameter เสมอ ป้องกัน SQL injection (CWE-89)
- ห้าม SELECT * — อาจดึง sensitive data กลับมาโดยไม่ตั้งใจ (CWE-200)
- connection ต้องปิดด้วย using เสมอ — ป้องกัน connection pool หมด (CWE-404)

ส่ง CancellationToken จาก Controller ถึง DB query — ถ้า user ปิด browser request หยุดทันที ไม่เสีย resource เปล่า

**Audit trail** — ทุก write operation (INSERT/UPDATE/DELETE) ต้องผ่าน Repository ที่มี logging ร่วมกับ LogFilter ทำให้ทุก API call ถูก log อัตโนมัติพร้อม CorrelationId, UserId, timestamp ตาม ISO 27001 A.8.15

**Data encryption** — Data in transit บังคับ HTTPS/TLS 1.2+ connection string ต้องใช้ Encrypt=true บังคับ encrypted connection ไปยัง SQL Server

---

## Slide 7 — Service & Controller (§4, §5) ⏱ 1 นาที

Service ทำงานหนัก Controller ทำแค่ "รับ → เรียก → ส่ง"

- Service ต้องมี Interface — เพื่อ DI container และ unit test mock ได้
- ห้าม new dependency ตรง — ใช้ Constructor Injection เท่านั้น
- ทุก method เริ่มด้วย Guard Clause — ตรวจข้อมูลเข้าทันที
- ห้าม return Entity ออกตรง — ต้อง map เป็น Response DTO เพื่อไม่ให้ field อย่าง PasswordHash รั่ว
- ใช้ ControllerBase แทน Controller — เบากว่า ไม่มี View/MVC method ที่ API ไม่ต้องการ

---

## Slide 8 — DI & Authentication (§6, §7) ⏱ 1 นาที

**DI** — เรียง Singleton → Scoped → Infra ใน Program.cs ระวัง Captive Dependency คือ Singleton จับ Scoped ไว้ ทำให้ข้อมูลปนกันข้าม request โดยไม่ error

**JWT** — validate ครบ 4 ค่า: Issuer, Audience, Lifetime, SigningKey ขาดตัวเดียว เหมือนประตูที่ล็อกไม่ครบ

ClockSkew ตั้งเป็น Zero — default 5 นาทีเผื่อเกินไป ถ้า attacker ขโมย token ที่หมดอายุ ยังมีหน้าต่าง 5 นาทีใช้งานได้ ตั้ง Zero ปิดหน้าต่างนี้

---

## Slide 9 — Error Handling & Config (§8, §9) ⏱ 1.5 นาที

**Error Handling:**
- ทุก Service method ต้อง try-catch — ถ้า StackTrace หลุดไปถึง client attacker ใช้วางแผนโจมตีได้ (CWE-209)
- ใช้ ILogger เท่านั้น ห้าม Console.WriteLine — deploy ขึ้น K8s ข้อมูลหายหมด
- Response ใช้ ResultModel เดียวกันทุก API — `{ status, isSuccess, message }` consumer เขียน handler ทีเดียวใช้ได้ทุกตัว

**Config:**
- แยก environment — Dev/UAT/Production ต่อ DB คนละตัว ห้ามยัดรวม
- **Secrets ห้ามอยู่ใน source code เด็ดขาด** — password, API key, signing key ถ้าอยู่ใน code ทุกคนเห็น ผิด ISO 27001:2022 A.8.9 ต้องใช้ K8s Secrets หรือ Vault แทน

**PII ใน log** — ห้าม log เลขบัตรประชาชน, เบอร์โทร, เลขบัญชี ต้อง mask ตาม PDPA โทษปรับสูงสุด 5 ล้านบาท

---

## Slide 10 — Input Validation (§15) ⏱ 1 นาที

"Never trust input" — HTTP request มาจากไหนก็ได้ attacker ใช้ Postman ยิงตรง ส่งอะไรมาก็ได้

- ใช้ FluentValidation แทน Data Annotation — แยก validator เป็น class ต่างหาก test ได้อิสระ
- string input ทุกตัวต้อง Trim — " admin " ≠ "admin" ใน DB
- ห้ามรับ dynamic/object — ใช้ strongly-typed DTO ให้ compiler ช่วยตรวจ
- pageSize จำกัดสูงสุด 100 — ป้องกัน user ส่ง pageSize=1000000 ทำ Denial of Service

---

## Slide 11 — Testing Standards (§12) ⏱ 1 นาที

ทุก Service class ต้องมี unit test — ไม่มี test เท่ากับแก้โค้ดแล้วลุ้น

- ใช้ xUnit — framework ที่ .NET team เองใช้ ออกแบบให้ test isolated
- เขียนตาม AAA Pattern — Arrange, Act, Assert
- ห้ามต่อ DB จริงใน unit test — ใช้ mock ไม่เช่นนั้น CI ช้ามาก
- Coverage อย่างต่ำ 80% ที่ Service layer — จุดที่คุ้มค่าที่สุด
- ทุก bug fix ต้องมี regression test — ล็อกไว้ไม่ให้ bug กลับมา

---

## Slide 12 — API Design (§13) ⏱ 1 นาที

- URL ต้องมี version `/api/v{version}/{resource}` — เพิ่ม v2 ได้โดยไม่ break v1
- ห้ามใส่ verb ใน URL — `GET /api/v1/customers/{id}` ไม่ใช่ `/api/v1/getCustomer`
- JSON ใช้ camelCase — frontend JavaScript ใช้ได้เลย
- DateTime ใช้ ISO 8601 UTC `2026-03-12T10:00:00Z` — ไม่สับสน timezone
- Collection ต้องมี pagination — ตาราง 10 ล้าน row ถ้าดึงหมด memory explode

---

## Slide 13 — Performance & HTTP Client (§17, §18) ⏱ 1.5 นาที

P95 ≤ 500ms — ใช้ P95 ไม่ใช่ average เพราะ average ซ่อน outlier ได้ API ถูกเรียกเป็น chain ถ้าแต่ละตัว 500ms chain 3 ตัว = 1.5 วินาที ยังรับได้

สิ่งที่ต้องระวัง:
- **N+1 Query** — ดึง 100 order แล้ว query customer ทีละตัว = 101 queries แค่ JOIN เหลือ 1
- **ห้าม .Result / .Wait()** — block thread ถ้า request เยอะ thread pool หมด server ค้าง ใช้ await แทน
- **ห้าม new HttpClient()** — socket exhaustion ใช้ IHttpClientFactory แทน
- **Retry ใช้ Exponential Backoff** — 1s → 2s → 4s ให้เวลา server ฟื้น
- **Circuit Breaker** — dependency fail ซ้ำ ๆ ตัดวงจร return error ทันทีไม่ต้องรอ timeout

---

## Slide 14 — Observability (§16) ⏱ 1 นาที

Production ไม่มี observability เหมือนขับรถกลางคืนไม่เปิดไฟ

เปิดไฟด้วย 3 อย่าง:
1. **Structured Logging** — เก็บเป็น field query ได้ ทำ dashboard, alert ได้
2. **CorrelationId** — request เดียววิ่งผ่าน 5 service ค้นหา ID เดียวได้ทั้ง chain
3. **Health Check** — K8s ใช้ liveness/readiness probe ตรวจว่า container ยังทำงานและพร้อมรับ traffic

ห้าม silent failure — error ที่ไม่มีใครรู้คือ error ที่อันตรายที่สุด critical ต้อง alert ทันที

**Log tamper-proof** — log ถูกส่ง streaming ไป centralized log server แบบ append-only application ไม่มีสิทธิ์ delete หรือ modify log ที่ส่งไปแล้ว รองรับ audit requirement

---

## Slide 15 — Code Quality Gates (§20, §14) ⏱ 1 นาที

- **Cyclomatic complexity ≤ 10** — bug rate เพิ่มแบบทวีคูณเมื่อเกิน 10
- **Cognitive complexity ≤ 15** — developer อ่านเข้าใจได้ใน 1 รอบ
- **Method ≤ 30 บรรทัด** — อ่านจบใน 1 หน้าจอ
- **Parameter ≤ 4 ตัว** — มนุษย์จำ working memory ได้ 4 ± 1 items

ทุกโปรเจคต้องมี .editorconfig — บังคับ IDE ทุกตัวใช้ setting เดียวกัน ไม่มี whitespace diff เวลา code review

---

## Slide 16 — Advanced Patterns (§22–§25) ⏱ 0.5 นาที

กลุ่มนี้ **แนะนำ ไม่บังคับ** — ใช้เมื่อโปรเจคต้องการ

- §22 **Resilience** — Circuit Breaker ตัดวงจรเมื่อ dependency ล่ม ป้องกัน cascading failure
- §23 **Concurrency** — ห้าม lock กับ async ใช้ SemaphoreSlim แทน
- §24 **Caching** — ทุก entry ต้องมี TTL ป้องกัน stale data
- §25 **Feature Flags** — flag ที่ stable แล้วต้อง cleanup ใน 1 sprint

---

## Slide 17 — Dependencies & DevOps (§19, §26) ⏱ 1 นาที

- NuGet ต้อง **pin version** — ห้ามใช้ wildcard `6.*` ป้องกัน breaking change
- ก่อน release ต้อง **สแกน CVE** — 60% ของ breach มาจาก third-party library
- License **ห้าม GPL** — ใช้ MIT หรือ Apache 2.0 เท่านั้นสำหรับ commercial

Developer **ห้ามแก้ Jenkinsfile** — เคยเกิดจริง แก้ผิดบรรทัดเดียว deploy ไม่ได้ 2 วัน กระทบ 3 ทีม

CI Pipeline มี 8 gates + ครอบคลุม SAST (SonarQube), SCA (OWASP Dependency-Check), DAST (pentest ปีละ 2 ครั้ง) **Remediation SLA:** Critical ภายใน 7 วัน, High ภายใน 30 วัน, Medium ภายใน 90 วัน

---

## Slide 18 — Starter Template (Divider) ⏱ 0.5 นาที

ฟังมา 17 slides อาจรู้สึกว่า "295 ข้อ จำไม่ไหว" — ข่าวดีคือ **ไม่ต้องจำ**

เราเตรียม Template ให้แล้ว เป็นโปรเจคจริงที่ทำตามทุกเกณฑ์ครบ compile ได้ test ผ่าน run ได้จริง

---

## Slide 19 — Template Details ⏱ 1 นาที

Template ทุกไฟล์มี comment `// §X.Y` บอกว่าบรรทัดนี้ตรงกับเกณฑ์ข้อไหน ไม่ต้องเปิดเอกสารเทียบ

**วิธีใช้:**
1. Copy Template เป็นโปรเจคใหม่
2. Rename SampleAPI เป็นชื่อโปรเจค
3. แก้ connection string ตาม environment
4. เพิ่ม feature ตาม pattern — ทุกครั้งสร้างครบ 7 ชิ้น: Controller, Service, Repository, Entity, DTO, Validator, Test

---

## Slide 20 — Thank You ⏱ 1 นาที

กลับมาที่ปัญหาตอนต้น — ถ้ามีมาตรฐานนี้ developer ใหม่ 3–5 วันเริ่มเขียน feature ได้ แทนที่จะเป็น 3 สัปดาห์

295 เกณฑ์ แก่นจริง ๆ คือ 4 เรื่อง:
1. โครงสร้างเป็นมาตรฐาน
2. ข้อมูลปลอดภัย
3. โค้ดมี test
4. Performance รับประกัน

สิ่งที่อยากให้ทำหลังออกจากห้องนี้ — bookmark 3 ชิ้น: **Criteria ฉบับเต็ม, Presentation, Template** โปรเจคถัดไปใช้ Template ได้เลย

ขอบคุณครับ มีคำถามเชิญได้เลย

---

> **สรุปเวลาต่อ slide:**
>
> | Slide | หัวข้อ | นาที |
> |-------|--------|------|
> | 1 | Title | 1.0 |
> | 2 | Executive Summary | 2.5 |
> | 3 | 8 Pillars Overview | 1.0 |
> | 4 | Project Structure | 1.0 |
> | 5 | Naming Convention | 1.0 |
> | 6 | DataAccess Layer | 1.5 |
> | 7 | Service & Controller | 1.0 |
> | 8 | DI & Authentication | 1.0 |
> | 9 | Error Handling & Config | 1.5 |
> | 10 | Input Validation | 1.0 |
> | 11 | Testing Standards | 1.0 |
> | 12 | API Design | 1.0 |
> | 13 | Performance & HTTP Client | 1.5 |
> | 14 | Observability | 1.0 |
> | 15 | Code Quality Gates | 1.0 |
> | 16 | Advanced Patterns | 0.5 |
> | 17 | Dependencies & DevOps | 1.0 |
> | 18 | Starter Template | 0.5 |
> | 19 | Template Details | 1.0 |
> | 20 | Thank You | 1.0 |
> | | **รวม** | **20.0** |
