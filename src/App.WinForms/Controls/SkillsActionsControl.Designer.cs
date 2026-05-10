namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class SkillsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
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
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tlpRoot = new TableLayoutPanel();
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

        _tlpRoot.SuspendLayout();
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

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbSkillsActions, 0, 0);

        // gbSkillsActions
        gbSkillsActions.Text = "Skills";
        gbSkillsActions.AutoSize = false;
        gbSkillsActions.Padding = new Padding(8, 18, 8, 8);
        gbSkillsActions.Dock = DockStyle.Fill;
        gbSkillsActions.Controls.Add(tlpRoot);

        // tlpRoot
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.ColumnCount = 1;
        tlpRoot.RowCount = 4;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbSelected, 0, 0);
        tlpRoot.Controls.Add(gbPlayerSkills, 0, 1);
        tlpRoot.Controls.Add(gbCreatureSkills, 0, 2);

        // gbSelected
        gbSelected.Text = "Selected";
        gbSelected.AutoSize = false;
        gbSelected.Padding = new Padding(8, 18, 8, 8);
        gbSelected.Dock = DockStyle.Fill;
        gbSelected.Controls.Add(tlpSelected);

        // tlpSelected
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.ColumnCount = 4;
        tlpSelected.RowCount = 1;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpSelected.Controls.Add(lblSkillId, 0, 0);
        tlpSelected.Controls.Add(nudSkillId, 1, 0);
        tlpSelected.Controls.Add(lblSkillLevel, 2, 0);
        tlpSelected.Controls.Add(nudSkillLevel, 3, 0);

        ConfigureLabel(lblSkillId, "ID:");
        ConfigureLabel(lblSkillLevel, "Level:");

        // nudSkillId
        nudSkillId.Dock = DockStyle.Fill;
        nudSkillId.Margin = new Padding(3, 4, 3, 4);
        nudSkillId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudSkillId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSkillId.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudSkillLevel
        nudSkillLevel.Dock = DockStyle.Fill;
        nudSkillLevel.Margin = new Padding(3, 4, 3, 4);
        nudSkillLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudSkillLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSkillLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // gbPlayerSkills
        gbPlayerSkills.Text = "Player Skills";
        gbPlayerSkills.AutoSize = false;
        gbPlayerSkills.Padding = new Padding(8, 18, 8, 8);
        gbPlayerSkills.Dock = DockStyle.Fill;
        gbPlayerSkills.Controls.Add(tlpPlayerSkills);

        // tlpPlayerSkills (2x2 grid of buttons)
        tlpPlayerSkills.Dock = DockStyle.Fill;
        tlpPlayerSkills.ColumnCount = 2;
        tlpPlayerSkills.RowCount = 2;
        tlpPlayerSkills.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerSkills.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpPlayerSkills.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerSkills.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpPlayerSkills.Controls.Add(btnLearnSkill, 0, 0);
        tlpPlayerSkills.Controls.Add(btnSetSkill, 1, 0);
        tlpPlayerSkills.Controls.Add(btnRemoveSkill, 0, 1);
        tlpPlayerSkills.Controls.Add(btnLearnAllJobSkills, 1, 1);

        ConfigureGridButton(btnLearnSkill, "Learn skill", btnLearnSkill_Click);
        ConfigureGridButton(btnSetSkill, "Set skill level", btnSetSkill_Click);
        ConfigureGridButton(btnRemoveSkill, "Remove skill", btnRemoveSkill_Click);
        ConfigureGridButton(btnLearnAllJobSkills, "Learn all skill", btnLearnAllJobSkills_Click);

        // gbCreatureSkills
        gbCreatureSkills.Text = "Creature Skills";
        gbCreatureSkills.AutoSize = false;
        gbCreatureSkills.Padding = new Padding(8, 18, 8, 8);
        gbCreatureSkills.Dock = DockStyle.Fill;
        gbCreatureSkills.Controls.Add(tlpCreature);

        // tlpCreature
        tlpCreature.Dock = DockStyle.Fill;
        tlpCreature.ColumnCount = 2;
        tlpCreature.RowCount = 3;
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
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

        // nudCreatureSlotIndex
        nudCreatureSlotIndex.Dock = DockStyle.Fill;
        nudCreatureSlotIndex.Margin = new Padding(3, 4, 3, 4);
        nudCreatureSlotIndex.Maximum = new decimal(new int[] { 10, 0, 0, 0 });

        ConfigureGridButton(btnLearnCreatureSkill, "Learn creature skill", btnLearnCreatureSkill_Click);
        ConfigureGridButton(btnLearnCreatureAllSkill, "Learn creature all skill", btnLearnCreatureAllSkill_Click);

        // SkillsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        Name = "SkillsActionsControl";
        Size = new Size(430, 390);

        ((System.ComponentModel.ISupportInitialize)nudCreatureSlotIndex).EndInit();
        tlpCreature.ResumeLayout(false);
        tlpCreature.PerformLayout();
        gbCreatureSkills.ResumeLayout(false);
        tlpPlayerSkills.ResumeLayout(false);
        gbPlayerSkills.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudSkillLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSkillId).EndInit();
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        gbSelected.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSkillsActions.ResumeLayout(false);
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
