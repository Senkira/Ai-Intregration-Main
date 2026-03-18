using SmartBankStatementAPI.Models;
using SmartBankStatementAPI.Models.Requests;

namespace SmartBankStatementAPI.Services.Interfaces;

/// <summary>
/// Statement service interface (§4.1)
/// </summary>
public interface IStatementService
{
    Task<ResultModel<IEnumerable<StatementResponse>>> GetStatementAsync(
        GetStatementRequest request, CancellationToken cancellationToken = default);

    Task<ResultModel<StatementResponse>> GetStatementByIdAsync(
        int statementId, CancellationToken cancellationToken = default);
}
