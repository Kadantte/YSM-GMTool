using System.Text.Json;
using App.Core.Abstractions;

namespace App.Core.Services;

public sealed class LocalCacheService : ILocalCacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly string _cacheDirectory;

    public LocalCacheService(string appDirectory)
    {
        _cacheDirectory = Path.Combine(appDirectory, "cache");
    }

    public async Task SaveAsync<T>(string key, IReadOnlyList<T> data, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_cacheDirectory);
        var path = GetPath(key);
        var json = JsonSerializer.Serialize(data, SerializerOptions);
        await File.WriteAllTextAsync(path, json, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var path = GetPath(key);
        if (!File.Exists(path))
        {
            return [];
        }

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        return JsonSerializer.Deserialize<List<T>>(json, SerializerOptions) ?? [];
    }

    public DateTime? GetCacheDate(string key)
    {
        var path = GetPath(key);
        return File.Exists(path) ? File.GetLastWriteTime(path) : null;
    }

    private string GetPath(string key) => Path.Combine(_cacheDirectory, $"{key}.json");
}
