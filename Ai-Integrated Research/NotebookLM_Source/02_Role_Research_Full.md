# 🔬 Role-Research: Security & DBA Reviewer — Pipeline Design

> **วันที่วิจัย:** 16 มีนาคม 2026 | **โปรเจคอ้างอิง:** `ESTATEMENT_API` (ASP.NET Core 3.1 + MySQL + AS400)  
> **สถานะ:** ✅ ผ่าน 7 Scenario Tests — ปิด 16 Gaps ครบ | **อ้างอิง:** `Role-Plan.md`

---

## 1. Tech Stack จาก ESTATEMENT_API

| Layer | Technology | Sec/DB Concern |
|-------|-----------|:-:|
| Framework | ASP.NET Core 3.1, N-Layer (Controller → Service → DataAccess) | — |
| Auth | JWT Bearer + `[Authorize]` | 🔐 |
| Database | MySQL × 2 DBs via EF Core + UnitOfWork NuGet | 🗄️ |
| External | AS400 Mainframe via Socket | 🔐🗄️ |
| Deploy | Jenkins (3 env) + Docker + K8s | — |

### Security Touchpoints ที่พบจาก Code

| รหัส | จุดพบ | ระดับ | §§ |
|:----:|-------|:-----:|:--:|
| S1 | JWT Key อยู่ใน appsettings ไม่ใช่ Vault | ⚠️ | §7.2 |
| S2 | Input validate แบบ manual (`IsNullOrEmpty`) ไม่มี FluentValidation | ⚠️ | §15 |
| S3 | `ex.ToMessage()` → **Stack trace leak** กลับ client (หลายไฟล์) | 🔴 | §8.6 |
| S4 | PII (email, idCard, mobileNo) ไม่มี masking | 🔴 | §16.4 |
| S5 | **Hard-coded test data** `11111111`–`99999999` bypass logic ใน Production | 🔴 | — |
| S6 | Connection string ใน config ไม่ใช่ Vault | ⚠️ | §3.21 |

### Database Touchpoints ที่พบจาก Code

| รหัส | จุดพบ | ระดับ | §§ |
|:----:|-------|:-----:|:--:|
| D1 | ใช้ EF Core + UoW NuGet (ห้ามใน Standard ใหม่ §3.30) | ⚠️ | §3.30 |
| D2 | PK ใช้ GUID base64 ไม่ standard | ⚠️ | — |
| D3 | `SaveChanges()` ไม่มี explicit transaction | ⚠️ | §3 |
| D4 | INSERT ไม่มี audit log (Who/What/When) | 🔴 | §3.27 |
| D5 | ไม่มี migration scripts ใน `dbs/` | 🔴 | — |
| D6 | AS400 = external → DBA ตรวจ SQL ภายในไม่ได้ | ⚠️ | — |

### Key Insight

```
Security design ผิดหลัง code = rewrite (Auth scheme, PII strategy) → Sec Pre-check อยู่ "ก่อน" Developer
Security implementation ต้องเห็น code จริง (S3, S5)                → Sec Code Review อยู่ "หลัง" Developer
DB Schema ผิดหลัง code = rewrite (D5, D2)                       → DBA Pre-check อยู่ "ก่อน" Developer
DB Query/Transaction ตรวจจาก code (D3, D4)                       → DBA Query อยู่ "หลัง" Developer
★ Sec และ DBA มีโครงสร้างเหมือนกัน: Pre-check (design) ก่อน Dev + Code Review หลัง Dev
```

---

## 2. Flow Design — เปรียบเทียบ 3 แนวทาง

```mermaid
block-beta
  columns 5
  
  block:A:1
    columns 1
    A_title["A: Sequential"]
    A_score["Score: 1.70"]
  end
  
  space
  
  block:B:1
    columns 1
    B_title["B: Parallel ⭐"]
    B_score["Score: 2.40"]
  end
  
  space
  
  block:C:1
    columns 1
    C_title["C: Split"]
    C_score["Score: 2.10"]
  end
```

| เกณฑ์ (น้ำหนัก) | A: Sequential | B: Parallel | C: Split |
|-----------------|:-:|:-:|:-:|
| Cycle Time (30%) | 🔴 ช้า | 🟢 **เร็วสุด** | 🟡 กลาง |
| DBA จับ Schema ก่อน code (25%) | ❌ | ❌ | ✅ |
| Sec เห็น code จริง (15%) | ✅ | ✅ | ✅ |
| ซับซ้อน (15%) | 🟢 ง่าย | 🟡 กลาง | 🔴 ซับซ้อน |
| Reject ง่าย (10%) | 🟡 | 🟢 | 🔴 |
| **Weighted** | **1.70** | **2.40** | **2.10** |

