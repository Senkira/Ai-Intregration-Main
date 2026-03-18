using SmartBankStatementAPI.DataAccess.Connections;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.DataAccess.SqlQueries;
using SmartBankStatementAPI.Models.Entities;

namespace SmartBankStatementAPI.DataAccess.Repositories;

/// <summary>
/// Statement repository — Dapper (§3.4, §3.12, §3.13)
/// </summary>
public class StatementRepository : BaseRepository, IStatementRepository
{
    public StatementRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public async Task<IEnumerable<StatementEntity>> GetByContractNoAsync(
        string contractNo, CancellationToken cancellationToken = default)
        => await QueryAsync<StatementEntity>(
            StatementQueries.GetByContractNo,
            new { ContractNo = contractNo },
            cancellationToken: cancellationToken);

    public async Task<StatementEntity?> GetByIdAsync(
        int statementId, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<StatementEntity>(
            StatementQueries.GetById,
            new { StatementId = statementId },
            cancellationToken: cancellationToken);

    public async Task<int> InsertAsync(
        StatementEntity entity, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            StatementQueries.Insert, entity,
            cancellationToken: cancellationToken);
}
