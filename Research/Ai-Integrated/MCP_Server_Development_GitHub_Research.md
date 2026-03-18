# การสร้าง MCP Server บนบริการของ GitHub - รายงานการวิจัย

**วันที่:** 16 มีนาคม 2026  
**หัวข้อ:** การพัฒนาและ Deploy MCP (Model Context Protocol) Server โดยใช้บริการของ GitHub

---

## 🎯 Executive Summary

**คำตอบสั้น: ✅ สามารถสร้าง MCP Server เองได้ และใช้บริการของ GitHub ในการ Deploy**

มีหลายวิธีในการสร้างและ deploy MCP server โดยใช้บริการของ GitHub:
1. ✅ **GitHub Actions** - สำหรับ build, test, และ deploy MCP server
2. ✅ **GitHub Container Registry (GHCR)** - เก็บ Docker images
3. ✅ **GitHub Apps** - สร้าง GitHub-specific MCP server
4. ✅ **GitHub-hosted MCP Endpoints** - ใช้บริการ MCP ที่ GitHub จัดหาให้
5. ⚠️ **GitHub Pages** - ไม่เหมาะสำหรับ MCP server (static hosting only)

---

## 📚 ส่วนที่ 1: การสร้าง MCP Server

### 1.1 MCP Server คืออะไร?

**Model Context Protocol (MCP)** เป็น protocol มาตรฐานที่ให้ AI agents สามารถเชื่อมต่อกับ:
- 🛠️ Tools และ functions
- 📊 Databases และ data sources
- 🌐 APIs และ external services
- 📁 File systems และ resources

### 1.2 สถาปัตยกรรม (Architecture)

```
┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│   AI Agent  │  ←───→  │  MCP Client  │  ←───→  │  MCP Server │
│  (Copilot)  │         │  (VS Code)   │         │  (Custom)   │
└─────────────┘         └──────────────┘         └─────────────┘
                                                         │
                                                         ↓
                                                  ┌─────────────┐
                                                  │   Tools &   │
                                                  │  Resources  │
                                                  └─────────────┘
```

**การทำงาน 7 ขั้นตอน:**
1. **Initialization** - Client เชื่อมต่อและตกลง protocol version
2. **Discovery** - Client ค้นหา tools และ resources ที่มี
3. **User Query** - AI model รับ input จากผู้ใช้
4. **Tool Selection** - Model เลือก tool ที่จะใช้
5. **Execution** - Client ส่ง request ไป MCP server
6. **Results** - Server ส่งผลลัพธ์กลับ
7. **Response** - Model สร้างคำตอบให้ผู้ใช้

---

## 💻 ส่วนที่ 2: วิธีการสร้าง MCP Server

### 2.1 TypeScript/Node.js Implementation

**ข้อดี:**
- ✅ SDK สมบูรณ์จาก `@modelcontextprotocol/sdk`
- ✅ ใช้โค้ดน้อย (< 100 บรรทัด)
- ✅ Type safety ด้วย Zod validation
- ✅ Support stdio transport

**ตัวอย่าง Weather MCP Server:**

```typescript
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import {
  CallToolRequestSchema,
  ListToolsRequestSchema,
} from "@modelcontextprotocol/sdk/types.js";
import { z } from "zod";

const GetWeatherSchema = z.object({
  city: z.string().describe("The city to get weather for"),
});

const server = new Server(
  {
    name: "weather-server",
    version: "1.0.0",
  },
  {
    capabilities: {
      tools: {},
    },
  }
);

// ลงทะเบียน tools
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: "get_weather",
        description: "Get current weather for a city",
        inputSchema: {
          type: "object",
          properties: {
            city: { type: "string", description: "City name" },
          },
          required: ["city"],
        },
      },
    ],
  };
});

// จัดการ tool calls
server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "get_weather") {
    const { city } = GetWeatherSchema.parse(request.params.arguments);
    
    // Business logic ของ tool
    const weather = {
      city,
      temperature: "22°C",
      condition: "Partly cloudy",
      humidity: "65%",
    };
    
    return {
      content: [
        {
          type: "text",
          text: JSON.stringify(weather, null, 2),
        },
      ],
    };
  }
  throw new Error(`Unknown tool: ${request.params.name}`);
});

// Start server
async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
  console.error("Weather MCP server running on stdio");
}

main().catch(console.error);
```

