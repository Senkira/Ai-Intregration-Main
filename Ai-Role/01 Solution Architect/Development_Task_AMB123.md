# Development Task: Implement Rule AMB123

**รหัสเอกสาร:** Development_Task_AMB123
**วันที่:** 26 กุมภาพันธ์ 2569
**ผู้รับผิดชอบ:** ทีม Developer / DevOps

---

## 1. วัตถุประสงค์ (Objective)

เพื่อ Implement เงื่อนไขทางธุรกิจใหม่ โดยให้ระบบตอบกลับด้วย `HTTP Status 203 Non-Authoritative Information` สำหรับลูกค้าประเภท **AMB123** ที่ร้องขอการ "นัดยื่นเอกสาร" ผ่าน API

## 2. ขอบเขตของงาน (Scope of Work)

- **In Scope:**
    - วิเคราะห์ API Request เพื่อระบุ Endpoint และ Field ที่เกี่ยวข้อง
    - เพิ่ม/แก้ไขการตั้งค่า (Configuration) บน **API Gateway** เท่านั้น
    - ทดสอบการทำงานของ Rule ที่สร้างขึ้นใหม่

- **Out of Scope:**
    - **ห้ามแก้ไขโค้ดใน Repository `OMNICHANNEL_API` โดยเด็ดขาด**
    - ห้ามแก้ไข Database Schema หรือข้อมูลใดๆ
    - ห้ามแก้ไขการทำงานของ API Endpoint อื่นๆ ที่ไม่เกี่ยวข้อง

## 3. ขั้นตอนการดำเนินงาน (Implementation Steps)

**Block 1: Analyze Request Signature**

1.  **Identify API Endpoint:**
    *   ตรวจสอบและยืนยัน Endpoint ที่เกี่ยวข้องกับการ "นัดยื่นเอกสาร"
    *   **คาดการณ์:** `POST /api/v1/appointments` (หรือใกล้เคียง)

2.  **Identify Customer Type Field:**
    *   ตรวจสอบโครงสร้าง JSON Payload ของ Request ที่ส่งมายัง Endpoint ข้างต้น
    *   ค้นหา Field ที่ใช้สำหรับระบุประเภทของลูกค้า
    *   **คาดการณ์:** `customer.type`, `customerInfo.customerType`, หรือ `customer_type`
    *   **ตัวอย่าง Payload ที่คาดหวัง:**
        ```json
        {
          "appointment_details": { ... },
          "customer_info": {
            "type": "AMB123",
            "id": "C123456"
          }
        }
        ```

**Block 2: Implement Gateway Rule**

3.  **Add Configuration Rule to API Gateway:**
    *   เข้าถึงส่วนการกำหนดค่าของ API Gateway ที่ใช้งานอยู่ (เช่น NGINX, Kong, Azure APIM)
    *   สร้าง Rule ใหม่ที่ทำงานบน Path และ Method ของ Endpoint ที่ระบุใน Block 1
    *   กำหนด Logic การทำงานของ Rule ดังนี้:
        > **IF** `Request Method` is `POST`
        > **AND** `Request Path` is `/api/v1/appointments`
        > **AND** `Request Body` contains a field `customer_info.type` with value `AMB123`
        > **THEN**
        >   **Stop** processing the request (do not forward to OMNICHANNEL_API)
        >   **Immediately Respond** with `HTTP Status 203 Non-Authoritative Information` and an empty body.

**Block 3: Deploy & Verify**

4.  **Deploy New Gateway Configuration:**
    *   นำการตั้งค่าใหม่ของ API Gateway ขึ้นระบบ (อาจเป็นการ Reload Config หรือ Deploy ใหม่ ขึ้นอยู่กับระบบ)

5.  **Create and Execute Test Plan:**
    *   **Test Case 1: Happy Path (Non-AMB123 Customer)**
        *   **Action:** ส่ง Request ไปยัง Endpoint พร้อม Payload ของลูกค้าประเภทอื่น (เช่น `NORMAL`)
        *   **Expected Result:** Request ถูกส่งต่อไปยัง `OMNICHANNEL_API` และได้รับ `HTTP 200 OK` หรือ `201 Created` ตามปกติ

    *   **Test Case 2: Target Path (AMB123 Customer)**
        *   **Action:** ส่ง Request ไปยัง Endpoint พร้อม Payload ของลูกค้าประเภท `AMB123`
        *   **Expected Result:** API Gateway ตอบกลับด้วย `HTTP 203 Non-Authoritative Information` ทันที และ Request **ไม่ถูกส่งต่อ** ไปยัง `OMNICHANNEL_API` (สามารถตรวจสอบได้จาก Log ของแอปพลิเคชัน)

## 4. เกณฑ์การยอมรับ (Acceptance Criteria)

- Test Case ทั้ง 2 กรณีในข้อ 5 ผ่านทั้งหมด
- ไม่มีผลกระทบกับการทำงานของ API Endpoint อื่นๆ
- ไม่มี Log Error ที่เกี่ยวข้องกับการเปลี่ยนแปลงนี้บน `OMNICHANNEL_API`
