using AstralAether.Core.AutoRegistry.Interfaces;

namespace AstralAether.Core.Sound;

public abstract class SoundParserElement : IRegistryElement
{
    public abstract SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier);
}
