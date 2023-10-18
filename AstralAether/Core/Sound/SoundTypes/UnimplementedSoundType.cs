namespace AstralAether.Core.Sound.SoundTypes;

public class UnimplementedSoundType : SoundType
{
    public UnimplementedSoundType(string path) : base(path) => MainIdentifier = string.Empty;
    public UnimplementedSoundType(string path, string mainIdentifier) : base(path, mainIdentifier) { }

    //public override string ToString() => "[Unimplemented] " + base.ToString();
}