### 2.2 Python Implementation

**ข้อดี:**
- ✅ ใช้ FastMCP framework
- ✅ Integration กับ Starlette (async web framework)
- ✅ รองรับ HTTP transport
- ✅ เหมาะสำหรับ data science และ ML workloads

**ตัวอย่าง Math Operations Server:**

```python
import json
import logging
from starlette.applications import Starlette
from starlette.responses import JSONResponse, PlainTextResponse
from starlette.routing import Route
from starlette.requests import Request
from starlette.middleware import Middleware
from starlette.middleware.cors import CORSMiddleware

from mcp.server.fastmcp import FastMCP

mcp = FastMCP("Math Operations MCP")

@mcp.tool()
def add(a: int, b: int) -> int:
    """Add two numbers."""
    return a + b

@mcp.tool()
def multiply(a: int, b: int) -> int:
    """Multiply two numbers."""
    return a * b

@mcp.tool()
def factorial(n: int) -> int:
    """Calculate factorial of a number."""
    if n < 0:
        raise ValueError("Factorial not defined for negative numbers")
    if n == 0 or n == 1:
        return 1
    result = 1
    for i in range(2, n + 1):
        result *= i
    return result

async def mcp_handler(request: Request):
    payload = await request.json()
    result = await mcp.handle_http(payload)
    return JSONResponse(result)

async def health(_request: Request):
    return PlainTextResponse("ok", status_code=200)

routes = [
    Route("/mcp", endpoint=mcp_handler, methods=["POST"]),
    Route("/health", endpoint=health, methods=["GET"]),
]

middleware = [
    Middleware(
        CORSMiddleware,
        allow_origins=["*"],
        allow_methods=["*"],
        allow_headers=["*"]
    ),
]

app = Starlette(routes=routes, middleware=middleware)
```

### 2.3 Production-Ready Node.js Server

**Features:**
- ✅ รองรับทั้ง Tools และ Resources
- ✅ Error handling ที่สมบูรณ์
- ✅ Dynamic resource loading
- ✅ Structured logging

```javascript
#!/usr/bin/env node
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import {
  CallToolRequestSchema,
  ListToolsRequestSchema,
  ListResourcesRequestSchema,
  ReadResourceRequestSchema,
  ErrorCode,
  McpError,
} from "@modelcontextprotocol/sdk/types.js";

const server = new Server(
  {
    name: "production-mcp-server",
    version: "1.0.0",
  },
  {
    capabilities: {
      tools: {},
      resources: {},
    },
  }
);

// Tool definitions
const toolDefinitions = [
  {
    name: "database_query",
    description: "Execute SQL query on database",
    inputSchema: {
      type: "object",
      properties: {
        query: { type: "string", description: "SQL query" },
        database: { type: "string", description: "Database name" },
      },
      required: ["query", "database"],
    },
  },
];

// Resource definitions
const resourceDefinitions = [
  {
    uri: "config://app-settings",
    name: "Application Settings",
    description: "Current application configuration",
    mimeType: "application/json",
  },
];

server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: toolDefinitions,
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;
  
  try {
    let result;
    
    switch (name) {
      case "database_query":
        // Implement actual database query logic
        result = await executeDatabaseQuery(args.query, args.database);
        break;
      
      default:
        throw new Error(`Unknown tool: ${name}`);
    }
    
    return {
      content: [{ type: "text", text: JSON.stringify(result, null, 2) }],
    };
  } catch (error) {
    return {
      content: [
        { type: "text", text: `Error: ${error.message}` },
      ],
      isError: true,
    };
  }
});

server.setRequestHandler(ListResourcesRequestSchema, async () => {
  return {
    resources: resourceDefinitions,
  };
});

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const { uri } = request.params;
  
  try {
    const content = await getResourceContent(uri);
    const resource = resourceDefinitions.find((r) => r.uri === uri);
    
    return {
      contents: [
        {
          uri,
          mimeType: resource?.mimeType || "text/plain",
          text: content,
        },
      ],
    };
  } catch (error) {
    throw new McpError(
      ErrorCode.InvalidRequest,
      `Failed to read resource: ${error.message}`
    );
  }
});

async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
  console.error("Production MCP Server running on stdio");
}

main().catch((error) => {
  console.error("Fatal error:", error);
  process.exit(1);
});
```

---

