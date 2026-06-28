# CLAUDE.md — Godot-AI-Tools-Template

This is the **template repo** for authoring Godot-MCP extension packages (the Godot analog of
`Unity-AI-Tools-Template`). It ships placeholder boilerplate that `commands/init.ps1` / `init.py`
customize into a real extension. Changes here propagate to every future extension created from it — keep
placeholder tokens consistent.

## Placeholders (replaced by `init`)

| Token | Example value | Used in |
| --- | --- | --- |
| `YOUR_FEATURE` | `Particles` | namespaces, `Tool_<Feature>`, folder/file names, the package id suffix |
| `YOUR_TOOL_PREFIX` | `particles` | tool ids (`<prefix>-echo`, …) |
| `YOUR_DISPLAY_NAME` | `Particles Tools` | package `Title`, docs |
| `YOUR_DESCRIPTION` | `AI MCP tools for Godot Particles.` | package `Description` |
| `YOUR_GITHUB_USERNAME_REPOSITORY` | `IvanMurzak/Godot-AI-Particles` | repo URLs |

The package id is always `com.IvanMurzak.Godot.MCP.<Feature>` and is written literally as
`com.IvanMurzak.Godot.MCP.YOUR_FEATURE`. `init` also renames `build/<id>.props` and activates CI
(`*.yml-sample` → `*.yml`).

## Build / test (no Godot binary)

```bash
dotnet build src/Godot-AI-YOUR_FEATURE/Godot-AI-YOUR_FEATURE.csproj   # source-only package compiles tools
dotnet test  tests/Godot-AI-YOUR_FEATURE.Tests/Godot-AI-YOUR_FEATURE.Tests.csproj
dotnet pack  src/Godot-AI-YOUR_FEATURE/Godot-AI-YOUR_FEATURE.csproj -p:Version=0.0.0-ci -o local-nuget
dotnet build testbed/YOUR_FEATURE-Testbed.csproj                      # consumes the local package (injection proof)
```

`Godot.NET.Sdk` supplies GodotSharp from NuGet, so no Godot install is needed to build/test/pack or to
prove the source-injection recipe (the testbed build is a faithful proxy for `godot --build-solutions`).

## Conventions

- Root namespace `com.IvanMurzak.Godot.MCP.<Feature>` (mirrors the core addon).
- Pure-managed tools → `Runtime/Tools/` (outside `#if TOOLS`, unit-testable); editor-driving tools →
  `Editor/Tools/` (behind `#if TOOLS`, main-thread-marshalled, E2E-verified only).
- One `[AiToolType] partial class Tool_<Feature>`; one `[AiTool]` method per partial-class file.
- Core MCP pins (`com.IvanMurzak.McpPlugin`, `com.IvanMurzak.ReflectorNet`) must match the core Godot-MCP
  addon; bump everywhere with `commands/update-core.ps1`. Never let GodotSharp become a package dep (see
  `docs/source-only-nuget-recipe.md`; CI asserts it).
- CI ships as `*.yml-sample` (inert on the template); `init` activates it. Never commit live
  `.github/workflows/*.yml` here.

## Find detail in

- `docs/source-only-nuget-recipe.md` — the packaging recipe (the centerpiece) + the consumer story.
- `docs/ci.md` — workflows, the version gate, multi-Godot matrix, the `NUGET_API_KEY` secret.
- `README.md` — the user-facing scaffold → init → write → build/test → publish → install walkthrough.
