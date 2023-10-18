using SoundVisualization.Core.AutoRegistry;
using SoundVisualization.Core.Chat.Attributes;
using SoundVisualization.Core.Handlers;

namespace SoundVisualization.Core.Chat;

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
