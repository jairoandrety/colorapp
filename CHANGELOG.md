# Changelog

## [2.0.0]
Palettes are now independent of each other, and every color has two variants.

### Breaking
- Removed the shared `ColorAppData` label list. Each palette now defines its own colors (`ColorPalette.slots`); palettes no longer need to share the same label set.
- `ColorPaletteSetup` (list of single-color palettes) is replaced by `ColorPaletteLibrary` (list of `ColorPalette`, each with two-variant `ColorSlot` entries). Existing `.asset` files are untouched and can be converted with **Window/ColorApp/Migrar assets a 2.0**.
- `ColorizerHandler.OnPalleteColorChange` (a public `Action` field) is now a static event, `ColorizerHandler.PaletteChanged`.
- `ColorizerData`'s single-color override (`customColor`) is now a Primary/Secondary pair (`overridePrimary`/`overrideSecondary`); existing values are preserved automatically on load.

### Added
- Two color variants per palette entry (`ColorSlot.primary` / `.secondary`), with per-palette variant names (Light/Dark by default, renamable).
- `ColorizerHandler.ActiveVariant` to switch every Colorizer in a scene between variants at once.
- Palette list and two-column editor in the ColorApp window, with a "Paleta activa" section to assign/preview the active palette and variant per scene.
- Sample palette offered when a project has no `ColorPaletteLibrary` yet.
- Warning in the editor when two palettes have a different number of colors or mismatched keys, since color binding is still by index.
- Migration tool for 1.x assets (`Window/ColorApp/Migrar assets a 2.0`), including pairing two same-shaped palettes (e.g. "Normal" + "Dark") into one two-variant palette.

### Fixed
- Multi-object editing on `Colorizer` (and its subclasses) and `ColorizerHandler`: selecting two or more showed "Multi-object editing is not supported" instead of the inspector.
- Inspector no longer stamps the first selected object's values onto the rest. `Color Index`, `Override Color` and the palette dropdown now show a mixed-value state and only write when the control actually changes.
- Editing a multi-object selection now recolors every selected Colorizer, not just the first one.
- Package no longer breaks in a Player build (`AssetDatabase` calls were reachable outside `#if UNITY_EDITOR`).
- `ColorizerRenderer` no longer leaks a cloned material every repaint; it now uses `MaterialPropertyBlock`.
- `ColorizerHandler`/`Colorizer` no longer scan the scene with `FindAnyObjectByType` per object; a Colorizer created before its Handler now still receives updates.
- Fixed inspector height mismatch on `Colorizer` and `ColorizerHandler` components (drawer height didn't match what was actually drawn).
- Fixed a `NullReferenceException` on adding `ColorizerRenderer`/`ColorizerHandler` via "Add Component" with nothing assigned yet.
- Fixed a `NullReferenceException` in the ColorApp window when no GUISkin was found.
- Fixed color labels not saving (missing `SetDirty`) and an `Invalid GUILayout state` error in the window.
- Fixed an out-of-range palette or color index throwing instead of clamping to the last available value.
- `package.json` now declares its real minimum Unity version (2021.3, was incorrectly 2017.1).

## [1.0.3] - 2025-04-08
Version Update to 1.0.3
- Improvements and bug fixes.
- Improved loading and editing color palettes.
- It's now easier to load custom palettes.
- Fixed the samples folder so it can be added as a sample.

## [1.0.2] - 2024-02-10
- Performance modifications and utilities added to package

### This is the first release of *Unity Package com.jairoandrety.colorapp*.
- Migrated the custom version of Color App to Unity Asset Store.
