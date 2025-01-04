using System.Data;
using System.Data.SqlClient;

namespace Mpmt.Data.Common
{
    /// <summary>
    /// The db connection manager.
    /// </summary>
    public static class DbConnectionManager
    {
        // Bind this connection string during application startup
        public static string DefaultConnectionString = string.Empty;

        /// <summary>
        /// Gets the default connection.
        /// </summary>
        /// <returns>An IDbConnection.</returns>
        public static IDbConnection GetDefaultConnection()
        {
            return new SqlConnection(DefaultConnectionString);
        }
    }
}
