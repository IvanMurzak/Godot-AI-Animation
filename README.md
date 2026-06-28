<h1 align="center">Godot AI Tools Template</h1>

<p align="center">
  Template for authoring custom <b>MCP tools</b> for the Godot editor — extensions for
  <a href="https://github.com/IvanMurzak/Godot-MCP">Godot-MCP / AI Game Developer</a>.
</p>

Write C# `[AiToolType]` tools (the same authoring model as Unity-MCP) and ship them as a **source-only
NuGet package** that compiles inside any consumer's Godot project — no bundled Godot, no version lock.
This template gives you the buildable skeleton, the packaging recipe, cross-platform setup scripts, CI,
and a test scaffold.

> **Use this template:** click **“Use this template”** on GitHub (this repo has the Template flag on),
> create your repo, clone it, then run `init` (below).

---

## 1. Scaffold → init

`init` renames the package to the `Godot-AI-<Feature>` convention (package id
`com.IvanMurzak.Godot.MCP.<Feature>`), replaces placeholders, and activates the CI workflows.

```powershell
# Windows / PowerShell
./commands/init.ps1 -Feature "Particles"
```
```bash
# macOS / Linux
python3 commands/init.py --feature Particles
```

Override any derived value: `-DisplayName`/`--display-name`, `-Description`/`--description`,
`-GitHubRepository`/`--github-repository`, `-ToolPrefix`/`--tool-prefix`.

What you get after `init -Feature Particles`:

```
src/Godot-AI-Particles/
  Godot-AI-Particles.csproj            # source-only package (Godot.NET.Sdk; recipe in docs/)
  build/com.IvanMurzak.Godot.MCP.Particles.props   # injects the source into the consumer
  Runtime/Tools/Tool_Particles.cs      # [AiToolType] family
  Runtime/Tools/Tool_Particles.Echo.cs # pure-managed sample tool  (particles-echo)
  Editor/Tools/Tool_Particles.EditorInfo.cs  # editor-only sample (#if TOOLS) (particles-editor-info)
tests/Godot-AI-Particles.Tests/        # xUnit unit scaffold (pure-managed tools)
testbed/                               # consumer Godot project for headless E2E
commands/                              # init / bump-version / get-version / update-core
.github/workflows/                     # test-pull-request, release, reusable test, bump_version
docs/source-only-nuget-recipe.md       # the packaging recipe (read this)
```

## 2. Write tools

Tools are `[AiToolType]` classes with `[AiTool("<name>")]` methods (ReflectorNet-reflected), one family
per `partial class Tool_<Feature>`, each method in its own partial-class file. Split by API surface — the
same rule the core addon follows:

- **Pure-managed** tools (no Godot native API) → `Runtime/Tools/`, **outside** `#if TOOLS`. They compile
  everywhere and are CI-unit-testable.
- **Editor-driving** tools (`EditorInterface`, live `Node`/`Resource`) → `Editor/Tools/`, **behind**
  `#if TOOLS`, marshalling every Godot call through `MainThread.Instance.Run(...)`. Verified by E2E.

```csharp
[AiToolType]
public partial class Tool_Particles
{
    [AiTool("particles-echo", Title = "Particles / Echo", ReadOnlyHint = true)]
    [Description("Tell the LLM exactly what this tool does and when to use it.")]
    public string Echo([Description("Describe each parameter for the LLM.")] string? message = null)
        => string.IsNullOrEmpty(message) ? "particles-ready" : message;
}
```

## 3. Build & test (no Godot binary needed)

`Godot.NET.Sdk` pulls GodotSharp from NuGet, so the package builds and unit-tests headless:

```bash
dotnet build src/Godot-AI-Particles/Godot-AI-Particles.csproj          # compiles tools (Godot API resolves)
dotnet test  tests/Godot-AI-Particles.Tests/Godot-AI-Particles.Tests.csproj   # pure-managed unit tests
```

End-to-end (real editor): boot the headless testbed, install the core addon, build, and call each tool
via `godot-cli run-tool` — see `.github/workflows/test.yml` and the Godot-MCP testbed runbook.

## 4. Publish to NuGet

The package is **source-only** (`docs/source-only-nuget-recipe.md`): `IncludeBuildOutput=false`, sources
injected via `build/<PackageId>.props`, and **no GodotSharp dependency** (only `McpPlugin` /
`ReflectorNet` min-versions). Release is automatic and version-gated:

1. Add the `NUGET_API_KEY` repo secret (`gh secret set NUGET_API_KEY`).
2. Bump the version: `./commands/bump-version.ps1 -NewVersion 0.1.0` (or run the `bump version` workflow).
3. Merge to `main`. `release.yml` runs the full test matrix, publishes the source-only package to NuGet,
   and cuts an atomic GitHub Release.

Keep the core pins in lockstep with the addon when it advances:
`./commands/update-core.ps1 -McpPlugin 6.11.0 -ReflectorNet 5.4.0`.

## 5. Install your extension (in a consumer Godot project)

Requires the core [`godot_mcp`](https://github.com/IvanMurzak/Godot-MCP) addon. Then either:

- **Extensions dock** — pick it inside the Godot editor (Install → adds the `<PackageReference>` → rebuild).
- **CLI** — `godot-cli install-extension com.IvanMurzak.Godot.MCP.Particles`.
- **By hand** — add `<PackageReference Include="com.IvanMurzak.Godot.MCP.Particles" Version="x.y.z" />`
  to the consumer `.csproj` and rebuild.

After a rebuild the tools are auto-discovered. To list your extension in the dock + CLI, append one entry
to the shared catalog `addons/godot_mcp/extensions.catalog.json` in **Godot-MCP**.

## Layout & docs

- `docs/source-only-nuget-recipe.md` — the packaging recipe (the centerpiece).
- `docs/ci.md` — workflows, the version gate, the multi-Godot matrix, required secrets.
- `CLAUDE.md` — notes for maintaining this template.

License: **Apache-2.0**.
