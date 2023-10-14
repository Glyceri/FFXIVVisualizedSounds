using AstralAether.Core.AutoRegistry;
using AstralAether.Core.Chat.Attributes;
using AstralAether.Core.Handlers;

namespace AstralAether.Core.Chat;

internal class ChatHandler : RegistryBase<ChatElement, AstralAetherChatAttribute>
{ 
    protected override void OnElementCreation(ChatElement element)
    {
        PluginHandlers.ChatGui.ChatMessage += element.OnChatMessage;
    }

    protected override void OnElementDestroyed(ChatElement element)
    {
        PluginHandlers.ChatGui.ChatMessage -= element.OnChatMessage;
    }
}
