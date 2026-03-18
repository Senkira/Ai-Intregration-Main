using SmartBankStatementAPI.Models.Entities;

namespace SmartBankStatementAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// Statement repository interface (§2.26, §3.5)
/// </summary>
public interface IStatementRepository
{
    Task<IEnumerable<StatementEntity>> GetByContractNoAsync(
        string contractNo, CancellationToken cancellationToken = default);

    Task<StatementEntity?> GetByIdAsync(
        int statementId, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(
        StatementEntity entity, CancellationToken cancellationToken = default);
}
