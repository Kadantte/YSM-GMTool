<div align="center">

# YSM-GMTool

**A modern Windows desktop GM utility for Rappelz private servers.**

Browse live game data, inspect player inventories & warehouses, and generate Lua GM commands straight to your clipboard — backed by your own MSSQL or MySQL game database.

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
[![Avalonia](https://img.shields.io/badge/Avalonia-11-8B44AC?logo=avalonia&logoColor=white)](https://avaloniaui.net/)
[![ReactiveUI](https://img.shields.io/badge/MVVM-ReactiveUI-902C9C)](https://www.reactiveui.net/)
[![Platform](https://img.shields.io/badge/platform-Windows%20x64-0078D6?logo=windows&logoColor=white)](#-requirements)
[![Database](https://img.shields.io/badge/database-MSSQL%20%7C%20MySQL-CC2927?logo=microsoftsqlserver&logoColor=white)](#-configuration)

![Playerchecker](Images/Playerchecker.png)

</div>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Requirements](#-requirements)
- [Installation](#-installation)
- [First-time Setup](#-first-time-setup)
- [Usage Guide](#-usage-guide)
- [Configuration](#-configuration)
- [Application Data](#-application-data)
- [Project Structure](#-project-structure)
- [Architecture & Extensibility](#-architecture--extensibility)
- [Tech Stack](#-tech-stack)
- [Building & Publishing](#-building--publishing)
- [Troubleshooting](#-troubleshooting)
- [License & Disclaimer](#-license--disclaimer)

---

## 🔎 Overview

YSM-GMTool is a Game-Master companion for **Rappelz** private servers. Instead of typing long, error-prone Lua commands by hand, you browse the actual game database (resources, characters, items) through a clean tabbed UI, click an action, and the correctly-formatted command is copied to your clipboard — ready to paste into the in-game GM console.

It connects **directly to your server's database** (Microsoft SQL Server or MySQL/MariaDB), so every list of monsters, items, skills, NPCs and players is real, searchable, and up to date.

The application is a full rewrite of the original WinForms tool, now built on **Avalonia 11 + ReactiveUI** on **.NET 10**, with a native dark Fluent theme, fully async database access, and a modular tab architecture.

> **Why a database, not flat files?** Reading straight from Arcadia/Telecaster/Auth means the tool always matches *your* server's content — custom items, renamed maps, modified skills — with no manual export step.

---

## ✨ Features

### Per-tab tooling

| Tab | What you can do |
|-----|-----------------|
| **Playerchecker** | Search characters by **account** or **char name** with live results; see **ONLINE / OFFLINE** status; load all/online characters; open a player's **inventory** or **warehouse** in a pop-out window (with item icons & double-tap copy). |
| **Monster** | Search monsters by ID or name; spawn at **your position**, the **selected player's position**, or **specific X/Y/Layer** coordinates; set amount and an optional **lifetime** (minutes, or permanent). |
| **Items** | Search items by ID or name (with icons); **give** items to yourself or another player (amount, enhance, level, status flag); **modify equipped items** — edit level/enhance/itemcode or change appearance per wear slot. |
| **Skills** | Search skills; **learn / set level / remove / learn-all** for yourself or a selected player; learn **creature skills** by slot. |
| **Buffs** | Browse state/buff records; cast **timed world states**, add/remove **event states**, and apply or remove **player / creature buffs** with custom level and duration. |
| **NPC** | Browse NPCs and search by **contact script**; **add** an NPC to the world (at coordinates, optionally for a selected player); **show / hide** NPCs; **warp** to an NPC. |
| **Summons** | Search summons (by summon name or card name); **insert** a summon with an optional starting **stage**; **stage-enhance** a summon slot (0–5). |
| **Warp** | **31 built-in** teleport locations plus your own **custom warp points** (added/removed and saved in settings); warp a player to a location, warp a player to you, or warp yourself to a player. |

### General & quality-of-life

- **Command-to-clipboard** — every action builds a typed Lua command and copies it straight to the OS clipboard. No manual formatting.
- **Optional `/run` prefix** — toggle **Append /run** in the top bar to prepend `/run ` to generated commands (comments and already-prefixed commands are left untouched).
- **Target-player selector** — pick a target from the top-bar dropdown (defaults to `self`); most commands adapt to *self* or *another player*. Manage your player list from the top bar.
- **Double-click to copy** — double-click any grid cell to copy its value to the clipboard.
- **Smart search** — search-as-you-type (debounced) or exact **Search** button; **ID-range** queries like `100-200`; diacritic-insensitive name matching.
- **Result limiting** — cap grids to **1000 rows** for snappy rendering while keeping the full DB subset searchable.
- **Entity icons** — point the tool at your icons folder and item/skill/buff/summon icons render in the grids and inventory window (rows grow to fit).
- **Local cache** — optionally cache entity data (and bulk **Export to Cache**) for fast, offline browsing.
- **Native dark theme**, compact density, always-visible scrollbars.

---

## 🧰 Requirements

- **Windows 10 / 11 (x64)**
- **[.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)** (the app is published framework-dependent)
- Access to your Rappelz server database, either:
  - **Microsoft SQL Server** / Azure SQL, or
  - **MySQL / MariaDB** (via MySqlConnector)
- Read access to the game schema: **Arcadia** (resources), **Telecaster** (characters & items) and **Auth/Accounts** (accounts)
- *(Build from source only)* the **[.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)**

---

## 📦 Installation

### Option A — Download a release build

Publish a self-contained-free build (see [Building & Publishing](#-building--publishing)) or grab one from your distribution channel, then run **`GM Tool.exe`**. Requires the .NET 10 Desktop Runtime.

### Option B — Run from source

```bash
git clone https://github.com/YoSiem/YSM-GMTool.git
cd YSM-GMTool
dotnet restore YSM-GMTool.slnx
dotnet run --project src/App.Desktop/App.Desktop.csproj
```

### Option C — One-step release publish (Windows)

```powershell
# Publishes framework-dependent win-x64 to Documents\YSMReleasedTools\GM-Tool\
./scripts/publish-release.ps1
```

The script stops any running `GM Tool.exe`, cleans the output folder, and publishes a fresh build.

---

## 🛠️ First-time Setup

1. **Launch** the application.
2. Click the **Settings (⚙ gear)** icon in the **top command bar**.
3. On the **Connection** tab:
   - Choose a provider: **MSSQL** or **MySQL** (the default port switches automatically — 1433 / 3306).
   - Enter **Server**, **Port**, **Database**, **User Id**, **Password**. *(For MSSQL you can use Integrated Security and toggle Encrypt / Trust Server Certificate.)*
   - Click **Test Connection** to verify, then click **Save**.
4. On the **Table Names** tab, set the database and resource-table tokens to match your server's schema (see [Configuration](#-configuration)).
5. *(Optional)* On the **General** tab, enable **Use local cache**, turn on **Entity icons** and point it at your icons folder, or hit **Export to Cache** for offline browsing.

<div align="center">
  <img src="Images/Settings.png" alt="Connection settings" width="48%">
  <img src="Images/Settings2.png" alt="Table-name settings" width="48%">
</div>

---

## 📖 Usage Guide

The window is split into a **top command bar** and a **tab strip** — there is no sidebar.

**Top command bar** (left → right): app logo & title · **target-player** dropdown (`self` by default) · **manage-players** flyout (add/remove) · **Append /run** toggle · **Settings (⚙)** · **About (ⓘ)**.

Typical workflow:

1. Pick (or add) a **target player** in the top bar — or leave it on `self`.
2. Open the tab you need and **search** for an entity (by ID, name, or contact script depending on the tab).
3. Fill in the action fields (amount, level, coordinates…) and click an **action button**.
4. The generated Lua command is now on your **clipboard** — paste it into the game's GM console.

### Playerchecker spotlight

Search **by Account** or **by Char Name**, load all or only **online** characters, then click **Load inventory** / **Load WH** to open that player's items in a **pop-out window** — complete with optional item icons and double-click-to-copy per column.

<div align="center">

| Items | Monsters | Skills |
|:---:|:---:|:---:|
| ![Items](Images/items.png) | ![Monster](Images/Monster.png) | ![Skills](Images/Skills.png) |
| **Buffs** | **NPCs** | **Summons** |
| ![Buffs](Images/Buffs.png) | ![NPCs](Images/NPCs.png) | ![Summons](Images/Summons.png) |

</div>

---

## ⚙️ Configuration

### Connection & database settings

All connection settings live in **Settings → Connection** and are persisted to `settings.json` (see [Application Data](#-application-data)). You can also override them with environment variables / a `.env` file (below).

### Table-name tokens — `src/App.Desktop/Config/queries.json`

Queries are grouped by **provider** (`MSSQL`, `MySQL`) and **entity key**. Tokens written as `{{TokenName}}` are replaced at runtime from **Settings → Table Names**:

| Token | Maps to | Default |
|-------|---------|---------|
| `{{ArcadiaName}}` | Resource database | `Arcadia` |
| `{{TelecasterName}}` | Character / item database | `Telecaster` |
| `{{AuthName}}` | Auth database | `Auth` |
| `{{AccountsName}}` | Accounts database | `Accounts` |
| `{{CharacterResource}}` | Character resource table | `CharacterResource` |
| `{{MonsterResource}}` | Monster resource table | `MonsterResource` |
| `{{StringResource}}` | String lookup table | `StringResource` |
| `{{ItemResource}}` | Item resource table | `ItemResource` |
| `{{SkillResource}}` | Skill resource table | `SkillResource` |
| `{{StateResource}}` | State / buff resource table | `StateResource` |
| `{{NpcResource}}` | NPC resource table | `NpcResource` |
| `{{SummonResource}}` | Summon resource table | `SummonResource` |

Parameterized queries use **Dapper named parameters** (`@SearchTerm`, `@OwnerId`, `@AccountName`). Entity keys in the file include `Playerchecker*`, `PlayerInventory`, `PlayerWarehouse`, `Monsters`, `Items`, `Skills`, `States`, `NPC`, `Summons`.

### Lua commands — `src/App.Core/Commands/LuaCommands.cs`

Lua commands are **hardcoded as a typed static class** (31 methods, formatted with `InvariantCulture` so decimal separators are always server-safe). There is no `lua_commands.json` — to match your server's Lua API, edit the relevant `LuaCommands` method.

### Environment variables & `.env`

The database provider and connection string can be supplied via environment variables, which **override** `settings.json` at startup:

```env
YSM_DB_PROVIDER=MSSQL
YSM_DB_CONNECTION_STRING=Server=localhost;Database=Arcadia;User Id=sa;Password=...;TrustServerCertificate=True
```

A `.env` file is auto-discovered next to the executable, in the working directory, the app-data folder, or any parent directory. Saving settings in-app also writes these keys to a `.env` next to the executable. `.env` is gitignored.

---

## 📁 Application Data

Stored under `%LocalAppData%\YSM-GMTool\`:

| Data | Path |
|------|------|
| User settings | `%LocalAppData%\YSM-GMTool\settings.json` |
| Entity cache | `%LocalAppData%\YSM-GMTool\cache\*.json` |
| Log files | `%LocalAppData%\YSM-GMTool\logs\gmtool-YYYYMMDD.log` |

Logs roll daily (Serilog file sink). The published executable installs to `%USERPROFILE%\Documents\YSMReleasedTools\GM-Tool\` when using the publish script.

---

## 🗂️ Project Structure

```
YSM-GMTool/
├─ YSM-GMTool.slnx              # Solution (modern .slnx format)
├─ Directory.Build.props        # Shared C# settings (latest lang, nullable, implicit usings)
├─ scripts/
│  └─ publish-release.ps1       # One-step framework-dependent win-x64 publish
├─ Images/                      # README screenshots
├─ docs/                        # Design plans
└─ src/
   ├─ App.Core/                 # Models, enums, abstractions, services
   │  └─ Commands/LuaCommands.cs    #   typed Lua command catalog (hardcoded)
   ├─ App.Data/                 # Dapper repository + DB connection factory (MSSQL/MySQL)
   └─ App.Desktop/              # Avalonia + ReactiveUI UI
      ├─ Shell/                     #   MainWindow + top command bar
      ├─ Features/                  #   one folder per tab (Playerchecker, Monster, …)
      ├─ Controls/                  #   shared EntityBrowser grid control
      ├─ Theme/                     #   dark Fluent theme
      └─ Config/queries.json        #   SQL per provider & entity
```

---

## 🏗️ Architecture & Extensibility

**Three projects:**

- **App.Core** — domain models, enums, service abstractions, and the typed `LuaCommands` catalog. No UI, no DB driver.
- **App.Data** — `GameDataRepository` (Dapper) and `DbConnectionFactory`, which creates a `SqlConnection` or `MySqlConnection` from the active provider. Queries come from `queries.json` via the query store.
- **App.Desktop** — the Avalonia + ReactiveUI shell, tab modules, view models, views, and theme. DI is wired with `Microsoft.Extensions.DependencyInjection` and bridged to ReactiveUI through Splat.

**Tabs are modules.** Each tab implements **`ITabModule`** (`Title`, `IconKey`, `Order`) by inheriting `TabModuleViewModel`. At startup the container **reflection-scans the assembly** for `ITabModule` implementations, registers each as a singleton, and the shell orders them by `Order`.

> **Add a new tab** by creating a `…TabViewModel : TabModuleViewModel` and a matching `…TabView`. It is auto-discovered and slotted into the tab strip — no manual registration. Browsing/searching grids are provided by the reusable `EntityBrowser` control, so a new tab mostly declares its columns and action buttons.

---

## 💻 Tech Stack

| Component | Library | Version |
|-----------|---------|---------|
| Runtime | .NET | 10 |
| UI framework | Avalonia (Desktop, Fluent, DataGrid) | 11.x |
| MVVM | Avalonia.ReactiveUI | 11.x |
| Icons | Projektanker.Icons.Avalonia.FontAwesome | 9.x |
| DI | Microsoft.Extensions.DependencyInjection + Splat bridge | 9.x / 15.x |
| Data access | Dapper | 2.1.66 |
| SQL Server | Microsoft.Data.SqlClient | 6.1.4 |
| MySQL / MariaDB | MySqlConnector | 2.5.0 |
| Logging | Serilog + Serilog.Sinks.File | 4.x / 7.x |

---

## 🔨 Building & Publishing

```bash
# Restore + build the whole solution
dotnet build YSM-GMTool.slnx -c Release

# Run the desktop app
dotnet run --project src/App.Desktop/App.Desktop.csproj

# Framework-dependent publish (win-x64) — produces "GM Tool.exe"
dotnet publish src/App.Desktop/App.Desktop.csproj -c Release -r win-x64 --self-contained false -o publish
```

The app targets `win-x64`, builds as a `WinExe` named **`GM Tool`**, and uses `Assets/Logo.ico`. The convenience script `scripts/publish-release.ps1` performs the publish into `Documents\YSMReleasedTools\GM-Tool\`.

---

## 🔧 Troubleshooting

| Problem | Solution |
|---------|----------|
| **SQL Server certificate error** | Add `TrustServerCertificate=True` to the connection string, or tick the checkbox in **Settings → Connection**. |
| **Empty grid after searching** | Verify the provider, connection details, and the **Table Names** tokens in Settings. |
| **Playerchecker shows no results** | Make sure `TelecasterName` and `ArcadiaName` are correct — these queries span multiple databases. |
| **Warehouse items not loading** | Verify the `AuthName` / `AccountsName` tokens and that the accounts database is reachable. |
| **Icons don't show** | Enable **Entity icons** in Settings → General and point it at a valid icons folder. |
| **Publish fails with a file lock** | Close `GM Tool.exe` from `%USERPROFILE%\Documents\YSMReleasedTools\GM-Tool\`, then rebuild (the publish script does this for you). |
| **App won't start** | Confirm the **.NET 10 Desktop Runtime** is installed; check the latest log in `%LocalAppData%\YSM-GMTool\logs\`. |

---

## 📜 License & Disclaimer

This repository does **not currently include a license file**, so default copyright applies — all rights reserved by the authors. If you intend to share or accept contributions, add a `LICENSE` (e.g. MIT) to clarify usage terms.

> **Disclaimer:** YSM-GMTool is an administration tool for **private** Rappelz servers you own or operate. It connects directly to your game database and issues GM-level commands — use it responsibly and only on servers you are authorized to manage.
