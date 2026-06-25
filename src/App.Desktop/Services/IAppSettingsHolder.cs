using App.Core.Models;

namespace App.Desktop.Services;

/// <summary>Single source of truth for the live application settings.</summary>
public interface IAppSettingsHolder
{
    AppSettings Current { get; }

    event EventHandler? Changed;

    /// <summary>Replaces <see cref="Current"/> and raises <see cref="Changed"/>.</summary>
    void Set(AppSettings settings);
}
