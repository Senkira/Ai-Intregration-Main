# OMNICHANNEL — Database Connection Analysis Report

**วันที่วิเคราะห์:** 26 กุมภาพันธ์ 2569  
**วิเคราะห์โดย:** Repos Finder Assistant  
**TFS Project:** OMNICHANNEL  
**Repos ที่วิเคราะห์:** OMNICHANNEL_API, OMNITRANSACTIONLOG_API, OMNICHANNEL  
**Repos ตัวอย่าง (RL Suite):** RLSR061, RLSR250, RLSR61, RLSR97 และอื่นๆ ใน project RLOP0001 / CI_RLOP0001

---

## 📦 Repositories ที่พบใน Project OMNICHANNEL

### OMNICHANNEL (3 repos)
- OMNITRANSACTIONLOG_API
- OMNICHANNEL_API
- OMNICHANNEL

---
**รวมทั้งหมด: 3 repos จาก 1 projects**

---

## 🏗️ สถาปัตยกรรมโดยรวม (Architecture Overview)

OMNICHANNEL ใช้ **Custom ESB Framework** ชื่อ **HboxBase** ที่พัฒนาโดย HisunGlobal  
บนพื้นฐานของ **Spring Boot 2.5.x** + **Spring JDBC**

```
[Client/Frontend]
       │
       ▼
[ESB Layer — HboxCoreServer]
       │
       ▼
[Application Module — extends HboxApplication]
  (เช่น Rlsr61, Rlsr250, Rlsr097 ...)
       │
       ▼
[DAO Layer — esbdao library (com.hisunglobal)]
  (เช่น Rlms04, Rlms97, Rltb08, Csms97 ...)
       │
       ▼
[DbCursor — Spring JdbcTemplate ภายใน]
       │
       ▼
[Database — SQL Server / DB2 / Oracle]
```

---

## 🔌 วิธีการ Connect Database (Database Connection Pattern)

### 1. Framework Layer: HboxBase + esbdao

ทุก module ไม่ได้ connect database โดยตรง แต่ผ่าน **2 library หลัก:**

| Library | GroupId | Version | หน้าที่ |
|---------|---------|---------|---------|
| `hboxbase` | `com.hisunglobal` | 1.0.4 | Core framework, transaction, cursor |
| `esbdao` | `com.hisunglobal` | 1.0.13 | DAO objects สำเร็จรูปสำหรับแต่ละ table |

ทั้งสอง library อยู่ใน **Nexus private repository:**  
`https://nexus-enterprise.siamac.com/repository/maven-public`

---

### 2. Spring Boot DataSource Auto-Configuration

**pom.xml** ของทุก module (เช่น `esbrlsr061`) ใช้:

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-jdbc</artifactId>
</dependency>
```

ซึ่งหมายความว่า Spring Boot จะ **auto-configure DataSource** จาก `application.properties`  
โดยใช้ **HikariCP** เป็น connection pool (default ของ Spring Boot 2.x)

**application.properties** ใน source code ถูกเก็บว่าง (blank) โดยเจตนา  
เพราะ config จริงจะถูก **inject ตอน deploy ผ่าน Kubernetes ConfigMap / Secret**

---

### 3. DAO Pattern — DbCursor

ตัวอย่างจาก `Rlsr61.java`:

```java
// Inject DAO object ผ่าน HboxDaoBeanManager
rlms97 = (Rlms97) hboxDaoBeanManager.getBean("rlms97", "rlms97");
rlms04 = (Rlms04) hboxDaoBeanManager.getBean("rlms04", "rlms04");

// Query ผ่าน filter + cursor
rlms04.setFilter("");
rlms04.addFilter("P4BRN = '" + wkbrnn.getValue() + "'");
rlms04.addFilter("P4CONT = '" + wpcon.getValue() + "'");
rlms04.setIndexName("-1");
DbCursor rlms04Cursor = rlms04.openCursor();

