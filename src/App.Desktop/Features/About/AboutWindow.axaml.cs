using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;

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

    private void OnOk(object? sender, RoutedEventArgs e) => Close();
}
