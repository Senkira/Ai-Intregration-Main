namespace SmartBankStatementAPI.Models;

/// <summary>
/// Response model for customer data (§2.30)
/// </summary>
public class CustomerResponse
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = string.Empty;
}
