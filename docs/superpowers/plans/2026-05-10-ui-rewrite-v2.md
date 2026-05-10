# UI Layout Rewrite v2 — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development to implement task-by-task.

**Goal:** Make every `*ActionsControl` render correctly on first paint without runtime measurement, without clipping, regardless of host height. Replace the broken half-AutoSize/half-Fill v1 rewrite with a strictly bottom-up AutoSize chain.

**Why v1 failed:** The host panel `pnlActionsHost` docks each ActionsControl with `Dock=Top`, which uses the control's explicit `Size` as a HARD height cap. v1 set explicit `Size = new Size(W, 390)` on each control AND used `Dock=Fill` on the outer GroupBox AND a `Percent 100%` filler row in the root TLP — three things that all fight an AutoSize content chain. Result: inner GroupBoxes get squeezed into ~390px and clip their buttons.

**Architecture:** Bottom-up AutoSize chain with NO Fill, NO Percent rows in the vertical chain. Every container at every level is `AutoSize=true, AutoSizeMode=GrowAndShrink, Dock=Top`. Only innermost TableLayoutPanels declare explicit Absolute pixel row heights for actual content rows. Width stretches naturally because `Dock=Top` sets Width = parent client width.

---

## File Structure

Modified: 8 files in `src/App.WinForms/Controls/`:
- `BuffsActionsControl.Designer.cs`
- `ItemsActionsControl.Designer.cs`
- `MonsterActionsControl.Designer.cs`
- `NpcsActionsControl.Designer.cs`
- `PlayerCheckerActionsControl.Designer.cs`
- `SkillsActionsControl.Designer.cs`
- `SummonsActionsControl.Designer.cs`
- `WarpActionsControl.Designer.cs`

The non-Designer `.cs` files (event handlers, constructor) are **not changed**. Only the `InitializeComponent()` body in each Designer file is rewritten.

---

## The Universal Sizing Pattern

Apply this template to EVERY control. The pattern is non-negotiable — deviating breaks the chain.

```
ActionsControl
  ├ AutoSize = true
  ├ AutoSizeMode = AutoSizeMode.GrowAndShrink
  ├ Padding = Padding.Empty
  ├ Margin = Padding.Empty
  └ NO explicit Size (do NOT set Size = new Size(...) on the control)

  └─ Outer wrapper GroupBox (e.g. gbSkillsActions)
       ├ Text = "Skills" (or whatever the section is called)
       ├ AutoSize = true
       ├ AutoSizeMode = AutoSizeMode.GrowAndShrink
       ├ Dock = DockStyle.Top
       ├ Padding = new Padding(8, 18, 8, 8)
       ├ Margin = new Padding(0)
       └ contains exactly ONE child: tlpRoot

       └─ Root TableLayoutPanel (tlpRoot)
            ├ AutoSize = true
            ├ AutoSizeMode = AutoSizeMode.GrowAndShrink
            ├ Dock = DockStyle.Top
            ├ ColumnCount = 1
            ├ ColumnStyles: ONE column, SizeType.Percent, 100F
            ├ RowCount = N (one per section GroupBox)
            ├ RowStyles: each row SizeType.AutoSize (NO Percent, NO Absolute)
            └ contains N section GroupBoxes (one per row)

            └─ Section GroupBox (e.g. gbSelected, gbPlayerSkills, etc.)
                 ├ Text = section name
                 ├ AutoSize = true
                 ├ AutoSizeMode = AutoSizeMode.GrowAndShrink
                 ├ Dock = DockStyle.Top
                 ├ Padding = new Padding(8, 18, 8, 8)
                 ├ Margin = new Padding(3, 3, 3, 6)   // 6px gap below
                 └ contains exactly ONE child: an inner TableLayoutPanel

                 └─ Inner TableLayoutPanel (e.g. tlpSelected)
                      ├ AutoSize = true
                      ├ AutoSizeMode = AutoSizeMode.GrowAndShrink
                      ├ Dock = DockStyle.Top
                      ├ ColumnCount = M (sized for content)
                      ├ ColumnStyles: see "Column conventions" below
                      ├ RowCount = R (one per row of widgets)
                      ├ RowStyles: each row SizeType.Absolute, with HEIGHT in pixels
                      │   - Input row (textbox / numeric / combo / checkbox / radio): 32F
                      │   - Button row: 36F
                      │   - Compound row (multiple stacked inputs in one row): sum of children heights
                      └ contains the actual widgets in cells
```

