namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class ItemsActionsControl
{
    private System.ComponentModel.IContainer components = null;

    private GroupBox gbItemActions;
    // The following tab fields are retained because ItemsActionsControl.cs references them.
    // They are not added to any container in the new layout.
    private TabControl tabItemsActions;
    private TabPage tabItem;
    private TabPage tabRandomOption;
    private TabPage tabItemUseFlag;
    private Label lblRandomOptionPlaceholder;
    private Label lblItemUseFlagPlaceholder;

    private TableLayoutPanel tlpItemRoot;
    private GroupBox gbInsertItem;
    private TableLayoutPanel tlpInsert;
    private Label lblItemId;
    private NumericUpDown nudItemId;
    private Label lblItemName;
    private TextBox txtItemName;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblEnhance;
    private NumericUpDown nudEnhance;
    private Label lblLevel;
    private NumericUpDown nudLevel;
    private CheckBox chkUseStatusFlag;
    private NumericUpDown nudStatusFlag;
    private Button btnAddYourself;
    private Button btnGiveOtherPlayer;
    private GroupBox gbModifyItem;
    private TableLayoutPanel tlpModify;
    private Label lblWearSlot;
    private ComboBox cmbWearSlot;
    private RadioButton rbTargetOwn;
    private RadioButton rbTargetOther;
    private Label lblModifyLevel;
    private NumericUpDown nudModifyLevel;
    private Label lblModifyEnhance;
    private NumericUpDown nudModifyEnhance;
    private Label lblModifyItemCode;
    private NumericUpDown nudModifyItemCode;
    private Button btnEditLevel;
    private Button btnEditEnhance;
    private Button btnChangeAppearance;
    private Button btnChangeItemCode;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbItemActions = new GroupBox();
        tabItemsActions = new TabControl();
        tabItem = new TabPage();
        tabRandomOption = new TabPage();
        tabItemUseFlag = new TabPage();
        lblRandomOptionPlaceholder = new Label();
        lblItemUseFlagPlaceholder = new Label();
        tlpItemRoot = new TableLayoutPanel();
        gbInsertItem = new GroupBox();
        tlpInsert = new TableLayoutPanel();
        lblItemId = new Label();
        nudItemId = new NumericUpDown();
        lblItemName = new Label();
        txtItemName = new TextBox();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblEnhance = new Label();
        nudEnhance = new NumericUpDown();
        lblLevel = new Label();
        nudLevel = new NumericUpDown();
        chkUseStatusFlag = new CheckBox();
        nudStatusFlag = new NumericUpDown();
        btnAddYourself = new Button();
        btnGiveOtherPlayer = new Button();
        gbModifyItem = new GroupBox();
        tlpModify = new TableLayoutPanel();
        lblWearSlot = new Label();
        cmbWearSlot = new ComboBox();
        rbTargetOwn = new RadioButton();
        rbTargetOther = new RadioButton();
        lblModifyLevel = new Label();
        nudModifyLevel = new NumericUpDown();
        lblModifyEnhance = new Label();
        nudModifyEnhance = new NumericUpDown();
        lblModifyItemCode = new Label();
        nudModifyItemCode = new NumericUpDown();
        btnEditLevel = new Button();
        btnEditEnhance = new Button();
        btnChangeAppearance = new Button();
        btnChangeItemCode = new Button();

        gbItemActions.SuspendLayout();
        tlpItemRoot.SuspendLayout();
        gbInsertItem.SuspendLayout();
        tlpInsert.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudItemId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudStatusFlag).BeginInit();
        gbModifyItem.SuspendLayout();
        tlpModify.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudModifyLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyEnhance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyItemCode).BeginInit();
        SuspendLayout();

        // Hide unused placeholder labels (kept for compatibility with .cs file).
        lblRandomOptionPlaceholder.Visible = false;
        lblItemUseFlagPlaceholder.Visible = false;

        // gbItemActions (outer wrapper)
        gbItemActions.Text = "Items";
        gbItemActions.AutoSize = true;
        gbItemActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbItemActions.Dock = DockStyle.Top;
        gbItemActions.Padding = new Padding(8, 18, 8, 8);
        gbItemActions.Margin = new Padding(0);
        gbItemActions.Controls.Add(tlpItemRoot);

        // tlpItemRoot
        tlpItemRoot.AutoSize = true;
        tlpItemRoot.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpItemRoot.Dock = DockStyle.Top;
        tlpItemRoot.ColumnCount = 1;
        tlpItemRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpItemRoot.RowCount = 2;
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpItemRoot.Controls.Add(gbInsertItem, 0, 0);
        tlpItemRoot.Controls.Add(gbModifyItem, 0, 1);

        // gbInsertItem
        gbInsertItem.Text = "Insert item";
        gbInsertItem.AutoSize = true;
        gbInsertItem.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbInsertItem.Dock = DockStyle.Top;
        gbInsertItem.Padding = new Padding(8, 18, 8, 8);
        gbInsertItem.Margin = new Padding(3, 3, 3, 6);
        gbInsertItem.Controls.Add(tlpInsert);

        // tlpInsert
        tlpInsert.AutoSize = true;
        tlpInsert.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpInsert.Dock = DockStyle.Top;
        tlpInsert.ColumnCount = 4;
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInsert.RowCount = 5;
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlpInsert.Controls.Add(lblItemId, 0, 0);
        tlpInsert.Controls.Add(nudItemId, 1, 0);
        tlpInsert.Controls.Add(lblItemName, 0, 1);
        tlpInsert.Controls.Add(txtItemName, 1, 1);
        tlpInsert.SetColumnSpan(txtItemName, 3);
        tlpInsert.Controls.Add(lblAmount, 0, 2);
        tlpInsert.Controls.Add(nudAmount, 1, 2);
        tlpInsert.Controls.Add(lblEnhance, 2, 2);
        tlpInsert.Controls.Add(nudEnhance, 3, 2);
        tlpInsert.Controls.Add(lblLevel, 0, 3);
        tlpInsert.Controls.Add(nudLevel, 1, 3);
        tlpInsert.Controls.Add(chkUseStatusFlag, 2, 3);
        tlpInsert.Controls.Add(nudStatusFlag, 3, 3);
        tlpInsert.Controls.Add(btnAddYourself, 0, 4);
        tlpInsert.SetColumnSpan(btnAddYourself, 2);
        tlpInsert.Controls.Add(btnGiveOtherPlayer, 2, 4);
        tlpInsert.SetColumnSpan(btnGiveOtherPlayer, 2);

        ConfigureLabel(lblItemId, "ID:");
        ConfigureLabel(lblItemName, "Name:");
        ConfigureLabel(lblAmount, "Amount:");
        ConfigureLabel(lblEnhance, "Enhance:");
        ConfigureLabel(lblLevel, "Level:");

        ConfigureNumeric(nudItemId, 1, 1_000_000_000, 1);
        txtItemName.Dock = DockStyle.Fill;
        txtItemName.Margin = new Padding(3, 4, 3, 4);
        txtItemName.ReadOnly = true;
        ConfigureNumeric(nudAmount, 1, 99_999, 1);
        nudEnhance.Dock = DockStyle.Fill;
        nudEnhance.Margin = new Padding(3, 4, 3, 4);
        nudEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        ConfigureNumeric(nudLevel, 1, 999, 1);

        chkUseStatusFlag.AutoSize = false;
        chkUseStatusFlag.Dock = DockStyle.Fill;
        chkUseStatusFlag.Margin = new Padding(3, 0, 3, 0);
        chkUseStatusFlag.Text = "Use flag";
        chkUseStatusFlag.UseVisualStyleBackColor = true;
        chkUseStatusFlag.CheckedChanged += chkUseStatusFlag_CheckedChanged;

        nudStatusFlag.Dock = DockStyle.Fill;
        nudStatusFlag.Margin = new Padding(3, 4, 3, 4);
        nudStatusFlag.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });

        ConfigureFillButton(btnAddYourself, "Add to yourself", btnAddYourself_Click);
        ConfigureFillButton(btnGiveOtherPlayer, "Give to player", btnGiveOtherPlayer_Click);

        // gbModifyItem
        gbModifyItem.Text = "Modify item";
        gbModifyItem.AutoSize = true;
        gbModifyItem.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbModifyItem.Dock = DockStyle.Top;
        gbModifyItem.Padding = new Padding(8, 18, 8, 8);
        gbModifyItem.Margin = new Padding(3, 3, 3, 6);
        gbModifyItem.Controls.Add(tlpModify);

        // tlpModify
        tlpModify.AutoSize = true;
        tlpModify.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpModify.Dock = DockStyle.Top;
        tlpModify.ColumnCount = 4;
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModify.RowCount = 5;
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

        tlpModify.Controls.Add(lblWearSlot, 0, 0);
        tlpModify.Controls.Add(cmbWearSlot, 1, 0);
        tlpModify.Controls.Add(rbTargetOwn, 2, 0);
        tlpModify.Controls.Add(rbTargetOther, 3, 0);
        tlpModify.Controls.Add(lblModifyLevel, 0, 1);
        tlpModify.Controls.Add(nudModifyLevel, 1, 1);
        tlpModify.Controls.Add(lblModifyEnhance, 2, 1);
        tlpModify.Controls.Add(nudModifyEnhance, 3, 1);
        tlpModify.Controls.Add(lblModifyItemCode, 0, 2);
        tlpModify.Controls.Add(nudModifyItemCode, 1, 2);
        tlpModify.SetColumnSpan(nudModifyItemCode, 3);
        tlpModify.Controls.Add(btnEditLevel, 0, 3);
        tlpModify.SetColumnSpan(btnEditLevel, 2);
        tlpModify.Controls.Add(btnEditEnhance, 2, 3);
        tlpModify.SetColumnSpan(btnEditEnhance, 2);
        tlpModify.Controls.Add(btnChangeAppearance, 0, 4);
        tlpModify.SetColumnSpan(btnChangeAppearance, 2);
        tlpModify.Controls.Add(btnChangeItemCode, 2, 4);
        tlpModify.SetColumnSpan(btnChangeItemCode, 2);

        ConfigureLabel(lblWearSlot, "Wear slot:");
        ConfigureLabel(lblModifyLevel, "Level:");
        ConfigureLabel(lblModifyEnhance, "Enhance:");
        ConfigureLabel(lblModifyItemCode, "Item code:");

        cmbWearSlot.Dock = DockStyle.Fill;
        cmbWearSlot.Margin = new Padding(3, 4, 3, 4);
        cmbWearSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWearSlot.DropDownWidth = 320;
        cmbWearSlot.FormattingEnabled = true;

        ConfigureRadio(rbTargetOwn, "Own");
        ConfigureRadio(rbTargetOther, "Other");

        ConfigureNumeric(nudModifyLevel, 1, 999, 1);
        nudModifyEnhance.Dock = DockStyle.Fill;
        nudModifyEnhance.Margin = new Padding(3, 4, 3, 4);
        nudModifyEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        ConfigureNumeric(nudModifyItemCode, 1, 1_000_000_000, 1);

        ConfigureFillButton(btnEditLevel, "Edit level", btnEditLevel_Click);
        ConfigureFillButton(btnEditEnhance, "Edit enhance", btnEditEnhance_Click);
        ConfigureFillButton(btnChangeAppearance, "Change appearance", btnChangeAppearance_Click);
        ConfigureFillButton(btnChangeItemCode, "Change item code", btnChangeItemCode_Click);

        // ItemsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = Padding.Empty;
        Margin = Padding.Empty;
        Controls.Add(gbItemActions);
        Name = "ItemsActionsControl";

        ((System.ComponentModel.ISupportInitialize)nudModifyItemCode).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyLevel).EndInit();
        tlpModify.ResumeLayout(false);
        gbModifyItem.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudStatusFlag).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudItemId).EndInit();
        tlpInsert.ResumeLayout(false);
        gbInsertItem.ResumeLayout(false);
        tlpItemRoot.ResumeLayout(false);
        gbItemActions.ResumeLayout(false);
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
