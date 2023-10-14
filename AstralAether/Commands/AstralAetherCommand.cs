using AstralAether.Core.AutoRegistry.Interfaces;

namespace AstralAether.Commands;

internal abstract class AstralAetherCommand : IRegistryElement
{
    internal abstract void OnCommand(string command, string args);
}
