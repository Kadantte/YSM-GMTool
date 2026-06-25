using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Desktop.Infrastructure;

public partial class DialogWindow : Window
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    public DialogWindow Configure(string title, string message, bool showCancel)
    {
        TitleText.Text = title;
        MessageText.Text = message;
        CancelButton.IsVisible = showCancel;
        return this;
    }

    private void OnOk(object? sender, RoutedEventArgs e) => Close(true);

    private void OnCancel(object? sender, RoutedEventArgs e) => Close(false);
}
