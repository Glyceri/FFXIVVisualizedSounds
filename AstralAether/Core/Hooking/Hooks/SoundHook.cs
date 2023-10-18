using AstralAether.Core.Handlers;
using AstralAether.Core.Hooking.Attributes;
using AstralAether.Core.Sound;
using AstralAether.Windows.AudioModules;
using AstralAether.Windows.Windows;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace AstralAether.Core.Hooking.Hooks;

[Hook]
internal unsafe class SoundHook : HookableElement
{
    public const string ResourceManager = "48 8B 05 ?? ?? ?? ?? 33 ED F0";

    // ====== FILES HOOKS ========

    [Signature("48 89 6C 24 ?? 41 54 41 56 41 57 48 81 EC", DetourName = nameof(ApricotListenerSoundPlayDetour))]
    readonly Hook<ApricotListenerSoundPlayDelegate>? apricotListenerSoundPlayHook = null!;



    private delegate nint ApricotListenerSoundPlayDelegate(nint a1, nint a2, nint a3, nint a4, nint a5, nint a6);
    private delegate void* PlaySpecificSoundDelegate(long a1, int idx);

    // E8 ? ? ? ? 48 89 43 08 48 83 C4 40 
    // Voice line gets created there

    //E8 ? ? ? ? EB 16 44 8B 4B 10 
    // wonder what that sound is?

