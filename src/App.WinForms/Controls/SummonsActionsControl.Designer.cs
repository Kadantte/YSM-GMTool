namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class SummonsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
    private GroupBox gbSummonActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbAddSummon;
    private GroupBox gbStageSummon;
    private TableLayoutPanel tlpAdd;
    private TableLayoutPanel tlpStage;
    private Label lblSummonId;
    private NumericUpDown nudSummonId;
    private CheckBox chkUseAddStage;
    private Label lblAddStage;
    private NumericUpDown nudAddStage;
    private Button btnAddSummon;
    private Label lblSlot;
    private ComboBox cmbSlot;
    private Label lblStage;
    private NumericUpDown nudStage;
    private Button btnStageSummon;

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
        gbSummonActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbAddSummon = new GroupBox();
        tlpAdd = new TableLayoutPanel();
        lblSummonId = new Label();
        nudSummonId = new NumericUpDown();
        chkUseAddStage = new CheckBox();
        lblAddStage = new Label();
        nudAddStage = new NumericUpDown();
        btnAddSummon = new Button();
        gbStageSummon = new GroupBox();
        tlpStage = new TableLayoutPanel();
        lblSlot = new Label();
        cmbSlot = new ComboBox();
        lblStage = new Label();
        nudStage = new NumericUpDown();
        btnStageSummon = new Button();

        _tlpRoot.SuspendLayout();
        gbSummonActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbAddSummon.SuspendLayout();
        tlpAdd.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSummonId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAddStage).BeginInit();
        gbStageSummon.SuspendLayout();
        tlpStage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudStage).BeginInit();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbSummonActions, 0, 0);

        // gbSummonActions
        gbSummonActions.Text = "Summon Commands";
        gbSummonActions.AutoSize = false;
        gbSummonActions.Padding = new Padding(8, 18, 8, 8);
        gbSummonActions.Dock = DockStyle.Fill;
        gbSummonActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.ColumnCount = 1;
        tlpRoot.RowCount = 2;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
        tlpRoot.Controls.Add(gbAddSummon, 0, 0);
        tlpRoot.Controls.Add(gbStageSummon, 0, 1);

        // gbAddSummon
        gbAddSummon.Text = "Add Summon";
        gbAddSummon.AutoSize = false;
        gbAddSummon.Padding = new Padding(8, 18, 8, 8);
        gbAddSummon.Dock = DockStyle.Fill;
        gbAddSummon.Controls.Add(tlpAdd);

        // tlpAdd
        tlpAdd.Dock = DockStyle.Fill;
        tlpAdd.ColumnCount = 3;
        tlpAdd.RowCount = 2;
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        tlpAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpAdd.Controls.Add(lblSummonId, 0, 0);
        tlpAdd.Controls.Add(nudSummonId, 1, 0);
        tlpAdd.Controls.Add(chkUseAddStage, 2, 0);
        tlpAdd.Controls.Add(lblAddStage, 0, 1);
        tlpAdd.Controls.Add(nudAddStage, 1, 1);
        tlpAdd.Controls.Add(btnAddSummon, 2, 1);

        ConfigureLabel(lblSummonId, "SummonID");
        ConfigureLabel(lblAddStage, "Stage");

        // nudSummonId
        nudSummonId.Dock = DockStyle.Fill;
        nudSummonId.Margin = new Padding(3, 4, 3, 4);
        nudSummonId.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });

        // chkUseAddStage
        chkUseAddStage.Anchor = AnchorStyles.Left;
        chkUseAddStage.AutoSize = true;
        chkUseAddStage.Text = "Use stage";
        chkUseAddStage.UseVisualStyleBackColor = true;
        chkUseAddStage.CheckedChanged += chkUseAddStage_CheckedChanged;

        // nudAddStage
        nudAddStage.Dock = DockStyle.Fill;
        nudAddStage.Margin = new Padding(3, 4, 3, 4);
        nudAddStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });

        // btnAddSummon
        btnAddSummon.Text = "Add Summon";
        btnAddSummon.AutoSize = false;
        btnAddSummon.Size = new Size(130, 28);
        btnAddSummon.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnAddSummon.Margin = new Padding(3);
        btnAddSummon.UseVisualStyleBackColor = true;
        btnAddSummon.Click += btnAddSummon_Click;

        // gbStageSummon
        gbStageSummon.Text = "Stage Summon";
        gbStageSummon.AutoSize = false;
        gbStageSummon.Padding = new Padding(8, 18, 8, 8);
        gbStageSummon.Dock = DockStyle.Fill;
        gbStageSummon.Controls.Add(tlpStage);

        // tlpStage
        tlpStage.Dock = DockStyle.Fill;
        tlpStage.ColumnCount = 2;
        tlpStage.RowCount = 3;
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpStage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpStage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpStage.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpStage.Controls.Add(lblSlot, 0, 0);
        tlpStage.Controls.Add(cmbSlot, 1, 0);
        tlpStage.Controls.Add(lblStage, 0, 1);
        tlpStage.Controls.Add(nudStage, 1, 1);
        tlpStage.Controls.Add(btnStageSummon, 0, 2);
        tlpStage.SetColumnSpan(btnStageSummon, 2);

        ConfigureLabel(lblSlot, "Slot");
        ConfigureLabel(lblStage, "Stage");

        // cmbSlot
        cmbSlot.Dock = DockStyle.Fill;
        cmbSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSlot.FormattingEnabled = true;
        cmbSlot.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5 });
        cmbSlot.Margin = new Padding(3, 4, 3, 4);

        // nudStage
        nudStage.Dock = DockStyle.Fill;
        nudStage.Margin = new Padding(3, 4, 3, 4);
        nudStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });

        // btnStageSummon
        btnStageSummon.Text = "Stage Summon";
        btnStageSummon.AutoSize = false;
        btnStageSummon.Size = new Size(130, 28);
        btnStageSummon.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnStageSummon.Margin = new Padding(3);
        btnStageSummon.UseVisualStyleBackColor = true;
        btnStageSummon.Click += btnStageSummon_Click;

        // SummonsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        MinimumSize = new Size(430, 0);
        Name = "SummonsActionsControl";
        Size = new Size(430, 280);

        ((System.ComponentModel.ISupportInitialize)nudStage).EndInit();
        tlpStage.ResumeLayout(false);
        tlpStage.PerformLayout();
        gbStageSummon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudAddStage).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSummonId).EndInit();
        tlpAdd.ResumeLayout(false);
        tlpAdd.PerformLayout();
        gbAddSummon.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSummonActions.ResumeLayout(false);
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
}
