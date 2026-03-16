# VS Code Settings สำหรับเครื่องอื่น

## 🎯 Quick Setup Guide

### วิธีที่ 1: npx (แนะนำ - ไม่ต้องติดตั้ง)

เพิ่มใน `.vscode/settings.json`:

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

**ข้อดี:**
- ไม่ต้องติดตั้งก่อน
- Auto-update เมื่อมี version ใหม่
- ใช้งานได้ทันทีในทุกเครื่อง

---

### วิธีที่ 2: Global Install

#### Step 1: Install

```bash
npm install -g @oatrepos/mcp-solution-architect
```

#### Step 2: Configure

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

### วิธีที่ 3: Local Project

#### Step 1: Add to package.json

```json
{
  "devDependencies": {
    "@oatrepos/mcp-solution-architect": "^1.0.0"
  },
  "scripts": {
    "mcp": "node node_modules/@oatrepos/mcp-solution-architect/dist/index.js"
  }
}
```

#### Step 2: Configure

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npm",
      "args": ["run", "mcp"],
      "cwd": "${workspaceFolder}"
    }
  }
}
```

---

### วิธีที่ 4: Git Repository (Development)

#### Step 1: Clone

```bash
git clone https://github.com/oatrepos/ai-code.git
cd ai-code/mcp-solution-architect
npm install
npm run build
```

#### Step 2: Configure (เปลี่ยน path ตามเครื่องของคุณ)

**Windows:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": [
        "C:/Users/YourName/Projects/ai-code/mcp-solution-architect/dist/index.js"
      ]
    }
  }
}
```

**Mac/Linux:**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": [
        "/Users/YourName/Projects/ai-code/mcp-solution-architect/dist/index.js"
      ]
    }
  }
}
```

---

## 📁 ตัวอย่าง settings.json แบบเต็ม

### สำหรับ Team (แชร์กันได้)

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
    },
    "coding-standards": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-coding-standards"
      ]
    }
  },
  "github.copilot.enable": {
    "*": true,
    "plaintext": false,
    "markdown": true
  }
}
```

### สำหรับ Windows Users

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "cmd",
      "args": [
        "/c",
        "npx",
        "-y",
        "@oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

### สำหรับ Mac/Linux Users

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "/bin/sh",
      "args": [
        "-c",
        "npx -y @oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

---

## 🔧 Advanced Configuration

### With Environment Variables

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"],
      "env": {
        "NODE_ENV": "production",
        "LOG_LEVEL": "info",
        "CODING_STANDARDS_PATH": "${workspaceFolder}/CodingStandard"
      }
    }
  }
}
```

### With Workspace Variables

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": [
        "${workspaceFolder}/node_modules/@oatrepos/mcp-solution-architect/dist/index.js"
      ],
      "cwd": "${workspaceFolder}"
    }
  }
}
```

### Multiple Servers

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"]
    },
    "solution-architect-dev": {
      "command": "node",
      "args": [
        "/path/to/local/development/mcp-solution-architect/dist/index.js"
      ]
    }
  }
}
```

---

## 📋 Verification Steps

### 1. Reload VS Code

- กด `Ctrl+Shift+P` (Windows) หรือ `Cmd+Shift+P` (Mac)
- พิมพ์: `Developer: Reload Window`

### 2. Check MCP Connection

เปิด Copilot Chat และพิมพ์:

```
@workspace list available MCP servers
```

ควรเห็น `solution-architect` ในรายการ

### 3. Test Tool

```
@workspace using solution-architect MCP, analyze this requirement:
"ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203"
with constraint: "ห้ามแก้ code"
```

---

## 🐛 Troubleshooting

### ปัญหา: MCP Server ไม่ขึ้น

**Check:**
1. ดู Output panel: `View > Output > GitHub Copilot`
2. ตรวจสอบ command path ถูกต้อง
3. ลอง run command ใน terminal ก่อน

### ปัญหา: npx ช้า

**Solution: Use cache**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "--prefer-online",
        "@oatrepos/mcp-solution-architect"
      ]
    }
  }
}
```

### ปัญหา: Windows path issues

**Solution: Use forward slashes**
```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": [
        "C:/Users/YourName/Projects/ai-code/mcp-solution-architect/dist/index.js"
      ]
    }
  }
}
```

---

## 💡 Best Practices

### 1. Version Pinning (สำหรับ production)

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": [
        "-y",
        "@oatrepos/mcp-solution-architect@1.0.0"
      ]
    }
  }
}
```

### 2. Team Settings (แชร์ผ่าน Git)

สร้างไฟล์ `.vscode/settings.json` ใน project:

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

Add to `.gitignore`:
```
# Keep team settings
!.vscode/settings.json
```

### 3. User-specific Override

ใน User Settings (`Ctrl+,` > User):
```json
{
  "mcpServers": {
    "solution-architect-local": {
      "command": "node",
      "args": ["/your/local/path/dist/index.js"]
    }
  }
}
```

---

## 📚 สำหรับแต่ละ OS

### Windows 10/11

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"],
      "env": {
        "USERPROFILE": "${env:USERPROFILE}"
      }
    }
  }
}
```

### macOS

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "/usr/local/bin/npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"],
      "env": {
        "HOME": "${env:HOME}"
      }
    }
  }
}
```

### Linux (Ubuntu/Debian)

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "/usr/bin/npx",
      "args": ["-y", "@oatrepos/mcp-solution-architect"],
      "env": {
        "HOME": "${env:HOME}"
      }
    }
  }
}
```

---

**แนะนำ:** ใช้วิธีที่ 1 (npx) เพราะใช้งานง่ายที่สุดและทำงานได้ทุกเครื่อง!
