namespace App.Core.Interfaces;

public interface IIconEncoder
{
    /// <summary>
    /// Decodes <paramref name="sourceBytes"/> as an image and re-encodes it as PNG.
    /// Returns null if the source could not be decoded.
    /// </summary>
    byte[]? TryEncodeAsPng(byte[] sourceBytes);
}
