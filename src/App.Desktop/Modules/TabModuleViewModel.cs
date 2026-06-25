using ReactiveUI;

namespace App.Desktop.Modules;

/// <summary>Base view model for shell tabs.</summary>
public abstract class TabModuleViewModel : ReactiveObject, ITabModule
{
    public abstract string Title { get; }

    public virtual string IconKey => string.Empty;

    public abstract int Order { get; }
}
