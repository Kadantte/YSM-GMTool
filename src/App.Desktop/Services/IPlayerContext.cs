using System.Collections.ObjectModel;

namespace App.Desktop.Services;

/// <summary>Target-player selection plus the saved player list (persisted via settings).</summary>
public interface IPlayerContext
{
    ReadOnlyObservableCollection<string> Players { get; }

    /// <summary>Selected target player; null/empty means "self".</summary>
    string? SelectedPlayer { get; set; }

    /// <summary><see cref="SelectedPlayer"/> trimmed, otherwise "self".</summary>
    string Resolve();

    /// <summary>Returns false when the resolved player is "self".</summary>
    bool TryResolveRequired(out string playerName);

    void Add(string name);

    void RemoveSelected();
}
