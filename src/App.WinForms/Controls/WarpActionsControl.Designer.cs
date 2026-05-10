namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class WarpActionsControl
{
    private System.ComponentModel.IContainer components = null;

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
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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

        // gbWarpActions (outer wrapper)
        gbWarpActions.Text = "Warp";
        gbWarpActions.AutoSize = true;
        gbWarpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbWarpActions.Dock = DockStyle.Top;
        gbWarpActions.Padding = new Padding(8, 18, 8, 8);
        gbWarpActions.Margin = new Padding(0);
        gbWarpActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.AutoSize = true;
        tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRoot.Dock = DockStyle.Top;
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.Controls.Add(gbWarpCommands, 0, 0);
        tlpRoot.Controls.Add(gbManageWarp, 0, 1);

        // gbWarpCommands
        gbWarpCommands.Text = "Warp commands";
        gbWarpCommands.AutoSize = true;
        gbWarpCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbWarpCommands.Dock = DockStyle.Top;
        gbWarpCommands.Padding = new Padding(8, 18, 8, 8);
        gbWarpCommands.Margin = new Padding(3, 3, 3, 6);
        gbWarpCommands.Controls.Add(tlpCommands);

        // tlpCommands
        tlpCommands.AutoSize = true;
        tlpCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCommands.Dock = DockStyle.Top;
        tlpCommands.ColumnCount = 4;
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCommands.RowCount = 3;
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlpCommands.Controls.Add(lblSelectedX, 0, 0);
        tlpCommands.Controls.Add(nudSelectedX, 1, 0);
        tlpCommands.Controls.Add(lblSelectedY, 2, 0);
        tlpCommands.Controls.Add(nudSelectedY, 3, 0);
        tlpCommands.Controls.Add(btnWarpToYou, 0, 1);
        tlpCommands.SetColumnSpan(btnWarpToYou, 2);
        tlpCommands.Controls.Add(btnWarp, 2, 1);
        tlpCommands.SetColumnSpan(btnWarp, 2);
        tlpCommands.Controls.Add(btnWarpToSomeone, 0, 2);
        tlpCommands.SetColumnSpan(btnWarpToSomeone, 2);
        tlpCommands.Controls.Add(btnOpenWorldmap, 2, 2);
        tlpCommands.SetColumnSpan(btnOpenWorldmap, 2);

        ConfigureLabel(lblSelectedX, "X:");
        ConfigureLabel(lblSelectedY, "Y:");
        ConfigureCoordNumeric(nudSelectedX);
        ConfigureCoordNumeric(nudSelectedY);

        ConfigureFillButton(btnWarpToYou, "Warp to you", btnWarpToYou_Click);
        ConfigureFillButton(btnWarp, "Warp", btnWarp_Click);
        ConfigureFillButton(btnWarpToSomeone, "Warp player", btnWarpToSomeone_Click);

        btnOpenWorldmap.Text = "Open worldmap";
        btnOpenWorldmap.AutoSize = false;
        btnOpenWorldmap.Dock = DockStyle.Fill;
        btnOpenWorldmap.Margin = new Padding(3);
        btnOpenWorldmap.UseVisualStyleBackColor = true;
        btnOpenWorldmap.Enabled = false;

        // gbManageWarp
        gbManageWarp.Text = "Manage warp locations";
        gbManageWarp.AutoSize = true;
        gbManageWarp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbManageWarp.Dock = DockStyle.Top;
        gbManageWarp.Padding = new Padding(8, 18, 8, 8);
        gbManageWarp.Margin = new Padding(3, 3, 3, 6);
        gbManageWarp.Controls.Add(tlpManage);

        // tlpManage
        tlpManage.AutoSize = true;
        tlpManage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpManage.Dock = DockStyle.Top;
        tlpManage.ColumnCount = 4;
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpManage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpManage.RowCount = 3;
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpManage.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlpManage.Controls.Add(lblAddX, 0, 0);
        tlpManage.Controls.Add(nudAddX, 1, 0);
        tlpManage.Controls.Add(lblAddY, 2, 0);
        tlpManage.Controls.Add(nudAddY, 3, 0);
        tlpManage.Controls.Add(lblLocationName, 0, 1);
        tlpManage.Controls.Add(txtLocationName, 1, 1);
        tlpManage.SetColumnSpan(txtLocationName, 3);
        tlpManage.Controls.Add(btnAdd, 0, 2);
        tlpManage.SetColumnSpan(btnAdd, 2);
        tlpManage.Controls.Add(btnRemoveSelected, 2, 2);
        tlpManage.SetColumnSpan(btnRemoveSelected, 2);

        ConfigureLabel(lblAddX, "X:");
        ConfigureLabel(lblAddY, "Y:");
        ConfigureLabel(lblLocationName, "Name:");
        ConfigureCoordNumeric(nudAddX);
        ConfigureCoordNumeric(nudAddY);

        txtLocationName.Dock = DockStyle.Fill;
        txtLocationName.Margin = new Padding(3, 4, 3, 4);

        ConfigureFillButton(btnAdd, "Add", btnAdd_Click);
        ConfigureFillButton(btnRemoveSelected, "Remove selected", btnRemoveSelected_Click);

        // WarpActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbWarpActions);
        Name = "WarpActionsControl";

        ((System.ComponentModel.ISupportInitialize)nudAddY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAddX).EndInit();
        tlpManage.ResumeLayout(false);
        gbManageWarp.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudSelectedY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSelectedX).EndInit();
        tlpCommands.ResumeLayout(false);
        gbWarpCommands.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbWarpActions.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureLabel(Label label, string text)
    {
        label.Text = text;
        label.AutoSize = false;
        label.Dock = DockStyle.Fill;
        label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        label.Margin = new Padding(3, 0, 3, 0);
    }

    private static void ConfigureCoordNumeric(NumericUpDown nud)
    {
        nud.Dock = DockStyle.Fill;
        nud.Margin = new Padding(3, 4, 3, 4);
        nud.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nud.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
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
