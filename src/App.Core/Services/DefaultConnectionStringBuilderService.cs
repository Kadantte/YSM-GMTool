using System.Data.Common;
using App.Core.Enums;
using App.Core.Abstractions;
using App.Core.Models;

namespace App.Core.Services;

public sealed class DefaultConnectionStringBuilderService : IConnectionStringBuilderService
{
    public string Build(DatabaseProvider provider, DatabaseConnectionSettings settings)
        => provider switch
        {
            DatabaseProvider.MSSQL => BuildMsSql(settings),
            DatabaseProvider.MySQL => BuildMySql(settings),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unsupported database provider.")
        };

    public bool TryParse(DatabaseProvider provider, string connectionString, out DatabaseConnectionSettings settings)
    {
        settings = new DatabaseConnectionSettings();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return false;
        }

        try
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (provider == DatabaseProvider.MSSQL)
            {
                ParseMsSql(builder, settings);
            }
            else
            {
                ParseMySql(builder, settings);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string BuildMsSql(DatabaseConnectionSettings settings)
    {
        var server = settings.Port > 0 ? $"{settings.Server},{settings.Port}" : settings.Server;
        var auth = settings.IntegratedSecurity
            ? "Integrated Security=True;"
            : $"User Id={settings.UserId};Password={settings.Password};";

        return $"Server={server};Database={settings.Database};{auth}Encrypt={(settings.Encrypt ? "True" : "False")};TrustServerCertificate={(settings.TrustServerCertificate ? "True" : "False")};";
    }

    private static string BuildMySql(DatabaseConnectionSettings settings)
    {
        return $"Server={settings.Server};Port={settings.Port};Database={settings.Database};User Id={settings.UserId};Password={settings.Password};SslMode=None;";
    }

    private static void ParseMsSql(DbConnectionStringBuilder builder, DatabaseConnectionSettings settings)
    {
        settings.Server = GetString(builder, "Server", GetString(builder, "Data Source", settings.Server));
        settings.Database = GetString(builder, "Database", GetString(builder, "Initial Catalog", settings.Database));
        settings.UserId = GetString(builder, "User ID", GetString(builder, "UID", settings.UserId));
        settings.Password = GetString(builder, "Password", GetString(builder, "PWD", settings.Password));
        settings.IntegratedSecurity = GetBool(builder, "Integrated Security", GetBool(builder, "Trusted_Connection", settings.IntegratedSecurity));
        settings.Encrypt = GetBool(builder, "Encrypt", settings.Encrypt);
        settings.TrustServerCertificate = GetBool(builder, "TrustServerCertificate", settings.TrustServerCertificate);

        if (!string.IsNullOrWhiteSpace(settings.Server))
        {
            var split = settings.Server.Split(',', 2, StringSplitOptions.TrimEntries);
            settings.Server = split[0];
            if (split.Length > 1 && int.TryParse(split[1], out var port))
            {
                settings.Port = port;
            }
        }
    }

    private static void ParseMySql(DbConnectionStringBuilder builder, DatabaseConnectionSettings settings)
    {
        settings.Server = GetString(builder, "Server", settings.Server);
        settings.Database = GetString(builder, "Database", settings.Database);
        settings.UserId = GetString(builder, "User ID", GetString(builder, "Uid", GetString(builder, "User", settings.UserId)));
        settings.Password = GetString(builder, "Password", settings.Password);
        settings.Port = GetInt(builder, "Port", settings.Port > 0 ? settings.Port : 3306);

        if (settings.Port == 0)
        {
            settings.Port = 3306;
        }
    }

    private static string GetString(DbConnectionStringBuilder builder, string key, string fallback)
    {
        if (!builder.TryGetValue(key, out var value) || value is null)
        {
            return fallback;
        }

        var text = Convert.ToString(value)?.Trim();
        return string.IsNullOrWhiteSpace(text) ? fallback : text;
    }

    private static bool GetBool(DbConnectionStringBuilder builder, string key, bool fallback)
    {
        if (!builder.TryGetValue(key, out var value) || value is null)
        {
            return fallback;
        }

        if (value is bool direct)
        {
            return direct;
        }

        if (bool.TryParse(Convert.ToString(value), out var parsed))
        {
            return parsed;
        }

        return fallback;
    }

    private static int GetInt(DbConnectionStringBuilder builder, string key, int fallback)
    {
        if (!builder.TryGetValue(key, out var value) || value is null)
        {
            return fallback;
        }

        if (value is int i)
        {
            return i;
        }

        return int.TryParse(Convert.ToString(value), out var parsed) ? parsed : fallback;
    }
}
