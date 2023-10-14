using Dalamud.Plugin;

namespace AstralAether.Core.Handlers;

public class StartHandler
{
    public void Start(ref DalamudPluginInterface dalamudPluginInterface, AstralAetherPlugin plugin)
    {
        PluginHandlers.Start(ref dalamudPluginInterface);
        PluginLink.Start(ref dalamudPluginInterface, ref plugin);
    }
}
