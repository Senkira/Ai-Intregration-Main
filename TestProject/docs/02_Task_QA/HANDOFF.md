# 📤 HANDOFF — Task QA → Developer

> **จาก:** Role 02 — Task QA Reviewer  
> **ถึง:** Role 03 — Developer  
> **วันที่:** 18 มีนาคม 2026

---

## QA Verdict

### ✅ APPROVED — ผ่าน 3-Gate ทั้งหมด

| Gate | Name | Result |
|:----:|------|:------:|
| 1 | Alignment Check | ✅ PASS |
| 2 | Scope Boundary Check | ✅ PASS |
| 3 | Over-Engineering Check | ✅ PASS |

---

## สิ่งที่ส่งมอบให้ Developer

| # | เอกสาร | ไฟล์ | Action |
|---|--------|------|--------|
| 1 | **Development Task (APPROVED)** | `03_Developer/Task/Development_Task_TP001.md` | 📋 Copy ไปยัง Developer queue แล้ว |
| 2 | **Task QA Report** | `02_Task_QA/Task_QA_Report_APPROVED_TP001.md` | 📖 อ่านเพื่อดู warnings |

---

## ⚠️ คำเตือนจาก QA ที่ Developer ต้องใส่ใจ

| # | คำเตือน | ระดับ | สิ่งที่ Developer ต้องทำ |
|---|--------|:-----:|------------------------|
| 1 | SQLite ≠ SQL Server | ⚠️ Low | เพิ่ม comment ว่า production ต้องเปลี่ยน |
| 2 | SHA256 ไม่ปลอดภัย | ⚠️ Low | เพิ่ม comment `// Production: use BCrypt/Argon2` |
| 3 | Swashbuckle deprecated | ⚠️ Low | ใช้ version ที่ยัง work + note ไว้ |

---

## สิ่งที่ Developer ต้องทำ

1. **อ่าน** `03_Developer/Task/Development_Task_TP001.md` — ทำตาม 30 Steps ใน 8 Blocks
2. **ปฏิบัติตาม** Out of Scope (OS-1 ถึง OS-8) อย่างเคร่งครัด
3. **ใส่ใจ** คำเตือนจาก QA 3 ข้อข้างบน
4. **ส่งมอบ** Development Report + Actual Code ให้ Role 04 Developer QC

---

## สิ่งที่ Developer ห้ามทำ (7 Prohibitions)

| # | ข้อห้าม |
|---|--------|
| 1 | ❌ ห้ามแก้ไฟล์ที่ไม่ได้ระบุใน Task |
| 2 | ❌ ห้ามเปลี่ยน code style โดยไม่จำเป็น |
| 3 | ❌ ห้ามเพิ่ม NuGet package นอก scope |
| 4 | ❌ ห้ามเปลี่ยน DB schema นอก Task |
| 5 | ❌ ห้าม gold plating (เพิ่ม feature เกิน) |
| 6 | ❌ ห้ามตัดสินใจเองเมื่อ Task ไม่ชัด — ต้องถาม |
| 7 | ❌ ห้าม assume code content โดยไม่อ่านจริง |

---

> **Next Step:** Developer อ่าน Task → Implement → สร้าง Report → ส่งต่อ QC
