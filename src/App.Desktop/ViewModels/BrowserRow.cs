namespace App.Desktop.ViewModels;

/// <summary>
/// A single displayed row. <see cref="Tag"/> carries the underlying entity record; <see cref="Values"/>
/// holds the per-column display values (in column order).
/// </summary>
public sealed class BrowserRow(object tag, params object?[] values)
{
    public object Tag { get; } = tag;

    public object?[] Values { get; } = values;

    public object? this[int i] => i >= 0 && i < Values.Length ? Values[i] : null;
}
