using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Desktop.Services;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Desktop.Shell;

public partial class MainWindow : Window
{
    private bool _settingsFlushed;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = Program.Services.GetRequiredService<ShellViewModel>();
        Closing += OnClosing;
    }

    private async void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_settingsFlushed)
        {
            return;
        }

        // Cancel the close, persist settings asynchronously, then close for real (no sync-over-async).
        e.Cancel = true;

        try
        {
            var holder = Program.Services.GetRequiredService<IAppSettingsHolder>();
            var settingsService = Program.Services.GetRequiredService<IAppSettingsService>();
            await settingsService.SaveAsync(holder.Current, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to persist settings during shutdown.");
        }
        finally
        {
            _settingsFlushed = true;
            Close();
        }
    }
}
