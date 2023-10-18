using SoundVisualization.Core.Sound.Attributes;
using SoundVisualization.Core.Sound.SoundTypes;
using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;
using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes.BaseTypes;

namespace SoundVisualization.Core.Sound.Parsers;

[SoundParser("voice")]
internal class VoiceSoundParser : SoundParserElement
{
    public override SoundType Parse(string fullPath, ref string[] splitPath, string mainIdentifier)
    {
        int length = splitPath.Length;
        if (length < 3) return new InvalidSoundType(fullPath);

        if (splitPath[2] == "Vo_Emote" && length > 3) return new VoEmoteSoundType(fullPath, ref splitPath, mainIdentifier);

        return new BaseVoiceType(fullPath, mainIdentifier);
    }
}
