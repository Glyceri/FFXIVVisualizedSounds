namespace AstralAether.Core.Sound.SoundTypes;

public class InvalidSoundType : SoundType
{
    public InvalidSoundType(string path) : base(path) { }

    public override string ToString() => "[INVALID] " + base.ToString();
}
