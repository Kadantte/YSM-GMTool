namespace App.WinForms.Forms;

partial class ExportProgressForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot = null!;
    private Label _lblStatus = null!;
    private ProgressBar _progressBar = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tlpRoot = new TableLayoutPanel();
        _lblStatus = new Label();
        _progressBar = new ProgressBar();
        _tlpRoot.SuspendLayout();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 2;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        _tlpRoot.Padding = new Padding(12);
        _tlpRoot.Controls.Add(_lblStatus, 0, 0);
        _tlpRoot.Controls.Add(_progressBar, 0, 1);

        // _lblStatus
        _lblStatus.Dock = DockStyle.Fill;
        _lblStatus.AutoSize = false;
        _lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        _lblStatus.Text = "Preparing…";

        // _progressBar
        _progressBar.Dock = DockStyle.Fill;
        _progressBar.Style = ProgressBarStyle.Marquee;

        // ExportProgressForm
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(420, 96);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        ControlBox = false;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        Text = "Export";
        Controls.Add(_tlpRoot);

        _tlpRoot.ResumeLayout(false);
        ResumeLayout(false);
    }
}
