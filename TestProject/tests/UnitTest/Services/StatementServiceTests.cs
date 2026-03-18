using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Models.Entities;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services;

namespace SmartBankStatementAPI.UnitTest.Services;

/// <summary>
/// Unit tests for StatementService (§12)
/// </summary>
public class StatementServiceTests
{
    private readonly IStatementRepository _statementRepository;
    private readonly ILogger<StatementService> _logger;
    private readonly StatementService _sut;

    public StatementServiceTests()
    {
        _statementRepository = Substitute.For<IStatementRepository>();
        _logger = Substitute.For<ILogger<StatementService>>();
        _sut = new StatementService(_statementRepository, _logger);
    }

    [Fact]
    public async Task GetStatementAsync_WithValidContractNo_ReturnsStatements()
    {
        // Arrange
        var entities = new List<StatementEntity>
        {
            new()
            {
                StatementId = 1,
                ContractNo = "CTR001",
                CutOffDate = "2025-01-31",
                AsOfDate = "2025-02-01",
                FilePath = "/files/stmt1.pdf",
                Status = "ACTIVE"
            }
        };

        _statementRepository
            .GetByContractNoAsync("CTR001", Arg.Any<CancellationToken>())
            .Returns(entities);

        var request = new GetStatementRequest { ContractNo = "CTR001" };

        // Act
        var result = await _sut.GetStatementAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.Single(result.Data!);
    }

    [Fact]
    public async Task GetStatementAsync_WithEmptyContractNo_Returns400()
    {
        // Arrange
        var request = new GetStatementRequest { ContractNo = "" };

        // Act
        var result = await _sut.GetStatementAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }

    [Fact]
    public async Task GetStatementByIdAsync_WhenNotFound_Returns404()
    {
        // Arrange
        _statementRepository
            .GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((StatementEntity?)null);

        // Act
        var result = await _sut.GetStatementByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Equal("Statement not found.", result.Message);
    }

    [Fact]
    public async Task GetStatementByIdAsync_WhenFound_ReturnsStatement()
    {
        // Arrange
        var entity = new StatementEntity
        {
            StatementId = 1,
            ContractNo = "CTR001",
            CutOffDate = "2025-01-31",
            AsOfDate = "2025-02-01",
            FilePath = "/files/stmt1.pdf",
            Status = "ACTIVE"
        };

        _statementRepository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(entity);

        // Act
        var result = await _sut.GetStatementByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.Equal("CTR001", result.Data!.ContractNo);
    }

    [Fact]
    public async Task GetStatementAsync_WhenRepositoryThrows_Returns500()
    {
        // Arrange
        _statementRepository
            .GetByContractNoAsync("CTR001", Arg.Any<CancellationToken>())
            .Returns<IEnumerable<StatementEntity>>(_ => throw new Exception("DB down"));

        var request = new GetStatementRequest { ContractNo = "CTR001" };

        // Act
        var result = await _sut.GetStatementAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Equal("Internal server error.", result.Message);
    }
}
