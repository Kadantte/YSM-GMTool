namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class NpcsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
    private GroupBox gbNpcActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelectedNpc;
    private GroupBox gbCommands;
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
    private TableLayoutPanel tlpCommands;
    private Button btnAddNpcToWorld;
    private Button btnShowNpc;
    private Button btnWarpToNpc;

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

        _tlpRoot.SuspendLayout();
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

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbNpcActions, 0, 0);

        // gbNpcActions
        gbNpcActions.Text = "NPC Commands";
        gbNpcActions.AutoSize = false;
        gbNpcActions.Padding = new Padding(8, 18, 8, 8);
        gbNpcActions.Dock = DockStyle.Fill;
        gbNpcActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.ColumnCount = 1;
        tlpRoot.RowCount = 2;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        tlpRoot.Controls.Add(gbSelectedNpc, 0, 0);
        tlpRoot.Controls.Add(gbCommands, 0, 1);

        // gbSelectedNpc
        gbSelectedNpc.Text = "Selected NPC";
        gbSelectedNpc.AutoSize = false;
        gbSelectedNpc.Padding = new Padding(8, 18, 8, 8);
        gbSelectedNpc.Dock = DockStyle.Fill;
        gbSelectedNpc.Controls.Add(tlpSelected);

        // tlpSelected
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.ColumnCount = 2;
        tlpSelected.RowCount = 7;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 7; i++)
        {
            tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        }
        tlpSelected.Controls.Add(lblNpcId, 0, 0);
        tlpSelected.Controls.Add(nudNpcId, 1, 0);
        tlpSelected.Controls.Add(lblNpcName, 0, 1);
        tlpSelected.Controls.Add(txtNpcName, 1, 1);
        tlpSelected.Controls.Add(lblContactScript, 0, 2);
        tlpSelected.Controls.Add(txtContactScript, 1, 2);
        tlpSelected.Controls.Add(lblX, 0, 3);
        tlpSelected.Controls.Add(nudX, 1, 3);
        tlpSelected.Controls.Add(lblY, 0, 4);
        tlpSelected.Controls.Add(nudY, 1, 4);
        tlpSelected.Controls.Add(lblLayer, 0, 5);
        tlpSelected.Controls.Add(nudLayer, 1, 5);
        tlpSelected.Controls.Add(chkHideNpc, 1, 6);

        ConfigureLabel(lblNpcId, "NPC ID");
        ConfigureLabel(lblNpcName, "Name");
        ConfigureLabel(lblContactScript, "Contact script");
        ConfigureLabel(lblX, "X");
        ConfigureLabel(lblY, "Y");
        ConfigureLabel(lblLayer, "Layer");

        // nudNpcId
        nudNpcId.Dock = DockStyle.Fill;
        nudNpcId.Margin = new Padding(3, 4, 3, 4);
        nudNpcId.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });

        // txtNpcName
        txtNpcName.Dock = DockStyle.Fill;
        txtNpcName.Margin = new Padding(3, 4, 3, 4);
        txtNpcName.ReadOnly = true;

        // txtContactScript
        txtContactScript.Dock = DockStyle.Fill;
        txtContactScript.Margin = new Padding(3, 4, 3, 4);
        txtContactScript.ReadOnly = true;

        // nudX
        nudX.Dock = DockStyle.Fill;
        nudX.Margin = new Padding(3, 4, 3, 4);
        nudX.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        // nudY
        nudY.Dock = DockStyle.Fill;
        nudY.Margin = new Padding(3, 4, 3, 4);
        nudY.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });

        // nudLayer
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Margin = new Padding(3, 4, 3, 4);
        nudLayer.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });

        // chkHideNpc
        chkHideNpc.AutoSize = true;
        chkHideNpc.Anchor = AnchorStyles.Left;
        chkHideNpc.Text = "Hide (visible = 1)";
        chkHideNpc.UseVisualStyleBackColor = true;

        // gbCommands
        gbCommands.Text = "Actions";
        gbCommands.AutoSize = false;
        gbCommands.Padding = new Padding(8, 18, 8, 8);
        gbCommands.Dock = DockStyle.Fill;
        gbCommands.Controls.Add(tlpCommands);

        // tlpCommands
        tlpCommands.Dock = DockStyle.Fill;
        tlpCommands.ColumnCount = 3;
        tlpCommands.RowCount = 1;
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        tlpCommands.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCommands.Controls.Add(btnAddNpcToWorld, 0, 0);
        tlpCommands.Controls.Add(btnShowNpc, 1, 0);
        tlpCommands.Controls.Add(btnWarpToNpc, 2, 0);

        ConfigureGridButton(btnAddNpcToWorld, "Add NPC to world", btnAddNpcToWorld_Click);
        ConfigureGridButton(btnShowNpc, "Show/Hide NPC", btnShowNpc_Click);
        ConfigureGridButton(btnWarpToNpc, "Warp to NPC", btnWarpToNpc_Click);

        // NpcsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        MinimumSize = new Size(400, 360);
        Name = "NpcsActionsControl";
        Size = new Size(580, 360);

        tlpCommands.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudNpcId).EndInit();
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        gbSelectedNpc.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbNpcActions.ResumeLayout(false);
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
