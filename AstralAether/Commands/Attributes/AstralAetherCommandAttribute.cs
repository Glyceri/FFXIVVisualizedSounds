using System;

namespace AstralAether.Commands.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AstralAetherCommandAttribute : Attribute
{
    public string command = string.Empty;
    public string description = string.Empty;
    public bool showInHelp = true;
    public string[] extraCommands = new string[0];

    public AstralAetherCommandAttribute(string command, string description, bool showInHelp)
    {
        this.command = command;
        this.description = description;
        this.showInHelp = showInHelp;
    }

    public AstralAetherCommandAttribute(string command, string description, bool showInHelp, params string[] extraCommands)
    {
        this.command = command;
        this.description = description;
        this.showInHelp = showInHelp;
        this.extraCommands = extraCommands;
    }
}
