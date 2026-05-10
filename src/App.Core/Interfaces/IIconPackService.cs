namespace App.Core.Interfaces;

public sealed record IconPackProgress(int Current, int Total);

public sealed record IconPackResult(int Packed, int Skipped, long TotalBytes);

public interface IIconPackService
{
    /// <summary>
    /// Packs icon images from <paramref name="sourceDirectory"/> into a SQLite blob store.
    /// If <paramref name="allowList"/> is non-null, only files whose stem (lowercased filename
    /// without extension) is in the set are packed. All blobs are re-encoded as PNG to compress
    /// uncompressed formats like BMP.
    /// </summary>
    Task<IconPackResult> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IReadOnlySet<string>? allowList,
        IProgress<IconPackProgress>? progress,
        CancellationToken cancellationToken);
}
