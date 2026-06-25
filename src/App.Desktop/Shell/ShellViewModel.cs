using System.Collections.Generic;
using System.Linq;
using App.Desktop.Modules;
using ReactiveUI;

namespace App.Desktop.Shell;

/// <summary>
/// Root view model for the shell window: the ordered tab list and the top command bar. Settings are
/// loaded, seeded, and applied to the holder/icon cache synchronously in <see cref="Program"/> before
/// this view model (and the tab view models) are constructed.
/// </summary>
public sealed class ShellViewModel : ReactiveObject
{
    private ITabModule? _selectedTab;

    public ShellViewModel(
        IEnumerable<ITabModule> modules,
        TopBarViewModel topBar)
    {
        TopBar = topBar;
        Tabs = modules.OrderBy(m => m.Order).ToList();
        SelectedTab = Tabs.FirstOrDefault();
    }

    public IReadOnlyList<ITabModule> Tabs { get; }

    public ITabModule? SelectedTab
    {
        get => _selectedTab;
        set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
    }

    public TopBarViewModel TopBar { get; }
}
