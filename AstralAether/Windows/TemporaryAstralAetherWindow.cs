using ImGuiNET;
using System;

namespace AstralAether.Windows;

public abstract class TemporaryAstralAetherWindow : AstralAetherWindow, IDisposable
{
    Action<object> callback { get; set; } = null!;
    public bool closed { get; private set; } = false;

    protected TemporaryAstralAetherWindow(string name, Action<object> callback, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow)
    {
        this.callback = callback; 
    }

    public void DoCallback(object data)
    {
        callback?.Invoke(data);
        Close();
    }
    public new void Dispose() { closed = true; OnDispose(); }
    public void Close() => Dispose();
    protected override void OnDispose() { }

    public unsafe override void OnDraw() { }
}
