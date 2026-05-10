namespace App.WinForms.Forms;

public sealed partial class ExportProgressForm : Form
{
    public ExportProgressForm()
    {
        InitializeComponent();
    }

    public void Report(string statusText, int current, int total)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => Report(statusText, current, total));
            return;
        }

        _lblStatus.Text = total > 0 ? $"{statusText}: {current} / {total}" : statusText;
        if (total > 0)
        {
            _progressBar.Style = ProgressBarStyle.Continuous;
            _progressBar.Maximum = total;
            _progressBar.Value = Math.Min(current, total);
        }
        else
        {
            _progressBar.Style = ProgressBarStyle.Marquee;
        }
    }
}
