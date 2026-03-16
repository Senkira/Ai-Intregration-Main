#!/usr/bin/env node

/**
 * Simple test - just check if server starts and responds to basic JSON-RPC
 */

import { spawn } from "child_process";

console.log("🧪 Testing Solution Architect MCP Server\n");

const serverProcess = spawn("node", ["dist/index.js"], {
  stdio: ["pipe", "pipe", "inherit"],
});

// Test 1: Initialize
const initRequest = {
  jsonrpc: "2.0",
  id: 1,
  method: "initialize",
  params: {
    protocolVersion: "2024-11-05",
    capabilities: {},
    clientInfo: {
      name: "test-client",
      version: "1.0.0",
    },
  },
};

console.log("📤 Sending initialize request...");
serverProcess.stdin.write(JSON.stringify(initRequest) + "\n");

// Test 2: List tools
setTimeout(() => {
  const listToolsRequest = {
    jsonrpc: "2.0",
    id: 2,
    method: "tools/list",
    params: {},
  };

  console.log("📤 Sending tools/list request...");
  serverProcess.stdin.write(JSON.stringify(listToolsRequest) + "\n");
}, 500);

// Test 3: Call analyze_requirement tool
setTimeout(() => {
  const analyzeRequest = {
    jsonrpc: "2.0",
    id: 3,
    method: "tools/call",
    params: {
      name: "analyze_requirement",
      arguments: {
        requirement: "ต้องการแก้เงื่อนไขลูกค้า AMB123 ให้ return 203",
        constraints: ["ห้ามแก้ code"],
        context: "OMNICHANNEL_API",
      },
    },
  };

  console.log("📤 Sending analyze_requirement request...");
  serverProcess.stdin.write(JSON.stringify(analyzeRequest) + "\n");
}, 1000);

let outputBuffer = "";

serverProcess.stdout.on("data", (data) => {
  const text = data.toString();
  outputBuffer += text;

  // Try to parse complete JSON-RPC responses
  const lines = outputBuffer.split("\n");
  outputBuffer = lines.pop() || ""; // Keep incomplete line

  lines.forEach((line) => {
    if (line.trim()) {
      try {
        const response = JSON.parse(line);
        console.log("\n📥 Received response:");
        console.log(JSON.stringify(response, null, 2));
        console.log("\n" + "=".repeat(80) + "\n");

        // If this is the analyze response, we're done
        if (response.id === 3) {
          console.log("✅ All tests completed!");
          setTimeout(() => {
            serverProcess.kill();
            process.exit(0);
          }, 100);
        }
      } catch (e) {
        // Not valid JSON, ignore
      }
    }
  });
});

serverProcess.on("exit", (code) => {
  console.log(`\nServer exited with code ${code}`);
});

// Timeout after 10 seconds
setTimeout(() => {
  console.log("\n⏱️ Test timeout - killing server");
  serverProcess.kill();
  process.exit(1);
}, 10000);
