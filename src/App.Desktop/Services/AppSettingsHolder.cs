using App.Core.Models;

namespace App.Desktop.Services;

public sealed class AppSettingsHolder : IAppSettingsHolder
{
    public AppSettings Current { get; private set; } = new();

    public event EventHandler? Changed;

    public void Set(AppSettings settings)
    {
        Current = settings ?? new AppSettings();
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
