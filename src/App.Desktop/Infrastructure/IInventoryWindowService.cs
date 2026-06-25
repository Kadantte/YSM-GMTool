using System.Collections.Generic;
using App.Core.Models.Entities;

namespace App.Desktop.Infrastructure;

/// <summary>Opens the inventory / warehouse results in a separate pop-out window.</summary>
public interface IInventoryWindowService
{
    void Show(string title, IReadOnlyList<InventoryItemRecord> items, bool iconsEnabled);
}
