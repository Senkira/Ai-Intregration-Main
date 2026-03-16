// §3.3 — IDbConnectionFactory: Factory สร้าง Connection
// §6.3 — Singleton: stateless, thread-safe, ไม่ hold connection

using System.Data.Common;

namespace SampleAPI.DataAccess.Connections;

/// <summary>
/// Factory สำหรับสร้าง Database Connection — return DbConnection เพราะ
/// OpenAsync() / BeginTransactionAsync() อยู่บน DbConnection เท่านั้น
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// สร้าง Connection สำหรับ Main Database
    /// </summary>
    DbConnection CreateMainConnection();

    /// <summary>
    /// สร้าง Connection สำหรับ Log Database
    /// </summary>
    DbConnection CreateLogConnection();
}