> **เลือก: Hybrid B+C** — ใช้ Parallel (B) เป็นหลัก + DBA Pre-check จาก (C) สำหรับ Schema Change

---

## 3. Revised Flow (ฉบับสมบูรณ์)

> **Design Principles:**
> - Architect ออกก่อนเสมอ — output (Tags + Scope + DDL Draft + Sec Design) กำหนดว่า DBA/Sec จะได้รับ task ไหน ตรวจอะไร
> - QA ตรวจเอกสาร **ก่อน** Pre-check ทำงาน — ป้องกัน DBA/Sec ทำเสียเปล่าถ้า Task โดน Reject
> - DBA/Sec **สมมาตรกัน** — ทั้งคู่มี Pre-check (design, ก่อน Dev) + Code Review (implementation, หลัง Dev)

```mermaid
flowchart TD
    REQ([📋 Requirement]) --> EMG{🚨 Emergency?}
    EMG -->|YES| FT_ARCH["Step 1: Architect<br/><i>minimal + [sec]</i>"]
    FT_ARCH --> FT_DEV["Step 4: Developer<br/><i>fix + report</i>"]
    FT_DEV --> FT_SEC["Step 5: Sec Reviewer<br/><i>verify + scan</i>"]
    FT_SEC --> FT_DONE([✅ HOTFIX DEPLOY])
    FT_DONE -.->|mandatory| FOLLOWUP["📋 Follow-up Task<br/><i>Full Review ภายใน 1 sprint</i>"]

    EMG -->|NO| ARCH

    subgraph S1 ["Step 1 — Solution Architect"]
        ARCH["Architect<br/>วิเคราะห์ Requirement + Solution Design"]
        ARCH_OUT["OUTPUT: Development_Task<br/>- Tags + Review Scope<br/>- DDL Draft (ถ้า schema)<br/>- Perf Baseline (ถ้า perf)"]
        ARCH --> ARCH_OUT
    end

    ARCH_OUT --> QA

    subgraph S2 ["Step 2 — Task QA"]
        QA["Task QA<br/>ตรวจเอกสาร + Tags + Scope"]
    end

    QA -->|REJECT| ARCH
    QA -->|APPROVE| PRECHECK_Q{"🗄️🔐 dba-schema<br/>or sec-design?"}

    PRECHECK_Q -->|YES| PRECHECK_GATE
    PRECHECK_Q -->|NO| DEV

    subgraph PRECHECK_GATE ["Step 3 — Pre-check Gate (parallel)"]
        direction LR
        DBA_PRE["🗄️ DBA Pre-check<br/><b>if [dba-schema]</b><br/>Schema + Naming + Index<br/>+ Migration Script"]
        SEC_PRE["🔐 Sec Pre-check<br/><b>if [sec-design]</b><br/>Auth Design + PII Strategy<br/>+ Input Validation Approach"]
    end

    PRECHECK_GATE -->|Any REJECT| CORRECTED["ส่ง Corrected Design<br/>กลับ Architect ยืนยัน"]
    CORRECTED --> ARCH
    PRECHECK_GATE -->|All APPROVE| DEV

    subgraph S4 ["Step 4 — Developer"]
        DEV["Developer<br/>Implement ตาม Task<br/>+ Observations Section"]
    end

    DEV --> PARALLEL

    subgraph PARALLEL ["Step 5 — Parallel Review Gate"]
        direction LR
        QC["Dev QC<br/><b>always</b><br/>Scope/Logic + Triage"]
        SEC["🔐 Sec Code Review<br/><b>if [sec] or [sec-design]</b><br/>Implementation vs Design<br/>+ Related Scan"]
        DBA_Q["🗄️ DBA Reviewer<br/><b>if [dba-query]</b><br/>Query/Tx/Audit<br/>+ Entity↔DDL"]
    end

    PARALLEL -->|Any REJECT| DEV
    PARALLEL -->|All APPROVE| STD_CHECK{std tag?}

    STD_CHECK -->|YES| STD
    subgraph S6 ["Step 6 — Coding Standard <i>(Conditional)</i>"]
        STD["Std Reviewer<br/>295 เกณฑ์<br/><i>ข้ามส่วนที่ Sec/DBA ตรวจแล้ว</i>"]
    end
    STD --> DONE
    STD_CHECK -->|NO| DONE

    DONE([✅ MERGE READY])

    style S1 fill:#0a2a1a,color:#fff
    style S2 fill:#1a1a2e,color:#fff
    style S3 fill:#0f2040,color:#79c0ff
    style S4 fill:#1a1a2e,color:#fff
    style PARALLEL fill:#0f3460,color:#fff
    style S6 fill:#16213e,color:#fff
    style FT_ARCH fill:#e94560,color:#fff
    style FT_DEV fill:#e94560,color:#fff
    style FT_SEC fill:#e94560,color:#fff
    style FT_DONE fill:#e94560,color:#fff
```

