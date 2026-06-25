using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace App.Desktop.Infrastructure;

/// <summary>
/// Converts an icon key (string) to a letterboxed <see cref="Bitmap"/> via <see cref="IconCache"/>.
/// The target square size is passed through the converter parameter (e.g. ConverterParameter=36).
/// </summary>
public sealed class IconKeyToBitmapConverter : IValueConverter
{
    public static readonly IconKeyToBitmapConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var key = value as string ?? value?.ToString();
        var size = ParseSize(parameter);
        return IconCache.Resolve(key, size);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();

    private static int ParseSize(object? parameter)
    {
        if (parameter is int i)
        {
            return i;
        }

        if (parameter is string s && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return 16;
    }
}
