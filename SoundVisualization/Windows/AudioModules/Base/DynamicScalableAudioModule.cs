using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace SoundVisualization.Windows.AudioModules.Base;

public unsafe abstract class DynamicScalableAudioModule : DynamicAudioModule
{
    readonly float startSize;
    public float StartSize => startSize;

    public DynamicScalableAudioModule(double timer, Vector4 colour, GameObject* followMe, Vector3 offset, float startSize) : base(timer, colour, followMe, offset)
    {
        this.startSize = startSize;
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
