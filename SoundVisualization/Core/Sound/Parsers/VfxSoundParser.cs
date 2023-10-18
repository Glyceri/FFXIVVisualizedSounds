using SoundVisualization.Core.Sound.Attributes;
using SoundVisualization.Core.Sound.SoundTypes;
using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;
using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes.BaseTypes;

namespace SoundVisualization.Core.Sound.Parsers;

[SoundParser("vfx")]
internal class VfxSoundParser : SoundParserElement
{
    public override SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        if (fullPath.EndsWith("SE_VFX_common")) return new VFXCommonSoundType(fullPath, ref splitPath, mainIdentifier);

        return new BaseVFXSoundType(fullPath, mainIdentifier);
    }
}
