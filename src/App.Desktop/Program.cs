using App.Core.Abstractions;
using App.Desktop.Composition;
using App.Desktop.Infrastructure;
using App.Desktop.Services;
using Avalonia;
using Avalonia.ReactiveUI;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;
using ReactiveUI;
using Serilog;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace App.Desktop;

internal static class Program
{
    public static IServiceProvider Services { get; private set; } = default!;

    [STAThread]
    public static void Main(string[] args)
    {
        var appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YSM-GMTool");
        var logsDir = Path.Combine(appDir, "logs");
        Directory.CreateDirectory(logsDir);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logsDir, "gmtool-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Map snake_case/SCREAMING SQL aliases -> PascalCase record props (explicit, before any query).
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        DotEnv.LoadIfPresent(appDir);

        // Bridge MS.DI -> Splat for ReactiveUI view location. The resolver stays backed by the
        // mutable collection through Avalonia's Setup() (so UseReactiveUI() can register into it);
        // it is switched to the built provider in App.OnFrameworkInitializationCompleted.
        var collection = new ServiceCollection().AddGmTool(appDir);
        collection.UseMicrosoftDependencyResolver();
        var resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();
        Services = collection.BuildServiceProvider();

        // Load + seed settings synchronously BEFORE any view model is constructed. The shell and tab
        // VMs resolve when the main window is built (in App.OnFrameworkInitializationCompleted), so the
        // settings holder and icon cache must already be populated; otherwise icon-state and row-height
        // snapshots taken at VM construction would be wrong (see #3 icon/column misalignment).
        var holder = Services.GetRequiredService<IAppSettingsHolder>();
        var settingsService = Services.GetRequiredService<IAppSettingsService>();
        var connectionStringBuilder = Services.GetRequiredService<IConnectionStringBuilderService>();
        var settings = settingsService.LoadAsync().GetAwaiter().GetResult();
        SettingsBootstrap.EnsureDefaults(settings);
        SettingsBootstrap.ApplyEnvironmentDefaults(settings, connectionStringBuilder);
        holder.Set(settings);
        IconCache.Configure(settings.EnableEntityIcons, settings.EntityIconsPath);

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current.Register<FontAwesomeIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }
}
