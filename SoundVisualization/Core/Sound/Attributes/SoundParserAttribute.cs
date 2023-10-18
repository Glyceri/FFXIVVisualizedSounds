using System;

namespace SoundVisualization.Core.Sound.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SoundParserAttribute : Attribute
{
    public string Identifier { get; private set; } = string.Empty;
    public SoundParserAttribute(string identifier) => Identifier = identifier;
}