### Column conventions for inner TLPs

- Label columns: `SizeType.Absolute, 130F` (or wider if a label text exceeds — round up to next 10).
- Input columns that should stretch with width: `SizeType.Percent, 100F`.
- Button columns: `SizeType.Percent, 100F` if button stretches, else `SizeType.Absolute, 110F`.
- Pairs of inputs on one row (e.g. "Level: [_] Enhance: [_]"): four columns `[Absolute 90, Percent 50, Absolute 90, Percent 50]`.

### Widget conventions (innermost cell content)

- **Label**:
  ```csharp
  label.AutoSize = false;
  label.Dock = DockStyle.Fill;
  label.TextAlign = ContentAlignment.MiddleLeft;
  label.Margin = new Padding(3, 0, 3, 0);
  ```
- **TextBox / NumericUpDown / ComboBox**:
  ```csharp
  control.Dock = DockStyle.Fill;
  control.Margin = new Padding(3, 4, 3, 4);
  ```
- **Button**:
  ```csharp
  button.AutoSize = false;
  button.Dock = DockStyle.Fill;
  button.Margin = new Padding(3);
  button.UseVisualStyleBackColor = true;
  button.Click += BtnFoo_Click;
  ```
- **CheckBox / RadioButton**:
  ```csharp
  rb.AutoSize = false;
  rb.Dock = DockStyle.Fill;
  rb.Margin = new Padding(3, 0, 3, 0);
  ```

### Critical rules (every deviation breaks the chain)

1. **No `Dock = DockStyle.Fill` anywhere in the vertical chain.** Use `DockStyle.Top` consistently from `ActionsControl` down through every container.
2. **No `Percent` row** in any TLP that's part of the vertical AutoSize chain. Percent rows require a fixed parent height to distribute from — they're the opposite of AutoSize. (Percent COLUMNS are fine — column width is independent.)
3. **No explicit `Size = new Size(...)` on the ActionsControl itself.** AutoSize manages it.
4. **No explicit `Size` on GroupBoxes.** AutoSize manages them.
5. **Innermost TLPs** (the ones holding actual widgets) have ONLY `Absolute` row heights — never AutoSize, never Percent. That's where the height is rooted.
6. The host panel `pnlActionsHost` already has `AutoScroll = true`, so if the resulting control is taller than the host, scrolling kicks in safely. We don't change the host.

---

## Per-Control Layouts

For each control, do NOT change field NAMES or event-handler subscriptions. Only change `InitializeComponent()`. The .cs (non-Designer) file remains untouched.

Each control follows the universal pattern above. The differences are: number of section GroupBoxes, what's inside each section (column counts, row counts), and the labels/captions.

### Task 1: PlayerCheckerActionsControl

Fields (preserve exactly): `gbPlayerChecker`, `btnLoadAllCharacters`, `btnLoadOnlineCharacters`, `btnLoadInventory`, `btnLoadWh`, `btnOpenInfos`.

Layout:
- Outer wrapper: `gbPlayerChecker`, text "Player Checker".
- Inside `gbPlayerChecker`: ONE inner TLP (call it `tlpInner`) with:
  - ColumnCount = 2, ColumnStyles `[Percent 50, Percent 50]`
  - RowCount = 3, all `RowStyle Absolute 36F`
  - Cells:
    - (0,0) `btnLoadAllCharacters` "Load all characters"
    - (1,0) `btnLoadOnlineCharacters` "Load online characters"
    - (0,1) `btnLoadInventory` "Load inventory" (col span 1)
    - (1,1) `btnLoadWh` "Load warehouse"
    - (0,2) `btnOpenInfos` "Open infos" with `tlpInner.SetColumnSpan(btnOpenInfos, 2)`

