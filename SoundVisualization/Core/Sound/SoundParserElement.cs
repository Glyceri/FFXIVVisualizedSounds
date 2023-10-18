using SoundVisualization.Core.AutoRegistry.Interfaces;

namespace SoundVisualization.Core.Sound;

public abstract class SoundParserElement : IRegistryElement
{
    public abstract SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier);
}
