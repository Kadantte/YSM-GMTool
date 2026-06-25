using System.Globalization;
using System.Threading.Tasks;
using App.Core.Models.Entities;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Playerchecker;

public partial class PlayercheckerTabView : UserControl
{
    public PlayercheckerTabView()
    {
        InitializeComponent();
        InventoryGrid.DoubleTapped += OnInventoryDoubleTapped;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void OnInventoryDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (InventoryGrid.SelectedItem is not InventoryItemRecord item)
        {
            return;
        }

        var index = InventoryGrid.CurrentColumn?.DisplayIndex ?? -1;
        var value = index switch
        {
            0 => item.ItemId.ToString(CultureInfo.InvariantCulture),
            1 => item.ItemName,
            2 => item.Count.ToString(CultureInfo.InvariantCulture),
            3 => item.Level.ToString(CultureInfo.InvariantCulture),
            4 => item.Enhance.ToString(CultureInfo.InvariantCulture),
            5 => item.WearInfo.ToString(CultureInfo.InvariantCulture),
            _ => null,
        };

        if (!string.IsNullOrWhiteSpace(value))
        {
            _ = CopyToClipboardAsync(value!);
        }
    }

    private static async Task CopyToClipboardAsync(string text)
    {
        var top = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (top?.Clipboard is { } cb)
        {
            await cb.SetTextAsync(text);
        }
    }
}
