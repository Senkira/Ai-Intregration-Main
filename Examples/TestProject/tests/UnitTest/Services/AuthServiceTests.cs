using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.Models.Entities;
using SmartBankStatementAPI.Models.Requests;
using SmartBankStatementAPI.Services;

namespace SmartBankStatementAPI.UnitTest.Services;

/// <summary>
/// Unit tests for AuthService (§12)
/// </summary>
public class AuthServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _logger = Substitute.For<ILogger<AuthService>>();

        var configData = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "SmartBankStatementAPI-Default-Secret-Key-For-Dev-Only-2025!!",
            ["Jwt:Issuer"] = "SmartBankStatementAPI",
            ["Jwt:ExpiresInHours"] = "8"
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        _sut = new AuthService(_userRepository, _configuration, _logger);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var passwordHash = AuthService.HashPassword("password123");
        var user = new UserEntity
        {
            UserId = 1,
            UserName = "testuser",
            PasswordHash = passwordHash,
            Role = "Admin",
            IsActive = true
        };

        _userRepository
            .GetByUserNameAsync("testuser", Arg.Any<CancellationToken>())
            .Returns(user);

        var request = new LoginRequest
        {
            UserName = "testuser",
            Password = "password123"
        };

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data!.Token);
        Assert.Equal("testuser", result.Data.UserName);
        Assert.Equal("Admin", result.Data.Role);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyCredentials_Returns400()
    {
        // Arrange
        var request = new LoginRequest { UserName = "", Password = "" };

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.Status);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_Returns401()
    {
        // Arrange
        var user = new UserEntity
        {
            UserId = 1,
            UserName = "testuser",
            PasswordHash = AuthService.HashPassword("correctpassword"),
            Role = "User",
            IsActive = true
        };

        _userRepository
            .GetByUserNameAsync("testuser", Arg.Any<CancellationToken>())
            .Returns(user);

        var request = new LoginRequest
        {
            UserName = "testuser",
            Password = "wrongpassword"
        };

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(401, result.Status);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_Returns401()
    {
        // Arrange
        _userRepository
            .GetByUserNameAsync("nobody", Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var request = new LoginRequest
        {
            UserName = "nobody",
            Password = "any"
        };

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(401, result.Status);
    }

    [Fact]
    public async Task LoginAsync_WhenUserInactive_Returns403()
    {
        // Arrange
        var user = new UserEntity
        {
            UserId = 1,
            UserName = "disabled",
            PasswordHash = AuthService.HashPassword("pass"),
            Role = "User",
            IsActive = false
        };

        _userRepository
            .GetByUserNameAsync("disabled", Arg.Any<CancellationToken>())
            .Returns(user);

        var request = new LoginRequest
        {
            UserName = "disabled",
            Password = "pass"
        };

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Equal("User account is disabled.", result.Message);
    }
}
