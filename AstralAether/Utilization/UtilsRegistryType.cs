using AstralAether.Core.AutoRegistry.Interfaces;
using AstralAether.Core.Handlers;
using System;

namespace AstralAether.Utilization;

internal class UtilsRegistryType : IRegistryElement, IDisposable
{
    protected UtilsHandler Utils => PluginLink.Utils;

    internal virtual void OnRegistered() { }
    internal virtual void OnLateRegistered() { }

    internal virtual void Dispose() { }

    void IDisposable.Dispose() => Dispose();
    
}
