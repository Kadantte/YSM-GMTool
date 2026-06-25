using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Core.Abstractions;
using App.Desktop.Infrastructure;
using App.Desktop.Services;
using Avalonia.Threading;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Shell;

/// <summary>
/// Right-sidebar view model: target-player management, the "append /run" toggle, and the live
/// command history. Wraps <see cref="IPlayerContext"/>, the settings holder + persistence, and
/// <see cref="ICommandHistoryService"/>.
/// </summary>
public sealed class SidebarViewModel : ReactiveObject
{
    private readonly IPlayerContext _player;
    private readonly IAppSettingsHolder _settings;
    private readonly IAppSettingsService _settingsService;
    private readonly ICommandHistoryService _history;
    private readonly IClipboardService _clipboard;
    private readonly ObservableCollection<string> _historyItems = [];

    private string _newPlayerName = string.Empty;
    private bool _appendRun;
    private string? _selectedCommand;

    public SidebarViewModel(
        IPlayerContext player,
        IAppSettingsHolder settings,
        IAppSettingsService settingsService,
        ICommandHistoryService history,
        IClipboardService clipboard)
    {
        _player = player;
        _settings = settings;
        _settingsService = settingsService;
        _history = history;
        _clipboard = clipboard;

        _appendRun = settings.Current.AppendGeneratedCommands;
        History = new ReadOnlyObservableCollection<string>(_historyItems);

        SyncHistory();
        _history.CommandsChanged += OnHistoryChanged;
        _settings.Changed += OnSettingsChanged;

        AddPlayer = ReactiveCommand.Create(() =>
        {
            _player.Add(NewPlayerName);
            NewPlayerName = string.Empty;
        });

        RemovePlayer = ReactiveCommand.Create(() => _player.RemoveSelected());

        var hasSelection = this.WhenAnyValue(x => x.SelectedCommand)
            .Select(static c => !string.IsNullOrWhiteSpace(c));
        CopySelected = ReactiveCommand.CreateFromTask(
            async () =>
            {
                if (!string.IsNullOrWhiteSpace(SelectedCommand))
                {
                    await _clipboard.SetTextAsync(SelectedCommand);
                }
            },
            hasSelection);

        var hasHistory = Observable
            .FromEventPattern(h => _history.CommandsChanged += h, h => _history.CommandsChanged -= h)
            .Select(_ => _history.Commands.Count > 0)
            .StartWith(_history.Commands.Count > 0);
        CopyAll = ReactiveCommand.CreateFromTask(
            () => _clipboard.SetTextAsync(string.Join(Environment.NewLine, _history.Commands)),
            hasHistory);
        ClearHistory = ReactiveCommand.Create(() => _history.Clear(), hasHistory);

        AddPlayer.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Add player failed."));
        RemovePlayer.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Remove player failed."));
        CopySelected.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Copy selected command failed."));
        CopyAll.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Copy all commands failed."));
    }

    public ReadOnlyObservableCollection<string> Players => _player.Players;

    public string? SelectedPlayer
    {
        get => _player.SelectedPlayer;
        set
        {
            if (!string.Equals(_player.SelectedPlayer, value, StringComparison.Ordinal))
            {
                _player.SelectedPlayer = value;
                this.RaisePropertyChanged();
            }
        }
    }

    public string NewPlayerName
    {
        get => _newPlayerName;
        set => this.RaiseAndSetIfChanged(ref _newPlayerName, value);
    }

    /// <summary>Two-way bound to <c>AppSettings.AppendGeneratedCommands</c> via the holder; persisted on change.</summary>
    public bool AppendRun
    {
        get => _appendRun;
        set
        {
            if (_appendRun == value)
            {
                return;
            }

            this.RaiseAndSetIfChanged(ref _appendRun, value);
            _settings.Current.AppendGeneratedCommands = value;
            QueueSave();
        }
    }

    public ReadOnlyObservableCollection<string> History { get; }

    public string? SelectedCommand
    {
        get => _selectedCommand;
        set => this.RaiseAndSetIfChanged(ref _selectedCommand, value);
    }

    public ReactiveCommand<Unit, Unit> AddPlayer { get; }

    public ReactiveCommand<Unit, Unit> RemovePlayer { get; }

    public ReactiveCommand<Unit, Unit> CopySelected { get; }

    public ReactiveCommand<Unit, Unit> CopyAll { get; }

    public ReactiveCommand<Unit, Unit> ClearHistory { get; }

    private void OnHistoryChanged(object? sender, EventArgs e)
        => Dispatcher.UIThread.Post(SyncHistory);

    private void OnSettingsChanged(object? sender, EventArgs e)
        => Dispatcher.UIThread.Post(() =>
        {
            this.RaiseAndSetIfChanged(ref _appendRun, _settings.Current.AppendGeneratedCommands, nameof(AppendRun));
            this.RaisePropertyChanged(nameof(SelectedPlayer));
        });

    private void SyncHistory()
    {
        _historyItems.Clear();
        foreach (var command in _history.Commands)
        {
            _historyItems.Add(command);
        }
    }

    private void QueueSave()
        => _ = SaveAsync();

    private async System.Threading.Tasks.Task SaveAsync()
    {
        try
        {
            await _settingsService.SaveAsync(_settings.Current).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to persist append-commands toggle.");
        }
    }
}
