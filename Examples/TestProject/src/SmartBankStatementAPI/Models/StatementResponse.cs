namespace SmartBankStatementAPI.Models;

/// <summary>
/// Response model for statement data (§2.30)
/// </summary>
public class StatementResponse
{
    public int StatementId { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public string CutOffDate { get; set; } = string.Empty;
    public string AsOfDate { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
