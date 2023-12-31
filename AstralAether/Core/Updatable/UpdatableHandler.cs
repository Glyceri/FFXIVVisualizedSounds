using Dalamud.Game;
using AstralAether.Core.AutoRegistry;
using AstralAether.Core.Handlers;
using AstralAether.Windows.Attributes;
using System.Collections.Generic;
using System.Reflection;
using Dalamud.Plugin.Services;

namespace AstralAether.Core.Updatable
{
    internal class UpdatableHandler : RegistryBase<Updatable, UpdatableAttribute>
    {
        List<Updatable> updatables => elements;

        public unsafe UpdatableHandler() 
        {
            PluginHandlers.Framework.Update += MainUpdate;
        }

        protected override void OnDipose()
        {
            PluginHandlers.Framework.Update -= MainUpdate;
        }

        protected override void OnAllRegistered() => updatables?.Sort(Compare);
        

        int Compare(Updatable a, Updatable b)
        {
            int aVal = a.GetType().GetCustomAttribute<UpdatableAttribute>()?.order ?? int.MaxValue;
            int bVal = b.GetType().GetCustomAttribute<UpdatableAttribute>()?.order ?? int.MaxValue;
            return aVal.CompareTo(bVal);
        }

        public void ClearAllUpdatables() => ClearAllElements();

        void MainUpdate(IFramework framework)
        {
            if (!(PluginHandlers.ClientState is { LocalPlayer: { } player })) return;

            foreach (Updatable updatable in updatables)
                updatable?.Update(framework);
        }
    }
}
