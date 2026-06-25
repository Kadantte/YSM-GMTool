using System.Collections.Generic;
using App.Core.Models.Entities;
using App.Desktop.Features.Playerchecker;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace App.Desktop.Infrastructure;

/// <summary>Shows a non-modal, owner-attached inventory/warehouse window.</summary>
public sealed class InventoryWindowService : IInventoryWindowService
{
    public void Show(string title, IReadOnlyList<InventoryItemRecord> items, bool iconsEnabled)
    {
        var window = new InventoryWindow
        {
            DataContext = new InventoryViewModel(title, items, iconsEnabled),
        };

        var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (owner is not null)
        {
            window.Show(owner);
        }
        else
        {
            window.Show();
        }
    }
}
