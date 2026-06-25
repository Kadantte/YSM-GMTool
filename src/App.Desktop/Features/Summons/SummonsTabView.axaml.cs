using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Summons;

public partial class SummonsTabView : UserControl
{
    public SummonsTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
