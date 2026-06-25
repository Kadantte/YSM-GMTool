namespace App.Desktop.Modules;

/// <summary>
/// Contract implemented by every tab in the shell. Implementations are discovered by
/// reflection at composition time and ordered by <see cref="Order"/>.
/// </summary>
public interface ITabModule
{
    string Title { get; }

    /// <summary>FontAwesome key, e.g. "fa-solid fa-box".</summary>
    string IconKey { get; }

    int Order { get; }
}