---

## 4. Architect Decision — Tags & Scope (กำหนดตั้งแต่ Step 1)

```mermaid
flowchart TD
    START([Task เข้ามา]) --> DB_Q{"Schema change?<br/>(CREATE/ALTER)"}
    DB_Q -->|YES| TAG_SCHEMA["✅ [dba-schema]<br/>+ [dba-query]"]
    DB_Q -->|NO| SQL_Q{"SQL/Query ใหม่?<br/>แก้ DataAccess?"}
    SQL_Q -->|YES| TAG_QUERY["✅ [dba-query]"]
    SQL_Q -->|NO| NO_DBA["❌ ไม่ติด dba"]

    START --> AUTH_Q{"New Auth/Endpoint?<br/>New PII handling?"}
    AUTH_Q -->|YES| TAG_SEC_D["✅ [sec-design]<br/>+ [sec]"]
    AUTH_Q -->|NO| INPUT_Q{"แก้ code<br/>ที่แตะ Auth/PII?"}
    INPUT_Q -->|YES| TAG_SEC["✅ [sec]"]
    INPUT_Q -->|NO| EXT_Q{"Secret / TLS<br/>/ External?"}
    EXT_Q -->|YES| TAG_SEC
    EXT_Q -->|NO| NO_SEC["❌ ไม่ติด sec"]

    START --> FEAT_Q{"New Feature?"}
    FEAT_Q -->|YES| TAG_FULL["✅ [std-full]"]
    FEAT_Q -->|NO| SIZE_Q{">10 lines?"}
    SIZE_Q -->|YES| TAG_PART["✅ [std-partial]"]
    SIZE_Q -->|NO| NO_STD["❌ ไม่ติด std"]

    style TAG_SCHEMA fill:#0f3460,color:#fff
    style TAG_QUERY fill:#0f3460,color:#fff
    style TAG_SEC_D fill:#e94560,color:#fff
    style TAG_SEC fill:#3a1020,color:#fff
    style TAG_FULL fill:#533483,color:#fff
    style TAG_PART fill:#533483,color:#fff
```

> **sec-design vs sec:**
> - `[sec-design]` = **Structural** — New endpoint + Auth, New PII handling → ต้อง Pre-check design ก่อน Dev (auto-add [sec])
> - `[sec]` only = **Code-level** — แก้ code ที่แตะ Auth/PII เดิม (design มีอยู่แล้ว) → ตรวจแค่ code หลัง Dev
>
> **R1:** ทุก Tag ต้องมี **Review Scope** กำกับ

---

## 5. Scenario Audit — ทดสอบ 7 กรณี จับ 16 Gaps

```mermaid
pie title Gap Severity (16 Gaps)
    "🔴 Critical (7)" : 7
    "🟡 Major (8)" : 8
    "🟠 Minor (1)" : 1
```

| # | Scenario | Tags | Gaps พบ | ตัวอย่าง Gap |
|---|---------|------|:-------:|-------------|
| S1 | เปลี่ยน error message | — | 0 | ✅ ไม่มี |
| S2 | เพิ่ม endpoint + Input ใหม่ | `[sec]` | 2 | Architect ติด tag ไม่มี scope / Dev เพิ่ม DB write นอกเอกสาร |
| S3 | สร้าง Table + Feature ใหม่ | `[dba-*]` `[sec]` `[std]` | 4 | Entity ไม่ตรง DDL / DBA Reject loop / EF Core vs SQL / Std ตรวจซ้ำ |
| S4 | Optimize query ช้า | `[dba-query]` | 3 | QA ตรวจ perf ไม่ได้ / ไม่มี baseline / AS400 ตรวจไม่ได้ |
| S5 | Security Hotfix | `[sec]` | 2 | Sec ตรวจแค่จุดเดียว / Hotfix ช้าเกิน |
| S6 | เพิ่ม Column ใน Table เดิม | `[dba-*]` | 3 | Migration ไม่ตรวจ / Backward compat / Existing data |
| S7 | แก้ Logic มี legacy test data | — | 2 | Legacy risk หลุด pipeline / Dev QC ไม่มีสิทธิ์ escalate |

