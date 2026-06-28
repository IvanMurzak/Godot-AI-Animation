# CLAUDE.md — Godot-AI-Animation

A **Godot-MCP extension**: an MCP tool family for Godot's built-in animation system —
`AnimationPlayer` nodes, `AnimationLibrary` resources, `Animation` resources, their tracks and
keyframes — shipped as a **source-only NuGet package** (`com.IvanMurzak.Godot.MCP.Animation`) that
compiles inside a consumer's Godot project against the consumer's own GodotSharp. Created from
[`Godot-AI-Tools-Template`](https://github.com/IvanMurzak/Godot-AI-Tools-Template). The packaging recipe
is the load-bearing detail — read `docs/source-only-nuget-recipe.md`.

## Layout

- `src/Godot-AI-Animation/` — the source-only package (`Godot.NET.Sdk`).
  - `Runtime/Tools/Tool_Animation.cs` — the `[AiToolType]` family (one partial class).
  - `Runtime/Tools/Tool_Animation.Ids.cs` — all tool-id consts (pure-managed; pinned by tests).
  - `Runtime/Tools/Tool_Animation.Defaults.cs` — `animation-defaults` (pure-managed tool).
  - `Runtime/Animation/` — pure-managed support types: `AnimationTrackType`, `AnimationDefaults`
    (starter config + parameter-validation rules), `AnimationInfo` (all unit-tested).
  - `Editor/Tools/Tool_Animation.{Editor,PlayerCreate,LibraryAdd,Create,AddTrack,InsertKey,Get}.cs` —
    editor tools behind `#if TOOLS` (touch `EditorInterface`/live nodes/resources; main-thread-marshalled;
    E2E-verified).
  - `build/com.IvanMurzak.Godot.MCP.Animation.props` — the source-injection props (auto-imported by NuGet
    in the consumer; MUST stay named `<PackageId>.props`).
- `tests/Godot-AI-Animation.Tests/` — xUnit specs for the pure-managed sources only (no Godot binary).
- `testbed/Animation-Testbed.csproj` — a consumer `Godot.NET.Sdk` project that restores the local-packed
  package; `dotnet build` of it is the source-injection proof.

## Tools

| Tool | Kind | File |
| --- | --- | --- |
| `animation-defaults` | pure-managed | `Runtime/Tools/Tool_Animation.Defaults.cs` |
| `animation-player-create` | editor | `Editor/Tools/Tool_Animation.PlayerCreate.cs` |
| `animation-library-add` | editor | `Editor/Tools/Tool_Animation.LibraryAdd.cs` |
| `animation-create` | editor | `Editor/Tools/Tool_Animation.Create.cs` |
| `animation-add-track` | editor | `Editor/Tools/Tool_Animation.AddTrack.cs` |
| `animation-insert-key` | editor | `Editor/Tools/Tool_Animation.InsertKey.cs` |
| `animation-get` | editor | `Editor/Tools/Tool_Animation.Get.cs` |

## Build / test (no Godot binary)

```bash
dotnet build src/Godot-AI-Animation/Godot-AI-Animation.csproj   # source-only package compiles tools
dotnet test  tests/Godot-AI-Animation.Tests/Godot-AI-Animation.Tests.csproj
dotnet pack  src/Godot-AI-Animation/Godot-AI-Animation.csproj -p:Version=0.0.0-ci -o local-nuget
dotnet build testbed/Animation-Testbed.csproj                  # consumes the local package (injection proof)
```

`Godot.NET.Sdk` supplies GodotSharp from NuGet, so no Godot install is needed to build/test/pack or to
prove the source-injection recipe (the testbed build is a faithful proxy for `godot --build-solutions`).
The recipe is verified to compile into the consumer across the CI's multi-Godot matrix (Godot 4.3/4.4/4.5).
When proving locally, note `dotnet pack` re-uses the **global NuGet cache** for an already-cached version:
if you re-pack the same `Version`, clear `~/.nuget/packages/com.ivanmurzak.godot.mcp.animation/<ver>` (or
pack a unique version) before re-restoring the testbed, or you'll silently build the stale cached source.

## Conventions

- Root namespace `com.IvanMurzak.Godot.MCP.Animation`. Every `.cs` starts with the Apache-2.0 header.
- Pure-managed tools (no Godot native API) → `Runtime/` (outside `#if TOOLS`, unit-testable); editor-driving
  tools → `Editor/` (behind `#if TOOLS`, every Godot call via `MainThread.Instance.Run(...)`, E2E-verified).
- The package declares ONLY the `com.IvanMurzak.McpPlugin` / `com.IvanMurzak.ReflectorNet` min-version
  deps; **GodotSharp must never become a package dependency** (CI asserts the nuspec). Keep the MCP pins in
  lockstep with the core Godot-MCP addon; bump with `commands/update-core.ps1`.
- One `[AiToolType] partial class Tool_Animation`; one `[AiTool]` method per partial-class file. New
  pure-managed sources must be added to the test csproj `<Compile Include>` list to be unit-tested.

## Find detail in

- `docs/source-only-nuget-recipe.md` — the packaging recipe (the centerpiece) + the consumer story.
- `docs/ci.md` — workflows, the version gate, multi-Godot matrix, required secrets.
