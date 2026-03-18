using System.Data.Common;

namespace __PROJECT_NAME__API.DataAccess.Connections;

/// <summary>
/// Connection Factory interface (§3.3)
/// </summary>
public interface IDbConnectionFactory
{
    DbConnection CreateMainConnection();
}
