using SoundVisualization.Core.AutoRegistry.Interfaces;
using SoundVisualization.Core.Handlers;
using System;

namespace SoundVisualization.Utilization;

internal class UtilsRegistryType : IRegistryElement, IDisposable
{
    protected UtilsHandler Utils => PluginLink.Utils;

    internal virtual void OnRegistered() { }
    internal virtual void OnLateRegistered() { }

    internal virtual void Dispose() { }

    void IDisposable.Dispose() => Dispose();
    
}
