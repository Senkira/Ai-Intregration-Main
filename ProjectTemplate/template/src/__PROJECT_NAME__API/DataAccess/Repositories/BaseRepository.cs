using System.Data.Common;
using Dapper;
using __PROJECT_NAME__API.DataAccess.Connections;

namespace __PROJECT_NAME__API.DataAccess.Repositories;

/// <summary>
/// Generic base class wrapping Dapper methods (§3.4)
/// ทุก Repository ต้อง extend class นี้
/// </summary>
public abstract class BaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private const int DefaultCommandTimeout = 30;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
        => _connectionFactory = connectionFactory;

    protected async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        object? param = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connectionFactory.CreateMainConnection();
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.QuerySingleOrDefaultAsync<T>(command)
            .ConfigureAwait(false);
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connectionFactory.CreateMainConnection();
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.QueryAsync<T>(command)
            .ConfigureAwait(false);
    }

    protected async Task<int> ExecuteAsync(
        string sql,
        object? param = null,
        int? commandTimeout = null,
        CancellationToken cancellationToken = default)
    {
        using var conn = _connectionFactory.CreateMainConnection();
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: commandTimeout ?? DefaultCommandTimeout,
            cancellationToken: cancellationToken);
        return await conn.ExecuteAsync(command)
            .ConfigureAwait(false);
    }

    protected async Task<T> ExecuteInTransactionAsync<T>(
        Func<DbConnection, DbTransaction, Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        await using var conn = _connectionFactory.CreateMainConnection();
        await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await conn.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
        try
        {
            var result = await action(conn, transaction).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
