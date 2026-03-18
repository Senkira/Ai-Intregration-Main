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
| 8 | Proposal / POC | คน | PM + Architect(คน) เสนอ solution — เป็น **business decision** ไม่ใช่ analysis | — |

> ❌ **Proposal ไม่ใช้ AI** — Architect AI ช่วยวิเคราะห์ได้แต่จุดประสงค์หลักคือ "should we do this?" (ตัดสินใจทำ) · AI ทำ full analysis ใน **Assessment** แทน

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
| 15 | **Get Req User#3** | คน | BA + User freeze requirement — **human negotiation** · ซ้ำ Assessment + Step 1 | — |
| 16 | **Infra Steering** | คน | Infra + DBA + Sec(คน) นำเสนอ — **presentation** ≠ review | — |
| 17 | Steering | คน | PM อนุมัติงบ/timeline — ตัดสินใจ ≠ AI | — |
| 18 | **Prepare Infra** | คน | Infra + DBA + Sec(คน) setup env — **operational** ≠ review/analyze | — |

> 🔍 **ข้อควรพิจารณา Pre-Develop columns:**
> - **Assessment** = ✅ ชัดเจน — เป็น **technical analysis ล้วนๆ** ตรง core (วิเคราะห์+ออกแบบ)
> - **Get Req User#3** = Arch update SAD ซ้ำกับ Assessment (SAD) + Step 1 (แตก task)
> - **Infra Steering** = DBA/Sec ระบุ spec → คนนำเสนอ = ไม่ใช่ core "review"
> - **Prepare Infra** = DBA/Sec "create/configure" ≠ core "review/analyze"

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

## 2. หลักการตัดสิน — "Core Capability ต้องตรงจุดประสงค์หลักของ Column"

### Ai Role Core Capabilities

| Ai Role | Core Capability | ประเภทงาน |
|---------|----------------|-----------|
| 🏗️ Architect | **วิเคราะห์** requirement → **ออกแบบ** solution | Analysis + Design |
| 📋 Task QA | **ตรวจสอบ** เอกสาร task ก่อน dev | Document Gate |
| 💻 Developer | **เขียน** code ตาม task | Implementation |
| ✅ Dev QC | **ตรวจสอบ** code + triage | Code Gate |
| 🔐 Sec | **ตรวจสอบ + วิเคราะห์** security (design + code) | Security Review |
| 🗄️ DBA | **ตรวจสอบ + วิเคราะห์** database (design + query) | DB Review |
| 📋 Std | **ตรวจสอบ** coding standard | Standard Review |

> **Core = วิเคราะห์ / ออกแบบ / ตรวจสอบ / เขียน code**
> **ไม่ใช่ Core = ตัดสินใจ business / ประชุม / deploy / monitor / สร้าง infrastructure**

### ทำไม 4 Column ถูกตัด

| Column | ทำไมดูเหมือนใช้ AI ได้ | ทำไมจริงๆ ไม่ควร |
|--------|----------------------|------------------|
| **Proposal/POC** | Arch วิเคราะห์ tech ได้ | จุดประสงค์หลัก = **business decision** "ควรทำไหม คุ้มไหม" → AI วิเคราะห์ได้แต่ตัดสินใจไม่ได้ · Assessment ทำ full analysis แล้ว |
| **Get Req User#3** | Arch finalize SAD ได้ | จุดประสงค์หลัก = **human negotiation** BA+User freeze req → Arch update SAD **ซ้ำกับ Assessment** (ทำ SAD แล้ว) + **Step 1** (แตก task แล้ว) |
| **Infra Steering** | DBA/Sec ระบุ spec ได้ | จุดประสงค์หลัก = **presentation** ขออนุมัติ infra → DBA provisioning ≠ core "review" · คน present ไม่ใช่ AI |
| **Prepare Infra** | DBA create schema ได้ | จุดประสงค์หลัก = **operational setup** → DBA/Sec "create/configure" ≠ core "review/analyze" |

