namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class MonsterActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
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
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _tlpRoot = new TableLayoutPanel();
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

        _tlpRoot.SuspendLayout();
        gbMonsterActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudMonsterId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).BeginInit();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbMonsterActions, 0, 0);

        // gbMonsterActions
        gbMonsterActions.Text = "Monster";
        gbMonsterActions.AutoSize = false;
        gbMonsterActions.Padding = new Padding(8, 18, 8, 8);
        gbMonsterActions.Dock = DockStyle.Fill;
        gbMonsterActions.Controls.Add(tlp);

        // tlp
        tlp.Dock = DockStyle.Fill;
        tlp.ColumnCount = 2;
        tlp.RowCount = 9;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 8; i++)
        {
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        }
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        tlp.Controls.Add(lblSpawnMode, 0, 0);
        tlp.Controls.Add(cmbSpawnMode, 1, 0);
        tlp.Controls.Add(lblMonsterId, 0, 1);
        tlp.Controls.Add(nudMonsterId, 1, 1);
        tlp.Controls.Add(lblAmount, 0, 2);
        tlp.Controls.Add(nudAmount, 1, 2);
        tlp.Controls.Add(lblX, 0, 3);
        tlp.Controls.Add(nudX, 1, 3);
        tlp.Controls.Add(lblY, 0, 4);
        tlp.Controls.Add(nudY, 1, 4);
        tlp.Controls.Add(lblLayer, 0, 5);
        tlp.Controls.Add(nudLayer, 1, 5);
        tlp.Controls.Add(chkUseLifetime, 0, 6);
        tlp.Controls.Add(lblMinutes, 0, 7);
        tlp.Controls.Add(nudMinutesLifetime, 1, 7);
        tlp.Controls.Add(btnCreateSpawnCommand, 0, 8);
        tlp.SetColumnSpan(btnCreateSpawnCommand, 2);

        ConfigureLabel(lblSpawnMode, "Spawntype:");
        ConfigureLabel(lblMonsterId, "ID:");
        ConfigureLabel(lblAmount, "Amount:");
        ConfigureLabel(lblX, "X:");
        ConfigureLabel(lblY, "Y:");
        ConfigureLabel(lblLayer, "Layer:");
        ConfigureLabel(lblMinutes, "Minutes:");

        // cmbSpawnMode
        cmbSpawnMode.Dock = DockStyle.Fill;
        cmbSpawnMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSpawnMode.FormattingEnabled = true;
        cmbSpawnMode.Items.AddRange(new object[] { "At your place", "At selected player place", "At specific coordinates" });
        cmbSpawnMode.Margin = new Padding(3, 4, 3, 4);
        cmbSpawnMode.SelectedIndexChanged += cmbSpawnMode_SelectedIndexChanged;

        // nudMonsterId
        nudMonsterId.Dock = DockStyle.Fill;
        nudMonsterId.Margin = new Padding(3, 4, 3, 4);
        nudMonsterId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudMonsterId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudMonsterId.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudAmount
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Margin = new Padding(3, 4, 3, 4);
        nudAmount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudX
        nudX.Dock = DockStyle.Fill;
        nudX.Margin = new Padding(3, 4, 3, 4);
        nudX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });

        // nudY
        nudY.Dock = DockStyle.Fill;
        nudY.Margin = new Padding(3, 4, 3, 4);
        nudY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });

        // nudLayer
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Margin = new Padding(3, 4, 3, 4);
        nudLayer.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
        nudLayer.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // chkUseLifetime
        chkUseLifetime.Anchor = AnchorStyles.Left;
        chkUseLifetime.AutoSize = true;
        chkUseLifetime.Text = "Lifetime:";
        chkUseLifetime.UseVisualStyleBackColor = true;
        chkUseLifetime.CheckedChanged += chkUseLifetime_CheckedChanged;

        // nudMinutesLifetime
        nudMinutesLifetime.Dock = DockStyle.Fill;
        nudMinutesLifetime.Margin = new Padding(3, 4, 3, 4);
        nudMinutesLifetime.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudMinutesLifetime.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
        nudMinutesLifetime.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });

        // btnCreateSpawnCommand
        btnCreateSpawnCommand.Text = "Create Command";
        btnCreateSpawnCommand.AutoSize = false;
        btnCreateSpawnCommand.Size = new Size(100, 28);
        btnCreateSpawnCommand.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnCreateSpawnCommand.Margin = new Padding(3);
        btnCreateSpawnCommand.UseVisualStyleBackColor = true;
        btnCreateSpawnCommand.Click += btnCreateSpawnCommand_Click;

        // MonsterActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        Name = "MonsterActionsControl";
        Size = new Size(430, 370);

        ((System.ComponentModel.ISupportInitialize)nudMonsterId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).EndInit();
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        gbMonsterActions.ResumeLayout(false);
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
