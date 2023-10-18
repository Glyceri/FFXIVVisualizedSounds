using System;

namespace SoundVisualization.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SoundVisualizationCommandAttribute : Attribute
{
    public string command = string.Empty;
    public string description = string.Empty;
    public bool showInHelp = true;
    public string[] extraCommands = new string[0];

    public SoundVisualizationCommandAttribute(string command, string description, bool showInHelp)
    {
        this.command = command;
        this.description = description;
        this.showInHelp = showInHelp;
    }

    public SoundVisualizationCommandAttribute(string command, string description, bool showInHelp, params string[] extraCommands)
    {
        this.command = command;
        this.description = description;
        this.showInHelp = showInHelp;
        this.extraCommands = extraCommands;
    }
}
