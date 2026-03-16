# เอกสารสั่งงาน: [Task-Appoint] เพิ่มเงื่อนไขการนัดหมายสำหรับ AMB123

**วันที่:** 26 กุมภาพันธ์ 2569
**โปรเจกต์:** OmniChannelServiceAPI


---

## 1. Objective

เพื่อเพิ่มเงื่อนไขการตรวจสอบลูกค้าที่ต้องการนัดหมาย โดยหากลูกค้ามีรหัสผลทาง Telesales เป็น `AMB123` ให้ API trả về status `203`

## 2. ขอบเขตและไฟล์ที่เกี่ยวข้อง (Scope of Work)

- **File ที่ต้องแก้ไข:** `OMNICHANNEL_API/src/OmniChannelServiceAPI/Services/OtpService.cs`
- **Method ที่ต้องแก้ไข:** `public ResponseOtpModel GetStatuResultCode(string code)`

## 3. ขั้นตอนการดำเนินงาน (Implementation Steps)

1.  **Checkout** Branch ใหม่จาก `develop` หรือ branch หลักของโปรเจกต์
2.  **เปิดไฟล์** `OMNICHANNEL_API/src/OmniChannelServiceAPI/Services/OtpService.cs`
3.  **ค้นหา Method** ที่ชื่อ `GetStatuResultCode(string code)`
4.  ภายใน Method ดังกล่าว ให้ **เพิ่ม block `else if`** เพื่อดักจับเงื่อนไขใหม่ **ก่อน** `else` block สุดท้าย

    ```csharp
    // โครงสร้างโค้ดเดิม
    if (code == null || code == "")
    {
        // ...
    }
    else if (_configuration["EagleEyeCode_Telesales:WI"].Split(',').Contains(code) || code == "AMB002" || code == "AMBB999")
    {
        // ... return status 201
    }
    else if (_configuration["EagleEyeCode_Telesales:DS"].Split(',').Contains(code) || code == "AMB000" || code == "AMB001")
    {
        // ... return status 202
    }
    
    // --> เพิ่ม Logic ใหม่ตรงนี้ <--

    else 
    {
        // ... return status 200
    }
    ```

5.  **เพิ่ม Logic** ใน `else if` block ใหม่ ให้ตรวจสอบว่า `code` ที่รับเข้ามามีค่าเท่ากับ `"AMB123"` หรือไม่
6.  หากเงื่อนไขเป็นจริง ให้ **Return `ResponseOtpModel` object ใหม่** โดยกำหนดค่าดังนี้:
    - `status = 203`
    - `success = true`
    - `message = "Verify Otp compelete"` (หรือข้อความตาม Standard เดิม)
7.  **ตรวจสอบให้แน่ใจ** ว่าไม่ได้ลบหรือแก้ไข Logic เดิมสำหรับเงื่อนไข `WI` (201), `DS` (202), และ `default` (200)
8.  **Build Project** เพื่อให้แน่ใจว่าไม่มี Compile Error
9.  **Commit** โค้ดที่แก้ไขและ Push branch เพื่อเปิด Pull Request

## 4. เกณฑ์การยอมรับ (Acceptance Criteria)

- เมื่อทำการทดสอบ (ผ่าน Postman หรือ Unit Test) โดยจำลองให้ `TelesaleResultCurrentCode` เป็น `AMB123` แล้วเรียก `AppointKYCVerify` จะต้องได้รับ Response ที่มี `status` เป็น `203`
- การทดสอบด้วย Code อื่นๆ (เช่น `AMB002`, `AMB001`) จะต้องได้ `status` เดิม (201, 202) ไม่เปลี่ยนแปลง
- การทดสอบด้วย Code ที่ไม่เข้าเงื่อนไขใดๆ จะต้องได้ `status` เป็น `200` ตามพฤติกรรมเดิม

## 5. ข้อควรระวัง (Out of Scope)

- **ห้าม** แก้ไข Logic การทำงานในส่วนอื่นๆ ที่นอกเหนือจาก Method `GetStatuResultCode`
- **ห้าม** เปลี่ยนแปลงค่าในไฟล์ `appsettings.json`
- **ห้าม** แก้ไขโครงสร้าง Database หรือ Entity Framework Models
