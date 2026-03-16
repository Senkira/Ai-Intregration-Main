# วิธีทดสอบ MCP Server แบบ Online (ไม่ใช้ path ในเครื่อง)

## 🚀 วิธีเร็วที่สุด: Publish to npm

### Step 1: Login to npm

```bash
npm login
```

(หรือสร้าง account ที่ https://www.npmjs.com/ ก่อน)

### Step 2: Build & Publish

```bash
cd mcp-solution-architect
npm run build
npm publish --access public
```

### Step 3: ทดสอบจากเครื่องอื่น

**ทดสอบ CLI:**
```bash
npx @oatrepos/mcp-solution-architect
```

**ทดสอบใน VS Code:**

settings.json:
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

---

## ✅ Verify

```bash
# Check published package
npm view @oatrepos/mcp-solution-architect

# Test installation
npx @oatrepos/mcp-solution-architect

# Should output: "Solution Architect MCP Server running on stdio"
```

---

## 🎯 ตอนนี้ทำอะไรได้บ้าง?

### ถ้ายังไม่ publish:

**Option A: ใช้ Git URL (ช้ากว่า)**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["github:oatrepos/ai-code"]
    }
  }
}
```

**Option B: Local test only**
```bash
# Link locally
cd mcp-solution-architect
npm link

# Then in VS Code:
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

---

## 📝 Publishing Checklist

- [ ] npm login (ต้องมี npm account)
- [ ] npm run build
- [ ] npm publish --access public
- [ ] Test: npx @oatrepos/mcp-solution-architect
- [ ] Update QUICKSTART.md with published name
- [ ] Share settings.json with team

---

**Next:** Run `npm publish --access public` เพื่อให้คนอื่นใช้งานได้!
