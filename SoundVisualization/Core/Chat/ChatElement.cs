using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using SoundVisualization.Core.AutoRegistry.Interfaces;

namespace SoundVisualization.Core.Chat;

internal abstract class ChatElement : IRegistryElement
{
    internal abstract void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled);
}
