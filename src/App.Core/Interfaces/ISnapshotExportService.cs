using App.Core.Enums;

namespace App.Core.Interfaces;

public sealed record SnapshotExportProgress(string EntityName, int Current, int Total);

public interface ISnapshotExportService
{
    Task ExportAsync(
        DatabaseProvider sourceProvider,
        string sourceConnectionString,
        IReadOnlyDictionary<string, string> queryTokens,
        string targetSnapshotPath,
        IProgress<SnapshotExportProgress>? progress,
        CancellationToken cancellationToken);
}
