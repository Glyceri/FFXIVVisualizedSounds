using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules.Base;

public abstract class StaticAudioModule : AudioModule
{
    readonly Vector3 position;
    public Vector3 Position => position;

    public Vector2 ScreenPosition => WorldToScreen(Position);

    public StaticAudioModule(double timer, Vector4 colour, Vector3 position) : base(timer, colour)
    {
        this.position = position;
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
