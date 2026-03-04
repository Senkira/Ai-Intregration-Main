# การวิเคราะห์ NDID Save Draft - SMS Notification

## 📋 ข้อมูลจาก BCD Request #69020075

### ชื่อ BCD
**CI_BCD_69020075_SMS_InformSaveDraft_NDID_and_Revise_Wording_SMS**

### วัตถุประสงค์
1. เพิ่ม/แก้ไข SMS แจ้งเตือนการบันทึกร่าง NDID
2. แก้ไขถ้อยคำใน SMS ให้ชัดเจนขึ้น
3. เพิ่ม SMS Remind สำหรับเตือนซ้ำ

---

## 🔍 การวิเคราะห์เงื่อนไขการส่ง SMS Save Draft

### จากการวิเคราะห์ระบบ NDID ทั่วไป

**NDID (National Digital ID)** เป็นระบบยืนยันตัวตนดิจิทัลแห่งชาติ ที่ใช้ในการสมัครบริการต่างๆ เช่น:
- สมัครสินเชื่อ
- เปิดบัญชีธนาคาร
- สมัครบริการออนไลน์

### เงื่อนไขที่คาดว่าจะได้รับ SMS Save Draft

#### ✅ **ผู้ที่จะได้รับ SMS:**

1. **ลูกค้าที่บันทึกร่างคำขอ (Save Draft) ผ่าน NDID**
   - กรอกข้อมูลใบสมัครไม่เสร็จสมบูรณ์
   - กดปุ่ม "บันทึกร่าง" หรือ "Save Draft"
   - ระบบจะบันทึกข้อมูลที่กรอกไว้

2. **มีเบอร์โทรศัพท์ที่ถูกต้องในระบบ**
   - ต้องระบุเบอร์โทรศัพท์มือถือในใบสมัคร
   - เบอร์โทรต้องเป็นเบอร์ที่ใช้งานได้

3. **สถานะคำขอยังไม่สมบูรณ์**
   - คำขอยังไม่ได้ Submit
   - ยังไม่ได้ดำเนินการผ่าน NDID Verification
   - สถานะ = "Draft" หรือ "Incomplete"

4. **ช่วงเวลาที่กำหนด (สำหรับ SMS Remind)**
   - หลังจากบันทึกร่างแล้ว X วัน ยังไม่ดำเนินการต่อ
   - เช่น หลัง 3 วัน, 7 วัน, 14 วัน

#### ❌ **ผู้ที่จะไม่ได้รับ SMS:**

1. **ลูกค้าที่ Submit คำขอเรียบร้อยแล้ว**
   - กรอกข้อมูลครบและกด Submit สำเร็จ
   - ผ่านการยืนยันตัวตนด้วย NDID แล้ว

2. **ไม่มีเบอร์โทรศัพท์ในระบบ**
   - ไม่ได้กรอกเบอร์โทรศัพท์
   - เบอร์โทรผิดรูปแบบหรือไม่สามารถใช้งานได้

3. **ลูกค้ายกเลิกคำขอแล้ว (Cancel)**
   - กด Cancel Application
   - สถานะ = "Cancelled"

4. **คำขอหมดอายุ (Expired)**
   - เก็บร่างไว้นานเกินกำหนด (เช่น 30 วัน)
   - สถานะ = "Expired"

5. **ปิดการแจ้งเตือน (Opt-out)**
   - ลูกค้าเลือกไม่รับ SMS แจ้งเตือน
   - ระบุในการตั้งค่าบัญชี

---

## 📱 ตัวอย่าง SMS ที่อาจส่ง

### SMS แจ้งเตือนบันทึกร่าง (Save Draft Notification)
```
[EASY BUY] คุณได้บันทึกร่างคำขอสินเชื่อผ่าน NDID เรียบร้อยแล้ว 
กรุณากลับมาดำเนินการต่อภายใน 7 วัน 
เข้าสู่ระบบ: [Link]
Ref: APPLXXXXXXX
```

### SMS เตือนซ้ำ (Remind SMS)
```
[EASY BUY] เตือน: คุณมีคำขอสินเชื่อที่ยังไม่เสร็จสมบูรณ์ 
กรุณาดำเนินการภายใน 3 วัน มิฉะนั้นคำขอจะหมดอายุ
เข้าสู่ระบบ: [Link]
Ref: APPLXXXXXXX
```

---

## 🔧 Repositories ที่เกี่ยวข้อง

1. **CI_BCD_69020075_SMS_InformSaveDraft_NDID_and_Revise_Wording_SMS**
   - เอกสาร BCD สำหรับ SMS แจ้งเตือนหลัก

2. **CI_BCD_SMS_InformSaveDraft_NDID_and_Revise_Wording_SMS_Remind**
   - เอกสาร BCD สำหรับ SMS เตือนซ้ำ

3. **NDIDIntegate_API (NDID Project)**
   - API หลักสำหรับจัดการ NDID
   - Endpoints: `/api/MobileCust/ndidreqeust`, `/api/RPController`

4. **OnlineAppInternal_API (NDID Project)**
   - API สำหรับจัดการข้อมูลลูกค้าและคำขอ

---

## 📅 Timeline

- **23-24 ก.พ. 2026**: สร้าง Repository และเพิ่มเอกสาร BCD
- **27 ก.พ. 2026**: วันที่วิเคราะห์ข้อมูล (วันนี้)

---

## ⚠️ หมายเหตุ

การวิเคราะห์นี้อิงจาก:
1. ชื่อ BCD Repository
2. Commit messages
3. โครงสร้างโค้ดที่พบใน NDID API
4. Best practices ของระบบ NDID ทั่วไป

**สำหรับข้อมูลที่แน่ชัด** ควรดูจาก:
- เอกสาร BCD ฉบับเต็ม (อาจอยู่ใน SharePoint/Document Management System)
- Source Code ของ SMS Service/Notification Service
- Database Schema ของตาราง Application/NDID
