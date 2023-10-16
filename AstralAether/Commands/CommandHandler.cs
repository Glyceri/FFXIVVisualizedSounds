using Dalamud.Game.Command;
using AstralAether.Commands.Attributes;
using AstralAether.Core.AutoRegistry;
using AstralAether.Core.Handlers;
using System.Collections.Generic;
using System.Reflection;

namespace AstralAether.Commands;

internal class CommandHandler : RegistryBase<AstralAetherCommand, AstralAetherCommandAttribute>
{
    internal List<AstralAetherCommand> commands => elements;

    protected override void OnElementCreation(AstralAetherCommand element)
    {
        AstralAetherCommandAttribute attribute = element.GetType().GetCustomAttribute<AstralAetherCommandAttribute>()!;
        PluginHandlers.CommandManager.AddHandler(attribute.command, new CommandInfo(element.OnCommand) { HelpMessage = attribute.description, ShowInHelp = attribute.showInHelp });
        foreach(string extraCommand in attribute.extraCommands)
            PluginHandlers.CommandManager.AddHandler(extraCommand, new CommandInfo(element.OnCommand) { HelpMessage = attribute.description, ShowInHelp = false });
    }

    protected override void OnElementDestroyed(AstralAetherCommand element)
    {
        AstralAetherCommandAttribute attribute = element.GetType().GetCustomAttribute<AstralAetherCommandAttribute>()!;
        PluginHandlers.CommandManager.RemoveHandler(attribute.command);
        foreach (string extraCommand in attribute.extraCommands)
            PluginHandlers.CommandManager.RemoveHandler(extraCommand);
    }

    public void ClearAllCommands() => ClearAllElements();
}
