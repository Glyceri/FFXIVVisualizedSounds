using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Text;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;

namespace AstralAether.Core.Hooking;

//Shamelessly stolen from: https://github.com/MidoriKami/KamiLib/blob/master/Hooking/Delegates.cs

public static unsafe class Delegates
{
    public delegate nint AddonOnSetup(AtkUnitBase* addon, int valueCount, AtkValue* values);
    public delegate void AddonDraw(AtkUnitBase* addon);
    public delegate byte AddonOnRefresh(AtkUnitBase* addon, int valueCount, AtkValue* values);
    public delegate void AddonFinalize(AtkUnitBase* addon);
    public delegate byte AddonUpdate(AtkUnitBase* addon);
    public delegate void EventReceive(AtkEventListener* addon, AtkEventType eventType, uint eventParam, AtkEvent* eventObj, void* eventData);

    public delegate void AddonOpen(AtkUnitBase* addon);

    public delegate void AddToScreenLogWithLogMessageId(Character* target, Character* source, uint spell, char a4, int a5, int a6, int a7, int a8);
    public delegate IntPtr SomethingSomethingFlyText(IntPtr a1, IntPtr a2, uint a3, float a4, char a5);
    public delegate void SomethingSomethingFlyText2(IntPtr a1, uint a2, uint a3, IntPtr a4, int a5, int a6, IntPtr a7, int a8, int a9, int a10);

    public delegate void SomethingSomethingFlyText3(float * a1, IntPtr a2, IntPtr a3, IntPtr a4);

    public delegate void AgentShow(AgentInterface* agent);
    public delegate nint AgentReceiveEvent(AgentInterface* agent, nint rawData, AtkValue* args, uint argCount, ulong sender);
}