## 🚀 ส่วนที่ 3: Deployment บน GitHub Services

### 3.1 GitHub Actions Workflow

**ใช้สำหรับ:**
- ✅ Build และ test MCP server
- ✅ Create Docker images
- ✅ Deploy ไปยัง cloud providers
- ✅ Validate MCP server ใน CI/CD

**ตัวอย่าง `.github/workflows/mcp-build-deploy.yml`:**

```yaml
name: Build and Deploy MCP Server

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

env:
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}/mcp-server

jobs:
  build:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'

      - name: Install dependencies
        run: npm ci

      - name: Run tests
        run: npm test

      - name: Build TypeScript
        run: npm run build

      - name: Log in to GitHub Container Registry
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Extract metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
          tags: |
            type=ref,event=branch
            type=ref,event=pr
            type=semver,pattern={{version}}
            type=sha

      - name: Build and push Docker image
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          labels: ${{ steps.meta.outputs.labels }}

  test-mcp:
    runs-on: ubuntu-latest
    needs: build
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Pull MCP Server image
        run: |
          docker pull ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}

      - name: Test MCP JSON-RPC stdio mode
        run: |
          echo '{"jsonrpc":"2.0","id":1,"method":"initialize","params":{}}' | \
          docker run -i ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}

      - name: Install MCP Client
        run: pip install mcp-client

      - name: Run integration tests
        run: python tests/test_mcp_integration.py

  deploy:
    runs-on: ubuntu-latest
    needs: [build, test-mcp]
    if: github.ref == 'refs/heads/main'
    
    steps:
      - name: Deploy to Cloud Run
        uses: google-github-actions/deploy-cloudrun@v2
        with:
          service: mcp-server
          image: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}
          region: asia-southeast1
```

### 3.2 Dockerfile for MCP Server

```dockerfile
FROM node:20-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY tsconfig.json ./
COPY src ./src
RUN npm run build

FROM node:20-alpine AS runtime

WORKDIR /app

RUN addgroup -g 1001 -S nodejs && \
    adduser -S nodejs -u 1001

COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json

USER nodejs

EXPOSE 3000

# Support both stdio and HTTP modes
CMD ["node", "dist/index.js"]
```

### 3.3 GitHub Container Registry (GHCR)

**ข้อดี:**
- ✅ ฟรีสำหรับ public repositories
- ✅ Integrated กับ GitHub Actions
- ✅ รองรับ OCI-compliant images
- ✅ Fine-grained access control

**การใช้งาน:**

```bash
# Login to GHCR
echo ${{ secrets.GITHUB_TOKEN }} | docker login ghcr.io -u ${{ github.actor }} --password-stdin

# Build image
docker build -t ghcr.io/oatrepos/mcp-server:latest .

# Push to GHCR
docker push ghcr.io/oatrepos/mcp-server:latest

# Pull from GHCR
docker pull ghcr.io/oatrepos/mcp-server:latest
```

### 3.4 GitHub-hosted MCP Server

**GitHub มี official MCP server พร้อมใช้งาน:**

- ✅ **Repository:** `github/github-mcp-server`
- ✅ **Features:**
  - OAuth authentication
  - Auto-update
  - No Docker management required
  - Managed infrastructure

**การติดตั้ง:**

```json
// VS Code settings.json
{
  "mcpServers": {
    "github": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-github"],
      "env": {
        "GITHUB_TOKEN": "${GITHUB_TOKEN}"
      }
    }
  }
}
```

หรือใช้ VS Code Command:
```
> GitHub MCP: Install Remote Server
```

### 3.5 Hosting Options Comparison

| Option | ข้อดี | ข้อเสีย | เหมาะสำหรับ |
|--------|-------|---------|-------------|
| **GitHub Actions** | CI/CD integration, ฟรี | Run time limits (6 ชม.) | Build & test |
| **GitHub Container Registry** | ฟรี (public), integrated | ต้อง manage deployment เอง | Store images |
| **GitHub-hosted MCP** | No infra, OAuth, auto-update | จำกัด features | General use |
| **Self-hosted Runners** | Full control, private network | ต้อง maintain infra | Enterprise |
| **Cloud Run (via GH Actions)** | Scalable, pay-per-use | Cost, complexity | Production |
| **GitHub Pages** | ❌ ไม่เหมาะ (static only) | ไม่รัน server apps | Documentation |

