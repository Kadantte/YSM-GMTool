# YSM-GMTool — Avalonia + ReactiveUI Rewrite Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rewrite the WinForms Rappelz GM tool from scratch as a clean Avalonia + ReactiveUI desktop app with a typed `ITabModule` extensibility model, hardcoded Lua commands, and 100% functional parity.

**Architecture:** Three layers — `App.Core` (dependency-free domain: models, settings, services, hardcoded Lua command catalog), `App.Data` (Dapper repository + connection factory, keeps `queries.json` verbatim for MSSQL+MySQL), and `App.Desktop` (Avalonia/ReactiveUI: DI bootstrap, dark theme, generic `EntityBrowser` VM/View, shell with sidebar + command history, and one self-contained module per tab). MVVM via ReactiveUI; DI via Microsoft.Extensions.DependencyInjection bridged to ReactiveUI's view location via Splat.

**Tech Stack:** .NET 10, Avalonia 11.x, Avalonia.ReactiveUI, Avalonia.Controls.DataGrid, Avalonia.Themes.Fluent, ReactiveUI, Microsoft.Extensions.DependencyInjection, Splat.Microsoft.Extensions.DependencyInjection, Projektanker.Icons.Avalonia(.FontAwesome), Dapper, Microsoft.Data.SqlClient, MySqlConnector, Serilog + Serilog.Sinks.File, System.Text.Json. **No test project** (per owner decision).

---

## Decisions taken (resolving inventory gaps)

These resolve the open questions surfaced by the parity inventory. They favor a clean, bug-free result while preserving behavior that matters. Raise objections during plan review.

1. **Command history (was dead):** the central dispatcher Adds the **final, `/run`-prefixed** command (exactly what is copied) to `ICommandHistoryService`, in-memory only (no cross-restart persistence — matches current intent). History is surfaced as a **permanent panel in the right sidebar** (replaces the orphan `tabCommandOverview`).
2. **Target-player selector (`cmbPlayers`):** rewritten as a **non-editable** dropdown of saved players (managed by New/＋/－). This fixes the WinForms editable-combo trap where free-typed text silently resolved to `self`.
3. **Orphan Lua templates** (`playerCheck`, `resetSkilltreeForPlayer`, `resetSummonSkillForPlayer`): **dropped** — they had no UI and were inert. Parity preserved (no behavior change).
4. **`queries.json`:** kept **verbatim** (both providers, all latent SQL quirks incl. token inconsistencies, `OFFSET 0 ROWS`, `COLLATE DATABASE_DEFAULT`, hard-coded per-query TOP/LIMIT). Do **not** "normalize" it. Relocated to `App.Desktop/Config/queries.json` (copied to output).
5. **Lua commands:** hardcoded in C# as a typed `LuaCommands` catalog in `App.Core`, formatted with **InvariantCulture** (via `FormattableString.Invariant`), centralized single/double-quote escaping. Exact strings preserved, including the literal `{durationMinutes}*100*60` and the `//regenerate` no-prefix rule.
6. **`settings.json`:** same path (`%LocalAppData%\YSM-GMTool\settings.json`), System.Text.Json with `JsonStringEnumConverter` added (back-compatible — also reads legacy integer provider values). `AppSettings` shape + `Clone()` preserved.
7. **Inventory/Warehouse header label:** surfaced as a **visible title** above the inventory grid (small UX win; was computed but never shown).
8. **Theme tokens:** canonical dark palette below; grid surface `#1E2229`, sidebar `#1E222A` (kept distinct as in source).
9. **`.env`:** preserved — env override at startup (env > settings.json) and dual-write of provider + connection string on Settings save; discovery order preserved.
10. **Placeholders:** "Random Option"/"Itemuseflag" sub-tabs and the disabled `OpenWorldmap` button are kept as visible disabled placeholders (parity, zero feature loss).
11. **Dapper `MatchNamesWithUnderscores`:** set **explicitly at composition root** before any query (not via a static ctor side-effect).
12. **Grid selection:** multi-select visual, single `SelectedRecord` drives actions (parity).
13. **`UiLayoutPolicy`:** deleted entirely — native Avalonia layout.

---

## Theme tokens (exact, from WinForms `ApplyReadabilityPalette`)

| Token | Hex | Usage |
|---|---|---|
| `WindowBackground` | `#181B21` | window / tab background |
| `SidebarBackground` | `#1E222A` | right sidebar |
| `PanelBackground` | `#222730`–`#242932` | action panels / inputs |
| `PanelAltBackground` | `#242932` | alt rows, inputs |
| `GridBackground` / `RowBackground` | `#1E2229` | data grid surface |
| `Foreground` | `#EBEEF5` | primary text |
| `MutedForeground` | `#BCC4D0` | labels/status |
| `Border` / `GridLine` | `#444E5F` | borders, grid lines |
| `HeaderBackground` | `#303743` | column headers |
| `Accent` / `SelectionBackground` | `#4876C4` | selection highlight |
| `SelectionForeground` | `#FFFFFF` | selected text |
| `IconColor` | `#DCDCDC` | toolbar icons |

---

## File structure

```
src/
  App.Core/                                  (net10.0; no UI/DB deps)
    Enums/DatabaseProvider.cs                MSSQL, MySQL
    Enums/QueryEntity.cs                      13 members (Npc -> "NPC")
    Models/AppSettings.cs                     + Clone()
    Models/DatabaseConnectionSettings.cs      + Clone()
    Models/TableNameSettings.cs               + Clone(), ToTokenMap()
    Models/WarpLocationSettings.cs            + Clone()
    Models/Entities/*.cs                       8 record models (init-only)
    Abstractions/IAppSettingsService.cs
    Abstractions/ICommandHistoryService.cs
    Abstractions/IConnectionStringBuilderService.cs
    Abstractions/IGameDataRepository.cs
    Abstractions/ILocalCacheService.cs
    Abstractions/INameNormalizer.cs
    Abstractions/IQueryStore.cs
    Commands/LuaEscape.cs                      single/double-quote escaping
    Commands/LuaCommands.cs                    ALL hardcoded Lua (replaces lua_commands.json)
    Services/JsonAppSettingsService.cs
    Services/FileQueryStore.cs
    Services/SearchNameNormalizer.cs
    Services/DefaultConnectionStringBuilderService.cs
    Services/CommandHistoryService.cs
    Services/LocalCacheService.cs
  App.Data/                                  (net10.0)
    Infrastructure/DbConnectionFactory.cs
    Repositories/GameDataRepository.cs
  App.Desktop/                               (net10.0, Avalonia)
    Config/queries.json                        (copied verbatim from App.WinForms/Config)
    Assets/Heaven_logo1.png, Logo.ico
    Program.cs                                  entry point + DI + Serilog + .env
    App.axaml / App.axaml.cs                    Fluent dark + ViewLocator + resources
    Composition/ServiceCollectionExtensions.cs  DI registrations + module scan
    Theme/Dark.axaml                            color tokens + control styles
    Infrastructure/ReactiveViewLocator.cs       VM->View via DI
    Infrastructure/AvaloniaClipboardService.cs  IClipboardService
    Infrastructure/DialogService.cs (+ DialogWindow.axaml)  IDialogService
    Services/CommandDispatcher.cs               ICommandDispatcher
    Services/PlayerContext.cs                   IPlayerContext (target player + list)
    Services/AppSettingsHolder.cs               IAppSettingsHolder (live settings)
    Modules/ITabModule.cs                       Title/IconKey/Order
    Modules/TabModuleViewModel.cs               base : ReactiveObject, ITabModule
    Controls/EntityBrowserView.axaml(.cs)       generic grid+search+actions slot
    ViewModels/EntityBrowserViewModel.cs        generic<TRecord>
    ViewModels/BrowserRow.cs, BrowserColumn.cs, SearchMode.cs
    Shell/MainWindow.axaml(.cs)
    Shell/ShellViewModel.cs                     tabs + sidebar + history
    Shell/SidebarView.axaml(.cs)                player mgmt + append toggle + history
    Features/Items/ItemsTabViewModel.cs, ItemsTabView.axaml(.cs)
    Features/Monster/...  Skills/... Buffs/... Npc/... Summons/... Warp/... Playerchecker/...
    Features/Settings/SettingsViewModel.cs, SettingsWindow.axaml(.cs)
    Features/About/AboutWindow.axaml(.cs)
  (App.WinForms/ deleted at the end)
```

---

## Verification model (no test project)

Each task ends with **build-green** and, where relevant, **run-and-observe**:
- Build: `dotnet build YSM-GMTool.slnx -c Debug` → expect `Build succeeded. 0 Error(s)`.
- Run: `dotnet run --project src/App.Desktop/App.Desktop.csproj` → observe the described behavior.
- Commit after each task.

---

## PHASE 0 — Solution scaffold

### Task 0.1: Create the App.Desktop Avalonia project

**Files:**
- Create: `src/App.Desktop/App.Desktop.csproj`
- Modify: `YSM-GMTool.slnx`

- [ ] **Step 1: Install Avalonia templates (once) and scaffold**

```bash
dotnet new install Avalonia.Templates
dotnet new avalonia.reactiveui -o src/App.Desktop -n App.Desktop
```

- [ ] **Step 2: Set csproj target + packages**

