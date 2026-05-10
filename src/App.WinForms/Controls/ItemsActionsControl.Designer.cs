namespace App.WinForms.Controls;

using System.Drawing;
using System.Windows.Forms;

partial class ItemsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel _tlpRoot;
    private GroupBox gbItemActions;
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
    private TableLayoutPanel tlpModifyButtons;
    private Button btnEditLevel;
    private Button btnEditEnhance;
    private Button btnChangeAppearance;
    private Button btnChangeItemCode;

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
        gbItemActions = new GroupBox();
        tabItemsActions = new TabControl();
        tabItem = new TabPage();
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
        tlpModifyButtons = new TableLayoutPanel();
        btnEditLevel = new Button();
        btnEditEnhance = new Button();
        btnChangeAppearance = new Button();
        btnChangeItemCode = new Button();
        tabRandomOption = new TabPage();
        lblRandomOptionPlaceholder = new Label();
        tabItemUseFlag = new TabPage();
        lblItemUseFlagPlaceholder = new Label();

        _tlpRoot.SuspendLayout();
        gbItemActions.SuspendLayout();
        tabItemsActions.SuspendLayout();
        tabItem.SuspendLayout();
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
        tlpModifyButtons.SuspendLayout();
        tabRandomOption.SuspendLayout();
        tabItemUseFlag.SuspendLayout();
        SuspendLayout();

        // _tlpRoot
        _tlpRoot.Dock = DockStyle.Fill;
        _tlpRoot.ColumnCount = 1;
        _tlpRoot.RowCount = 1;
        _tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _tlpRoot.Controls.Add(gbItemActions, 0, 0);

        // gbItemActions
        gbItemActions.Text = "Items";
        gbItemActions.AutoSize = false;
        gbItemActions.Padding = new Padding(8, 18, 8, 8);
        gbItemActions.Dock = DockStyle.Fill;
        gbItemActions.Controls.Add(tabItemsActions);

        // tabItemsActions
        tabItemsActions.Dock = DockStyle.Fill;
        tabItemsActions.SelectedIndex = 0;
        tabItemsActions.Controls.Add(tabItem);
        tabItemsActions.Controls.Add(tabRandomOption);
        tabItemsActions.Controls.Add(tabItemUseFlag);

        // tabItem
        tabItem.Text = "Item";
        tabItem.Padding = new Padding(3);
        tabItem.UseVisualStyleBackColor = true;
        tabItem.Controls.Add(tlpItemRoot);

        // tlpItemRoot
        tlpItemRoot.Dock = DockStyle.Fill;
        tlpItemRoot.ColumnCount = 1;
        tlpItemRoot.RowCount = 3;
        tlpItemRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpItemRoot.Controls.Add(gbInsertItem, 0, 0);
        tlpItemRoot.Controls.Add(gbModifyItem, 0, 1);

        // gbInsertItem
        gbInsertItem.Text = "Give / Insert Commands";
        gbInsertItem.AutoSize = true;
        gbInsertItem.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbInsertItem.Padding = new Padding(8, 18, 8, 8);
        gbInsertItem.Dock = DockStyle.Top;
        gbInsertItem.Controls.Add(tlpInsert);

        // tlpInsert
        tlpInsert.AutoSize = true;
        tlpInsert.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpInsert.Dock = DockStyle.Top;
        tlpInsert.ColumnCount = 4;
        tlpInsert.RowCount = 4;
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpInsert.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpInsert.Controls.Add(lblItemId, 0, 0);
        tlpInsert.Controls.Add(nudItemId, 1, 0);
        tlpInsert.Controls.Add(lblItemName, 2, 0);
        tlpInsert.Controls.Add(txtItemName, 3, 0);
        tlpInsert.Controls.Add(lblAmount, 0, 1);
        tlpInsert.Controls.Add(nudAmount, 1, 1);
        tlpInsert.Controls.Add(lblEnhance, 2, 1);
        tlpInsert.Controls.Add(nudEnhance, 3, 1);
        tlpInsert.Controls.Add(lblLevel, 0, 2);
        tlpInsert.Controls.Add(nudLevel, 1, 2);
        tlpInsert.Controls.Add(chkUseStatusFlag, 2, 2);
        tlpInsert.Controls.Add(nudStatusFlag, 3, 2);
        tlpInsert.Controls.Add(btnAddYourself, 0, 3);
        tlpInsert.Controls.Add(btnGiveOtherPlayer, 2, 3);
        tlpInsert.SetColumnSpan(btnAddYourself, 2);
        tlpInsert.SetColumnSpan(btnGiveOtherPlayer, 2);

        ConfigureLabel(lblItemId, "ID:");
        ConfigureLabel(lblItemName, "Name:");
        ConfigureLabel(lblAmount, "Amount:");
        ConfigureLabel(lblEnhance, "Enhance:");
        ConfigureLabel(lblLevel, "Level:");

        // nudItemId
        nudItemId.Dock = DockStyle.Fill;
        nudItemId.Margin = new Padding(3, 4, 3, 4);
        nudItemId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudItemId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudItemId.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // txtItemName
        txtItemName.Dock = DockStyle.Fill;
        txtItemName.Margin = new Padding(3, 4, 3, 4);
        txtItemName.ReadOnly = true;

        // nudAmount
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Margin = new Padding(3, 4, 3, 4);
        nudAmount.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudEnhance
        nudEnhance.Dock = DockStyle.Fill;
        nudEnhance.Margin = new Padding(3, 4, 3, 4);
        nudEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });

        // nudLevel
        nudLevel.Dock = DockStyle.Fill;
        nudLevel.Margin = new Padding(3, 4, 3, 4);
        nudLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // chkUseStatusFlag
        chkUseStatusFlag.Anchor = AnchorStyles.Left;
        chkUseStatusFlag.AutoSize = true;
        chkUseStatusFlag.Text = "Statusflag";
        chkUseStatusFlag.UseVisualStyleBackColor = true;
        chkUseStatusFlag.CheckedChanged += chkUseStatusFlag_CheckedChanged;

        // nudStatusFlag
        nudStatusFlag.Dock = DockStyle.Fill;
        nudStatusFlag.Margin = new Padding(3, 4, 3, 4);
        nudStatusFlag.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });

        ConfigureGridButton(btnAddYourself, "Add yourself", btnAddYourself_Click);
        ConfigureGridButton(btnGiveOtherPlayer, "Give other player", btnGiveOtherPlayer_Click);

        // gbModifyItem
        gbModifyItem.Text = "Modify Equipped Item";
        gbModifyItem.AutoSize = true;
        gbModifyItem.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbModifyItem.Padding = new Padding(8, 18, 8, 8);
        gbModifyItem.Dock = DockStyle.Top;
        gbModifyItem.Controls.Add(tlpModify);

        // tlpModify
        tlpModify.AutoSize = true;
        tlpModify.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpModify.Dock = DockStyle.Top;
        tlpModify.ColumnCount = 4;
        tlpModify.RowCount = 4;
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
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
        tlpModify.Controls.Add(tlpModifyButtons, 0, 3);
        tlpModify.SetColumnSpan(nudModifyItemCode, 3);
        tlpModify.SetColumnSpan(tlpModifyButtons, 4);

        ConfigureLabel(lblWearSlot, "Targeted Wear-Slot:");
        ConfigureLabel(lblModifyLevel, "Level:");
        ConfigureLabel(lblModifyEnhance, "Enhance:");
        ConfigureLabel(lblModifyItemCode, "Itemcode:");

        // cmbWearSlot
        cmbWearSlot.Dock = DockStyle.Fill;
        cmbWearSlot.Margin = new Padding(3, 4, 3, 4);
        cmbWearSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWearSlot.DropDownWidth = 320;
        cmbWearSlot.FormattingEnabled = true;

        // rbTargetOwn
        rbTargetOwn.Anchor = AnchorStyles.Left;
        rbTargetOwn.AutoSize = true;
        rbTargetOwn.Text = "Own";
        rbTargetOwn.UseVisualStyleBackColor = true;

        // rbTargetOther
        rbTargetOther.Anchor = AnchorStyles.Left;
        rbTargetOther.AutoSize = true;
        rbTargetOther.Text = "Other";
        rbTargetOther.UseVisualStyleBackColor = true;

        // nudModifyLevel
        nudModifyLevel.Dock = DockStyle.Fill;
        nudModifyLevel.Margin = new Padding(3, 4, 3, 4);
        nudModifyLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudModifyLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudModifyLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // nudModifyEnhance
        nudModifyEnhance.Dock = DockStyle.Fill;
        nudModifyEnhance.Margin = new Padding(3, 4, 3, 4);
        nudModifyEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });

        // nudModifyItemCode
        nudModifyItemCode.Dock = DockStyle.Fill;
        nudModifyItemCode.Margin = new Padding(3, 4, 3, 4);
        nudModifyItemCode.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudModifyItemCode.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudModifyItemCode.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // tlpModifyButtons (2x2 grid)
        tlpModifyButtons.Dock = DockStyle.Fill;
        tlpModifyButtons.ColumnCount = 2;
        tlpModifyButtons.RowCount = 2;
        tlpModifyButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModifyButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpModifyButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpModifyButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tlpModifyButtons.Margin = new Padding(0);
        tlpModifyButtons.Controls.Add(btnEditLevel, 0, 0);
        tlpModifyButtons.Controls.Add(btnEditEnhance, 1, 0);
        tlpModifyButtons.Controls.Add(btnChangeAppearance, 0, 1);
        tlpModifyButtons.Controls.Add(btnChangeItemCode, 1, 1);

        ConfigureGridButton(btnEditLevel, "Edit level", btnEditLevel_Click);
        ConfigureGridButton(btnEditEnhance, "Edit Enhance", btnEditEnhance_Click);
        ConfigureGridButton(btnChangeAppearance, "Change appearance", btnChangeAppearance_Click);
        ConfigureGridButton(btnChangeItemCode, "Change itemcode of targeted wear-slot", btnChangeItemCode_Click);

        // tabRandomOption
        tabRandomOption.Text = "Random Option";
        tabRandomOption.Padding = new Padding(3);
        tabRandomOption.UseVisualStyleBackColor = true;
        tabRandomOption.Controls.Add(lblRandomOptionPlaceholder);

        // lblRandomOptionPlaceholder
        lblRandomOptionPlaceholder.AutoSize = true;
        lblRandomOptionPlaceholder.Location = new Point(16, 20);
        lblRandomOptionPlaceholder.Text = "Random Option (coming soon)";

        // tabItemUseFlag
        tabItemUseFlag.Text = "Itemuseflag";
        tabItemUseFlag.Padding = new Padding(3);
        tabItemUseFlag.UseVisualStyleBackColor = true;
        tabItemUseFlag.Controls.Add(lblItemUseFlagPlaceholder);

        // lblItemUseFlagPlaceholder
        lblItemUseFlagPlaceholder.AutoSize = true;
        lblItemUseFlagPlaceholder.Location = new Point(16, 20);
        lblItemUseFlagPlaceholder.Text = "Itemuseflag (coming soon)";

        // ItemsActionsControl
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_tlpRoot);
        Name = "ItemsActionsControl";
        Size = new Size(460, 560);

        tabItemUseFlag.ResumeLayout(false);
        tabItemUseFlag.PerformLayout();
        tabRandomOption.ResumeLayout(false);
        tabRandomOption.PerformLayout();
        tlpModifyButtons.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudModifyItemCode).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyLevel).EndInit();
        tlpModify.ResumeLayout(false);
        tlpModify.PerformLayout();
        gbModifyItem.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)nudStatusFlag).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudItemId).EndInit();
        tlpInsert.ResumeLayout(false);
        tlpInsert.PerformLayout();
        gbInsertItem.ResumeLayout(false);
        tlpItemRoot.ResumeLayout(false);
        tabItem.ResumeLayout(false);
        tabItemsActions.ResumeLayout(false);
        gbItemActions.ResumeLayout(false);
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
