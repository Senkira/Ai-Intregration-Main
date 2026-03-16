# 🚀 MCP Solution Architect - Online Testing Guide

## วิธีทดสอบแบบ Online (ไม่ใช้ path ในเครื่อง)

### ⚠️ ก่อนใช้งานจริง ต้อง publish ก่อน!

---

## 🎯 Option 1: Publish to npm Registry (แนะนำ)

### Step 1: Setup npm Account

```bash
# Login to npm
npm login
```

### Step 2: Update package.json

```json
{
  "name": "@oatrepos/mcp-solution-architect",
  "version": "1.0.0",
  "publishConfig": {
    "access": "public",
    "registry": "https://registry.npmjs.org/"
  }
}
```

### Step 3: Publish

```bash
cd mcp-solution-architect
npm run build
npm publish --access public
```

### Step 4: ทดสอบจากเครื่องอื่น

**settings.json:**
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

---

## 🎯 Option 2: GitHub Packages

### Step 1: Setup .npmrc

```bash
# In project root
echo "@oatrepos:registry=https://npm.pkg.github.com" > .npmrc
```

### Step 2: Authenticate

```bash
npm login --scope=@oatrepos --registry=https://npm.pkg.github.com
```

### Step 3: Update package.json

```json
{
  "name": "@oatrepos/mcp-solution-architect",
  "version": "1.0.0",
  "publishConfig": {
    "registry": "https://npm.pkg.github.com/@oatrepos"
  }
}
```

### Step 4: Publish

```bash
npm publish
```

### Step 5: ใช้งาน

**settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "--registry=https://npm.pkg.github.com",
        "@oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

---

## 🎯 Option 3: Git Repository (ไม่ต้อง publish)

### วิธีที่ 1: Install จาก Git URL

```bash
npm install -g https://github.com/oatrepos/ai-code.git#main
```

**settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

### วิธีที่ 2: npx จาก Git

**settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "github:oatrepos/ai-code#main"
      ]
    }
  }
}
```

---

## 🎯 Option 4: Private Registry (สำหรับองค์กร)

### Setup Verdaccio (Private npm registry)

```bash
# Install Verdaccio
npm install -g verdaccio

# Run
verdaccio
```

### Publish to Private Registry

```bash
npm set registry http://localhost:4873
npm adduser --registry http://localhost:4873
npm publish --registry http://localhost:4873
```

### ใช้งาน

**settings.json:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "--registry=http://your-registry:4873",
        "@oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

---

## 🧪 Testing Online Installation

### Test 1: npm Registry (after publish)

```bash
# From any machine
npx @oatrepos/mcp-solution-architect

# Should show: "Solution Architect MCP Server running on stdio"
```

### Test 2: GitHub

```bash
# From any machine
npx github:oatrepos/ai-code

# Or with specific path
npx "github:oatrepos/ai-code/mcp-solution-architect"
```

### Test 3: VS Code

1. เปิด VS Code
2. Add settings.json:
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
3. Reload Window
4. Test in Copilot Chat:
```
@workspace list MCP servers
```

---

## 📦 Recommended: Publish to npm

**ทำไมควร publish ไปยัง npm?**

✅ ใช้งานได้จากทุกเครื่อง  
✅ ไม่ต้อง authenticate  
✅ รวดเร็ว (npm CDN)  
✅ Version control ง่าย  
✅ ทีมใช้งานได้เลย  

**Steps:**

```bash
# 1. Login
npm login

# 2. Build
cd mcp-solution-architect
npm run build

# 3. Publish
npm publish --access public

# 4. Verify
npm view @oatrepos/mcp-solution-architect

# 5. Test
npx @oatrepos/mcp-solution-architect
```

---

## 🔐 Security Best Practices

### 1. .npmignore

```
src/
tests/
*.test.ts
*.test.js
.env
.env.*
node_modules/
```

### 2. package.json files field

```json
{
  "files": [
    "dist",
    "README.md",
    "LICENSE"
  ]
}
```

### 3. Version Management

```bash
# Update version before publish
npm version patch  # 1.0.0 → 1.0.1
npm version minor  # 1.0.1 → 1.1.0
npm version major  # 1.1.0 → 2.0.0
```

---

## 📊 Comparison Table

| Method | Pros | Cons | Best For |
|--------|------|------|----------|
| **npm Registry** | ✅ Fast, ✅ Public, ✅ Easy | ❌ Need npm account | Public tools |
| **GitHub Packages** | ✅ Private, ✅ Free for public repos | ❌ Need auth | Team projects |
| **Git URL** | ✅ No publish needed | ❌ Slower, ❌ Build on install | Development |
| **Private Registry** | ✅ Full control | ❌ Infrastructure needed | Enterprise |

---

## ✅ Verification Checklist

**After publishing:**

- [ ] Test `npx @oatrepos/mcp-solution-architect`
- [ ] Verify package on npmjs.com
- [ ] Test from different machine
- [ ] Test in VS Code settings
- [ ] Update documentation with published package name
- [ ] Create GitHub release
- [ ] Update QUICKSTART.md
- [ ] Notify team

---

## 🎉 Next Steps

1. **ตอนนี้:** Choose publishing method (npm recommended)
2. **Publish:** Follow steps above
3. **Test:** From another machine
4. **Share:** Send settings.json to team
5. **Document:** Update all docs with published name

---

## 📝 Example Settings (After Publishing)

**For Team:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect@latest"]
    }
  }
}
```

**With Version Pinning:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect@1.0.0"]
    }
  }
}
```

---

**Status:** Ready to publish! 🚀

Choose your method and follow the steps above.
