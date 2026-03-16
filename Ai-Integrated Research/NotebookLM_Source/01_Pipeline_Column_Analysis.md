# 📊 Pipeline Column Analysis — ช่องไหนควรใช้ Ai Role?

> **วัตถุประสงค์:** วิเคราะห์ทุก column ใน Project Pipeline Board ว่าช่องไหน **มีศักยภาพ** ที่จะใช้ Ai Role ช่วยได้จริง  
> **วันที่วิเคราะห์:** 16 มีนาคม 2026  
> **Ai Roles ที่มีจริง (7 ตัว):** 01 Solution Architect · 02 Task QA · 03 Developer · 04 Developer QC · Sec · DBA · Std Reviewer

---

## 1. สรุป Pipeline Board — 27 Columns ทั้งหมด

### Phase: Meta (Board Management)

| # | Column | คน/AI? | เหตุผล |
|---|--------|:------:|--------|
| 1 | Pre-approve | คน | เป็น board admin — อนุมัติเข้า board |
| 2 | Document | คน | จัดเก็บเอกสาร — ไม่มี logic |
| 3 | Color Status | คน | กำหนดสี status ของ card |
| 4 | Delay Code | คน | ใส่รหัส delay — board management |
| 5 | Status Project | คน | สรุปสถานะ — board management |

> ❌ **ไม่มี column ไหนใน Meta ที่ควรใช้ AI** — เป็น board admin ทั้งหมด

---

### Phase: (R) Requirement

| # | Column | คน/AI? | เหตุผล | Ai Role ที่เป็นไปได้ |
|---|--------|:------:|--------|---------------------|
| 6 | Business Concept | คน | User Owner เสนอ idea — AI ไม่รู้ business context | — |
| 7 | Business Requirement | คน | BA สัมภาษณ์ User — ต้องคุยกันจริง | — |
| 8 | Proposal / POC | 🟡 **ลุ้น** | **Architect AI** ช่วยออกแบบ high-level solution ได้ | 01 Architect |

> 🔍 **ข้อควรพิจารณา Proposal/POC:**
> - ✅ Architect AI ช่วยวิเคราะห์ tech stack, risk, alternative ได้ดี
> - ⚠️ แต่ Proposal ต้อง buy-in จากคน — AI ออกแบบได้ แต่ตัดสินใจไม่ได้
> - ⚠️ POC ต้อง Dev ลงมือทำจริง — AI ช่วยได้แค่วิเคราะห์ direction

---

### Phase: (D) Delivery — ก่อน Develop

| # | Column | คน/AI? | เหตุผล | Ai Role ที่เป็นไปได้ |
|---|--------|:------:|--------|---------------------|
| 9 | Get Scope | คน | PM + Architect(คน) กำหนด scope — ต้องคุยกัน | — |
| 10 | Kick off | คน | ประชุม kick off — ไม่ใช่งาน AI | — |
| 11 | Get Req User#1 | คน | BA สัมภาษณ์ User — ต้องคุยกันจริง | — |
| 12 | Pre Steering | คน | PM เสนอแผนกับ management — ไม่ใช่งาน AI | — |
| 13 | **Assessment** | ✅ **AI** | **Architect + DBA + Sec** ทำ technical deep-dive ได้ | 01 Architect, DBA, Sec |
| 14 | Get Req User#2 | คน | BA สัมภาษณ์ User | — |
| 15 | **Get Req User#3** | 🟡 **ลุ้น** | **Architect AI** ช่วย finalize SAD + แตก task list ได้ | 01 Architect |
| 16 | **Infra Steering** | 🟡 **ลุ้น** | **DBA + Sec** ช่วยระบุ technical requirements ได้ | DBA, Sec |
| 17 | Steering | คน | PM อนุมัติงบ/timeline — ตัดสินใจ ≠ AI | — |
| 18 | **Prepare Infra** | 🟡 **ลุ้น** | **DBA + Sec** ช่วย generate DDL + security config ได้ | DBA, Sec |

> 🔍 **ข้อควรพิจารณา Pre-Develop columns:**
> - **Assessment** = ✅ ชัดเจน — AI ทำได้ดีเพราะเป็น technical analysis ล้วนๆ
> - **Get Req User#3** = Architect finalize solution จาก requirement สุดท้าย — AI ทำได้
> - **Infra Steering** = DBA/Sec ระบุ technical spec — AI ทำได้ แต่ต้องคนนำเสนอ
> - **Prepare Infra** = DBA create schema / Sec configure infra — AI generate ได้ แต่คน deploy

---

### Phase: ⚡ Develop (Step 1-6) — **แกนหลัก Ai-Role Pipeline**

| # | Column | คน/AI? | Step | Ai Roles |
|---|--------|:------:|:----:|----------|
| 19 | **Develop** | ✅ **AI ทั้ง 7** | 1-6 | 01 Architect, 02 Task QA, 03 Developer, 04 Dev QC, Sec, DBA, Std |

> ✅ **ไม่มีข้อกังขา** — Develop = ขั้นตอนที่ AI ทำงานเต็มรูปแบบ