---

## 6. Gap Resolution — 12 Revision Rules

```mermaid
flowchart LR
    subgraph GAPS ["16 Gaps"]
        G_CRIT["🔴 7 Critical"]
        G_MAJ["🟡 8 Major"]
        G_MIN["🟠 1 Minor"]
    end

    subgraph RULES ["12 Revision Rules"]
        R1["R1: Architect ระบุ<br/>Review Scope"]
        R2["R2: Dev QC เพิ่ม<br/>tag ย้อนหลัง"]
        R3["R3: DBA Reject ให้<br/>Corrected DDL"]
        R4["R4: DBA output<br/>Migration Script"]
        R5["R5: DBA-Query เทียบ<br/>Entity↔DDL"]
        R6["R6: Sec สแกน<br/>Related Pattern"]
        R7["R7: Std ข้ามส่วน<br/>ที่ตรวจแล้ว"]
        R8["R8: Dev เพิ่ม<br/>Observations"]
        R9["R9: Observation<br/>Escalation Path"]
        R10["R10: Fast Track<br/>Hotfix Mode"]
        R11["R11: DBA รองรับ<br/>EF Core + External"]
        R12["R12: Architect ระบุ<br/>Perf Baseline"]
    end

    GAPS --> RULES
    RULES --> RESULT["✅ 16/16 ปิดครบ"]

    style RESULT fill:#079992,color:#fff
```

| Rule | ปิด Gap | สิ่งที่เปลี่ยน |
|:----:|:-------:|-------------|
| **R1** | G1 | Architect ต้องระบุ **Review Scope** (§§ + คำอธิบาย) ทุกครั้งที่ติด tag |
| **R2** | G2, G16 | Dev QC มีสิทธิ์ **เพิ่ม tag ย้อนหลัง** + สร้าง Follow-up Task |
| **R3** | G4 | DBA Reject → **ส่ง Corrected DDL กลับ** (Architect แค่ยืนยัน ไม่ต้องออกแบบเอง) |
| **R4** | G12-G14 | DBA Pre-check output = **Migration Script** + Backward Compat + Data Impact |
| **R5** | G3 | DBA-Query **เทียบ Entity ↔ DDL** ที่ Pre-check approve |
| **R6** | G10 | Sec **สแกน Related Pattern** ทั้ง codebase → Related Findings (ไม่ block) |
| **R7** | G6 | Std Reviewer **รับ Sec/DBA Report → ข้าม §§ ที่ตรวจแล้ว** |
| **R8** | G15 | Developer Report **บังคับมี Observations** (Security/DB/Quality — กรอก "None" ถ้าไม่พบ) |
| **R9** | G15-G16 | Dev QC อ่าน Observations → **Escalate**: เพิ่ม tag / สร้าง Follow-up |
| **R10** | G11 | **Fast Track**: Arch → Dev → Sec (ข้าม QA+QC) สำหรับ Production incident |
| **R11** | G5, G9 | DBA รองรับ **Dapper + EF Core (LINQ) + External System** (calling pattern) |
| **R12** | G8 | Architect ระบุ **Performance Baseline** (ก่อน/หลัง/วิธีวัด) ถ้า perf task |

---

## 7. Role Registry & Review Scope Matrix

```mermaid
flowchart TD
    subgraph ALWAYS ["Always (ทุก Task)"]
        R01["Step 1: Architect<br/>วิเคราะห์ + Tags + Scope"]
        R02["Step 2: Task QA<br/>ตรวจเอกสาร + Tags"]
        R03["Step 4: Developer<br/>Implement + Observations"]
        R04["Step 5: Dev QC<br/>Scope/Logic + Triage"]
    end

    subgraph PRE ["Step 3: Pre-check Gate"]
        DBA_P["🗄️ DBA Pre-check<br/><i>[dba-schema]</i>"]
        SEC_P["🔐 Sec Pre-check<br/><i>[sec-design]</i>"]
    end

    subgraph POST ["Step 5: Code Review (parallel)"]
        SEC["🔐 Sec Code Review<br/><i>[sec] / [sec-design]</i>"]
        DBA_R["🗄️ DBA Query Review<br/><i>[dba-query]</i>"]
        R05["Step 6: Std Reviewer<br/><i>[std-*]</i>"]
    end

    R01 --> R02 --> R03 --> R04
    R02 -.->|if dba-schema| DBA_P
    R02 -.->|if sec-design| SEC_P
    DBA_P -.->|approved| R03
    SEC_P -.->|approved| R03
    R04 -.->|parallel| SEC
    R04 -.->|parallel| DBA_R
    R04 -.-> R05

    style SEC_P fill:#e94560,color:#fff
    style SEC fill:#3a1020,color:#fff
    style DBA_P fill:#0f3460,color:#fff
    style DBA_R fill:#0f3460,color:#fff
    style R05 fill:#533483,color:#fff
```

