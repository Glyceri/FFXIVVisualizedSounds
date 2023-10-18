namespace SoundVisualization.Core.Sound;

public abstract class SoundType
{
    public string Path { get; protected set; } = string.Empty;
    public string MainIdentifier { get; protected set; } = string.Empty;

    public SoundType(string path)
    {
        Path = path;
    }

    public SoundType(string path, string mainIdentifier)
    {
        Path = path;
        MainIdentifier = mainIdentifier;
    }

    public virtual new string ToString() => string.Format("{0,-10} {1,-60}", $"[{MainIdentifier.ToUpperInvariant()}]", $"[{Path}]");
}
