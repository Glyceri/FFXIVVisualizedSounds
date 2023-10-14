using Dalamud.Plugin;
using AstralAether.Core.Handlers;

namespace AstralAether;

public sealed class AstralAetherPlugin : IDalamudPlugin
{
    public AstralAetherPlugin(DalamudPluginInterface dalamud) => new StartHandler().Start(ref dalamud, this);
    public void Dispose() => PluginLink.QuitHandler.Quit();
}