---

## 🎨 ส่วนที่ 4: Use Cases และตัวอย่างจริง

### 4.1 Custom Coding Standard Enforcement MCP

**Use Case:** สร้าง MCP server ที่ enforce coding standards ของเรา

```typescript
import { Server } from "@modelcontextprotocol/sdk/server/index.js";

const server = new Server({
  name: "coding-standard-enforcer",
  version: "1.0.0",
});

server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: [
    {
      name: "validate_csharp_code",
      description: "Validate C# code against our coding standards",
      inputSchema: {
        type: "object",
        properties: {
          code: { type: "string" },
          rules: { type: "array", items: { type: "string" } },
        },
        required: ["code"],
      },
    },
    {
      name: "suggest_fixes",
      description: "Suggest fixes for coding standard violations",
      inputSchema: {
        type: "object",
        properties: {
          violations: { type: "array" },
        },
        required: ["violations"],
      },
    },
  ],
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "validate_csharp_code") {
    const { code, rules = [] } = request.params.arguments;
    
    // Load our coding standards
    const standards = await loadCodingStandards();
    const violations = [];
    
    // Check naming conventions (§2)
    if (!code.match(/class [A-Z][a-zA-Z0-9]+/)) {
      violations.push({
        rule: "2.1",
        severity: "error",
        message: "Class name must be PascalCase",
        line: findViolationLine(code),
      });
    }
    
    // Check service layer patterns (§4)
    if (code.includes("class") && code.includes("Service")) {
      if (!code.includes("interface I")) {
        violations.push({
          rule: "4.1",
          severity: "error",
          message: "Every Service must have an Interface",
        });
      }
      
      if (!code.includes("async Task")) {
        violations.push({
          rule: "4.4",
          severity: "error",
          message: "Service methods must be async",
        });
      }
    }
    
    return {
      content: [{
        type: "text",
        text: JSON.stringify({
          isValid: violations.length === 0,
          violations,
          summary: `Found ${violations.length} violations`,
        }, null, 2),
      }],
    };
  }
});
```

### 4.2 Database Analysis MCP

**Use Case:** ให้ Copilot วิเคราะห์ database schema และ suggest optimizations

```typescript
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { Pool } from "pg";

const dbPool = new Pool({
  connectionString: process.env.DATABASE_URL,
});

const server = new Server({
  name: "database-analyzer",
  version: "1.0.0",
});

server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: [
    {
      name: "analyze_table",
      description: "Analyze table structure and suggest indexes",
      inputSchema: {
        type: "object",
        properties: {
          tableName: { type: "string" },
        },
        required: ["tableName"],
      },
    },
    {
      name: "check_n_plus_one",
      description: "Detect N+1 query patterns in logs",
      inputSchema: {
        type: "object",
        properties: {
          logFile: { type: "string" },
        },
        required: ["logFile"],
      },
    },
  ],
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "analyze_table") {
    const { tableName } = request.params.arguments;
    
    // Get table structure
    const columns = await dbPool.query(`
      SELECT column_name, data_type, is_nullable
      FROM information_schema.columns
      WHERE table_name = $1
    `, [tableName]);
    
    // Get existing indexes
    const indexes = await dbPool.query(`
      SELECT indexname, indexdef
      FROM pg_indexes
      WHERE tablename = $1
    `, [tableName]);
    
    // Analyze query patterns
    const queries = await analyzeQueryLogs(tableName);
    
    // Suggest optimizations
    const suggestions = [];
    
    // Check for missing indexes
    const frequentWhereColumns = queries
      .filter(q => q.type === "SELECT")
      .flatMap(q => q.whereColumns)
      .reduce((acc, col) => {
        acc[col] = (acc[col] || 0) + 1;
        return acc;
      }, {});
    
    for (const [column, count] of Object.entries(frequentWhereColumns)) {
      if (count > 10 && !indexes.some(idx => idx.indexdef.includes(column))) {
        suggestions.push({
          type: "index",
          priority: "high",
          suggestion: `CREATE INDEX idx_${tableName}_${column} ON ${tableName}(${column})`,
          reason: `Column '${column}' used in WHERE clause ${count} times without index`,
        });
      }
    }
    
    return {
      content: [{
        type: "text",
        text: JSON.stringify({
          table: tableName,
          columns: columns.rows,
          indexes: indexes.rows,
          suggestions,
        }, null, 2),
      }],
    };
  }
});
```

