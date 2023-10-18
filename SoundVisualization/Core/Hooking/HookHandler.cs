using SoundVisualization.Core.AutoRegistry;
using SoundVisualization.Core.Handlers;
using SoundVisualization.Core.Hooking.Attributes;
using Dalamud.Plugin.Services;

namespace SoundVisualization.Core.Hooking;

internal class HookHandler : RegistryBase<HookableElement, HookAttribute>
{
    protected override void OnElementCreation(HookableElement element)
    {
        PluginHandlers.Hooking.InitializeFromAttributes(element);
        element.OnInit();
    }

    public HookHandler()
    {
        PluginHandlers.Framework.Update += OnUpdate;
    }

    protected override void OnDipose()
    {
        PluginHandlers.Framework.Update -= OnUpdate;
    }

    protected void OnUpdate(IFramework framework)
    {
        foreach(HookableElement el in elements)
            el?.OnUpdate(framework);
    }
}
