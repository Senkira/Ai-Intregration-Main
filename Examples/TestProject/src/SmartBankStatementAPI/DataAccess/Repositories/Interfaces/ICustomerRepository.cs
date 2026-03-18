using SmartBankStatementAPI.Models.Entities;

namespace SmartBankStatementAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// Customer repository interface (§2.26, §3.5)
/// </summary>
public interface ICustomerRepository
{
    Task<CustomerEntity?> GetByIdAsync(
        string customerId, CancellationToken cancellationToken = default);

    Task<IEnumerable<CustomerEntity>> GetActiveListAsync(
        CancellationToken cancellationToken = default);

    Task<int> InsertAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default);

    Task<int> UpdateAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default);
}
