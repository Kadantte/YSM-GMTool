using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Items;

public partial class ItemsTabView : UserControl
{
    public ItemsTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
