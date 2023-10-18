using ImGuiNET;
using System.Numerics;

namespace SoundVisualization.Windows.AudioModules.Base;

public abstract class StaticAudioModule : AudioModule
{
    readonly Vector3 position;
    public Vector3 Position => position;

    public Vector2 ScreenPosition => overrideScreenPosition != Vector2.Zero ? overrideScreenPosition : WorldToScreen(Position);

    readonly Vector2 overrideScreenPosition;

    public StaticAudioModule(double timer, Vector4 colour, Vector3 position) : base(timer, colour)
    {
        this.position = position;
    }

    public StaticAudioModule(double timer, Vector4 colour, Vector2 ScreenPosition) : base(timer, colour)
    {
        overrideScreenPosition = ScreenPosition;
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
