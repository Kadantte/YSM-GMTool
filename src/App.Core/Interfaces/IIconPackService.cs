namespace App.Core.Interfaces;

public sealed record IconPackProgress(int Current, int Total);

public interface IIconPackService
{
    Task<int> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IProgress<IconPackProgress>? progress,
        CancellationToken cancellationToken);
}
