using SoundVisualization.Core.Handlers;
using ImGuiNET;
using System.Numerics;

namespace SoundVisualization.Windows.AudioModules.Base;

public abstract class AudioModule
{
    protected bool blackened = false;
    public bool IsBlackened { get => blackened; }

    double timer;
    public float Timer => (float)timer;

    readonly uint colour;
    public uint Colour => colour;
    Vector4 unbakedColour;
    public Vector4 UnbakedColour => unbakedColour;

    public AudioModule(double timer, Vector4 colour) 
    {
        this.timer = timer;
        unbakedColour = colour;
        this.colour = GetColour(colour);
    }

    public abstract void Draw(ImDrawListPtr drawListPtr);

    public virtual bool Tick(double time)
    {
        if (blackened) return true;
        timer -= time;
        return timer <= 0;
    }

    uint GetColour(Vector4 color)
    {
        uint ret = (byte)(color.W * 255);
        ret <<= 8;
        ret += (byte)(color.Z * 255);
        ret <<= 8;
        ret += (byte)(color.Y * 255);
        ret <<= 8;
        ret += (byte)(color.X * 255);
        return ret;
    }

    protected Vector2 WorldToScreen(Vector3 pos) => PluginHandlers.GameGui.WorldToScreen(pos, out var screenPos) ? screenPos : Vector2.Zero;
}