using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Infrastructure;

public sealed class SqliteIconSource : IIconSource, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly object _lock = new();

    public SqliteIconSource(string iconsDbPath)
    {
        _connection = new SqliteConnection($"Data Source={iconsDbPath};Mode=ReadOnly");
        _connection.Open();
    }

    public byte[]? TryGetIconBytes(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return null;
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT png FROM Icons WHERE file_name = $n";
            cmd.Parameters.AddWithValue("$n", fileName);
            var result = cmd.ExecuteScalar();
            return result as byte[];
        }
    }

    public void Dispose() => _connection.Dispose();
}
