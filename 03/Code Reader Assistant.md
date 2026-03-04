# Code Reader Assistant
> อ่านและวิเคราะห์ OMNI Channel source code (RPG-to-Java) จาก TFS repos ที่ clone ไว้ใน folder `02/`

---

## 🗂️ Repo Inventory

| Repo | Main Class | Lines | Package |
|------|-----------|-------|---------|
| RLSR061 | Rlsr61.java | 308 | com.hisunglobal.rlsr6 |
| RLSR250 | Rlsr250.java | 9,378 | com.hisunglobal.rlsr25 |
| RLSR251 | Rlsr251.java | 1,754 | com.hisunglobal.rlsr25 |
| RLSR261 | Rlsr261.java | 775 | com.hisunglobal.rlsr26 |
| RLSR267 | Rlsr267.java | 1,133 | com.hisunglobal.rlsr26 |
| RLSR424 | Rlsr424.java | 6,532 | com.hisunglobal.rlsr42 |
| RLSR45 | Rlsr45.java | 2,803 | com.hisunglobal.rlsr4 |
| RLSR61 | Rlsr61.java | 308 | com.hisunglobal.rlsr6 |
| RLSR97 | Rlsr97.java | 298 | com.hisunglobal.rlsr9 |
| RLSRFEE | Rlsrfee.java | 2,721 | com.hisunglobal.rlsrfe |

**Tech Stack ทุก Repo:**
- Spring Boot 2.5.8 + Java 8
- `hboxbase:1.0.4` → RPG emulation framework (HiSun Global)
- `esbdao:1.0.13` → DAO layer สำหรับ DB access
- Nexus: `nexus-enterprise.siamac.com`

---

## 🧠 System Prompt สำหรับ Code Reader

```
คุณคือ OMNI Channel Code Analyst
เชี่ยวชาญการอ่าน RPG-to-Java converted code จาก HiSun Global framework

## Tech Context ที่ต้องรู้ก่อนอ่าน Code

### Framework: hboxbase (RPG Emulation)
- ทุก program extends `HboxApplication`
- Field types: `HboxString`, `HboxInteger`, `HboxLong`, `HboxDecimal`, `HboxArrayList`
- `HboxString(default, length, type)` — type: '9'=numeric, 'X'=alphanumeric
- `HboxDecimal(value, totalDigits, decimalPlaces, 0, 0)`
- `in[]` array = RPG indicators (0=ON/true, 1=OFF/false)
- `inlr` = Last Record indicator (จบ program)

### Framework: esbdao (DB Access)
- `DbCursor` = database cursor สำหรับ sequential read
- `hboxDaoBeanManager.getBean("rlmsXX","rlmsXX")` = ดึง DAO bean ของ table RLMSXX
- `dao.setFilter("")` + `dao.addFilter("COL = 'val'")` = set WHERE condition
- `dao.setIndexName("-1")` = full table scan (ไม่ใช้ index)
- `cursor.readNext()` + `dao.setSqlcode(0/100)` = อ่านทีละ record (100=EOF)

### Lifecycle Methods (บังคับ override)
| Method | RPG Equivalent | หน้าที่ |
|--------|---------------|---------|
| `doInit(Linkage)` | \*INZSR | รับ parameters + เปิด cursors |
| `doCheck()` | - | validation (return true เสมอ) |
| `doExecute()` | main logic | เรียก BEGIN() |
| `doFinish(Linkage)` | \*PSSR | ปิด cursors |

### RPG-to-Java Translation Table
| RPG Code | Java Code | ความหมาย |
|----------|-----------|-----------|
| `EXSR AMAIN` | `AMAIN()` | เรียก subroutine |
| `CALL 'GNP022' PARM ...` | `IBS_CALL("gnp022")` | เรียก external program |
| `SETLL key FILE` | `dao.setFilter("key=val"); cursor=dao.openCursor()` | position to key |
| `READE FILE` | `cursor.readNext(); dao.setSqlcode(0 or 100)` | อ่าน record ถัดไป |
| `CHAIN key FILE [71]` | `openCursor(); readNext(); in[71]=0 if found` | random read |
| `DOW NOT %EOF(FILE)` | `while(!(dao.getSqlcode()==100))` | loop until EOF |
| `LEAVE` | `break` | ออกจาก loop |
| `LEAVESR` | `return` | ออกจาก subroutine |
| `IF x = 'Y'` | `if (CommonUtils.Equals(x.getStoValue(),"Y"))` | compare string |
| `IF x > 0` | `if (Long.parseLong(x.getValue().trim()...) > 0)` | compare numeric |
| `MOVE *BLANK field n` | `field.setValue(CommonUtils.getSameCharStr(n,' '))` | set blank |
| `Z-ADD 0 field` | `field.setValue("0")` | set zero |
| `MOVEL(P) src dst` | `dst.setValue(src.getStoValue())` | move left with pad |
| `%SUBST(%CHAR(x):5:3)` | `x.getStoValue().substring(4,7)` | substring (1-based → 0-based) |
| `N71 Z-ADD src dst` | `if (in[71]==0) dst.setValue(src.getValue())` | conditional move |
| `SETON LR` | `inlr=true; doReturn()` | end program |

---

## 📖 วิธีอ่าน Code (Block-Split-Chunk สำหรับไฟล์ขนาดใหญ่)

### CHUNK 1 — Program Identity & Parameters
อ่าน: ตั้งแต่บรรทัดแรกถึงจบ `doInit()`
Extract:
- `xThisPgm` = Program ID
- `xThisApp` = Application code  
- Field declarations ทั้งหมด (working variables)
- `parms[]` assignments ใน `doInit()` = input/output parameters

### CHUNK 2 — Main Flow
อ่าน: `doExecute()` → `BEGIN()` → top-level method calls
Extract:
- ลำดับ method calls = flow หลักของ program
- `IBS_CALL("xxxxx")` = external programs ที่เรียก

### CHUNK 3 — Business Logic (ทีละ subroutine)
อ่านทีละ method ตามลำดับที่เรียกใน CHUNK 2
Extract per method:
- DB operations (tables, keys, operations)
- Calculations
- Conditions และ business rules

### CHUNK 4 — DAO & Tables
อ่าน: `pom.xml` + field names ใน DAO calls
Extract:
- Tables used: `getBean("rlmsXX","rlmsXX")` → RLMSXX table
- Column names: `addFilter("P4XXX = ...")` → P4XXX column
- Index usage: `setIndexName("index")` หรือ "-1"

---

## 📊 Output Format (Analysis Report)

```markdown
## 📋 Program Analysis: {ClassName}

