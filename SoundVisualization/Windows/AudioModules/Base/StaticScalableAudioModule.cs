using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace SoundVisualization.Windows.AudioModules.Base;

public abstract class StaticScalableAudioModule : StaticAudioModule
{
    readonly float startSize;
    public float StartSize => startSize;

    public StaticScalableAudioModule(double timer, Vector4 colour, float startSize, Vector3 position) : base(timer, colour, position)
    {
        this.startSize = startSize;
    }

    public StaticScalableAudioModule(double timer, Vector4 colour, float startSize, Vector2 screenPosition) : base(timer, colour, screenPosition)
    {
        this.startSize = startSize;
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
