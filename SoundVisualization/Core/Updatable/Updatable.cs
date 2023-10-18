using Dalamud.Game;
using SoundVisualization.Core.AutoRegistry.Interfaces;
using Dalamud.Plugin.Services;

namespace SoundVisualization.Core.Updatable
{
    internal abstract class Updatable : IDisposableRegistryElement
    {
        public void Dispose() => OnDispose();
        protected virtual void OnDispose() { }
        public abstract unsafe void Update(IFramework frameWork);
    }
}
