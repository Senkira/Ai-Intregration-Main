# ✅ วิธีใช้งาน MCP Server แบบ Online (ไม่ใช้ path ในเครื่อง)

## 🎯 สำหรับทีม: 3 วิธีที่ใช้งานได้จริง

---

## 1️⃣ Publish to npm (แนะนำที่สุด!)

### ขั้นตอน:

```bash
# 1. Login to npm (ครั้งเดียว)
npm login

# 2. Build & Publish
cd mcp-solution-architect
npm run build
npm publish --access public
```

### ใช้งาน (จากเครื่องอื่นๆ):

**VS Code settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"]
    }
  }
}
```

**ทดสอบ CLI:**
```bash
npx @oatrepos/mcp-solution-architect
```

✅ **ข้อดี:** ใช้งานได้ทันที, รวดเร็ว, ไม่ต้อง config อะไร  
❌ **ข้อเสีย:** ต้องมี npm account

---

## 2️⃣ Docker Image (สำหรับแชร์ในทีม)

### Build และ Save:

```bash
cd mcp-solution-architect
docker build -t mcp-solution-architect:1.0.0 .

# Save to file
docker save mcp-solution-architect:1.0.0 > mcp-sa.tar
```

### แชร์ให้ทีม:

1. แชร์ไฟล์ `mcp-sa.tar` ผ่าน network drive
2. ทีมโหลดแล้วรัน:

```bash
docker load < mcp-sa.tar
```

### ใช้งาน:

**VS Code settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "docker",
      "args": [
        "run",
        "-i",
        "--rm",
        "mcp-solution-architect:1.0.0"
      ]
    }
  }
}
```

**ทดสอบ CLI:**
```bash
docker run -i --rm mcp-solution-architect:1.0.0
```

✅ **ข้อดี:** ไม่ต้อง install dependencies, consistent environment  
❌ **ข้อเสีย:** ต้องมี Docker

---

## 3️⃣ Git Repository (สำหรับ development)

### Clone และ Link:

```bash
# 1. Clone
git clone https://github.com/oatrepos/ai-code.git
cd ai-code/mcp-solution-architect

# 2. Install & Build
npm install
npm run build

# 3. Link globally
npm link
```

### ใช้งาน:

**VS Code settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

✅ **ข้อดี:** สามารถแก้ไข code ได้  
❌ **ข้อเสีย:** ต้อง clone และ build ในทุกเครื่อง

---

## 📊 ตารางเปรียบเทียบ

| วิธี | Setup Time | Easy to Share | Updates | Best For |
|------|-----------|---------------|---------|----------|
| **npm** | ⚡ 1 นาที | ✅ Copy settings | ✅ Auto | Production |
| **Docker** | 🐌 5 นาที | ✅ Share .tar | Manual | Team |
| **Git** | 🐌 10 นาที | ❌ Each clone | ✅ Git pull | Development |

---

## 🚀 แนะนำ: Publish to npm

**ทำครั้งเดียว (maintainer):**

```bash
cd mcp-solution-architect
npm login  # ใช้ npm account
npm run build
npm publish --access public
```

**ทีมใช้งาน (ทุกคน):**

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"]
    }
  }
}
```

**เสร็จ!** ใช้งานได้เลย ไม่ต้อง config path อะไรเลย

---

## 📝 Example Settings (Production Ready)

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-solution-architect@1.0.0"
      ],
      "env": {
        "NODE_OPTIONS": "--max-old-space-size=4096"
      }
    }
  }
}
```

---

## ✅ Checklist สำหรับ Publish

- [ ] Test locally: `npm run build && node dist/index.js`
- [ ] npm login
- [ ] npm publish --access public  
- [ ] Test: `npx @oatrepos/mcp-solution-architect`
- [ ] Verify: npm view @oatrepos/mcp-solution-architect
- [ ] Share settings.json with team
- [ ] Update documentation

---

## 🎯 Next Steps

1. **ตอนนี้ (ถ้าต้องการให้คนอื่นใช้ได้เลย):**
   ```bash
   npm login
   npm publish --access public
   ```

2. **หรือ (ถ้าต้องการทดสอบก่อน):**
   ```bash
   docker build -t mcp-sa .
   docker save mcp-sa > mcp-sa.tar
   # แชร์ mcp-sa.tar ให้ทีม
   ```

3. **หรือ (ถ้า development only):**
   ```bash
   npm link
   # แต่ละคนต้อง clone และ link เอง
   ```

---

**คำแนะนำ:** ใช้วิธี **npm publish** จะง่ายที่สุด!

```bash
npm publish --access public
```

แค่นี้ทั้งทีมใช้งานได้เลย! 🎉
