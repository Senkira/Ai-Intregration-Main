# Ai-Integration-Main

AI-Integrated Development Framework — รวมเครื่องมือ, แนวทาง, และ Template สำหรับการพัฒนาซอฟต์แวร์ด้วย AI

## โครงสร้างโปรเจกต์ (Project Structure)

```
├── Ai-Role/                    # AI Role Prompts & Workflows
│   ├── 01 Solution Architect/  #   ├─ Pipeline: วิเคราะห์และออกแบบ Solution
│   ├── 02 Task QA/             #   ├─ Pipeline: ตรวจสอบ Task ก่อนพัฒนา
│   ├── 03 Developer/           #   ├─ Pipeline: พัฒนาตาม Task
│   ├── 04 Developer QC/        #   ├─ Pipeline: ตรวจสอบคุณภาพโค้ด
│   ├── 05 Repos Finder Assistant/  # Utility: ค้นหา Repository
│   ├── 06 Clone Assistant/     #   Utility: Clone Repository
│   └── 07 Code Reader Assistant/   # Utility: อ่านและวิเคราะห์โค้ด
│
├── CodingStandard/             # มาตรฐานการเขียนโค้ด (.NET)
│   ├── Coding_Standard_Criteria.md
│   ├── Research/
│   ├── Template/               #   └─ SampleAPI project template
│   └── NotebookLM_Source/
│
├── ProjectTemplate/            # Template สำหรับสร้างโปรเจกต์ใหม่
│   ├── New-Project.ps1         #   └─ PowerShell script สร้างโปรเจกต์
│   └── template/
│
├── Examples/                   # ตัวอย่างโปรเจกต์
│   ├── SimpleBlog/             #   ├─ ตัวอย่างเบื้องต้น
│   └── TestProject/            #   └─ ตัวอย่างเต็มรูปแบบ (SmartBankStatement)
│
├── Research/                   # เอกสารวิจัยและศึกษา
│   ├── Ai-Integrated/          #   ├─ วิจัย AI Integration & MCP
│   └── *.html / *.md           #   └─ Gemini, Internalnelra, etc.
│
└── mcp-solution-architect/     # MCP Server สำหรับ Solution Architect
    ├── src/                    #   ├─ TypeScript source
    └── Dockerfile              #   └─ Container deployment
```

## Development Pipeline

```
Solution Architect → Task QA → Developer → Developer QC → ✅ APPROVED
```

แต่ละขั้นตอนมี AI Role Prompt, ตัวอย่าง Chat, และ Report Template อยู่ใน `Ai-Role/`

## Tech Stack

- **.NET 10** + **Dapper** — Backend API
- **xUnit** + **NSubstitute** — Testing
- **TypeScript** + **Node.js** — MCP Server
- **Docker** — Deployment
