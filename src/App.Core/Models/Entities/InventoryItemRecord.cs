namespace App.Core.Models.Entities;

public sealed class InventoryItemRecord
{
    public int ItemId { get; init; }

    public string ItemName { get; init; } = string.Empty;

    public int Count { get; init; }

    public int Level { get; init; }

    public int Enhance { get; init; }

    public int WearInfo { get; init; }

    public int AwakenSid { get; init; }

    public int RandomOptionSid { get; init; }

    public int SummonCode { get; init; }

    public string? IconFileName { get; init; }
}
