using SoundVisualization.Core.Handlers;
using SoundVisualization.Core.Sound.SoundTypes;

namespace SoundVisualization.Core.Sound;

public static class SoundParser
{
    // Why store this and not type it? For some reason, a japanese path slash is different. I've tested this, ran into this issue before.
    // I don't know why, it's real, and it's messed this project up.
    public const char SlashSymbol = '/';

    public static SoundType ParseSound(string path)
    {
        if (!IsSoundValid(path)) return new InvalidSoundType(path);
        path = path.Replace(".scd", string.Empty);
        string[] splitPath = SplitPath(path);
        string MainIdentifier = GetMainIdentifier(ref splitPath);
        if (MainIdentifier == string.Empty) return new UnparsedSoundType(path);
        return ParsePath(path, ref splitPath, MainIdentifier); 
    }

    static SoundType ParsePath(string basePath, ref string[] Path, string MainIdentifier) => PluginLink.ParserHandler.Parse(basePath, ref Path, MainIdentifier);

    static string[] SplitPath(string path) => path.Split(SlashSymbol);

    static string GetMainIdentifier(ref string[] path)
    {
        if (path.Length < 2) return string.Empty;
        return path[1];
    }

    static bool IsSoundValid(string path)
    {
        if (path == null) return false;
        if (path.Length == 0) return false;
        if (!path.StartsWith($"sound{SlashSymbol}")) return false;
        if (!path.EndsWith(".scd")) return false;

        return true;
    }
}
