namespace AstralAether.Core.Sound.SoundTypes;

public class UnparsedSoundType : SoundType
{
    public UnparsedSoundType(string path) : base(path) =>  MainIdentifier = string.Empty;
    public UnparsedSoundType(string path, string mainIdentifier) : base(path, mainIdentifier) { }

    public override string ToString() => "[Unparsed] " + base.ToString();
}
