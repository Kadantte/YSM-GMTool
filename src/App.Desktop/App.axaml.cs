using App.Desktop.Infrastructure;
using App.Desktop.Shell;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace App.Desktop;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Avalonia's Setup() (incl. UseReactiveUI registration) has finished against the mutable
        // collection-backed resolver; now point Splat at the built, immutable provider.
        Program.Services.UseMicrosoftDependencyResolver();

        DataTemplates.Add(new ReactiveViewLocator());
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Program.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