### 4.3 GitHub Integration MCP

**Use Case:** ให้ Copilot สามารถดึงข้อมูลจาก GitHub repos

```typescript
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { Octokit } from "@octokit/rest";

const octokit = new Octokit({
  auth: process.env.GITHUB_TOKEN,
});

const server = new Server({
  name: "github-repo-analyzer",
  version: "1.0.0",
});

server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: [
    {
      name: "analyze_repo_structure",
      description: "Analyze repository structure and patterns",
      inputSchema: {
        type: "object",
        properties: {
          owner: { type: "string" },
          repo: { type: "string" },
        },
        required: ["owner", "repo"],
      },
    },
    {
      name: "find_coding_patterns",
      description: "Search for specific coding patterns across repo",
      inputSchema: {
        type: "object",
        properties: {
          owner: { type: "string" },
          repo: { type: "string" },
          pattern: { type: "string" },
        },
        required: ["owner", "repo", "pattern"],
      },
    },
  ],
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "analyze_repo_structure") {
    const { owner, repo } = request.params.arguments;
    
    // Get repository tree
    const { data: repoData } = await octokit.repos.get({ owner, repo });
    const { data: tree } = await octokit.git.getTree({
      owner,
      repo,
      tree_sha: repoData.default_branch,
      recursive: "1",
    });
    
    // Analyze structure
    const structure = {
      totalFiles: tree.tree.length,
      fileTypes: {},
      directories: new Set(),
      codeQuality: {},
    };
    
    for (const item of tree.tree) {
      if (item.type === "blob") {
        const ext = item.path.split(".").pop();
        structure.fileTypes[ext] = (structure.fileTypes[ext] || 0) + 1;
      } else if (item.type === "tree") {
        structure.directories.add(item.path);
      }
    }
    
    // Check for common patterns
    structure.hasDockerfile = tree.tree.some(item => item.path === "Dockerfile");
    structure.hasJenkinsfile = tree.tree.some(item => item.path.startsWith("Jenkinsfile"));
    structure.hasTests = tree.tree.some(item => item.path.includes("test"));
    
    return {
      content: [{
        type: "text",
        text: JSON.stringify(structure, null, 2),
      }],
    };
  }
});
```

---

## 📋 ส่วนที่ 5: Best Practices

### 5.1 Security

```typescript
// ❌ อย่าทำ
const server = new Server({
  name: "unsafe-server",
});

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  // Dangerous: Direct SQL injection
  const result = await db.query(request.params.arguments.query);
  return { content: [{ type: "text", text: JSON.stringify(result) }] };
});

// ✅ ทำแบบนี้
server.setRequestHandler(CallToolRequestSchema, async (request) => {
  // Validate input
  const schema = z.object({
    query: z.string().max(1000),
    params: z.array(z.any()).max(10),
  });
  
  const validated = schema.parse(request.params.arguments);
  
  // Use parameterized queries
  const result = await db.query(validated.query, validated.params);
  
  // Sanitize output
  return {
    content: [{
      type: "text",
      text: JSON.stringify(sanitizeOutput(result)),
    }],
  };
});
```

### 5.2 Error Handling

```typescript
import { McpError, ErrorCode } from "@modelcontextprotocol/sdk/types.js";

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  try {
    // Tool logic
    const result = await performOperation(request.params.arguments);
    
    return {
      content: [{ type: "text", text: JSON.stringify(result) }],
    };
  } catch (error) {
    // Log error with context
    console.error("Tool execution failed:", {
      tool: request.params.name,
      arguments: request.params.arguments,
      error: error.message,
      stack: error.stack,
    });
    
    // Return user-friendly error
    if (error instanceof ValidationError) {
      throw new McpError(
        ErrorCode.InvalidParams,
        `Invalid input: ${error.message}`
      );
    }
    
    if (error instanceof NotFoundError) {
      throw new McpError(
        ErrorCode.InvalidRequest,
        `Resource not found: ${error.message}`
      );
    }
    
    // Generic error
    throw new McpError(
      ErrorCode.InternalError,
      "An unexpected error occurred"
    );
  }
});
```

### 5.3 Testing

