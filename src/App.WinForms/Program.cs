using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Services;
using App.Data.Infrastructure;
using App.Data.Repositories;
using App.Data.Services;
using Serilog;

namespace App.WinForms;

internal static class Program
{
    private const string DbProviderEnvKey = "YSM_DB_PROVIDER";
    private const string DbConnectionStringEnvKey = "YSM_DB_CONNECTION_STRING";

    [STAThread]
    private static void Main()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appDirectory = Path.Combine(localAppData, "YSM-GMTool");
        var logsDirectory = Path.Combine(appDirectory, "logs");
        Directory.CreateDirectory(logsDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logsDirectory, "gmtool-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            LoadDotEnvIfPresent(appDirectory);
            ApplicationConfiguration.Initialize();
            ApplyNativeDarkMode();
            RegisterUnhandledExceptionHandlers();

            var snapshotPath = Path.Combine(AppContext.BaseDirectory, "gmtool-snapshot.db");
            var iconsDbPath  = Path.Combine(AppContext.BaseDirectory, "gmtool-icons.db");
            var mode = File.Exists(snapshotPath) ? AppMode.OfflineSnapshot : AppMode.LiveDb;
            Log.Information("App started in {Mode} mode", mode);

            IIconSource? offlineIconSource = mode == AppMode.OfflineSnapshot && File.Exists(iconsDbPath)
                ? new SqliteIconSource(iconsDbPath)
                : null;

            var queryFile = Path.Combine(AppContext.BaseDirectory, "Config", "queries.json");
            var luaFile = Path.Combine(AppContext.BaseDirectory, "Config", "lua_commands.json");
            var settingsFile = Path.Combine(appDirectory, "settings.json");

            IQueryStore queryStore = new FileQueryStore(queryFile);
            ILuaCommandTemplateStore templateStore = new FileLuaCommandTemplateStore(luaFile);
            IAppSettingsService settingsService = new JsonAppSettingsService(settingsFile);
            INameNormalizer normalizer = new SearchNameNormalizer();
            IConnectionStringBuilderService connectionStringBuilder = new DefaultConnectionStringBuilderService();
            ILuaCommandBuilder commandBuilder = new LuaCommandBuilder(templateStore);
            ICommandHistoryService commandHistory = new CommandHistoryService();
            IGameDataRepository repository = new GameDataRepository(queryStore, new DbConnectionFactory());
            ILocalCacheService localCache = new LocalCacheService(appDirectory);
            ISnapshotExportService exportService = new SnapshotExportService(repository);
            IIconPackService iconPackService = new IconPackService();

            Application.Run(new MainForm(
                repository,
                settingsService,
                connectionStringBuilder,
                commandBuilder,
                commandHistory,
                normalizer,
                localCache,
                mode,
                snapshotPath,
                offlineIconSource,
                exportService,
                iconPackService));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            MessageBox.Show($"Fatal error:{Environment.NewLine}{ex.Message}", "GM Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void RegisterUnhandledExceptionHandlers()
    {
        Application.ThreadException += (_, args) =>
        {
            Log.Error(args.Exception, "Unhandled UI thread exception.");
            MessageBox.Show($"Unhandled error:{Environment.NewLine}{args.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
                Log.Fatal(ex, "Unhandled non-UI exception.");
            }
        };

        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            Log.Error(args.Exception, "Unobserved task exception.");
            args.SetObserved();
        };
    }

    private static void ApplyNativeDarkMode()
    {
#if NET10_0_OR_GREATER
        // Variant A (.NET 10 GA/current SDK): official built-in dark mode API.
        Application.SetColorMode(SystemColorMode.Dark);
#else
        // Variant B (older previews): no stable SetColorMode API.
#endif
    }

    private static void LoadDotEnvIfPresent(string appDirectory)
    {
        var envPath = FindDotEnvPath(appDirectory);
        if (string.IsNullOrWhiteSpace(envPath) || !File.Exists(envPath))
        {
            return;
        }

        foreach (var rawLine in File.ReadLines(envPath))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            if (value.StartsWith('\"') && value.EndsWith('\"') && value.Length >= 2)
            {
                value = value[1..^1];
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            Environment.SetEnvironmentVariable(key, value);
        }

        // Keep explicit keys visible in logs/debugging context.
        Log.Information(
            ".env loaded. {ProviderKey} set: {ProviderSet}, {ConnectionKey} set: {ConnectionSet}",
            DbProviderEnvKey,
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(DbProviderEnvKey)),
            DbConnectionStringEnvKey,
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(DbConnectionStringEnvKey)));
    }

    private static string? FindDotEnvPath(string appDirectory)
    {
        var directCandidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, ".env"),
            Path.Combine(Directory.GetCurrentDirectory(), ".env"),
            Path.Combine(appDirectory, ".env")
        };

        foreach (var candidate in directCandidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        var searchRoots = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory,
            appDirectory
        };

        foreach (var root in searchRoots.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var directory = new DirectoryInfo(root);
            while (directory is not null)
            {
                var candidate = Path.Combine(directory.FullName, ".env");
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }
}
