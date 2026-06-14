CKAN-plugins
============

A collection of plugins for [CKAN](https://github.com/KSP-CKAN/CKAN) (Comprehensive Kerbal Archive Network), the mod manager for Kerbal Space Program.

> **Target CKAN Version**: v1.36.5 (builds against `ckan.exe` directly)
> **.NET Framework**: 4.8.1
> **Language**: C# 7.3+

---

## Plugins

### PartManagerPlugin (v2.0.0)

A CKAN GUI plugin that provides a tab for managing individual mod parts — enabling/disabling parts by moving them in/out of a cache directory, plus scanning craft files for missing parts.

**Features:**

- **Mod List** — Lists installed mods that contain parts; multi-select to filter the parts grid
- **Parts Grid** — DataGridView showing all parts (title, part name, file path) with per-part enable/disable checkboxes
- **Bulk Actions** — Enable All / Disable All buttons for the selected mods
- **Filtering** — Text + regex filtering by path, part name, or title
- **Stats** — Live counter showing total / enabled / disabled part counts
- **Persistence** — Disabled part lists are saved to `<CKAN>/PartManager/PartManager.json` as JSON
- **Craft Scanner** — Scans KSP `.craft` files (in `ships/VAB/`, `ships/SPH/`, etc.) to detect which parts are used in your vessels, then cross-references against GameData to find missing parts
- **Quick Lookup** — Select any missing part and open a browser search on CKAN, Spacedock, GitHub, or KerbalX

**Files:**

| File | Description |
|------|-------------|
| `PartManagerPlugin.cs` | Plugin entry point — implements `IGUIPlugin`, creates tab page, handles lifecycle |
| `PartManagerUI.cs` | Main WinForms `UserControl` — UI logic, registry access, event handlers |
| `PartManagerUI.Designer.cs` | WinForms designer layout — control hierarchy and property setup |
| `PartManagerUI.resx` | WinForms resource file |
| `Cache.cs` | Static helpers for moving parts to/from `<CKAN>/PartManager/cache/` |
| `CraftParser.cs` | Parses `.craft` files using regex (`part = name_serial#`) to extract part names |
| `PartScanner.cs` | Scans `GameData/*.cfg` files to check if a part exists in the installation |
| `ConfigNode.cs` | KSP-style `ConfigNode` data structure (name/value/node hierarchy) |
| `ConfigNodeReader.cs` | Parser for KSP `.cfg` format into `ConfigNode` trees |
| `Properties/AssemblyInfo.cs` | Assembly metadata (v2.0.0.0) |

### KerbalStuffPlugin (v1.0.0)

Maps KerbalStuff-hosted mods to CKAN modules. Uses the old `CKAN.IGUIPlugin` / `CKAN.Version` API.

> **⚠️ Needs migration** — uses deprecated `CKAN.IGUIPlugin` and `CKAN.Version` namespaces. Requires updates to `CKAN.GUI.IGUIPlugin` and `CKAN.Versioning.ModuleVersion` for CKAN v1.36.4+ compatibility.

### LogViewPlugin (v1.0.0)

Displays live log4net output from CKAN in a GUI tab. Uses the old `CKAN.IGUIPlugin` / `CKAN.Version` API.

> **⚠️ Needs migration** — same API deprecations as KerbalStuffPlugin. Also uses `log4net` which is ILMerged into `ckan.exe`.

### MigrationToolPlugin (v1.1.0)

GUI for migrating KSP installations (moving mods between game instances). Uses the old `CKAN.IGUIPlugin` / `CKAN.Version` API. Depends on `Newtonsoft.Json`.

> **⚠️ Needs migration** — same API deprecations as above. `Newtonsoft.Json` is ILMerged into `ckan.exe`.

---

## Build & Deployment

### Prerequisites

- Visual Studio 2022 (or [Build Tools for Visual Studio 2022](https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022))
- .NET Framework 4.8.1 SDK
- CKAN v1.36.4+ installed at the configured `CKANPath`

### Building

```powershell
# From the project directory (e.g., PartManagerPlugin):
MSBuild.exe PartManagerPlugin.sln /p:Configuration=Debug /t:Rebuild
```

### CKANPath Configuration

The path to CKAN is configured via the `CKANPath` MSBuild property in each `.csproj` file:

```xml
<CKANPath Condition="'$(CKANPath)' == ''">C:\Users\jared\Documents\_KSP\_software\CKAN</CKANPath>
```

The DLL is referenced with `<Private>False</Private>` so it is **not** copied to the plugin's output directory — CKAN loads it from its own location at runtime.

### Deployment

Copy the built `.dll` to the CKAN plugins folder of your game instance:

```
<KSP>/CKAN/Plugins/PartManagerPlugin.dll
```

Common locations:

| Instance | Path |
|----------|------|
| CKAN source | `C:\Users\jared\Documents\_KSP\_software\CKAN\Plugins\` |
| Steam KSP | `C:\Program Files (x86)\Steam\steamapps\common\Kerbal Space Program\CKAN\Plugins\` |
| H: drive KSP | `H:\_STEAM\steamapps\common\Kerbal Space Program\CKAN\Plugins\` |

### Plugin Loading (CKAN Internals)

CKAN loads plugins from `<CurrentInstance.CkanDir>/Plugins/` at startup:

1. `Assembly.UnsafeLoadFrom(dll)` loads the assembly
2. Looks for type `{DLLName}.{DLLName}` via `assembly.GetType()`
3. `Activator.CreateInstance(type)` creates the instance
4. Checks `is IGUIPlugin` — if not, the plugin is skipped
5. On success, the plugin is added to `m_DormantPlugins`
6. If `activate=true`, `ActivatePlugin()` calls `plugin.Initialize()` and moves it to active plugins
7. If `Initialize()` throws, the exception is caught/logged but the plugin stays dormant

---

## CKAN v1.36.4+ API Migration Notes

### Key API Changes

| Old (pre-1.36.4) | New (1.36.4+) |
|---|---|
| `CKAN.IGUIPlugin` (interface) | `CKAN.GUI.IGUIPlugin` (abstract class) |
| `CKAN.Version` | `CKAN.Versioning.ModuleVersion` |
| `Main.Instance.CurrentInstance.Registry` | `RegistryManager.Instance(instance, repoData).registry` |
| `Main.Instance.TabController` | Removed — use `FindMainTabControl()` recursive search |
| `modChangedCallback` | `Main.Instance.ManageMods.OnRegistryChanged` (parameterless event) |

### `IGUIPlugin` Abstract Class

The new `CKAN.GUI.IGUIPlugin` is an **abstract class** (not an interface). Override these members:

```csharp
public class MyPlugin : IGUIPlugin     // inherits, not implements
{
    public override void Initialize() { ... }
    public override void Deinitialize() { ... }
    public override string GetName() { ... }
    public override ModuleVersion GetVersion() { ... }
}
```

### `ModuleVersion`

Requires a `"v"` prefix in the version string:

```csharp
private readonly ModuleVersion VERSION = new ModuleVersion("v2.0.0");  // ✅
private readonly ModuleVersion VERSION = new ModuleVersion("2.0.0");   // ❌ fails
```

### Finding the Main TabControl

`TabController` was removed. Use recursive control search instead:

```csharp
private static TabControl FindMainTabControl()
{
    var byName = FindControlByName(Main.Instance, "MainTabControl") as TabControl;
    if (byName != null) return byName;
    return FindControlByType<TabControl>(Main.Instance);
}
```

### Registry Access

```csharp
var repoData = ServiceLocator.Container.Resolve<RepositoryDataManager>();
var registry = RegistryManager.Instance(instance, repoData).registry;
```

### ILMerged Dependencies

All CKAN internal dependencies (`Autofac`, `Newtonsoft.Json`, `log4net`, etc.) are **ILMerged** into `ckan.exe`. Do not reference them separately — reference only `ckan.exe` with `<Private>False</Private>`.

---

## Bug Fixes & Troubleshooting

### Fixed: Missing `new` Creation Statements in Designer

**Symptom**: `NullReferenceException` in `InitializeComponent()` at a seemingly arbitrary line.

**Cause**: The WinForms designer file was hand-edited to add new controls (`CraftGroupBox`, `StatsLabel`, `ScanShipsButton`, `MissingPartsListBox`, `LookupCkanButton`, `LookupSpacedockButton`, `LookupGithubButton`, `LookupKerbalxButton`, `CraftStatusLabel`) but their `new` object creation statements were missing.

**Fix**: Ensure every control field has a corresponding `new Type()` declaration in `InitializeComponent()` before any property assignments.

### Fixed: Stale DLL Deployment

**Symptom**: Source code had fixes but the plugin still threw NRE at runtime.

**Cause**: The `.csproj` used a custom `OutputPath=bin\DebugNew\` for testing, but the deployed DLL was the old version from `bin\Debug\`. The source was saved *after* the last build.

**Fix**: Always use `MSBuild.exe /t:Rebuild` and verify the output DLL timestamp matches the source file timestamps. Compare timestamps across all deployment locations.

### Fixed: Plugin Dormant After Deploy

**Symptom**: Plugin loads but stays dormant — no tab appears in CKAN.

**Causes & fixes**:
- `IGUIPlugin` is now an abstract class → use `class MyPlugin : IGUIPlugin` (not `: CKAN.IGUIPlugin`)
- `ModuleVersion` constructor fails without `"v"` prefix
- NRE in `Initialize()` → check the full call stack including `InitializeComponent()`
- log4net has no file-based logging configured → exceptions are silently discarded

### Common Build Issues

| Issue | Fix |
|-------|-----|
| `MSBuild.exe` not found | Use VS 2022 Build Tools path: `C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe` |
| Missing `Autofac` / `Newtonsoft.Json` | Both are ILMerged into `ckan.exe` — no separate references needed |
| `ckan.exe` locked by another process | Use a different output path (e.g., `bin\DebugNew\`) or kill the locking process |
| Wrong .NET Framework | All plugins target .NET Framework 4.8.1 (not .NET Core / .NET 5+) |

---

## Plugin Compatibility Matrix

| Plugin | CKAN API Version | Status |
|--------|-----------------|--------|
| **PartManagerPlugin** | `CKAN.GUI.IGUIPlugin` / `CKAN.Versioning.ModuleVersion` | ✅ **v2.0.0** — fully migrated |
| KerbalStuffPlugin | `CKAN.IGUIPlugin` / `CKAN.Version` | ⚠️ Pre-1.36.4 — needs migration |
| LogViewPlugin | `CKAN.IGUIPlugin` / `CKAN.Version` | ⚠️ Pre-1.36.4 — needs migration |
| MigrationToolPlugin | `CKAN.IGUIPlugin` / `CKAN.Version` | ⚠️ Pre-1.36.4 — needs migration |
