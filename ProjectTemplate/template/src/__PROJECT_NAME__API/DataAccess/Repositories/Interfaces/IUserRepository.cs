using __PROJECT_NAME__API.Models.Entities;

namespace __PROJECT_NAME__API.DataAccess.Repositories.Interfaces;

/// <summary>
/// User repository interface (§2.26, §3.5)
/// </summary>
public interface IUserRepository
{
    Task<UserEntity?> GetByUserNameAsync(
        string userName, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(
        UserEntity entity, CancellationToken cancellationToken = default);

    Task<int> UpdateLastLoginAsync(
        int userId, CancellationToken cancellationToken = default);
}