Replace `src/App.Desktop/App.Desktop.csproj` with:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
    <ApplicationIcon>Assets/Logo.ico</ApplicationIcon>
    <AssemblyName>GM Tool</AssemblyName>
    <Product>GM Tool</Product>
    <RuntimeIdentifiers>win-x64</RuntimeIdentifiers>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.*" />
    <PackageReference Include="Avalonia.Desktop" Version="11.*" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.*" />
    <PackageReference Include="Avalonia.Controls.DataGrid" Version="11.*" />
    <PackageReference Include="Avalonia.ReactiveUI" Version="11.*" />
    <PackageReference Include="Avalonia.Diagnostics" Version="11.*" Condition="'$(Configuration)'=='Debug'" />
    <PackageReference Include="Projektanker.Icons.Avalonia.FontAwesome" Version="9.*" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.*" />
    <PackageReference Include="Splat.Microsoft.Extensions.DependencyInjection" Version="15.*" />
    <PackageReference Include="Serilog" Version="4.*" />
    <PackageReference Include="Serilog.Sinks.File" Version="7.*" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\App.Core\App.Core.csproj" />
    <ProjectReference Include="..\App.Data\App.Data.csproj" />
  </ItemGroup>
  <ItemGroup>
    <AvaloniaResource Include="Assets\**" />
    <None Update="Config\queries.json"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory></None>
  </ItemGroup>
</Project>
```

- [ ] **Step 3: Copy assets + queries.json verbatim**

```bash
mkdir -p src/App.Desktop/Config src/App.Desktop/Assets
cp src/App.WinForms/Config/queries.json src/App.Desktop/Config/queries.json
cp src/App.WinForms/Heaven_logo1.png src/App.Desktop/Assets/Heaven_logo1.png
cp src/App.WinForms/Logo.ico src/App.Desktop/Assets/Logo.ico
```

- [ ] **Step 4: Add project to solution**

```bash
dotnet sln YSM-GMTool.slnx add src/App.Desktop/App.Desktop.csproj
```

- [ ] **Step 5: Build (scaffold compiles)**

Run: `dotnet build src/App.Desktop/App.Desktop.csproj`
Expected: `Build succeeded`.

- [ ] **Step 6: Commit**

```bash
git add -A && git commit -m "chore: scaffold App.Desktop Avalonia project"
```

---

## PHASE 1 — App.Core rewrite

> App.Core is dependency-free. Most models/services are ports of the existing (good) code; the new piece is `Commands/`. Interfaces move to an `Abstractions/` folder; `ILuaCommandBuilder`/`ILuaCommandTemplateStore` are NOT recreated.

### Task 1.1: Port enums, entity records, settings models

**Files:**
- Create: `src/App.Core/Enums/DatabaseProvider.cs`, `Enums/QueryEntity.cs`
- Create: `src/App.Core/Models/Entities/{Player,InventoryItem,Monster,Item,Skill,State,Npc,Summon}Record.cs`
- Create: `src/App.Core/Models/{AppSettings,DatabaseConnectionSettings,TableNameSettings,WarpLocationSettings}.cs`

- [ ] **Step 1: Port verbatim from existing App.Core**

These classes already exist and are correct. Copy them 1:1 from the current `src/App.Core/...` into the rewritten App.Core (same namespaces, same property names, same defaults, keep `Clone()` and `ToTokenMap()`). Field-level parity reference (do not change names/defaults):

- `DatabaseProvider { MSSQL = 0, MySQL = 1 }`
- `QueryEntity { Playerchecker, PlayercheckerByCharName, PlayercheckerByAccount, PlayercheckerAll, PlayercheckerOnline, PlayerInventory, PlayerWarehouse, Monsters, Items, Skills, States, Npc, Summons }`
- Entity records (all `sealed class`, `{ get; init; }`): see the column→property table in the data appendix below. Keep `string` defaults `""`, `OnlineStatus` default `"OFFLINE"`.
- `AppSettings`: Provider=MSSQL, ConnectionString="", Connection=new(), TableNames=new(), Players=[], SelectedPlayer=null, AppendGeneratedCommands=true, LimitSelectQueries=true, UseLocalCache=false, EnableEntityIcons=false, EntityIconsPath="", WarpLocations=[]; deep `Clone()`.
- `DatabaseConnectionSettings`: Server="127.0.0.1", Port=1433, Database="", UserId="", Password="", IntegratedSecurity=false, Encrypt=true, TrustServerCertificate=true; `Clone()`.
- `TableNameSettings`: 12 string tokens with defaults (Arcadia, Telecaster, Auth, Accounts, CharacterResource, MonsterResource, StringResource, ItemResource, SkillResource, StateResource, NpcResource, SummonResource); `Clone()`; `ToTokenMap()` → `IReadOnlyDictionary<string,string>` (StringComparer.Ordinal; blank value falls back to the key name; non-blank trimmed).
- `WarpLocationSettings`: X=0, Y=0, Name=""; `Clone()`.

- [ ] **Step 2: Build**

Run: `dotnet build src/App.Core/App.Core.csproj` → `Build succeeded`.

- [ ] **Step 3: Commit** — `git commit -am "feat(core): port enums, entity records, settings models"`

### Task 1.2: Port services + abstractions (minus Lua builder)

**Files:**
- Create: `src/App.Core/Abstractions/{IAppSettingsService,ICommandHistoryService,IConnectionStringBuilderService,IGameDataRepository,ILocalCacheService,INameNormalizer,IQueryStore}.cs`
- Create: `src/App.Core/Services/{JsonAppSettingsService,FileQueryStore,SearchNameNormalizer,DefaultConnectionStringBuilderService,CommandHistoryService,LocalCacheService}.cs`

- [ ] **Step 1: Port verbatim, with one change to JsonAppSettingsService**

Copy all listed services/interfaces 1:1 from current App.Core (logic preserved exactly — `FileQueryStore.ResolveEntityKey` keeps `Npc => "NPC"`; `SearchNameNormalizer` keeps diacritic-stripping; `DefaultConnectionStringBuilderService` keeps both providers' Build/TryParse; `LocalCacheService` keeps `cache/{key}.json` + `GetCacheDate`; `CommandHistoryService` in-memory). Do **not** port `ILuaCommandBuilder`, `ILuaCommandTemplateStore`, `LuaCommandBuilder`, `FileLuaCommandTemplateStore`.

Change ONLY `JsonAppSettingsService.JsonOptions` to add the string-enum converter (back-compatible with legacy integer enums):

```csharp
private static readonly JsonSerializerOptions JsonOptions = new()
{
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() },
};
```

- [ ] **Step 2: Build** → `dotnet build src/App.Core/App.Core.csproj`
- [ ] **Step 3: Commit** — `git commit -am "feat(core): port services + abstractions (drop lua json builder)"`

### Task 1.3: Hardcoded Lua command catalog

**Files:**
- Create: `src/App.Core/Commands/LuaEscape.cs`
- Create: `src/App.Core/Commands/LuaCommands.cs`

- [ ] **Step 1: Escaping helper**

```csharp
namespace App.Core.Commands;

public static class LuaEscape
{
    /// <summary>Escape for a Lua single-quoted string literal: '\' then '\''.</summary>
    public static string Single(string value)
        => (value ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("'", "\\'", StringComparison.Ordinal);

    /// <summary>Escape for a Lua double-quoted string literal: '\' then '\"'.</summary>
    public static string Double(string value)
        => (value ?? string.Empty).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal);
}
```

- [ ] **Step 2: LuaCommands catalog (verbatim ports of every used template)**

`FormattableString.Invariant` guarantees invariant numeric formatting (matches the old `Convert.ToString(.., InvariantCulture)`). Player names are escaped inside the builder, so callers pass raw names.

```csharp
using static System.FormattableString;

namespace App.Core.Commands;

public static class LuaCommands
{
    // Monster
    public static string MonsterRegenerate(int monsterId, int count)
        => Invariant($"//regenerate {monsterId} {count}");
    public static string MonsterAddNpcAtPlayer(string playerName, int monsterId, int count, int minutesLifetime)
    {
        var p = LuaEscape.Single(playerName);
        return Invariant($"add_npc( gv('x','{p}'), gv('y','{p}'),{monsterId},{count},{minutesLifetime}, gv('layer','{p}'))");
    }
    public static string MonsterAddNpcAtCoords(int x, int y, int monsterId, int count, int minutesLifetime, int layer)
        => Invariant($"add_npc( {x},{y},{monsterId},{count},{minutesLifetime},{layer})");

    // Items
    public static string InsertItemSelf(int itemId, int amount, int enhance, int level, int statusFlag)
        => Invariant($"insert_item({itemId},{amount},{enhance},{level},{statusFlag})");
    public static string InsertItemPlayer(int itemId, int amount, int enhance, int level, int statusFlag, string playerName)
        => Invariant($"insert_item({itemId},{amount},{enhance},{level},{statusFlag},'{LuaEscape.Single(playerName)}')");
    public static string SetWearItemLevelOwn(int wearSlot, int level)
        => Invariant($"set_item_level(get_wear_item_handle({wearSlot}),{level})");
    public static string SetWearItemLevelPlayer(int wearSlot, string playerName, int level)
        => Invariant($"set_item_level(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{level})");
    public static string SetWearItemEnhanceOwn(int wearSlot, int enhance)
        => Invariant($"set_item_enhance(get_wear_item_handle({wearSlot}),{enhance})");
    public static string SetWearItemEnhancePlayer(int wearSlot, string playerName, int enhance)
        => Invariant($"set_item_enhance(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{enhance})");
    public static string SetWearItemAppearanceOwn(int wearSlot, int itemCode)
        => Invariant($"set_item_appearance_code(get_wear_item_handle({wearSlot}),{itemCode})");
    public static string SetWearItemAppearancePlayer(int wearSlot, string playerName, int itemCode)
        => Invariant($"set_item_appearance_code(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{itemCode})");
    public static string ChangeWearItemCodeOwn(int wearSlot, int itemCode)
        => Invariant($"change_item_code(get_wear_item_handle({wearSlot}),{itemCode})");
    public static string ChangeWearItemCodePlayer(int wearSlot, string playerName, int itemCode)
        => Invariant($"change_item_code(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{itemCode})");

