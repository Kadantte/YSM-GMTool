namespace App.Core.Abstractions;

public interface ILocalCacheService
{
    Task SaveAsync<T>(string key, IReadOnlyList<T> data, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> LoadAsync<T>(string key, CancellationToken cancellationToken = default);

    DateTime? GetCacheDate(string key);
}
