# Animation Tools

AI MCP tools for Godot's built-in **animation system** (`AnimationPlayer` + `AnimationLibrary` +
`Animation`), for [Godot-MCP / AI Game Developer](https://github.com/IvanMurzak/Godot-MCP).

A **source-only** MCP tool extension: the package ships C# source (no compiled DLL, no bundled Godot)
that compiles inside your Godot project against your own GodotSharp, so it never locks you to a Godot
version.

## Install

Requires the core [`godot_mcp`](https://github.com/IvanMurzak/Godot-MCP) addon in your Godot C# project.

```bash
# via the godot-cli (resolves from the shared catalog, edits your .csproj, rebuilds)
godot-cli install-extension com.IvanMurzak.Godot.MCP.Animation

# …or add the reference manually and rebuild:
#   <PackageReference Include="com.IvanMurzak.Godot.MCP.Animation" Version="0.1.0" />
```

…or pick it from the **Extensions** dock inside the Godot editor.

After a rebuild, the extension's `[AiToolType]` tool family is auto-discovered — no registry edit.

## Tools

| Tool | Kind | Description |
| --- | --- | --- |
| `animation-defaults` | pure-managed | Return the recommended starter config (length, loop mode, no tracks) for a new animation. |
| `animation-player-create` | editor | Create an `AnimationPlayer` node in the edited scene (optional `name`/`parentPath`). |
| `animation-library-add` | editor | Add a new named `AnimationLibrary` to a player. |
| `animation-create` | editor | Create an `Animation` in a player's library (`name`, optional `libraryName`/`length`/`loopMode`); the library is auto-created when missing. |
| `animation-add-track` | editor | Add a `value` or 3D transform (`position-3d`/`rotation-3d`/`scale-3d`) track to an animation. |
| `animation-insert-key` | editor | Insert a keyframe on a track (scalar `value`, or `x`/`y`/`z` for transform tracks). |
| `animation-get` | editor | Read a player's libraries + animation list, and (optionally) one animation's full config (read-only). |

All editor tools marshal every Godot call onto the editor main thread; lengths/times are clamped to
valid ranges before they touch a resource.

License: Apache-2.0.
