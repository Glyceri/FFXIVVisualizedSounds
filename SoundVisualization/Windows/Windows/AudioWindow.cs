using SoundVisualization.Windows.Attributes;
using SoundVisualization.Windows.AudioModules.Base;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using System.Collections.Generic;

namespace SoundVisualization.Windows.Windows;

[PersistentAstralAetherWindow]
internal class AudioWindow : AstralAetherWindow
{
    public List<AudioModule> audioModules = new List<AudioModule>();

    public AudioWindow() : base("Audio Window")
    {
        SizeConstraints = new WindowSizeConstraints()
        {
            MaximumSize = new Vector2(500, 90),
            MinimumSize = new Vector2(500, 90),
        };

        Flags |= ImGuiWindowFlags.NoMove;
        Flags |= ImGuiWindowFlags.NoBackground;
        Flags |= ImGuiWindowFlags.NoInputs;
        Flags |= ImGuiWindowFlags.NoNavFocus;
        Flags |= ImGuiWindowFlags.NoResize;
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoTitleBar;
        Flags |= ImGuiWindowFlags.NoDecoration;
        Flags |= ImGuiWindowFlags.NoFocusOnAppearing;

        ForceMainWindow = true;

        IsOpen = true;
    }

    public unsafe override void OnDraw()
    {
        ImDrawListPtr imDrawListPtr = ImGui.GetBackgroundDrawList();
        foreach (AudioModule audioModule in audioModules)
        {
            if (audioModule.IsBlackened) continue;
            audioModule?.Draw(imDrawListPtr);
        }
    }
}
