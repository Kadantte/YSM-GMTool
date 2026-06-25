using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop.Shell;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = Program.Services.GetRequiredService<ShellViewModel>();
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
