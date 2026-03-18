using System.ComponentModel.DataAnnotations;

namespace SmartBankStatementAPI.Models.Requests;

/// <summary>
/// Request model for creating customer (§2.29: Action+Feature+Request)
/// </summary>
public class CreateCustomerRequest
{
    [Required(ErrorMessage = "CustomerId is required.")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "CustomerId must be 13 characters.")]
    public string CustomerId { get; set; } = string.Empty;

    [Required(ErrorMessage = "CustomerName is required.")]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string? PhoneNumber { get; set; }
}
