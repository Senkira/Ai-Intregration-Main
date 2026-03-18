namespace __PROJECT_NAME__API.Models.Entities;

/// <summary>
/// DB Mapping — app_user table (§2.27: Entity suffix)
/// </summary>
public class UserEntity
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}
