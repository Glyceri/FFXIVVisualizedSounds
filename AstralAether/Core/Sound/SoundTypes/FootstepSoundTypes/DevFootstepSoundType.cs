using AstralAether.Core.Sound.SoundTypes.BaseTypes;
namespace AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;

internal class DevFootstepSoundType : BaseFootstepSoundType
{
    public int InternalType { get; private set; } = -1;

    public DevFootstepSoundType(string path, ref string[] splitPath, string mainIdentifier) : base(path, mainIdentifier)
    {
        string[] splittedInteresting = splitPath[3].Split("_");
        InternalType = int.Parse(splittedInteresting[0]);
    }

    public new string ToString() => $"{InternalType}";
}