using SmartBankStatementAPI.DataAccess.Connections;
using SmartBankStatementAPI.DataAccess.Repositories.Interfaces;
using SmartBankStatementAPI.DataAccess.SqlQueries;
using SmartBankStatementAPI.Models.Entities;

namespace SmartBankStatementAPI.DataAccess.Repositories;

/// <summary>
/// User repository — Dapper (§3.4, §3.12, §3.13)
/// </summary>
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory) { }

    public async Task<UserEntity?> GetByUserNameAsync(
        string userName, CancellationToken cancellationToken = default)
        => await QuerySingleOrDefaultAsync<UserEntity>(
            UserQueries.GetByUserName,
            new { UserName = userName },
            cancellationToken: cancellationToken);

    public async Task<int> InsertAsync(
        UserEntity entity, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            UserQueries.Insert, entity,
            cancellationToken: cancellationToken);

    public async Task<int> UpdateLastLoginAsync(
        int userId, CancellationToken cancellationToken = default)
        => await ExecuteAsync(
            UserQueries.UpdateLastLogin,
            new { UserId = userId, LastLoginDate = DateTime.UtcNow },
            cancellationToken: cancellationToken);
}
