# 🧪 Testing MCP Server Online (Without Local Path)

## วิธีทดสอบแบบ Online โดยไม่ใช้ path ในเครื่อง

### 🎯 Option 1: HTTP Server (ใช้งานได้เลยตอนนี้!)

**Start server บนเครื่องที่มี source code:**

```bash
cd mcp-solution-architect
npm run build
PORT=3000 node dist/http-server.js
```

**Server จะรันที่:**
```
http://localhost:3000
```

**ถ้าต้องการให้เครื่องอื่นเข้าถึงได้:**
- เปิด port 3000 ใน firewall
- ใช้ IP address ของเครื่อง (เช่น `http://192.168.1.100:3000`)

---

### 🌐 Test จากเครื่องอื่น

#### Test 1: Health Check

```bash
curl http://server-ip:3000/health
```

**ผลลัพธ์:**
```json
{
  "status": "ok",
  "service": "mcp-solution-architect",
  "version": "1.0.0"
}
```

#### Test 2: Analyze Requirement

```bash
curl -X POST http://server-ip:3000/api/analyze \
  -H "Content-Type: application/json" \
  -d '{
    "requirement": "ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203",
    "constraints": ["ห้ามแก้ code"],
    "context": "OMNICHANNEL_API"
  }'
```

#### Test 3: Generate Solution Table

```bash
curl -X POST http://server-ip:3000/api/solution-table \
  -H "Content-Type: application/json" \
  -d '{
    "requirement": "Add AMB123 condition",
    "selectedApproach": "API Gateway"
  }'
```

#### Test 4: Validate Solution

```bash
curl -X POST http://server-ip:3000/api/validate \
  -H "Content-Type: application/json" \
  -d '{
    "solutionBlocks": [
      {"block": 1, "task": "Setup Gateway", "method": "Install Nginx"},
      {"block": 2, "task": "Add Rules", "method": "Configure"}
    ],
    "requirement": "AMB123"
  }'
```

---

### 🐳 Option 2: Docker (แชร์ image ได้)

**Build Docker image:**

```bash
cd mcp-solution-architect
docker build -t mcp-solution-architect:latest .
```

**Run container (stdio mode):**

```bash
docker run -i mcp-solution-architect:latest
```

**Run container (HTTP mode):**

```bash
docker run -p 3000:3000 mcp-solution-architect:latest node dist/http-server.js
```

**Share image:**

```bash
# Save image
docker save mcp-solution-architect:latest > mcp-solution-architect.tar

# On another machine
docker load < mcp-solution-architect.tar
docker run -i mcp-solution-architect:latest
```

**Or push to Docker Hub:**

```bash
docker tag mcp-solution-architect:latest oatrepos/mcp-solution-architect:latest
docker push oatrepos/mcp-solution-architect:latest

# คนอื่นใช้:
docker pull oatrepos/mcp-solution-architect:latest
docker run -i oatrepos/mcp-solution-architect:latest
```

---

### 📦 Option 3: ngrok (Public URL ทันที)

**Install ngrok:**
```bash
# Windows
choco install ngrok

# Mac
brew install ngrok
```

**Start server:**
```bash
cd mcp-solution-architect
npm run build
PORT=3000 node dist/http-server.js
```

**Expose to internet:**
```bash
ngrok http 3000
```

**จะได้ URL แบบ:**
```
https://abc123.ngrok.io
```

**ทดสอบจากเครื่องอื่น (ทั่วโลก!):**
```bash
curl https://abc123.ngrok.io/health
```

---

### 🌍 Option 4: Deploy to Cloud

#### Vercel

**vercel.json:**
```json
{
  "version": 2,
  "builds": [
    {
      "src": "dist/http-server.js",
      "use": "@vercel/node"
    }
  ],
  "routes": [
    {
      "src": "/(.*)",
      "dest": "dist/http-server.js"
    }
  ]
}
```

```bash
npm install -g vercel
vercel deploy
```

#### Railway

```bash
npm install -g railway
railway login
railway init
railway up
```

#### Heroku

```bash
heroku create mcp-solution-architect
git push heroku main
```

---

### ✅ สรุป: วิธีที่ใช้งานได้เลยตอนนี้

**1. HTTP Server (Local Network)**

```bash
# เครื่อง A (มี source code)
cd mcp-solution-architect
npm run build  
PORT=3000 node dist/http-server.js

# เครื่อง B (test)
curl http://192.168.1.100:3000/health
```

**2. ngrok (Internet)**

```bash
# เครื่อง A
PORT=3000 node dist/http-server.js

# Terminal อื่น
ngrok http 3000

# เครื่อง B (anywhere!)
curl https://abc123.ngrok.io/health
```

**3. Docker**

```bash
# Build once
docker build -t mcp-sa .

# Run anywhere
docker run -p 3000:3000 mcp-sa node dist/http-server.js
```

---

### 🎮 Demo Commands

**Start Server:**
```powershell
cd E:\Work\ESB\AiStandart\Ai-Integrated Process\mcp-solution-architect
npm run build
$env:PORT=3000; node dist/http-server.js
```

**Test from PowerShell (same or different machine):**

```powershell
# Health check
Invoke-RestMethod -Uri "http://localhost:3000/health"

# Analyze
$body = @{
    requirement = "ต้องการแก้เงื่อนไขลูกค้า AMB123"
    constraints = @("ห้ามแก้ code")
    context = "OMNICHANNEL_API"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:3000/api/analyze" -Method Post -Body $body -ContentType "application/json"
```

---

### 📝 VS Code Config (ใช้ HTTP Server)

**ถ้ามี HTTP server รันอยู่ที่ `http://server-ip:3000`:**

ยังไม่มี MCP HTTP client support ใน VS Code โดยตรง แต่สามารถ:

1. ใช้ REST API endpoints
2. สร้าง wrapper script ที่เรียก HTTP API
3. หรือใช้ stdio mode กับ Docker:

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "docker",
      "args": [
        "run",
        "-i",
        "--rm",
        "mcp-solution-architect:latest"
      ]
    }
  }
}
```

---

**แนะนำ:** ใช้ **HTTP Server + ngrok** ถ้าต้องการให้คนอื่นทดสอบได้ทันที!

```bash
# Terminal 1
cd mcp-solution-architect
npm run build
PORT=3000 node dist/http-server.js

# Terminal 2  
ngrok http 3000

# แชร์ URL ที่ได้ เช่น https://abc123.ngrok.io
```
