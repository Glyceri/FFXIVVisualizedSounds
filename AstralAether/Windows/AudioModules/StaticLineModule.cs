using AstralAether.Windows.AudioModules.Base;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules;

public class StaticLineModule : StaticAudioModule
{
    readonly float rimSize;

    readonly Vector3 endPosition;
    public Vector3 EndPosition => endPosition;

    public Vector2 ScreenEndPosition => WorldToScreen(endPosition);

    public StaticLineModule(double timer, Vector4 colour, Vector3 position, Vector3 endPosition, float rimSize = 0.3f) : base(timer, colour, position)
    {
        this.endPosition = endPosition;
        this.rimSize = rimSize;
    }

    public override void Draw(ImDrawListPtr drawListPtr)
    {
        drawListPtr.AddLine(ScreenPosition, ScreenEndPosition, Colour, rimSize);
    }
}
