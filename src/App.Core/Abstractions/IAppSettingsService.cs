using App.Core.Models;

namespace App.Core.Abstractions;

public interface IAppSettingsService
{
    Task<AppSettings> LoadAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(AppSettings settings, CancellationToken cancellationToken = default);
}
