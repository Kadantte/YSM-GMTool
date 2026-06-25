using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace App.Desktop.Infrastructure;

/// <summary>Resolves a <c>*ViewModel</c> to its matching <c>*View</c> via DI.</summary>
public sealed class ReactiveViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        var vmName = data!.GetType().FullName!;
        var viewName = vmName.Replace("ViewModel", "View", StringComparison.Ordinal);
        var viewType = Type.GetType(viewName);
        if (viewType is null)
        {
            return new TextBlock { Text = "View not found: " + viewName };
        }

        return (Control)(Program.Services.GetService(viewType) ?? Activator.CreateInstance(viewType)!);
    }

    public bool Match(object? data) => data is ReactiveUI.IReactiveObject;
}