    // Skills
    public static string LearnSkill(int skillId) => Invariant($"learn_skill({skillId})");
    public static string LearnSkillForPlayer(int skillId, string playerName)
        => Invariant($"learn_skill({skillId},'{LuaEscape.Single(playerName)}')");
    public static string SetSkill(int skillId, int level) => Invariant($"set_skill({skillId},{level})");
    public static string SetSkillForPlayer(int skillId, int level, string playerName)
        => Invariant($"set_skill({skillId},{level},'{LuaEscape.Single(playerName)}')");
    public static string LearnAllSkill(string playerName)
        => Invariant($"learn_all_skill('{LuaEscape.Single(playerName)}')");
    public static string RemoveSkill(int skillId) => Invariant($"remove_skill({skillId})");
    public static string RemoveSkillForPlayer(int skillId, string playerName)
        => Invariant($"remove_skill({skillId},'{LuaEscape.Single(playerName)}')");
    public static string LearnCreatureSkillSelf(int skillId)
        => Invariant($"creature_learn_skill({skillId}, gcv(get_creature_handle(0), \"handle\"))");
    public static string LearnCreatureSkill(int skillId, string playerName)
        => Invariant($"creature_learn_skill({skillId}, gcv(get_creature_handle(0), \"handle\"), '{LuaEscape.Single(playerName)}')");
    public static string LearnCreatureAllSkill(int slotIndex)
        => Invariant($"learn_creature_all_skill({slotIndex})");
    public static string LearnCreatureAllSkillForPlayer(int slotIndex, string playerName)
        => Invariant($"learn_creature_all_skill({slotIndex},'{LuaEscape.Single(playerName)}')");

    // Buffs / states (duration emitted literally as N*100*60)
    public static string CastWorldState(int stateId, int level, int durationMinutes)
        => Invariant($"cast_world_state({stateId},{level},{durationMinutes}*100*60)");
    public static string AddEventState(int stateId, int level)
        => Invariant($"add_event_state({stateId},{level})");
    public static string RemoveEventState(int stateId)
        => Invariant($"remove_event_state({stateId},get_state_level({stateId}))");
    public static string AddPlayerState(int stateId, int level, int durationMinutes, string playerName)
        => Invariant($"add_state({stateId},{level},{durationMinutes}*100*60,'{LuaEscape.Single(playerName)}')");
    public static string RemovePlayerState(int stateId, string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"remove_state({stateId},get_state_level({stateId},'{p}'),'{p}')"); }
    public static string AddCreatureState(int stateId, int level, int durationMinutes, string playerName)
        => Invariant($"add_cstate({stateId},{level},{durationMinutes}*100*60,'{LuaEscape.Single(playerName)}')");
    public static string RemoveCreatureState(int stateId, string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"remove_cstate({stateId},get_state_level({stateId},'{p}'),'{p}')"); }

    // NPC
    public static string AddNpcToWorld(int x, int y, int layer, int npcId)
        => Invariant($"add_npc_to_world({x},{y},{layer},{npcId})");
    public static string AddNpcToWorldForPlayer(int x, int y, int layer, string playerName, int npcId)
        => Invariant($"add_npc_to_world({x},{y},{layer},'{LuaEscape.Single(playerName)}',{npcId})");
    public static string ShowNpc(int x, int y, int npcId, int layer, int visible)
        => Invariant($"show_npc({x},{y},{npcId},{layer},{visible})");
    public static string WarpToNpcCoordinates(int x, int y, string playerName) // NOTE: double-quote escaping
        => Invariant($"warp({x},{y},\"{LuaEscape.Double(playerName)}\")");

    // Warp
    public static string WarpToLocationForPlayer(int x, int y, string playerName)
        => Invariant($"warp({x},{y},'{LuaEscape.Single(playerName)}')");
    public static string WarpPlayerToYou(string playerName)
        => Invariant($"warp(gv(\"x\"),gv(\"y\"),'{LuaEscape.Single(playerName)}')");
    public static string WarpYouToPlayer(string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"warp(gv(\"x\",'{p}'),gv(\"y\",'{p}'))"); }

    // Summons
    public static string InsertSummonById(int summonId)
        => Invariant($"insert_summon_by_summon_id({summonId})");
    public static string InsertSummonByIdWithStage(int summonId, int stage)
        => Invariant($"insert_summon_by_summon_id({summonId},{stage})");
    public static string StageSummon(int slot, int stage)
        => Invariant($"creature_enhance({slot},{stage})");
}
```

- [ ] **Step 3: Build** → `dotnet build src/App.Core/App.Core.csproj`
- [ ] **Step 4: Commit** — `git commit -am "feat(core): hardcoded Lua command catalog + escaping"`

---

## PHASE 2 — App.Data rewrite (keeps queries.json)

### Task 2.1: Port DbConnectionFactory + GameDataRepository

**Files:**
- Create: `src/App.Data/Infrastructure/DbConnectionFactory.cs`
- Create: `src/App.Data/Repositories/GameDataRepository.cs`

- [ ] **Step 1: Port verbatim with one change**

Copy both files 1:1 from current App.Data (Dapper, `CommandDefinition`, `ApplyQueryTokens` ordinal `{{Token}}` replace, `%term%` LIKE wrapping, per-method QueryEntity mapping, `TestConnectionAsync` open/close). **Change:** remove the `static GameDataRepository()` ctor that set `DefaultTypeMap.MatchNamesWithUnderscores`; the flag is set explicitly at startup (Task 3.1) instead. Keep all method signatures identical to `IGameDataRepository`.

- [ ] **Step 2: Build** → `dotnet build src/App.Data/App.Data.csproj`
- [ ] **Step 3: Commit** — `git commit -am "feat(data): port repository + connection factory"`

---

## PHASE 3 — App.Desktop foundation

### Task 3.1: Program.cs, DI, Serilog, .env, Dapper flag

**Files:**
- Create: `src/App.Desktop/Program.cs`
- Create: `src/App.Desktop/Composition/ServiceCollectionExtensions.cs`
- Create: `src/App.Desktop/app.manifest` (DPI-aware; Avalonia template provides one — keep it)

- [ ] **Step 1: ServiceCollectionExtensions — register everything + module scan**

```csharp
using System.Reflection;
using App.Core.Abstractions;
using App.Core.Services;
using App.Data.Infrastructure;
using App.Data.Repositories;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGmTool(this IServiceCollection s, string appDir)
    {
        var queryFile = System.IO.Path.Combine(AppContext.BaseDirectory, "Config", "queries.json");
        var settingsFile = System.IO.Path.Combine(appDir, "settings.json");

        // Core services
        s.AddSingleton<IQueryStore>(_ => new FileQueryStore(queryFile));
        s.AddSingleton<IAppSettingsService>(_ => new JsonAppSettingsService(settingsFile));
        s.AddSingleton<INameNormalizer, SearchNameNormalizer>();
        s.AddSingleton<IConnectionStringBuilderService, DefaultConnectionStringBuilderService>();
        s.AddSingleton<ICommandHistoryService, CommandHistoryService>();
        s.AddSingleton<ILocalCacheService>(_ => new LocalCacheService(appDir));
        s.AddSingleton<DbConnectionFactory>();
        s.AddSingleton<IGameDataRepository, GameDataRepository>();

        // Desktop infra
        s.AddSingleton<IClipboardService, AvaloniaClipboardService>();
        s.AddSingleton<IDialogService, DialogService>();
        s.AddSingleton<IAppSettingsHolder, AppSettingsHolder>();
        s.AddSingleton<IPlayerContext, PlayerContext>();
        s.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        // Shell + settings/about VMs
        s.AddSingleton<Shell.ShellViewModel>();
        s.AddTransient<Features.Settings.SettingsViewModel>();

        s.AddTabModules(typeof(ServiceCollectionExtensions).Assembly);
        return s;
    }

    /// <summary>Reflection-scan for ITabModule implementations and register each + IEnumerable&lt;ITabModule&gt;.</summary>
    public static IServiceCollection AddTabModules(this IServiceCollection s, Assembly asm)
    {
        foreach (var t in asm.GetTypes()
                     .Where(t => !t.IsAbstract && typeof(ITabModule).IsAssignableFrom(t)))
        {
            s.AddSingleton(t);
            s.AddSingleton(typeof(ITabModule), sp => (ITabModule)sp.GetRequiredService(t));
        }
        return s;
    }
}
```

- [ ] **Step 2: Program.cs — entry point, Serilog, .env, Dapper flag, Splat bridge**

```csharp
using App.Desktop.Composition;
using Avalonia;
using Avalonia.ReactiveUI;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace App.Desktop;

internal static class Program
{
    public static IServiceProvider Services { get; private set; } = default!;

