using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Services;

public sealed class IconPackService : IIconPackService
{
    public async Task<int> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IProgress<IconPackProgress>? progress,
        CancellationToken cancellationToken)
    {
        if (File.Exists(targetIconsDbPath)) File.Delete(targetIconsDbPath);

        await using var conn = new SqliteConnection($"Data Source={targetIconsDbPath}");
        await conn.OpenAsync(cancellationToken);

        await using (var schema = conn.CreateCommand())
        {
            schema.CommandText = """
                CREATE TABLE Icons (file_name TEXT PRIMARY KEY COLLATE NOCASE, png BLOB NOT NULL);
                CREATE TABLE Meta  (key TEXT PRIMARY KEY, value TEXT);
                """;
            await schema.ExecuteNonQueryAsync(cancellationToken);
        }

        string[] extensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];
        var files = Directory
            .EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
            .Where(p => extensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase))
            .ToArray();
        await using var tx = (SqliteTransaction)await conn.BeginTransactionAsync(cancellationToken);

        var inserted = 0;
        for (var i = 0; i < files.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Path.GetFileName(files[i]);
            var bytes = await File.ReadAllBytesAsync(files[i], cancellationToken);

            await using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT OR REPLACE INTO Icons(file_name, png) VALUES ($n, $p)";
            cmd.Parameters.AddWithValue("$n", name);
            cmd.Parameters.AddWithValue("$p", bytes);
            await cmd.ExecuteNonQueryAsync(cancellationToken);
            inserted++;

            if ((i & 0x1F) == 0) progress?.Report(new IconPackProgress(i + 1, files.Length));
        }

        await using (var meta = conn.CreateCommand())
        {
            meta.Transaction = tx;
            meta.CommandText = "INSERT OR REPLACE INTO Meta(key,value) VALUES ('packed_at',$t),('source_dir',$d),('file_count',$c)";
            meta.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("O"));
            meta.Parameters.AddWithValue("$d", sourceDirectory);
            meta.Parameters.AddWithValue("$c", inserted.ToString());
            await meta.ExecuteNonQueryAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);
        progress?.Report(new IconPackProgress(files.Length, files.Length));
        return inserted;
    }
}
