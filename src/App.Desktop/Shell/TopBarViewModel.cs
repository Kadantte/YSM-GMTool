using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Desktop.Infrastructure;
using App.Desktop.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Shell;

/// <summary>
/// Top command-bar view model: target-player management, the "append /run" toggle, and the
/// Settings/About commands. Wraps <see cref="IPlayerContext"/> and the settings holder + persistence.
/// </summary>
public sealed class TopBarViewModel : ReactiveObject
{
    private readonly IPlayerContext _player;
    private readonly IAppSettingsHolder _settings;
    private readonly IAppSettingsService _settingsService;
    private readonly IDialogService _dialog;

    private string _newPlayerName = string.Empty;
    private bool _appendRun;

    public TopBarViewModel(
        IPlayerContext player,
        IAppSettingsHolder settings,
        IAppSettingsService settingsService,
        IDialogService dialog)
    {
        _player = player;
        _settings = settings;
        _settingsService = settingsService;
        _dialog = dialog;

        _appendRun = settings.Current.AppendGeneratedCommands;
        _settings.Changed += OnSettingsChanged;

        AddPlayer = ReactiveCommand.Create(() =>
        {
            _player.Add(NewPlayerName);
            NewPlayerName = string.Empty;
        });

        RemovePlayer = ReactiveCommand.Create(() => _player.RemoveSelected());

        OpenSettings = ReactiveCommand.CreateFromTask(() => OpenWindowAsync("App.Desktop.Features.Settings.SettingsWindow"));
        OpenAbout = ReactiveCommand.CreateFromTask(() => OpenWindowAsync("App.Desktop.Features.About.AboutWindow"));

        AddPlayer.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Add player failed."));
        RemovePlayer.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Remove player failed."));
        OpenSettings.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Open settings failed."));
        OpenAbout.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Open about failed."));
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

    public ReactiveCommand<Unit, Unit> AddPlayer { get; }

    public ReactiveCommand<Unit, Unit> RemovePlayer { get; }

    public ReactiveCommand<Unit, Unit> OpenSettings { get; }

    public ReactiveCommand<Unit, Unit> OpenAbout { get; }

    private void OnSettingsChanged(object? sender, EventArgs e)
        => Dispatcher.UIThread.Post(() =>
        {
            this.RaiseAndSetIfChanged(ref _appendRun, _settings.Current.AppendGeneratedCommands, nameof(AppendRun));
            this.RaisePropertyChanged(nameof(SelectedPlayer));
        });

    private void QueueSave()
        => _ = SaveAsync();

    private async Task SaveAsync()
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

    /// <summary>
    /// Opens a window resolved by full type name. Resolving by name keeps the bar decoupled from those
    /// types; if the type is missing it surfaces as an info dialog.
    /// </summary>
    private async Task OpenWindowAsync(string windowTypeName)
    {
        var windowType = Type.GetType(windowTypeName);
        if (windowType is null)
        {
            await _dialog.ShowInfoAsync("GM Tool", "This window is not available yet.");
            return;
        }

        var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        var window = (Window)(Program.Services.GetService(windowType) ?? Activator.CreateInstance(windowType)!);

        if (owner is not null)
        {
            await window.ShowDialog(owner);
        }
        else
        {
            window.Show();
        }
    }
}
