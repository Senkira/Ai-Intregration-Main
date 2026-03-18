# Internal-Nelra Service - Copilot Instructions

## ⚠️ CRITICAL: Always Check Current Date First!

Before making any Internal-Nelra queries, **ALWAYS** get the current date to ensure accurate and relevant results.

```powershell
# Get current date first (MANDATORY!)
Get-Date -Format "yyyy-MM-dd"
# Use this date in your query to ensure accuracy
```

---

## 📌 Service Overview

**Internal-Nelra Service** — Thai-optimized multi-provider AI proxy + 100% Perplexity API coverage for real-time web search

**Service URL:** `https://internal-nelra-427169545674.asia-southeast1.run.app`

### Available Endpoints:

| Endpoint                      | Use Case                                               | Body Format              |
| ----------------------------- | ------------------------------------------------------ | ------------------------ |
| `POST /api/search`            | **Simple search** (query string → content + citations) | `{query, model}`         |
| `POST /api/pplx/sonar`        | **Full Perplexity proxy** (messages array, all params) | `{model, messages, ...}` |
| `POST /api/pplx/sonar/stream` | **Streaming** (SSE)                                    | `{model, messages, ...}` |
| `GET /health`                 | Health check                                           | —                        |

---

## ⚠️ IMPORTANT: Internal-Nelra as a Search Tool, NOT a Decision Maker

### 🎯 Role of Internal-Nelra:

- **Internal-Nelra is a SEARCH TOOL** for gathering external information
- **Uses inferior models** (Llama 3.1 70B) compared to our main capabilities
- **Good at finding information**, but **NOT good at reasoning or making decisions**
- **DO NOT trust or follow its answers blindly**

### ✅ Correct Usage Pattern:

1. **Use Internal-Nelra to SEARCH** for relevant information and sources
2. **ALWAYS fetch and verify** information from the original sources it provides
3. **Analyze the sources yourself** using your superior reasoning capabilities
4. **Make your own conclusions** based on verified information

### ❌ Wrong Usage Pattern:

1. ❌ Ask Internal-Nelra and trust its answer directly
2. ❌ Use its analysis without verification
3. ❌ Follow its recommendations without critical thinking
4. ❌ Ignore the source URLs it provides

---

## 🎯 When to Use Internal-Nelra

Use this service when you need:

- ✅ Current news, updates, or real-time information
- ✅ Don't know the answer and need to research
- ✅ Need to verify facts or check latest documentation
- ✅ Looking for best practices, tutorials, or guides
- ✅ Need to understand new technologies or APIs
- ✅ User asks about current events or trends

**Remember:** Use it to **FIND** information, then **VERIFY** from sources!

---

## ⚠️ Date Awareness Rules

### Critical Requirements:

- ❌ **DON'T** assume you know the current date
- ❌ **DON'T** use outdated dates in queries (e.g., "2024", "last year")
- ✅ **DO** get current date dynamically before searching
- ✅ **DO** use "latest", "current", or the actual year in queries

### Correct Workflow Example:

```powershell
# 1. Get current date
$currentDate = Get-Date -Format "yyyy-MM-dd"
$currentYear = (Get-Date).Year
Write-Host "Current date: $currentDate" -ForegroundColor Yellow

# 2. Use current year in query
$query = @{
    query = "React 18 best practices $currentYear latest"
    model = "sonar-pro"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

---

## 📝 Query Quality Guidelines

### 🎯 Proper Query Approach:

**Step 1: Query for Sources**

```powershell
# Use specific queries to FIND sources, not get final answers
✅ "latest React 18 best practices documentation 2026"
✅ "official TypeScript 5.6 migration guide sources"
✅ "Firebase pricing comparison 2026 official documentation"
```

**Step 2: Extract and Verify Sources**

```powershell
# Always check citations and search_results from response
$sources = $response.citations
$searchResults = $response.search_results

