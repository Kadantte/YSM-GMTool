# Offline Snapshot Mode, Embedded Icons, and UI Layout Rewrite

**Date:** 2026-05-10
**Status:** Approved (design phase)
**Scope:** Three coordinated changes shipped as one spec.

## Goals

1. Allow distributing the GM Tool to users who must NOT have a live connection to the production database. They get a pre-baked snapshot file and work entirely offline.
2. Make the UI render in its final form on first paint — no runtime measurement, no resize flicker after launch.
3. Eliminate the requirement for users to maintain a separate icons directory; in offline mode icons travel with the app as a single file.

## Non-Goals

- No support for editing/persisting changes back into the snapshot — snapshots are read-only.
- No mid-session toggle between Live and Offline mode (mode determined once, at startup).
- No snapshot of player/account/inventory/warehouse data — those modules require live DB by design.
- No automatic snapshot refresh / sync mechanism — admin re-runs export manually and redistributes the file.

---

## Architecture Overview

### Mode Detection (one-time, at startup)

`Program.cs` decides the application mode by checking for the snapshot file next to the executable:

```
snapshotPath = Path.Combine(AppContext.BaseDirectory, "gmtool-snapshot.db")
mode = File.Exists(snapshotPath) ? AppMode.OfflineSnapshot : AppMode.LiveDb
```

The chosen `AppMode` is registered in DI and injected into `MainForm` and `SettingsForm`. There is no UI toggle.

### File Layout (next to `GM Tool.exe`)

| File | Purpose | Required in mode |
|------|---------|------------------|
| `gmtool-snapshot.db` | SQLite snapshot of resource entities | Offline (presence triggers offline mode) |
| `gmtool-icons.db` | SQLite blob store of icon PNGs | Offline (optional — falls back to no-icons if missing) |

In Live mode, neither file is read; the app uses the configured live DB connection and `EntityIconsPath` directory as today.

### Mode-Specific UI Behavior

| Element | Live mode | Offline mode |
|---------|-----------|--------------|
| Window title | `GM Tool` | `GM Tool (Offline Snapshot)` |
| PlayerChecker tab | visible | hidden |
| Items / Monster / Skills / Buffs / NPCs / Summons / Warps tabs | visible | visible |
| Settings → Connection tab | visible | hidden |
| Settings → Table Names tab | visible | hidden |
| Settings → General tab | visible | visible (but `EntityIconsPath` field hidden) |
| Settings → Export Snapshot button | visible (new) | hidden |

---

## Component 1 — Snapshot Database

### Engine

SQLite via `Microsoft.Data.Sqlite`. Single file. Read-only at runtime (`PRAGMA query_only = ON`).

### Schema

Denormalized, one table per supported entity, columns 1:1 with the corresponding `*Record` C# model. String resource lookups (`StringResource` joins) are pre-resolved during export so runtime queries never join.

```sql
CREATE TABLE Items    (item_id INTEGER PRIMARY KEY, name_en TEXT NOT NULL, icon_file_name TEXT);
CREATE TABLE Monsters (id INTEGER PRIMARY KEY, name TEXT NOT NULL, level INTEGER, location TEXT);
CREATE TABLE Npcs     (npc_id INTEGER PRIMARY KEY, npc_title TEXT NOT NULL, x REAL, y REAL, contact_script TEXT);
CREATE TABLE Skills   (skill_id INTEGER PRIMARY KEY, skillname TEXT NOT NULL, icon_file_name TEXT);
CREATE TABLE States   (state_id INTEGER PRIMARY KEY, buff_name TEXT NOT NULL, icon_file_name TEXT);
CREATE TABLE Summons  (summon_id INTEGER PRIMARY KEY, summon_name TEXT NOT NULL, card_name TEXT, icon_file_name TEXT);

CREATE TABLE Meta     (key TEXT PRIMARY KEY, value TEXT);
-- Meta keys: schema_version, exported_at (ISO-8601 UTC), source_provider (MSSQL|MySQL), tool_version

CREATE INDEX idx_items_name    ON Items(name_en);
CREATE INDEX idx_monsters_name ON Monsters(name);
CREATE INDEX idx_npcs_title    ON Npcs(npc_title);
CREATE INDEX idx_skills_name   ON Skills(skillname);
CREATE INDEX idx_states_name   ON States(buff_name);
CREATE INDEX idx_summons_name  ON Summons(summon_name);
```

