namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbPlayerChecker;
    private TableLayoutPanel tlpInner;
    private Button btnLoadAllCharacters;
    private Button btnLoadOnlineCharacters;
    private Button btnLoadInventory;
    private Button btnLoadWh;
    private Button btnOpenInfos;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbPlayerChecker = new GroupBox();
        tlpInner = new TableLayoutPanel();
        btnLoadAllCharacters = new Button();
        btnLoadOnlineCharacters = new Button();
        btnLoadInventory = new Button();
        btnLoadWh = new Button();
        btnOpenInfos = new Button();

        gbPlayerChecker.SuspendLayout();
        tlpInner.SuspendLayout();
        SuspendLayout();

        // gbPlayerChecker (outer wrapper)
        gbPlayerChecker.Text = "Player Checker";
        gbPlayerChecker.AutoSize = true;
        gbPlayerChecker.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbPlayerChecker.Dock = DockStyle.Top;
        gbPlayerChecker.Padding = new Padding(8, 18, 8, 8);
        gbPlayerChecker.Margin = new Padding(0);
        gbPlayerChecker.Controls.Add(tlpInner);

        // tlpInner
        tlpInner.AutoSize = true;
        tlpInner.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpInner.Dock = DockStyle.Top;
        tlpInner.ColumnCount = 2;
        tlpInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInner.RowCount = 3;
        tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpInner.Controls.Add(btnLoadAllCharacters, 0, 0);
        tlpInner.Controls.Add(btnLoadOnlineCharacters, 1, 0);
        tlpInner.Controls.Add(btnLoadInventory, 0, 1);
        tlpInner.Controls.Add(btnLoadWh, 1, 1);
        tlpInner.Controls.Add(btnOpenInfos, 0, 2);
        tlpInner.SetColumnSpan(btnOpenInfos, 2);

        ConfigureFillButton(btnLoadAllCharacters, "Load all characters", btnLoadAllCharacters_Click);
        ConfigureFillButton(btnLoadOnlineCharacters, "Load online characters", btnLoadOnlineCharacters_Click);
        ConfigureFillButton(btnLoadInventory, "Load inventory", btnLoadInventory_Click);
        ConfigureFillButton(btnLoadWh, "Load warehouse", btnLoadWh_Click);
        ConfigureFillButton(btnOpenInfos, "Open infos", btnOpenInfos_Click);

        // PlayerCheckerActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbPlayerChecker);
        Name = "PlayerCheckerActionsControl";

        tlpInner.ResumeLayout(false);
        gbPlayerChecker.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureFillButton(Button button, string text, EventHandler handler)
    {
        button.Text = text;
        button.AutoSize = false;
        button.Dock = DockStyle.Fill;
        button.Margin = new Padding(3);
        button.UseVisualStyleBackColor = true;
        button.Click += handler;
    }
}
