using Dalamud.Interface.Windowing;
using ImGuiNET;
using SoundVisualization.Core.AutoRegistry.Interfaces;

namespace SoundVisualization.Windows;

public abstract class AstralAetherWindow : Window, IDisposableRegistryElement
{
    protected AstralAetherWindow(string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow) { }

    public void Dispose() => OnDispose();
    protected virtual void OnDispose() { }

    protected readonly bool drawToggle = false;

    public sealed override unsafe void Draw()
    {
        OnDraw();
    }

    public unsafe virtual void OnDraw() { }
    public override void Update() { base.Update(); }

    protected void SameLine() => ImGui.SameLine();
    protected void SameLineNoMargin() => ImGui.SameLine(0, 0);
    protected void SameLinePretendSpace() => ImGui.SameLine(0, 3f);  
}
