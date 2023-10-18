using Dalamud.Plugin;

namespace SoundVisualization.Core.Handlers;

public class StartHandler
{
    public void Start(ref DalamudPluginInterface dalamudPluginInterface, SoundVisualizationPlugin plugin)
    {
        PluginHandlers.Start(ref dalamudPluginInterface);
        PluginLink.Start(ref dalamudPluginInterface, ref plugin);
    }
}
