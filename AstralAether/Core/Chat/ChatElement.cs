using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using AstralAether.Core.AutoRegistry.Interfaces;

namespace AstralAether.Core.Chat;

internal abstract class ChatElement : IRegistryElement
{
    internal abstract void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled);
}
