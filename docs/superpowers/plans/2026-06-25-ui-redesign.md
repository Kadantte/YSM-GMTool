# GM Tool UI Redesign Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Redesign the Avalonia GM Tool UI per owner feedback — denser/smaller typography, text-only styled tabs, a top command bar replacing the right sidebar, a narrower data grid with a roomier responsive action panel, an always-visible scrollbar, a settings-driven row height with row-scaled icons, removal of the command-history feature, and a fix for the icon/column-misalignment bug.

**Architecture:** View/theme-layer redesign only — the existing architecture is preserved unchanged: `ITabModule` extensibility, ReactiveUI MVVM, DI (Microsoft.Extensions.DI + Splat bridge), the `Theme/*.axaml` resource dictionaries, and the generic `EntityBrowser`. No changes to `App.Core`/`App.Data` except adding one `AppSettings.RowHeight` field and removing the now-unused `ICommandHistoryService`/`CommandHistoryService`. The right sidebar (`Shell/Sidebar*`) is replaced by a top bar (`Shell/TopBar*`); `MainWindow` becomes a `DockPanel`.

**Tech Stack:** .NET 10, Avalonia 11.3.x, Avalonia.ReactiveUI, Avalonia.Controls.DataGrid, FluentTheme (Dark), ReactiveUI, Microsoft.Extensions.DependencyInjection, Splat. No test project (owner decision) — verify via `dotnet build` (0/0) + launching the app and observing.

---

## Decisions (locked from brainstorming, 2026-06-25)

