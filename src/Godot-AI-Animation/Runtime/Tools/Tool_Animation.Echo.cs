/*
┌──────────────────────────────────────────────────────────────────┐
│  Author: Ivan Murzak (https://github.com/IvanMurzak)             │
│  Copyright (c) 2026 Ivan Murzak                                  │
│  Licensed under the Apache License, Version 2.0.                 │
│  See the LICENSE file in the project root for more information.  │
└──────────────────────────────────────────────────────────────────┘
*/
#nullable enable
using System.ComponentModel;
using com.IvanMurzak.McpPlugin;

namespace com.IvanMurzak.Godot.MCP.Animation
{
    public partial class Tool_Animation
    {
        /// <summary>The tool id, exposed as a const so tests / E2E reference the exact string.</summary>
        public const string EchoToolId = "animation-echo";

        /// <summary>
        /// Pure-managed sample tool — no Godot native API, so it lives OUTSIDE <c>#if TOOLS</c> and is
        /// fully CI-unit-testable (see <c>Tool_Animation_EchoTests</c>) and E2E-verifiable via
        /// <c>godot-cli run-tool animation-echo</c>. Replace it with your real tool(s); keep
        /// pure-managed tools here and editor-driving tools under <c>../../Editor/Tools/</c>.
        /// </summary>
        [AiTool
        (
            EchoToolId,
            Title = "Animation Tools / Echo",
            ReadOnlyHint = true,
            IdempotentHint = true,
            OpenWorldHint = false
        )]
        [Description("Sample readiness probe for the Animation Tools extension. Returns the input " +
            "'message' echoed back, or 'animation-ready' when omitted. Proves the extension's " +
            "tool family is discovered and callable end-to-end after the package compiles into the " +
            "consumer's Godot project.")]
        public string Echo
        (
            [Description("Optional message to echo back. When null or empty, returns 'animation-ready'.")]
            string? message = null
        )
        {
            return string.IsNullOrEmpty(message) ? "animation-ready" : message;
        }
    }
}
