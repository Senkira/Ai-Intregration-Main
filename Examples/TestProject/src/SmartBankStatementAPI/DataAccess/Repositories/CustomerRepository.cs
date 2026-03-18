using SmartBankStatementAPI.DataAccess.Connections;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.DataAccess.SqlQueries;
using SmartBankStatementAPI.Models.Entities;

namespace SmartBankStatementAPI.DataAccess.Repositories;

/// <summary>
/// Customer repository — Dapper (§3.4, §3.12, §3.13)
/// </summary>
public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public async Task<CustomerEntity?> GetByIdAsync(
        string customerId, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<CustomerEntity>(
            CustomerQueries.GetById,
            new { CustomerId = customerId },
            cancellationToken: cancellationToken);

    public async Task<IEnumerable<CustomerEntity>> GetActiveListAsync(
        CancellationToken cancellationToken = default)
        => await QueryAsync<CustomerEntity>(
            CustomerQueries.GetActiveList,
            cancellationToken: cancellationToken);

    public async Task<int> InsertAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            CustomerQueries.Insert, entity,
            cancellationToken: cancellationToken);

    public async Task<int> UpdateAsync(
        CustomerEntity entity, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            CustomerQueries.Update, entity,
            cancellationToken: cancellationToken);
}