1. **Top command bar** replaces the right sidebar. Holds: bigger logo + "GM Tool"; target-player `ComboBox`; a "manage players" icon-button opening a `Flyout` (text box + list + add/remove); a `/run` checkbox; ⚙ Settings and ⓘ About icon-buttons. `MainWindow` = `DockPanel` (top bar docked top, `TabControl` fills).
2. **Text-only tabs**, no icons. Active tab = colored bottom-border underline (style `TabItem` in `Theme/Controls.axaml`). `ITabModule.IconKey` stays in the interface but is no longer rendered.
3. **Compact density**: chrome text 12–13px, section headers ~13px/SemiBold, control height ~28px. Override the large FluentTheme defaults via styles in `Theme/Controls.axaml`.
4. **Data grid**: default grid/actions split ~42/58 (draggable splitter), smaller font, **row height from `AppSettings.RowHeight`** (default 26, range 18–48), **always-visible scrollbar**, **icon size = row height − padding**.
5. **#3 bug fix** (icon shows filename / Name column shows ID): root cause is the time-dependent `Icons()` snapshot (columns built at VM construction before settings load = icons off; row values built at load after settings load = icons on → off-by-one). Fix: load settings synchronously before tab VMs are constructed, derive a single consistent icons-on flag, and rebuild columns when icon settings change.
6. **Action panels** (all 8 tabs): responsive sections (`auto-fit minmax(~120px,1fr)`), labeled compact fields, emphasized primary action; **drop empty placeholders** (Items "Random Option"/"Itemuseflag" sub-tabs; Warp disabled "OpenWorldmap").
7. **Remove command history** (#8): delete the history UI, remove `ICommandHistoryService` usage from `CommandDispatcher`, delete `ICommandHistoryService`/`CommandHistoryService` and their registration.
8. **Settings/About** = small icon-buttons in the top bar.

---

## File structure (changes)

```
src/App.Core/
  Models/AppSettings.cs                         MODIFY  + int RowHeight (default 26) + Clone
  Abstractions/ICommandHistoryService.cs        DELETE
  Services/CommandHistoryService.cs             DELETE
src/App.Desktop/
  Theme/Controls.axaml                          MODIFY  density + TabItem + section-header styles
  Shell/MainWindow.axaml(.cs)                   MODIFY  DockPanel: TopBarView + TabControl
  Shell/ShellViewModel.cs                       MODIFY  Sidebar -> TopBar; sync settings load
  Shell/SidebarView.axaml(.cs)                  DELETE
  Shell/SidebarViewModel.cs                     DELETE
  Shell/TopBarView.axaml(.cs)                   CREATE  logo + player combo + flyout + /run + icons
  Shell/TopBarViewModel.cs                      CREATE  player mgmt + append toggle (no history)
  Services/CommandDispatcher.cs                 MODIFY  drop history Add
  Composition/ServiceCollectionExtensions.cs    MODIFY  drop history reg; Sidebar->TopBar; sync load
  Program.cs                                    MODIFY  load settings synchronously before app run
  Controls/EntityBrowserView.axaml(.cs)         MODIFY  scrollbar, row height, density, icon size, column rebuild
  ViewModels/EntityBrowserViewModel.cs          MODIFY  RowHeight + IconsEnabled snapshot + ColumnsChanged
  Infrastructure/IconKeyToBitmapConverter.cs    MODIFY  size from row height (if used)
  Features/Settings/SettingsViewModel.cs        MODIFY  + RowHeight slider state + map
  Features/Settings/SettingsWindow.axaml        MODIFY  + Row height slider in General
  Features/*/*.axaml (8 tabs)                    MODIFY  responsive sections, drop placeholders
  Features/Items/ItemsTabViewModel.cs            MODIFY  remove placeholder-only members if any
```

---

## Verification model (no test project)

Each task: `dotnet build src/App.Desktop/App.Desktop.csproj -c Debug` → `0 errors, 0 warnings`; where UI behavior changes, launch and observe (smoke launch: run `bin/Debug/net10.0/GM Tool.exe`, confirm clean startup, check `%LocalAppData%/YSM-GMTool/logs/*.log` has no `[FTL]`). Commit per task. The FINAL task additionally clicks through every tab + Settings + About.

---

## PHASE A — Foundation (theme, settings field, remove history)

### Task A1: Density + tab + section-header theme

**Files:** Modify `src/App.Desktop/Theme/Controls.axaml` (read current content first; append/adjust styles).

- [ ] **Step 1: Add compact density + tab + section-header styles**

Add Avalonia `Style` rules (selectors) for a denser chrome and text-only tabs. Target values: `TextBlock`/control base font 13; `Button`,`TextBox`,`ComboBox`,`NumericUpDown` `MinHeight="28"`, `Padding` tightened; `DataGridColumnHeader` font 12. Style `TabItem` so it's text-only with an active underline (no icons):

```xml
<Style Selector="TabItem">
  <Setter Property="FontSize" Value="13" />
  <Setter Property="Padding" Value="11,7" />
  <Setter Property="MinHeight" Value="0" />
</Style>
<Style Selector="TabItem:selected /template/ Border#PART_SelectedPipe">
  <Setter Property="Background" Value="{DynamicResource Accent}" />
</Style>
<Style Selector="TabItem:selected">
  <Setter Property="Foreground" Value="{DynamicResource Accent}" />
</Style>
```
Add a reusable section-header class for action panels:
```xml
<Style Selector="TextBlock.sectionHeader">
  <Setter Property="FontSize" Value="13" />
  <Setter Property="FontWeight" Value="SemiBold" />
  <Setter Property="Foreground" Value="{DynamicResource MutedForeground}" />
  <Setter Property="Margin" Value="0,4,0,4" />
</Style>
<Style Selector="Border.sectionDivider">
  <Setter Property="BorderBrush" Value="{DynamicResource Border}" />
  <Setter Property="BorderThickness" Value="0,0,0,1" />
</Style>
```
(Inspect the FluentTheme `TabItem` template part name; if `PART_SelectedPipe` differs in 11.3, use the actual selected-state pipe/border part, or restyle via `TabItem:selected` border. Verify by running.)

- [ ] **Step 2: Build** → `dotnet build src/App.Desktop/App.Desktop.csproj -c Debug` (0/0).
- [ ] **Step 3: Smoke launch**, confirm tabs show text with an active underline and chrome is denser; no `[FTL]`.
- [ ] **Step 4: Commit** — `git commit -am "style(desktop): compact density + text-only tab styling + section-header styles"`.

### Task A2: AppSettings.RowHeight

**Files:** Modify `src/App.Core/Models/AppSettings.cs`.

- [ ] **Step 1: Add the field + clone**

```csharp
public int RowHeight { get; set; } = 26;
```
Add `RowHeight = RowHeight,` to `Clone()`.

- [ ] **Step 2: Build** `dotnet build src/App.Core/App.Core.csproj` (0/0).
- [ ] **Step 3: Commit** — `git commit -am "feat(core): add AppSettings.RowHeight (default 26)"`.

### Task A3: Remove command history

**Files:** Modify `src/App.Desktop/Services/CommandDispatcher.cs`; delete `src/App.Core/Abstractions/ICommandHistoryService.cs`, `src/App.Core/Services/CommandHistoryService.cs`; modify `src/App.Desktop/Composition/ServiceCollectionExtensions.cs`.

- [ ] **Step 1: Strip history from the dispatcher**

`CommandDispatcher` currently depends on `ICommandHistoryService` and calls `history.Add(final)`. Remove that dependency and call. Result:
```csharp
public sealed class CommandDispatcher(IClipboardService clipboard, IAppSettingsHolder settings) : ICommandDispatcher
{
    public Task DispatchAsync(string luaCommand) => clipboard.SetTextAsync(ApplyRunPrefix(luaCommand));

    private string ApplyRunPrefix(string command)
    {
        if (!settings.Current.AppendGeneratedCommands) return command;
        var t = command.TrimStart();
        if (t.StartsWith("//", StringComparison.Ordinal) || t.StartsWith("/run ", StringComparison.OrdinalIgnoreCase))
            return command;
        return "/run " + command;
    }
}
```

- [ ] **Step 2: Delete the service + interface files** and remove their `using`/registration. In `ServiceCollectionExtensions`, delete `s.AddSingleton<ICommandHistoryService, CommandHistoryService>();`.
- [ ] **Step 3: Build** the solution; fix any dangling references (the only consumers were the dispatcher + the sidebar history panel, removed in Task B1). Expect 0/0 after Task B1; if building before B1, temporarily expect SidebarViewModel errors — do A3 and B1 together if needed (note: B1 deletes the sidebar entirely).
- [ ] **Step 4: Commit** — `git commit -am "refactor(desktop): remove command-history feature"`.

---

## PHASE B — Top bar (replaces sidebar)

### Task B1: TopBarViewModel + TopBarView; rewire MainWindow & Shell

**Files:** Create `Shell/TopBarViewModel.cs`, `Shell/TopBarView.axaml(.cs)`; delete `Shell/SidebarViewModel.cs`, `Shell/SidebarView.axaml(.cs)`; modify `Shell/MainWindow.axaml(.cs)`, `Shell/ShellViewModel.cs`, `Composition/ServiceCollectionExtensions.cs`.

- [ ] **Step 1: TopBarViewModel** (port the non-history members from `SidebarViewModel`; read it first). Wraps `IPlayerContext` (Players, SelectedPlayer, NewPlayerName, `AddPlayer`/`RemovePlayer` commands) and the append toggle (`AppendRun` two-way to `settings.Current.AppendGeneratedCommands` via the holder + save). NO history members. Expose `OpenSettings`/`OpenAbout` (delegate to the shell's commands, or move them here — keep them on ShellViewModel and bind through). Keep it a `ReactiveObject`.

- [ ] **Step 2: TopBarView.axaml** — a horizontal bar:
```xml
<UserControl ... x:Class="App.Desktop.Shell.TopBarView" x:CompileBindings="False">
  <Border BorderBrush="{DynamicResource Border}" BorderThickness="0,0,0,1" Padding="10,6">
    <Grid ColumnDefinitions="Auto,Auto,*,Auto,Auto,Auto,Auto,Auto">
      <Image Grid.Column="0" Width="40" Height="40" Source="/Assets/Heaven_logo1.png" Margin="0,0,10,0"/>
      <TextBlock Grid.Column="1" Text="GM Tool" FontSize="15" FontWeight="SemiBold" VerticalAlignment="Center"/>
      <TextBlock Grid.Column="3" Text="Target" VerticalAlignment="Center" Margin="0,0,6,0" Foreground="{DynamicResource MutedForeground}"/>
      <ComboBox Grid.Column="4" Width="150" ItemsSource="{Binding Players}" SelectedItem="{Binding SelectedPlayer, Mode=TwoWay}"/>
      <Button Grid.Column="5" Width="30" Margin="6,0" ToolTip.Tip="Manage players">
        <PathIcon .../> <!-- or text "+"; flyout below -->
        <Button.Flyout>
          <Flyout>
            <StackPanel Width="220" Spacing="6">
              <TextBox Watermark="New player name" Text="{Binding NewPlayerName, Mode=TwoWay}"/>
              <StackPanel Orientation="Horizontal" Spacing="6">
                <Button Content="Add" Command="{Binding AddPlayer}"/>
                <Button Content="Remove selected" Command="{Binding RemovePlayer}"/>
              </StackPanel>
              <ListBox ItemsSource="{Binding Players}" SelectedItem="{Binding SelectedPlayer, Mode=TwoWay}" MaxHeight="160"/>
            </StackPanel>
          </Flyout>
        </Button.Flyout>
      </Button>
      <CheckBox Grid.Column="6" Content="/run" IsChecked="{Binding AppendRun, Mode=TwoWay}" Margin="8,0"/>
      <StackPanel Grid.Column="7" Orientation="Horizontal" Spacing="4">
        <Button Width="30" ToolTip.Tip="Settings" Command="{Binding OpenSettings}"><!-- gear icon --></Button>
        <Button Width="30" ToolTip.Tip="About" Command="{Binding OpenAbout}"><!-- info icon --></Button>
      </StackPanel>
    </Grid>
  </Border>
</UserControl>
```
(Use the FontAwesome icon provider already wired — `<i:Icon Value="fa-solid fa-gear"/>` etc. from `Projektanker.Icons.Avalonia`; the namespace is registered in App.axaml/Theme. For "manage players" use `fa-solid fa-user-plus`. These are toolbar action icons, allowed per the design.) Code-behind: only `InitializeComponent();` (generated — do NOT hand-write `AvaloniaXamlLoader.Load`).

- [ ] **Step 3: ShellViewModel** — replace `SidebarViewModel Sidebar` with `TopBarViewModel TopBar`; keep `OpenSettings`/`OpenAbout` (TopBarView binds to them via the shell DataContext, or expose them on TopBarViewModel). Keep the tab list + settings init. (See Task C-Fix for moving settings load to synchronous.)

- [ ] **Step 4: MainWindow.axaml** — `DockPanel`:
```xml
<DockPanel>
  <local:TopBarView DockPanel.Dock="Top" DataContext="{Binding TopBar}"/>
  <TabControl ItemsSource="{Binding Tabs}" SelectedItem="{Binding SelectedTab}">
    <TabControl.ItemTemplate><DataTemplate><TextBlock Text="{Binding Title}"/></DataTemplate></TabControl.ItemTemplate>
  </TabControl>
</DockPanel>
```
Bind Settings/About: if they live on ShellViewModel, the TopBarView buttons need the shell as DataContext for those two — simplest: expose `OpenSettings`/`OpenAbout` on `TopBarViewModel` (inject the same logic) so the bar binds entirely to `TopBar`. Window properties (Title, size, icon) unchanged.

- [ ] **Step 5: DI** — in `ServiceCollectionExtensions`, replace `AddSingleton<SidebarViewModel>` with `AddSingleton<TopBarViewModel>`.

- [ ] **Step 6: Build + smoke launch** — top bar renders with bigger logo, player combo + manage flyout, /run, settings/about icons; tabs are text-only; no right sidebar; no `[FTL]`.
- [ ] **Step 7: Commit** — `git commit -am "feat(desktop): top command bar replaces right sidebar; bigger logo; manage-players flyout"`.

---

## PHASE C — Data grid + icon/column fix

### Task C1: Synchronous settings load (fixes #3 root cause + icon timing)

**Files:** Modify `src/App.Desktop/Program.cs`, `src/App.Desktop/Shell/ShellViewModel.cs`, `src/App.Desktop/Composition/ServiceCollectionExtensions.cs`.

- [ ] **Step 1: Load settings before the UI/tab VMs construct.** Move settings load out of the async fire-and-forget in `ShellViewModel`. In `Program.Main`, after building `Services` and before `StartWithClassicDesktopLifetime`, synchronously load + seed:
```csharp
var holder = Services.GetRequiredService<IAppSettingsHolder>();
var settingsService = Services.GetRequiredService<IAppSettingsService>();
var csb = Services.GetRequiredService<IConnectionStringBuilderService>();
var settings = settingsService.LoadAsync().GetAwaiter().GetResult();
SettingsBootstrap.EnsureDefaults(settings);
SettingsBootstrap.ApplyEnvironmentDefaults(settings, csb);
holder.Set(settings);
IconCache.Configure(settings.EnableEntityIcons, settings.EntityIconsPath);
```
Extract `EnsureDefaults`/`ApplyEnvironmentDefaults` from `ShellViewModel` into a small static `Composition/SettingsBootstrap.cs` so both can call them. Remove the async `InitializeSettings`/`AppSettingsLoaded` from `ShellViewModel` (the holder is already seeded before the shell builds). Tab VMs (resolved when the shell builds) now see the real settings → `Icons()` is correct at construction.

- [ ] **Step 2: Build + smoke launch** (no DB needed) — confirm clean startup; settings (e.g., row height, icons flags) are present at construction.
- [ ] **Step 3: Commit** — `git commit -am "fix(desktop): load settings synchronously before tab construction (icon-state timing)"`.

### Task C2: EntityBrowser density, row height, scrollbar, icon size, column rebuild

**Files:** Modify `src/App.Desktop/Controls/EntityBrowserView.axaml(.cs)`, `src/App.Desktop/ViewModels/EntityBrowserViewModel.cs`.

- [ ] **Step 1: Grid layout + scrollbar + row height + density (axaml).** Change the root `Grid ColumnDefinitions="*,Auto,Auto"` so the grid pane is narrower and actions wider — make the splitter govern two star columns defaulting ~42/58 (e.g. `ColumnDefinitions="42*,Auto,58*"`, actions panel `Width=Auto MinWidth=300` becomes a star column). On `RecordsGrid` add:
```xml
RowHeight="{Binding RowHeight}"
FontSize="12"
ScrollViewer.VerticalScrollBarVisibility="Visible"
ScrollViewer.HorizontalScrollBarVisibility="Auto"
```
Set the actions `StackPanel` to fill its star column (remove fixed `Width=360`; use `MinWidth=300`). Keep the draggable splitter.

- [ ] **Step 2: RowHeight on the VM.** Add `public int RowHeight { get; }` to `EntityBrowserViewModel<TRecord>` (and the `IEntityBrowser` interface), sourced from `IAppSettingsHolder.Current.RowHeight` (passed in or read at construction; clamp 18–48). Bind `RecordsGrid.RowHeight`.

- [ ] **Step 3: Icon size = row height.** In `BuildImageColumn`, size the `Image` to `RowHeight - 4` (min 8) instead of `column.ImageSize`, and call `IconCache.Resolve(key, RowHeight - 4)`. Pass the browser's `RowHeight` into the column builder.

- [ ] **Step 4: Column rebuild on settings change.** Expose an event/observable on the browser (e.g. `event EventHandler? ColumnsChanged`) raised when the icon-on flag or row height changes; in the View, re-run `BuildColumns` on that event (in addition to `OnDataContextChanged`). This makes a Settings change (icons on/off, row height) re-shape the grid without a restart.

- [ ] **Step 5: Build + smoke launch** — grid is narrower/denser, rows ~26px, scrollbar always shown.
- [ ] **Step 6: Commit** — `git commit -am "feat(desktop): denser narrower grid, settings row height, always-on scrollbar, row-scaled icons"`.

### Task C3: Fix icon/column off-by-one (#3) — verify root cause, make icons-on consistent

**Files:** Modify each `Features/*/*TabViewModel.cs` that uses icons (Items, Skills, Buffs, Summons), and/or `EntityBrowserViewModel.cs`.

- [ ] **Step 1: Confirm root cause (systematic-debugging).** Root cause (already diagnosed): the tab VMs compute `Columns` from `Icons()` at construction and `rowValuesSelector` calls `Icons()` again at load; with Task C1 these now both see loaded settings, but they must use the SAME snapshot to stay aligned even if settings change at runtime. Verify by reading one icon tab VM (e.g. `Features/Items/ItemsTabViewModel.cs`).

- [ ] **Step 2: Single icons-on snapshot.** In each icon tab VM, capture `var iconsOn = Icons();` ONCE and use that same boolean for BOTH the `Columns` list AND the `rowValuesSelector` shape (don't call `Icons()` separately in the selector). This guarantees columns and row-values always have matching shape/order. Example (Items):
```csharp
var iconsOn = Icons();
Browser = new EntityBrowserViewModel<ItemRecord>(
    ...,
    rowValuesSelector: x => iconsOn
        ? new object?[] { x.IconFileName ?? "", x.ItemId, x.NameEn }
        : new object?[] { x.ItemId, x.NameEn },
    ...)
{
    Columns = iconsOn
        ? new[] { new BrowserColumn("Icon", 44, IsImage: true, ImageSize: 36), new BrowserColumn("ID", 80), new BrowserColumn("Name", 460, Fill: true) }
        : new[] { new BrowserColumn("ID", 80), new BrowserColumn("Name", 460, Fill: true) },
};
```
Apply the same one-snapshot rule to Skills, Buffs, Summons.

- [ ] **Step 3: Build + smoke launch.** Full icon rendering requires the owner's DB + icons folder (see final task); the structural alignment fix is verified by code review + that columns/row-values now derive from one `iconsOn`.
- [ ] **Step 4: Commit** — `git commit -am "fix(desktop): align grid columns with row values via single icons-on snapshot (#3)"`.

---

## PHASE D — Settings + action panels

### Task D1: Row height setting in the Settings window

**Files:** Modify `src/App.Desktop/Features/Settings/SettingsViewModel.cs`, `src/App.Desktop/Features/Settings/SettingsWindow.axaml`.

- [ ] **Step 1: VM.** Add `int RowHeight` (two-way) to the working settings, default from `AppSettings.RowHeight`, clamped 18–48; include it in the save mapping (read current `ReadIntoSettings`/equivalent and add `RowHeight`).
- [ ] **Step 2: XAML.** In the General tab add:
```xml
<StackPanel Orientation="Horizontal" Spacing="8">
  <TextBlock Text="Row height" VerticalAlignment="Center" Width="120"/>
  <Slider Minimum="18" Maximum="48" Value="{Binding RowHeight, Mode=TwoWay}" Width="220"/>
  <TextBlock Text="{Binding RowHeight}" VerticalAlignment="Center"/>
</StackPanel>
```
- [ ] **Step 3:** Ensure on Settings OK the shell re-applies settings (holder.Set + `IconCache.Configure` + browsers' `ColumnsChanged`/row-height refresh). Wire the existing settings-applied path to also refresh open browsers.
- [ ] **Step 4: Build + smoke launch**, open Settings, confirm the slider shows/edits row height.
- [ ] **Step 5: Commit** — `git commit -am "feat(desktop): row height setting in Settings (18-48)"`.

### Task D2: Redesign action panels (all 8 tabs)

**Files:** Modify `Features/{Playerchecker,Monster,Items,Skills,Buffs,Npc,Summons,Warp}/*TabView.axaml` (and remove placeholder-only bindings in the matching VMs if any).

Shared pattern (apply to each tab's action content hosted in `EntityBrowserView.Actions`):
- Wrap fields in sections, each preceded by `<TextBlock Classes="sectionHeader" Text="..."/>` + a `Border Classes="sectionDivider"`.
- Lay fields in a responsive grid: a `WrapPanel` or a `UniformGrid`/`Grid` that reflows; simplest responsive option in Avalonia is `<WrapPanel>` with fixed-width labeled field blocks (~130px) so they wrap as the panel narrows. Each field block: `StackPanel` with a small label `TextBlock` (12px, MutedForeground) above the input (`NumericUpDown`/`ComboBox`/`TextBox`, height 28).
- Button rows: `WrapPanel`/`StackPanel` of equal buttons; emphasize the primary with `Classes="primary"` (add a `Button.primary` style in Controls.axaml: 2px accent border).

- [ ] **Step 1:** Implement the shared field-block + section pattern for **Items** first (reference tab), binding to the existing `ItemsTabViewModel` properties/commands (unchanged). Remove the "Random Option"/"Itemuseflag" placeholder sub-tabs entirely.
- [ ] **Step 2:** Apply the same pattern to Monster, Skills, Buffs, NPC, Summons, Warp, Playerchecker — each binds to its existing VM members (do not change VM logic; this is XAML restructuring). For **Warp**, remove the disabled "OpenWorldmap" button. For **Playerchecker**, keep the inventory/warehouse `DataGrid` below; apply density there too (row height binding optional; keep readable).
- [ ] **Step 3:** Remove any VM members that existed solely for the removed placeholders (check Items/Warp VMs; most likely none — the placeholders were XAML-only).
- [ ] **Step 4: Build + smoke launch**, switch to each tab, confirm panels render, reflow on resize, and have no placeholders.
- [ ] **Step 5: Commit** — `git commit -am "feat(desktop): redesign action panels (responsive sections), drop placeholders"`.

---

## PHASE E — Verification

### Task E1: Full UI verification

- [ ] **Step 1: Build** the solution Release + Debug → 0/0.
- [ ] **Step 2: Self-check harness** (temporary, as used previously): construct every tab View + Settings + About, log PASS/FAIL, revert after. Expect 10/10.
- [ ] **Step 3: Launch + click through** every tab and open Settings/About; confirm: text tabs with active underline, top bar, denser grid + always-on scrollbar, roomier responsive action panels, no command history, bigger logo, row-height slider in Settings. No `[FTL]` in the log.
- [ ] **Step 4: Note for owner acceptance:** icon rendering (#3) and live data require the owner's DB + icons folder — the structural fix is in; confirm visually with real data on first run.
- [ ] **Step 5: Commit any final fixes** and stop (the finishing-a-development-branch skill handles merge).

---

## Self-review notes

- **Coverage:** #1 density (A1, C2) · #2 no-icon tabs/sections (A1, B1, D2) · #3 icon/column bug (C1, C3) · #4 actions readability/responsive (D2) · #5 always-on scrollbar (C2) · #6 smaller/narrower grid + more action room (A1, C2) · #7 bigger logo (B1) · #8 remove history (A3, B1) · #9 row height setting + row-scaled icons (A2, C2, D1) · #10 layout rethink (B1, C2, D2). All mapped.
- **Architecture preserved:** `ITabModule`, MVVM, DI, theme dictionaries unchanged; changes are View/theme-layer + one `AppSettings` field + removal of an unused service.
- **No-placeholder caveat:** for the 8-tab action-panel restructure (D2), the shared XAML pattern is specified once and each tab binds to its existing, unchanged VM members — the executor reads each current `*TabView.axaml`/VM and restructures the layout to the pattern. New/changed infrastructure (top bar, theme styles, row-height wiring, dispatcher, settings sync) has concrete code above.
- **Type consistency:** new names used consistently — `TopBarViewModel`/`TopBarView`, `AppSettings.RowHeight`, `EntityBrowserViewModel.RowHeight` + `ColumnsChanged`, `SettingsBootstrap.EnsureDefaults/ApplyEnvironmentDefaults`, `iconsOn` snapshot.
