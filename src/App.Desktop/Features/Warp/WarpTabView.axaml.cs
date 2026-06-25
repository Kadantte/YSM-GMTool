using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Warp;

public partial class WarpTabView : UserControl
{
    public WarpTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
