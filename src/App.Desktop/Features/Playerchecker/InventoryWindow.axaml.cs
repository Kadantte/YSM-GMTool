using System;
using System.Collections.Generic;
using System.Globalization;
using App.Core.Models.Entities;
using App.Desktop.Infrastructure;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace App.Desktop.Features.Playerchecker;

public partial class InventoryWindow : Window
{
    private const int IconSize = 36;

    // Copy accessor per visible column, aligned with the columns added in BuildColumns
    // (null = not copyable, e.g. the icon column).
    private readonly List<Func<InventoryItemRecord, string?>?> _copyAccessors = [];

    public InventoryWindow()
    {
        InitializeComponent();
        ItemsGrid.DoubleTapped += OnGridDoubleTapped;
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is not InventoryViewModel vm)
        {
            return;
        }

        BuildColumns(vm.IconsEnabled);
        ItemsGrid.RowHeight = vm.IconsEnabled ? IconSize + 6 : double.NaN;
    }

    private void BuildColumns(bool iconsEnabled)
    {
        ItemsGrid.Columns.Clear();
        _copyAccessors.Clear();

        if (iconsEnabled)
        {
            ItemsGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = "Icon",
                Width = new DataGridLength(44),
                CanUserSort = false,
                IsReadOnly = true,
                CellTemplate = new FuncDataTemplate<InventoryItemRecord>((item, _) =>
                {
                    var image = new Image { Width = IconSize, Height = IconSize, Stretch = Stretch.None };
                    image.Source = IconCache.Resolve(item?.IconFileName, IconSize) as Bitmap;
                    return image;
                }),
            });
            _copyAccessors.Add(null);
        }

        AddText("Item ID", "ItemId", 90, fill: false, x => x.ItemId.ToString(CultureInfo.InvariantCulture));
        AddText("Name", "ItemName", 160, fill: true, x => x.ItemName);
        AddText("Count", "Count", 80, fill: false, x => x.Count.ToString(CultureInfo.InvariantCulture));
        AddText("Level", "Level", 75, fill: false, x => x.Level.ToString(CultureInfo.InvariantCulture));
        AddText("Enhance", "Enhance", 95, fill: false, x => x.Enhance.ToString(CultureInfo.InvariantCulture));
        AddText("Wear", "WearInfo", 80, fill: false, x => x.WearInfo.ToString(CultureInfo.InvariantCulture));
    }

    private void AddText(string header, string property, int minWidth, bool fill, Func<InventoryItemRecord, string?> accessor)
    {
        ItemsGrid.Columns.Add(new DataGridTextColumn
        {
            Header = header,
            Binding = new Binding(property),
            Width = fill ? new DataGridLength(1, DataGridLengthUnitType.Star) : DataGridLength.Auto,
            MinWidth = minWidth,
            IsReadOnly = true,
        });
        _copyAccessors.Add(accessor);
    }

    private void OnGridDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (ItemsGrid.SelectedItem is not InventoryItemRecord item)
        {
            return;
        }

        var index = ItemsGrid.CurrentColumn?.DisplayIndex ?? -1;
        if (index < 0 || index >= _copyAccessors.Count)
        {
            return;
        }

        var value = _copyAccessors[index]?.Invoke(item);
        if (!string.IsNullOrWhiteSpace(value) && Clipboard is { } cb)
        {
            _ = cb.SetTextAsync(value!);
        }
    }
}
