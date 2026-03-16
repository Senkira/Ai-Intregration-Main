# Solution Architect MCP Server - Installation Guide

## 🚀 วิธีติดตั้งและใช้งานจากเครื่องอื่น

### Option 1: Install via npm (แนะนำ)

```bash
# Install globally from npm (ถ้า publish แล้ว)
npm install -g @oatrepos/mcp-solution-architect

# หรือ install จาก GitHub
npm install -g https://github.com/oatrepos/ai-code.git#main:mcp-solution-architect
```

**VS Code Settings:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

---

### Option 2: Install via npx (ไม่ต้อง install)

**VS Code Settings:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["@oatrepos/mcp-solution-architect"]
    }
  }
}
```

---

### Option 3: Clone และ Link (สำหรับ development)

```bash
# 1. Clone repo
git clone https://github.com/oatrepos/ai-code.git
cd ai-code/mcp-solution-architect

# 2. Install dependencies
npm install

# 3. Build
npm run build

# 4. Link globally
npm link
```

**VS Code Settings (Windows):**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": ["C:/path/to/ai-code/mcp-solution-architect/dist/index.js"]
    }
  }
}
```

**VS Code Settings (Mac/Linux):**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": ["/path/to/ai-code/mcp-solution-architect/dist/index.js"]
    }
  }
}
```

---

### Option 4: HTTP Server (ใช้ผ่าน network)

```bash
# On server machine
cd mcp-solution-architect
npm run build
PORT=3000 node dist/http-server.js
```

**Access from other machines:**
```bash
curl http://server-ip:3000/health
curl -X POST http://server-ip:3000/api/analyze \
  -H "Content-Type: application/json" \
  -d '{"requirement":"Add AMB123 condition","constraints":["no code changes"]}'
```

---

## 📝 Configuration Examples

### ตัวอย่าง settings.json สำหรับทีม

#### สำหรับ Windows

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-solution-architect"
      ],
      "env": {
        "NODE_OPTIONS": "--max-old-space-size=4096"
      }
    }
  }
}
```

#### สำหรับ Mac/Linux

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-solution-architect"
      ],
      "env": {
        "NODE_OPTIONS": "--max-old-space-size=4096"
      }
    }
  }
}
```

#### สำหรับ Docker (Advanced)

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "docker",
      "args": [
        "run",
        "--rm",
        "-i",
        "oatrepos/mcp-solution-architect:latest"
      ]
    }
  }
}
```

---

## 🔧 Troubleshooting

### ปัญหา: Command not found

**Solution:**
```bash
# ตรวจสอบว่าติดตั้งแล้ว
npm list -g @oatrepos/mcp-solution-architect

# ถ้ายังไม่ได้ติดตั้ง
npm install -g @oatrepos/mcp-solution-architect
```

### ปัญหา: Permission denied (Mac/Linux)

**Solution:**
```bash
sudo npm install -g @oatrepos/mcp-solution-architect
# หรือ
npm config set prefix ~/.npm-global
export PATH=~/.npm-global/bin:$PATH
```

### ปัญหา: Port already in use (HTTP Server)

**Solution:**
```bash
# เปลี่ยน port
PORT=3001 node dist/http-server.js
```

---

## 📦 Publishing to npm (สำหรับ maintainer)

```bash
# 1. Login to npm
npm login

# 2. Publish
npm publish --access public

# 3. Test installation
npm install -g @oatrepos/mcp-solution-architect
```

---

## 🐳 Docker Deployment (Advanced)

**Dockerfile:**
```dockerfile
FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm ci --production

COPY dist ./dist

CMD ["node", "dist/index.js"]
```

**Build & Run:**
```bash
docker build -t mcp-solution-architect .
docker run -i mcp-solution-architect
```

---

## 🌐 ใช้งานผ่าน Network

### Start HTTP Server

```bash
cd mcp-solution-architect
PORT=3000 node dist/http-server.js
```

### API Endpoints

#### Health Check
```bash
curl http://localhost:3000/health
```

#### Analyze Requirement
```bash
curl -X POST http://localhost:3000/api/analyze \
  -H "Content-Type: application/json" \
  -d '{
    "requirement": "ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203",
    "constraints": ["ห้ามแก้ code"],
    "context": "OMNICHANNEL_API"
  }'
```

#### Generate Solution Table
```bash
curl -X POST http://localhost:3000/api/solution-table \
  -H "Content-Type: application/json" \
  -d '{
    "requirement": "Add AMB123 condition",
    "selectedApproach": "API Gateway"
  }'
```

---

## ✅ Verification

### Test การติดตั้ง

```bash
# Test command
mcp-solution-architect --help

# หรือ
npx @oatrepos/mcp-solution-architect --help

# Test ใน VS Code
# 1. Reload VS Code
# 2. Open Copilot Chat
# 3. พิมพ์: @workspace list available MCP tools
```

---

## 🔐 Security Notes

### สำหรับ HTTP Server

- ใช้ใน development environment เท่านั้น
- สำหรับ production ควรเพิ่ม:
  - Authentication (JWT, API Key)
  - HTTPS/TLS
  - Rate limiting
  - CORS configuration

### Environment Variables

```bash
# .env file
PORT=3000
NODE_ENV=production
API_KEY=your-secret-key
ALLOWED_ORIGINS=http://localhost:3001,https://your-domain.com
```

---

## 📚 Additional Resources

- **GitHub:** https://github.com/oatrepos/ai-code
- **npm:** https://www.npmjs.com/package/@oatrepos/mcp-solution-architect
- **Documentation:** See README.md in the package

---

## 💡 Tips

1. **ใช้ npx แทน global install** - ง่ายกว่า ไม่ต้อง maintain version
2. **ใช้ package.json scripts** - เพิ่ม script ในโปรเจกต์เพื่อให้ทีมใช้งานง่าย
3. **Version pinning** - ระบุ version ใน settings.json เพื่อความเสถียร

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

**Last Updated:** March 16, 2026
