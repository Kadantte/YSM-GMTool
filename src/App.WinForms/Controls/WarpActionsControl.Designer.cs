namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class WarpActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
    private GroupBox gbWarpActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbWarpCommands;
    private TableLayoutPanel tlpCommands;
    private Label lblSelectedX;
    private NumericUpDown nudSelectedX;
    private Label lblSelectedY;
    private NumericUpDown nudSelectedY;
    private Button btnWarpToYou;
    private Button btnWarp;
    private Button btnWarpToSomeone;
    private Button btnOpenWorldmap;
    private GroupBox gbManageWarp;
    private TableLayoutPanel tlpManage;
    private Label lblAddX;
    private NumericUpDown nudAddX;
    private Label lblAddY;
    private NumericUpDown nudAddY;
    private Label lblLocationName;
    private TextBox txtLocationName;
    private Button btnAdd;
    private Button btnRemoveSelected;

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
        gbWarpActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbWarpCommands = new GroupBox();
        tlpCommands = new TableLayoutPanel();
        lblSelectedX = new Label();
        nudSelectedX = new NumericUpDown();
        lblSelectedY = new Label();
        nudSelectedY = new NumericUpDown();
        btnWarpToYou = new Button();
        btnWarp = new Button();
        btnWarpToSomeone = new Button();
        btnOpenWorldmap = new Button();
        gbManageWarp = new GroupBox();
        tlpManage = new TableLayoutPanel();
        lblAddX = new Label();
        nudAddX = new NumericUpDown();
        lblAddY = new Label();
        nudAddY = new NumericUpDown();
        lblLocationName = new Label();
        txtLocationName = new TextBox();
        btnAdd = new Button();
        btnRemoveSelected = new Button();

        _tlpRoot.SuspendLayout();
        gbWarpActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbWarpCommands.SuspendLayout();
        tlpCommands.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSelectedX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudSelectedY).BeginInit();
        gbManageWarp.SuspendLayout();
        tlpManage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAddX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAddY).BeginInit();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbWarpActions, 0, 0);

        // gbWarpActions
        gbWarpActions.Text = "Warp Commands";
        gbWarpActions.AutoSize = false;
        gbWarpActions.Padding = new Padding(8, 18, 8, 8);
        gbWarpActions.Dock = DockStyle.Fill;
        gbWarpActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.ColumnCount = 1;
        tlpRoot.RowCount = 2;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.Controls.Add(gbWarpCommands, 0, 0);
        tlpRoot.Controls.Add(gbManageWarp, 0, 1);

        // gbWarpCommands
        gbWarpCommands.Text = "Selected Warp";
        gbWarpCommands.AutoSize = true;
        gbWarpCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbWarpCommands.Padding = new Padding(8, 18, 8, 8);
        gbWarpCommands.Dock = DockStyle.Top;
        gbWarpCommands.Controls.Add(tlpCommands);

        // tlpCommands
        tlpCommands.AutoSize = true;
        tlpCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCommands.Dock = DockStyle.Top;
        tlpCommands.ColumnCount = 4;
        tlpCommands.RowCount = 4;
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.Controls.Add(lblSelectedX, 0, 0);
        tlpCommands.Controls.Add(nudSelectedX, 1, 0);
        tlpCommands.Controls.Add(lblSelectedY, 2, 0);
        tlpCommands.Controls.Add(nudSelectedY, 3, 0);
        tlpCommands.Controls.Add(btnWarp, 0, 1);
        tlpCommands.Controls.Add(btnWarpToYou, 0, 2);
        tlpCommands.Controls.Add(btnWarpToSomeone, 2, 2);
        tlpCommands.Controls.Add(btnOpenWorldmap, 0, 3);
        tlpCommands.SetColumnSpan(btnWarp, 4);
        tlpCommands.SetColumnSpan(btnWarpToYou, 2);
        tlpCommands.SetColumnSpan(btnWarpToSomeone, 2);
        tlpCommands.SetColumnSpan(btnOpenWorldmap, 4);

        ConfigureLabel(lblSelectedX, "X");
        ConfigureLabel(lblSelectedY, "Y");

        // nudSelectedX
        nudSelectedX.Dock = DockStyle.Fill;
        nudSelectedX.Margin = new Padding(3, 4, 3, 4);
        nudSelectedX.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudSelectedX.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        // nudSelectedY
        nudSelectedY.Dock = DockStyle.Fill;
        nudSelectedY.Margin = new Padding(3, 4, 3, 4);
        nudSelectedY.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudSelectedY.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        ConfigureGridButton(btnWarp, "Warp", btnWarp_Click);
        ConfigureGridButton(btnWarpToYou, "Warp to you", btnWarpToYou_Click);
        ConfigureGridButton(btnWarpToSomeone, "Warp to someone", btnWarpToSomeone_Click);

        // btnOpenWorldmap (no handler)
        btnOpenWorldmap.Text = "OpenWorldmap";
        btnOpenWorldmap.AutoSize = false;
        btnOpenWorldmap.Dock = DockStyle.Fill;
        btnOpenWorldmap.Margin = new Padding(3);
        btnOpenWorldmap.UseVisualStyleBackColor = true;
        btnOpenWorldmap.Enabled = false;

        // gbManageWarp
        gbManageWarp.Text = "Manage Warps";
        gbManageWarp.AutoSize = true;
        gbManageWarp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbManageWarp.Padding = new Padding(8, 18, 8, 8);
        gbManageWarp.Dock = DockStyle.Top;
        gbManageWarp.Controls.Add(tlpManage);

        // tlpManage
        tlpManage.AutoSize = true;
        tlpManage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpManage.Dock = DockStyle.Top;
        tlpManage.ColumnCount = 4;
        tlpManage.RowCount = 3;
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpManage.Controls.Add(lblAddX, 0, 0);
        tlpManage.Controls.Add(nudAddX, 1, 0);
        tlpManage.Controls.Add(lblAddY, 2, 0);
        tlpManage.Controls.Add(nudAddY, 3, 0);
        tlpManage.Controls.Add(lblLocationName, 0, 1);
        tlpManage.Controls.Add(txtLocationName, 1, 1);
        tlpManage.Controls.Add(btnAdd, 0, 2);
        tlpManage.Controls.Add(btnRemoveSelected, 2, 2);
        tlpManage.SetColumnSpan(txtLocationName, 3);
        tlpManage.SetColumnSpan(btnAdd, 2);
        tlpManage.SetColumnSpan(btnRemoveSelected, 2);

        ConfigureLabel(lblAddX, "X");
        ConfigureLabel(lblAddY, "Y");
        ConfigureLabel(lblLocationName, "Loc");

        // nudAddX
        nudAddX.Dock = DockStyle.Fill;
        nudAddX.Margin = new Padding(3, 4, 3, 4);
        nudAddX.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudAddX.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        // nudAddY
        nudAddY.Dock = DockStyle.Fill;
        nudAddY.Margin = new Padding(3, 4, 3, 4);
        nudAddY.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudAddY.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        // txtLocationName
        txtLocationName.Dock = DockStyle.Fill;
        txtLocationName.Margin = new Padding(3, 4, 3, 4);
        txtLocationName.PlaceholderText = "Location name";

        ConfigureGridButton(btnAdd, "Add", btnAdd_Click);
        ConfigureGridButton(btnRemoveSelected, "X", btnRemoveSelected_Click);

        // WarpActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        MinimumSize = new Size(430, 0);
        Name = "WarpActionsControl";
        Size = new Size(430, 330);

        ((System.ComponentModel.ISupportInitialize)nudAddY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAddX).EndInit();
        tlpManage.ResumeLayout(false);
        tlpManage.PerformLayout();
        gbManageWarp.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudSelectedY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSelectedX).EndInit();
        tlpCommands.ResumeLayout(false);
        tlpCommands.PerformLayout();
        gbWarpCommands.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbWarpActions.ResumeLayout(false);
        _tlpRoot.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        label.AutoSize = false;
        label.Margin = new Padding(3, 0, 3, 0);
    }

    private static void ConfigureGridButton(Button button, string text, EventHandler handler)
    {
        button.Text = text;
        button.AutoSize = false;
        button.Dock = DockStyle.Fill;
        button.Margin = new Padding(3);
        button.UseVisualStyleBackColor = true;
        button.Click += handler;
    }
}
