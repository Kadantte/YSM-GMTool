# Offline Snapshot Mode, Embedded Icons, and UI Layout Rewrite — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add an offline snapshot mode (SQLite-based) so the GM Tool can be distributed to users with no live DB access, embed icons in a companion SQLite file, and rewrite all `*ActionsControl` layouts to use deterministic Designer-defined `TableLayoutPanel` structures (eliminating the runtime `UiLayoutPolicy` measurement pass that causes flicker on first paint).

**Architecture:** A single new `DatabaseProvider.Sqlite` lets the existing repository/Dapper pipeline target a snapshot file with the same query interface as MSSQL/MySQL. Mode is decided once at startup based on the presence of `gmtool-snapshot.db` next to the executable. Icons in offline mode come from a sibling `gmtool-icons.db` via a new `IIconSource` abstraction. UI changes are isolated to the WinForms project: Designer-defined `TableLayoutPanel` layouts replace runtime measurement code.

**Tech Stack:** .NET 10, WinForms, Dapper, Microsoft.Data.Sqlite (new), Microsoft.Data.SqlClient (existing), MySqlConnector (existing), Serilog (existing).

**Spec:** [docs/superpowers/specs/2026-05-10-offline-snapshot-and-ui-rewrite-design.md](../specs/2026-05-10-offline-snapshot-and-ui-rewrite-design.md)

---

## File Structure

### New files
- `src/App.Core/Enums/AppMode.cs` — `LiveDb | OfflineSnapshot`
- `src/App.Core/Interfaces/IIconSource.cs`
- `src/App.Core/Interfaces/ISnapshotExportService.cs`
- `src/App.Core/Interfaces/IIconPackService.cs`
- `src/App.Core/Services/DirectoryIconSource.cs`
- `src/App.Data/Infrastructure/SqliteSnapshotInitializer.cs` — runs `CREATE TABLE` script
- `src/App.Data/Infrastructure/SqliteIconSource.cs` — implements `IIconSource` via `gmtool-icons.db`
- `src/App.Data/Services/SnapshotExportService.cs`
- `src/App.Data/Services/IconPackService.cs`
- `src/App.WinForms/Forms/ExportProgressForm.cs` (+ Designer)
- `tests/App.Data.Tests/App.Data.Tests.csproj` — xUnit test project (new)
- `tests/App.Data.Tests/SnapshotExportServiceTests.cs`
- `tests/App.Data.Tests/SqliteIconSourceTests.cs`
- `tests/App.Data.Tests/IconPackServiceTests.cs`

### Modified files
- `src/App.Core/Enums/DatabaseProvider.cs` — add `Sqlite`
- `src/App.Data/App.Data.csproj` — add `Microsoft.Data.Sqlite` package
- `src/App.Data/Infrastructure/DbConnectionFactory.cs` — handle `Sqlite`
- `src/App.WinForms/Config/queries.json` — add `Sqlite` provider section
- `src/App.WinForms/Program.cs` — mode detection, DI wiring
- `src/App.WinForms/MainForm.cs` — accept `AppMode`, hide PlayerChecker offline, remove dynamic layout pass
- `src/App.WinForms/MainForm.Designer.cs` — set fixed `MinimumSize`
- `src/App.WinForms/Forms/SettingsForm.cs` — accept `AppMode`, hide Connection/TableNames tabs offline, add Export Snapshot button
- `src/App.WinForms/Forms/AboutForm.cs` — remove `ApplyFixedButtonSizes` call
- `src/App.WinForms/Controls/*.cs` and `*.Designer.cs` (8 controls) — TableLayoutPanel rewrite
- `src/App.WinForms/Models/AppSettings.cs` — no schema change needed (settings ignored offline)
- `YSM-GMTool.slnx` — register test project

### Deleted files
- `src/App.WinForms/Layout/UiLayoutPolicy.cs`

---

## Task 1: Add SQLite package and Sqlite provider enum

**Files:**
- Modify: `src/App.Data/App.Data.csproj`
- Modify: `src/App.Core/Enums/DatabaseProvider.cs`

- [ ] **Step 1: Add Microsoft.Data.Sqlite package reference**

Edit `src/App.Data/App.Data.csproj`, add inside the existing `<ItemGroup>` with PackageReferences:

```xml
<PackageReference Include="Microsoft.Data.Sqlite" Version="9.0.0" />
```

- [ ] **Step 2: Restore and verify**

Run: `dotnet restore src/App.Data/App.Data.csproj`
Expected: Restore succeeds, no errors.

- [ ] **Step 3: Add Sqlite enum value**

Replace contents of `src/App.Core/Enums/DatabaseProvider.cs`:

```csharp
namespace App.Core.Enums;

public enum DatabaseProvider
{
    MSSQL,
    MySQL,
    Sqlite
}
```

- [ ] **Step 4: Build solution to confirm enum addition compiles**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Build succeeds. Existing `switch` in `DbConnectionFactory.Create` will still compile (default case throws).

- [ ] **Step 5: Commit**

```bash
git add src/App.Data/App.Data.csproj src/App.Core/Enums/DatabaseProvider.cs
git commit -m "feat: add Microsoft.Data.Sqlite package and Sqlite provider enum value"
```

---

## Task 2: Wire Sqlite provider in DbConnectionFactory

**Files:**
- Modify: `src/App.Data/Infrastructure/DbConnectionFactory.cs`

- [ ] **Step 1: Add Sqlite case to factory**

Replace the body of `Create` in `src/App.Data/Infrastructure/DbConnectionFactory.cs`:

```csharp
return provider switch
{
    DatabaseProvider.MSSQL  => new SqlConnection(connectionString),
    DatabaseProvider.MySQL  => new MySqlConnection(connectionString),
    DatabaseProvider.Sqlite => new Microsoft.Data.Sqlite.SqliteConnection(connectionString),
    _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unsupported database provider.")
};
```

- [ ] **Step 2: Build**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Build succeeds.

- [ ] **Step 3: Commit**

```bash
git add src/App.Data/Infrastructure/DbConnectionFactory.cs
git commit -m "feat: wire SqliteConnection in DbConnectionFactory"
```

---

## Task 3: Create test project for App.Data services

**Files:**
- Create: `tests/App.Data.Tests/App.Data.Tests.csproj`
- Create: `tests/App.Data.Tests/UsingDirectives.cs`
- Modify: `YSM-GMTool.slnx`

- [ ] **Step 1: Create test project file**

Create `tests/App.Data.Tests/App.Data.Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\App.Core\App.Core.csproj" />
    <ProjectReference Include="..\..\src\App.Data\App.Data.csproj" />
  </ItemGroup>

</Project>
```

- [ ] **Step 2: Add global using for xUnit**

Create `tests/App.Data.Tests/UsingDirectives.cs`:

```csharp
global using Xunit;
```

- [ ] **Step 3: Add test project to solution**

Run: `dotnet sln YSM-GMTool.slnx add tests/App.Data.Tests/App.Data.Tests.csproj`
Expected: Project added to slnx.

