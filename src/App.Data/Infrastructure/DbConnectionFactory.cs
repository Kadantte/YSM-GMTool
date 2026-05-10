using System.Data.Common;
using App.Core.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySqlConnector;

namespace App.Data.Infrastructure;

public sealed class DbConnectionFactory
{
    public DbConnection Create(DatabaseProvider provider, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string is empty. Open Settings and configure the database connection.");
        }

        return provider switch
        {
            DatabaseProvider.MSSQL => new SqlConnection(connectionString),
            DatabaseProvider.MySQL => new MySqlConnection(connectionString),
            DatabaseProvider.Sqlite => new SqliteConnection(connectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unsupported database provider.")
        };
    }
}