---

### Phase: UAT — Go Live — Post — Assessment (หลัง Develop)

| # | Column | คน/AI? | เหตุผล | ข้อกังขา |
|---|--------|:------:|--------|---------|
| 20 | **UAT** | คน | User ทดสอบ, BA เตรียม test case | Defect fix กลับเข้า Develop |
| 21 | **Go Live** | คน | Infra deploy, DBA run migration, Sec verify | ⚠️ DBA/Sec ทำในฐานะ "คน" ไม่ใช่ AI |
| 22 | **Post Implement** | คน | Monitoring, warranty | Hotfix กลับเข้า Develop (R10) |
| 23 | **Project Assessment** | คน | Retrospective, closure | — |

> 🔍 **ข้อควรพิจารณา UAT+ columns:**
> - **UAT defect fix** → ไม่ใช่ card ใหม่ใน UAT — กลับเข้า Develop column ทำ Step 4→5
> - **Go Live DBA/Sec** → เป็นการ execute จาก output ที่ AI ทำไว้แล้วใน Develop — ไม่ต้องใช้ AI อีก
> - **Post monitoring** → เป็นงาน operational ไม่ใช่ design/review — AI ไม่มี role สำหรับนี้
> - **Retrospective** → ต้องการ human judgment จากประสบการณ์จริง

---

## 2. Concern Matrix — ทำไมบาง Column ไม่ควรมี AI

| Concern | อธิบาย | Column ที่โดน |
|---------|--------|:----------:|
| **Human Decision** | ตัดสินใจ budget, priority, go/no-go | Steering, Business Concept |
| **Physical Interaction** | ต้องคุยกับคนจริง สัมภาษณ์ | Get Req User#1-3, Kick off |
| **Operational Execution** | Deploy, run script, configure prod | Go Live, Prepare Infra(บางส่วน) |
| **Board Management** | จัดการ card/color/status | Meta columns ทั้ง 5 |
| **Post-facto Judgment** | ประเมินจากประสบการณ์จริง | Post, Assessment |

---

## 3. สรุป — Column ที่ควรใช้ AI (ชัดเจน + ลุ้น)

### ✅ ชัดเจน — ใช้ AI ได้เลย

| Column | Ai Roles | เหตุผล |
|--------|----------|--------|
| **Assessment** | 🏗️ Architect + 🗄️ DBA + 🔐 Sec | Technical deep-dive ล้วนๆ — AI ทำได้เต็มที่ |
| **Develop** (Step 1-6) | ทั้ง 7 Roles | แกนหลักของ Ai-Role Pipeline |

### 🟡 ลุ้น — AI ช่วยได้ แต่มีข้อจำกัด

| Column | Ai Roles | ช่วยอะไร | ข้อจำกัด |
|--------|----------|---------|---------|
| **Proposal/POC** | 🏗️ Architect | วิเคราะห์ tech direction, risk | คนต้องตัดสินใจ buy-in |
| **Get Req User#3** | 🏗️ Architect | Finalize SAD, แตก task list | Requirement มาจากคน |
| **Infra Steering** | 🗄️ DBA + 🔐 Sec | ระบุ technical spec | คนนำเสนอ |
| **Prepare Infra** | 🗄️ DBA + 🔐 Sec | Generate DDL + security config | คน deploy |

### ❌ ไม่ควรมี AI

| Column Group | จำนวน | เหตุผล |
|-------------|:-----:|--------|
| Meta (5) | 5 | Board management |
| Requirement (3) ยกเว้น Proposal | 2 | Human interaction |
| Delivery (4) ยกเว้น ลุ้น | 4 | Human decision + meeting |
| UAT + Go Live + Post + Assessment | 4 | Operational / Post-facto |
| **รวมไม่ใช้** | **15** | |

---

## 4. คำถามสำหรับ NotebookLM วิเคราะห์

> **คำถามที่ 1:** จาก Role-Research.md และ Role-Plan.md ช่อง Proposal/POC, Get Req User#3, Infra Steering, Prepare Infra ควรมี Ai Role จริงหรือไม่? มี evidence อะไรสนับสนุน?

> **คำถามที่ 2:** ช่อง Assessment ที่ใช้ Architect+DBA+Sec AI ทำ SAD+Impact Report — output เหล่านี้ถูกอ้างอิงใน Step 1-6 ตรงไหนบ้าง? ครบหรือไม่?

> **คำถามที่ 3:** Fast Track (R10) สำหรับ Hotfix — ควรอยู่ใน Develop column เท่านั้น หรือควรมี card ใน Post Implement ด้วย?

> **คำถามที่ 4:** ถ้าตัด Ai Role ออกจาก UAT+ ทั้งหมด แล้ว Go Live DBA run migration script จะใช้ script จากไหน? trace ย้อนกลับไปถึง Step 3 ได้ชัดเจนหรือไม่?

> **คำถามที่ 5:** Column ไหนที่ยังไม่มี Ai Role แต่ถ้ามีจะลด risk ได้มากที่สุด? จัดลำดับความสำคัญอย่างไร?
