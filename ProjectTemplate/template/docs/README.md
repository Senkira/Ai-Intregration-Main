# __PROJECT_NAME__ API — Document Pipeline

> **รหัสโปรเจค:** __TASK_ID__  
> **วันที่สร้าง:** __DATE__  
> **สถานะ:** 🔄 IN PROGRESS

---

## Pipeline Flow

```
📁 docs/
│
├── 📋 README.md ........................ เอกสารนี้
├── 📋 Project_Summary.md .............. สรุปโปรเจค
│
│   STEP 1 ─ Solution Architect
├── 📁 01_Solution_Architect/
│   │   ├── Solution_Analysis.md
│   │   ├── Development_Task.md
│   │   └── HANDOFF.md → Role 02
│
│   STEP 2 ─ Task QA Reviewer
├── 📁 02_Task_QA/
│   │   ├── Task_QA_Report.md
│   │   └── HANDOFF.md → Role 03
│
│   STEP 3 ─ Developer
├── 📁 03_Developer/
│   │   ├── Task/ (approved copy)
│   │   ├── Development_Report.md
│   │   └── HANDOFF.md → Role 04
│
│   STEP 4 ─ Developer QC
├── 📁 04_Developer_QC/
│   │   ├── Dev_QC_Report.md
│   │   └── HANDOFF.md (Final Verdict)
│
│   Cross-Check
└── 📁 05_Coding_Standard_Checklist/
    │   └── Checklist.md
```

---

## วิธีใช้

1. เริ่มจาก Role 01 — เขียน Solution Analysis + Development Task
2. อ่าน HANDOFF.md ในแต่ละ folder เพื่อดูว่าต้องส่งอะไรต่อ
3. แต่ละ Role เขียน output ลงใน folder ของตัวเอง
4. Role สุดท้าย (04) ออก Final Verdict
