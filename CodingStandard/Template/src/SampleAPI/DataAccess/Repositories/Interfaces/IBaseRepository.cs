// §3.4 — IBaseRepository: Interface สำหรับ Generic Base
// §3.5 — รองรับ Multi-DB ผ่าน method parameter

namespace SampleAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// Base Repository Interface — Generic Data Access Methods
/// ทุก Repository ต้อง inherit จาก IBaseRepository
/// </summary>
public interface IBaseRepository
{
    /// <summary>
    /// Query รายการเดียว (§3.12 — async + §3.13 — CancellationToken)
    /// </summary>
    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Query หลายรายการ
    /// </summary>
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute INSERT/UPDATE/DELETE (§3.27 — ควรมี Audit Log)
    /// </summary>
    Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute ภายใน Transaction (§3.9 — Transaction ทุก multi-operation)
    /// </summary>
    Task<T> ExecuteInTransactionAsync<T>(
        Func<System.Data.IDbTransaction, CancellationToken, Task<T>> operation,
        int commandTimeout = 30,
        CancellationToken cancellationToken = default);
}
