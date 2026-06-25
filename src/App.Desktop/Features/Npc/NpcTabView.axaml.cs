using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Npc;

public partial class NpcTabView : UserControl
{
    public NpcTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