```typescript
// tests/mcp-server.test.ts
import { describe, it, expect } from "vitest";
import { Client } from "@modelcontextprotocol/sdk/client/index.js";
import { StdioClientTransport } from "@modelcontextprotocol/sdk/client/stdio.js";

describe("MCP Server", () => {
  let client: Client;
  
  beforeEach(async () => {
    const transport = new StdioClientTransport({
      command: "node",
      args: ["dist/index.js"],
    });
    
    client = new Client({
      name: "test-client",
      version: "1.0.0",
    });
    
    await client.connect(transport);
  });
  
  it("should list available tools", async () => {
    const response = await client.request({
      method: "tools/list",
    });
    
    expect(response.tools).toHaveLength(3);
    expect(response.tools[0].name).toBe("get_weather");
  });
  
  it("should execute tool successfully", async () => {
    const response = await client.request({
      method: "tools/call",
      params: {
        name: "get_weather",
        arguments: { city: "Bangkok" },
      },
    });
    
    expect(response.content[0].text).toContain("Bangkok");
  });
  
  it("should handle errors gracefully", async () => {
    await expect(
      client.request({
        method: "tools/call",
        params: {
          name: "unknown_tool",
          arguments: {},
        },
      })
    ).rejects.toThrow("Unknown tool");
  });
});
```

### 5.4 Performance

```typescript
// Use connection pooling
const dbPool = new Pool({
  max: 20,
  idleTimeoutMillis: 30000,
  connectionTimeoutMillis: 2000,
});

// Cache frequently accessed data
const cache = new Map();

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const cacheKey = `${request.params.name}:${JSON.stringify(request.params.arguments)}`;
  
  // Check cache
  if (cache.has(cacheKey)) {
    const cached = cache.get(cacheKey);
    if (Date.now() - cached.timestamp < 60000) { // 1 minute TTL
      return cached.data;
    }
  }
  
  // Execute tool
  const result = await executeToolLogic(request);
  
  // Cache result
  cache.set(cacheKey, {
    data: result,
    timestamp: Date.now(),
  });
  
  return result;
});
```

---

## 🎓 ส่วนที่ 6: ตัวอย่างโครงสร้างโปรเจค

```
my-mcp-server/
├── .github/
│   └── workflows/
│       ├── build-deploy.yml
│       └── test.yml
├── src/
│   ├── index.ts                 # Main server entry point
│   ├── server.ts                # Server setup
│   ├── tools/
│   │   ├── index.ts
│   │   ├── database.ts          # Database tools
│   │   ├── github.ts            # GitHub tools
│   │   └── validator.ts         # Code validation tools
│   ├── resources/
│   │   ├── index.ts
│   │   └── config.ts            # Config resources
│   └── utils/
│       ├── db.ts                # Database utilities
│       ├── validation.ts        # Input validation
│       └── error.ts             # Error handling
├── tests/
│   ├── integration/
│   │   └── mcp-server.test.ts
│   └── unit/
│       ├── tools.test.ts
│       └── resources.test.ts
├── .dockerignore
├── .gitignore
├── Dockerfile
├── docker-compose.yml           # For local development
├── package.json
├── tsconfig.json
└── README.md
```

---

## 📊 ส่วนที่ 7: ตารางเปรียบเทียบ Deployment Options

| Deployment Method | Setup Complexity | Cost | Scalability | Maintenance | Best For |
|-------------------|------------------|------|-------------|-------------|----------|
| **GitHub Actions only** | ⭐⭐ Easy | ✅ Free | ⭐ Limited | ⭐⭐ Low | CI/CD testing |
| **GHCR + Self-hosted** | ⭐⭐⭐ Medium | 💰 Infrastructure cost | ⭐⭐⭐ Good | ⭐⭐⭐ Medium | Enterprise |
| **GitHub-hosted MCP** | ⭐ Very Easy | ✅ Free | ⭐⭐⭐⭐ Excellent | ✅ Zero | Quick start |
| **GH Actions → Cloud Run** | ⭐⭐⭐⭐ Complex | 💰💰 Pay-per-use | ⭐⭐⭐⭐⭐ Excellent | ⭐⭐ Low | Production |
| **GH Actions → AWS Lambda** | ⭐⭐⭐⭐ Complex | 💰💰 Pay-per-use | ⭐⭐⭐⭐⭐ Excellent | ⭐⭐ Low | Production |

---

## 🎯 ส่วนที่ 8: Recommendations สำหรับทีมเรา

