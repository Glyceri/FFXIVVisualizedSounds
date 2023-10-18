using SoundVisualization.Core.AutoRegistry;
using SoundVisualization.Core.Sound.Attributes;
using SoundVisualization.Core.Sound.SoundTypes;
using System.Collections.Generic;

namespace SoundVisualization.Core.Sound;

internal class ParserHandler : RegistryBase<SoundParserElement, SoundParserAttribute>
{
    Dictionary<string, SoundParserElement> soundParsers = new Dictionary<string, SoundParserElement>();

    protected override void OnElementCreation(SoundParserElement element, SoundParserAttribute attribute) => soundParsers.Add(attribute.Identifier, element);
    protected override void OnDipose() => soundParsers.Clear();

    public SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        if (soundParsers.ContainsKey(mainIdentifier)) return soundParsers[mainIdentifier].Parse(fullPath, ref splitPath, mainIdentifier);
        return new UnparsedSoundType(fullPath, mainIdentifier);
    }
}