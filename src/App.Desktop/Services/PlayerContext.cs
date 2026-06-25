using System.Collections.ObjectModel;
using App.Core.Abstractions;
using Serilog;

namespace App.Desktop.Services;

/// <summary>
/// Backs the right-sidebar player selector. Seeded from the live settings; mutations are
/// reflected back into <see cref="IAppSettingsHolder.Current"/> and persisted via a debounced,
/// fire-and-forget save.
/// </summary>
public sealed class PlayerContext : IPlayerContext
{
    private const string Self = "self";

    private readonly IAppSettingsHolder _settings;
    private readonly IAppSettingsService _settingsService;
    private readonly ObservableCollection<string> _players = [];
    private readonly object _saveGate = new();

    private string? _selectedPlayer;
    private CancellationTokenSource? _saveCts;

    public PlayerContext(IAppSettingsHolder settings, IAppSettingsService settingsService)
    {
        _settings = settings;
        _settingsService = settingsService;
        Players = new ReadOnlyObservableCollection<string>(_players);
        Reseed();
        _settings.Changed += (_, _) => Reseed();
    }

    public ReadOnlyObservableCollection<string> Players { get; }

    public string? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            if (string.Equals(_selectedPlayer, value, StringComparison.Ordinal))
            {
                return;
            }

            _selectedPlayer = value;
            _settings.Current.SelectedPlayer = value;
            QueueSave();
        }
    }

    public string Resolve()
        => string.IsNullOrWhiteSpace(SelectedPlayer) ? Self : SelectedPlayer.Trim();

    public bool TryResolveRequired(out string playerName)
    {
        playerName = Resolve();
        return !string.Equals(playerName, Self, StringComparison.OrdinalIgnoreCase);
    }

    public void Add(string name)
    {
        var trimmed = name?.Trim();
        if (string.IsNullOrEmpty(trimmed) || _players.Contains(trimmed, StringComparer.Ordinal))
        {
            return;
        }

        _players.Add(trimmed);
        _settings.Current.Players = [.. _players];
        SelectedPlayer = trimmed;
        QueueSave();
    }

    public void RemoveSelected()
    {
        if (string.IsNullOrEmpty(_selectedPlayer))
        {
            return;
        }

        var index = _players.IndexOf(_selectedPlayer);
        if (index < 0)
        {
            return;
        }

        _players.RemoveAt(index);
        _settings.Current.Players = [.. _players];
        SelectedPlayer = _players.Count > 0 ? _players[Math.Min(index, _players.Count - 1)] : null;
        QueueSave();
    }

    private void Reseed()
    {
        var current = _settings.Current;

        _players.Clear();
        foreach (var player in current.Players ?? [])
        {
            if (!string.IsNullOrWhiteSpace(player))
            {
                _players.Add(player);
            }
        }

        _selectedPlayer = current.SelectedPlayer;
    }

    private void QueueSave()
    {
        CancellationToken token;
        lock (_saveGate)
        {
            _saveCts?.Cancel();
            _saveCts?.Dispose();
            _saveCts = new CancellationTokenSource();
            token = _saveCts.Token;
        }

        _ = SaveAfterDelayAsync(token);
    }

    private async Task SaveAfterDelayAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(400), token).ConfigureAwait(false);
            await _settingsService.SaveAsync(_settings.Current, token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Superseded by a newer save request.
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to persist player context settings.");
        }
    }
}
