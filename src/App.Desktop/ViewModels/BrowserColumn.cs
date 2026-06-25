namespace App.Desktop.ViewModels;

/// <summary>Declarative column definition consumed by the EntityBrowser view to build grid columns.</summary>
public sealed record BrowserColumn(string Header, int Width, bool Fill = false, bool IsImage = false, int ImageSize = 16);