// Read row
if (rlms04Cursor.readNext()) rlms04.setSqlcode(0);
else rlms04.setSqlcode(100);  // EOF = sqlcode 100 (เหมือน IBM AS400/RPG)
```

**Pattern นี้คือการ Wrap JDBC ให้ทำงานเหมือน AS400/RPG Cursor:**
- `setFilter()` / `addFilter()` → WHERE clause
- `openCursor()` → execute query
- `readNext()` → fetch next row
- `sqlcode == 100` → End of File (EOF)

---

### 4. Transaction Management

```java
import org.springframework.transaction.support.DefaultTransactionDefinition;
import org.springframework.transaction.TransactionDefinition;
```

ใช้ **Spring Transaction Management** มาตรฐาน  
Transaction ถูกจัดการโดย `HboxCoreServer` ใน framework layer

---

### 5. Database Driver ที่รองรับ

จาก `effective-pom.xml` พบว่า platform รองรับหลาย database:

| Database | Driver Version |
|----------|---------------|
| **MS SQL Server** | `mssql-jdbc:9.2.1.jre8` |
| **Oracle** | `ojdbc:19.3.0.0` |
| **IBM DB2** | `db2-jdbc:11.5.7.0` |
| **MariaDB** | `mariadb:2.7.4` |
| **MySQL** | `mysql:8.0.27` |

> ⚠️ Database จริงที่ใช้งานต้อง config ใน `application.properties` ตอน deploy

---

### 6. Multi-DataSource Pattern (Multiple Schema)

จาก imports ใน `Rlsr250.java` พบว่า module เดียวสามารถ access หลาย schema:

```java
import com.hisunglobal.dao.rlt.rltb08.Rltb08;   // Schema: rlt (RL Transaction)
import com.hisunglobal.dao.csm.csms97.Csms97;    // Schema: csm (Customer Master)
import com.hisunglobal.dao.rlm.rlms09.Rlms09;    // Schema: rlm (RL Master)
import com.hisunglobal.dao.csm.csmsact.Csmsact;  // Schema: csm (Customer Account)
import com.hisunglobal.dao.gnt.gntb105.Gntb105;  // Schema: gnt (General Transaction)
import com.hisunglobal.dao.gnm.gnmx02.Gnmx02;    // Schema: gnm (General Master)
```

**DAO Naming Convention:**
```
com.hisunglobal.dao.[schema].[tablename].[ClassName]
                     ├── rlm = RL Master
                     ├── rlt = RL Transaction  
                     ├── csm = Customer Master
                     ├── gnt = General Transaction
                     └── gnm = General Master
```

---

### 7. Deployment — Config Injection via Docker/Kubernetes

จาก `Dockerfile`:

```dockerfile
FROM maven:3.2.3-jdk-8
COPY . .
RUN mvn deploy -f ${POM}
```

และ Jenkinsfile ใช้ pattern:

```groovy
environment {
    SERVERIP = "172.26.249.11"
}
stage('PUSH RELEASE') {
    sh "docker build --build-arg POM='${POM_RELEASE}' -t ${PROJECTNAME}:${VERSION_RELEASE} ..."
}
```

**application.properties จริงถูก inject ตอน run container** ไม่ได้เก็บใน source code  
เพื่อความปลอดภัย (credentials ไม่อยู่ใน git)

---

## 📊 สรุป Database Connection Flow

```
1. [Dev] Code ใช้ DAO class จาก esbdao library
         ↓
2. [Runtime] HboxDaoBeanManager.getBean() inject DAO bean
         ↓
3. [Spring Boot] Auto-configure DataSource + HikariCP pool
         ↓
4. [Deploy] application.properties inject ผ่าน Kubernetes Secret/ConfigMap
         ↓
5. [Database] ต่อผ่าน JDBC driver ที่ configured
              (SQL Server / Oracle / DB2 ตาม environment)
```

---

## 🔑 Key Findings

| ประเด็น | รายละเอียด |
|---------|-----------|
| **Framework** | HboxBase (custom ESB ของ HisunGlobal) บน Spring Boot 2.5.8 |
| **Connection Pool** | HikariCP (Spring Boot default) |
| **DAO Pattern** | DbCursor — เลียนแบบ IBM AS400/RPG Cursor |
| **Transaction** | Spring Transaction Management |
| **Config Management** | application.properties ว่างใน source, inject ตอน deploy |
| **Multi-schema** | รองรับ — DAO แยกตาม schema prefix (rlm, rlt, csm, gnm, gnt) |
| **Build/Deploy** | Docker + Jenkins + Nexus Artifact Repository |

---

*Report นี้วิเคราะห์จาก source code ของ repos ใน TFS Project OMNICHANNEL และ RLOP0001*
