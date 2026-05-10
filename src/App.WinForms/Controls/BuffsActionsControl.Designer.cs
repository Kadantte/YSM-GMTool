namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class BuffsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
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
    private TableLayoutPanel tlpBottom;
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
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tlpRoot = new TableLayoutPanel();
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
        tlpBottom = new TableLayoutPanel();
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

        _tlpRoot.SuspendLayout();
        gbBuffActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelected.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudStateId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).BeginInit();
        tlpBottom.SuspendLayout();
        gbGlobalBuffs.SuspendLayout();
        tlpGlobalButtons.SuspendLayout();
        gbPlayerCreature.SuspendLayout();
        tlpPlayerCreature.SuspendLayout();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbBuffActions, 0, 0);

        // gbBuffActions
        gbBuffActions.Text = "Buffs";
        gbBuffActions.AutoSize = false;
        gbBuffActions.Padding = new Padding(8, 18, 8, 8);
        gbBuffActions.Dock = DockStyle.Fill;
        gbBuffActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.ColumnCount = 1;
        tlpRoot.RowCount = 2;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbSelected, 0, 0);
        tlpRoot.Controls.Add(tlpBottom, 0, 1);

        // gbSelected
        gbSelected.Text = "Selected";
        gbSelected.AutoSize = false;
        gbSelected.Padding = new Padding(8, 18, 8, 8);
        gbSelected.Dock = DockStyle.Fill;
        gbSelected.Controls.Add(tlpSelected);

        // tlpSelected
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.ColumnCount = 4;
        tlpSelected.RowCount = 3;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tlpSelected.Controls.Add(lblStateId, 0, 0);
        tlpSelected.Controls.Add(nudStateId, 1, 0);
        tlpSelected.Controls.Add(lblBuffName, 0, 1);
        tlpSelected.Controls.Add(txtBuffName, 1, 1);
        tlpSelected.Controls.Add(lblBuffLevel, 2, 0);
        tlpSelected.Controls.Add(nudBuffLevel, 3, 0);
        tlpSelected.Controls.Add(lblDuration, 2, 1);
        tlpSelected.Controls.Add(nudDurationMinutes, 3, 1);
        tlpSelected.Controls.Add(lblDurationUnit, 3, 2);

        ConfigureLabel(lblStateId, "Buff-ID:");
        ConfigureLabel(lblBuffName, "Buffname:");
        ConfigureLabel(lblBuffLevel, "Bufflevel:");
        ConfigureLabel(lblDuration, "Duration:");
        ConfigureLabel(lblDurationUnit, "Min");

        // nudStateId
        nudStateId.Dock = DockStyle.Fill;
        nudStateId.Margin = new Padding(3, 4, 3, 4);
        nudStateId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudStateId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudStateId.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // txtBuffName
        txtBuffName.Dock = DockStyle.Fill;
        txtBuffName.Margin = new Padding(3, 4, 3, 4);
        txtBuffName.ReadOnly = true;

        // nudBuffLevel
        nudBuffLevel.Dock = DockStyle.Fill;
        nudBuffLevel.Margin = new Padding(3, 4, 3, 4);
        nudBuffLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudBuffLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudBuffLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudDurationMinutes
        nudDurationMinutes.Dock = DockStyle.Fill;
        nudDurationMinutes.Margin = new Padding(3, 4, 3, 4);
        nudDurationMinutes.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudDurationMinutes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudDurationMinutes.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // tlpBottom
        tlpBottom.Dock = DockStyle.Fill;
        tlpBottom.ColumnCount = 2;
        tlpBottom.RowCount = 1;
        tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpBottom.Controls.Add(gbGlobalBuffs, 0, 0);
        tlpBottom.Controls.Add(gbPlayerCreature, 1, 0);

        // gbGlobalBuffs
        gbGlobalBuffs.Text = "Global Buffs";
        gbGlobalBuffs.AutoSize = false;
        gbGlobalBuffs.Padding = new Padding(8, 18, 8, 8);
        gbGlobalBuffs.Dock = DockStyle.Fill;
        gbGlobalBuffs.Controls.Add(tlpGlobalButtons);

        // tlpGlobalButtons
        tlpGlobalButtons.Dock = DockStyle.Fill;
        tlpGlobalButtons.ColumnCount = 1;
        tlpGlobalButtons.RowCount = 4;
        tlpGlobalButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpGlobalButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpGlobalButtons.Controls.Add(btnAddTimedWorldState, 0, 0);
        tlpGlobalButtons.Controls.Add(btnAddEventState, 0, 1);
        tlpGlobalButtons.Controls.Add(btnRemoveEventState, 0, 2);

        ConfigureGridButton(btnAddTimedWorldState, "Add Timed World State", btnAddTimedWorldState_Click);
        ConfigureGridButton(btnAddEventState, "Add Event State", btnAddEventState_Click);
        ConfigureGridButton(btnRemoveEventState, "Remove Event State", btnRemoveEventState_Click);

        // gbPlayerCreature
        gbPlayerCreature.Text = "Player/Creature Buffs";
        gbPlayerCreature.AutoSize = false;
        gbPlayerCreature.Padding = new Padding(8, 18, 8, 8);
        gbPlayerCreature.Dock = DockStyle.Fill;
        gbPlayerCreature.Controls.Add(tlpPlayerCreature);

        // tlpPlayerCreature
        tlpPlayerCreature.Dock = DockStyle.Fill;
        tlpPlayerCreature.ColumnCount = 1;
        tlpPlayerCreature.RowCount = 5;
        tlpPlayerCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPlayerCreature.Controls.Add(rbPlayer, 0, 0);
        tlpPlayerCreature.Controls.Add(rbSummon, 0, 1);
        tlpPlayerCreature.Controls.Add(btnAddBuff, 0, 2);
        tlpPlayerCreature.Controls.Add(btnRemoveBuff, 0, 3);

        // rbPlayer
        rbPlayer.AutoSize = true;
        rbPlayer.Anchor = AnchorStyles.Left;
        rbPlayer.Text = "Player";
        rbPlayer.UseVisualStyleBackColor = true;

        // rbSummon
        rbSummon.AutoSize = true;
        rbSummon.Anchor = AnchorStyles.Left;
        rbSummon.Text = "Summon";
        rbSummon.UseVisualStyleBackColor = true;

        ConfigureGridButton(btnAddBuff, "Add Buff", btnAddBuff_Click);
        ConfigureGridButton(btnRemoveBuff, "Remove Buff", btnRemoveBuff_Click);

        // BuffsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        Name = "BuffsActionsControl";
        Size = new Size(430, 330);

        tlpPlayerCreature.ResumeLayout(false);
        tlpPlayerCreature.PerformLayout();
        gbPlayerCreature.ResumeLayout(false);
        tlpGlobalButtons.ResumeLayout(false);
        gbGlobalBuffs.ResumeLayout(false);
        tlpBottom.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudStateId).EndInit();
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        gbSelected.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbBuffActions.ResumeLayout(false);
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