- [ ] **Step 4: Verify test project builds and runs (zero tests yet)**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj`
Expected: Build succeeds, "0 tests run".

- [ ] **Step 5: Commit**

```bash
git add tests/App.Data.Tests/ YSM-GMTool.slnx
git commit -m "test: scaffold App.Data.Tests xUnit project"
```

---

## Task 4: Snapshot schema initializer

**Files:**
- Create: `src/App.Data/Infrastructure/SqliteSnapshotInitializer.cs`

- [ ] **Step 1: Write failing test**

Create `tests/App.Data.Tests/SqliteSnapshotInitializerTests.cs`:

```csharp
using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SqliteSnapshotInitializerTests
{
    [Fact]
    public void Initialize_CreatesAllExpectedTablesAndIndexes()
    {
        var path = Path.GetTempFileName();
        File.Delete(path);
        try
        {
            SqliteSnapshotInitializer.Initialize(path);

            using var conn = new SqliteConnection($"Data Source={path}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type IN ('table','index') ORDER BY name";
            using var reader = cmd.ExecuteReader();
            var names = new List<string>();
            while (reader.Read()) names.Add(reader.GetString(0));

            Assert.Contains("Items", names);
            Assert.Contains("Monsters", names);
            Assert.Contains("Npcs", names);
            Assert.Contains("Skills", names);
            Assert.Contains("States", names);
            Assert.Contains("Summons", names);
            Assert.Contains("Meta", names);
            Assert.Contains("idx_items_name", names);
            Assert.Contains("idx_monsters_name", names);
            Assert.Contains("idx_npcs_title", names);
            Assert.Contains("idx_skills_name", names);
            Assert.Contains("idx_states_name", names);
            Assert.Contains("idx_summons_name", names);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
```

- [ ] **Step 2: Run test, verify it fails**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter SqliteSnapshotInitializerTests`
Expected: FAIL — `SqliteSnapshotInitializer` does not exist.

- [ ] **Step 3: Implement initializer**

Create `src/App.Data/Infrastructure/SqliteSnapshotInitializer.cs`:

```csharp
using Microsoft.Data.Sqlite;

namespace App.Data.Infrastructure;

public static class SqliteSnapshotInitializer
{
    public const int SchemaVersion = 1;

    private const string CreateScript = """
        CREATE TABLE IF NOT EXISTS Items
            (item_id INTEGER PRIMARY KEY, name_en TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Monsters
            (id INTEGER PRIMARY KEY, name TEXT NOT NULL, level INTEGER, location TEXT);
        CREATE TABLE IF NOT EXISTS Npcs
            (npc_id INTEGER PRIMARY KEY, npc_title TEXT NOT NULL, x REAL, y REAL, contact_script TEXT);
        CREATE TABLE IF NOT EXISTS Skills
            (skill_id INTEGER PRIMARY KEY, skillname TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS States
            (state_id INTEGER PRIMARY KEY, buff_name TEXT NOT NULL, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Summons
            (summon_id INTEGER PRIMARY KEY, summon_name TEXT NOT NULL, card_name TEXT, icon_file_name TEXT);
        CREATE TABLE IF NOT EXISTS Meta
            (key TEXT PRIMARY KEY, value TEXT);

        CREATE INDEX IF NOT EXISTS idx_items_name    ON Items(name_en);
        CREATE INDEX IF NOT EXISTS idx_monsters_name ON Monsters(name);
        CREATE INDEX IF NOT EXISTS idx_npcs_title    ON Npcs(npc_title);
        CREATE INDEX IF NOT EXISTS idx_skills_name   ON Skills(skillname);
        CREATE INDEX IF NOT EXISTS idx_states_name   ON States(buff_name);
        CREATE INDEX IF NOT EXISTS idx_summons_name  ON Summons(summon_name);
        """;

    public static void Initialize(string filePath)
    {
        using var connection = new SqliteConnection($"Data Source={filePath}");
        connection.Open();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = CreateScript;
        cmd.ExecuteNonQuery();
    }
}
```

- [ ] **Step 4: Run test, verify pass**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter SqliteSnapshotInitializerTests`
Expected: PASS.

- [ ] **Step 5: Commit**

```bash
git add src/App.Data/Infrastructure/SqliteSnapshotInitializer.cs tests/App.Data.Tests/SqliteSnapshotInitializerTests.cs
git commit -m "feat: SqliteSnapshotInitializer creates snapshot schema"
```

---

## Task 5: Snapshot export service interface

**Files:**
- Create: `src/App.Core/Interfaces/ISnapshotExportService.cs`

- [ ] **Step 1: Define interface**

Create `src/App.Core/Interfaces/ISnapshotExportService.cs`:

```csharp
using App.Core.Enums;

namespace App.Core.Interfaces;

public sealed record SnapshotExportProgress(string EntityName, int Current, int Total);

public interface ISnapshotExportService
{
    Task ExportAsync(
        DatabaseProvider sourceProvider,
        string sourceConnectionString,
        IReadOnlyDictionary<string, string> queryTokens,
        string targetSnapshotPath,
        IProgress<SnapshotExportProgress>? progress,
        CancellationToken cancellationToken);
}
```

- [ ] **Step 2: Build**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Succeeds.

- [ ] **Step 3: Commit**

```bash
git add src/App.Core/Interfaces/ISnapshotExportService.cs
git commit -m "feat: ISnapshotExportService interface"
```

---

## Task 6: SnapshotExportService implementation (via repository)

**Files:**
- Create: `src/App.Data/Services/SnapshotExportService.cs`
- Create: `tests/App.Data.Tests/SnapshotExportServiceTests.cs`

- [ ] **Step 1: Write failing test using a fixture Sqlite as the "source"**

Create `tests/App.Data.Tests/SnapshotExportServiceTests.cs`:

```csharp
using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Services;
using App.Data.Infrastructure;
using App.Data.Repositories;
using App.Data.Services;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SnapshotExportServiceTests
{
    [Fact]
    public async Task Export_CopiesAllEntitiesAndWritesMeta()
    {
        var sourcePath = Path.GetTempFileName();
        var targetPath = Path.GetTempFileName();
        File.Delete(sourcePath);
        File.Delete(targetPath);

        try
        {
            // Arrange — build a "source" Sqlite that has the same schema and a Sqlite query set
            SqliteSnapshotInitializer.Initialize(sourcePath);
            using (var seed = new SqliteConnection($"Data Source={sourcePath}"))
            {
                seed.Open();
                using var cmd = seed.CreateCommand();
                cmd.CommandText = """
                    INSERT INTO Items VALUES (1,'Sword','sword.png');
                    INSERT INTO Monsters VALUES (10,'Goblin',5,'Forest');
                    INSERT INTO Npcs VALUES (20,'Blacksmith',1.0,2.0,'shop.lua');
                    INSERT INTO Skills VALUES (30,'Slash','slash.png');
                    INSERT INTO States VALUES (40,'Poison','poison.png');
                    INSERT INTO Summons VALUES (50,'Wolf','Wolf Card','wolf.png');
                    """;
                cmd.ExecuteNonQuery();
            }

            var queryStore = new TestQueryStore();   // see helper below
            var factory = new DbConnectionFactory();
            var repo = new GameDataRepository(queryStore, factory);
            var service = new SnapshotExportService(repo);

            // Act
            await service.ExportAsync(
                DatabaseProvider.Sqlite,
                $"Data Source={sourcePath}",
                new Dictionary<string, string>(),
                targetPath,
                progress: null,
                CancellationToken.None);

            // Assert
            using var conn = new SqliteConnection($"Data Source={targetPath}");
            conn.Open();
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Items"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Monsters"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Npcs"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Skills"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM States"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Summons"));
            Assert.Equal("1", ScalarString(conn, "SELECT value FROM Meta WHERE key='schema_version'"));
            Assert.Equal("Sqlite", ScalarString(conn, "SELECT value FROM Meta WHERE key='source_provider'"));
        }
        finally
        {
            if (File.Exists(sourcePath)) File.Delete(sourcePath);
            if (File.Exists(targetPath)) File.Delete(targetPath);
        }
    }

    private static long ScalarLong(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand(); cmd.CommandText = sql;
        return (long)cmd.ExecuteScalar()!;
    }

    private static string ScalarString(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand(); cmd.CommandText = sql;
        return (string)cmd.ExecuteScalar()!;
    }
}

internal sealed class TestQueryStore : IQueryStore
{
    public string GetQuery(DatabaseProvider provider, QueryEntity entity, IReadOnlyDictionary<string, string>? tokens = null)
        => entity switch
        {
            QueryEntity.Items    => "SELECT item_id, name_en, icon_file_name FROM Items",
            QueryEntity.Monsters => "SELECT id, name, level, location FROM Monsters",
            QueryEntity.Npc      => "SELECT npc_id, npc_title, x, y, contact_script FROM Npcs",
            QueryEntity.Skills   => "SELECT skill_id, skillname, icon_file_name FROM Skills",
            QueryEntity.States   => "SELECT state_id, buff_name, icon_file_name FROM States",
            QueryEntity.Summons  => "SELECT summon_id, summon_name, card_name, icon_file_name FROM Summons",
            _ => throw new NotSupportedException(entity.ToString())
        };
}
```

- [ ] **Step 2: Run test, verify it fails (no service yet)**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter SnapshotExportServiceTests`
Expected: FAIL — `SnapshotExportService` does not exist.

- [ ] **Step 3: Implement the service**

Create `src/App.Data/Services/SnapshotExportService.cs`:

```csharp
using System.Globalization;
using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Models.Entities;
using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Services;

public sealed class SnapshotExportService : ISnapshotExportService
{
    private readonly IGameDataRepository _repo;

    public SnapshotExportService(IGameDataRepository repo) => _repo = repo;

    public async Task ExportAsync(
        DatabaseProvider sourceProvider,
        string sourceConnectionString,
        IReadOnlyDictionary<string, string> queryTokens,
        string targetSnapshotPath,
        IProgress<SnapshotExportProgress>? progress,
        CancellationToken cancellationToken)
    {
        if (File.Exists(targetSnapshotPath)) File.Delete(targetSnapshotPath);
        SqliteSnapshotInitializer.Initialize(targetSnapshotPath);

        await using var target = new SqliteConnection($"Data Source={targetSnapshotPath}");
        await target.OpenAsync(cancellationToken);
        await using var tx = (SqliteTransaction)await target.BeginTransactionAsync(cancellationToken);

        var items    = await _repo.GetItemsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Items", items, target, tx, "INSERT INTO Items VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.ItemId); cmd.Parameters.AddWithValue("$b", r.NameEn); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });

        var monsters = await _repo.GetMonstersAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Monsters", monsters, target, tx, "INSERT INTO Monsters VALUES ($a,$b,$c,$d)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.Id); cmd.Parameters.AddWithValue("$b", r.Name); cmd.Parameters.AddWithValue("$c", (object?)r.Level ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.Location ?? DBNull.Value); });

        var npcs     = await _repo.GetNpcsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Npcs", npcs, target, tx, "INSERT INTO Npcs VALUES ($a,$b,$c,$d,$e)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.NpcId); cmd.Parameters.AddWithValue("$b", r.NpcTitle); cmd.Parameters.AddWithValue("$c", (object?)r.X ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.Y ?? DBNull.Value); cmd.Parameters.AddWithValue("$e", (object?)r.ContactScript ?? DBNull.Value); });

        var skills   = await _repo.GetSkillsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Skills", skills, target, tx, "INSERT INTO Skills VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.SkillId); cmd.Parameters.AddWithValue("$b", r.Skillname); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });

        var states   = await _repo.GetStatesAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "States", states, target, tx, "INSERT INTO States VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.StateId); cmd.Parameters.AddWithValue("$b", r.BuffName); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });

        var summons  = await _repo.GetSummonsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Summons", summons, target, tx, "INSERT INTO Summons VALUES ($a,$b,$c,$d)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.SummonId); cmd.Parameters.AddWithValue("$b", r.SummonName); cmd.Parameters.AddWithValue("$c", (object?)r.CardName ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.IconFileName ?? DBNull.Value); });

        // Meta
        WriteMeta(target, tx, "schema_version", SqliteSnapshotInitializer.SchemaVersion.ToString(CultureInfo.InvariantCulture));
        WriteMeta(target, tx, "exported_at",    DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
        WriteMeta(target, tx, "source_provider", sourceProvider.ToString());

        await tx.CommitAsync(cancellationToken);
    }

    private static void ReportAndInsert<T>(
        IProgress<SnapshotExportProgress>? progress,
        string entityName,
        IReadOnlyList<T> rows,
        SqliteConnection conn,
        SqliteTransaction tx,
        string sql,
        Action<SqliteCommand, T> bind)
    {
        var total = rows.Count;
        for (var i = 0; i < total; i++)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            bind(cmd, rows[i]);
            cmd.ExecuteNonQuery();
            if ((i & 0x1F) == 0) progress?.Report(new SnapshotExportProgress(entityName, i + 1, total));
        }
        progress?.Report(new SnapshotExportProgress(entityName, total, total));
    }

    private static void WriteMeta(SqliteConnection c, SqliteTransaction tx, string key, string value)
    {
        using var cmd = c.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = "INSERT OR REPLACE INTO Meta(key,value) VALUES ($k,$v)";
        cmd.Parameters.AddWithValue("$k", key);
        cmd.Parameters.AddWithValue("$v", value);
        cmd.ExecuteNonQuery();
    }
}
```

Note: The repository methods `GetNpcsAsync`, `GetStatesAsync`, `GetSummonsAsync` may not yet exist on `IGameDataRepository`. If they don't, before continuing, add them mirroring the existing `GetItemsAsync`/`GetMonstersAsync` shape (no parameters except provider/connection/tokens/cancellation), backed by `QueryEntity.Npc`/`QueryEntity.States`/`QueryEntity.Summons`. Inspect `src/App.Core/Interfaces/IGameDataRepository.cs` and `src/App.Data/Repositories/GameDataRepository.cs` to confirm and add as needed in this same step.

- [ ] **Step 4: Run test, verify pass**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter SnapshotExportServiceTests`
Expected: PASS — all 6 entity counts equal 1, meta rows present.

- [ ] **Step 5: Commit**

```bash
git add src/App.Data/Services/SnapshotExportService.cs src/App.Core/Interfaces/IGameDataRepository.cs src/App.Data/Repositories/GameDataRepository.cs tests/App.Data.Tests/SnapshotExportServiceTests.cs
git commit -m "feat: SnapshotExportService copies all resource entities into a snapshot DB"
```

---

## Task 7: Add Sqlite section to queries.json

**Files:**
- Modify: `src/App.WinForms/Config/queries.json`

- [ ] **Step 1: Inspect existing queries.json**

Run: `head -80 src/App.WinForms/Config/queries.json`
Note the JSON structure (top-level provider keys `MSSQL`, `MySQL`, each with entity keys).

- [ ] **Step 2: Add Sqlite provider section**

Add a new top-level `"Sqlite"` object alongside `"MSSQL"` and `"MySQL"` with these entity keys (use the canonical `QueryEntity` names as keys, matching how the existing JSON does it):

```json
"Sqlite": {
  "Items":    "SELECT item_id, name_en, icon_file_name FROM Items WHERE name_en LIKE @SearchTerm ESCAPE '\\' ORDER BY name_en",
  "Monsters": "SELECT id, name, level, location FROM Monsters WHERE name LIKE @SearchTerm ESCAPE '\\' ORDER BY name",
  "Skills":   "SELECT skill_id, skillname, icon_file_name FROM Skills WHERE skillname LIKE @SearchTerm ESCAPE '\\' ORDER BY skillname",
  "States":   "SELECT state_id, buff_name, icon_file_name FROM States WHERE buff_name LIKE @SearchTerm ESCAPE '\\' ORDER BY buff_name",
  "Npc":      "SELECT npc_id, npc_title, x, y, contact_script FROM Npcs WHERE npc_title LIKE @SearchTerm ESCAPE '\\' ORDER BY npc_title",
  "Summons":  "SELECT summon_id, summon_name, card_name, icon_file_name FROM Summons WHERE summon_name LIKE @SearchTerm ESCAPE '\\' ORDER BY summon_name"
}
```

If existing queries do NOT take `@SearchTerm` (i.e. the `Items` / `Monsters` queries today are unfiltered `SELECT ... FROM ... ORDER BY ...`), match the existing shape instead — the goal is parity with how repository methods invoke them. Inspect a working pair (e.g. `MSSQL.Items`) and mirror the same parameter shape.

- [ ] **Step 3: Verify JSON is valid**

Run: `dotnet build YSM-GMTool.slnx`
Then run the app once briefly against a snapshot path to ensure config loads — defer real verification to Task 13's smoke test.

- [ ] **Step 4: Commit**

```bash
git add src/App.WinForms/Config/queries.json
git commit -m "feat: add Sqlite query set to queries.json"
```

---

## Task 8: Icon source abstraction (live-mode implementation)

**Files:**
- Create: `src/App.Core/Interfaces/IIconSource.cs`
- Create: `src/App.Core/Services/DirectoryIconSource.cs`
- Modify call sites in `src/App.WinForms/MainForm.cs` and `src/App.WinForms/Controls/EntityBrowserControl.cs` to use `IIconSource` instead of direct file reads.

- [ ] **Step 1: Define interface**

Create `src/App.Core/Interfaces/IIconSource.cs`:

```csharp
namespace App.Core.Interfaces;

public interface IIconSource
{
    /// <summary>Returns raw PNG bytes for the given icon file name, or null if not found.</summary>
    byte[]? TryGetIconBytes(string fileName);
}
```

- [ ] **Step 2: Implement directory source**

Create `src/App.Core/Services/DirectoryIconSource.cs`:

```csharp
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed class DirectoryIconSource : IIconSource
{
    private readonly string? _baseDirectory;

    public DirectoryIconSource(string? baseDirectory) => _baseDirectory = baseDirectory;

    public byte[]? TryGetIconBytes(string fileName)
    {
        if (string.IsNullOrWhiteSpace(_baseDirectory) || string.IsNullOrWhiteSpace(fileName))
            return null;

        var path = Path.Combine(_baseDirectory, fileName);
        if (!File.Exists(path)) return null;

        try { return File.ReadAllBytes(path); }
        catch { return null; }
    }
}
```

- [ ] **Step 3: Replace direct icon-file reads in WinForms with IIconSource**

In `MainForm.cs` (around line 394 where `iconsPath` is computed) and in any `EntityBrowserControl` icon resolution path, change construction to obtain an `IIconSource` (initially always `DirectoryIconSource(_settings.EntityIconsPath)` in live mode) and call `TryGetIconBytes(record.IconFileName)`. Decode bytes to `Image` with `Image.FromStream(new MemoryStream(bytes))`. Keep the existing in-memory cache via `LocalCacheService` keyed by file name.

Locate the existing icon-loading code first (search for `EntityIconsPath` and `iconsPath` usages), then refactor them to consume `IIconSource`. Do NOT change visible behavior — this step is a pure refactor that introduces the abstraction.

- [ ] **Step 4: Build and run smoke test in live mode**

Run: `dotnet build YSM-GMTool.slnx`
Manually launch the app against your live DB, open Items tab, confirm icons still render exactly as before.

- [ ] **Step 5: Commit**

```bash
git add src/App.Core/Interfaces/IIconSource.cs src/App.Core/Services/DirectoryIconSource.cs src/App.WinForms/MainForm.cs src/App.WinForms/Controls/EntityBrowserControl.cs
git commit -m "refactor: introduce IIconSource abstraction with DirectoryIconSource (live-mode parity)"
```

---

## Task 9: Sqlite icon source + icon pack service

**Files:**
- Create: `src/App.Core/Interfaces/IIconPackService.cs`
- Create: `src/App.Data/Infrastructure/SqliteIconSource.cs`
- Create: `src/App.Data/Services/IconPackService.cs`
- Create: `tests/App.Data.Tests/IconPackServiceTests.cs`
- Create: `tests/App.Data.Tests/SqliteIconSourceTests.cs`

- [ ] **Step 1: Define IIconPackService**

Create `src/App.Core/Interfaces/IIconPackService.cs`:

```csharp
namespace App.Core.Interfaces;

public sealed record IconPackProgress(int Current, int Total);

public interface IIconPackService
{
    Task<int> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IProgress<IconPackProgress>? progress,
        CancellationToken cancellationToken);
}
```

- [ ] **Step 2: Write failing test for IconPackService**

Create `tests/App.Data.Tests/IconPackServiceTests.cs`:

```csharp
using App.Data.Services;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class IconPackServiceTests
{
    [Fact]
    public async Task PackAsync_StoresAllPngsKeyedByFileName()
    {
        var srcDir = Path.Combine(Path.GetTempPath(), "icons_" + Guid.NewGuid().ToString("N"));
        var dbPath = Path.GetTempFileName(); File.Delete(dbPath);
        Directory.CreateDirectory(srcDir);
        try
        {
            var aBytes = new byte[] { 1, 2, 3 };
            var bBytes = new byte[] { 4, 5 };
            File.WriteAllBytes(Path.Combine(srcDir, "a.png"), aBytes);
            File.WriteAllBytes(Path.Combine(srcDir, "b.png"), bBytes);

            var service = new IconPackService();
            var packed = await service.PackAsync(srcDir, dbPath, null, CancellationToken.None);

            Assert.Equal(2, packed);

            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT png FROM Icons WHERE file_name=$n";
            cmd.Parameters.AddWithValue("$n", "a.png");
            Assert.Equal(aBytes, (byte[])cmd.ExecuteScalar()!);
        }
        finally
        {
            if (Directory.Exists(srcDir)) Directory.Delete(srcDir, true);
            if (File.Exists(dbPath)) File.Delete(dbPath);
        }
    }
}
```

- [ ] **Step 3: Write failing test for SqliteIconSource**

Create `tests/App.Data.Tests/SqliteIconSourceTests.cs`:

```csharp
using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SqliteIconSourceTests
{
    [Fact]
    public void TryGetIconBytes_ReturnsBlobOrNull()
    {
        var dbPath = Path.GetTempFileName(); File.Delete(dbPath);
        try
        {
            using (var c = new SqliteConnection($"Data Source={dbPath}"))
            {
                c.Open();
                using var cmd = c.CreateCommand();
                cmd.CommandText = "CREATE TABLE Icons(file_name TEXT PRIMARY KEY, png BLOB NOT NULL); INSERT INTO Icons VALUES('x.png', x'010203');";
                cmd.ExecuteNonQuery();
            }
            using var src = new SqliteIconSource(dbPath);
            Assert.Equal(new byte[] { 1, 2, 3 }, src.TryGetIconBytes("x.png"));
            Assert.Null(src.TryGetIconBytes("missing.png"));
        }
        finally
        {
            if (File.Exists(dbPath)) File.Delete(dbPath);
        }
    }
}
```

- [ ] **Step 4: Run both tests, verify they fail**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "IconPackServiceTests|SqliteIconSourceTests"`
Expected: FAIL — types do not exist.

- [ ] **Step 5: Implement IconPackService**

Create `src/App.Data/Services/IconPackService.cs`:

```csharp
using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Services;

public sealed class IconPackService : IIconPackService
{
    public async Task<int> PackAsync(
        string sourceDirectory,
        string targetIconsDbPath,
        IProgress<IconPackProgress>? progress,
        CancellationToken cancellationToken)
    {
        if (File.Exists(targetIconsDbPath)) File.Delete(targetIconsDbPath);

        await using var conn = new SqliteConnection($"Data Source={targetIconsDbPath}");
        await conn.OpenAsync(cancellationToken);

        await using (var schema = conn.CreateCommand())
        {
            schema.CommandText = """
                CREATE TABLE Icons (file_name TEXT PRIMARY KEY COLLATE NOCASE, png BLOB NOT NULL);
                CREATE TABLE Meta  (key TEXT PRIMARY KEY, value TEXT);
                """;
            await schema.ExecuteNonQueryAsync(cancellationToken);
        }

        var files = Directory.EnumerateFiles(sourceDirectory, "*.png", SearchOption.AllDirectories).ToArray();
        await using var tx = (SqliteTransaction)await conn.BeginTransactionAsync(cancellationToken);

        var inserted = 0;
        for (var i = 0; i < files.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Path.GetFileName(files[i]);
            var bytes = await File.ReadAllBytesAsync(files[i], cancellationToken);

            await using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT OR REPLACE INTO Icons(file_name, png) VALUES ($n, $p)";
            cmd.Parameters.AddWithValue("$n", name);
            cmd.Parameters.AddWithValue("$p", bytes);
            await cmd.ExecuteNonQueryAsync(cancellationToken);
            inserted++;

            if ((i & 0x1F) == 0) progress?.Report(new IconPackProgress(i + 1, files.Length));
        }

        await using (var meta = conn.CreateCommand())
        {
            meta.Transaction = tx;
            meta.CommandText = "INSERT OR REPLACE INTO Meta(key,value) VALUES ('packed_at',$t),('source_dir',$d),('file_count',$c)";
            meta.Parameters.AddWithValue("$t", DateTime.UtcNow.ToString("O"));
            meta.Parameters.AddWithValue("$d", sourceDirectory);
            meta.Parameters.AddWithValue("$c", inserted.ToString());
            await meta.ExecuteNonQueryAsync(cancellationToken);
        }

        await tx.CommitAsync(cancellationToken);
        progress?.Report(new IconPackProgress(files.Length, files.Length));
        return inserted;
    }
}
```

- [ ] **Step 6: Implement SqliteIconSource**

Create `src/App.Data/Infrastructure/SqliteIconSource.cs`:

```csharp
using App.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace App.Data.Infrastructure;

public sealed class SqliteIconSource : IIconSource, IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly object _lock = new();

    public SqliteIconSource(string iconsDbPath)
    {
        _connection = new SqliteConnection($"Data Source={iconsDbPath};Mode=ReadOnly");
        _connection.Open();
    }

    public byte[]? TryGetIconBytes(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return null;
        lock (_lock)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT png FROM Icons WHERE file_name = $n";
            cmd.Parameters.AddWithValue("$n", fileName);
            var result = cmd.ExecuteScalar();
            return result as byte[];
        }
    }

    public void Dispose() => _connection.Dispose();
}
```

- [ ] **Step 7: Run tests, verify pass**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "IconPackServiceTests|SqliteIconSourceTests"`
Expected: PASS.

- [ ] **Step 8: Commit**

```bash
git add src/App.Core/Interfaces/IIconPackService.cs src/App.Data/Infrastructure/SqliteIconSource.cs src/App.Data/Services/IconPackService.cs tests/App.Data.Tests/IconPackServiceTests.cs tests/App.Data.Tests/SqliteIconSourceTests.cs
git commit -m "feat: SqliteIconSource and IconPackService for offline icons"
```

---

## Task 10: AppMode enum and startup mode detection

**Files:**
- Create: `src/App.Core/Enums/AppMode.cs`
- Modify: `src/App.WinForms/Program.cs`

- [ ] **Step 1: Add enum**

Create `src/App.Core/Enums/AppMode.cs`:

```csharp
namespace App.Core.Enums;

public enum AppMode
{
    LiveDb,
    OfflineSnapshot
}
```

- [ ] **Step 2: Detect mode at startup and pass into MainForm**

Open `src/App.WinForms/Program.cs`. At the top of `Main` (after the existing logging/setup but before `Application.Run(new MainForm(...))`), add:

```csharp
var snapshotPath = Path.Combine(AppContext.BaseDirectory, "gmtool-snapshot.db");
var iconsDbPath  = Path.Combine(AppContext.BaseDirectory, "gmtool-icons.db");
var mode = File.Exists(snapshotPath) ? AppMode.OfflineSnapshot : AppMode.LiveDb;
Log.Information("App started in {Mode} mode", mode);
```

Then construct the `IIconSource` accordingly:

```csharp
IIconSource iconSource = mode == AppMode.OfflineSnapshot && File.Exists(iconsDbPath)
    ? new SqliteIconSource(iconsDbPath)
    : new DirectoryIconSource(settings.EntityIconsPath);
```

Pass `mode`, `snapshotPath`, and `iconSource` into `MainForm`'s constructor (extend the constructor signature and update accordingly).

- [ ] **Step 3: Build**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Succeeds.

- [ ] **Step 4: Commit**

```bash
git add src/App.Core/Enums/AppMode.cs src/App.WinForms/Program.cs src/App.WinForms/MainForm.cs
git commit -m "feat: detect AppMode at startup and select IIconSource implementation"
```

---

## Task 11: Hide Live-DB-only UI in offline mode

**Files:**
- Modify: `src/App.WinForms/MainForm.cs`
- Modify: `src/App.WinForms/Forms/SettingsForm.cs`

- [ ] **Step 1: In MainForm, hide PlayerChecker when mode is OfflineSnapshot**

In `MainForm.cs`, after the sidebar buttons and tabs are constructed, add:

```csharp
if (_mode == AppMode.OfflineSnapshot)
{
    _btnPlayerChecker.Visible = false;        // sidebar button (use actual field name)
    _playerCheckerActions.Visible = false;
    Text = "GM Tool (Offline Snapshot)";
}
```

Also disable any "Load Inventory" / "Load Warehouse" buttons inside `EntityBrowserControl` when mode is offline (or omit those buttons entirely for offline). If those buttons live elsewhere, locate by `grep` for "Inventory" and "Warehouse" in the WinForms project and add the same `Visible=false` guard.

- [ ] **Step 2: Configure repository connection routing for offline mode**

Where the `MainForm` (or its presenter) currently builds `(provider, connectionString)` to pass to `GameDataRepository`, switch on `_mode`:

```csharp
var (provider, connectionString) = _mode == AppMode.OfflineSnapshot
    ? (DatabaseProvider.Sqlite, $"Data Source={_snapshotPath};Mode=ReadOnly")
    : (_settings.Provider, _connectionStringBuilder.Build(_settings));
```

Pass an empty `queryTokens` dictionary in offline mode (Sqlite queries don't use tokens).

- [ ] **Step 3: In SettingsForm, hide Connection and TableNames tabs when offline**

In `SettingsForm.cs` constructor (after `_workingSettings` is initialized and tabs are created), add:

```csharp
if (_mode == AppMode.OfflineSnapshot)
{
    _tabsControl.TabPages.Remove(_tabConnection);
    _tabsControl.TabPages.Remove(_tabTableNames);
    if (_lblIconsPath is not null) _lblIconsPath.Visible = false;
    if (_txtEntityIconsPath is not null) _txtEntityIconsPath.Visible = false;
    if (_btnBrowseEntityIconsPath is not null) _btnBrowseEntityIconsPath.Visible = false;
}
```

(Adjust to actual field names — verify by reading the current file.)

- [ ] **Step 4: Build and run smoke test**

Run: `dotnet build YSM-GMTool.slnx`
Place a dummy `gmtool-snapshot.db` (just an empty file) next to the built exe, launch — verify title shows "(Offline Snapshot)" and PlayerChecker tab/button is gone, Settings shows only General tab. Then delete the dummy file and verify live mode still appears as before. (At this point a real snapshot is not yet loadable; the goal is purely to verify the UI gating.)

- [ ] **Step 5: Commit**

```bash
git add src/App.WinForms/MainForm.cs src/App.WinForms/Forms/SettingsForm.cs
git commit -m "feat: hide PlayerChecker, Connection, and TableNames UI in offline mode"
```

---

## Task 12: Settings — Export Snapshot button + progress dialog

**Files:**
- Create: `src/App.WinForms/Forms/ExportProgressForm.cs`
- Create: `src/App.WinForms/Forms/ExportProgressForm.Designer.cs`
- Modify: `src/App.WinForms/Forms/SettingsForm.cs`

- [ ] **Step 1: Create the progress form**

Create `src/App.WinForms/Forms/ExportProgressForm.cs`:

```csharp
using System.ComponentModel;

namespace App.WinForms.Forms;

public sealed partial class ExportProgressForm : Form
{
    public ExportProgressForm()
    {
        InitializeComponent();
    }

    public void Report(string entityName, int current, int total)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => Report(entityName, current, total));
            return;
        }

        _lblStatus.Text = $"{entityName}: {current} / {total}";
        _progressBar.Style = total > 0 ? ProgressBarStyle.Continuous : ProgressBarStyle.Marquee;
        if (total > 0)
        {
            _progressBar.Maximum = total;
            _progressBar.Value = Math.Min(current, total);
        }
    }
}
```

Create `src/App.WinForms/Forms/ExportProgressForm.Designer.cs` with a fixed `TableLayoutPanel` containing a `Label` (`_lblStatus`) and `ProgressBar` (`_progressBar`). Form `Size = 420 × 120`, `FormBorderStyle = FixedDialog`, `StartPosition = CenterParent`, `ControlBox = false`, `MinimizeBox = false`, `MaximizeBox = false`. (Use Visual Studio Designer to author this — keep the layout fully Designer-defined per the spec's UI policy.)

- [ ] **Step 2: Add Export button to Settings → General tab**

In `SettingsForm.cs`, inside the General tab construction (only when `_mode == AppMode.LiveDb`), add a button:

```csharp
var btnExportSnapshot = new Button
{
    Text = "Export Database to Snapshot…",
    Name = "btnExportSnapshot",
    AutoSize = false,
    Size = new Size(220, 28),
    Anchor = AnchorStyles.Top | AnchorStyles.Left
};
btnExportSnapshot.Click += BtnExportSnapshot_Click;
tlpGeneral.Controls.Add(btnExportSnapshot, 0, /* next row index */);
```

- [ ] **Step 3: Implement the click handler**

Add to `SettingsForm.cs`:

```csharp
private async void BtnExportSnapshot_Click(object? sender, EventArgs e)
{
    using var dlg = new SaveFileDialog
    {
        Title = "Save Snapshot",
        Filter = "GM Tool Snapshot (*.db)|*.db",
        FileName = "gmtool-snapshot.db",
        InitialDirectory = AppContext.BaseDirectory
    };
    if (dlg.ShowDialog(this) != DialogResult.OK) return;

    using var progressForm = new ExportProgressForm();
    var progress = new Progress<SnapshotExportProgress>(p => progressForm.Report(p.EntityName, p.Current, p.Total));

    var task = _exportService.ExportAsync(
        _workingSettings.Provider,
        _connectionStringBuilder.Build(_workingSettings),
        _workingSettings.GetQueryTokens(),
        dlg.FileName,
        progress,
        CancellationToken.None);

    progressForm.Show(this);
    try
    {
        await task;
        MessageBox.Show(this, "Snapshot exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Snapshot export failed");
        MessageBox.Show(this, $"Export failed: {ex.Message}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        progressForm.Close();
    }

    if (MessageBox.Show(this, "Also pack icons from the configured Entity Icons Path?", "Pack Icons",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    {
        if (string.IsNullOrWhiteSpace(_workingSettings.EntityIconsPath) ||
            !Directory.Exists(_workingSettings.EntityIconsPath))
        {
            MessageBox.Show(this, "Entity Icons Path is not configured or does not exist.", "Pack Icons",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var iconsDb = Path.Combine(Path.GetDirectoryName(dlg.FileName)!, "gmtool-icons.db");
        try
        {
            var packed = await _iconPackService.PackAsync(_workingSettings.EntityIconsPath, iconsDb, null, CancellationToken.None);
            MessageBox.Show(this, $"Packed {packed} icons.", "Pack Icons", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Icon pack failed");
            MessageBox.Show(this, $"Icon pack failed: {ex.Message}", "Pack Icons", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
```

`_exportService` and `_iconPackService` are constructor-injected `ISnapshotExportService` and `IIconPackService`. Update `SettingsForm`'s constructor signature and the call site in `MainForm` accordingly.

- [ ] **Step 4: Build and smoke test**

Run: `dotnet build YSM-GMTool.slnx`
Run app in live mode → Settings → click Export → choose a temp path → confirm progress runs and snapshot file is created. Verify it opens with `sqlite3` CLI or DB Browser:

```bash
sqlite3 /tmp/gmtool-snapshot.db ".tables"
```
Expected: lists `Items Meta Monsters Npcs Skills States Summons`.

- [ ] **Step 5: Commit**

```bash
git add src/App.WinForms/Forms/ExportProgressForm.cs src/App.WinForms/Forms/ExportProgressForm.Designer.cs src/App.WinForms/Forms/SettingsForm.cs src/App.WinForms/MainForm.cs
git commit -m "feat: Settings UI for exporting snapshot and packing icons"
```

---

## Task 13: End-to-end offline-mode smoke test

**Files:** none (manual verification)

- [ ] **Step 1: Build a snapshot from live**

Run app live → Settings → Export → save to `<repo>/src/App.WinForms/bin/Debug/net10.0-windows/gmtool-snapshot.db`. Pack icons when prompted.

- [ ] **Step 2: Restart app**

Close and re-launch. The app should boot in Offline mode (title contains "Offline Snapshot"). PlayerChecker tab is hidden. Settings → only General tab visible.

- [ ] **Step 3: Verify each entity tab works**

Open Items / Monster / Skills / Buffs / NPCs / Summons / Warps in turn. Type a partial name; results appear from the snapshot. Icons render (loaded from `gmtool-icons.db`).

- [ ] **Step 4: Generate a Lua command from a result**

Right-click an item / monster → action → confirm Lua command is built and copied to clipboard exactly as in live mode.

- [ ] **Step 5: Document any issues, fix and re-verify**

If any step fails, debug, fix, re-run from the failing step. Commit fixes individually.

- [ ] **Step 6: Commit any fixes from this task with descriptive messages**

If no fixes needed, no commit.

---

## Task 14: Audit existing ActionsControls and remove UiLayoutPolicy entry points

**Files:**
- Modify: `src/App.WinForms/MainForm.cs`
- Modify: `src/App.WinForms/Forms/SettingsForm.cs`
- Modify: `src/App.WinForms/Forms/AboutForm.cs`
- Delete: `src/App.WinForms/Layout/UiLayoutPolicy.cs`

This task removes the dynamic layout machinery. The 8 ActionsControls likely already render acceptably without it, with at most cosmetic glitches that Tasks 15–22 then fix per-control. We make the cut here so subsequent UI tasks have a clean slate.

- [ ] **Step 1: Remove all UiLayoutPolicy callsites**

In `src/App.WinForms/MainForm.cs`:
- Remove the entire `ApplyGlobalLayoutPolicySafe` method.
- Remove `_layoutDebounceTimer`, `_layoutPassInProgress` fields and any `Tick` handler that calls the layout pass.
- Remove `ResizeActionControlHeight` and `GetVisibleActionHosts` if they are only used by the removed layout pass.
- Remove any code in `MainForm` that wires resize/visible-changed events to the debounced layout pass.

In `src/App.WinForms/Forms/SettingsForm.cs` and `src/App.WinForms/Forms/AboutForm.cs`:
- Remove the `BeginInvoke((Action)(() => UiLayoutPolicy.ApplyFixedButtonSizes(this)));` lines.

- [ ] **Step 2: Delete UiLayoutPolicy.cs**

Run: `git rm src/App.WinForms/Layout/UiLayoutPolicy.cs`
Also delete the empty `src/App.WinForms/Layout/` directory if no other files remain.

- [ ] **Step 3: Build**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Builds cleanly. If unresolved references remain, scrub them — they refer to removed types and indicate missed call sites in Step 1.

- [ ] **Step 4: Quick visual check**

Run the app. Each tab will probably look slightly off (sections clipping, button sizes inconsistent). Note the worst offenders — Tasks 15–22 will fix them properly via Designer.

- [ ] **Step 5: Set MainForm MinimumSize from Designer**

Open `src/App.WinForms/MainForm.Designer.cs`, locate `this.MinimumSize = new System.Drawing.Size(...)` (or add it if missing in `InitializeComponent`). Set to a value that comfortably hosts the tallest action control — start with `new Size(1100, 720)` and adjust later if any control overflows.

- [ ] **Step 6: Commit**

```bash
git add -A
git commit -m "refactor: remove UiLayoutPolicy and all dynamic layout entry points"
```

---

## Tasks 15–22: TableLayoutPanel rewrite, one ActionsControl per task

For each of the 8 controls below, perform the same procedure (described once in Task 15). Each task takes ~30 min and is committed independently.

### Standard Layout Conventions (apply to every control)

- Root: `TableLayoutPanel`, `Dock = Fill`, fixed `RowCount`/`ColumnCount`.
- Section title row: `RowStyle = Absolute, 24px`.
- Single-line input row: `Absolute, 28px`.
- Action button row: `Absolute, 32px`.
- Stretch row (max one per section): `Percent, 100%`.
- Label column: `Absolute, 130px` (consistent across all controls).
- Input/content column: `Percent, 100%`.
- Right-aligned button column: `Absolute, 110px`.
- Buttons: `AutoSize=false`, explicit `Size = 100×28`, `Anchor=Top|Left`, `Margin=3,3,3,3`. Never `Dock=Fill`.
- GroupBoxes: `AutoSize=false`, explicit `Size`, `Padding=8,18,8,8`. Each contains its own inner `TableLayoutPanel`.

---

## Task 15: Rewrite PlayerCheckerActionsControl

**Files:**
- Modify: `src/App.WinForms/Controls/PlayerCheckerActionsControl.cs`
- Modify: `src/App.WinForms/Controls/PlayerCheckerActionsControl.Designer.cs`

- [ ] **Step 1: Inventory current control**

Open `PlayerCheckerActionsControl.Designer.cs`. List every GroupBox, button, label, input, and their current rough size/position. Note each section's purpose and the buttons inside.

- [ ] **Step 2: Plan the layout**

On paper or in a comment: define a root `TableLayoutPanel` with N rows × M columns. Map each existing GroupBox to a cell. Inside each GroupBox, define an inner `TableLayoutPanel` per the conventions above.

- [ ] **Step 3: Edit Designer.cs to apply the new structure**

Replace the existing `InitializeComponent` body (or substantially edit it) so that:
- Root is a `TableLayoutPanel` per conventions.
- Each `GroupBox` has explicit `Size`, `Padding=8,18,8,8`, `AutoSize=false`, contains an inner `TableLayoutPanel`.
- All buttons use `AutoSize=false`, explicit `Size=100×28`, `Anchor=Top|Left`, `Margin=3,3,3,3`.

For complex controls, the easiest path is to open the form/control in Visual Studio Designer, drop a new `TableLayoutPanel` as the root, drag existing children into the right cells, then save (Designer regenerates the file). If editing by hand, follow the structure shown in this snippet template:

```csharp
this._tlpRoot = new TableLayoutPanel
{
    Name = "tlpRoot",
    Dock = DockStyle.Fill,
    ColumnCount = 1,
    RowCount = 3,
    AutoSize = false
};
this._tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));   // search section
this._tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));   // results section
this._tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));    // filler
this._tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
this._tlpRoot.Controls.Add(this._grpSearch, 0, 0);
this._tlpRoot.Controls.Add(this._grpResults, 0, 1);
this.Controls.Add(this._tlpRoot);
```

Replicate the same pattern inside each GroupBox (its own `TableLayoutPanel` with row/column styles and child controls assigned to cells).

- [ ] **Step 4: Build**

Run: `dotnet build YSM-GMTool.slnx`
Expected: Builds.

- [ ] **Step 5: Visual check at MinimumSize and expanded**

Launch app (in live mode for this control), open PlayerChecker tab. Resize window to MinimumSize and to ~1.5× — confirm no clipping, buttons fully visible, no overlapping text.

- [ ] **Step 6: Commit**

```bash
git add src/App.WinForms/Controls/PlayerCheckerActionsControl.cs src/App.WinForms/Controls/PlayerCheckerActionsControl.Designer.cs
git commit -m "refactor: PlayerCheckerActionsControl uses Designer-defined TableLayoutPanel layout"
```

---

## Task 16: Rewrite ItemsActionsControl

Same procedure as Task 15, applied to `ItemsActionsControl`. (Repeat all 6 steps; do not skim — write the full layout from scratch following the conventions.)

**Files:**
- Modify: `src/App.WinForms/Controls/ItemsActionsControl.cs`
- Modify: `src/App.WinForms/Controls/ItemsActionsControl.Designer.cs`

- [ ] Inventory current control
- [ ] Plan layout (rows/cols, sizes)
- [ ] Edit Designer.cs to apply new structure (follow conventions; use the snippet template from Task 15 Step 3 as a model)
- [ ] Build
- [ ] Visual check at MinimumSize and expanded
- [ ] Commit with message `refactor: ItemsActionsControl uses Designer-defined TableLayoutPanel layout`

---

## Task 17: Rewrite MonsterActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/MonsterActionsControl.cs`
- Modify: `src/App.WinForms/Controls/MonsterActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: MonsterActionsControl …`)

---

## Task 18: Rewrite SkillsActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/SkillsActionsControl.cs`
- Modify: `src/App.WinForms/Controls/SkillsActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: SkillsActionsControl …`)

---

## Task 19: Rewrite BuffsActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/BuffsActionsControl.cs`
- Modify: `src/App.WinForms/Controls/BuffsActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: BuffsActionsControl …`)

---

## Task 20: Rewrite NpcsActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/NpcsActionsControl.cs`
- Modify: `src/App.WinForms/Controls/NpcsActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: NpcsActionsControl …`)

