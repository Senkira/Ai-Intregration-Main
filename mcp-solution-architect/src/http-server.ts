#!/usr/bin/env node

/**
 * HTTP Server version of MCP Solution Architect
 * Allows remote access from other machines
 */

import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { SSEServerTransport } from "@modelcontextprotocol/sdk/server/sse.js";
import express from "express";
import cors from "cors";
import {
  CallToolRequestSchema,
  ListToolsRequestSchema,
  ListResourcesRequestSchema,
  ReadResourceRequestSchema,
  ErrorCode,
  McpError,
} from "@modelcontextprotocol/sdk/types.js";
import { z } from "zod";
import * as fs from "fs/promises";
import * as path from "path";

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// MCP Server setup (reuse the same server logic)
const server = new Server(
  {
    name: "solution-architect-mcp",
    version: "1.0.0",
  },
  {
    capabilities: {
      tools: {},
      resources: {},
    },
  }
);

// Import handlers from main index
import {
  analyzeRequirement,
  generateSolutionTable,
  validateSolution,
  auditSolution,
  generateTaskDocument,
  getCodingStandards,
  getArchitecturePatterns,
  toolDefinitions,
  resourceDefinitions,
} from "./handlers.js";

// Setup handlers
server.setRequestHandler(ListToolsRequestSchema, async () => ({
  tools: toolDefinitions,
}));

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  const { name, arguments: args } = request.params;

  try {
    let result: any;

    switch (name) {
      case "analyze_requirement":
        result = await analyzeRequirement(args);
        break;
      case "generate_solution_table":
        result = await generateSolutionTable(args);
        break;
      case "validate_solution":
        result = await validateSolution(args);
        break;
      case "generate_task_document":
        result = await generateTaskDocument(args);
        break;
      case "audit_solution":
        result = await auditSolution(args);
        break;
      default:
        throw new Error(`Unknown tool: ${name}`);
    }

    return {
      content: [
        {
          type: "text",
          text: typeof result === "string" ? result : JSON.stringify(result, null, 2),
        },
      ],
    };
  } catch (error: any) {
    return {
      content: [{ type: "text", text: `Error: ${error.message}` }],
      isError: true,
    };
  }
});

server.setRequestHandler(ListResourcesRequestSchema, async () => ({
  resources: resourceDefinitions,
}));

server.setRequestHandler(ReadResourceRequestSchema, async (request) => {
  const { uri } = request.params;

  try {
    let content = "";

    switch (uri) {
      case "solution-architect://coding-standards":
        content = await getCodingStandards();
        break;
      case "solution-architect://architecture-patterns":
        content = await getArchitecturePatterns();
        break;
      default:
        throw new McpError(ErrorCode.InvalidRequest, `Unknown resource: ${uri}`);
    }

    return {
      contents: [{ uri, mimeType: "text/markdown", text: content }],
    };
  } catch (error: any) {
    throw new McpError(ErrorCode.InternalError, `Failed to read resource: ${error.message}`);
  }
});

// HTTP Routes
app.get("/health", (req, res) => {
  res.json({ status: "ok", service: "mcp-solution-architect", version: "1.0.0" });
});

app.post("/mcp", async (req, res) => {
  const transport = new SSEServerTransport("/mcp/messages", res);
  await server.connect(transport);
  
  // Handle the MCP request
  // This is simplified - in production you'd handle the full SSE lifecycle
  res.json({ message: "MCP endpoint - use SSE transport" });
});

// Simple JSON API endpoints for easier testing
app.post("/api/analyze", async (req, res) => {
  try {
    const result = await analyzeRequirement(req.body);
    res.json({ success: true, data: result });
  } catch (error: any) {
    res.status(400).json({ success: false, error: error.message });
  }
});

app.post("/api/solution-table", async (req, res) => {
  try {
    const result = await generateSolutionTable(req.body);
    res.json({ success: true, data: result });
  } catch (error: any) {
    res.status(400).json({ success: false, error: error.message });
  }
});

app.post("/api/validate", async (req, res) => {
  try {
    const result = await validateSolution(req.body);
    res.json({ success: true, data: result });
  } catch (error: any) {
    res.status(400).json({ success: false, error: error.message });
  }
});

app.post("/api/audit", async (req, res) => {
  try {
    const result = await auditSolution(req.body);
    res.json({ success: true, data: result });
  } catch (error: any) {
    res.status(400).json({ success: false, error: error.message });
  }
});

app.post("/api/task-document", async (req, res) => {
  try {
    const result = await generateTaskDocument(req.body);
    res.json({ success: true, data: result });
  } catch (error: any) {
    res.status(400).json({ success: false, error: error.message });
  }
});

// Start server
app.listen(PORT, () => {
  console.log(`🚀 MCP Solution Architect HTTP Server running on http://localhost:${PORT}`);
  console.log(`📋 Health check: http://localhost:${PORT}/health`);
  console.log(`🔧 API endpoints available at: http://localhost:${PORT}/api/*`);
});
