using AstralAether.Core.Sound.Attributes;
using AstralAether.Core.Sound.SoundTypes;
using AstralAether.Core.Sound.SoundTypes.BaseTypes;
using AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;

namespace AstralAether.Core.Sound.Parsers;

[SoundParser("battle")]
internal class BattleSoundParser : SoundParserElement
{
    public override SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        if (splitPath.Length < 4) return new InvalidSoundType(fullPath);

        string identifier = splitPath[2];

        if (identifier == "mon") return ParseMon(fullPath, ref splitPath, mainIdentifier); 

        return new BaseBattleSoundType(fullPath, mainIdentifier);
    }

    SoundType ParseMon(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        string[] dashSplit = splitPath[3].Split('_');
        if (dashSplit.Length == 1) return new BattleMonFootstepSoundtype(fullPath, ref splitPath, mainIdentifier);
        else if (dashSplit.Length == 2) return new BattleMonSoundType(fullPath, ref splitPath, mainIdentifier);

        return new BaseBattleSoundType(fullPath, mainIdentifier);
    }
}
