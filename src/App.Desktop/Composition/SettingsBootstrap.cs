using System;
using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Models;
using App.Desktop.Infrastructure;

namespace App.Desktop.Composition;

/// <summary>
/// Seeds an <see cref="AppSettings"/> instance after it is loaded from disk: fills in null
/// collections/objects with defaults and applies the <c>YSM_DB_*</c> environment overrides. Used by
/// <see cref="Program"/> to prepare settings synchronously before the shell and tab view models are
/// constructed (so the icon cache and row-height state are correct at construction time).
/// </summary>
internal static class SettingsBootstrap
{
    public static void EnsureDefaults(AppSettings settings)
    {
        settings.Connection ??= new();
        settings.TableNames ??= new();
        settings.Players ??= [];
        settings.WarpLocations ??= [];
        settings.EntityIconsPath ??= string.Empty;
    }

    public static void ApplyEnvironmentDefaults(
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
