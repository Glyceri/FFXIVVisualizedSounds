using AstralAether.Core.Sound.Attributes;
using AstralAether.Core.Sound.SoundTypes;
using AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;

namespace AstralAether.Core.Sound.Parsers;

[SoundParser("foot")]
internal class FootstepSoundParser : SoundParserElement
{
    public override SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        if (splitPath.Length < 4) return new InvalidSoundType(fullPath);

        string identifier = splitPath[2];

        if (identifier == "foot") return new FootstepSoundType(fullPath, ref splitPath, mainIdentifier);
        if (identifier == "dev") return new DevFootstepSoundType(fullPath, ref splitPath, mainIdentifier);

        return new UnimplementedSoundType(fullPath, mainIdentifier);
    }
}
