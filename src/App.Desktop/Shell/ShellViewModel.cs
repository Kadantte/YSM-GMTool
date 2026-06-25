using System.Collections.Generic;
using System.Linq;
using App.Desktop.Modules;
using ReactiveUI;

namespace App.Desktop.Shell;

/// <summary>
/// Root view model for the shell window. Phase 3 wires the tab list only; the sidebar,
/// settings/about commands, and settings bootstrap are added in Phase 5/10.
/// </summary>
public sealed class ShellViewModel : ReactiveObject
{
    private ITabModule? _selectedTab;

    public ShellViewModel(IEnumerable<ITabModule> modules)
    {
        Tabs = modules.OrderBy(m => m.Order).ToList();
        SelectedTab = Tabs.FirstOrDefault();
    }

    public IReadOnlyList<ITabModule> Tabs { get; }

    public ITabModule? SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }
}
