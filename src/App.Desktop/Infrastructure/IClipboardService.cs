namespace App.Desktop.Infrastructure;

public interface IClipboardService
{
    Task SetTextAsync(string text);
}