    [STAThread]
    public static void Main(string[] args)
    {
        var appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YSM-GMTool");
        var logsDir = Path.Combine(appDir, "logs");
        Directory.CreateDirectory(logsDir);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logsDir, "gmtool-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Map snake_case/SCREAMING SQL aliases -> PascalCase record props (explicit, before any query).
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        DotEnv.LoadIfPresent(appDir);

        var collection = new ServiceCollection().AddGmTool(appDir);
        collection.UseMicrosoftDependencyResolver();              // bridge -> Splat for ReactiveUI view location
        var resolver = Splat.Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();
        Services = collection.BuildServiceProvider();
        Services.UseMicrosoftDependencyResolver();

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            throw;
        }
        finally { Log.CloseAndFlush(); }
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace().UseReactiveUI();
}
```

- [ ] **Step 3: DotEnv helper (port discovery + parse from current Program.cs)**

Create `src/App.Desktop/Infrastructure/DotEnv.cs` with `LoadIfPresent(string appDir)` ported from the WinForms `Program.LoadDotEnvIfPresent`/`FindDotEnvPath` (same search roots: `AppContext.BaseDirectory`, `Directory.GetCurrentDirectory()`, `appDir`, plus parent walk; KEY=VALUE parse; strip surrounding quotes; `Environment.SetEnvironmentVariable`). Keys: `YSM_DB_PROVIDER`, `YSM_DB_CONNECTION_STRING`.

- [ ] **Step 4: Build** (will fail until App.axaml exists — proceed to 3.2 then build)
- [ ] **Step 5: Commit** after 3.2 builds.

### Task 3.2: App.axaml, theme, ViewLocator

**Files:**
- Create/replace: `src/App.Desktop/App.axaml`, `App.axaml.cs`
- Create: `src/App.Desktop/Theme/Dark.axaml`
- Create: `src/App.Desktop/Infrastructure/ReactiveViewLocator.cs`

- [ ] **Step 1: Theme/Dark.axaml** — a `ResourceDictionary` defining the brushes from the theme-tokens table (`WindowBackground`, `SidebarBackground`, `PanelBackground`, `GridBackground`, `Foreground`, `MutedForeground`, `Border`, `HeaderBackground`, `Accent`, `SelectionForeground`, `IconColor`) as `SolidColorBrush` resources, plus baseline `Styles` for `DataGrid`, `Button`, `TextBox`, `ComboBox`, `NumericUpDown`, `TabControl` matching the dark palette.

- [ ] **Step 2: App.axaml** — Fluent theme (Dark variant) + DataGrid theme + FontAwesome + merged Dark.axaml:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:icons="clr-namespace:Projektanker.Icons.Avalonia;assembly=Projektanker.Icons.Avalonia"
             x:Class="App.Desktop.App"
             RequestedThemeVariant="Dark">
  <Application.Styles>
    <FluentTheme />
    <StyleInclude Source="avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml" />
    <icons:IconSettings />
  </Application.Styles>
  <Application.Resources>
    <ResourceDictionary>
      <ResourceDictionary.MergedDictionaries>
        <ResourceInclude Source="avares://App.Desktop/Theme/Dark.axaml" />
      </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
  </Application.Resources>
</Application>
```

- [ ] **Step 3: ReactiveViewLocator** — `IDataTemplate` resolving `*ViewModel` → `*View` from DI:

```csharp
using App.Desktop; // Program.Services
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop.Infrastructure;

public sealed class ReactiveViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        var vmName = data!.GetType().FullName!;
        var viewName = vmName.Replace("ViewModel", "View", StringComparison.Ordinal);
        var viewType = Type.GetType(viewName);
        if (viewType is null) return new TextBlock { Text = "View not found: " + viewName };
        return (Control)(Program.Services.GetService(viewType) ?? Activator.CreateInstance(viewType)!);
    }
    public bool Match(object? data) => data is ReactiveUI.IReactiveObject;
}
```

- [ ] **Step 4: App.axaml.cs** — wire ViewLocator + open MainWindow from DI:

```csharp
using App.Desktop.Infrastructure;
using App.Desktop.Shell;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace App.Desktop;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        DataTemplates.Add(new ReactiveViewLocator());
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = Program.Services.GetRequiredService<MainWindow>();
        base.OnFrameworkInitializationCompleted();
    }
}
```

Register `MainWindow` in DI (add `s.AddSingleton<Shell.MainWindow>();` in `AddGmTool`).

- [ ] **Step 5: Build** → `dotnet build src/App.Desktop/App.Desktop.csproj` (after MainWindow stub from Task 5.1 exists; if building now, add a temporary empty `MainWindow`). Expected once shell exists: succeeds.
- [ ] **Step 6: Commit** — `git commit -am "feat(desktop): bootstrap, DI, dark theme, view locator"`

### Task 3.3: Foundation services (clipboard, dialog, settings holder, player context, dispatcher)

**Files:**
- Create: `Infrastructure/IClipboardService.cs` + `AvaloniaClipboardService.cs`
- Create: `Infrastructure/IDialogService.cs` + `DialogService.cs` + `DialogWindow.axaml(.cs)`
- Create: `Services/IAppSettingsHolder.cs` + `AppSettingsHolder.cs`
- Create: `Services/IPlayerContext.cs` + `PlayerContext.cs`
- Create: `Services/ICommandDispatcher.cs` + `CommandDispatcher.cs`

- [ ] **Step 1: IClipboardService / AvaloniaClipboardService**

```csharp
namespace App.Desktop.Infrastructure;
public interface IClipboardService { Task SetTextAsync(string text); }
```

