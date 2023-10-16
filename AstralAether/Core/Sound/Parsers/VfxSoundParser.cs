using AstralAether.Core.Sound.Attributes;
using AstralAether.Core.Sound.SoundTypes;
using AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;

namespace AstralAether.Core.Sound.Parsers;

[SoundParser("vfx")]
internal class VfxSoundParser : SoundParserElement
{
    public override SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        if (fullPath.EndsWith("SE_VFX_common")) return new VFXCommonSoundType(fullPath, ref splitPath, mainIdentifier);

        return new UnimplementedSoundType(fullPath, mainIdentifier);
    }
}
