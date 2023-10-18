using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes.BaseTypes;

namespace SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;

internal class VoEmoteSoundType : BaseVoiceType
{
    public int Emote { get; private set; }

    public VoEmoteSoundType(string fullPath, ref string[] splitPath, string mainIdentifier) : base(fullPath, mainIdentifier)
    {
        try
        {
            Emote = int.Parse(splitPath[3]);
        }
        catch { }
    }
}