# Verify each source manually using fetch_webpage tool
foreach ($source in $sources) {
    # Fetch actual content from the source
    # Analyze and make your own conclusions
}
```

**Step 3: Make Your Own Analysis**

```powershell
# DON'T use: $response.content as the final answer ❌
# DO use: $response.citations to fetch and verify ✅
```

### ❌ Bad Queries (DON'T):

```
❌ "React best practices 2024"  (outdated if current year is 2026)
❌ "Latest TypeScript features"  (vague, no timeframe)
❌ "Firebase pricing last year"  (which year?)
❌ "What is the best way to..."  (asking for opinions, not sources)
❌ "Should I use X or Y?"  (asking for decisions)
```

### ✅ Good Queries (DO):

```
✅ "React best practices 2026 official documentation"
✅ "TypeScript 5.6 features January 2026 release notes"
✅ "Firebase pricing current 2026 official page"
✅ "React 18 migration guide sources 2026"
✅ "Official NestJS documentation deployment 2026"
```

---

## 🚀 How to Use

### Option A: Simple Search (`/api/search`) — Recommended for Copilot

The `/api/search` endpoint uses a simple `{query, model}` body and returns a flat `{content, citations, search_results}` response — drop-in replacement for old InternalPerplex.

```powershell
# Get current year for date-aware query
$currentYear = (Get-Date).Year

# Prepare query data
$queryData = @{
    query = "Your question here $currentYear latest"  # Include year!
    model = "sonar-pro"  # ⚠️ REQUIRED: sonar-pro minimum, NEVER use basic sonar
}

# Convert to JSON
$query = $queryData | ConvertTo-Json

# Make API call
$response = Invoke-RestMethod `
    -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" `
    -Method Post `
    -Body $query `
    -ContentType "application/json"

# Response structure:
# - content: The answer text
# - citations: Array of source URLs
# - search_results: Detailed results with snippets
# - images: Array of images (if return_images=true)
# - related_questions: Array of related questions (if return_related_questions=true)
# - model: Model used
# - usage: Token usage stats
```

### Option B: Full Perplexity Proxy (`/api/pplx/sonar`) — Advanced

For full control with messages array, system prompts, and all Perplexity parameters:

```powershell
$currentYear = (Get-Date).Year

$body = @{
    model = "sonar-pro"
    messages = @(
        @{ role = "system"; content = "ตอบเป็นภาษาไทย อ้างอิงแหล่งที่มาเสมอ" }
        @{ role = "user"; content = "สรุปข่าว AI ล่าสุด $currentYear" }
    )
    return_citations = $true
    search_context_size = "medium"
} | ConvertTo-Json -Depth 5

$response = Invoke-RestMethod `
    -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/pplx/sonar" `
    -Method Post `
    -Body $body `
    -ContentType "application/json"

