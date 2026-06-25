using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Buffs;

public partial class BuffsTabView : UserControl
{
    public BuffsTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
