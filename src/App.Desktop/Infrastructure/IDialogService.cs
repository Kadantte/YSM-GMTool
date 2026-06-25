namespace App.Desktop.Infrastructure;

/// <summary>Centralizes the modal message boxes that the WinForms app did via <c>MessageBox.Show</c>.</summary>
public interface IDialogService
{
    Task ShowInfoAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    Task<bool> ConfirmAsync(string title, string message);
}
