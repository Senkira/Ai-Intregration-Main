using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Models.Entities;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services;

namespace SmartBankStatementAPI.UnitTest.Services;

/// <summary>
/// Unit tests for CustomerService (§12)
/// Tests business logic via mock repositories — no DB required
/// </summary>
public class CustomerServiceTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;
    private readonly CustomerService _sut; // System Under Test

    public CustomerServiceTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _logger = Substitute.For<ILogger<CustomerService>>();
        _sut = new CustomerService(_customerRepository, _logger);
    }

    [Fact]
    public async Task GetCustomerAsync_WithValidId_ReturnsCustomer()
    {
        // Arrange
        var entity = new CustomerEntity
        {
            CustomerId = "1234567890123",
            CustomerName = "Test Customer",
            Email = "test@example.com",
            Status = "ACTIVE"
        };

        _customerRepository
            .GetByIdAsync("1234567890123", Arg.Any<CancellationToken>())
            .Returns(entity);

        // Act
        var result = await _sut.GetCustomerAsync("1234567890123");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.Data);
        Assert.Equal("1234567890123", result.Data!.CustomerId);
        Assert.Equal("Test Customer", result.Data.CustomerName);
    }

    [Fact]
    public async Task GetCustomerAsync_WithNullId_Returns400()
    {
        // Act
        var result = await _sut.GetCustomerAsync(null!);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
        Assert.Equal("CustomerId is required.", result.Message);
    }

    [Fact]
    public async Task GetCustomerAsync_WithEmptyId_Returns400()
    {
        // Act
        var result = await _sut.GetCustomerAsync("  ");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }

    [Fact]
    public async Task GetCustomerAsync_WhenNotFound_Returns404()
    {
        // Arrange
        _customerRepository
            .GetByIdAsync("9999999999999", Arg.Any<CancellationToken>())
            .Returns((CustomerEntity?)null);

        // Act
        var result = await _sut.GetCustomerAsync("9999999999999");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Equal("Customer not found.", result.Message);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_Returns201()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            CustomerId = "1234567890123",
            CustomerName = "New Customer",
            Email = "new@example.com"
        };

        _customerRepository
            .GetByIdAsync("1234567890123", Arg.Any<CancellationToken>())
            .Returns((CustomerEntity?)null);

        _customerRepository
            .InsertAsync(Arg.Any<CustomerEntity>(), Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        var result = await _sut.CreateCustomerAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.Status);
        Assert.Equal("Customer created successfully.", result.Message);
    }

    [Fact]
    public async Task CreateCustomerAsync_WhenDuplicate_Returns409()
    {
        // Arrange
        var existing = new CustomerEntity
        {
            CustomerId = "1234567890123",
            CustomerName = "Existing"
        };

        _customerRepository
            .GetByIdAsync("1234567890123", Arg.Any<CancellationToken>())
            .Returns(existing);

        var request = new CreateCustomerRequest
        {
            CustomerId = "1234567890123",
            CustomerName = "Duplicate"
        };

        // Act
        var result = await _sut.CreateCustomerAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Status);
        Assert.Equal("Customer already exists.", result.Message);
    }

    [Fact]
    public async Task GetActiveCustomersAsync_ReturnsAllActive()
    {
        // Arrange
        var entities = new List<CustomerEntity>
        {
            new() { CustomerId = "001", CustomerName = "A", Status = "ACTIVE" },
            new() { CustomerId = "002", CustomerName = "B", Status = "ACTIVE" }
        };

        _customerRepository
            .GetActiveListAsync(Arg.Any<CancellationToken>())
            .Returns(entities);

        // Act
        var result = await _sut.GetActiveCustomersAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.Equal(2, result.Data!.Count());
    }

    [Fact]
    public async Task GetCustomerAsync_WhenRepositoryThrows_Returns500()
    {
        // Arrange
        _customerRepository
            .GetByIdAsync("1234567890123", Arg.Any<CancellationToken>())
            .Returns<CustomerEntity?>(_ => throw new InvalidOperationException("DB error"));

        // Act
        var result = await _sut.GetCustomerAsync("1234567890123");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Equal("Internal server error.", result.Message);
    }
}
