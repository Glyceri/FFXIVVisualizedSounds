using SoundVisualization.Core.AutoRegistry.Interfaces;

namespace SoundVisualization.Commands;

internal abstract class SoundVisualizationCommand : IRegistryElement
{
    internal abstract void OnCommand(string command, string args);
}
