using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Core.Models.Entities;
using ReactiveUI;

namespace App.Desktop.Features.Playerchecker;

/// <summary>View model for the pop-out inventory / warehouse window.</summary>
public sealed class InventoryViewModel : ReactiveObject
{
    public InventoryViewModel(string title, IReadOnlyList<InventoryItemRecord> items, bool iconsEnabled)
    {
        Title = title;
        IconsEnabled = iconsEnabled;
        Items = new ObservableCollection<InventoryItemRecord>(items);
    }

    public string Title { get; }

    /// <summary>When true the grid shows an item-icon column (icons scaled to the row).</summary>
    public bool IconsEnabled { get; }

    public ObservableCollection<InventoryItemRecord> Items { get; }
}