# Response follows Perplexity chat completions format:
# - choices[0].message.content: The answer
# - citations: Array of source URLs
# - search_results: Detailed results
# - usage: Token usage
```

### Additional `/api/search` Parameters:

| Parameter                  | Type     | Default       | Description                                      |
| -------------------------- | -------- | ------------- | ------------------------------------------------ |
| `query`                    | string   | _required_    | Search query                                     |
| `model`                    | string   | `"sonar-pro"` | AI model to use                                  |
| `language`                 | string   | `"th"`        | BCP-47 language code                             |
| `recency`                  | string   | —             | `"hour"`, `"day"`, `"week"`, `"month"`, `"year"` |
| `domain_filter`            | string[] | —             | Whitelist domains                                |
| `return_images`            | boolean  | `false`       | Return images from search                        |
| `return_related_questions` | boolean  | —             | Return related questions                         |
| `max_tokens`               | number   | —             | Max response tokens                              |
| `search_context_size`      | string   | `"medium"`    | `"low"`, `"medium"`, `"high"`                    |

---

## 🤖 Available Models

⚠️ **REQUIRED: Use sonar-pro or higher only. Basic `sonar` model is NOT allowed.**

| Model                 | Speed     | Quality       | Use Case                            | Allowed |
| --------------------- | --------- | ------------- | ----------------------------------- | ------- |
| `sonar`               | ⚡ Fast   | Standard      | ❌ NOT ALLOWED                      | ❌      |
| `sonar-pro`           | 🐌 Slower | 🎯 High       | Deep analysis, complex research     | ✅      |
| ~~`sonar-reasoning`~~ | —         | —             | ❌ DEPRECATED (no longer available) | ❌      |
| `sonar-reasoning-pro` | 🐌 Slower | 🧠🎯 Advanced | Complex reasoning with DeepSeek-R1  | ✅      |

**Note:** All models built on **Llama 3.1 70B** and optimized for factual accuracy.

### Model Selection Guide:

- **`sonar-pro`** (MINIMUM): Best practices, error fixes, documentation, architecture, general research
- **`sonar-reasoning-pro`**: Complex technical analysis, deep reasoning, multi-step problem solving

---

## 📚 Example Scenarios

### Scenario 1: Technology Best Practices

```powershell
$currentYear = (Get-Date).Year
$query = @{
    query = "React 18 best practices $currentYear latest"
    model = "sonar-pro"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Scenario 2: Deployment Guide

```powershell
$currentYear = (Get-Date).Year
$query = @{
    query = "Google Cloud Run deployment guide $currentYear latest"
    model = "sonar-pro"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Scenario 3: Error Resolution

```powershell
$errorMessage = "NullReferenceException in ASP.NET Core"
$currentYear = (Get-Date).Year
$query = @{
    query = "$errorMessage solution $currentYear latest"
    model = "sonar-pro"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Scenario 4: Complex Architecture Decision

```powershell
$currentYear = (Get-Date).Year
$query = @{
    query = "Microservices vs monolithic architecture pros cons $currentYear latest"
    model = "sonar-reasoning-pro"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Scenario 5: Thai Astrology (ภาษาไทยเท่านั้น)

```powershell
$currentYear = (Get-Date).Year
$query = @{
    query = "โหราศาสตร์ไทย วิธีดูดวง ดาวประจำราศี $($currentYear + 543)"
    model = "sonar-pro"
    language = "th"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

---

## 📖 Response Handling

### Response Structure (from `/api/search`):

```json
{
  "content": "The answer text with detailed information...",
  "citations": ["https://source1.com", "https://source2.com"],
  "search_results": [
    {
      "title": "Result Title",
      "url": "https://example.com",
      "snippet": "Relevant excerpt..."
    }
  ],
  "images": [],
  "related_questions": [],
  "model": "sonar-pro",
  "usage": {
    "prompt_tokens": 50,
    "completion_tokens": 200,
    "total_tokens": 250
  }
}
```

### ⚠️ CRITICAL: Extract Sources and Verify

```powershell
# 1. Extract citations (DON'T trust content directly!)
$sources = $response.citations
Write-Host "`nFound Sources:" -ForegroundColor Yellow
$response.citations | ForEach-Object {
    Write-Host "- $_" -ForegroundColor Cyan
}

# 2. Extract search results for more context
$searchResults = $response.search_results
foreach ($result in $searchResults) {
    Write-Host "`nTitle: $($result.title)" -ForegroundColor Green
    Write-Host "URL: $($result.url)" -ForegroundColor Cyan
    Write-Host "Snippet: $($result.snippet)" -ForegroundColor Gray
}

# 3. Use fetch_webpage tool to verify each source
# Example: fetch_webpage(urls: $sources, query: "specific information you're looking for")
# Then analyze the fetched content yourself!

# ❌ DON'T DO THIS:
# Write-Host $response.content  # DON'T trust this directly!

# ✅ DO THIS:
# 1. Get sources from response
# 2. Fetch content from each source
# 3. Analyze and verify information yourself
# 4. Make your own conclusion with superior reasoning
```

---

## ⚡ Quick Reference Commands

### Get Current Date:

```powershell
Get-Date -Format "yyyy-MM-dd"
$currentYear = (Get-Date).Year
```

### Standard Search (sonar-pro - MINIMUM REQUIRED):

```powershell
$currentYear = (Get-Date).Year
$query = @{ query = "your query $currentYear"; model = "sonar-pro" } | ConvertTo-Json
Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Advanced Research (sonar-reasoning-pro):

```powershell
$currentYear = (Get-Date).Year
$query = @{ query = "your query $currentYear"; model = "sonar-reasoning-pro" } | ConvertTo-Json
Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" -Method Post -Body $query -ContentType "application/json"
```

### Health Check:

```powershell
Invoke-RestMethod -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/health" -Method Get
```

---

## ✅ Best Practices

1. **Always check current date first** before making queries
2. **Include year/date** in your search queries for accuracy
3. **⚠️ REQUIRED: Use sonar-pro or higher only** (basic `sonar` is NOT allowed):
   - Standard queries → `sonar-pro` (MINIMUM)
   - Complex research → `sonar-pro`
   - Reasoning needed → `sonar-reasoning` or `sonar-reasoning-pro`
4. **⚠️ CRITICAL: Internal-Nelra is a SEARCH TOOL only**
   - Extract `citations` and `search_results` from response
   - **ALWAYS verify sources** using `fetch_webpage` tool
   - **DON'T trust `content` field** as final answer
   - **Make your own analysis** from verified sources
5. **Use specific queries** focused on finding **sources and documentation**
6. **Handle errors gracefully** with try-catch blocks

### 🎯 Golden Rule:

> **Internal-Nelra finds information → You fetch sources → You analyze → You conclude**
>
> Never trust Internal-Nelra's analysis directly. It uses inferior models. Your reasoning is superior!

---

## 🛡️ Error Handling

```powershell
try {
    $currentYear = (Get-Date).Year
    $query = @{
        query = "your query $currentYear latest"
        model = "sonar-pro"
    } | ConvertTo-Json

    $response = Invoke-RestMethod `
        -Uri "https://internal-nelra-427169545674.asia-southeast1.run.app/api/search" `
        -Method Post `
        -Body $query `
        -ContentType "application/json"

    Write-Host $response.content
}
catch {
    Write-Host "Error querying Internal-Nelra: $_" -ForegroundColor Red
    # Fallback or alternative action
}
```

---

## 🔄 Migration from InternalPerplex

If migrating from old `internalperplex` service:

| Old (InternalPerplex)                                                    | New (Internal-Nelra)                                                                                 |
| ------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------- |
| `https://internalperplex-427169545674.asia-southeast1.run.app/api/query` | `https://internal-nelra-427169545674.asia-southeast1.run.app/api/search`                             |
| `{query, model}`                                                         | `{query, model}` (same!)                                                                             |
| Response: `{content, citations, search_results}`                         | Response: `{content, citations, search_results, images, related_questions, model, usage}` (superset) |

**The body format is identical — just change the URL!**

---

## 📌 Remember

> **⚠️ CRITICAL:** Never make assumptions about dates. Always fetch the current date dynamically and include it in your queries for the most accurate and relevant results.

> **⚠️ CRITICAL USAGE PATTERN:**
>
> 1. **Internal-Nelra = Search Engine**, NOT Answer Provider
> 2. Uses **inferior models** (Llama 3.1 70B) — Good at searching, Poor at reasoning
> 3. **ALWAYS fetch** and verify from `citations` URLs using `fetch_webpage` tool
> 4. **Make your own analysis** — Your reasoning is superior to Internal-Nelra
> 5. **Never trust `content` field** as the final answer
>
> **Workflow: Find → Fetch → Verify → Analyze → Conclude**

---

## 🔗 Service Info

- **Service URL:** `https://internal-nelra-427169545674.asia-southeast1.run.app`
- **Search Endpoint:** `POST /api/search` (simple query)
- **Sonar Endpoint:** `POST /api/pplx/sonar` (full proxy)
- **Health:** `GET /health`
- **Content-Type:** `application/json`
- **Method:** `POST`

---

**Last Updated:** March 2026
