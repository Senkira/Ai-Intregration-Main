using System.ComponentModel.DataAnnotations;

namespace SmartBankStatementAPI.Models.Requests;

/// <summary>
/// Request model for getting statements (§2.29: Action+Feature+Request)
/// </summary>
public class GetStatementRequest
{
    [Required(ErrorMessage = "ContractNo is required.")]
    public string ContractNo { get; set; } = string.Empty;

    public string? CutOffDate { get; set; }
}
