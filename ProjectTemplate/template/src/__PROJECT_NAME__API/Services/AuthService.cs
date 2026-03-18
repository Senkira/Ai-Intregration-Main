using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using __PROJECT_NAME__API.DataAccess.Repositories.Interfaces;
using __PROJECT_NAME__API.Models;
using __PROJECT_NAME__API.Models.Requests;
using __PROJECT_NAME__API.Services.Interfaces;

namespace __PROJECT_NAME__API.Services;

/// <summary>
/// Authentication service — JWT token generation (§4, §7)
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ResultModel<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken cancellationToken = default)
    {
        var result = new ResultModel<LoginResponse>();
        try
        {
            // ── Guard Clause (§4.5) ──
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                result.Status = 400;
                result.IsSuccess = false;
                result.Message = "UserName and Password are required.";
                return result;
            }

            var user = await _userRepository.GetByUserNameAsync(
                request.UserName, cancellationToken);

            if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for user: {UserName}", request.UserName);
                result.Status = 401;
                result.IsSuccess = false;
                result.Message = "Invalid username or password.";
                return result;
            }

            if (!user.IsActive)
            {
                result.Status = 403;
                result.IsSuccess = false;
                result.Message = "User account is disabled.";
                return result;
            }

            var token = GenerateJwtToken(user.UserId, user.UserName, user.Role);
            var expiresInHours = _configuration.GetValue("Jwt:ExpiresInHours", 8);

            await _userRepository.UpdateLastLoginAsync(user.UserId, cancellationToken);

            _logger.LogInformation("User {UserName} logged in successfully", user.UserName);

            result.Status = 200;
            result.IsSuccess = true;
            result.Data = new LoginResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(expiresInHours)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LoginAsync for {UserName}", request.UserName);
            result.Status = 500;
            result.IsSuccess = false;
            result.Message = "Internal server error.";
        }
        return result;
    }

    private string GenerateJwtToken(int userId, string userName, string role)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                ?? "__PROJECT_NAME__API-Default-Secret-Key-For-Dev-Only!!"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresInHours = _configuration.GetValue("Jwt:ExpiresInHours", 8);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "__PROJECT_NAME__API",
            audience: _configuration["Jwt:Issuer"] ?? "__PROJECT_NAME__API",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiresInHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool VerifyPassword(string password, string hash)
        => HashPassword(password) == hash;

    /// <summary>
    /// Hash password with SHA256 — Production: use BCrypt/Argon2
    /// </summary>
    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
