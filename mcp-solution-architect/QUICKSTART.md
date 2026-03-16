# Quick Setup สำหรับเครื่องอื่น

## วิธีที่เร็วที่สุด (แนะนำ)

1. **ติดตั้ง Node.js** (ถ้ายังไม่มี)
   - Download: https://nodejs.org/

2. **เปิด VS Code**

3. **เพิ่ม config นี้ใน settings.json:**

กด `Ctrl+Shift+P` (Windows) หรือ `Cmd+Shift+P` (Mac)
พิมพ์: `Preferences: Open User Settings (JSON)`

วางโค้ดนี้:

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

4. **Reload VS Code**
   - กด `Ctrl+Shift+P` พิมพ์ `Developer: Reload Window`

5. **ทดสอบ**

เปิด Copilot Chat และพิมพ์:

```
@workspace using solution-architect, analyze this:
"ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203"
constraint: "ห้ามแก้ code"
```

---

## เสร็จแล้ว! 🎉

MCP Server จะทำงานอัตโนมัติทุกครั้งที่เปิด VS Code

---

## ปัญหา?

### ถ้า npx ช้า

ใช้ global install แทน:

```bash
npm install -g @oatrepos/mcp-solution-architect
```

แล้วเปลี่ยน settings:

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "mcp-solution-architect"
    }
  }
}
```

### ถ้า Windows ใช้ไม่ได้

ลองเปลี่ยนเป็น:

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "cmd",
      "args": ["/c", "npx", "-y", "@oatrepos/mcp-solution-architect"]
    }
  }
}
```

---

**ดู Full Documentation:** INSTALL.md และ SETTINGS.md