Columns map 1:1 to fields on the corresponding `*Record` types in `App.Core/Models/Entities/` using snake_case (Dapper's `MatchNamesWithUnderscores` already handles binding).

### Schema Versioning

`Meta.schema_version` written by exporter. App refuses to load a snapshot whose `schema_version` does not match the version it was built with, with a clear error message: "Snapshot was created by an incompatible tool version. Re-run export with the matching tool."

### Export Flow

1. Admin opens **Settings → General** tab and clicks **"Export Database to Snapshot…"** (new button — only visible when live DB is configured).
2. SaveFileDialog (default filename `gmtool-snapshot.db`, default folder = exe folder).
3. Modal progress dialog shows per-entity progress (`Exporting Monsters: 1234 / 5678`).
4. `SnapshotExportService` runs:
   - Creates the SQLite file with the schema above.
   - For each entity, calls the existing live-DB repository method to fetch all rows, then bulk-inserts into the snapshot inside a single transaction.
   - Writes `Meta` rows.
5. On success, offers to also pack icons (next component) using the currently-configured `EntityIconsPath`.

### Runtime Integration

- Add `DatabaseProvider.Sqlite` enum value.
- `DbConnectionFactory.Create` returns `SqliteConnection` for `Sqlite`.
- `queries.json` gets a new top-level provider section `"Sqlite"` with simplified queries (`SELECT id, name, icon_id, ... FROM Items WHERE name LIKE @SearchTerm ESCAPE '\'`). Token substitution (`{{ArcadiaName}}` etc.) does not apply — Sqlite queries reference normalized table names directly.
- `GameDataRepository` and presenters are unchanged for supported entities; they go through the same `QueryAsync<T>` path with `provider=Sqlite` and `connectionString="Data Source=<path>;Mode=ReadOnly"`.
- Methods that do not apply offline (`GetPlayersAsync`, `GetCharactersBySearchAsync`, `GetAllCharactersAsync`, `GetOnlineCharactersAsync`, `GetInventoryAsync`, `GetWarehouseAsync`) throw `NotSupportedException("Operation not available in offline snapshot mode.")` when called against the Sqlite provider. The UI never invokes them in offline mode because PlayerChecker is hidden.

---

## Component 2 — Icons Database (offline only)

### Schema

```sql
CREATE TABLE Icons (file_name TEXT PRIMARY KEY, png BLOB NOT NULL);
CREATE TABLE Meta  (key TEXT PRIMARY KEY, value TEXT);
-- Meta keys: packed_at, source_dir, file_count, tool_version
```

### Pack Flow

1. After snapshot export completes, the app prompts: "Also pack icons from `<EntityIconsPath>` into `gmtool-icons.db`?"
2. `IconPackService` scans the directory recursively for `*.png` files, stores each as a BLOB row keyed by file name (case-insensitive, matching how `IconFileName` is stored in resources).
3. Existing rows are replaced (`INSERT OR REPLACE`).

### Runtime Integration

- A small abstraction `IIconSource` with two implementations:
  - `DirectoryIconSource` (live mode) — reads PNG from `<EntityIconsPath>\<file_name>` (current behavior).
  - `SqliteIconSource` (offline mode) — opens `gmtool-icons.db` once, reads BLOB by `file_name` on demand.
- Both wrapped by the existing `LocalCacheService` (in-memory cache of decoded `Image`).
- DI selects the implementation based on `AppMode`. If offline mode is active but `gmtool-icons.db` is missing, the app uses a null source (icons render as blank — no crash, logged once at startup).
- In live mode the `EntityIconsPath` setting and behavior are completely unchanged.

---

## Component 3 — UI Layout Rewrite

### Problem (current state)

`UiLayoutPolicy` runs a debounced runtime measurement pass that:
- iterates every `GroupBox`/`Panel`/`TableLayoutPanel`/`UserControl`,
- measures preferred content height in 4 iterative passes,
- resizes containers, then resizes the form's `MinimumSize`,
- logs warnings when sections still clip.

This pass fires after the form is shown and on every visibility/resize change. The user sees it as flickering, late layout, and "unprofessional" first paint.

### Solution

Every `*ActionsControl` is rebuilt with a deterministic Designer-defined `TableLayoutPanel` layout. No runtime measurement.

### Standard Layout Conventions (apply to all 8 ActionsControls)

Root container: `TableLayoutPanel` with `Dock = Fill`, fixed `RowCount` and `ColumnCount`.

**Standard row heights:**
- Section title row: `RowStyle = Absolute, 24px`
- Single-line input row: `Absolute, 28px`
- Action button row: `Absolute, 32px`
- Stretch/filler row (max one per section): `Percent, 100%`

**Standard column widths:**
- Label column: `Absolute, 110-150px` (consistent across forms)
- Input/content column: `AutoSize` or `Percent, 100%`
- Right-aligned button column: `Absolute, 110px`

**Buttons:**
- `AutoSize = false`, explicit `Size` (e.g., 100×28) set in Designer
- `Anchor = Top | Left`
- `Margin = 3, 3, 3, 3`
- Never `Dock = Fill`

**GroupBoxes:**
- `AutoSize = false`, explicit `Size` set in Designer based on content
- `Padding = 8, 18, 8, 8` (extra top padding for title)
- Each contains its own inner `TableLayoutPanel`

**MainForm:**
- `MinimumSize` set once in Designer (sized to fit the largest `*ActionsControl`)
- Existing `SplitContainer` (sidebar + workspace) and right-side player target panel — structure unchanged

### Code Removed

- Entire file: `src/App.WinForms/Layout/UiLayoutPolicy.cs`
- From `MainForm.cs`: `ApplyGlobalLayoutPolicySafe`, `_layoutDebounceTimer`, `_layoutPassInProgress`, `ResizeActionControlHeight`, `GetVisibleActionHosts`, the dynamic `MinimumSize` update block
- `UiLayoutPolicy.ApplyFixedButtonSizes` calls in `SettingsForm.cs` and `AboutForm.cs`

### Code Kept

- Dark theme/coloring helpers — unchanged
- DataGridView configuration — unchanged
- Right-side target player panel — unchanged

### Validation

After each control is rebuilt:
1. Open in Designer, confirm `Size` matches actual content + 8px margin.
2. Run the tool manually, switch to that tab, verify no clipping at `MinimumSize` and at expanded sizes.
3. No log warnings about clipped sections (the validator that produced those warnings is being deleted, but the underlying problem must be solved by the new layout, not hidden).

### Estimated Effort

8 controls × ~30 min each = ~4 hours of focused Designer work.

---

## Cross-Cutting Concerns

### Configuration Files

- `queries.json` — add `"Sqlite"` provider section with offline-friendly queries for the 6 supported entities.
- `lua_commands.json` — no changes (Lua command generation works identically in both modes).
- `.env` / `settings.json` — no schema changes required; live-mode settings are simply ignored when offline mode is active.

### Logging

- Mode determination logged at startup: `"App started in {Mode} mode"`.
- Snapshot/icon loading errors logged with file path and recovery action taken.
- Export progress logged at INFO; per-row insert errors at WARNING.

### Distribution

Admin workflow:
1. Build/publish the tool as today (`scripts/publish-release.ps1`).
2. Run the tool against live DB, click **Export Database to Snapshot…**, save `gmtool-snapshot.db` next to the published exe.
3. Pack icons when prompted, producing `gmtool-icons.db` next to the exe.
4. Zip and distribute the entire publish folder. Recipients get a self-contained, read-only offline tool.

### Testing Strategy

- **Snapshot export**: integration test against a small fixture DB; verify row counts and a few sample values match between live and snapshot.
- **Offline mode runtime**: load a fixture snapshot, verify each entity browser tab populates and search works.
- **Icons DB**: pack a fixture folder, then load and verify byte-for-byte equality of a few PNGs.
- **Mode detection**: smoke tests for both modes (file present / absent).
- **UI layout**: manual visual verification across all 8 ActionsControls at MinimumSize and expanded; no flicker on tab switch.

---

## Open Questions / Future Work

- Snapshot delta updates (currently full re-export only) — out of scope for v1.
- Compression of snapshot/icons DB — SQLite's built-in storage is fine for v1; revisit if file size becomes a problem.
- Signed snapshots to verify provenance — out of scope.
