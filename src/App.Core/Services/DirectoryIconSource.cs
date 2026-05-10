using App.Core.Interfaces;

namespace App.Core.Services;

public sealed class DirectoryIconSource : IIconSource
{
    private static readonly string[] IconExtensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];

    private readonly string? _baseDirectory;

    public DirectoryIconSource(string? baseDirectory) => _baseDirectory = baseDirectory;

    public byte[]? TryGetIconBytes(string fileName)
    {
        if (string.IsNullOrWhiteSpace(_baseDirectory) || !Directory.Exists(_baseDirectory) || string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        if (Path.IsPathRooted(fileName))
        {
            return File.Exists(fileName) ? SafeRead(fileName) : null;
        }

        if (Path.HasExtension(fileName))
        {
            var direct = Path.Combine(_baseDirectory, fileName);
            return File.Exists(direct) ? SafeRead(direct) : null;
        }

        foreach (var extension in IconExtensions)
        {
            var candidate = Path.Combine(_baseDirectory, fileName + extension);
            if (File.Exists(candidate))
            {
                return SafeRead(candidate);
            }
        }

        return null;
    }

    private static byte[]? SafeRead(string path)
    {
        try
        {
            return File.ReadAllBytes(path);
        }
        catch
        {
            return null;
        }
    }
}
