using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Models;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Shell;

/// <summary>
/// Root view model for the shell window: the ordered tab list and the top command bar. On
/// construction it loads persisted settings, applies the <c>YSM_DB_*</c> environment overrides,
/// seeds the settings holder, and configures the icon cache.
/// </summary>
public sealed class ShellViewModel : ReactiveObject
{
    private ITabModule? _selectedTab;

    public ShellViewModel(
        IEnumerable<ITabModule> modules,
        TopBarViewModel topBar,
        IAppSettingsService settingsService,
        IAppSettingsHolder settingsHolder,
        IConnectionStringBuilderService connectionStringBuilder)
    {
        TopBar = topBar;
        Tabs = modules.OrderBy(m => m.Order).ToList();
        SelectedTab = Tabs.FirstOrDefault();

        InitializeSettings(settingsService, settingsHolder, connectionStringBuilder);
    }

    public IReadOnlyList<ITabModule> Tabs { get; }

    public ITabModule? SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    public TopBarViewModel TopBar { get; }

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
}
