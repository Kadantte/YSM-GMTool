namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
    private GroupBox gbPlayerChecker;
    private TableLayoutPanel _tlpInner;
    private TableLayoutPanel tlpLoadButtons;
    private TableLayoutPanel tlpActionButtons;
    private Button btnLoadAllCharacters;
    private Button btnLoadOnlineCharacters;
    private Button btnLoadInventory;
    private Button btnLoadWh;
    private Button btnOpenInfos;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tlpRoot = new TableLayoutPanel();
        gbPlayerChecker = new GroupBox();
        _tlpInner = new TableLayoutPanel();
        tlpLoadButtons = new TableLayoutPanel();
        tlpActionButtons = new TableLayoutPanel();
        btnLoadAllCharacters = new Button();
        btnLoadOnlineCharacters = new Button();
        btnLoadInventory = new Button();
        btnLoadWh = new Button();
        btnOpenInfos = new Button();

        _tlpRoot.SuspendLayout();
        gbPlayerChecker.SuspendLayout();
        _tlpInner.SuspendLayout();
        tlpLoadButtons.SuspendLayout();
        tlpActionButtons.SuspendLayout();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 2;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbPlayerChecker, 0, 0);

        // gbPlayerChecker
        gbPlayerChecker.Text = "Playerchecker Actions";
        gbPlayerChecker.AutoSize = true;
        gbPlayerChecker.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbPlayerChecker.Padding = new Padding(8, 18, 8, 8);
        gbPlayerChecker.Dock = DockStyle.Top;
        gbPlayerChecker.Margin = new Padding(3);
        gbPlayerChecker.Controls.Add(_tlpInner);

        // _tlpInner
        _tlpInner.AutoSize = true;
        _tlpInner.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _tlpInner.Dock = DockStyle.Top;
        _tlpInner.ColumnCount = 1;
        _tlpInner.RowCount = 2;
        _tlpInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        _tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        _tlpInner.Controls.Add(tlpLoadButtons, 0, 0);
        _tlpInner.Controls.Add(tlpActionButtons, 0, 1);

        // tlpLoadButtons
        tlpLoadButtons.Dock = DockStyle.Fill;
        tlpLoadButtons.ColumnCount = 2;
        tlpLoadButtons.RowCount = 1;
        tlpLoadButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpLoadButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpLoadButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpLoadButtons.Margin = new Padding(0);
        tlpLoadButtons.Controls.Add(btnLoadAllCharacters, 0, 0);
        tlpLoadButtons.Controls.Add(btnLoadOnlineCharacters, 1, 0);

        // tlpActionButtons
        tlpActionButtons.Dock = DockStyle.Fill;
        tlpActionButtons.ColumnCount = 3;
        tlpActionButtons.RowCount = 1;
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        tlpActionButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpActionButtons.Margin = new Padding(0);
        tlpActionButtons.Controls.Add(btnLoadInventory, 0, 0);
        tlpActionButtons.Controls.Add(btnLoadWh, 1, 0);
        tlpActionButtons.Controls.Add(btnOpenInfos, 2, 0);

        // btnLoadAllCharacters
        btnLoadAllCharacters.Text = "Load All Characters";
        btnLoadAllCharacters.AutoSize = false;
        btnLoadAllCharacters.Size = new Size(160, 28);
        btnLoadAllCharacters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnLoadAllCharacters.Margin = new Padding(3);
        btnLoadAllCharacters.UseVisualStyleBackColor = true;
        btnLoadAllCharacters.Click += btnLoadAllCharacters_Click;

        // btnLoadOnlineCharacters
        btnLoadOnlineCharacters.Text = "Load Online Characters";
        btnLoadOnlineCharacters.AutoSize = false;
        btnLoadOnlineCharacters.Size = new Size(160, 28);
        btnLoadOnlineCharacters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnLoadOnlineCharacters.Margin = new Padding(3);
        btnLoadOnlineCharacters.UseVisualStyleBackColor = true;
        btnLoadOnlineCharacters.Click += btnLoadOnlineCharacters_Click;

        // btnLoadInventory
        btnLoadInventory.Text = "Load inventory";
        btnLoadInventory.AutoSize = false;
        btnLoadInventory.Size = new Size(100, 28);
        btnLoadInventory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnLoadInventory.Margin = new Padding(3);
        btnLoadInventory.UseVisualStyleBackColor = true;
        btnLoadInventory.Click += btnLoadInventory_Click;

        // btnLoadWh
        btnLoadWh.Text = "Load WH";
        btnLoadWh.AutoSize = false;
        btnLoadWh.Size = new Size(100, 28);
        btnLoadWh.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnLoadWh.Margin = new Padding(3);
        btnLoadWh.UseVisualStyleBackColor = true;
        btnLoadWh.Click += btnLoadWh_Click;

        // btnOpenInfos
        btnOpenInfos.Text = "Open infos";
        btnOpenInfos.AutoSize = false;
        btnOpenInfos.Size = new Size(100, 28);
        btnOpenInfos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnOpenInfos.Margin = new Padding(3);
        btnOpenInfos.UseVisualStyleBackColor = true;
        btnOpenInfos.Click += btnOpenInfos_Click;

        // PlayerCheckerActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        Name = "PlayerCheckerActionsControl";
        Size = new Size(360, 130);

        tlpActionButtons.ResumeLayout(false);
        tlpLoadButtons.ResumeLayout(false);
        _tlpInner.ResumeLayout(false);
        gbPlayerChecker.ResumeLayout(false);
        _tlpRoot.ResumeLayout(false);
        ResumeLayout(false);
    }
}
