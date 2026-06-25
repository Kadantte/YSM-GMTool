using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Models;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Shell;

/// <summary>
/// Root view model for the shell window: the ordered tab list, the right sidebar, and the
/// Settings/About commands. On construction it loads persisted settings, applies the
/// <c>YSM_DB_*</c> environment overrides, seeds the settings holder, and configures the icon cache.
/// </summary>
public sealed class ShellViewModel : ReactiveObject
{
    private readonly IDialogService _dialog;
    private ITabModule? _selectedTab;

    public ShellViewModel(
        IEnumerable<ITabModule> modules,
        SidebarViewModel sidebar,
        IAppSettingsService settingsService,
        IAppSettingsHolder settingsHolder,
        IConnectionStringBuilderService connectionStringBuilder,
        IDialogService dialog)
    {
        _dialog = dialog;
        Sidebar = sidebar;
        Tabs = modules.OrderBy(m => m.Order).ToList();
        SelectedTab = Tabs.FirstOrDefault();

        OpenSettings = ReactiveCommand.CreateFromTask(() => OpenWindowAsync("App.Desktop.Features.Settings.SettingsWindow"));
        OpenAbout = ReactiveCommand.CreateFromTask(() => OpenWindowAsync("App.Desktop.Features.About.AboutWindow"));
        OpenSettings.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Open settings failed."));
        OpenAbout.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Open about failed."));

        InitializeSettings(settingsService, settingsHolder, connectionStringBuilder);
    }

    public IReadOnlyList<ITabModule> Tabs { get; }

    public ITabModule? SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    public SidebarViewModel Sidebar { get; }

    public ReactiveCommand<Unit, Unit> OpenSettings { get; }

    public ReactiveCommand<Unit, Unit> OpenAbout { get; }

    private void InitializeSettings(
        IAppSettingsService settingsService,
        IAppSettingsHolder holder,
        IConnectionStringBuilderService connectionStringBuilder)
    {
        AppSettingsLoaded(settingsService, holder, connectionStringBuilder).ContinueWith(
            t =>
            {
                if (t.Exception is { } ex)
                {
                    Log.Error(ex, "Failed to initialize application settings.");
                }
            },
            TaskScheduler.Default);
    }

    private static async Task AppSettingsLoaded(
        IAppSettingsService settingsService,
        IAppSettingsHolder holder,
        IConnectionStringBuilderService connectionStringBuilder)
    {
        var settings = await settingsService.LoadAsync().ConfigureAwait(false);

        EnsureDefaults(settings);
        ApplyEnvironmentDefaults(settings, connectionStringBuilder);

        holder.Set(settings);
        IconCache.Configure(settings.EnableEntityIcons, settings.EntityIconsPath);
    }

    private static void EnsureDefaults(AppSettings settings)
    {
        settings.Connection ??= new();
        settings.TableNames ??= new();
        settings.Players ??= [];
        settings.WarpLocations ??= [];
        settings.EntityIconsPath ??= string.Empty;
    }

    private static void ApplyEnvironmentDefaults(
        AppSettings settings,
        IConnectionStringBuilderService connectionStringBuilder)
    {
        var providerRaw = Environment.GetEnvironmentVariable(DotEnv.DbProviderEnvKey);
        if (!string.IsNullOrWhiteSpace(providerRaw)
            && Enum.TryParse<DatabaseProvider>(providerRaw, ignoreCase: true, out var provider))
        {
            settings.Provider = provider;
        }

        var connectionString = Environment.GetEnvironmentVariable(DotEnv.DbConnectionStringEnvKey);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            settings.ConnectionString = connectionString.Trim();
            if (connectionStringBuilder.TryParse(settings.Provider, settings.ConnectionString, out var parsed))
            {
                settings.Connection = parsed;
            }
        }
    }

    /// <summary>
    /// Opens a window resolved by full type name. The Settings/About windows are added in Phase 10;
    /// resolving by name keeps the shell decoupled from those types. Until they exist, this is a no-op
    /// surfaced to the user as an info dialog.
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
