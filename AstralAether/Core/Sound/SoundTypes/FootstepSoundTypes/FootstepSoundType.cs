using AstralAether.Core.Sound.SoundTypes.BaseTypes;
using AstralAether.Utilization.UtilsModule;

namespace AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;

internal class FootstepSoundType : BaseFootstepSoundType
{
    public string InternalType { get; private set; } = string.Empty;
    public string FootstepType { get; private set; } = string.Empty;
    public Gender Gender { get; private set; } = Gender.Male;
    public string Unk1 { get; private set; } = string.Empty;
    public string FootWearType { get; private set; } = string.Empty;

    public FootstepSoundType(string path, ref string[] splitPath, string mainIdentifier) : base(path, mainIdentifier)
    {
        string[] splittedInteresting = splitPath[3].Split("_");

        InternalType = splittedInteresting[0];
        FootstepType = splittedInteresting[1];
        Gender = splittedInteresting[2] == "f" ? Gender.Female : Gender.Male;
        Unk1 = splittedInteresting[3];
        FootWearType = splittedInteresting[4];
    }

    public override string ToString() => base.ToString() + $" {InternalType}_{FootstepType}_{Gender}_{Unk1}_{FootWearType}";
}
