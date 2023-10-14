using Dalamud.Game;
using AstralAether.Core.AutoRegistry.Interfaces;
using Dalamud.Plugin.Services;

namespace AstralAether.Core.Hooking;

public unsafe class HookableElement : IDisposableRegistryElement
{
    public void Dispose() { OnDispose(); }

    internal virtual void OnDispose() { }

    internal virtual void OnInit() { }

    internal virtual void OnUpdate(IFramework framework) { }
}
