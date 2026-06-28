<h1 align="center">Godot AI Animation</h1>

<p align="center">
  AI <b>MCP tools</b> for Godot's built-in <b>animation system</b> (AnimationPlayer + AnimationLibrary +
  Animation) — an extension for
  <a href="https://github.com/IvanMurzak/Godot-MCP">Godot-MCP / AI Game Developer</a>.
</p>

`Godot-AI-Animation` is a Godot-MCP extension. It adds a focused MCP tool family for Godot's built-in
animation stack — `AnimationPlayer` nodes, `AnimationLibrary` resources, `Animation` resources, their
tracks and keyframes — authored in C# with `[AiToolType]` / `[AiTool]` (the same model as Unity-MCP and
the core Godot-MCP addon), and shipped as a **source-only NuGet package** that compiles inside any
consumer's Godot project against the consumer's own GodotSharp — no bundled Godot, no version lock.
Created from
[`Godot-AI-Tools-Template`](https://github.com/IvanMurzak/Godot-AI-Tools-Template).

## Tools

| Tool | Kind | Description |
| --- | --- | --- |
| `animation-defaults` | pure-managed | Return the recommended starter config (length, loop mode, suggested track types) for a new animation. |
| `animation-player-create` | editor (`#if TOOLS`) | Create an `AnimationPlayer` node in the edited scene; optional parent and name. |
| `animation-library-add` | editor (`#if TOOLS`) | Add an `AnimationLibrary` to an `AnimationPlayer` (library name). |
| `animation-create` | editor (`#if TOOLS`) | Create an `Animation` in a library (name, length, loop mode). |
| `animation-add-track` | editor (`#if TOOLS`) | Add a track to an animation (track type `value`/`position-3d`/`rotation-3d`/`scale-3d`; node path; sub-property for value tracks). |
| `animation-insert-key` | editor (`#if TOOLS`) | Insert a keyframe into a track (animation, track index, time, value). |
| `animation-get` | editor (`#if TOOLS`) | Read an `AnimationPlayer`'s animation list / an animation's config (read-only). |

Pure-managed tools (no Godot native API) live under `src/Godot-AI-Animation/Runtime/` and are
CI-unit-tested; editor-driving tools live under `Editor/` behind `#if TOOLS` and marshal every Godot
call onto the editor main thread via `MainThread.Instance.Run(...)`.

## Install (in a consumer Godot project)

Requires the core [`godot_mcp`](https://github.com/IvanMurzak/Godot-MCP) addon. Then either:

- **Extensions dock** — pick it inside the Godot editor (Install → adds the `<PackageReference>` → rebuild).
- **CLI** — `godot-cli install-extension com.IvanMurzak.Godot.MCP.Animation`.
- **By hand** — add `<PackageReference Include="com.IvanMurzak.Godot.MCP.Animation" Version="x.y.z" />`
  to the consumer `.csproj` and rebuild.

After a rebuild the `[AiToolType]` tool family is auto-discovered — no registry edit.

## Build & test (no Godot binary needed)

`Godot.NET.Sdk` pulls GodotSharp from NuGet, so the package builds and unit-tests headless:

```bash
dotnet build src/Godot-AI-Animation/Godot-AI-Animation.csproj            # compiles tools (Godot API resolves)
dotnet test  tests/Godot-AI-Animation.Tests/Godot-AI-Animation.Tests.csproj   # pure-managed unit tests
dotnet pack  src/Godot-AI-Animation/Godot-AI-Animation.csproj -p:Version=0.0.0-ci -o local-nuget
dotnet build testbed/Animation-Testbed.csproj                            # consumer build = source-injection proof
```

The testbed build proves the source-injection recipe: the package's `.cs` are injected as `<Compile>`
items into the consumer and compile against the consumer's own GodotSharp. CI runs this across a
multi-Godot-version matrix; an end-to-end leg additionally boots real headless Godot, installs the core
addon, and (once a local MCP server is wired) calls each tool via `godot-cli run-tool`.

## Docs

- `docs/source-only-nuget-recipe.md` — the packaging recipe (the centerpiece).
- `docs/ci.md` — workflows, the version gate, the multi-Godot matrix, required secrets.
- `CLAUDE.md` — maintainer notes.

## Publish

Source-only, version-gated release (see `docs/ci.md`): publishing uses NuGet Trusted Publishing (OIDC,
no stored API key), bump `<Version>` (`commands/bump-version.ps1 -NewVersion x.y.z`), merge to `main`;
`release.yml` runs the full matrix, publishes the package to NuGet, and cuts an atomic GitHub Release.

License: **Apache-2.0**.
