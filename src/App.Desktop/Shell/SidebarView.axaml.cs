using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Shell;

public partial class SidebarView : UserControl
{
    public SidebarView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
