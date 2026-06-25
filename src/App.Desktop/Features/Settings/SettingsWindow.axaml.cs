using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop.Features.Settings;

public partial class SettingsWindow : Window
{
    private readonly SettingsViewModel _viewModel;

    public SettingsWindow()
    {
        InitializeComponent();

        _viewModel = Program.Services.GetRequiredService<SettingsViewModel>();
        _viewModel.FolderPicker = PickFolderAsync;
        _viewModel.CloseRequested += (_, _) => Close();
        DataContext = _viewModel;
    }

    private async Task<string?> PickFolderAsync(string? startPath)
    {
        var options = new FolderPickerOpenOptions
        {
            Title = "Select folder with icon files",
            AllowMultiple = false,
        };

        if (!string.IsNullOrWhiteSpace(startPath))
        {
            options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(startPath);
        }

        var folders = await StorageProvider.OpenFolderPickerAsync(options);
        var folder = folders.FirstOrDefault();
        return folder?.TryGetLocalPath();
    }
}