    [Signature("40 53 48 83 EC 20 44 8B 41 50 ", DetourName = nameof(GetTargetDetour))]
    readonly Hook<GetTarget>? apricotGetTarget;
    delegate IntPtr GetTarget(IntPtr a1, uint a2);
    IntPtr GetTargetDetour(IntPtr a1, uint a2)
    {
        GameObject* target = (GameObject*)apricotGetTarget!.Original(a1, a2);
        SoundStorage.RegisterLastAccessedGameObject(target);
        if (target != null)
        {
            PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.05, new Vector4(1, 0, 0, 1f), target->Position, 200, 5, 4f));
        }
        return (IntPtr)target;
    }


    [Signature("40 53 48 83 EC 20 8B 51 50 ", DetourName = nameof(GetCaster))]
    readonly Hook<GetCasterDelegate>? apricotGetCaster;
    delegate IntPtr GetCasterDelegate(IntPtr a1);
    IntPtr GetCaster(IntPtr a1)
    {
        
        GameObject* caster = (GameObject*)apricotGetCaster!.Original(a1);
        SoundStorage.RegisterLastAccessedGameObject(caster);
        if (caster != null)
        {
            PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.05, new Vector4(1, 1, 1, 1f), caster->Position, 200, 5, 4f));
        }
        return (IntPtr)caster;
    }

    GameObject* lastScheduledGameobject;
    [Signature(" E8 ?? ?? ?? ?? 48 85 C0 74 11 48 8D 88 10 02 00 00 ")]
    readonly Hook<GameObjectFromScheduler>? gameObjectFromSchedulerHook;
    delegate GameObject* GameObjectFromScheduler(IntPtr scheduler);
    GameObject* OnGameObjectFromScheduler(IntPtr scheduler)
    {
        lastScheduledGameobject = gameObjectFromSchedulerHook!.Original(scheduler);
        SoundStorage.RegisterLastAccessedGameObject(lastScheduledGameobject);
        return lastScheduledGameobject;
    }

    // Other cutscene hook
    // 40 53 55 56 57 41 54 41 55 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 63 AC 24 ?? ?? ?? ?? 
    [Signature("E8 ?? ?? ?? ?? 48 83 C8 FF 0F 1F 40 00 ", DetourName = nameof(OnCutsceneVoiceDetour))]
    readonly Hook<OnCutsceneVoice>? onCutsceneVoiceHook;
    delegate int OnCutsceneVoice(IntPtr* filePath, int a2, char* a3, int a4, int a5);
    int OnCutsceneVoiceDetour(IntPtr* filePath, int a2, char* a3, int a4, int a5)
    {
        // this means the line is voiced
        return onCutsceneVoiceHook!.Original(filePath, a2, a3, a4, a5);
    }


    GameObject* lastPosGotten;

    [Signature("E8 ?? ?? ?? ?? 49 63 F4 ")]
    readonly Hook<GetPositionDelegate>? getPositionHook;
    private delegate IntPtr GetPositionDelegate(GameObject* gObject);
    public IntPtr OnGetPostion(GameObject* gObject)
    {

        lastPosGotten = gObject;
        if (lastPosGotten != null)
        {
           // SoundStorage.LastAccessedGameObject = lastPosGotten;
        }
        return getPositionHook!.Original(gObject);
    }

    [Signature("E8 ?? ?? ?? ?? 4C 8B 74 24 ?? 48 8B 74 24 ?? 4C 8B 7C 24 ?? ")]
    readonly Hook<Instrument>? instrumentHook;
    private delegate void Instrument(IntPtr* a1, IntPtr a2, IntPtr a3, int a4);
    public void InstrumentDetour(IntPtr* a1, IntPtr a2, IntPtr a3, int a4)
    {

        PluginLog.Log("Instrument! " + Marshal.PtrToStringUTF8((IntPtr)((GameObject*)a2)->Name));
        instrumentHook!.Original(a1, a2, a3, a4);
    }

    internal override void OnInit()
    {
        loadCharacterSoundHook =
            PluginHandlers.Hooking.HookFromAddress<LoadCharacterSound>(
                (nint)VfxContainer.MemberFunctionPointers.LoadCharacterSound,
                LoadCharacterSoundDetour);

        apricotListenerSoundPlayHook?.Enable();
        loadCharacterSoundHook?.Enable();
        alternativeSoundPlay?.Enable();
        streamSoundPlayerHook?.Enable();
        voiceDelegateHook?.Enable();
        instrumentHook?.Enable();
        apricotGetCaster?.Enable();
        apricotGetTarget?.Enable();
        gameObjectFromSchedulerHook?.Enable();
        getPositionHook?.Enable();
        onCutsceneVoiceHook?.Enable();
    }

    internal override void OnDispose()
    {
        apricotListenerSoundPlayHook?.Dispose();
        loadCharacterSoundHook?.Dispose();
        alternativeSoundPlay?.Dispose();
        streamSoundPlayerHook?.Dispose();
        voiceDelegateHook?.Dispose();
        instrumentHook?.Dispose();
        apricotGetCaster?.Dispose();
        apricotGetTarget?.Dispose();
        gameObjectFromSchedulerHook?.Dispose();
        getPositionHook?.Dispose();
        onCutsceneVoiceHook?.Dispose();
    }

    [Signature("40 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 8D 42 FF 48 8B F9 3D ?? ?? ?? ?? ", DetourName = nameof(SomethingDetour))]
    readonly Hook<somethingVoiceHookDelegate>? voiceDelegateHook;
    private delegate IntPtr somethingVoiceHookDelegate(IntPtr a1, IntPtr a2);
    public IntPtr SomethingDetour(IntPtr a1, IntPtr a2)
    {
        PluginLog.Log("Something: Vo_CM/Vo_cm_PC");
        return voiceDelegateHook!.Original(a1, a2);
    }

    [Signature("E8 ?? ?? ?? ?? 48 89 47 18 89 5F 20 ", DetourName = nameof(StreamSoundPlayerDetour))]
    readonly Hook<StreamSoundPlayer>? streamSoundPlayerHook;
    delegate IntPtr StreamSoundPlayer(IntPtr a1, IntPtr a2, float a3, uint a4);
    IntPtr StreamSoundPlayerDetour(IntPtr a1, IntPtr a2, float a3, uint a4)
    {
        //PluginLog.LogWarning($"Stream sound: " + Marshal.PtrToStringUTF8(a2));
        return streamSoundPlayerHook!.Original(a1, a2, a3, a4);
    }

    [Signature("E8 ?? ?? ?? ?? 8B 7D 77 85 FF ", DetourName = nameof(UISoundPlayDetour))]
    readonly Hook<AlternativeSoundPlay>? alternativeSoundPlay;
    private delegate IntPtr AlternativeSoundPlay(IntPtr a1, IntPtr fileName, float a3, int a4, int a5, byte a6, uint a7);
    IntPtr UISoundPlayDetour(IntPtr a1, IntPtr fileName, float a3, int a4, int a5, byte a6, uint a7)
    {
        PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.3, new Vector4(0, 0, 0, 0.5f), ImGui.GetMousePos(), 200, 5, 4f));

        return alternativeSoundPlay!.Original(a1, fileName, a3, a4, a5, a6, a7);
    }

    private unsafe nint ApricotListenerSoundPlayDetour(nint a1, nint a2, nint a3, nint a4, nint a5, nint a6)
    {
        if (a6 == nint.Zero) return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);

        try
        {
            GameObject* gameObject = (*(delegate* unmanaged<nint, GameObject*>**)a6)[1](a6);
            if (gameObject != null) SoundStorage.RegisterLastAccessedGameObject(gameObject);
        }
        catch { }
        return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);
    }

    /// <summary> Characters load some of their voice lines or whatever with this function. </summary>
    private delegate nint LoadCharacterSound(VfxContainer* character, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7);
    private Hook<LoadCharacterSound>? loadCharacterSoundHook;
    private nint LoadCharacterSoundDetour(VfxContainer* container, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7)
    {
        SoundStorage.RegisterLastAccessedGameObject((GameObject*)container->OwnerObject);
        return loadCharacterSoundHook.Original(container, unk1, unk2, unk3, unk4, unk5, unk6, unk7);
    }
}
