using Dalamud.Plugin;
using SoundVisualization.Core.Handlers;

namespace SoundVisualization;

public sealed class SoundVisualizationPlugin : IDalamudPlugin
{
    public SoundVisualizationPlugin(DalamudPluginInterface dalamud) => new StartHandler().Start(ref dalamud, this);
    public void Dispose() => PluginLink.QuitHandler.Quit();
}