### ใครตรวจอะไร (Primary — ไม่ซ้ำกัน)

| Concern | Dev QC | 🔐 Sec-Pre | 🔐 Sec-Code | 🗄️ DBA-Pre | 🗄️ DBA-Query | Std |
|---------|:------:|:---------:|:----------:|:----------:|:------------:|:---:|
| Logic/Scope ถูกตาม Task | ✅ | | | | | |
| Observation Triage | ✅ | | | | | |
| Entity ↔ DDL match | basic | | | | ✅ | |
| Auth Scheme, Authz Policy | | ✅ | | | | |
| PII Strategy, Input Rules | | ✅ | | | | |
| Auth/Validation Implementation | | | ✅ | | | |
| Error Leak, Secret, PII in Code | | | ✅ | | | |
| Test Data Bypass, TLS | | | ✅ | | | |
| Related Pattern Scan | | | ✅ | | | |
| Schema Design, Naming | | | | ✅ | | |
| Index, Migration, Rollback | | | | ✅ | | |
| Backward Compat, Data Impact | | | | ✅ | | |
| Query Performance, N+1 | | | | | ✅ | |
| Transaction, Audit Log | | | | | ✅ | |
| Connection Pool, EF/Dapper | | | | | ✅ | |
| Code Naming §2, Format §14 | | | | | | ✅ |
| Structure §1, DI §11 | | | | | | ✅ |
| Testing §12, Async §3.12 | | | | | | ✅ |
| API Design §10,§13, YAGNI §20 | | | | | | ✅ |

### Coding Standard §§ แบ่งตาม Reviewer

```mermaid
pie title 295 เกณฑ์ แบ่งตาม Primary Reviewer
    "🔐 Sec (~50 คะแนน)" : 50
    "🗄️ DBA (~30 คะแนน)" : 30
    "📋 Std (~215 คะแนน)" : 215
```

| Reviewer | §§ ที่รับผิดชอบ |
|----------|----------------|
| 🔐 **Sec** | §3.6 (SQL Injection), §3.21-3.23 (ConnSec), §7 (Auth), §8.6 (Error Leak), §9 (Secret), §15 (Validation), §16.4 (PII), §18-§19 (Advanced Sec) |
| 🗄️ **DBA** | §3.16-3.20 (Timeout), §3.24-3.27 (Pool/Audit), §17 (Performance) |
| 📋 **Std** | ที่เหลือทั้งหมด: §1-§2, §3.1-3.15, §3.28-3.30, §4-§6, §8, §10-§14, §16, §20-§26 |

---

## 8. Document Flow & Folder Structure

```mermaid
flowchart LR
    subgraph ARCH_OUT ["Step 1: Architect Output"]
        DT["Development_Task_{ID}.md<br/><i>+ Tags + Scope + DDL + Sec Design</i>"]
    end

    subgraph QA_OUT ["Step 2: Task QA"]
        QR["Task_QA_Report_{ID}.md"]
    end

    subgraph PRECHECK_OUT ["Step 3: Pre-check Gate (parallel)"]
        DP["DBA_Precheck_{ID}.md<br/>+ Migration_Script_{ID}.sql"]
        SP["Sec_Precheck_{ID}.md<br/>+ Approved Auth/PII Design"]
    end

    subgraph DEV_OUT ["Step 4: Developer"]
        CODE["💻 Code Changes"]
        DR["Development_Report_{ID}.md<br/><i>+ Observations</i>"]
    end

    subgraph REVIEW_OUT ["Step 5: Parallel Review Gate"]
        QC["Dev_QC_Report_{ID}.md"]
        SR["Security_Review_{ID}.md"]
        DBR["DBA_Review_{ID}.md"]
    end

    ARCH_OUT -->|"Task file"| QA_OUT
    QA_OUT -->|"if pre-check needed"| PRECHECK_OUT
    QA_OUT -->|"no pre-check"| DEV_OUT
    PRECHECK_OUT -->|"Approved Design"| DEV_OUT
    DEV_OUT -->|"Code + Report"| REVIEW_OUT
```

