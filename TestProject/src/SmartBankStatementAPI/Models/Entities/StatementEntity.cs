namespace SmartBankStatementAPI.Models.Entities;

/// <summary>
/// DB Mapping — estm_statement table (§2.27: Entity suffix)
/// </summary>
public class StatementEntity
{
    public int StatementId { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public string CutOffDate { get; set; } = string.Empty;
    public string AsOfDate { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
