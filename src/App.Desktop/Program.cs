using App.Desktop.Composition;
using App.Desktop.Infrastructure;
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
