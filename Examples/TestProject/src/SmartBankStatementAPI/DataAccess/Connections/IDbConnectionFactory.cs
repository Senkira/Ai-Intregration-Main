using System.Data.Common;

namespace SmartBankStatementAPI.DataAccess.Connections;

/// <summary>
/// Connection Factory interface (§3.3)
/// </summary>
public interface IDbConnectionFactory
{
    DbConnection CreateMainConnection();
}
