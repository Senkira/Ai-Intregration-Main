using __PROJECT_NAME__API.Models;
using __PROJECT_NAME__API.Models.Requests;

namespace __PROJECT_NAME__API.Services.Interfaces;

/// <summary>
/// Authentication service interface (§4.1)
/// </summary>
public interface IAuthService
{
    Task<ResultModel<LoginResponse>> LoginAsync(
        LoginRequest request, CancellationToken cancellationToken = default);
}
