using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Infrastructure;

public sealed class SqliteIconSource : IIconSource, IDisposable
{
    private static readonly string[] IconExtensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];

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

        var key = Path.GetFileName(fileName.Trim());

        lock (_lock)
        {
            var direct = QueryByName(key);
            if (direct is not null) return direct;

            if (Path.HasExtension(key)) return null;

            foreach (var ext in IconExtensions)
            {
                var candidate = QueryByName(key + ext);
                if (candidate is not null) return candidate;
            }
            return null;
        }
    }

    private byte[]? QueryByName(string name)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT png FROM Icons WHERE file_name = $n";
        cmd.Parameters.AddWithValue("$n", name);
        return cmd.ExecuteScalar() as byte[];
    }

    public void Dispose() => _connection.Dispose();
}
