using App.Desktop.Infrastructure;
using App.Desktop.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace App.Desktop.Controls;

/// <summary>
/// Generic browser view: a data grid (left) + a search/actions panel (right). Columns are built
/// from the bound <see cref="IEntityBrowser.Columns"/>; the per-tab action panel is supplied via
/// the <see cref="Actions"/> property.
/// </summary>
public partial class EntityBrowserView : UserControl
{
    /// <summary>Per-tab action panel hosted under the search controls.</summary>
    public static readonly StyledProperty<object?> ActionsProperty =
        AvaloniaProperty.Register<EntityBrowserView, object?>(nameof(Actions));

    private IEntityBrowser? _browser;

    public EntityBrowserView()
    {
        InitializeComponent();

        RecordsGrid.DoubleTapped += OnGridDoubleTapped;

        ByIdRadio.IsCheckedChanged += (_, _) => UpdateSearchMode();
        ByNameRadio.IsCheckedChanged += (_, _) => UpdateSearchMode();
        ByContactRadio.IsCheckedChanged += (_, _) => UpdateSearchMode();

        DataContextChanged += OnDataContextChanged;
    }

    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ActionsProperty)
        {
            ActionsHost.Content = change.NewValue;
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        _browser = DataContext as IEntityBrowser;
        if (_browser is null)
        {
            return;
        }

        BuildColumns(_browser.Columns);
        SyncSearchModeRadios(_browser.SearchMode);
    }

    private void BuildColumns(IReadOnlyList<BrowserColumn> columns)
    {
        RecordsGrid.Columns.Clear();

        for (var i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            var index = i;

            if (column.IsImage)
            {
                RecordsGrid.Columns.Add(BuildImageColumn(column, index));
            }
            else
            {
                RecordsGrid.Columns.Add(BuildTextColumn(column, index));
            }
        }
    }

    private DataGridColumn BuildTextColumn(BrowserColumn column, int index)
    {
        // Custom sort: CanUserSortColumns is off, so a tappable header TextBlock drives sorting.
        var header = new TextBlock { Text = column.Header };
        header.Tapped += (_, _) => _browser?.SortByColumn(index);

        return new DataGridTextColumn
        {
            Header = header,
            Binding = new Binding($"[{index}]"),
            Width = column.Fill
                ? new DataGridLength(Math.Max(1, column.Width), DataGridLengthUnitType.Star)
                : new DataGridLength(column.Width),
            IsReadOnly = true,
        };
    }

    private DataGridColumn BuildImageColumn(BrowserColumn column, int index)
    {
        var imageColumn = new DataGridTemplateColumn
        {
            Header = column.Header,
            Width = new DataGridLength(column.Width),
            IsReadOnly = true,
            CellTemplate = new FuncDataTemplate<BrowserRow>((row, _) =>
            {
                var image = new Image
                {
                    Width = column.ImageSize,
                    Height = column.ImageSize,
                    Stretch = Avalonia.Media.Stretch.None,
                };

                var key = row?[index] as string ?? row?[index]?.ToString();
                image.Source = IconCache.Resolve(key, column.ImageSize) as Bitmap;
                return image;
            }),
        };

        return imageColumn;
    }

    private void OnGridDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (RecordsGrid.SelectedItem is not BrowserRow row || _browser is null)
        {
            return;
        }

        var index = RecordsGrid.CurrentColumn?.DisplayIndex ?? -1;
        if (index < 0 || index >= _browser.Columns.Count || _browser.Columns[index].IsImage)
        {
            return;
        }

        var value = row[index]?.ToString();
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

    private void UpdateSearchMode()
    {
        if (_browser is null)
        {
            return;
        }

        if (ByIdRadio.IsChecked == true)
        {
            _browser.SearchMode = SearchMode.ById;
        }
        else if (ByContactRadio.IsChecked == true)
        {
            _browser.SearchMode = SearchMode.ByContactScript;
        }
        else
        {
            _browser.SearchMode = SearchMode.ByName;
        }
    }

    private void SyncSearchModeRadios(SearchMode mode)
    {
        switch (mode)
        {
            case SearchMode.ById:
                ByIdRadio.IsChecked = true;
                break;
            case SearchMode.ByContactScript:
                ByContactRadio.IsChecked = true;
                break;
            default:
                ByNameRadio.IsChecked = true;
                break;
        }
    }
}
