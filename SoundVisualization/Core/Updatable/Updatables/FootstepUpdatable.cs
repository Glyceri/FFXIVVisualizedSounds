using SoundVisualization.Core.Handlers;
using SoundVisualization.Windows.Attributes;
using SoundVisualization.Windows.AudioModules.Base;
using SoundVisualization.Windows.Windows;
using Dalamud.Plugin.Services;

namespace SoundVisualization.Core.Updatable.Updatables;

[Updatable]
internal class FootstepUpdatable : Updatable
{
    AudioWindow audioWindow;
    public FootstepUpdatable()
    {
        audioWindow = PluginLink.WindowHandler.GetWindow<AudioWindow>();
    }

    public override void Update(IFramework frameWork)
    {
        audioWindow.IsOpen = true;
        for (int i = audioWindow.audioModules.Count - 1; i >= 0; i--)
        {
            AudioModule module = audioWindow.audioModules[i];
            if (module.Tick(frameWork.UpdateDelta.TotalSeconds))
                audioWindow.audioModules.RemoveAt(i);
        }
    }
}
