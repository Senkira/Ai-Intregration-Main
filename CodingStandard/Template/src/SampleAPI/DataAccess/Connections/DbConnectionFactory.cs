// §3.3 — DbConnectionFactory: ห้าม new SqlConnection() ตรงใน Repository
// §6.3 — Singleton: stateless, thread-safe

using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace SampleAPI.DataAccess.Connections;

/// <summary>
/// Factory สร้าง SqlConnection จาก Configuration
/// Registered เป็น Singleton เพราะไม่มี state (§6.3)
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _mainConnectionString;
    private readonly string _logConnectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        // §9.3 — Connection String จาก appsettings ห้าม Hard-code
        // §3.21 — ห้ามอยู่ใน source code
        _mainConnectionString = configuration.GetConnectionString("MainDb")
            ?? throw new InvalidOperationException("MainDb connection string is not configured.");
        _logConnectionString = configuration.GetConnectionString("LogDb")
            ?? throw new InvalidOperationException("LogDb connection string is not configured.");
    }

    /// <inheritdoc />
    public DbConnection CreateMainConnection() =>
        new SqlConnection(_mainConnectionString);

    /// <inheritdoc />
    public DbConnection CreateLogConnection() =>
        new SqlConnection(_logConnectionString);
}
