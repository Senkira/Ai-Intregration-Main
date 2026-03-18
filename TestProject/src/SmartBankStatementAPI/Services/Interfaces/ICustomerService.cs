using SmartBankStatementAPI.Models;
using SmartBankStatementAPI.Models.Requests;

namespace SmartBankStatementAPI.Services.Interfaces;

/// <summary>
/// Customer service interface (§4.1)
/// </summary>
public interface ICustomerService
{
    Task<ResultModel<CustomerResponse>> GetCustomerAsync(
        string customerId, CancellationToken cancellationToken = default);

    Task<ResultModel<IEnumerable<CustomerResponse>>> GetActiveCustomersAsync(
        CancellationToken cancellationToken = default);

    Task<ResultModel> CreateCustomerAsync(
        CreateCustomerRequest request, CancellationToken cancellationToken = default);
}
