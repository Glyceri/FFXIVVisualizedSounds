using Dalamud.Interface.Windowing;
using AstralAether.Core.AutoRegistry;
using AstralAether.Core.Handlers;
using AstralAether.Windows.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AstralAether.Windows.Handler;

internal class WindowsHandler : RegistryBase<AstralAetherWindow, PersistentAstralAetherWindowAttribute>
{
    WindowSystem windowSystem = new WindowSystem("Astral Aether");
    public WindowSystem WindowSystem { get => windowSystem; }

    List<AstralAetherWindow> astralAetherWindows => elements;
    List<TemporaryAstralAetherWindow> temporaryWindows = new List<TemporaryAstralAetherWindow>();

    public WindowsHandler() : base()
    {
        PluginHandlers.PluginInterface.UiBuilder.Draw += Draw;
    }

    protected override void OnDipose()
    {
        windowSystem.RemoveAllWindows();
        PluginHandlers.PluginInterface.UiBuilder.Draw -= Draw;
    }

    public AstralAetherWindow GetWindow(Type windowType) => GetElement(windowType);
    public T GetWindow<T>() where T : AstralAetherWindow => (T)GetWindow(typeof(T));

    protected override void OnElementCreation(AstralAetherWindow element)
    {
        windowSystem.AddWindow(element);

        Type t = element.GetType();

        if (t.GetCustomAttribute<ModeToggleAstralAetherWindowAttribute>() != null)
            t.GetField("drawToggle", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(element, true);

        if (t.GetCustomAttribute<MainAstralAetherWindowAttribute>() != null)
            PluginHandlers.PluginInterface.UiBuilder.OpenMainUi += () => PluginLink.WindowHandler.ToggleWindow(element.GetType());

        if (t.GetCustomAttribute<ConfigAstralAetherWindowAttribute>() != null)
            PluginHandlers.PluginInterface.UiBuilder.OpenConfigUi += () => PluginLink.WindowHandler.ToggleWindow(element.GetType());
    }

    public T AddTemporaryWindow<T>(string message, Action<object> callback, Window blackenedWindow = null!) where T : TemporaryAstralAetherWindow
    {
        TemporaryAstralAetherWindow petWindow = (Activator.CreateInstance(typeof(T), new object[3] { message, callback, blackenedWindow }) as TemporaryAstralAetherWindow)!;
        temporaryWindows.Add(petWindow);
        windowSystem.AddWindow(petWindow);
        return (T)petWindow;
    }

    public void ToggleWindow<T>() where T : AstralAetherWindow => GetWindow<T>().IsOpen = !GetWindow<T>().IsOpen;
    public void CloseWindow<T>() where T : AstralAetherWindow => GetWindow<T>().IsOpen = false;
    public void OpenWindow<T>() where T : AstralAetherWindow => GetWindow<T>().IsOpen = true;

    public void ToggleWindow(Type windowType) => GetWindow(windowType).IsOpen = !GetWindow(windowType).IsOpen;
    public void CloseWindow(Type windowType) => GetWindow(windowType).IsOpen = false;
    public void OpenWindow(Type windowType) => GetWindow(windowType).IsOpen = true;

    public void CloseAllWindows()
    {
        foreach (AstralAetherWindow window in astralAetherWindows)
            window.IsOpen = false;
    }

    public void Draw()
    {
        windowSystem.Draw();
        if (PluginHandlers.ClientState.LocalPlayer! == null)
        {
            CloseAllWindows();
            return;
        }
        for (int i = temporaryWindows.Count - 1; i >= 0; i--)
            if (temporaryWindows[i].closed)
            {
                windowSystem.RemoveWindow(temporaryWindows[i]);
                temporaryWindows.RemoveAt(i);
            }

        
    }

    bool initialized = false;

    internal void Initialize()
    {
        if (initialized) return;
        initialized = true;

        foreach (AstralAetherWindow element in elements)
            if (element is InitializableAstralAetherWindow initWindow)
                initWindow.OnInitialized();
    }
}
