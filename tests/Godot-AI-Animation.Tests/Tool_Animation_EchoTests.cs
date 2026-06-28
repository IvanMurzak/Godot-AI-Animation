/*
┌──────────────────────────────────────────────────────────────────┐
│  Author: Ivan Murzak (https://github.com/IvanMurzak)             │
│  Copyright (c) 2026 Ivan Murzak                                  │
│  Licensed under the Apache License, Version 2.0.                 │
│  See the LICENSE file in the project root for more information.  │
└──────────────────────────────────────────────────────────────────┘
*/
#nullable enable
using com.IvanMurzak.Godot.MCP.Animation;
using Xunit;

namespace com.IvanMurzak.Godot.MCP.Animation.Tests
{
    /// <summary>
    /// One-test-per-tool unit scaffold (the Unity/Unreal template parity pattern). Pins the
    /// PURE-MANAGED sample tool <c>animation-echo</c> by constructing the tool family and
    /// invoking the method directly — no Godot binary, no MCP server. Copy this class per pure-managed
    /// tool you add; verify editor-only tools (#if TOOLS) via the headless-Godot E2E instead.
    /// </summary>
    public class Tool_Animation_EchoTests
    {
        [Fact]
        public void Echo_WithMessage_ReturnsItVerbatim()
        {
            var tool = new Tool_Animation();
            Assert.Equal("hello-godot", tool.Echo("hello-godot"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Echo_WithoutMessage_ReturnsReadySentinel(string? message)
        {
            var tool = new Tool_Animation();
            Assert.Equal("animation-ready", tool.Echo(message));
        }

        [Fact]
        public void EchoToolId_IsStable()
        {
            // The id the dock / godot-cli / catalog reference must not drift silently.
            Assert.Equal("animation-echo", Tool_Animation.EchoToolId);
        }
    }
}
