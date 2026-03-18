namespace __PROJECT_NAME__API.Models;

/// <summary>
/// Response model for login result (§2.30)
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