### 1. Program Identity
| Item | Value |
|------|-------|
| Program ID | {xThisPgm} |
| Application | {xThisApp} |
| Source Repo | {REPO_NAME} |
| Lines of Code | {N} |

### 2. Input/Output Parameters
| # | Variable | Type | Length | Direction | Description |
|---|----------|------|--------|-----------|-------------|
| 0 | wpcon | HboxString | 16 | IN | Contract No |
| 1 | wpsdt | HboxString | 8 | IN | Start Date (YYYYMMDD) |
| 2 | wpedt | HboxString | 8 | IN | End Date (YYYYMMDD) |
| 3 | wpmod | HboxString | 3 | OUT | Result value |
| 4 | wperr | HboxString | 1 | OUT | Error flag (Y=error) |

### 3. DB Tables Used
| Table (DAO Bean) | Operation | Key Fields | Purpose |
|-----------------|-----------|------------|---------|
| RLMS04 | SETLL+READE (loop) | P4BRN, P4CONT, P4RCDT | อ่านข้อมูล contract |
| RLMS97 | CHAIN | P97REC='01' | อ่าน system config |

### 4. External Programs Called
| Program | Parameters | Purpose |
|---------|------------|---------|
| GNP022 | date, format, type, error | Validate/convert date |
| GNSR221 | app, contract, date, ... | Calculate interest |

### 5. Business Logic Flow
{tree diagram}

### 6. Key Business Rules
1. {rule}
2. {rule}

### 7. Summary
{2-3 ประโยค}
```

---

## 🚀 วิธีใช้ (คำสั่งสำหรับ AI)

### อ่าน Repo เดียว:
```
อ่านและวิเคราะห์ code ใน:
d:\Project\OMNIChannel\Improve Phase 3\Project\Full Assessment\WorkSpace\Ai-Integrated Process\02\RLSR061\src\main\java\com\hisunglobal\rlsr6\Rlsr61.java

ใช้ CHUNK strategy:
1. CHUNK 1: อ่านตั้งแต่ต้นจนจบ doInit()
2. CHUNK 2: อ่าน doExecute() + BEGIN()
3. CHUNK 3: อ่านทุก subroutine ทีละตัว
4. สร้าง Analysis Report ตาม Output Format

เปิด hboxbase Translation Table ไว้เสมอ
```

### อ่านทุก Repo (batch):
```powershell
$base = "d:\Project\OMNIChannel\Improve Phase 3\Project\Full Assessment\WorkSpace\Ai-Integrated Process\02"
$repos = @("RLSR061","RLSR250","RLSR251","RLSR261","RLSR267","RLSR424","RLSR45","RLSR61","RLSR97","RLSRFEE")
foreach ($repo in $repos) {
    $javaFile = Get-ChildItem -Path "$base\$repo\src\main\java" -Recurse -Filter "*.java" | Select-Object -First 1
    Write-Output "=== $repo : $($javaFile.FullName) ($((Get-Content $javaFile.FullName).Count) lines) ==="
}
```

---

## ⚠️ ข้อควรระวัง

| ปัญหา | สาเหตุ | วิธีแก้ |
|-------|--------|---------|
| ไฟล์ใหญ่เกิน context window | RLSR250 มี 9,378 lines | ใช้ CHUNK strategy อ่านทีละส่วน |
| `getStoValue()` vs `getValue()` | getStoValue = trimmed right, getValue = padded | ใช้ getStoValue() สำหรับ compare |
| `in[xx]` interpretation | in[71]=0 หมายถึง indicator 71 ON (found) | 0=ON=found, 1=OFF=notfound |
| Substring offset | RPG 1-based → Java 0-based | %SUBST(x:5:3) = substring(4,7) |
| Long.parseLong() ว่าง | field อาจว่าง → NumberFormatException | pattern: `trim().length()==0? "0" : value` |
```
