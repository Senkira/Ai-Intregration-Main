using SmartBankStatementAPI.Models;
using SmartBankStatementAPI.Models.Requests;

namespace SmartBankStatementAPI.Services.Interfaces;

/// <summary>
/// Authentication service interface (§4.1)
/// </summary>
public interface IAuthService
{
    Task<ResultModel<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken cancellationToken = default);
}
