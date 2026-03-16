# MCP Solution Architect Server

MCP Server implementation สำหรับ AI Solution Architect ตาม Ai-Role/01 Solution Architect methodology

## Features

### Tools
1. **analyze_requirement** - วิเคราะห์ requirement ด้วย TOT (Tree of Thoughts)
2. **generate_solution_table** - สร้าง Block-Split-Chunk solution table (Table 1)
3. **validate_solution** - ตรวจสอบด้วย 2-Table Validation (Table 2)
4. **audit_solution** - Audit state ขั้นสุดท้าย
5. **generate_task_document** - สร้างเอกสารสั่งงานสำหรับ Developer

### Resources
1. **solution-architect://coding-standards** - Coding standards reference
2. **solution-architect://architecture-patterns** - Architecture patterns guide

## Installation

```bash
cd mcp-solution-architect
npm install
npm run build
```

## Configuration

Add to VS Code settings.json:

```json
{
  "mcpServers": {
    "solution-architect": {
      "command": "node",
      "args": [
        "E:\\Work\\ESB\\AiStandart\\Ai-Integrated Process\\mcp-solution-architect\\dist\\index.js"
      ]
    }
  }
}
```

## Usage Examples

### 1. Analyze Requirement

```typescript
// Input
{
  "requirement": "ต้องการแก้เงื่อนไขลูกค้าที่ต้องการนัดยื่นเอกสารให้มี AMB123 ด้วย โดยถ้าเป็น AMB123 จะต้อง return 203",
  "constraints": ["ห้ามแก้ code"],
  "context": "OMNICHANNEL_API project"
}

// Output: TOT analysis with multiple options scored and ranked
```

### 2. Generate Solution Table

```typescript
// Input
{
  "requirement": "Add AMB123 condition",
  "selectedApproach": "API Gateway / Reverse Proxy"
}

// Output: Block-Split-Chunk table with 8 blocks and dependencies
```

### 3. Validate Solution

```typescript
// Input
{
  "solutionBlocks": [...],
  "requirement": "Original requirement"
}

// Output: 2-Table validation with 5 criteria checklist
```

### 4. Audit Solution

```typescript
// Input
{
  "solutionDocument": "Full solution markdown"
}

// Output: Comprehensive audit with 5 verification checks
```

### 5. Generate Task Document

```typescript
// Input
{
  "requirement": "Add AMB123 condition",
  "solutionBlocks": [...],
  "outputPath": "./task-amb123.md"
}

// Output: Complete task document for developers
```

## Methodology

This MCP server implements the Solution Architect workflow:

```
Start Loop
  ├─ TOT: แยกทางเลือกทั้งหมด
  ├─ Block-Split-Chunk: แบ่งเป็น blocks
  ├─ COT: วิเคราะห์แนวทาง
  ├─ ReAct Loop: Vote solution (10/10)
  └─ Iterate until optimal

Step 1: Solution Table (Block-Split-Chunk)
Step 2: Verification Table (2-Table Validation)
  └─ If any ❌ → back to Step 1
  └─ If all ✅ → Step 3
Step 3: Audit State (Final verification)
Step 4: Generate implementation tasks
```

## Testing

Test the server:

```bash
npm run test
```

Or test tools manually with MCP Inspector:

```bash
npx @modelcontextprotocol/inspector node dist/index.js
```

## File Structure

```
mcp-solution-architect/
├── src/
│   └── index.ts          # Main server implementation
├── dist/                 # Compiled JavaScript
├── package.json
├── tsconfig.json
└── README.md
```

## Integration with VS Code

Once configured, you can use this MCP server in GitHub Copilot Chat:

```
@workspace Using solution-architect MCP, analyze this requirement:
"ต้องการแก้เงื่อนไขลูกค้าที่ต้องการนัดยื่นเอกสารให้มี AMB123 ด้วย โดยถ้าเป็น AMB123 จะต้อง return 203"
with constraint: "ห้ามแก้ code"
```

## Output Examples

See [Ai-Role/01 Solution Architect/Solution_Analysis_AMB123.md](../Ai-Role/01%20Solution%20Architect/Solution_Analysis_AMB123.md) for expected output format.

## License

MIT
