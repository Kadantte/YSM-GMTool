using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Services;

public sealed class IconPackService : IIconPackService
{
    private static readonly string[] IconExtensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];

    private readonly IIconEncoder? _encoder;

    public IconPackService(IIconEncoder? encoder = null)
    {
        _encoder = encoder;
    }

    public async Task<IconPackResult> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IReadOnlySet<string>? allowList,
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

        var candidates = Directory
            .EnumerateFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
            .Where(p => IconExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase))
            .ToArray();

        var files = allowList is null
            ? candidates
            : candidates
                .Where(p => allowList.Contains(Path.GetFileNameWithoutExtension(p).ToLowerInvariant()))
                .ToArray();

        await using var tx = (SqliteTransaction)await conn.BeginTransactionAsync(cancellationToken);

        var inserted = 0;
        var skipped = 0;
        long totalBytes = 0;

        for (var i = 0; i < files.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Path.GetFileName(files[i]);
            var sourceBytes = await File.ReadAllBytesAsync(files[i], cancellationToken);

            byte[] blob;
            if (_encoder is not null)
            {
                var encoded = _encoder.TryEncodeAsPng(sourceBytes);
                if (encoded is null) { skipped++; continue; }
                blob = encoded;
            }
            else
            {
                blob = sourceBytes;
            }

            await using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT OR REPLACE INTO Icons(file_name, png) VALUES ($n, $p)";
            cmd.Parameters.AddWithValue("$n", name);
            cmd.Parameters.AddWithValue("$p", blob);
            await cmd.ExecuteNonQueryAsync(cancellationToken);
            inserted++;
            totalBytes += blob.Length;

            if ((i & 0x1F) == 0) progress?.Report(new IconPackProgress(i + 1, files.Length));
        }

        await using (var meta = conn.CreateCommand())
        {
            meta.Transaction = tx;
            meta.CommandText = "INSERT OR REPLACE INTO Meta(key,value) VALUES ('packed_at',$t),('source_dir',$d),('file_count',$c),('skipped',$s)";
            meta.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("O"));
            meta.Parameters.AddWithValue("$d", sourceDirectory);
            meta.Parameters.AddWithValue("$c", inserted.ToString());
            meta.Parameters.AddWithValue("$s", skipped.ToString());
            await meta.ExecuteNonQueryAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);

        // Compact the DB file
        await using (var vacuum = conn.CreateCommand())
        {
            vacuum.CommandText = "VACUUM;";
            await vacuum.ExecuteNonQueryAsync(cancellationToken);
        }

        progress?.Report(new IconPackProgress(files.Length, files.Length));
        return new IconPackResult(inserted, skipped, totalBytes);
    }
}