---

## Task 21: Rewrite SummonsActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/SummonsActionsControl.cs`
- Modify: `src/App.WinForms/Controls/SummonsActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: SummonsActionsControl …`)

---

## Task 22: Rewrite WarpActionsControl

Same procedure as Task 15.

**Files:**
- Modify: `src/App.WinForms/Controls/WarpActionsControl.cs`
- Modify: `src/App.WinForms/Controls/WarpActionsControl.Designer.cs`

- [ ] Inventory, plan, edit Designer, build, visual check, commit (`refactor: WarpActionsControl …`)

---

## Task 23: Final MinimumSize tuning

**Files:**
- Modify: `src/App.WinForms/MainForm.Designer.cs`

- [ ] **Step 1: Walk every tab at the temporary MinimumSize from Task 14**

Launch app. Open every tab. Identify the tallest/widest control. If any clip at the current `MinimumSize`, increase the dimension that's too small.

- [ ] **Step 2: Set the final MinimumSize**

Update `this.MinimumSize = new System.Drawing.Size(W, H);` in `MainForm.Designer.cs` to the smallest values where every tab fits without clipping.

- [ ] **Step 3: Verify on first launch**

Close and reopen app. Confirm initial window size ≥ MinimumSize and every tab is visually clean immediately on first paint, no flicker, no resize after open.

