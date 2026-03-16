# NotebookLM Source — Coding Standard Presentation

> รวมไฟล์ทั้งหมดสำหรับอัปโหลดเข้า Google NotebookLM เพื่อสร้าง AI-powered notebook

---

## 📁 รายการไฟล์ (เรียงตามลำดับความสำคัญ)

| # | ไฟล์ | เนื้อหา | ขนาด | ลำดับแนะนำ |
|---|------|---------|------|-----------|
| 01 | `01_Coding_Standard_Criteria.md` | **เอกสารเกณฑ์ฉบับเต็ม** — 295 เกณฑ์ 26 หัวข้อ พร้อม reference mapping | ~192 KB | ⭐ อัปโหลดก่อน |
| 02 | `02_Speaker_Notes_Full.md` | **สคริปต์พรีเซนฉบับเต็ม** — อธิบายเหตุผลทุกเกณฑ์ 20 slides | ~61 KB | ⭐ อัปโหลดก่อน |
| 03 | `03_Speaker_Notes_20min.md` | **สคริปต์พรีเซนฉบับ 20 นาที** — กระชับเน้น key message | ~21 KB | ⭐ อัปโหลดก่อน |
| 04 | `04_Overview_Diagram.md` | **แผนภาพโครงสร้าง** — 8 Pillars, 26 topics, flow | ~5 KB | แนะนำ |
| 05 | `05_Why_ESTATEMENT_API_As_Standard.md` | **เหตุผลเลือก ESTATEMENT_API เป็น template** | ~18 KB | แนะนำ |
| 06 | `06_Glossary.md` | **อภิธานศัพท์** — คำศัพท์เทคนิคทั้งหมด | ~47 KB | เสริม |
| 07 | `07_Dapper_Migration_Analysis.md` | **วิเคราะห์ Dapper** — เปรียบเทียบ vs EF Core | ~27 KB | เสริม |
| 08 | `08_Database_Access_Methods_NET10.md` | **วิเคราะห์ Database Access** — methods ใน .NET 10 | ~27 KB | เสริม |
| 09 | `09_Template_README.md` | **คู่มือ Starter Template** — วิธีใช้ Template โปรเจค | ~6 KB | เสริม |
| 10 | `10_Template_GEMINI.md` | **Gemini AI Guidance** — guidelines สำหรับ AI ช่วยเขียนโค้ด | ~7 KB | เสริม |

---

## 🚀 วิธีใช้กับ NotebookLM

### ขั้นตอน
1. ไปที่ [notebooklm.google.com](https://notebooklm.google.com)
2. สร้าง Notebook ใหม่ ตั้งชื่อ **"Coding Standard — ASP.NET Core"**
3. อัปโหลดไฟล์ตามลำดับ ⭐ ก่อน แล้วค่อยเพิ่ม "แนะนำ" และ "เสริม"

### ⚠️ ข้อจำกัด NotebookLM
- **Source limit:** สูงสุด 50 sources ต่อ notebook
- **File size:** แต่ละไฟล์ไม่เกิน 500,000 words (~200 MB text)  
- ไฟล์ทั้ง 10 รวม ~411 KB — **ไม่เกิน limit**

### 💡 Prompt แนะนำสำหรับ NotebookLM

**สร้าง Audio Overview:**
> สร้าง podcast อธิบาย Coding Standard นี้สำหรับ developer ทีม ESB ใช้ภาษาไทย เน้นเหตุผลเบื้องหลังแต่ละเกณฑ์

**สรุปภาพรวม:**
> สรุป 295 เกณฑ์ เป็น 8 กลุ่มหลัก อธิบายแต่ละกลุ่มใน 2-3 ประโยค พร้อมระบุ reference สากลที่อ้างอิง

**เตรียม Q&A:**
> จากเอกสารทั้งหมด รวบรวมคำถามที่ management หรือ developer น่าจะถาม พร้อมคำตอบ อ้างอิงจากเนื้อหาในเอกสาร

**เตรียม Audit:**
> Map เกณฑ์ทั้งหมดกับ ISO 27001:2022, OWASP ASVS 5.0, CWE Top 25 แสดงเป็นตารางว่าแต่ละข้อตรงกับ reference ไหน

---

## 📝 หมายเหตุ
- ไฟล์ HTML (`Coding_Standard_Slides.html`, `Coding_Standard_Presentation.html`) ไม่ได้รวม เพราะ NotebookLM อ่าน Markdown ได้ดีกว่า HTML
- Template source code (`src/`, `tests/`) ไม่ได้รวม เพราะเป็น binary/code ไม่ใช่ text narrative
- ถ้าต้องการเพิ่ม source code เป็น reference ให้ export เป็น `.md` หรือ `.txt` ก่อนอัปโหลด
