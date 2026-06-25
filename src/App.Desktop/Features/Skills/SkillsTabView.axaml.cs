using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Desktop.Features.Skills;

public partial class SkillsTabView : UserControl
{
    public SkillsTabView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
