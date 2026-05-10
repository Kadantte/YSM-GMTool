namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class BuffsActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbBuffActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelected;
    private TableLayoutPanel tlpSelected;
    private Label lblStateId;
    private NumericUpDown nudStateId;
    private Label lblBuffName;
    private TextBox txtBuffName;
    private Label lblBuffLevel;
    private NumericUpDown nudBuffLevel;
    private Label lblDuration;
    private NumericUpDown nudDurationMinutes;
    private Label lblDurationUnit;
    private GroupBox gbGlobalBuffs;
    private TableLayoutPanel tlpGlobalButtons;
    private Button btnAddTimedWorldState;
    private Button btnAddEventState;
    private Button btnRemoveEventState;
    private GroupBox gbPlayerCreature;
    private TableLayoutPanel tlpPlayerCreature;
    private RadioButton rbPlayer;
    private RadioButton rbSummon;
    private Button btnAddBuff;
    private Button btnRemoveBuff;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbBuffActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbSelected = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblStateId = new Label();
        nudStateId = new NumericUpDown();
        lblBuffName = new Label();
        txtBuffName = new TextBox();
        lblBuffLevel = new Label();
        nudBuffLevel = new NumericUpDown();
        lblDuration = new Label();
        nudDurationMinutes = new NumericUpDown();
        lblDurationUnit = new Label();
        gbGlobalBuffs = new GroupBox();
        tlpGlobalButtons = new TableLayoutPanel();
        btnAddTimedWorldState = new Button();
        btnAddEventState = new Button();
        btnRemoveEventState = new Button();
        gbPlayerCreature = new GroupBox();
        tlpPlayerCreature = new TableLayoutPanel();
        rbPlayer = new RadioButton();
        rbSummon = new RadioButton();
        btnAddBuff = new Button();
        btnRemoveBuff = new Button();

        gbBuffActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelected.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudStateId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).BeginInit();
        gbGlobalBuffs.SuspendLayout();
        tlpGlobalButtons.SuspendLayout();
        gbPlayerCreature.SuspendLayout();
        tlpPlayerCreature.SuspendLayout();
        SuspendLayout();

        // gbBuffActions (outer wrapper)
        gbBuffActions.Text = "Buffs";
        gbBuffActions.AutoSize = true;
        gbBuffActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbBuffActions.Dock = DockStyle.Top;
        gbBuffActions.Padding = new Padding(8, 18, 8, 8);
        gbBuffActions.Margin = new Padding(0);
        gbBuffActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.AutoSize = true;
        tlpRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpRoot.Dock = DockStyle.Top;
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowCount = 3;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.Controls.Add(gbSelected, 0, 0);
        tlpRoot.Controls.Add(gbGlobalBuffs, 0, 1);
        tlpRoot.Controls.Add(gbPlayerCreature, 0, 2);

        // gbSelected
        gbSelected.Text = "Selected";
        gbSelected.AutoSize = true;
        gbSelected.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSelected.Dock = DockStyle.Top;
        gbSelected.Padding = new Padding(8, 18, 8, 8);
        gbSelected.Margin = new Padding(3, 3, 3, 6);
        gbSelected.Controls.Add(tlpSelected);

        // tlpSelected
        tlpSelected.AutoSize = true;
        tlpSelected.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpSelected.Dock = DockStyle.Top;
        tlpSelected.ColumnCount = 4;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.RowCount = 3;
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.Controls.Add(lblStateId, 0, 0);
        tlpSelected.Controls.Add(nudStateId, 1, 0);
        tlpSelected.SetColumnSpan(nudStateId, 3);
        tlpSelected.Controls.Add(lblBuffName, 0, 1);
        tlpSelected.Controls.Add(txtBuffName, 1, 1);
        tlpSelected.SetColumnSpan(txtBuffName, 3);
        tlpSelected.Controls.Add(lblBuffLevel, 0, 2);
        tlpSelected.Controls.Add(nudBuffLevel, 1, 2);
        tlpSelected.Controls.Add(lblDuration, 2, 2);
        tlpSelected.Controls.Add(nudDurationMinutes, 3, 2);

        ConfigureLabel(lblStateId, "State ID:");
        ConfigureLabel(lblBuffName, "Buff name:");
        ConfigureLabel(lblBuffLevel, "Level:");
        ConfigureLabel(lblDuration, "Duration (min):");
        lblDurationUnit.Visible = false;

        ConfigureNumeric(nudStateId, 1, 1_000_000_000, 1);
        ConfigureNumeric(nudBuffLevel, 1, 999, 1);
        ConfigureNumeric(nudDurationMinutes, 1, 100_000, 1);

        txtBuffName.Dock = DockStyle.Fill;
        txtBuffName.Margin = new Padding(3, 4, 3, 4);
        txtBuffName.ReadOnly = true;

        // gbGlobalBuffs
        gbGlobalBuffs.Text = "Global / world buffs";
        gbGlobalBuffs.AutoSize = true;
        gbGlobalBuffs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbGlobalBuffs.Dock = DockStyle.Top;
        gbGlobalBuffs.Padding = new Padding(8, 18, 8, 8);
        gbGlobalBuffs.Margin = new Padding(3, 3, 3, 6);
        gbGlobalBuffs.Controls.Add(tlpGlobalButtons);

        // tlpGlobalButtons
        tlpGlobalButtons.AutoSize = true;
        tlpGlobalButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpGlobalButtons.Dock = DockStyle.Top;
        tlpGlobalButtons.ColumnCount = 1;
        tlpGlobalButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpGlobalButtons.RowCount = 3;
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.Controls.Add(btnAddTimedWorldState, 0, 0);
        tlpGlobalButtons.Controls.Add(btnAddEventState, 0, 1);
        tlpGlobalButtons.Controls.Add(btnRemoveEventState, 0, 2);

        ConfigureFillButton(btnAddTimedWorldState, "Add timed world state", btnAddTimedWorldState_Click);
        ConfigureFillButton(btnAddEventState, "Add event state", btnAddEventState_Click);
        ConfigureFillButton(btnRemoveEventState, "Remove event state", btnRemoveEventState_Click);

        // gbPlayerCreature
        gbPlayerCreature.Text = "Player / creature buffs";
        gbPlayerCreature.AutoSize = true;
        gbPlayerCreature.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbPlayerCreature.Dock = DockStyle.Top;
        gbPlayerCreature.Padding = new Padding(8, 18, 8, 8);
        gbPlayerCreature.Margin = new Padding(3, 3, 3, 6);
        gbPlayerCreature.Controls.Add(tlpPlayerCreature);

        // tlpPlayerCreature
        tlpPlayerCreature.AutoSize = true;
        tlpPlayerCreature.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpPlayerCreature.Dock = DockStyle.Top;
        tlpPlayerCreature.ColumnCount = 2;
        tlpPlayerCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerCreature.RowCount = 2;
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerCreature.Controls.Add(rbPlayer, 0, 0);
        tlpPlayerCreature.Controls.Add(rbSummon, 1, 0);
        tlpPlayerCreature.Controls.Add(btnAddBuff, 0, 1);
        tlpPlayerCreature.Controls.Add(btnRemoveBuff, 1, 1);

        ConfigureRadio(rbPlayer, "Player");
        ConfigureRadio(rbSummon, "Summon/Creature");
        ConfigureFillButton(btnAddBuff, "Add buff", btnAddBuff_Click);
        ConfigureFillButton(btnRemoveBuff, "Remove buff", btnRemoveBuff_Click);

        // BuffsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbBuffActions);
        Name = "BuffsActionsControl";

        tlpPlayerCreature.ResumeLayout(false);
        gbPlayerCreature.ResumeLayout(false);
        tlpGlobalButtons.ResumeLayout(false);
        gbGlobalBuffs.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudStateId).EndInit();
        tlpSelected.ResumeLayout(false);
        gbSelected.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbBuffActions.ResumeLayout(false);
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

    private static void ConfigureRadio(RadioButton rb, string text)
    {
        rb.Text = text;
        rb.AutoSize = false;
        rb.Dock = DockStyle.Fill;
        rb.Margin = new Padding(3, 0, 3, 0);
        rb.UseVisualStyleBackColor = true;
    }
}
