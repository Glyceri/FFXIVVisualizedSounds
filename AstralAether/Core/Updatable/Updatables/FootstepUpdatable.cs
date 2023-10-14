using AstralAether.Core.Handlers;
using AstralAether.Windows.Attributes;
using AstralAether.Windows.Windows;
using Dalamud.Plugin.Services;

namespace AstralAether.Core.Updatable.Updatables;

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
        for (int i = audioWindow.footSteps.Count - 1; i >= 0; i--)
        {
            if ((audioWindow.footSteps[i].timer -= frameWork.UpdateDelta.TotalSeconds) <= 0)
            {
                audioWindow.footSteps.RemoveAt(i);
            }
        }
    }
}
