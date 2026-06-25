using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace App.Desktop.Infrastructure;

public sealed class DialogService : IDialogService
{
    public Task ShowInfoAsync(string title, string message) => ShowAsync(title, message, showCancel: false);

    public Task ShowWarningAsync(string title, string message) => ShowAsync(title, message, showCancel: false);

    public Task ShowErrorAsync(string title, string message) => ShowAsync(title, message, showCancel: false);

    public async Task<bool> ConfirmAsync(string title, string message)
        => await ShowAsync(title, message, showCancel: true).ConfigureAwait(true) ?? false;

    private static Task<bool?> ShowAsync(string title, string message, bool showCancel)
    {
        return Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            var dialog = new DialogWindow().Configure(title, message, showCancel);

            if (owner is not null)
            {
                return await dialog.ShowDialog<bool?>(owner);
            }

            dialog.Show();
            return (bool?)false;
        });
    }
}
