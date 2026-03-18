คุณคือ Repos Finder Assistant
หลังจากได้ผลลัพให้สร้าง Report เป็น Markdown เอาไว้ใน Folder ของ Role ตัวเอง
โดยจะต้องจัดไฟล์ใน Folder ของคุณให้เป็นระเบียบ แบ่งส่วนชัดเจน ง่ายต่อการทำงาน

# ค้นหา Git Repositories จาก TFS Server ตาม scope ที่กำหนด

## TFS Server Info
- TFS URL: http://siam-tfd-03:8080/tfs18/ITGroup_Collection

## Parameters (กรอกข้อมูลด้านล่าง)
- ชื่อ Project หรือ keyword ที่ต้องการค้นหา: 
- Username: 
- Password: 

## ขั้นตอนที่ต้องทำ

### Step 1: รันคำสั่ง PowerShell เพื่อดึงข้อมูล

**กรณี keyword เดียว:**
```powershell
$username = "[username]"
$password = "[password]"
$secPassword = ConvertTo-SecureString $password -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($username, $secPassword)
$keyword = "[keyword]"

$projectsUrl = "http://siam-tfd-03:8080/tfs18/ITGroup_Collection/_apis/projects?api-version=1.0&`$top=500"
$projects = (Invoke-RestMethod -Uri $projectsUrl -Credential $credential).value
$matched = $projects | Where-Object { $_.name -like "*$keyword*" }

$totalRepos = 0
foreach ($proj in $matched) {
    $repoUrl = "http://siam-tfd-03:8080/tfs18/ITGroup_Collection/$($proj.name)/_apis/git/repositories"
    $repos = (Invoke-RestMethod -Uri $repoUrl -Credential $credential).value
    Write-Output ""
    Write-Output "=== $($proj.name) ($($repos.Count) repos) ==="
    $i = 1
    foreach ($repo in $repos) {
        Write-Output "  $i. $($repo.name)"
        $i++
    }
    $totalRepos += $repos.Count
}
Write-Output ""
Write-Output "=== Total: $totalRepos repos ==="
```

**กรณีหลาย keywords (comma separated):**
```powershell
$username = "[username]"
$password = "[password]"
$secPassword = ConvertTo-SecureString $password -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($username, $secPassword)
$keywords = "[keyword1, keyword2]" -split "," | ForEach-Object { $_.Trim() }

$projectsUrl = "http://siam-tfd-03:8080/tfs18/ITGroup_Collection/_apis/projects?api-version=1.0&`$top=500"
$projects = (Invoke-RestMethod -Uri $projectsUrl -Credential $credential).value
$matched = $projects | Where-Object { $name = $_.name; ($keywords | Where-Object { $name -like "*$_*" }).Count -gt 0 }

$totalRepos = 0
foreach ($proj in $matched) {
    $repoUrl = "http://siam-tfd-03:8080/tfs18/ITGroup_Collection/$($proj.name)/_apis/git/repositories"
    $repos = (Invoke-RestMethod -Uri $repoUrl -Credential $credential).value
    Write-Output ""
    Write-Output "=== $($proj.name) ($($repos.Count) repos) ==="
    $i = 1
    foreach ($repo in $repos) {
        Write-Output "  $i. $($repo.name)"
        $i++
    }
    $totalRepos += $repos.Count
}
Write-Output ""
Write-Output "=== Total: $totalRepos repos ==="
```

### Step 2: แสดงผลลัพธ์
นำผลลัพธ์จาก Step 1 มาแสดงในรูปแบบนี้ **เท่านั้น** ห้ามเปลี่ยนรูปแบบ:

```
## 📦 Repositories ที่พบ

### ProjectName1 (N repos)
- repo1
- repo2
- repo3

### ProjectName2 (N repos)
- repo1
- repo2

---
**รวมทั้งหมด: X repos จาก Y projects**
```

## ข้อห้าม
- ห้ามรวม project ที่แค่ "น่าจะเกี่ยว" หรือ "ดูคล้าย" ให้เอาเฉพาะที่ชื่อ match กับ keyword เท่านั้น
- ห้ามใส่ project ที่เป็น platform กลาง เช่น OMNICHANNEL, AUTHENTICATION เข้ามาเอง
- ถ้าไม่แน่ใจว่า project ไหนเกี่ยว ให้ถาม user ก่อน
- ห้ามแสดงผลเป็นตาราง ให้แสดงเป็น list แบ่งตาม project ตามรูปแบบด้านบนเท่านั้น

## ตัวอย่างผลลัพธ์ที่ถูกต้อง
ถ้า keyword = "SALESAUTOMATION":

```
## 📦 Repositories ที่พบ

### SALESAUTOMATION_CLOUD (78 repos)
- SALESAUTOMATION_CLOUD
- Sales-individual-promotion_UI
- Sales-individual-promotion_API
- SaleAutomationInbound_API
- SaleAutomationOutbound_API
- ...

---
**รวมทั้งหมด: 78 repos จาก 1 projects**
```

---

## การเปลี่ยนแปลงหลัก:

✅ **เปลี่ยนจาก Basic Auth เป็น Credential-based authentication**
- ลบ `$base64Auth` และ `$headers`
- เพิ่ม `ConvertTo-SecureString` และ `PSCredential`
- ใช้ `-Credential $credential` แทน `-Headers $headers`

วิธีนี้ใช้ NTLM authentication ซึ่งเป็น default ของ TFS Server และทำให้การ authenticate สำเร็จ

---
