using __PROJECT_NAME__API.DataAccess.Connections;
using __PROJECT_NAME__API.DataAccess.Repositories.Interfaces;
using __PROJECT_NAME__API.DataAccess.SqlQueries;
using __PROJECT_NAME__API.Models.Entities;

namespace __PROJECT_NAME__API.DataAccess.Repositories;

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
