// §12 — Unit Test Reference Implementation
// §12.2 — xUnit
// §12.3 — Moq
// §12.8 — Method Naming: {Method}_{Scenario}_{Expected}
// §12.9 — AAA Pattern: Arrange → Act → Assert
// §12.10 — ห้าม DB จริง ใช้ Mock Repository

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.DataAccess.Repositories.Interfaces;
using SampleAPI.Models.Entities;
using SampleAPI.Models.Requests;
using SampleAPI.Services;

namespace SampleAPI.UnitTest.Services;

/// <summary>
/// Unit Test สำหรับ CustomerService
/// §12.1 — ทุก Service Class ต้องมี Unit Test
/// </summary>
public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepo;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly CustomerService _sut; // System Under Test

    public CustomerServiceTests()
    {
        _mockRepo = new Mock<ICustomerRepository>();
        _mockLogger = new Mock<ILogger<CustomerService>>();

        // §4.8 — Config from appsettings (mock ด้วย InMemoryCollection)
        var configData = new Dictionary<string, string?>
        {
            ["Messages:CustomerNotFound"] = "Customer not found.",
            ["Messages:CustomerIdRequired"] = "CustomerId is required.",
            ["Messages:InternalServerError"] = "Internal server error."
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        _sut = new CustomerService(
            _mockRepo.Object, _mockLogger.Object, _configuration);
    }

    // ═══════════════════════════════════════════════════════
    // GetCustomerAsync Tests
    // ═══════════════════════════════════════════════════════

    [Fact]
    public async Task GetCustomerAsync_WhenFound_Returns200WithData()
    {
        // §12.9 — Arrange
        var entity = new CustomerEntity
        {
            CustomerId = "CUST001",
            CustomerName = "John Doe",
            Email = "john@example.com",
            PhoneNumber = "0812345678",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        _mockRepo.Setup(r => r.GetByIdAsync("CUST001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // §12.9 — Act
        var result = await _sut.GetCustomerAsync("CUST001");

        // §12.9 — Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal("CUST001", result.Data!.CustomerId);
        Assert.Equal("John Doe", result.Data.CustomerName);
    }

    [Fact]
    public async Task GetCustomerAsync_WhenNotFound_Returns404()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync("NOTEXIST", It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerEntity?)null);

        // Act
        var result = await _sut.GetCustomerAsync("NOTEXIST");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Equal("Customer not found.", result.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetCustomerAsync_WhenInvalidId_Returns400(string? customerId)
    {
        // Act
        var result = await _sut.GetCustomerAsync(customerId!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }

    [Fact]
    public async Task GetCustomerAsync_WhenDbThrows_Returns500AndLogs()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync("CUST001", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB connection failed"));

        // Act
        var result = await _sut.GetCustomerAsync("CUST001");

        // Assert — §8.6 ห้าม return exception message
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Equal("Internal server error.", result.Message);
    }

    // ═══════════════════════════════════════════════════════
    // GetCustomersAsync Tests
    // ═══════════════════════════════════════════════════════

    [Fact]
    public async Task GetCustomersAsync_WhenSuccess_ReturnsPaginatedData()
    {
        // Arrange
        var entities = new List<CustomerEntity>
        {
            new() { CustomerId = "C1", CustomerName = "Alice", Email = "a@x.com", IsActive = true },
            new() { CustomerId = "C2", CustomerName = "Bob", Email = "b@x.com", IsActive = true }
        };

        _mockRepo.Setup(r => r.GetPagedAsync(1, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);
        _mockRepo.Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(50);

        // Act
        var result = await _sut.GetCustomersAsync(1, 20);

        // Assert — §13.5 Pagination structure
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data!.Data.Count);
        Assert.Equal(1, result.Data.Pagination.Page);
        Assert.Equal(20, result.Data.Pagination.PageSize);
        Assert.Equal(50, result.Data.Pagination.TotalCount);
        Assert.Equal(3, result.Data.Pagination.TotalPages);
    }

    // ═══════════════════════════════════════════════════════
    // CreateCustomerAsync Tests
    // ═══════════════════════════════════════════════════════

    [Fact]
    public async Task CreateCustomerAsync_WhenValid_Returns201()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            CustomerName = "Jane Doe",
            Email = "jane@example.com",
            PhoneNumber = "0891234567"
        };

        _mockRepo.Setup(r => r.CreateAsync(
                It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.CreateCustomerAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.Status);

        // §12 — Verify repository was called
        _mockRepo.Verify(r => r.CreateAsync(
            It.Is<CustomerEntity>(e =>
                e.CustomerName == "Jane Doe" &&
                e.Email == "jane@example.com"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCustomerAsync_WhenNullRequest_Returns400()
    {
        // Act
        var result = await _sut.CreateCustomerAsync(null!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }
}
