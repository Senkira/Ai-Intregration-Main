using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace __PROJECT_NAME__API.DataAccess.Connections;

/// <summary>
/// Connection Factory implementation — SQLite for demo (§3.3)
/// Production: replace with SqlConnection + proper connection string
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
        => _configuration = configuration;

    public DbConnection CreateMainConnection()
        => new SqliteConnection(_configuration.GetConnectionString("MainDb"));
}
