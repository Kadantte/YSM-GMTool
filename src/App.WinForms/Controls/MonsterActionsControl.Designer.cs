namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class MonsterActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbMonsterActions;
    private TableLayoutPanel tlp;
    private Label lblSpawnMode;
    private ComboBox cmbSpawnMode;
    private Label lblMonsterId;
    private NumericUpDown nudMonsterId;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblX;
    private NumericUpDown nudX;
    private Label lblY;
    private NumericUpDown nudY;
    private Label lblLayer;
    private NumericUpDown nudLayer;
    private CheckBox chkUseLifetime;
    private Label lblMinutes;
    private NumericUpDown nudMinutesLifetime;
    private Button btnCreateSpawnCommand;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbMonsterActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblSpawnMode = new Label();
        cmbSpawnMode = new ComboBox();
        lblMonsterId = new Label();
        nudMonsterId = new NumericUpDown();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblX = new Label();
        nudX = new NumericUpDown();
        lblY = new Label();
        nudY = new NumericUpDown();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        chkUseLifetime = new CheckBox();
        lblMinutes = new Label();
        nudMinutesLifetime = new NumericUpDown();
        btnCreateSpawnCommand = new Button();

        gbMonsterActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudMonsterId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).BeginInit();
        SuspendLayout();

        // gbMonsterActions
        gbMonsterActions.Text = "Monster";
        gbMonsterActions.AutoSize = true;
        gbMonsterActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbMonsterActions.Dock = DockStyle.Top;
        gbMonsterActions.Padding = new Padding(8, 18, 8, 8);
        gbMonsterActions.Margin = new Padding(0);
        gbMonsterActions.Controls.Add(tlp);

        // tlp
        tlp.AutoSize = true;
        tlp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlp.Dock = DockStyle.Top;
        tlp.ColumnCount = 4;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlp.RowCount = 6;
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlp.Controls.Add(lblSpawnMode, 0, 0);
        tlp.Controls.Add(cmbSpawnMode, 1, 0);
        tlp.SetColumnSpan(cmbSpawnMode, 3);
        tlp.Controls.Add(lblMonsterId, 0, 1);
        tlp.Controls.Add(nudMonsterId, 1, 1);
        tlp.Controls.Add(lblAmount, 2, 1);
        tlp.Controls.Add(nudAmount, 3, 1);
        tlp.Controls.Add(lblX, 0, 2);
        tlp.Controls.Add(nudX, 1, 2);
        tlp.Controls.Add(lblY, 2, 2);
        tlp.Controls.Add(nudY, 3, 2);
        tlp.Controls.Add(lblLayer, 0, 3);
        tlp.Controls.Add(nudLayer, 1, 3);
        tlp.Controls.Add(chkUseLifetime, 2, 3);
        tlp.Controls.Add(lblMinutes, 0, 4);
        tlp.Controls.Add(nudMinutesLifetime, 1, 4);
        tlp.Controls.Add(btnCreateSpawnCommand, 0, 5);
        tlp.SetColumnSpan(btnCreateSpawnCommand, 4);

        ConfigureLabel(lblSpawnMode, "Spawntype:");
        ConfigureLabel(lblMonsterId, "ID:");
        ConfigureLabel(lblAmount, "Amount:");
        ConfigureLabel(lblX, "X:");
        ConfigureLabel(lblY, "Y:");
        ConfigureLabel(lblLayer, "Layer:");
        ConfigureLabel(lblMinutes, "Minutes:");

        cmbSpawnMode.Dock = DockStyle.Fill;
        cmbSpawnMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSpawnMode.FormattingEnabled = true;
        cmbSpawnMode.Items.AddRange(new object[] { "At your place", "At selected player place", "At specific coordinates" });
        cmbSpawnMode.Margin = new Padding(3, 4, 3, 4);
        cmbSpawnMode.SelectedIndexChanged += cmbSpawnMode_SelectedIndexChanged;

        ConfigureNumeric(nudMonsterId, 1, 1_000_000_000, 1);
        ConfigureNumeric(nudAmount, 1, 10_000, 1);
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
        nudLayer.Value = new decimal(new int[] { 1, 0, 0, 0 });

        chkUseLifetime.AutoSize = false;
        chkUseLifetime.Dock = DockStyle.Fill;
        chkUseLifetime.Margin = new Padding(3, 0, 3, 0);
        chkUseLifetime.Text = "Lifetime:";
        chkUseLifetime.UseVisualStyleBackColor = true;
        chkUseLifetime.CheckedChanged += chkUseLifetime_CheckedChanged;

        nudMinutesLifetime.Dock = DockStyle.Fill;
        nudMinutesLifetime.Margin = new Padding(3, 4, 3, 4);
        nudMinutesLifetime.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudMinutesLifetime.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
        nudMinutesLifetime.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });

        ConfigureFillButton(btnCreateSpawnCommand, "Create Command", btnCreateSpawnCommand_Click);

        // MonsterActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbMonsterActions);
        Name = "MonsterActionsControl";

        ((System.ComponentModel.ISupportInitialize)nudMonsterId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).EndInit();
        tlp.ResumeLayout(false);
        gbMonsterActions.ResumeLayout(false);
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
