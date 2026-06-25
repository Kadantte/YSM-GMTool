namespace App.Core.Interfaces;

public interface IIconSource
{
    /// <summary>Returns raw image bytes for the given icon key, or null if not found.</summary>
    byte[]? TryGetIconBytes(string fileName);
}