- [ ] **Step 4: Commit**

```bash
git add src/App.WinForms/MainForm.Designer.cs
git commit -m "chore: finalize MainForm MinimumSize after layout rewrite"
```

---

## Task 24: Documentation update

**Files:**
- Modify: `README.md`

- [ ] **Step 1: Add an "Offline Snapshot Mode" section to README**

After the "First-time Setup" section in `README.md`, add:

```markdown
## 📦 Offline Snapshot Mode

The tool can run fully offline against a pre-baked snapshot file, with no live database connection.

### Producing a snapshot (admin)
1. Configure the tool against your live DB as usual.
2. Open **Settings → General**, click **Export Database to Snapshot…**
3. Save `gmtool-snapshot.db` next to `GM Tool.exe` (or anywhere; copy it there before shipping).
4. When prompted, also pack icons — produces `gmtool-icons.db` next to the snapshot.

### Using a snapshot (recipient)
Place `gmtool-snapshot.db` (and optionally `gmtool-icons.db`) next to `GM Tool.exe`. Launch — the app boots in offline mode automatically (window title shows "Offline Snapshot"). PlayerChecker, Inventory, and Warehouse features are hidden because they require a live DB; all browsing and Lua command generation works as in live mode.
```

- [ ] **Step 2: Commit**

```bash
git add README.md
git commit -m "docs: document offline snapshot mode"
```

---

## Acceptance Checklist (run before declaring complete)

- [ ] `dotnet build YSM-GMTool.slnx` — succeeds with zero warnings introduced.
- [ ] `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj` — all green.
- [ ] Live mode: app launches, all tabs work as before, icons load from `EntityIconsPath`.
- [ ] Export: produces a snapshot file readable by `sqlite3 gmtool-snapshot.db ".tables"`.
- [ ] Icon pack: produces a `gmtool-icons.db` containing one row per PNG in `EntityIconsPath`.
- [ ] Offline mode: with `gmtool-snapshot.db` next to exe, app boots offline, title shows "(Offline Snapshot)", PlayerChecker hidden, Settings shows only General tab; all 6 entity tabs render and search; icons load from `gmtool-icons.db`.
- [ ] UI: no flicker on first paint, no late layout adjustments, every tab visually clean at MinimumSize.
- [ ] `UiLayoutPolicy.cs` is deleted from the repo.
