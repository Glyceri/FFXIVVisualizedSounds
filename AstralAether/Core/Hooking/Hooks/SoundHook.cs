using AstralAether.Core.Handlers;
using AstralAether.Core.Hooking.Attributes;
using AstralAether.Core.Sound;
using AstralAether.Utilization.UtilsModule;
using AstralAether.Windows.AudioModules;
using AstralAether.Windows.Windows;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
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



    private Hook<LoadCharacterSound> _loadCharacterSoundHook;

    

    private delegate IntPtr WhatTheFrick(IntPtr a1);

    private delegate IntPtr BattleSoundHook1(IntPtr a1, float x, float y, float z, uint soundType, int a6, uint a7, uint a8, int a9, uint a10, byte a11, uint a12, char a13);

    bool enabled = true;

    // E8 ? ? ? ? 48 89 43 08 48 83 C4 40 
    // Voice line gets created there

    //E8 ? ? ? ? EB 16 44 8B 4B 10 
    // wonder what that sound is?

    [Signature("E8 ?? ?? ?? ?? EB 16 44 8B 4B 10 ")]
    readonly Hook<PossibleVFXSoundOrigin>? vfxSoundOrigin;

    [Signature("40 53 55 56 57 41 56 48 81 EC ?? ?? ?? ?? 48 8B F9 "//, DetourName = nameof(VfxOwODetour)
                                                                    )]
    readonly Hook<VfxOwO>? vfxOwo;

    [Signature("E8 ?? ?? ?? ?? 48 8B 7C 24 ?? 48 8B 74 24 ?? 48 89 85 ?? ?? ?? ?? ")]
    readonly Hook<OnGodForReal>? onGod;

    [Signature("E8 ?? ?? ?? ?? EB B7 0F 28 D6")]
    readonly Hook<AlternativeTo>? alternative;

    [Signature("E8 ?? ?? ?? ?? 48 63 87 ?? ?? ?? ?? 48 83 BC C7 ?? ?? ?? ?? ?? ", DetourName = nameof(AltEndingDetour))]
    readonly Hook<AltEnding>? altEnding;



    [Signature("E8 ?? ?? ?? ?? 4C 8B 74 24 ?? 48 8B 74 24 ?? 4C 8B 7C 24 ?? ")]
    readonly Hook<Instrument>? instrumentHook;
    private delegate void Instrument(IntPtr* a1, IntPtr a2, IntPtr a3, int a4);
    public void InstrumentDetour(IntPtr* a1, IntPtr a2, IntPtr a3, int a4)
    {

        PluginLog.Log("Instrument! " + Marshal.PtrToStringUTF8((IntPtr)((GameObject*)a2)->Name));
        instrumentHook!.Original(a1, a2, a3, a4);
    }


    private delegate IntPtr AltEnding(IntPtr a1, IntPtr a2);

        private delegate IntPtr AlternativeTo(IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5, char a6, int a7);

    private delegate IntPtr OnGodForReal(IntPtr a1, IntPtr a2, int a3, int* a4, IntPtr* a5, IntPtr* a6);

    private delegate IntPtr VfxOwO(IntPtr a1);
    private delegate IntPtr PossibleVFXSoundOrigin(IntPtr a1, IntPtr soundPath, float presumeVolume, int a4, int hexPosX, int hexPosY, int hexPosZ, int a8, int a9, int a10, char a11, int a12, char a13);

    internal override void OnInit()
    {
        _loadCharacterSoundHook =
            PluginHandlers.Hooking.HookFromAddress<LoadCharacterSound>(
                (nint)VfxContainer.MemberFunctionPointers.LoadCharacterSound,
                LoadCharacterSoundDetour);

        apricotListenerSoundPlayHook?.Enable(); 
        _loadCharacterSoundHook?.Enable();
        vfxSoundOrigin?.Enable();
        altEnding?.Enable();
        alternativeSoundPlay?.Enable(); 
        streamSoundPlayerHook?.Enable();
        testingHook?.Enable();
        voiceDelegateHook?.Enable();
        testing3?.Enable();
        instrumentHook?.Enable();
        whatCOuldThisBe?.Enable();
        soundBattleLive?.Enable();
        battleETCHook?.Enable();
        battleEtcHook2?.Enable();
        thunderHook?.Enable();

    }

    internal override void OnDispose()
    {
        apricotListenerSoundPlayHook?.Dispose();
        _loadCharacterSoundHook?.Dispose();
        altEnding?.Dispose();
        vfxSoundOrigin?.Dispose();
        alternativeSoundPlay?.Dispose();
        streamSoundPlayerHook?.Dispose();
        testingHook?.Dispose();
        voiceDelegateHook?.Dispose();
        testing3?.Dispose();
        instrumentHook?.Dispose();
        whatCOuldThisBe?.Dispose();
        soundBattleLive?.Dispose();
        battleETCHook?.Dispose();
        battleEtcHook2?.Dispose();
        thunderHook?.Dispose();
    }

    [Signature("40 53 48 83 EC 20 48 8B D9 84 D2 74 25 ", DetourName = nameof(OnThunder))]
    readonly Hook<ThunderDelegate>? thunderHook;
    private delegate void ThunderDelegate(IntPtr a1, bool a2);
    void OnThunder(IntPtr a1, bool a2)
    {
        // a1 + 88 is sometimes the thunder sound file path
        PluginLog.Log("THUNDER STRUCK!");
        thunderHook!.Original(a1, a2);
    }


    [Signature("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 4B 14 ", DetourName = nameof(BattleEtc2Detour))]
    readonly Hook<SoundBattleEtc2>? battleEtcHook2;
    private delegate void SoundBattleEtc2(IntPtr a1,  IntPtr a2, IntPtr a3);
    void BattleEtc2Detour(IntPtr a1, IntPtr a2, IntPtr a3)
    {
        PluginLog.Log("Battle Etc 2");
        battleEtcHook2!.Original(a1, a2, a3);
    }

    [Signature("E8 ?? ?? ?? ?? F6 43 20 01 74 08 ", DetourName = nameof(SomethingBattleEtcDetour))]
    readonly Hook<SomethingBattleETC>? battleETCHook;
    private delegate void SomethingBattleETC(IntPtr a1, IntPtr a2);
    void SomethingBattleEtcDetour(IntPtr a1, IntPtr a2)
    {
        PluginLog.Log("battle ETC Detour!!!!");
        battleETCHook!.Original(a1, a2);
    }

    [Signature("40 53 57 41 56 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? ", DetourName = nameof(SoundBattleLive))]
    readonly Hook<whatCouldThisBe>? soundBattleLive;

    IntPtr SoundBattleLive(IntPtr a1, uint a2)
    {

        PluginLog.Log("Sound Battle Live");
        return soundBattleLive!.Original(a1, a2);
    }

    [Signature("E8 ?? ?? ?? ?? C7 43 ?? ?? ?? ?? ?? C6 43 26 01 ", DetourName = nameof(WhatCouldThisBe))]
    readonly Hook<whatCouldThisBe>? whatCOuldThisBe;
    private delegate IntPtr whatCouldThisBe(IntPtr a1, uint a2);
    IntPtr WhatCouldThisBe(IntPtr a1, uint a2)
    {

        PluginLog.Log("What could this be!");
        return whatCOuldThisBe!.Original(a1, a2);
    }


    [Signature("E8 ?? ?? ?? ?? 0F B6 06 3C 05 ")]
    readonly Hook<testing2>? testing3;
    private delegate IntPtr testing2(IntPtr a1, int a2, uint a3, IntPtr* a4, byte a5, uint a6, int a7, char a8);
    IntPtr testing3detour(IntPtr a1, int a2, uint a3, IntPtr* a4, byte a5, uint a6, int a7, char a8)
    {
        //PluginLog.Log("Testing 3!");
        return testing3!.Original(a1, a2, a3, a4, a5, a6, a7, a8);
    }

    [Signature("40 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 8D 42 FF 48 8B F9 3D ?? ?? ?? ?? ", DetourName = nameof(SomethingDetour))]
    readonly Hook<somethingVoiceHookDelegate>? voiceDelegateHook;
    public IntPtr SomethingDetour(IntPtr a1, IntPtr a2)
    {
        PluginLog.Log("Something: Vo_CM/Vo_cm_PC");
        return voiceDelegateHook!.Original(a1, a2);
    }

    private delegate IntPtr somethingVoiceHookDelegate(IntPtr a1, IntPtr a2);

    [Signature("E8 ?? ?? ?? ?? FF C6 83 FE 48", DetourName = nameof(testingDetour))]
    readonly Hook<testing>? testingHook;

    private delegate IntPtr testing(IntPtr a1, IntPtr a2, IntPtr a3, int a4);

    IntPtr testingDetour(IntPtr a1, IntPtr a2, IntPtr a3, int a4)
    {
        //PluginLog.Log("Testing Hook! " + Marshal.PtrToStringUTF8(a1!));
        return testingHook!.Original(a1, a2, a3, a4);
    }

    [Signature("E8 ?? ?? ?? ?? 48 89 47 18 89 5F 20 ", DetourName = nameof(StreamSoundPlayerDetour2))]
    readonly Hook<StreamSoundPlayer>? streamSoundPlayerHook2;
    IntPtr StreamSoundPlayerDetour2(IntPtr a1, IntPtr a2, float a3, uint a4)
    {
        PluginLog.LogWarning($"Stream sound2: " + Marshal.PtrToStringUTF8(a2));
        return streamSoundPlayerHook!.Original(a1, a2, a3, a4);
    }

    [Signature("E8 ?? ?? ?? ?? 48 89 47 18 89 5F 20 ", DetourName = nameof(StreamSoundPlayerDetour))]
    readonly Hook<StreamSoundPlayer>? streamSoundPlayerHook;
    delegate IntPtr StreamSoundPlayer(IntPtr a1, IntPtr a2, float a3, uint a4);
    IntPtr StreamSoundPlayerDetour(IntPtr a1, IntPtr a2, float a3, uint a4)
    {
        PluginLog.LogWarning($"Stream sound: " + Marshal.PtrToStringUTF8(a2));
        return streamSoundPlayerHook!.Original(a1, a2, a3, a4);
    }

    [Signature("E8 ?? ?? ?? ?? 8B 7D 77 85 FF ", DetourName = nameof(ALternativeSoundPlayDetour))]
    readonly Hook<AlternativeSoundPlay>? alternativeSoundPlay;
    private delegate IntPtr AlternativeSoundPlay(IntPtr a1, IntPtr fileName, float a3, int a4, int a5, byte a6, uint a7);
    IntPtr ALternativeSoundPlayDetour(IntPtr a1, IntPtr fileName, float a3, int a4, int a5, byte a6, uint a7)
    {
        PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.3, new Vector4(0, 0, 0, 0.5f), ImGui.GetMousePos(), 200, 5, 4f));
        
        //PluginLog.LogWarning($"UI Sound {a3}. {a4}, {a5} {a6} {a7}: " + Marshal.PtrToStringUTF8(fileName));
        return alternativeSoundPlay!.Original(a1, fileName, a3, a4, a5, a6, a7);
    }

    private IntPtr AltEndingDetour(IntPtr a1, IntPtr a2)
    {
        //PluginLog.LogInformation("Alt Ending ----------");
        return altEnding!.Original(a1, a2);
    }

    private IntPtr VFXOriginDetour(IntPtr a1, IntPtr soundPath, float presumeVolume, int a4, int hexPosX, int hexPosY, int hexPosZ, int a8, int a9, int a10, char a11, int a12, char a13)
    {
        try
        {
            //PluginLog.Log("What is this? :" + Marshal.PtrToStringUTF8(soundPath) + ",          Pos: " + MakeFromSLODWORDPOS(hexPosX, hexPosY, hexPosZ).ToString());
        }
        catch (Exception e) { PluginLog.Log(e.Message); }

        string path = Marshal.PtrToStringUTF8(soundPath)!;

        //PluginLog.LogWarning("VFX: " + path);

        //SoundStorage.RegisterLastVFXLocation(MathUtils.instance.MakeFromSLODWORDPOS(hexPosX, hexPosY, hexPosZ));

        //PluginLog.Log(MathUtils.instance.MakeFromSLODWORDPOS(hexPosX, hexPosY, hexPosZ).ToString());

        if (path == $"sound{SoundParser.SlashSymbol}vfx{SoundParser.SlashSymbol}SE_VFX_common.scd")
        {
            //PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(1.5, new Vector4(1, 1, 1, 0.5f), SoundStorage.LastVFXLocation, 40, 4, 4f));
        }

        return vfxSoundOrigin!.Original(a1, soundPath, presumeVolume, a4, hexPosX, hexPosY, hexPosZ, a8, a9, a10, a11, a12, a13);
    }

    Vector3 prePos = new Vector3();

    private unsafe nint ApricotListenerSoundPlayDetour(nint a1, nint a2, nint a3, nint a4, nint a5, nint a6)
    {
        //PluginLog.Log("Apricot!");
        //enabled = true;


        //enabled = false;

        /*
        if (a6 == nint.Zero)
            return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);
        */

        //PluginLog.Log("Path: " + Marshal.PtrToStringUTF8((IntPtr)a2));
       

        GameObject* gameObject = (*(delegate* unmanaged<nint, GameObject*>**)a6)[1](a6);
        if (gameObject != null)
        {
            SoundStorage.RegisterLastApricot(gameObject);
            //PluginLog.Log(((nint)gameObject).ToString());
            //PluginLog.Log(Marshal.PtrToStringUTF8((IntPtr)gameObject->Name)!);
        }
        else
        {
            
            try
            {
                DrawObject* drawObject = ((DrawObject**)a6)[1];
                if (drawObject != null)
                {
                   // PluginLog.Log(drawObject->Object.Position.ToString());
                }
            }catch(Exception e) { PluginLog.Log(e.Message); }
        }
        //PluginLog.Log("");
        return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);
    }

    /// <summary> Characters load some of their voice lines or whatever with this function. </summary>
    private delegate nint LoadCharacterSound(VfxContainer* character, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7);

    private nint LoadCharacterSoundDetour(VfxContainer* container, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7)
    {
        try
        {
            
                }
        catch { }
        if (container->OwnerObject != null)
        {
            PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new PlayerSpeechModule(3.0, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), container->OwnerObject, Vector3.Zero, "j_ago"));
        }

        return _loadCharacterSoundHook.Original(container, unk1, unk2, unk3, unk4, unk5, unk6, unk7);
    }
}

public struct ResourceItemStruct
{
    public uint Hash;
    public ulong Address;
    public string Path;
    public uint Refs;
}
