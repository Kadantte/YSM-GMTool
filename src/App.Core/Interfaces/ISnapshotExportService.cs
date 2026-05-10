using App.Core.Enums;

namespace App.Core.Interfaces;

public sealed record SnapshotExportProgress(string EntityName, int Current, int Total);

public sealed record SnapshotExportResult(IReadOnlyList<(string EntityName, int RowCount)> EntityCounts);

public interface ISnapshotExportService
{
    Task<SnapshotExportResult> ExportAsync(
        DatabaseProvider sourceProvider,
        string sourceConnectionString,
        IReadOnlyDictionary<string, string> queryTokens,
        string targetSnapshotPath,
        IProgress<SnapshotExportProgress>? progress,
        CancellationToken cancellationToken);
}
