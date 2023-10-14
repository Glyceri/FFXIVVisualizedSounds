using Dalamud.Plugin;
using AstralAether.Commands;
using AstralAether.Core.Chat;
using AstralAether.Core.Updatable;
using AstralAether.Windows.Handler;
using AstralAether.Core.Hooking;
using AstralAether.Utilization;

namespace AstralAether.Core.Handlers;

internal class PluginLink
{
    internal static Configuration Configuration { get; set; } = null!;
    internal static AstralAetherPlugin QuickChatPlugin { get; set; } = null!;
    internal static WindowsHandler WindowHandler { get; set; } = null!;
    internal static DalamudPluginInterface DalamudPlugin { get; set; } = null!;
    internal static CommandHandler CommandHandler { get; set; } = null!;
    internal static UpdatableHandler UpdatableHandler { get; set; } = null!;
    internal static HookHandler HookHandler { get; set; } = null!;
    internal static QuitHandler QuitHandler { get; set; } = null!;
    internal static ChatHandler ChatHandler { get; private set; } = null!;
    internal static UtilsHandler Utils { get; private set; } = null!;

    internal static void Start(ref DalamudPluginInterface dalamud, ref AstralAetherPlugin quickChatPlugin)
    {
        DalamudPlugin = dalamud;
        QuickChatPlugin = quickChatPlugin;
        Configuration = PluginHandlers.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize();
        WindowHandler = new WindowsHandler();
        CommandHandler = new CommandHandler();
        Utils = new UtilsHandler();
        ChatHandler = new ChatHandler();
        UpdatableHandler = new UpdatableHandler();
        HookHandler = new HookHandler();
        WindowHandler.Initialize();
        QuitHandler = new QuitHandler();
    }
}
