using Dalamud.Configuration;
using Dalamud.Plugin;
using AstralAether.Core.Handlers;
using System;

namespace AstralAether;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public void Initialize()
    {
        
    }

    public void Save()
    {
        PluginLink.DalamudPlugin!.SavePluginConfig(this);
    }
}
