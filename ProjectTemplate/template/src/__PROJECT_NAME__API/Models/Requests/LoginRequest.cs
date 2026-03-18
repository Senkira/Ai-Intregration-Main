using System.ComponentModel.DataAnnotations;

namespace __PROJECT_NAME__API.Models.Requests;

/// <summary>
/// Request model for login (§2.29: Action+Feature+Request)
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
