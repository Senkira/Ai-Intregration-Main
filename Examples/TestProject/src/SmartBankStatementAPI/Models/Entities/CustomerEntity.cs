namespace SmartBankStatementAPI.Models.Entities;

/// <summary>
/// DB Mapping — customer table (§2.27: Entity suffix)
/// </summary>
public class CustomerEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
