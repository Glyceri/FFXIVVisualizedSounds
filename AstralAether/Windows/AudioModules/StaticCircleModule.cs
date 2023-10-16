using AstralAether.Windows.AudioModules.Base;
using ImGuiNET;
using System;
using System.Numerics;

namespace AstralAether.Windows.AudioModules;

public class StaticCircleModule : StaticScalableAudioModule
{
    public float ClampSize { get; set; } = 18;
    public bool Animated { get; set; } = true;

    readonly int sections;
    readonly float rimSize;

    public StaticCircleModule(double timer, Vector4 colour, Vector3 position, float startSize, int sections = 12, float rimSize = 3.0f) : base(timer, colour, startSize, position) 
    { 
        this.sections = sections;
        this.rimSize = rimSize;
    }

    public StaticCircleModule(double timer, Vector4 colour, Vector2 screenPosition, float startSize, int sections = 12, float rimSize = 3.0f) : base(timer, colour, startSize, screenPosition)
    {
        this.sections = sections;
        this.rimSize = rimSize;
    }


    public override void Draw(ImDrawListPtr drawListPtr)
    {
        if (ScreenPosition == Vector2.Zero) return;
        float size = Math.Clamp(StartSize * (Animated ? Timer : 1.0f), 0, ClampSize);
        drawListPtr.AddCircle(ScreenPosition, size, Colour, sections, rimSize);
    }
}
