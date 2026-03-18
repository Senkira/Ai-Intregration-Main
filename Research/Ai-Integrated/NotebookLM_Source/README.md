# NotebookLM Source — Pipeline Column Research

> รวมไฟล์สำหรับอัปโหลดเข้า Google NotebookLM เพื่อวิเคราะห์ว่า **column ไหนในProject Pipeline ควรใช้ Ai Role**

---

## 📁 รายการไฟล์

| # | ไฟล์ | เนื้อหา | ลำดับ |
|---|------|---------|------|
| 01 | `01_Pipeline_Column_Analysis.md` | **ตาราง 27 columns** — วิเคราะห์ คน/AI + คำถาม 5 ข้อ | ⭐ อัปโหลดก่อน |
| 02 | `02_Role_Research_Full.md` | **งานวิจัย Pipeline Design** — Step 1-6, R1-R12, 8 Roles | ⭐ อัปโหลดก่อน |
| 03 | `03_Role_Plan_Full.md` | **Gap Analysis + Improvement Plan** — 7 Ai Roles vs 295 เกณฑ์ | ⭐ อัปโหลดก่อน |

---

## 🚀 วิธีใช้

1. ไปที่ [notebooklm.google.com](https://notebooklm.google.com)
2. สร้าง Notebook ใหม่ ตั้งชื่อ **"Ai-Role Pipeline Column Research"**
3. อัปโหลดไฟล์ทั้ง 3 ตามลำดับ
4. ถามคำถามจาก **Section 4** ใน `01_Pipeline_Column_Analysis.md`

---

## 💡 Prompt แนะนำ

### วิเคราะห์ Column

```
จาก 27 columns ใน Pipeline Board วิเคราะห์ว่าช่องไหนควรมี Ai Role จริง
ให้ trace ย้อนกลับจาก Develop (Step 1-6) ว่า input มาจาก column ไหน
แล้วจัดลำดับความสำคัญ
```

### ตรวจสอบความสมเหตุสมผล

```
column ที่ตั้งเป็น "ลุ้น" (Proposal, Get Req User#3, Infra Steering, Prepare Infra)
มี evidence จาก Role-Research.md หรือ Role-Plan.md สนับสนุนหรือไม่?
ถ้าไม่มี ควรตัดออก
```

### Audio Overview

```
สร้าง podcast สรุป "ช่องไหนควรใช้ AI ช่องไหนไม่ควร" ให้ทีม ESB ฟัง
ใช้ภาษาไทย 5-10 นาที
```
