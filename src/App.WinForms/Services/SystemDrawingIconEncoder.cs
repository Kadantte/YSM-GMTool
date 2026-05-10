using System.Drawing;
using System.Drawing.Imaging;
using App.Core.Interfaces;

namespace App.WinForms.Services;

public sealed class SystemDrawingIconEncoder : IIconEncoder
{
    public byte[]? TryEncodeAsPng(byte[] sourceBytes)
    {
        try
        {
            using var input = new MemoryStream(sourceBytes);
            using var image = Image.FromStream(input);
            using var output = new MemoryStream();
            image.Save(output, ImageFormat.Png);
            return output.ToArray();
        }
        catch
        {
            return null;
        }
    }
}
