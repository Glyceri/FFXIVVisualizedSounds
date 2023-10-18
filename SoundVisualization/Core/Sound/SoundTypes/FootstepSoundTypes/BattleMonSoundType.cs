using SoundVisualization.Core.Sound.SoundTypes.BaseTypes;

namespace SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;

internal class BattleMonSoundType : BaseBattleSoundType
{
    public int InternalType { get; private set; } = -1;
    public string InternalType2 { get; private set; } = string.Empty;
    public BattleMonSoundType(string path, ref string[] splitPath, string mainIdentifier) : base(path, mainIdentifier)
    {
        string[] splittedInteresting = splitPath[3].Split("_");
        InternalType = int.Parse(splittedInteresting[0]);
        InternalType2 = splittedInteresting[1];
    }

    //public new string ToString() => $"{InternalType}_{InternalType2}";
}
