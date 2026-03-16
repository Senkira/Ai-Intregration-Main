#!/usr/bin/env node

/**
 * Test script for Solution Architect MCP Server
 * This simulates how Copilot would interact with the MCP server
 */

import { Client } from "@modelcontextprotocol/sdk/client/index.js";
import { StdioClientTransport } from "@modelcontextprotocol/sdk/client/stdio.js";

async function testMCPServer() {
  console.log("🧪 Testing Solution Architect MCP Server\n");

  // Create client
  const transport = new StdioClientTransport({
    command: "node",
    args: ["dist/index.js"],
  });

  const client = new Client(
    {
      name: "test-client",
      version: "1.0.0",
    },
    {
      capabilities: {},
    }
  );

  try {
    await client.connect(transport);
    console.log("✅ Connected to MCP server\n");

    // Test 1: List tools
    console.log("📋 Test 1: List available tools");
    const toolsResponse = await client.request({
      method: "tools/list",
    });
    console.log(`Found ${toolsResponse.tools?.length || 0} tools:`);
    toolsResponse.tools?.forEach((tool) => {
      console.log(`  - ${tool.name}: ${tool.description}`);
    });
    console.log();

    // Test 2: Analyze requirement
    console.log("🎯 Test 2: Analyze requirement (TOT methodology)");
    const analyzeResponse = await client.request({
      method: "tools/call",
      params: {
        name: "analyze_requirement",
        arguments: {
          requirement: "ต้องการแก้เงื่อนไขลูกค้าที่ต้องการนัดยื่นเอกสารให้มี AMB123 ด้วย โดยถ้าเป็น AMB123 จะต้อง return 203",
          constraints: ["ห้ามแก้ code"],
          context: "OMNICHANNEL_API project - existing ASP.NET Core API",
        },
      },
    });
    console.log("Analysis Result:");
    console.log(analyzeResponse.content?.[0]?.text || "No result");
    console.log("\n" + "=".repeat(80) + "\n");

    // Test 3: Generate solution table
    console.log("📊 Test 3: Generate solution table (Block-Split-Chunk)");
    const solutionTableResponse = await client.request({
      method: "tools/call",
      params: {
        name: "generate_solution_table",
        arguments: {
          requirement: "Add AMB123 condition to return HTTP 203",
          selectedApproach: "API Gateway / Reverse Proxy",
        },
      },
    });
    console.log("Solution Table:");
    console.log(solutionTableResponse.content?.[0]?.text || "No result");
    console.log("\n" + "=".repeat(80) + "\n");

    // Test 4: Validate solution
    console.log("✅ Test 4: Validate solution (2-Table Validation)");
    const validateResponse = await client.request({
      method: "tools/call",
      params: {
        name: "validate_solution",
        arguments: {
          solutionBlocks: [
            { block: 1, task: "Setup API Gateway", method: "Install Nginx", dependencies: "" },
            { block: 2, task: "Define Routing Rules", method: "Configure routes", dependencies: "Block 1" },
            { block: 3, task: "Implement Condition", method: "Add AMB123 check", dependencies: "Block 2" },
          ],
          requirement: "Add AMB123 condition",
        },
      },
    });
    console.log("Validation Result:");
    console.log(validateResponse.content?.[0]?.text || "No result");
    console.log("\n" + "=".repeat(80) + "\n");

    // Test 5: Audit solution
    console.log("🔍 Test 5: Audit solution (Final verification)");
    const auditResponse = await client.request({
      method: "tools/call",
      params: {
        name: "audit_solution",
        arguments: {
          solutionDocument: "Solution using API Gateway for AMB123 customer type handling",
        },
      },
    });
    console.log("Audit Result:");
    console.log(auditResponse.content?.[0]?.text || "No result");
    console.log("\n" + "=".repeat(80) + "\n");

    // Test 6: Generate task document
    console.log("📝 Test 6: Generate task document");
    const taskDocResponse = await client.request({
      method: "tools/call",
      params: {
        name: "generate_task_document",
        arguments: {
          requirement: "Add AMB123 condition to return HTTP 203",
          solutionBlocks: [
            { block: 1, task: "Setup API Gateway", method: "Install and configure gateway" },
            { block: 2, task: "Add routing rules", method: "Configure route matching" },
            { block: 3, task: "Implement condition logic", method: "Add AMB123 check" },
          ],
          outputPath: "./test-task-output.md",
        },
      },
    });
    console.log("Task Document:");
    console.log(taskDocResponse.content?.[0]?.text || "No result");
    console.log("\n" + "=".repeat(80) + "\n");

    // Test 7: List resources
    console.log("📚 Test 7: List available resources");
    const resourcesResponse = await client.request({
      method: "resources/list",
    });
    console.log(`Found ${resourcesResponse.resources?.length || 0} resources:`);
    resourcesResponse.resources?.forEach((resource) => {
      console.log(`  - ${resource.name}: ${resource.description}`);
    });
    console.log();

    console.log("✅ All tests completed successfully!\n");

    await client.close();
  } catch (error) {
    console.error("❌ Test failed:", error.message);
    process.exit(1);
  }
}

// Run tests
testMCPServer().catch((error) => {
  console.error("Fatal error:", error);
  process.exit(1);
});
