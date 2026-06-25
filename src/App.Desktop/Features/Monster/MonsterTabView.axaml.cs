using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Monster;

public partial class MonsterTabView : UserControl
{
    public MonsterTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