`AvaloniaClipboardService` resolves the top-level window clipboard:

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
namespace App.Desktop.Infrastructure;
public sealed class AvaloniaClipboardService : IClipboardService
{
    public async Task SetTextAsync(string text)
    {
        var top = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (top?.Clipboard is { } cb) await cb.SetTextAsync(text);
    }
}
```

- [ ] **Step 2: IDialogService + themed DialogWindow**

`IDialogService`:
```csharp
namespace App.Desktop.Infrastructure;
public interface IDialogService
{
    Task ShowInfoAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    Task<bool> ConfirmAsync(string title, string message);
}
```
`DialogWindow.axaml` = a small dark modal (title, message TextBlock, OK [+ Cancel for confirm]). `DialogService` opens it via `ShowDialog(owner)` where owner = MainWindow. Centralizes all the old `MessageBox.Show(...)` calls.

- [ ] **Step 3: IAppSettingsHolder** — single source of truth for live settings:

```csharp
using App.Core.Models;
namespace App.Desktop.Services;
public interface IAppSettingsHolder
{
    AppSettings Current { get; }
    event EventHandler? Changed;
    void Set(AppSettings settings);  // replaces Current, raises Changed
}
```
`AppSettingsHolder` stores `Current` (init to `new AppSettings()`), `Set` assigns + raises `Changed`.

- [ ] **Step 4: IPlayerContext** — target player + saved list, persists via settings:

```csharp
namespace App.Desktop.Services;
public interface IPlayerContext
{
    System.Collections.ObjectModel.ReadOnlyObservableCollection<string> Players { get; }
    string? SelectedPlayer { get; set; }                 // null/empty => "self"
    string Resolve();                                     // SelectedPlayer trimmed, else "self"
    bool TryResolveRequired(out string playerName);       // false when "self"
    void Add(string name);
    void RemoveSelected();
}
```
`PlayerContext` is seeded from `IAppSettingsHolder.Current.Players`/`SelectedPlayer`; on Add/Remove/Select it mutates the holder's settings and triggers a save via `IAppSettingsService` (debounced fire-and-forget). `Resolve()` => `string.IsNullOrWhiteSpace(SelectedPlayer) ? "self" : SelectedPlayer.Trim()`. `TryResolveRequired` => `Resolve()` and return `!equals("self", OrdinalIgnoreCase)`.

- [ ] **Step 5: ICommandDispatcher** — the single funnel (fixes history):

```csharp
namespace App.Desktop.Services;
public interface ICommandDispatcher { Task DispatchAsync(string luaCommand); }
```
```csharp
using App.Core.Abstractions;
using App.Desktop.Infrastructure;
namespace App.Desktop.Services;
public sealed class CommandDispatcher(IClipboardService clipboard, ICommandHistoryService history, IAppSettingsHolder settings) : ICommandDispatcher
{
    public async Task DispatchAsync(string luaCommand)
    {
        var final = ApplyRunPrefix(luaCommand);
        history.Add(final);                 // <-- the fix: record every dispatched command
        await clipboard.SetTextAsync(final);
    }
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

- [ ] **Step 6: Build + Commit** — `git commit -am "feat(desktop): clipboard/dialog/settings/player/dispatch services"`

### Task 3.4: ITabModule + base VM

**Files:** Create `Modules/ITabModule.cs`, `Modules/TabModuleViewModel.cs`

- [ ] **Step 1:**
```csharp
namespace App.Desktop.Modules;
public interface ITabModule
{
    string Title { get; }
    string IconKey { get; }   // FontAwesome key, e.g. "fa-solid fa-box"
    int Order { get; }
}
```
```csharp
using ReactiveUI;
namespace App.Desktop.Modules;
public abstract class TabModuleViewModel : ReactiveObject, ITabModule
{
    public abstract string Title { get; }
    public virtual string IconKey => string.Empty;
    public abstract int Order { get; }
}
```
- [ ] **Step 2: Build + Commit** — `git commit -am "feat(desktop): ITabModule contract + base VM"`

---

## PHASE 4 — Shared EntityBrowser (VM + View)

### Task 4.1: Browser support types

**Files:** Create `ViewModels/BrowserRow.cs`, `ViewModels/BrowserColumn.cs`, `ViewModels/SearchMode.cs`

- [ ] **Step 1:**
```csharp
namespace App.Desktop.ViewModels;
public enum SearchMode { ById, ByName, ByContactScript }
public sealed class BrowserRow(object tag, params object?[] values)
{
    public object Tag { get; } = tag;
    public object?[] Values { get; } = values;
    public object? this[int i] => i >= 0 && i < Values.Length ? Values[i] : null;
}
public sealed record BrowserColumn(string Header, int Width, bool Fill = false, bool IsImage = false, int ImageSize = 16);
```
- [ ] **Step 2: Build + Commit**

### Task 4.2: EntityBrowserViewModel&lt;TRecord&gt;

**Files:** Create `ViewModels/EntityBrowserViewModel.cs`

Port the presenter logic (Phase-8 inventory) into a ReactiveUI VM. Constructor contract mirrors the old presenter:

- [ ] **Step 1: Implement** with these members (reactive):
  - Ctor params: `loadAllAsync`, `idSelector`, `nameSelector`, `rowValuesSelector`, `INameNormalizer`, optional `searchableTextSelector`, `secondarySearchTextSelector`, `sqlSearchAsync`, `maxRowsSelector`.
  - Config props (set by tab VM): `IReadOnlyList<BrowserColumn> Columns`, `string ByIdLabel`, `string ByNameLabel`, `bool IdSearchEnabled`, `bool SecondarySearchEnabled`, `string SecondaryLabel`, `bool RealtimeVisible`, `bool LoadAllVisible`, `int DebounceMs`.
  - Observable state: `[Reactive]`-style props for `SearchText`, `SearchMode`, `RealtimeEnabled`, `Status`, `ObservableCollection<BrowserRow> Rows`, `BrowserRow? SelectedRow`, `TRecord? SelectedRecord`.
  - `ReactiveCommand LoadAll`, `ReactiveCommand Filter` (both async, with `.ThrownExceptions` → dialog).
  - Replicate: build search index (OrderBy id), in-memory PLINQ filter vs SQL path (presence of `sqlSearchAsync`), ID-range regex `^\s*(?<from>\d+)\s*-\s*(?<to>\d+)\s*$` (min/max swap), `ApplyRowLimit` (Take maxRows), numeric-vs-string sort on column click, **exact status strings** ("No data loaded. Click Load All.", "Loading data from database...", "Loaded {n:N0} record(s). Showing {m:N0}.", in-memory "Loaded {n:N0} records. Showing {m:N0}.", "Enter a search term and press Search.", "Searching...", "Found {n:N0} record(s). Showing {m:N0}."), provider-agnostic cancellation guard (OperationCanceledException OR message contains "Operation cancelled by user.").
  - Debounce: `this.WhenAnyValue(x => x.SearchText).Throttle(TimeSpan.FromMilliseconds(DebounceMs), RxApp.MainThreadScheduler)` gated by `RealtimeEnabled` → `Filter.Execute()`.
  - `SelectedRecord` derived from `SelectedRow?.Tag as TRecord`; expose `IObservable<TRecord?> WhenSelectedRecordChanged`.
  - Icon resolution: an `IconCache` (see Task 4.3) consulted by the View's image converter.
- [ ] **Step 2: Build + Commit** — `git commit -am "feat(desktop): generic EntityBrowserViewModel"`

### Task 4.3: Icon cache + converter

**Files:** Create `Infrastructure/IconCache.cs`, `Infrastructure/IconKeyToBitmapConverter.cs`

- [ ] **Step 1:** Port resolution/letterbox/negative-cache logic into an `IconCache` keyed `"{size}:{trimmedKey}"`. Path probe: rooted→exists; has-extension→root/key; else probe `[.png,.jpg,.jpeg,.bmp,.gif,.webp]`. Letterbox into a square `iconSize` Avalonia `Bitmap`, no upscale, centered. Converter (`IMultiValueConverter` taking key + size, or a converter parameter) returns `Bitmap?`. Configure via `IconCache.Configure(enabled, rootPath)`.
- [ ] **Step 2: Build + Commit**

### Task 4.4: EntityBrowserView

**Files:** Create `Controls/EntityBrowserView.axaml(.cs)`

- [ ] **Step 1:** XAML: a `Grid` with a `GridSplitter` — left = `DataGrid` (read-only, `SelectionMode=Extended`, `CanUserSortColumns=False` (custom sort via header tap), bound `ItemsSource=Rows`, `SelectedItem=SelectedRow`); right pane = `StackPanel` with the search box, the three search-mode `RadioButton`s (visibility/labels bound to config props), the realtime `CheckBox`, `Search`/`Load All` buttons, the status `TextBlock`, and an `Actions` `ContentControl` (a styled property exposing the per-tab action panel). Columns are built in code-behind from `Columns` (text columns + `DataGridTemplateColumn` for image columns using the icon converter). Default split ~0.72 with min sizes 320/360; "user moved" flag stops auto-ratio.
  - Add a styled `public static readonly StyledProperty<object?> ActionsProperty;` so a tab View can do `<controls:EntityBrowserView DataContext="{Binding Browser}"><controls:EntityBrowserView.Actions>...</controls:EntityBrowserView.Actions></controls:EntityBrowserView>`.
  - Double-tap a (non-image) cell → copy cell text to clipboard.
- [ ] **Step 2: Build + Commit** — `git commit -am "feat(desktop): EntityBrowserView + icon cache"`

---

## PHASE 5 — Shell

### Task 5.1: ShellViewModel + MainWindow

**Files:** Create `Shell/ShellViewModel.cs`, `Shell/MainWindow.axaml(.cs)`

- [ ] **Step 1: ShellViewModel** — pulls `IEnumerable<ITabModule>`, exposes `IReadOnlyList<ITabModule> Tabs` (ordered by `Order`), `SelectedTab`, the `SidebarViewModel`, and `OpenSettings`/`OpenAbout` `ReactiveCommand`s. On construction it `await`s `IAppSettingsService.LoadAsync()`, applies env overrides (provider/connection string parse — port `ApplyEnvironmentDefaults`), seeds `IAppSettingsHolder`, `IPlayerContext`, and `IconCache.Configure(...)`.

```csharp
using System.Collections.Generic; using System.Linq;
using App.Desktop.Modules; using ReactiveUI;
namespace App.Desktop.Shell;
public sealed class ShellViewModel : ReactiveObject
{
    public IReadOnlyList<ITabModule> Tabs { get; }
    private ITabModule? _selectedTab;
    public ITabModule? SelectedTab { get => _selectedTab; set => this.RaiseAndSetIfChanged(ref _selectedTab, value); }
    public SidebarViewModel Sidebar { get; }
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> OpenSettings { get; }
    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> OpenAbout { get; }
    public ShellViewModel(IEnumerable<ITabModule> modules, SidebarViewModel sidebar, /* settings init deps */ ...)
    { Tabs = modules.OrderBy(m => m.Order).ToList(); Sidebar = sidebar; SelectedTab = Tabs.FirstOrDefault(); /* commands... */ }
}
```

- [ ] **Step 2: MainWindow.axaml** — `Grid` `ColumnDefinitions="*,310"`: col 0 = `TabControl ItemsSource={Binding Tabs} SelectedItem={Binding SelectedTab}` with an `ItemTemplate` showing `IconKey` (FontAwesome `<i:Icon>`) + `Title`, and `ContentTemplate` rendering the module VM (ViewLocator resolves each tab's View); col 1 = `SidebarView`. Window: `Title="GM Tool"`, dark background, `Icon="/Assets/Logo.ico"`, `Width=1725 Height=860 MinWidth=1280 MinHeight=720`, `WindowStartupLocation=CenterScreen`.
- [ ] **Step 3: MainWindow.axaml.cs** — `DataContext = Program.Services.GetRequiredService<ShellViewModel>()`; handle `Closing` to flush settings save (await-safe, no `Task.Run().Wait`).
- [ ] **Step 4: Build + run** → app opens with empty tab list (no modules yet) + sidebar.
- [ ] **Step 5: Commit** — `git commit -am "feat(desktop): shell window + view model"`

### Task 5.2: SidebarView (player mgmt + append toggle + command history)

**Files:** Create `Shell/SidebarViewModel.cs`, `Shell/SidebarView.axaml(.cs)`

- [ ] **Step 1: SidebarViewModel** — wraps `IPlayerContext` (Players, SelectedPlayer, `NewPlayerName`, `AddPlayer`/`RemovePlayer` commands), the append toggle (`AppendRun` bool two-way bound to `settings.AppendGeneratedCommands` via the holder + save), and `ICommandHistoryService` (an `ObservableCollection<string> History` kept in sync via `CommandsChanged`, with `SelectedCommand`, `CopySelected`/`CopyAll`/`ClearHistory` commands).
- [ ] **Step 2: SidebarView.axaml** — vertical `DockPanel`/`Grid`, sidebar background `#1E222A`, padding 8: top-right Settings(gear)/About(info) icon buttons; logo `Image` (`/Assets/Heaven_logo1.png`); "Player" label + non-editable `ComboBox` (Players/SelectedPlayer); "New:" `TextBox` + ＋/－ buttons; "Append /run to commands" `CheckBox`; then a **Command history** group filling remaining space: `ListBox` (History/SelectedCommand) + Copy selected / Copy all / Clear buttons.
- [ ] **Step 3: Build + run** → can add/remove/select players; append toggle persists; history panel present (empty).
- [ ] **Step 4: Commit** — `git commit -am "feat(desktop): sidebar with player mgmt + live command history"`

---

## PHASE 6 — Items tab (exemplar; establishes the per-tab pattern)

### Task 6.1: ItemsTabViewModel

**Files:** Create `Features/Items/ItemsTabViewModel.cs`

This is the reference pattern every other tab follows: a `TabModuleViewModel` holding an `EntityBrowserViewModel<TRecord>` (`Browser`), input properties, and `ReactiveCommand`s that build a Lua string via `LuaCommands` and call `ICommandDispatcher.DispatchAsync`. Player target via `IPlayerContext`; guards via `IDialogService`.

- [ ] **Step 1: Implement**

```csharp
using App.Core.Abstractions; using App.Core.Commands; using App.Core.Models.Entities;
using App.Desktop.Infrastructure; using App.Desktop.Modules; using App.Desktop.Services; using App.Desktop.ViewModels;
using ReactiveUI; using System.Reactive; using System.Reactive.Linq;

namespace App.Desktop.Features.Items;

public sealed class ItemsTabViewModel : TabModuleViewModel
{
    public override string Title => "Items";
    public override string IconKey => "fa-solid fa-box";
    public override int Order => 30;

    public EntityBrowserViewModel<ItemRecord> Browser { get; }

    // Insert inputs (defaults per parity inventory)
    private int _itemId = 1; public int ItemId { get => _itemId; set => this.RaiseAndSetIfChanged(ref _itemId, value); }
    private string _itemName = ""; public string ItemName { get => _itemName; set => this.RaiseAndSetIfChanged(ref _itemName, value); }
    private int _amount = 1;  public int Amount { get => _amount; set => this.RaiseAndSetIfChanged(ref _amount, value); }     // 1..99999
    private int _enhance = 0; public int Enhance { get => _enhance; set => this.RaiseAndSetIfChanged(ref _enhance, value); }   // 0..999
    private int _level = 1;   public int Level { get => _level; set => this.RaiseAndSetIfChanged(ref _level, value); }         // 1..999
    private bool _useStatusFlag; public bool UseStatusFlag { get => _useStatusFlag; set => this.RaiseAndSetIfChanged(ref _useStatusFlag, value); }
    private int _statusFlagValue; public int StatusFlagValue { get => _statusFlagValue; set => this.RaiseAndSetIfChanged(ref _statusFlagValue, value); } // 0..999999
    public int StatusFlag => UseStatusFlag ? StatusFlagValue : 0;

    // Modify inputs
    public string[] WearSlots { get; } = ItemsWearSlots.All; // 38 entries verbatim (see Step 2)
    private string _wearSlot = ItemsWearSlots.Default;        // "0 - WEAR_WEAPON"
    public string WearSlot { get => _wearSlot; set => this.RaiseAndSetIfChanged(ref _wearSlot, value); }
    public int WearSlotIndex => ItemsWearSlots.ParseIndex(WearSlot);
    private bool _applyToOther; public bool ApplyToOther { get => _applyToOther; set => this.RaiseAndSetIfChanged(ref _applyToOther, value); }
    private int _modifyLevel = 1;   public int ModifyLevel { get => _modifyLevel; set => this.RaiseAndSetIfChanged(ref _modifyLevel, value); }   // 1..999
    private int _modifyEnhance = 0; public int ModifyEnhance { get => _modifyEnhance; set => this.RaiseAndSetIfChanged(ref _modifyEnhance, value); } // 0..999
    private int _modifyItemCode = 1; public int ModifyItemCode { get => _modifyItemCode; set => this.RaiseAndSetIfChanged(ref _modifyItemCode, value); } // 1..1e9

    public ReactiveCommand<Unit, Unit> AddYourself { get; }
    public ReactiveCommand<Unit, Unit> GiveOtherPlayer { get; }
    public ReactiveCommand<Unit, Unit> EditLevel { get; }
    public ReactiveCommand<Unit, Unit> EditEnhance { get; }
    public ReactiveCommand<Unit, Unit> ChangeAppearance { get; }
    public ReactiveCommand<Unit, Unit> ChangeItemCode { get; }

    private readonly ICommandDispatcher _cmd; private readonly IPlayerContext _player; private readonly IDialogService _dlg;

    public ItemsTabViewModel(IGameDataRepository repo, ILocalCacheService cache, INameNormalizer norm,
        IAppSettingsHolder settings, ICommandDispatcher cmd, IPlayerContext player, IDialogService dlg)
    {
        _cmd = cmd; _player = player; _dlg = dlg;
        bool Icons() => settings.Current.EnableEntityIcons && !string.IsNullOrWhiteSpace(settings.Current.EntityIconsPath) && System.IO.Directory.Exists(settings.Current.EntityIconsPath);

        Browser = new EntityBrowserViewModel<ItemRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache ? cache.LoadAsync<ItemRecord>("items", ct)
                                                               : repo.GetItemsAsync(settings.Current.Provider, ConnStr(settings), Tokens(settings), ct),
            idSelector: x => x.ItemId,
            nameSelector: x => x.NameEn,
            rowValuesSelector: x => Icons() ? new object?[] { x.IconFileName ?? "", x.ItemId, x.NameEn } : new object?[] { x.ItemId, x.NameEn },
            normalizer: norm,
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns = Icons()
                ? new[] { new BrowserColumn("Icon", 44, IsImage: true, ImageSize: 36), new BrowserColumn("ID", 80), new BrowserColumn("Name", 460, Fill: true) }
                : new[] { new BrowserColumn("ID", 80), new BrowserColumn("Name", 460, Fill: true) },
        };

        // auto-populate from selection
        Browser.WhenSelectedRecordChanged.Where(r => r is not null).Subscribe(r =>
        { ItemId = r!.ItemId; ItemName = r.NameEn; ModifyItemCode = r.ItemId; });

        AddYourself = ReactiveCommand.CreateFromTask(async () =>
        {
            if (ItemId <= 0) { await _dlg.ShowWarningAsync("Items", "Select item or enter Item ID first."); return; }
            await _cmd.DispatchAsync(LuaCommands.InsertItemSelf(ItemId, Amount, Enhance, Level, StatusFlag));
        });
        GiveOtherPlayer = ReactiveCommand.CreateFromTask(async () =>
        {
            if (ItemId <= 0) { await _dlg.ShowWarningAsync("Items", "Select item or enter Item ID first."); return; }
            if (!_player.TryResolveRequired(out var p)) { await _dlg.ShowWarningAsync("Items", "Select player in the right sidebar for 'Give other player'."); return; }
            await _cmd.DispatchAsync(LuaCommands.InsertItemPlayer(ItemId, Amount, Enhance, Level, StatusFlag, p));
        });
        EditLevel = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemLevelOwn(WearSlotIndex, ModifyLevel),
            other: p => LuaCommands.SetWearItemLevelPlayer(WearSlotIndex, p, ModifyLevel), requireItemCode: false));
        EditEnhance = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemEnhanceOwn(WearSlotIndex, ModifyEnhance),
            other: p => LuaCommands.SetWearItemEnhancePlayer(WearSlotIndex, p, ModifyEnhance), requireItemCode: false));
        ChangeAppearance = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemAppearanceOwn(WearSlotIndex, ModifyItemCode),
            other: p => LuaCommands.SetWearItemAppearancePlayer(WearSlotIndex, p, ModifyItemCode), requireItemCode: true));
        ChangeItemCode = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.ChangeWearItemCodeOwn(WearSlotIndex, ModifyItemCode),
            other: p => LuaCommands.ChangeWearItemCodePlayer(WearSlotIndex, p, ModifyItemCode), requireItemCode: true));
    }

    private async Task ModifyAsync(Func<string> self, Func<string,string> other, bool requireItemCode)
    {
        if (requireItemCode && ModifyItemCode <= 0) { await _dlg.ShowWarningAsync("Items", "Itemcode must be greater than 0."); return; }
        if (ApplyToOther)
        {
            if (!_player.TryResolveRequired(out var p)) { await _dlg.ShowWarningAsync("Items", "Select player in the right sidebar for 'Other' mode."); return; }
            await _cmd.DispatchAsync(other(p)); return;
        }
        await _cmd.DispatchAsync(self());
    }

    // Helpers shared by all tabs (factor into a base/util in Task 6.3):
    private static string ConnStr(IAppSettingsHolder s) => /* build/return as in old GetConfiguredConnectionString */ ...;
    private static IReadOnlyDictionary<string,string> Tokens(IAppSettingsHolder s) => s.Current.TableNames.ToTokenMap();
}
```

- [ ] **Step 2:** Create `Features/Items/ItemsWearSlots.cs` with the **38** verbatim entries (the `WearSlotItems` list from `ItemsActionsControl.cs`), `Default = "0 - WEAR_WEAPON"`, and `ParseIndex(string)` = int before first space (fallback 0).
- [ ] **Step 3: Build** (ItemsTabView in next task) — defer build to 6.2.
- [ ] **Step 4: Commit** after 6.2.

### Task 6.2: ItemsTabView + connection-string helper

**Files:** Create `Features/Items/ItemsTabView.axaml(.cs)`; create `Services/ConnectionStringResolver.cs` (extract the old `GetConfiguredConnectionString` precedence into a shared injectable used by all tabs and Playerchecker).

- [ ] **Step 1: ConnectionStringResolver** — port `GetConfiguredConnectionString`: if `Connection.Server` & `Connection.Database` set → `Build(provider, Connection)` (and update `Current.ConnectionString`); else if `ConnectionString` non-empty → use it; else throw `InvalidOperationException("Connection string is empty. ...")`. Inject `IConnectionStringBuilderService` + `IAppSettingsHolder`. Replace the per-tab `ConnStr(...)` helper with this service.

- [ ] **Step 2: ItemsTabView.axaml** — `controls:EntityBrowserView DataContext="{Binding Browser}"` with `EntityBrowserView.Actions` = the action panel mirroring the WinForms layout:
  - "Give / Insert" group: ID (`NumericUpDown` Min 1 Max 1e9) + Name (read-only `TextBox`), Amount (1..99999), Enhance (0..999), Level (1..999), `CheckBox` "Statusflag" + `NumericUpDown` (0..999999, `IsEnabled={Binding UseStatusFlag}`), buttons "Add yourself"/"Give other player".
  - "Modify Equipped Item" group: "Targeted Wear-Slot" `ComboBox` (WearSlots/WearSlot), Own/Other `RadioButton`s (`ApplyToOther`), ModifyLevel (1..999), ModifyEnhance (0..999), Itemcode (1..1e9), buttons "Edit level"/"Edit Enhance"/"Change appearance"/"Change itemcode".
  - Two disabled placeholder sub-tabs "Random Option" / "Itemuseflag" with "coming soon" text (use a nested `TabControl` to mirror the original, or simple disabled expanders).
- [ ] **Step 3: Build + run** → Items tab appears, loads (Load All), search/sort/icons work, all 6 buttons copy the exact Lua and append to history.
- [ ] **Step 4: Commit** — `git commit -am "feat(desktop): Items tab (exemplar) + connection resolver"`

### Task 6.3 (optional cleanup): factor a `CommandActionViewModel` base

- [ ] If repetition is high, extract a small base holding `ICommandDispatcher`/`IPlayerContext`/`IDialogService` + helpers (`GuardItemAsync`, `ResolveRequiredPlayerAsync`). Keep it minimal. Commit.

---

## PHASE 7 — Command tabs (Monster, Skills, Buffs, NPC, Summons)

Each task = one module (VM + View) following the Items exemplar. Only the **unique** inputs/commands are specified; reuse the Items VM/View structure, the `EntityBrowserViewModel<T>` wiring (local-cache branch + `maxRows` + icons where noted), and the dispatcher/player/dialog plumbing.

### Task 7.1: Monster (`Features/Monster`, Order 20, icon "fa-solid fa-dragon")

- [ ] Browser: `MonsterRecord`, cache key `"monsters"`, **icons OFF**, columns ID(80)/Name(340,fill)/Level(90)/Location(300,fill), searchable `[Name, Location]`. Auto-populate: selection → `MonsterId`.
- [ ] Inputs: `SpawnMode` ComboBox (DropDownList) verbatim `["At your place","At selected player place","At specific coordinates"]` default index 0; `MonsterId` (1..1e9, default 1); `Amount` (1..10000, default 1); `X`,`Y` (−100000..100000, default 0); `Layer` (−1000..1000, default 1); `UseLifetime` CheckBox; `MinutesLifetime` (−1..100000, default −1). Enable rules (port `ToggleInputsByMode`): X/Y/Layer enabled only in coords mode; `UseLifetime` enabled in coords OR player mode (else forced off + minutes −1); `MinutesLifetime` enabled when lifetime usable AND checked; checking lifetime with minutes&lt;1 sets 1. Effective `minutesLifetime = UseLifetime ? Max(1, MinutesLifetime) : -1`.
- [ ] Single "Create Command" `ReactiveCommand`:
```csharp
// guard: MonsterId<=0 (and no selection) -> "Select a monster or enter Monster ID first." (title "Monster")
var ml = UseLifetime ? Math.Max(1, MinutesLifetime) : -1;
switch (SpawnMode) {
  case "At your place": await _cmd.DispatchAsync(LuaCommands.MonsterRegenerate(MonsterId, Amount)); break;
  case "At selected player place":
     if (!_player.TryResolveRequired(out var p)) { warn "Select player in the right sidebar for 'At selected player place'."; return; }
     await _cmd.DispatchAsync(LuaCommands.MonsterAddNpcAtPlayer(p, MonsterId, Amount, ml)); break;
  default: await _cmd.DispatchAsync(LuaCommands.MonsterAddNpcAtCoords(X, Y, MonsterId, Amount, ml, Layer)); break;
}
```
- [ ] Build + run + commit.

### Task 7.2: Skills (`Features/Skills`, Order 40, icon "fa-solid fa-wand-sparkles")

- [ ] Browser: `SkillRecord`, cache `"skills"`, **icons ON when enabled** (Icon 44/36 at index 0), columns ID(80)/Name(460,fill). Selection → `SkillId`.
- [ ] Inputs: `SkillId` (1..1e9, default 1), `SkillLevel` (1..999, default 1), `CreatureSlotIndex` (0..10, default 0).
- [ ] Commands (self vs other via `IPlayerContext.Resolve()=="self"`; `ResolveSkillId` guard "Select skill or enter Skill ID first." / title "Skills"):
  - Learn skill: self `LearnSkill(id)` / other `LearnSkillForPlayer(id,p)`.
  - Set skill level: self `SetSkill(id,SkillLevel)` / other `SetSkillForPlayer(id,SkillLevel,p)`.
  - Remove skill: self `RemoveSkill(id)` / other `RemoveSkillForPlayer(id,p)`.
  - Learn all skill: **other only** — if self → warn "Select player in the right sidebar for Learn all skill."; else `LearnAllSkill(p)`.
  - Learn creature skill: self `LearnCreatureSkillSelf(id)` / other `LearnCreatureSkill(id,p)` (ignores slot — hardcoded handle 0).
  - Learn creature all skill: self `LearnCreatureAllSkill(CreatureSlotIndex)` / other `LearnCreatureAllSkillForPlayer(CreatureSlotIndex,p)` (no skillId, no guard).
- [ ] Build + run + commit.

### Task 7.3: Buffs (`Features/Buffs`, Order 50, icon "fa-solid fa-bolt")

- [ ] Browser: `StateRecord`, cache `"states"`, **icons ON when enabled** (Icon 28/20 at index 0), columns State ID(100)/Buff name(460,fill). Selection → `StateId` + `BuffName` (display-only).
- [ ] Inputs: `StateId` (1..1e9, default 1), `BuffName` (read-only text), `BuffLevel` (1..999, default 1), `DurationMinutes` (1..100000, default 1), `IsSummonTarget` (Player/Summon radio; default Player). `ResolveStateId` guard "Select buff/state or enter Buff-ID first." / title "Buffs".
- [ ] Commands:
  - Add Timed World State: `CastWorldState(StateId, BuffLevel, DurationMinutes)`.
  - Add Event State: `AddEventState(StateId, BuffLevel)`.
  - Remove Event State: `RemoveEventState(StateId)`.
  - Add Buff: require player (else "Select player in the right sidebar."); `IsSummonTarget ? AddCreatureState(StateId,BuffLevel,DurationMinutes,p) : AddPlayerState(...)`.
  - Remove Buff: require player; `IsSummonTarget ? RemoveCreatureState(StateId,p) : RemovePlayerState(StateId,p)`.
- [ ] Build + run + commit.

### Task 7.4: NPC (`Features/Npc`, Order 60, icon "fa-solid fa-person")

- [ ] Browser: `NpcRecord`, cache `"npcs"`, **icons OFF**, columns NPC ID(90)/Name(420,fill)/X(90)/Y(90)/Contact script(280,fill). **Secondary search enabled**, label "for Contact script", `secondarySearchTextSelector = ContactScript`. `rowValuesSelector = [NpcId, NpcTitle, X?("0.###")??"", Y?("0.###")??"", ContactScript??""]`, `searchableTextSelector = [NpcTitle]`. Selection → NpcId, NpcName, ContactScript, NpcX=round(X), NpcY=round(Y).
- [ ] Inputs: `NpcId` (0..2e9, default 0; 0 = sentinel "no manual id"), `NpcName`/`ContactScript` (read-only), `NpcX`/`NpcY` (−1000000..1000000, default 0), `Layer` (−1000..1000, default 0), `HideNpc` CheckBox → `VisibleFlag = HideNpc ? 1 : 0`. `ResolveNpcId` guard "Select npc or enter NPC ID first." / title "NPC".
- [ ] Commands:
  - Add NPC to world: self `AddNpcToWorld(NpcX,NpcY,Layer,npcId)` / other `AddNpcToWorldForPlayer(NpcX,NpcY,Layer,p,npcId)`.
  - Show/Hide NPC: `ShowNpc(NpcX,NpcY,npcId,Layer,VisibleFlag)` (no player).
  - Warp to NPC: require player (else "Select player in the right sidebar for warp to NPC."); `WarpToNpcCoordinates(NpcX,NpcY,p)` (**double-quote escaping** — already handled in LuaCommands).
- [ ] Build + run + commit.

### Task 7.5: Summons (`Features/Summons`, Order 70, icon "fa-solid fa-paw")

- [ ] Browser: `SummonRecord`, cache `"summons"`, **icons ON when enabled** (Icon 44/36 at index 0), columns Summon ID(100)/Summon Name(320,fill)/Card Name(320,fill), searchable `[SummonName, CardName]`. Selection → `SummonId`.
- [ ] Inputs: `SummonId` (0..2e9, default 0), `UseAddStage` CheckBox (enables `AddStage`), `AddStage` (0..10, default 0), `Slot` ComboBox values `0..5` (default 0), `Stage` (0..10, default 0). `ResolveSummonId` guard "Select summon or enter Summon ID first." / title "Summons".
- [ ] Commands:
  - Add Summon: `UseAddStage ? InsertSummonByIdWithStage(SummonId, AddStage) : InsertSummonById(SummonId)` (guard first).
  - Stage Summon: `StageSummon(Slot, Stage)` (no guard).
- [ ] Build + run + commit.

---

## PHASE 8 — Warp tab (settings-backed list)

### Task 8.1: Warp (`Features/Warp`, Order 80, icon "fa-solid fa-location-dot")

**Files:** `Features/Warp/WarpTabViewModel.cs`, `WarpTabView.axaml(.cs)`

- [ ] **Step 1:** The Warp browser is **not** DB/cache-backed — it lists `IAppSettingsHolder.Current.WarpLocations`. Use the EntityBrowser with an in-memory loader returning the warp list (rows `[X, Y, Name]`, `Tag = WarpLocationSettings`). Config: **icons OFF**, columns X(120)/Y(120)/Name(420,fill); `ConfigureSearchLabels("", "by Name")`, **ID search disabled**, status "Ready."; filter by `Name.Contains(term, OrdinalIgnoreCase)` (blank → all). Load All → reload full list. Seed defaults: if `WarpLocations` empty, populate from the **31** hardcoded defaults — copy the list verbatim from `src/App.WinForms/MainForm.cs:1202-1238` (`GetDefaultWarpLocations`) into a `WarpDefaults.cs`.
- [ ] **Step 2:** Selection → `SelectedX`/`SelectedY` (display) + prime `AddX`/`AddY`; track `_selectedWarp`.
- [ ] **Step 3:** Inputs: `SelectedX`/`SelectedY` (display, −1000000..1000000), `AddX`/`AddY` (−1000000..1000000, default 0), `LocationName` text. Commands:
  - Warp: guard `_selectedWarp == null` → "Select warp first." (title "Warp"); require player (else "Select player in the right sidebar."); `WarpToLocationForPlayer(_selectedWarp.X, _selectedWarp.Y, p)`.
  - Warp to you: require player; `WarpPlayerToYou(p)`.
  - Warp to someone: require player; `WarpYouToPlayer(p)`.
  - OpenWorldmap: disabled no-op button (parity).
  - Add: guard blank name → "Location name is required."; append `WarpLocationSettings{X=AddX,Y=AddY,Name}`; refresh; clear name only; save settings.
  - Remove selected: guard null → "Select warp first."; remove by ref then by value match; if none → "Selected warp was not found."; refresh; save settings.
- [ ] **Step 4: Build + run + commit.**

---

## PHASE 9 — Playerchecker tab (SQL search + inventory/warehouse)

### Task 9.1: Playerchecker (`Features/Playerchecker`, Order 10, icon "fa-solid fa-users")

**Files:** `Features/Playerchecker/PlayercheckerTabViewModel.cs`, `PlayercheckerTabView.axaml(.cs)`

- [ ] **Step 1: Browser** — `PlayerRecord`, **no local cache** (always live DB). Columns ID(80)/Name(230,fill)/Account(200,fill)/Level(80)/Job(160,fill)/Status(90). `ConfigureSearchLabels("by Account","by Char Name")`; **SQL search path** via `sqlSearchAsync = (term, mode, ct) => repo.GetCharactersBySearchAsync(provider, conn, term, searchByAccount: mode==SearchMode.ById, tokens, ct)`; `loadAllAsync = GetPlayersAsync` (but Load All button hidden); **realtime hidden + off**; debounce 350; default mode ByName ("by Char Name"). Selection → `_selectedPlayer` (whole record).
- [ ] **Step 2: Action buttons** (no Lua): "Load All Characters" (`GetAllCharactersAsync` via `Browser.LoadExternalAsync`), "Load Online Characters" (`GetOnlineCharactersAsync`), "Load inventory" (guard no selection → "Select a player first." title "Load Inventory"; `GetInventoryAsync(playerId)`), "Load WH" (guard; `GetWarehouseAsync(account)` — **keyed by Account**), "Open infos" (guard; info dialog body `Name/Account/Level/Job/Status`).
- [ ] **Step 3: Inventory grid** — a second `DataGrid` below the browser (vertical `GridSplitter`), bound to an `ObservableCollection<InventoryItemRecord>`; columns Item ID(90)/Name(300,fill)/Count(70)/Level(70)/Enhance(75)/Wear(60); header sort; double-tap cell → copy. Show a **visible title** above it ("Inventory — {name} ({n} item(s))" / "Warehouse — {account} ({n} item(s))").
- [ ] **Step 4: Build + run + commit.**

---

## PHASE 10 — Settings, About, finalize

### Task 10.1: SettingsWindow + SettingsViewModel

**Files:** `Features/Settings/SettingsViewModel.cs`, `SettingsWindow.axaml(.cs)`

- [ ] **Step 1: VM** binds a working `AppSettings.Clone()`. Three tabs:
  - **Connection:** Provider ComboBox (enum), Server, Port (1..65535), Database, UserId, Password (masked), Integrated security / Encrypt / Trust cert (MSSQL-only enable), Arcadia/Telecaster/Auth name. Provider change auto-swaps default port (1433↔3306) only when matching the other provider's default; `UpdateAuthUi` enable rules. On load: if `Connection.Server` blank but `ConnectionString` set → `TryParse` to rehydrate; port fallback to provider default when ≤0.
  - **Table Names:** Accounts + the 9 resource tokens (Character/Monster/String/Item/Skill/State/Npc/Summon).
  - **General:** Limit Select (1000), Use local cache, Enable entity icons + Icons folder (Browse… via `IStorageProvider.OpenFolderPickerAsync`) + Export to local cache + cache status label.
- [ ] **Step 2: Commands** — Test (validate → `repo.TestConnectionAsync`), Save (validate → write `.env` provider+connstring → return settings), Cancel, Export cache (validate → sequential `GetMonsters/Items/Skills/States/Npcs/Summons` → `cache.SaveAsync(key,...)` with per-entity status), Browse, RefreshCacheStatus (oldest of 6 `GetCacheDate`). Validation order: Server required → Database required → MSSQL non-integrated UserId required → MySQL UserId required → icons path required/exists when enabled. Status line shared across actions (use a `Status` property + dialogs via `IDialogService`).
- [ ] **Step 3: Wire** — Shell `OpenSettings` opens `SettingsWindow` modal; on OK: `holder.Set(updated.Clone())`, re-seed `PlayerContext`/`IconCache`, reconfigure browsers (raise a shell event browsers subscribe to, or recreate affected tab browsers), `await settingsService.SaveAsync`. `.env` written to `AppContext.BaseDirectory/.env` (port `TrySaveToEnv`).
- [ ] **Step 4: Build + run + commit.**

### Task 10.2: AboutWindow

**Files:** `Features/About/AboutWindow.axaml(.cs)`

- [ ] Small modal: logo (`/Assets/Heaven_logo1.png`), product name "GM Tool", version (`Assembly` informational version), OK button. Shell `OpenAbout` opens it. Build + commit.

### Task 10.3: Settings persistence lifecycle + env override on startup

- [ ] In `ShellViewModel` init: `LoadAsync` → `EnsureDefaults` (seed WarpLocations if empty) → apply env (`YSM_DB_PROVIDER`/`YSM_DB_CONNECTION_STRING` → provider/connection via `TryParse`) → `holder.Set` → seed `PlayerContext` + `IconCache`. In `MainWindow.Closing`: persist settings (set SelectedPlayer + AppendGeneratedCommands from current state) and `await SaveAsync` (cancel close, save, then close — no sync-over-async). Build + commit.

### Task 10.4: Delete App.WinForms; finalize publish

- [ ] **Step 1:** Remove the old project + artifacts:
```bash
dotnet sln YSM-GMTool.slnx remove src/App.WinForms/App.WinForms.csproj
git rm -r src/App.WinForms
```
- [ ] **Step 2:** Update `README.md` run/build/publish commands to target `src/App.Desktop`. Replace the WinForms auto-publish MSBuild hack with a clean publish profile/script:
```bash
dotnet publish src/App.Desktop/App.Desktop.csproj -c Release -r win-x64 --self-contained false -o publish
```
- [ ] **Step 3: Full build + run** → `dotnet build YSM-GMTool.slnx -c Release` succeeds; app runs; smoke-test each tab generates the expected Lua (compare a sample of clipboard outputs against the parity appendix) and history records them.
- [ ] **Step 4: Commit** — `git commit -am "chore: remove WinForms project, finalize Avalonia rewrite"`

---

## Data appendix — entity column → property contract (Dapper `MatchNamesWithUnderscores=true`)

| Record | Property → SQL alias |
|---|---|
| PlayerRecord | PlayerId←Player_ID, PlayerName←Player_Name, Account←Account, Level←Level, JobName←Job_Name, OnlineStatus←Online_Status |
| InventoryItemRecord | ItemId←Item_ID, ItemName←Item_Name, Count←Count, Level←Level, Enhance←Enhance, WearInfo←wear_info, AwakenSid←awaken_sid, RandomOptionSid←random_option_sid, SummonCode←summon_code |
| ItemRecord | ItemId←Item_ID, NameEn←Name_EN, IconFileName←icon_file_name |
| SkillRecord | SkillId←Skill_id, Skillname←Skillname, IconFileName←icon_file_name |
| StateRecord | StateId←state_id, BuffName←buff_name, IconFileName←icon_file_name |
| MonsterRecord | Id←id, Name←name, Level←level, Location←location |
| NpcRecord | NpcId←NPC_id, NpcTitle←npc_title, X←x, Y←y, ContactScript←contact_script |
| SummonRecord | SummonId←Summon_ID, SummonName←Summon_Name, CardName←Card_Name, IconFileName←icon_file_name |

Repository method → QueryEntity → params: GetPlayers→Playerchecker; GetCharactersBySearch→PlayercheckerByAccount|ByCharName (@SearchTerm=`%term%`); GetAllCharacters→PlayercheckerAll; GetOnlineCharacters→PlayercheckerOnline; GetInventory→PlayerInventory (@OwnerId); GetWarehouse→PlayerWarehouse (@AccountName); GetMonsters/Items/Skills/States/Npcs(NPC)/Summons→respective. `queries.json` kept verbatim.

---

## Self-review notes

- **Spec coverage:** every tab (8), the shared browser, command pipeline (+ history fix), Core models/services, data layer, settings dialog (3 tabs + cache export + .env), about, theme, bootstrap, and the `ITabModule` extensibility model each map to a task above.
- **Command parity:** every Lua template from `lua_commands.json` that has UI is reproduced in `LuaCommands` (Task 1.3); orphans dropped by decision. Self/other variants, double-quote escaping (warpToNpc), and the literal `*100*60` duration are preserved.
- **Type consistency:** shared service names are stable across tasks (`ICommandDispatcher.DispatchAsync`, `IPlayerContext.TryResolveRequired/Resolve`, `IDialogService.ShowWarningAsync`, `EntityBrowserViewModel<T>`, `IconCache.Configure`, `ConnectionStringResolver`).
- **Extensibility goal:** adding a tab = add one `*TabViewModel : TabModuleViewModel` + `*TabView.axaml`; it is auto-registered by `AddTabModules` reflection scan and ordered by `Order`. Adding a button = a `ReactiveCommand` + a `LuaCommands` method + a `<Button>`. No shell edits.
