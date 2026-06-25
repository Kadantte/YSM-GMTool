using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.About;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        var version = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            ?? "1.0.0";

        VersionText.Text = $"Version {version}";
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void OnOk(object? sender, RoutedEventArgs e) => Close();
}
