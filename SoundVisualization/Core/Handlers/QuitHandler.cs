namespace SoundVisualization.Core.Handlers;

internal class QuitHandler
{
    public void Quit()
    {
        PluginLink.WindowHandler?.Dispose();
        PluginLink.CommandHandler?.Dispose();
        PluginLink.UpdatableHandler?.Dispose();
        PluginLink.ChatHandler?.Dispose();
        PluginLink.HookHandler?.Dispose();
        PluginLink.Utils?.Dispose();
        PluginLink.ParserHandler?.Dispose();    
    }
}