### Folder Structure

```
Ai-Role/
├── 01 Solution Architect/          ← ปรับ: +Tags +Scope +Baseline (R1, R12)
├── 02 Task QA/                     ← ปรับ: +Validate Tags (R1)
├── 03 Developer/                   ← ปรับ: +Observations (R8)
├── 04 Developer QC/                ← ปรับ: +Triage (R2, R9) +Entity↔DDL (R5)
├── 05 Coding Standard Reviewer/    ← ใหม่ (จาก Role-Plan)
├── Sec/                            ← 🆕
│   ├── Security Reviewer Prompt.md     ← Code Review (post-impl)
│   ├── Security Pre-check Prompt.md    ← 🆕 Design Review (pre-impl)
│   ├── Security_Checklist.md
│   └── Security Review Report/
└── DBA/                            ← 🆕
    ├── DBA Reviewer Prompt.md          ← Query Review (post-impl)
    ├── DBA Pre-check Prompt.md         ← Schema Review (pre-impl)
    └── DBA Review Report/
```

---

## 9. Step Count ตาม Task Type

| Task Type | Steps ที่ต้องผ่าน | ตัวอย่าง |
|-----------|:-:|---------|
| Simple fix | 1→2→4→5(QC) | เปลี่ยน error message |
| แก้ Auth/Input เดิม | 1→2→4→5(QC+Sec) | แก้ validation ใน code เดิม |
| New Auth endpoint | 1→2→**3(🔐 Sec Pre)**→4→5(QC+Sec) | เพิ่ม endpoint + Auth + PII |
| DB query change | 1→2→4→5(QC+DBA) | แก้ SQL/LINQ |
| Schema change | 1→2→**3(🗄️ DBA Pre)**→4→5(QC+DBA) | เพิ่ม column |
| Full feature | 1→2→**3(🗄️+🔐 parallel)**→4→5(QC+Sec+DBA)→**6** | New table + endpoints + Auth |
| Security Hotfix | 1→4→5(Sec) **(Fast Track)** | แก้ stack trace leak |

---

## 10. Observation Escalation Path

```mermaid
flowchart TD
    DEV["Developer พบ issue นอก scope"] -->|"บันทึกใน Report<br/>Observations section"| QC["Dev QC อ่าน Observations"]

    QC --> TYPE{"ประเภท?"}
    TYPE -->|Security| SEC_ACT{"ยังอยู่ใน<br/>Review Cycle?"}
    SEC_ACT -->|YES| ADD_SEC["เพิ่ม [sec] tag<br/>trigger Sec Review"]
    SEC_ACT -->|NO| FU_SEC["สร้าง Follow-up Task<br/>+ tag [sec]"]

    TYPE -->|DB| DB_ACT{"ยังอยู่ใน<br/>Review Cycle?"}
    DB_ACT -->|YES| ADD_DBA["เพิ่ม [dba-query] tag"]
    DB_ACT -->|NO| FU_DBA["สร้าง Follow-up Task"]

    TYPE -->|Quality| FU_Q["สร้าง Follow-up Task"]

    ADD_SEC --> NOBLOCK["✅ ไม่ block task ปัจจุบัน"]
    FU_SEC --> NOBLOCK
    ADD_DBA --> NOBLOCK
    FU_DBA --> NOBLOCK
    FU_Q --> NOBLOCK

    style DEV fill:#e94560,color:#fff
    style NOBLOCK fill:#079992,color:#fff
```

---

## 11. สรุป

| Metric | ก่อน (4 Roles) | หลัง (8 Roles) |
|--------|:-:|:-:|
| Security Coverage | ~10% | **~95%** |
| DB Coverage | ~5% | **~90%** |
| Coding Standard Coverage | ~10% | **~95%** |
| Scenarios ทดสอบ | 0 | **7** |
| Gaps พบ/ปิด | 0/0 | **16/16 ✅** |
| Review Overlap | N/A | **0%** |
| Revision Rules | 0 | **12** (R1-R12) |

> **Next Step:** Approve → สร้าง Prompt จริงใน `Ai-Role/Sec/` และ `Ai-Role/DBA/`
