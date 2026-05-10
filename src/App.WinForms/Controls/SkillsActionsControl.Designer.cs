namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class SkillsActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbSkillsActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelected;
    private TableLayoutPanel tlpSelected;
    private Label lblSkillId;
    private NumericUpDown nudSkillId;
    private Label lblSkillLevel;
    private NumericUpDown nudSkillLevel;
    private GroupBox gbPlayerSkills;
    private TableLayoutPanel tlpPlayerSkills;
    private Button btnLearnSkill;
    private Button btnSetSkill;
    private Button btnRemoveSkill;
    private Button btnLearnAllJobSkills;
    private GroupBox gbCreatureSkills;
    private TableLayoutPanel tlpCreature;
    private Label lblCreatureSlotIndex;
    private NumericUpDown nudCreatureSlotIndex;
    private Button btnLearnCreatureSkill;
    private Button btnLearnCreatureAllSkill;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbSkillsActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbSelected = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblSkillId = new Label();
        nudSkillId = new NumericUpDown();
        lblSkillLevel = new Label();
        nudSkillLevel = new NumericUpDown();
        gbPlayerSkills = new GroupBox();
        tlpPlayerSkills = new TableLayoutPanel();
        btnLearnSkill = new Button();
        btnSetSkill = new Button();
        btnRemoveSkill = new Button();
        btnLearnAllJobSkills = new Button();
        gbCreatureSkills = new GroupBox();
        tlpCreature = new TableLayoutPanel();
        lblCreatureSlotIndex = new Label();
        nudCreatureSlotIndex = new NumericUpDown();
        btnLearnCreatureSkill = new Button();
        btnLearnCreatureAllSkill = new Button();

        gbSkillsActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelected.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSkillId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudSkillLevel).BeginInit();
        gbPlayerSkills.SuspendLayout();
        tlpPlayerSkills.SuspendLayout();
        gbCreatureSkills.SuspendLayout();
        tlpCreature.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudCreatureSlotIndex).BeginInit();
        SuspendLayout();

        // gbSkillsActions (outer wrapper)
        gbSkillsActions.Text = "Skills";
        gbSkillsActions.AutoSize = true;
        gbSkillsActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSkillsActions.Dock = DockStyle.Top;
        gbSkillsActions.Padding = new Padding(8, 18, 8, 8);
        gbSkillsActions.Margin = new Padding(0);
        gbSkillsActions.Controls.Add(tlpRoot);

        // tlpRoot (3 sections stacked)
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
        tlpRoot.Controls.Add(gbPlayerSkills, 0, 1);
        tlpRoot.Controls.Add(gbCreatureSkills, 0, 2);

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
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.RowCount = 1;
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.Controls.Add(lblSkillId, 0, 0);
        tlpSelected.Controls.Add(nudSkillId, 1, 0);
        tlpSelected.Controls.Add(lblSkillLevel, 2, 0);
        tlpSelected.Controls.Add(nudSkillLevel, 3, 0);

        ConfigureLabel(lblSkillId, "ID:");
        ConfigureLabel(lblSkillLevel, "Level:");
        ConfigureNumeric(nudSkillId, 1, 1_000_000_000, 1);
        ConfigureNumeric(nudSkillLevel, 1, 999, 1);

        // gbPlayerSkills
        gbPlayerSkills.Text = "Player Skills";
        gbPlayerSkills.AutoSize = true;
        gbPlayerSkills.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbPlayerSkills.Dock = DockStyle.Top;
        gbPlayerSkills.Padding = new Padding(8, 18, 8, 8);
        gbPlayerSkills.Margin = new Padding(3, 3, 3, 6);
        gbPlayerSkills.Controls.Add(tlpPlayerSkills);

        // tlpPlayerSkills (2x2 buttons)
        tlpPlayerSkills.AutoSize = true;
        tlpPlayerSkills.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpPlayerSkills.Dock = DockStyle.Top;
        tlpPlayerSkills.ColumnCount = 2;
        tlpPlayerSkills.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerSkills.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerSkills.RowCount = 2;
        tlpPlayerSkills.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerSkills.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerSkills.Controls.Add(btnLearnSkill, 0, 0);
        tlpPlayerSkills.Controls.Add(btnSetSkill, 1, 0);
        tlpPlayerSkills.Controls.Add(btnRemoveSkill, 0, 1);
        tlpPlayerSkills.Controls.Add(btnLearnAllJobSkills, 1, 1);

        ConfigureFillButton(btnLearnSkill, "Learn skill", btnLearnSkill_Click);
        ConfigureFillButton(btnSetSkill, "Set skill level", btnSetSkill_Click);
        ConfigureFillButton(btnRemoveSkill, "Remove skill", btnRemoveSkill_Click);
        ConfigureFillButton(btnLearnAllJobSkills, "Learn all skills", btnLearnAllJobSkills_Click);

        // gbCreatureSkills
        gbCreatureSkills.Text = "Creature Skills";
        gbCreatureSkills.AutoSize = true;
        gbCreatureSkills.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbCreatureSkills.Dock = DockStyle.Top;
        gbCreatureSkills.Padding = new Padding(8, 18, 8, 8);
        gbCreatureSkills.Margin = new Padding(3, 3, 3, 6);
        gbCreatureSkills.Controls.Add(tlpCreature);

        // tlpCreature
        tlpCreature.AutoSize = true;
        tlpCreature.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpCreature.Dock = DockStyle.Top;
        tlpCreature.ColumnCount = 2;
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCreature.RowCount = 3;
        tlpCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCreature.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpCreature.Controls.Add(lblCreatureSlotIndex, 0, 0);
        tlpCreature.Controls.Add(nudCreatureSlotIndex, 1, 0);
        tlpCreature.Controls.Add(btnLearnCreatureSkill, 0, 1);
        tlpCreature.Controls.Add(btnLearnCreatureAllSkill, 0, 2);
        tlpCreature.SetColumnSpan(btnLearnCreatureSkill, 2);
        tlpCreature.SetColumnSpan(btnLearnCreatureAllSkill, 2);

        ConfigureLabel(lblCreatureSlotIndex, "Creature slot (0=all):");
        ConfigureNumeric(nudCreatureSlotIndex, 0, 10, 0);
        ConfigureFillButton(btnLearnCreatureSkill, "Learn creature skill", btnLearnCreatureSkill_Click);
        ConfigureFillButton(btnLearnCreatureAllSkill, "Learn all creature skills", btnLearnCreatureAllSkill_Click);

        // SkillsActionsControl itself
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbSkillsActions);
        Name = "SkillsActionsControl";

        ((System.ComponentModel.ISupportInitialize)nudCreatureSlotIndex).EndInit();
        tlpCreature.ResumeLayout(false);
        gbCreatureSkills.ResumeLayout(false);
        tlpPlayerSkills.ResumeLayout(false);
        gbPlayerSkills.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudSkillLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSkillId).EndInit();
        tlpSelected.ResumeLayout(false);
        gbSelected.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSkillsActions.ResumeLayout(false);
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
