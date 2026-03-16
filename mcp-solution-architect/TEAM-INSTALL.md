# 🚀 วิธีติดตั้ง MCP Solution Architect (สำหรับทีม)

## ทำตามขั้นตอนนี้เพื่อใช้งาน MCP Server

---

## 📋 ขั้นตอนการติดตั้ง

### Step 1: Clone Repository

```bash
git clone https://github.com/oatrepos/ai-code.git
cd ai-code/mcp-solution-architect
```

### Step 2: Install Dependencies

```bash
npm install
```

### Step 3: Build

```bash
npm run build
```

### Step 4: Link (ทำให้ใช้งานได้ทุกที่)

```bash
npm link
```

---

## ⚙️ Configure VS Code

### เปิด VS Code Settings

กด `Ctrl+Shift+P` (Windows) หรือ `Cmd+Shift+P` (Mac)

พิมพ์: `Preferences: Open User Settings (JSON)`

### วาง Config นี้

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

### Reload VS Code

กด `Ctrl+Shift+P` พิมพ์: `Developer: Reload Window`

---

## ✅ ทดสอบ

### 1. ทดสอบ CLI

```bash
mcp-solution-architect
```

ควรเห็น: `Solution Architect MCP Server running on stdio`

กด `Ctrl+C` เพื่อหยุด

### 2. ทดสอบใน VS Code

เปิด **Copilot Chat** และพิมพ์:

```
@workspace using solution-architect, analyze this requirement:
"ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203"
with constraint: "ห้ามแก้ code"
```

ควรได้ผลลัพธ์การวิเคราะห์พร้อม TOT methodology

---

## 🎯 คำสั่งที่ใช้ได้

### 1. Analyze Requirement (วิเคราะห์ความต้องการ)

```
@workspace analyze requirement: "[ความต้องการ]"
constraints: "[ข้อจำกัด]"
```

### 2. Generate Solution Table (สร้างแผนงาน)

```
@workspace generate solution table for: "[ความต้องการ]"
approach: "[วิธีการที่เลือก]"
```

### 3. Validate Solution (ตรวจสอบ)

```
@workspace validate solution with blocks: [solution blocks]
```

### 4. Audit Solution (ตรวจสอบขั้นสุดท้าย)

```
@workspace audit solution: "[solution document]"
```

### 5. Generate Task Document (สร้างเอกสารงาน)

```
@workspace generate task document for: "[ความต้องการ]"
```

---

## 🔧 Troubleshooting

### ปัญหา: command not found

**Solution:**
```bash
cd ai-code/mcp-solution-architect
npm link
```

### ปัญหา: Permission denied (Mac/Linux)

**Solution:**
```bash
sudo npm link
```

### ปัญหา: VS Code ไม่เห็น MCP

**Solution:**
1. ตรวจสอบ settings.json ถูกต้อง
2. Reload Window (`Ctrl+Shift+P` > `Developer: Reload Window`)
3. ตรวจสอบ Output panel (`View > Output > GitHub Copilot`)

---

## 📝 คำสั่งแบบเต็ม (Copy & Paste)

### Windows PowerShell

```powershell
# Clone
git clone https://github.com/oatrepos/ai-code.git
cd ai-code\mcp-solution-architect

# Install & Build
npm install
npm run build

# Link
npm link

# Test
mcp-solution-architect
# (กด Ctrl+C เพื่อหยุด)
```

### Mac/Linux

```bash
# Clone
git clone https://github.com/oatrepos/ai-code.git
cd ai-code/mcp-solution-architect

# Install & Build
npm install
npm run build

# Link
sudo npm link

# Test
mcp-solution-architect
# (กด Ctrl+C เพื่อหยุด)
```

---

## 🎉 เสร็จแล้ว!

ตอนนี้คุณสามารถใช้งาน MCP Solution Architect ใน VS Code ได้แล้ว

ทดสอบโดยเปิด Copilot Chat และพิมพ์:

```
@workspace list MCP servers
```

ควรเห็น `solution-architect` ในรายการ

---

## 💡 Tips

- **Update:** เมื่อมี version ใหม่ ให้ `git pull && npm run build`
- **Uninstall:** `npm unlink -g mcp-solution-architect`
- **Check version:** `npm list -g mcp-solution-architect`

---

**หากมีปัญหา:** ติดต่อทีม DevOps หรือดูเพิ่มเติมที่ README.md