Skip the `tlpRoot` wrapper for this control — `gbPlayerChecker` directly contains `tlpInner`. (Apply the universal pattern but with the simplification that there's only ONE section.)

### Task 2: MonsterActionsControl

Fields: `gbMonsterActions`, `lblSpawnMode`, `cmbSpawnMode`, `lblMonsterId`, `nudMonsterId`, `lblAmount`, `nudAmount`, `lblX`, `nudX`, `lblY`, `nudY`, `lblLayer`, `nudLayer`, `chkUseLifetime`, `lblMinutes`, `nudMinutesLifetime`, `btnCreateSpawnCommand`.

Layout (single section like PlayerChecker — outer GroupBox directly holds inner TLP):
- Outer: `gbMonsterActions`, text "Monster".
- Inner TLP `tlp`:
  - ColumnCount = 4, ColumnStyles `[Absolute 130, Percent 50, Absolute 130, Percent 50]`
  - RowCount = 6, all `RowStyle Absolute 32F` except the button row which is `Absolute 36F`
  - Rows (label/value/label/value pattern):
    - Row 0: `lblSpawnMode`, `cmbSpawnMode` (col span 3), — set `tlp.SetColumnSpan(cmbSpawnMode, 3)`
    - Row 1: `lblMonsterId`, `nudMonsterId`, `lblAmount`, `nudAmount`
    - Row 2: `lblX`, `nudX`, `lblY`, `nudY`
    - Row 3: `lblLayer`, `nudLayer`, `chkUseLifetime`, (empty)
    - Row 4: `lblMinutes`, `nudMinutesLifetime` (col span 1), (empty), (empty) — keep nudMinutesLifetime in col 1 only
    - Row 5: `btnCreateSpawnCommand` with `tlp.SetColumnSpan(btnCreateSpawnCommand, 4)`, RowStyle Absolute 36F

### Task 3: SkillsActionsControl

Fields: `gbSkillsActions`, `gbSelected`, `lblSkillId`, `nudSkillId`, `lblSkillLevel`, `nudSkillLevel`, `gbPlayerSkills`, `btnLearnSkill`, `btnSetSkill`, `btnRemoveSkill`, `btnLearnAllJobSkills`, `gbCreatureSkills`, `lblCreatureSlotIndex`, `nudCreatureSlotIndex`, `btnLearnCreatureSkill`, `btnLearnCreatureAllSkill`.

Three section GroupBoxes inside `tlpRoot` (rows 0/1/2 all AutoSize):

**Section `gbSelected`**, text "Selected":
- Inner TLP `tlpSelected`: 4 cols `[Abs 60, Pct 50, Abs 80, Pct 50]`, 1 row `Abs 32`
- (0,0) `lblSkillId` "ID:", (1,0) `nudSkillId`, (2,0) `lblSkillLevel` "Level:", (3,0) `nudSkillLevel`

**Section `gbPlayerSkills`**, text "Player Skills":
- Inner TLP `tlpPlayerSkills`: 2 cols `[Pct 50, Pct 50]`, 2 rows both `Abs 36`
- (0,0) `btnLearnSkill` "Learn skill", (1,0) `btnSetSkill` "Set skill level"
- (0,1) `btnRemoveSkill` "Remove skill", (1,1) `btnLearnAllJobSkills` "Learn all skills"

**Section `gbCreatureSkills`**, text "Creature Skills":
- Inner TLP `tlpCreature`: 2 cols `[Abs 160, Pct 100]`, 3 rows `[Abs 32, Abs 36, Abs 36]`
- (0,0) `lblCreatureSlotIndex` "Creature slot (0=all):", (1,0) `nudCreatureSlotIndex`
- (0,1) `btnLearnCreatureSkill` with col span 2 — `tlpCreature.SetColumnSpan(btnLearnCreatureSkill, 2)`
- (0,2) `btnLearnCreatureAllSkill` with col span 2

### Task 4: BuffsActionsControl

Fields: `gbBuffActions`, `gbSelected`, `lblStateId`, `nudStateId`, `lblBuffName`, `txtBuffName`, `lblBuffLevel`, `nudBuffLevel`, `lblDuration`, `nudDurationMinutes`, `lblDurationUnit`, `gbGlobalBuffs`, `btnAddTimedWorldState`, `btnAddEventState`, `btnRemoveEventState`, `gbPlayerCreature`, `rbPlayer`, `rbSummon`, `btnAddBuff`, `btnRemoveBuff`.

Three sections inside `tlpRoot`:

**`gbSelected`** "Selected":
- Inner TLP: 4 cols `[Abs 100, Pct 50, Abs 100, Pct 50]`, 3 rows all `Abs 32`
- Row 0: `lblStateId` "State ID:", `nudStateId` (col span 3) — `SetColumnSpan(nudStateId, 3)`
- Row 1: `lblBuffName` "Buff name:", `txtBuffName` (col span 3)
- Row 2: `lblBuffLevel` "Level:", `nudBuffLevel`, `lblDuration` "Duration (min):", `nudDurationMinutes`
  - `lblDurationUnit` is unused in this layout — set `lblDurationUnit.Visible = false` and don't add to TLP.

**`gbGlobalBuffs`** "Global / world buffs":
- Inner TLP: 1 col `Pct 100`, 3 rows all `Abs 36`
- (0,0) `btnAddTimedWorldState` "Add timed world state"
- (0,1) `btnAddEventState` "Add event state"
- (0,2) `btnRemoveEventState` "Remove event state"

**`gbPlayerCreature`** "Player / creature buffs":
- Inner TLP: 2 cols `[Pct 50, Pct 50]`, 2 rows `[Abs 28, Abs 36]`
- (0,0) `rbPlayer` "Player", (1,0) `rbSummon` "Summon/Creature"
- (0,1) `btnAddBuff` "Add buff", (1,1) `btnRemoveBuff` "Remove buff"

(`tlpBottom` field is obsolete — set to `null` field but don't reference; if removing it would change field set, just don't add it to any container.)

### Task 5: NpcsActionsControl

Fields: `gbNpcActions`, `gbSelectedNpc`, `lblNpcId`, `nudNpcId`, `lblNpcName`, `txtNpcName`, `lblContactScript`, `txtContactScript`, `lblX`, `nudX`, `lblY`, `nudY`, `lblLayer`, `nudLayer`, `chkHideNpc`, `gbCommands`, `btnAddNpcToWorld`, `btnShowNpc`, `btnWarpToNpc`.

Two sections:

**`gbSelectedNpc`** "Selected NPC":
- Inner TLP `tlpSelected`: 4 cols `[Abs 110, Pct 50, Abs 80, Pct 50]`, 5 rows all `Abs 32`
- Row 0: `lblNpcId` "NPC ID:", `nudNpcId`, (empty), (empty)
- Row 1: `lblNpcName` "Name:", `txtNpcName` (col span 3)
- Row 2: `lblContactScript` "Contact script:", `txtContactScript` (col span 3)
- Row 3: `lblX` "X:", `nudX`, `lblY` "Y:", `nudY`
- Row 4: `lblLayer` "Layer:", `nudLayer`, `chkHideNpc` (col span 2)
  - `chkHideNpc.Text = "Hide NPC"`

**`gbCommands`** "Commands":
- Inner TLP `tlpCommands`: 1 col `Pct 100`, 3 rows all `Abs 36`
- (0,0) `btnAddNpcToWorld` "Add NPC to world"
- (0,1) `btnShowNpc` "Show NPC"
- (0,2) `btnWarpToNpc` "Warp to NPC"

### Task 6: SummonsActionsControl

Fields: `gbSummonActions`, `gbAddSummon`, `lblSummonId`, `nudSummonId`, `chkUseAddStage`, `lblAddStage`, `nudAddStage`, `btnAddSummon`, `gbStageSummon`, `lblSlot`, `cmbSlot`, `lblStage`, `nudStage`, `btnStageSummon`.

Two sections:

**`gbAddSummon`** "Add summon":
- Inner TLP `tlpAdd`: 4 cols `[Abs 110, Pct 50, Abs 110, Pct 50]`, 3 rows `[Abs 32, Abs 32, Abs 36]`
- Row 0: `lblSummonId` "Summon ID:", `nudSummonId`, (empty), (empty)
- Row 1: `chkUseAddStage` (col span 2, text "Force stage"), `lblAddStage` "Stage:", `nudAddStage`
- Row 2: `btnAddSummon` "Add summon" with col span 4

**`gbStageSummon`** "Set stage on existing":
- Inner TLP `tlpStage`: 4 cols `[Abs 110, Pct 50, Abs 110, Pct 50]`, 2 rows `[Abs 32, Abs 36]`
- Row 0: `lblSlot` "Slot:", `cmbSlot`, `lblStage` "Stage:", `nudStage`
- Row 1: `btnStageSummon` "Set stage" with col span 4

### Task 7: WarpActionsControl

Fields: `gbWarpActions`, `gbWarpCommands`, `lblSelectedX`, `nudSelectedX`, `lblSelectedY`, `nudSelectedY`, `btnWarpToYou`, `btnWarp`, `btnWarpToSomeone`, `btnOpenWorldmap`, `gbManageWarp`, `lblAddX`, `nudAddX`, `lblAddY`, `nudAddY`, `lblLocationName`, `txtLocationName`, `btnAdd`, `btnRemoveSelected`.

Two sections:

**`gbWarpCommands`** "Warp commands":
- Inner TLP `tlpCommands`: 4 cols `[Abs 60, Pct 50, Abs 60, Pct 50]`, 3 rows `[Abs 32, Abs 36, Abs 36]`
- Row 0: `lblSelectedX` "X:", `nudSelectedX`, `lblSelectedY` "Y:", `nudSelectedY`
- Row 1: `btnWarpToYou` "Warp to you" (col span 2), `btnWarp` "Warp" (col span 2)
- Row 2: `btnWarpToSomeone` "Warp player" (col span 2), `btnOpenWorldmap` "Open worldmap" (col span 2)

**`gbManageWarp`** "Manage warp locations":
- Inner TLP `tlpManage`: 4 cols `[Abs 60, Pct 50, Abs 60, Pct 50]`, 3 rows `[Abs 32, Abs 32, Abs 36]`
- Row 0: `lblAddX` "X:", `nudAddX`, `lblAddY` "Y:", `nudAddY`
- Row 1: `lblLocationName` "Name:", `txtLocationName` (col span 3)
- Row 2: `btnAdd` "Add" (col span 2), `btnRemoveSelected` "Remove selected" (col span 2)

### Task 8: ItemsActionsControl

Largest control. Fields: `gbItemActions`, `lblRandomOptionPlaceholder`, `lblItemUseFlagPlaceholder` (mark Visible=false, don't add), `gbInsertItem`, `lblItemId`, `nudItemId`, `lblItemName`, `txtItemName`, `lblAmount`, `nudAmount`, `lblEnhance`, `nudEnhance`, `lblLevel`, `nudLevel`, `chkUseStatusFlag`, `nudStatusFlag`, `btnAddYourself`, `btnGiveOtherPlayer`, `gbModifyItem`, `lblWearSlot`, `cmbWearSlot`, `rbTargetOwn`, `rbTargetOther`, `lblModifyLevel`, `nudModifyLevel`, `lblModifyEnhance`, `nudModifyEnhance`, `lblModifyItemCode`, `nudModifyItemCode`, `btnEditLevel`, `btnEditEnhance`, `btnChangeAppearance`, `btnChangeItemCode`.

Two sections inside `tlpItemRoot`:

**`gbInsertItem`** "Insert item":
- Inner TLP `tlpInsert`: 4 cols `[Abs 90, Pct 50, Abs 90, Pct 50]`, 5 rows `[Abs 32, Abs 32, Abs 32, Abs 32, Abs 36]`
- Row 0: `lblItemId` "ID:", `nudItemId`, (empty), (empty)
- Row 1: `lblItemName` "Name:", `txtItemName` (col span 3)
- Row 2: `lblAmount` "Amount:", `nudAmount`, `lblEnhance` "Enhance:", `nudEnhance`
- Row 3: `lblLevel` "Level:", `nudLevel`, `chkUseStatusFlag` "Use flag", `nudStatusFlag`
  - `chkUseStatusFlag.Text = "Use flag"`
- Row 4: `btnAddYourself` "Add to yourself" (col span 2), `btnGiveOtherPlayer` "Give to player" (col span 2)

**`gbModifyItem`** "Modify item":
- Inner TLP `tlpModify`: 4 cols `[Abs 90, Pct 50, Abs 90, Pct 50]`, 5 rows `[Abs 32, Abs 32, Abs 32, Abs 32, Abs 36]`
- Row 0: `lblWearSlot` "Wear slot:", `cmbWearSlot`, `rbTargetOwn` "Own", `rbTargetOther` "Other"
- Row 1: `lblModifyLevel` "Level:", `nudModifyLevel`, `lblModifyEnhance` "Enhance:", `nudModifyEnhance`
- Row 2: `lblModifyItemCode` "Item code:", `nudModifyItemCode` (col span 3)
- Row 3: `btnEditLevel` "Edit level" (col span 2), `btnEditEnhance` "Edit enhance" (col span 2)
- Row 4: `btnChangeAppearance` "Change appearance" (col span 2), `btnChangeItemCode` "Change item code" (col span 2)

(Skip the obsolete `tlpModifyButtons` field.)

---

## Tasks (one per control)

For each task:
1. Open the existing `<Name>ActionsControl.Designer.cs` file.
2. Re-read the `<Name>ActionsControl.cs` (non-Designer) once to confirm event-handler method names referenced by buttons (e.g., `btnSearch_Click`). Preserve those subscriptions exactly.
3. **Replace** the body of `InitializeComponent()` per the layout for that control above.
4. **Update field declarations at top** of the partial class to match the controls actually used (remove obsolete fields like `_tlpRoot`, `tlpRoot` outer wrapper if simplified; add any new ones referenced).
5. Apply the universal pattern strictly — re-read the Critical Rules section before each control.
6. Build: `dotnet build YSM-GMTool.slnx`. Must succeed.
7. Commit per control:
   - Task 1: `git commit -m "fix(ui): PlayerCheckerActionsControl bottom-up AutoSize chain"`
   - Task 2: `git commit -m "fix(ui): MonsterActionsControl bottom-up AutoSize chain"`
   - Task 3: `git commit -m "fix(ui): SkillsActionsControl bottom-up AutoSize chain"`
   - Task 4: `git commit -m "fix(ui): BuffsActionsControl bottom-up AutoSize chain"`
   - Task 5: `git commit -m "fix(ui): NpcsActionsControl bottom-up AutoSize chain"`
   - Task 6: `git commit -m "fix(ui): SummonsActionsControl bottom-up AutoSize chain"`
   - Task 7: `git commit -m "fix(ui): WarpActionsControl bottom-up AutoSize chain"`
   - Task 8: `git commit -m "fix(ui): ItemsActionsControl bottom-up AutoSize chain"`

After all 8: run `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj`. Must remain 4/4 green.

---

## Reference Implementation: SkillsActionsControl

Use this as the canonical example. Apply the same structural pattern (with content adjusted) to every other control.

```csharp
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
```

Note the use of `Dock = DockStyle.Fill` ON LEAF WIDGETS (label, numeric, button). That's fine — Fill at the leaf level means "fill the cell of your parent TLP". The cell has fixed size from RowStyle/ColumnStyle, so filling it is well-defined. The forbidden Fill is in the vertical chain of CONTAINERS (the GroupBoxes and the TLPs that hold sections).

---

## Acceptance

- `dotnet build YSM-GMTool.slnx` — clean.
- `dotnet test` — 4/4 green.
- Manually run app: PlayerChecker, Items, Monster, Skills, Buffs, NPCs, Summons, Warp tabs each render with NO clipping, NO buttons cut off, sections sized to content.
- Resizing the window vertically does not clip sections (or if very small, scroll appears in `pnlActionsHost`).
