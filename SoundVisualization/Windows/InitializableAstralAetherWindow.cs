using ImGuiNET;

namespace SoundVisualization.Windows;

public abstract class InitializableAstralAetherWindow : AstralAetherWindow
{
    protected InitializableAstralAetherWindow(string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow) { }

    public abstract void OnInitialized();
}