### 8.1 แนวทางที่แนะนำ

**สำหรับ Development:**
1. ✅ ใช้ **TypeScript** เพื่อ type safety
2. ✅ Deploy ผ่าน **GitHub Actions**
3. ✅ เก็บ images ใน **GitHub Container Registry (GHCR)**
4. ✅ ใช้ **Docker** สำหรับ local development

**สำหรับ Production:**
1. ✅ Deploy ไปยัง **Google Cloud Run** (serverless)
2. ✅ ใช้ **VPC-SC** สำหรับ security
3. ✅ Enable **monitoring** และ **logging**
4. ✅ Implement **rate limiting** และ **authentication**

### 8.2 ตัวอย่าง Use Cases สำหรับทีมเรา

#### Use Case 1: Coding Standard Enforcer
```typescript
// สร้าง MCP server ที่ enforce coding standards
// Tools:
// - validate_code(code, language)
// - suggest_fixes(violations)
// - get_standard_rules(section)
```

#### Use Case 2: Database Schema Analyzer
```typescript
// สร้าง MCP server ที่วิเคราะห์ database
// Tools:
// - analyze_table(tableName)
// - check_indexes(tableName)
// - suggest_optimizations()
```

#### Use Case 3: Repository Pattern Finder
```typescript
// สร้าง MCP server ที่ค้นหา code patterns
// Tools:
// - find_pattern(pattern, repo)
// - analyze_dependencies()
// - check_vulnerabilities()
```

---

## 📚 แหล่งข้อมูลอ้างอิง

### Official Documentation
1. **Model Context Protocol Specification**  
   https://spec.modelcontextprotocol.io/

2. **MCP TypeScript SDK**  
   https://github.com/modelcontextprotocol/typescript-sdk

3. **GitHub MCP Server**  
   https://github.com/github/github-mcp-server

4. **GitHub Actions Documentation**  
   https://docs.github.com/en/actions

5. **GitHub Container Registry**  
   https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry

### Tutorials & Guides
1. **How to Build MCP Server (Northflank)**  
   https://northflank.com/blog/how-to-build-and-deploy-a-model-context-protocol-mcp-server

2. **MCP Complete Guide 2026 (SainAm Tech)**  
   https://sainam.tech/blog/mcp-complete-guide-2026/

3. **MCP Server Development Guide (ToLearn)**  
   https://tolearn.blog/blog/mcp-model-context-protocol-guide

4. **GitHub MCP Server Practical Guide (GitHub Blog)**  
   https://github.blog/ai-and-ml/generative-ai/a-practical-guide-on-how-to-use-the-github-mcp-server/

5. **Running Terraform MCP in GitHub Actions (Thomas Thornton)**  
   https://thomasthornton.cloud/running-hashicorps-terraform-mcp-server-in-github-actions/

---

## ✅ สรุป

### คำตอบสั้น
**✅ ใช่ สามารถสร้าง MCP Server เองได้ และ deploy บนบริการของ GitHub**

### สิ่งที่ทำได้:
1. ✅ สร้าง MCP server ด้วย TypeScript, Python, หรือ Node.js
2. ✅ Build และ test ผ่าน GitHub Actions
3. ✅ เก็บ Docker images ใน GitHub Container Registry (GHCR)
4. ✅ Deploy ไปยัง cloud providers (Cloud Run, AWS Lambda)
5. ✅ ใช้ GitHub-hosted MCP server สำหรับ quick start

### สิ่งที่ไม่เหมาะ:
- ❌ Deploy MCP server บน GitHub Pages (static hosting only)
- ❌ Run MCP server ตลอดเวลาใน GitHub Actions (time limit 6 hours)

### Next Steps สำหรับทีม:
1. 🎯 เลือก use case แรก (แนะนำ: Coding Standard Enforcer)
2. 🛠️ สร้าง prototype ด้วย TypeScript + MCP SDK
3. 🧪 ทดสอบใน local environment ก่อน
4. 🚀 Setup GitHub Actions workflow
5. ☁️ Deploy ไปยัง Cloud Run
6. 📊 Monitor usage และ iterate

---

**หมายเหตุ:** เอกสารนี้จัดทำจากการวิจัยเมื่อวันที่ 16 มีนาคม 2026 โดยใช้ข้อมูลจาก official documentation และ verified sources
