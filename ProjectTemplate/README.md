# 🏗️ Project Template — Coding Standard Compliant

> **Version:** 1.0  
> **Framework:** .NET 10 + Dapper + xUnit + NSubstitute  
> **Coding Standard:** §1–§12 (Core Mandatory)

---

## วิธีใช้

### Quick Start

```powershell
# สร้างโปรเจคใหม่ชื่อ "RLSR061"
.\New-Project.ps1 -ProjectName "RLSR061" -TaskId "RLSR061-001"

# สร้างไปยัง path ที่ต้องการ
.\New-Project.ps1 -ProjectName "OmniChannel" -OutputPath "E:\Projects" -TaskId "OC-001"
```

### จากนั้น

```powershell
cd RLSR061
dotnet build RLSR061.slnx
dotnet test
```

---

## สิ่งที่ Template มีให้

### 📁 Source Code — พร้อมใช้งาน

| Layer | Files | สิ่งที่มี |
|-------|:-----:|---------|
| **Infrastructure** | 5 | Program.cs (DI, JWT, Swagger, Filters) + appsettings ×4 |
| **Auth Feature** | 8 | AuthController + AuthService + UserRepository + UserQueries |
| **DataAccess Base** | 4 | IDbConnectionFactory + DbConnectionFactory + BaseRepository |
| **Models Base** | 3 | ResultModel + HealthCheckModel + LoginResponse |
| **Filters** | 2 | LogFilter + AvailableFilter |
| **Tests** | 1 | AuthServiceTests (5 tests) |

### 📁 Docs Pipeline — Template เปล่าพร้อมกรอก

```
docs/
├── README.md .................... Pipeline flow
├── Project_Summary.md ........... สรุปโปรเจค
├── 01_Solution_Architect/ ....... SA output templates
├── 02_Task_QA/ .................. QA report template
├── 03_Developer/ ................ Dev report template
├── 04_Developer_QC/ ............. QC report template
└── 05_Coding_Standard_Checklist/  Checklist template
```

---

## สิ่งที่ต้องทำหลังสร้างโปรเจค

### ✅ ใช้ได้เลย (ไม่ต้องแก้)

- [x] JWT Authentication (Login/Token)
- [x] HealthCheck endpoint
- [x] LogFilter + AvailableFilter
- [x] BaseRepository (Dapper wrapper)
- [x] ResultModel
- [x] appsettings 4 environments
- [x] AuthService + 5 Unit Tests

### 📝 ต้องเพิ่มเอง (ตาม business domain)

| # | สิ่งที่ต้องทำ | ไฟล์ที่ต้องแก้ | ตัวอย่าง |
|---|-------------|-------------|---------|
| 1 | **สร้าง Entity** | `Models/Entities/` | `OrderEntity.cs` |
| 2 | **สร้าง Request** | `Models/Requests/` | `CreateOrderRequest.cs` |
| 3 | **สร้าง Response** | `Models/` | `OrderResponse.cs` |
| 4 | **สร้าง SqlQueries** | `DataAccess/SqlQueries/` | `OrderQueries.cs` |
| 5 | **สร้าง Repository** | `DataAccess/Repositories/` | `OrderRepository.cs` + Interface |
| 6 | **สร้าง Service** | `Services/` | `OrderService.cs` + Interface |
| 7 | **สร้าง Controller** | `Controllers/` | `OrderController.cs` |
| 8 | **Register DI** | `Program.cs` | Uncomment TODO lines |
| 9 | **สร้าง DB Tables** | `Program.cs` → `InitializeDatabase` | เพิ่ม CREATE TABLE |
| 10 | **สร้าง Unit Tests** | `tests/UnitTest/Services/` | `OrderServiceTests.cs` |

---

## Coding Standard Pattern Reference

### Service Pattern (§4)

```csharp
public async Task<ResultModel<XxxResponse>> GetXxxAsync(
    string id, CancellationToken cancellationToken = default)
{
    var result = new ResultModel<XxxResponse>();
    try
    {
        // Guard (§4.5)
        if (string.IsNullOrWhiteSpace(id))
        {
            result.Status = 400; result.IsSuccess = false;
            result.Message = "Id is required.";
            return result;
        }

        // Business Logic
        var entity = await _xxxRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            result.Status = 404; result.IsSuccess = false;
            result.Message = "Not found.";
            return result;
        }

        result.Status = 200; result.IsSuccess = true;
        result.Data = MapToResponse(entity);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error in GetXxxAsync for {Id}", id);
        result.Status = 500; result.IsSuccess = false;
        result.Message = "Internal server error.";
    }
    return result;
}
```

### Controller Pattern (§5)

```csharp
[Authorize]
[HttpGet("{id}")]
public async Task<IActionResult> GetXxxAsync(
    string id, CancellationToken cancellationToken)
{
    var result = await _xxxService.GetXxxAsync(id, cancellationToken);
    return StatusCode(result.Status, result);
}
```

### Repository Pattern (§3)

```csharp
public class XxxRepository : BaseRepository, IXxxRepository
{
    public XxxRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public async Task<XxxEntity?> GetByIdAsync(
        string id, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<XxxEntity>(
            XxxQueries.GetById,
            new { Id = id },
            cancellationToken: cancellationToken);
}
```

### Unit Test Pattern (§12)

```csharp
[Fact]
public async Task GetXxxAsync_WithValidId_ReturnsXxx()
{
    // Arrange
    _xxxRepository.GetByIdAsync("001", Arg.Any<CancellationToken>())
        .Returns(new XxxEntity { Id = "001" });

    // Act
    var result = await _sut.GetXxxAsync("001");

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(200, result.Status);
}
```

---

## ข้อควรรู้

| Item | ค่าที่ตั้งไว้ | Production |
|------|-------------|-----------|
| DB | SQLite (file-based) | เปลี่ยนเป็น SQL Server |
| Password Hash | SHA256 | เปลี่ยนเป็น BCrypt/Argon2 |
| JWT Key | Hardcoded ใน appsettings | ใช้ Secret Vault |
| Swagger | Swashbuckle 7.3.1 | พิจารณา built-in OpenAPI |
