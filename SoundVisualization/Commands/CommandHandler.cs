using Dalamud.Game.Command;
using SoundVisualization.Commands.Attributes;
using SoundVisualization.Core.AutoRegistry;
using SoundVisualization.Core.Handlers;
using System.Collections.Generic;
using System.Reflection;

namespace SoundVisualization.Commands;

internal class CommandHandler : RegistryBase<SoundVisualizationCommand, AstralAetherCommandAttribute>
{
    internal List<SoundVisualizationCommand> commands => elements;

    protected override void OnElementCreation(SoundVisualizationCommand element)
    {
        AstralAetherCommandAttribute attribute = element.GetType().GetCustomAttribute<AstralAetherCommandAttribute>()!;
        PluginHandlers.CommandManager.AddHandler(attribute.command, new CommandInfo(element.OnCommand) { HelpMessage = attribute.description, ShowInHelp = attribute.showInHelp });
        foreach(string extraCommand in attribute.extraCommands)
            PluginHandlers.CommandManager.AddHandler(extraCommand, new CommandInfo(element.OnCommand) { HelpMessage = attribute.description, ShowInHelp = false });
    }

    protected override void OnElementDestroyed(SoundVisualizationCommand element)
    {
        AstralAetherCommandAttribute attribute = element.GetType().GetCustomAttribute<AstralAetherCommandAttribute>()!;
        PluginHandlers.CommandManager.RemoveHandler(attribute.command);
        foreach (string extraCommand in attribute.extraCommands)
            PluginHandlers.CommandManager.RemoveHandler(extraCommand);
    }

    public void ClearAllCommands() => ClearAllElements();
}