---

## 3. สรุปผลตัดสิน — 2 จุดที่ใช้ AI + เหตุผล

### ✅ Assessment — Technical Analysis (project-level)

| Ai Role | ทำอะไร | Output → ไปไหน |
|---------|--------|:-------------:|
| 🏗️ Architect | Solution Architecture Design | SAD → **Step 1 input** |
| 🗄️ DBA | Database Impact Assessment | DB Impact + Draft ERD → **Step 3 DBA Pre-check input** |
| 🔐 Sec | Security Impact Assessment | Sec Impact + Auth Strategy → **Step 3 Sec Pre-check input** |

> ✅ **ตรง core ทั้ง 3 roles** — เป็น analysis/design ล้วน
> ✅ **Output ถูกใช้จริง** — trace ถึง Step 1 + Step 3 ของ Develop

### ✅ Develop Step 1-6 — Per-Task Pipeline (ทั้ง 7 Roles)

> ✅ แกนหลักของ Ai-Role Pipeline — ไม่มีข้อกังขา

### ❌ ที่เหลือ — งานคน (21 columns)

| กลุ่ม | จำนวน | เหตุผลรวม |
|-------|:-----:|-----------|
| Meta | 5 | Board management |
| Requirement (Business Concept, BRD, Proposal) | 3 | Human decision + negotiation |
| Delivery ก่อน Assess (Get Scope → Pre Steering) | 4 | Meeting + human planning |
| Delivery หลัง Assess (Get Req#2-3, Infra, Steering, Prepare) | 5 | Human negotiation / presentation / operational setup |
| UAT → Project Assessment | 4 | Operational / post-facto judgment |
| **รวมงานคน** | **21** | |

---

## 4. Data Flow — Assessment → Develop (จุดเชื่อมต่อ)

```
Assessment (project-level)
├── Arch → SAD ──────────────────┐
├── DBA → DB Impact + Draft ERD ─┼── [คนทำ: Get Req, Steering, Prepare Infra] ──┐
└── Sec → Sec Impact + Strategy ─┘                                               │
                                                                                  ▼
Develop (per-task) ──────────────────────────────────────────────────────────────────
├── Step 1: Architect ← SAD + Impact Reports → Development_Task
├── Step 2: Task QA ← Development_Task → APPROVE/REJECT
├── Step 3: DBA Pre-check ← Draft ERD → Migration_Script (if [dba-schema])
│           Sec Pre-check ← Strategy → Approved Design (if [sec-design])
├── Step 4: Developer → Code + Report (with Observations)
├── Step 5: Dev QC + Sec Review + DBA Review (parallel)
└── Step 6: Std Reviewer → ✅ MERGE READY
```

---

## 5. คำถามสำหรับ NotebookLM ตรวจสอบ

> **คำถามที่ 1:** Assessment ผลิต SAD + DB Impact + Sec Impact → Step 1-6 อ้างอิง output เหล่านี้ตรงไหนบ้าง? trace ได้ครบหรือไม่?

> **คำถามที่ 2:** ระหว่าง Assessment กับ Develop มี 7 columns คั่น (Get Req#2, Get Req#3, Infra Steering, Steering, Prepare Infra, ...) → ถ้า requirement เปลี่ยนระหว่างทาง SAD ที่ทำใน Assessment ยัง valid อยู่ไหม?

> **คำถามที่ 3:** Proposal ที่ตัดออก → ถ้า Architect(คน) เสนอ solution ผิดทาง → Assessment จะจับได้ไหม? หรือเป็น blind spot?

> **คำถามที่ 4:** Prepare Infra ที่ตัดออก → DBA(คน) create initial schema จาก Draft ERD → ถ้า schema ผิด Step 3 DBA Pre-check จะจับได้ไหม?

> **คำถามที่ 5:** สร้าง podcast สรุป "ทำไม AI ทำงานแค่ 2 จุด" สำหรับทีม ESB — ใช้ภาษาไทย 5-10 นาที
