using SoundVisualization.Core.Sound.SoundTypes.BaseTypes;

namespace SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;

internal class SmallFootstepSound : BaseFootstepSoundType
{
    public string InternalType { get; private set; } = string.Empty;
    public string MeshType { get; private set; } = string.Empty;
    public string FootstepType { get; private set; } = string.Empty;

    public SmallFootstepSound(string path, ref string[] splitPath, string mainIdentifier) : base(path, mainIdentifier)
    {
        string[] splittedInteresting = splitPath[3].Split("_");

        InternalType = splittedInteresting[0];
        MeshType = splittedInteresting[1];
        FootstepType = splittedInteresting[2];
    }
}
