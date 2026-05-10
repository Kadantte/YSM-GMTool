namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class NpcsActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbNpcActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelectedNpc;
    private TableLayoutPanel tlpSelected;
    private Label lblNpcId;
    private NumericUpDown nudNpcId;
    private Label lblNpcName;
    private TextBox txtNpcName;
    private Label lblContactScript;
    private TextBox txtContactScript;
    private Label lblX;
    private NumericUpDown nudX;
    private Label lblY;
    private NumericUpDown nudY;
    private Label lblLayer;
    private NumericUpDown nudLayer;
    private CheckBox chkHideNpc;
    private GroupBox gbCommands;
    private TableLayoutPanel tlpCommands;
    private Button btnAddNpcToWorld;
    private Button btnShowNpc;
    private Button btnWarpToNpc;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbNpcActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbSelectedNpc = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblNpcId = new Label();
        nudNpcId = new NumericUpDown();
        lblNpcName = new Label();
        txtNpcName = new TextBox();
        lblContactScript = new Label();
        txtContactScript = new TextBox();
        lblX = new Label();
        nudX = new NumericUpDown();
        lblY = new Label();
        nudY = new NumericUpDown();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        chkHideNpc = new CheckBox();
        gbCommands = new GroupBox();
        tlpCommands = new TableLayoutPanel();
        btnAddNpcToWorld = new Button();
        btnShowNpc = new Button();
        btnWarpToNpc = new Button();

        gbNpcActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelectedNpc.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudNpcId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        gbCommands.SuspendLayout();
        tlpCommands.SuspendLayout();
        SuspendLayout();

        // gbNpcActions (outer wrapper)
        gbNpcActions.Text = "NPCs";
        gbNpcActions.AutoSize = true;
        gbNpcActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbNpcActions.Dock = DockStyle.Top;
        gbNpcActions.Padding = new Padding(8, 18, 8, 8);
        gbNpcActions.Margin = new Padding(0);
        gbNpcActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.AutoSize = true;
        tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRoot.Dock = DockStyle.Top;
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.Controls.Add(gbSelectedNpc, 0, 0);
        tlpRoot.Controls.Add(gbCommands, 0, 1);

        // gbSelectedNpc
        gbSelectedNpc.Text = "Selected NPC";
        gbSelectedNpc.AutoSize = true;
        gbSelectedNpc.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSelectedNpc.Dock = DockStyle.Top;
        gbSelectedNpc.Padding = new Padding(8, 18, 8, 8);
        gbSelectedNpc.Margin = new Padding(3, 3, 3, 6);
        gbSelectedNpc.Controls.Add(tlpSelected);

        // tlpSelected
        tlpSelected.AutoSize = true;
        tlpSelected.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpSelected.Dock = DockStyle.Top;
        tlpSelected.ColumnCount = 4;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.RowCount = 5;
        for (int i = 0; i < 5; i++)
        {
            tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        }

        tlpSelected.Controls.Add(lblNpcId, 0, 0);
        tlpSelected.Controls.Add(nudNpcId, 1, 0);
        tlpSelected.Controls.Add(lblNpcName, 0, 1);
        tlpSelected.Controls.Add(txtNpcName, 1, 1);
        tlpSelected.SetColumnSpan(txtNpcName, 3);
        tlpSelected.Controls.Add(lblContactScript, 0, 2);
        tlpSelected.Controls.Add(txtContactScript, 1, 2);
        tlpSelected.SetColumnSpan(txtContactScript, 3);
        tlpSelected.Controls.Add(lblX, 0, 3);
        tlpSelected.Controls.Add(nudX, 1, 3);
        tlpSelected.Controls.Add(lblY, 2, 3);
        tlpSelected.Controls.Add(nudY, 3, 3);
        tlpSelected.Controls.Add(lblLayer, 0, 4);
        tlpSelected.Controls.Add(nudLayer, 1, 4);
        tlpSelected.Controls.Add(chkHideNpc, 2, 4);
        tlpSelected.SetColumnSpan(chkHideNpc, 2);

        ConfigureLabel(lblNpcId, "NPC ID:");
        ConfigureLabel(lblNpcName, "Name:");
        ConfigureLabel(lblContactScript, "Contact script:");
        ConfigureLabel(lblX, "X:");
        ConfigureLabel(lblY, "Y:");
        ConfigureLabel(lblLayer, "Layer:");

        ConfigureNumeric(nudNpcId, 1, 1_000_000_000, 1);
        nudX.Dock = DockStyle.Fill;
        nudX.Margin = new Padding(3, 4, 3, 4);
        nudX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudY.Dock = DockStyle.Fill;
        nudY.Margin = new Padding(3, 4, 3, 4);
        nudY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Margin = new Padding(3, 4, 3, 4);
        nudLayer.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });

        txtNpcName.Dock = DockStyle.Fill;
        txtNpcName.Margin = new Padding(3, 4, 3, 4);
        txtNpcName.ReadOnly = true;
        txtContactScript.Dock = DockStyle.Fill;
        txtContactScript.Margin = new Padding(3, 4, 3, 4);
        txtContactScript.ReadOnly = true;

        chkHideNpc.AutoSize = false;
        chkHideNpc.Dock = DockStyle.Fill;
        chkHideNpc.Margin = new Padding(3, 0, 3, 0);
        chkHideNpc.Text = "Hide NPC";
        chkHideNpc.UseVisualStyleBackColor = true;

        // gbCommands
        gbCommands.Text = "Commands";
        gbCommands.AutoSize = true;
        gbCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbCommands.Dock = DockStyle.Top;
        gbCommands.Padding = new Padding(8, 18, 8, 8);
        gbCommands.Margin = new Padding(3, 3, 3, 6);
        gbCommands.Controls.Add(tlpCommands);

        // tlpCommands
        tlpCommands.AutoSize = true;
        tlpCommands.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCommands.Dock = DockStyle.Top;
        tlpCommands.ColumnCount = 1;
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCommands.RowCount = 3;
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.Controls.Add(btnAddNpcToWorld, 0, 0);
        tlpCommands.Controls.Add(btnShowNpc, 0, 1);
        tlpCommands.Controls.Add(btnWarpToNpc, 0, 2);

        ConfigureFillButton(btnAddNpcToWorld, "Add NPC to world", btnAddNpcToWorld_Click);
        ConfigureFillButton(btnShowNpc, "Show NPC", btnShowNpc_Click);
        ConfigureFillButton(btnWarpToNpc, "Warp to NPC", btnWarpToNpc_Click);

        // NpcsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbNpcActions);
        Name = "NpcsActionsControl";

        tlpCommands.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudNpcId).EndInit();
        tlpSelected.ResumeLayout(false);
        gbSelectedNpc.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbNpcActions.ResumeLayout(false);
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

    private static void ConfigureNumeric(NumericUpDown nud, int min, int max, int value)
    {
        nud.Dock = DockStyle.Fill;
        nud.Margin = new Padding(3, 4, 3, 4);
        nud.Minimum = min;
        nud.Maximum = max;
        nud.Value = value;
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
