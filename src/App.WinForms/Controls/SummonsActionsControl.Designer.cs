namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class SummonsActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbSummonActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbAddSummon;
    private TableLayoutPanel tlpAdd;
    private Label lblSummonId;
    private NumericUpDown nudSummonId;
    private CheckBox chkUseAddStage;
    private Label lblAddStage;
    private NumericUpDown nudAddStage;
    private Button btnAddSummon;
    private GroupBox gbStageSummon;
    private TableLayoutPanel tlpStage;
    private Label lblSlot;
    private ComboBox cmbSlot;
    private Label lblStage;
    private NumericUpDown nudStage;
    private Button btnStageSummon;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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

        // gbSummonActions (outer wrapper)
        gbSummonActions.Text = "Summons";
        gbSummonActions.AutoSize = true;
        gbSummonActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSummonActions.Dock = DockStyle.Top;
        gbSummonActions.Padding = new Padding(8, 18, 8, 8);
        gbSummonActions.Margin = new Padding(0);
        gbSummonActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.AutoSize = true;
        tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRoot.Dock = DockStyle.Top;
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.Controls.Add(gbAddSummon, 0, 0);
        tlpRoot.Controls.Add(gbStageSummon, 0, 1);

        // gbAddSummon
        gbAddSummon.Text = "Add summon";
        gbAddSummon.AutoSize = true;
        gbAddSummon.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbAddSummon.Dock = DockStyle.Top;
        gbAddSummon.Padding = new Padding(8, 18, 8, 8);
        gbAddSummon.Margin = new Padding(3, 3, 3, 6);
        gbAddSummon.Controls.Add(tlpAdd);

        // tlpAdd
        tlpAdd.AutoSize = true;
        tlpAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpAdd.Dock = DockStyle.Top;
        tlpAdd.ColumnCount = 4;
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpAdd.RowCount = 3;
        tlpAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpAdd.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlpAdd.Controls.Add(lblSummonId, 0, 0);
        tlpAdd.Controls.Add(nudSummonId, 1, 0);
        tlpAdd.Controls.Add(chkUseAddStage, 0, 1);
        tlpAdd.SetColumnSpan(chkUseAddStage, 2);
        tlpAdd.Controls.Add(lblAddStage, 2, 1);
        tlpAdd.Controls.Add(nudAddStage, 3, 1);
        tlpAdd.Controls.Add(btnAddSummon, 0, 2);
        tlpAdd.SetColumnSpan(btnAddSummon, 4);

        ConfigureLabel(lblSummonId, "Summon ID:");
        ConfigureLabel(lblAddStage, "Stage:");

        nudSummonId.Dock = DockStyle.Fill;
        nudSummonId.Margin = new Padding(3, 4, 3, 4);
        nudSummonId.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });

        chkUseAddStage.AutoSize = false;
        chkUseAddStage.Dock = DockStyle.Fill;
        chkUseAddStage.Margin = new Padding(3, 0, 3, 0);
        chkUseAddStage.Text = "Force stage";
        chkUseAddStage.UseVisualStyleBackColor = true;
        chkUseAddStage.CheckedChanged += chkUseAddStage_CheckedChanged;

        nudAddStage.Dock = DockStyle.Fill;
        nudAddStage.Margin = new Padding(3, 4, 3, 4);
        nudAddStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });

        ConfigureFillButton(btnAddSummon, "Add summon", btnAddSummon_Click);

        // gbStageSummon
        gbStageSummon.Text = "Set stage on existing";
        gbStageSummon.AutoSize = true;
        gbStageSummon.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbStageSummon.Dock = DockStyle.Top;
        gbStageSummon.Padding = new Padding(8, 18, 8, 8);
        gbStageSummon.Margin = new Padding(3, 3, 3, 6);
        gbStageSummon.Controls.Add(tlpStage);

        // tlpStage
        tlpStage.AutoSize = true;
        tlpStage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpStage.Dock = DockStyle.Top;
        tlpStage.ColumnCount = 4;
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpStage.RowCount = 2;
        tlpStage.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpStage.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpStage.Controls.Add(lblSlot, 0, 0);
        tlpStage.Controls.Add(cmbSlot, 1, 0);
        tlpStage.Controls.Add(lblStage, 2, 0);
        tlpStage.Controls.Add(nudStage, 3, 0);
        tlpStage.Controls.Add(btnStageSummon, 0, 1);
        tlpStage.SetColumnSpan(btnStageSummon, 4);

        ConfigureLabel(lblSlot, "Slot:");
        ConfigureLabel(lblStage, "Stage:");

        cmbSlot.Dock = DockStyle.Fill;
        cmbSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSlot.FormattingEnabled = true;
        cmbSlot.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5 });
        cmbSlot.Margin = new Padding(3, 4, 3, 4);

        nudStage.Dock = DockStyle.Fill;
        nudStage.Margin = new Padding(3, 4, 3, 4);
        nudStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });

        ConfigureFillButton(btnStageSummon, "Set stage", btnStageSummon_Click);

        // SummonsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbSummonActions);
        Name = "SummonsActionsControl";

        ((System.ComponentModel.ISupportInitialize)nudStage).EndInit();
        tlpStage.ResumeLayout(false);
        gbStageSummon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudAddStage).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSummonId).EndInit();
        tlpAdd.ResumeLayout(false);
        gbAddSummon.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSummonActions.ResumeLayout(false);
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
