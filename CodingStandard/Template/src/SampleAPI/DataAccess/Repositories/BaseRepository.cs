// §3.4 — BaseRepository: Generic Base Class wrap Dapper methods
// §3.12 — ทุก method เป็น async Task<T>
// §3.14 — ส่ง CancellationToken ผ่าน CommandDefinition
// §3.16 — default commandTimeout 30s

using System.Data;
using System.Data.Common;
using Dapper;
using SampleAPI.DataAccess.Connections;
using SampleAPI.DataAccess.Repositories.Interfaces;

namespace SampleAPI.DataAccess.Repositories;

/// <summary>
/// BaseRepository — wrap Dapper methods ลด boilerplate
/// ใช้ ConfigureAwait(false) เพราะเป็น library code (§ note)
/// </summary>
public class BaseRepository : IBaseRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// สร้าง Connection จาก Factory — override ได้สำหรับ multi-DB (§3.5)
    /// </summary>
    protected virtual DbConnection CreateConnection() =>
        _connectionFactory.CreateMainConnection();

    /// <inheritdoc />
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default)
    {
        // §3.10 — using ปิด connection เสมอ
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        // §3.14 — CancellationToken ผ่าน CommandDefinition เท่านั้น
        var command = new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            commandTimeout: commandTimeout,
            cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<T>(command)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var command = new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            commandTimeout: commandTimeout,
            cancellationToken: cancellationToken);

        return await connection.QueryAsync<T>(command)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var command = new CommandDefinition(
            commandText: sql,
            parameters: parameters,
            commandTimeout: commandTimeout,
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<IDbTransaction, CancellationToken, Task<T>> operation,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default)
    {
        // §3.9 — Transaction ทุก multi-operation
        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        try
        {
            var result = await operation(transaction, cancellationToken)
                .ConfigureAwait(false);
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
