using AstralAether.Core.Handlers;
using AstralAether.Core.Hooking.Attributes;
using AstralAether.Windows.AudioModules;
using AstralAether.Windows.Windows;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace AstralAether.Core.Hooking.Hooks;

[Hook]
internal unsafe class SoundHook : HookableElement
{
    public const string ResourceManager = "48 8B 05 ?? ?? ?? ?? 33 ED F0";

    public delegate void* GetResourceSyncPrototype(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType,
            int* resourceHash, byte* path, nint resParams);

    public delegate void* GetResourceAsyncPrototype(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType,
        int* resourceHash, byte* path, nint resParams, bool isUnknown);

    // ====== FILES HOOKS ========

    public Hook<GetResourceSyncPrototype> GetResourceSyncHook { get; private set; }

    public Hook<GetResourceAsyncPrototype> GetResourceAsyncHook { get; private set; }

    [Signature("48 89 6C 24 ?? 41 54 41 56 41 57 48 81 EC", DetourName = nameof(ApricotListenerSoundPlayDetour))]
    readonly Hook<ApricotListenerSoundPlayDelegate>? apricotListenerSoundPlayHook = null!;

    [Signature("48 83 EC 48 4D 63 D0")]
    readonly Hook<FOOTSTEPFOOTSTEP>? _temp;

    [Signature("E9 ?? ?? ?? ?? 49 8B CE E8 ?? ?? ?? ?? 8B F8 ")]
    readonly Hook<FootstepPrePre>? _FootPrePre;

    private delegate nint ApricotListenerSoundPlayDelegate(nint a1, nint a2, nint a3, nint a4, nint a5, nint a6);
    private delegate void* PlaySpecificSoundDelegate(long a1, int idx);

    private delegate char FOOTSTEPFOOTSTEP(IntPtr a1, char* a2, int a3, int a4, int a5, int a6, int a7);

    private delegate void FootstepPrePre(IntPtr a1, uint a2, int a3);

    private Hook<LoadCharacterSound> _loadCharacterSoundHook;

    [Signature("E8 ?? ?? ?? ?? 83 3D ?? ?? ?? ?? ?? 73 6C ")]
    readonly Hook<FOOTSTEPPRE>? _pre;

    [Signature("E8 ?? ?? ?? ?? 4C 8B A4 24 ?? ?? ?? ?? 48 8B AC 24 ?? ?? ?? ?? 4C 8B AC 24 ?? ?? ?? ?? 48 8B 9C 24 ?? ?? ?? ?? ")]
    readonly Hook<BattleSound>? _BattleSound;

    [Signature(" E8 ?? ?? ?? ?? 0F B6 06 3C 05 ")]
    readonly Hook<BattleSoundPre>? _battleSoundPre;

    private delegate void FOOTSTEPPRE(IntPtr a1,
        float a2,
        float a3,
        float a4,
        uint a5,
        int a6,
        uint a7,
        uint a8,
        uint a9,
        int a10,
        int a11,
        int a12,
        int a13,
        float a14,
        int a15,
        uint a16,
        char a17);

    private delegate IntPtr BattleSound(IntPtr a1,
        float a2,
        float a3,
        float a4,
        uint a5,
        int a6,
        uint a7,
        uint a8,
        int a9,
        uint a10,
        byte a11,
        uint a12,
        char a13);
    // E8 ? ? ? ? 0F B6 06 3C 05 
    private delegate IntPtr BattleSoundPre(IntPtr a1,
        int a2,
        uint a3,
        IntPtr* a4,
        byte a5,
        uint a6,
        int a7,
        char a8);

    bool enabled = true;

    internal override void OnInit()
    {
        GetResourceSyncHook = PluginHandlers.Hooking.HookFromSignature<GetResourceSyncPrototype>("E8 ?? ?? 00 00 48 8D 8F ?? ?? 00 00 48 89 87 ?? ?? 00 00", GetResourceSyncHandler);
        GetResourceAsyncHook = PluginHandlers.Hooking.HookFromSignature<GetResourceAsyncPrototype>("E8 ?? ?? ?? 00 48 8B D8 EB ?? F0 FF 83 ?? ?? 00 00", GetResourceAsyncHandler);

        _loadCharacterSoundHook =
            PluginHandlers.Hooking.HookFromAddress<LoadCharacterSound>(
                (nint)VfxContainer.MemberFunctionPointers.LoadCharacterSound,
                LoadCharacterSoundDetour);

        apricotListenerSoundPlayHook?.Enable(); 
        _loadCharacterSoundHook?.Enable();
        GetResourceSyncHook?.Enable();
        GetResourceAsyncHook?.Enable();
        _temp?.Enable();
        _pre?.Enable();
        _FootPrePre?.Enable();
        _BattleSound?.Enable();
        _battleSoundPre?.Enable();
    }

    internal override void OnDispose()
    {
        apricotListenerSoundPlayHook?.Dispose();
        _loadCharacterSoundHook?.Dispose();
        GetResourceSyncHook?.Dispose();
        GetResourceAsyncHook?.Dispose();
        _temp?.Dispose();
        _pre?.Dispose();
        _FootPrePre?.Dispose();
        _battleSoundPre?.Dispose();
        _BattleSound?.Dispose();
    }

    GameObject* lastFoot = null!;
    Vector3 footPos = new Vector3();

    public void FootPrePre(IntPtr a1, uint a2, int a3)
    {
        lastFoot = (GameObject*)a1;
        _FootPrePre!.Original(a1, a2, a3);
    }

    public void Pre(IntPtr a1,
        float a2,
        float a3,
        float a4,
        uint a5,
        int a6,
        uint a7,
        uint a8,
        uint a9,
        int a10,
        int a11,
        int a12,
        int a13,
        float a14,
        int a15,
        uint a16,
        char a17)
    {
        footPos = new Vector3(a2, a3, a4);
        PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.5, new Vector4(0.4f, 0.4f, 1.0f, 1.0f), footPos, lastFoot->HitboxRadius * 60));

        _pre!.Original(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);
    }

    Vector3 prePos = new Vector3();

    public IntPtr BattleSoundPreDetour(IntPtr a1,
        int a2,
        uint a3,
        IntPtr* a4,
        byte a5,
        uint a6,
        int a7,
        char a8)
    {
        VfxContainer* vfxContainer = (VfxContainer*)(a1);
        GameObject* gObject = &vfxContainer->OwnerObject->Character.GameObject;
        if (gObject != null)
        {
            lastFoot = gObject;
            prePos = gObject->Position;
            PluginLog.Log("Name: " + Marshal.PtrToStringUTF8((IntPtr)gObject->Name)!);
            PluginLog.Log("Pos: " + gObject->Position.ToString());
        }
        return _battleSoundPre!.Original(a1, a2, a3, a4, a5, a6, a7, a8);
    }

    public IntPtr BattleSoundDetour(IntPtr a1,
        float a2,
        float a3,
        float a4,
        uint a5,
        int a6,
        uint a7,
        uint a8,
        int a9,
        uint a10,
        byte a11,
        uint a12,
        char a13)
    {

        if (lastFoot != null)
        {
            PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new PlayerSpeechModule(13.0, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), (BattleChara*)lastFoot, Vector3.Zero, "j_ago"));
        }
       
        return _BattleSound!.Original(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
    }

    public char TempDetour(IntPtr a1, char* a2, int a3, int a4, int a5, int a6, int a7)
    {
        try
        {
            //Character* chara = (Character*)a1;
            //PluginLog.Log(Marshal.PtrToStringUTF8((IntPtr)chara->GameObject.Name)!);

        }catch(Exception e) { PluginLog.Log(e.Message); }
        return _temp!.Original(a1, a2, a3, a4, a5, a6, a7);
    }

    private unsafe nint ApricotListenerSoundPlayDetour(nint a1, nint a2, nint a3, nint a4, nint a5, nint a6)
    {
        //enabled = true;
        

        //enabled = false;

        /*
        if (a6 == nint.Zero)
            return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);
        */
       

        GameObject* gameObject = (*(delegate* unmanaged<nint, GameObject*>**)a6)[1](a6);
        if (gameObject != null)
        {
            //PluginLog.Log(((nint)gameObject).ToString());
            PluginLog.Log(Marshal.PtrToStringUTF8((IntPtr)gameObject->Name)!);
        }
        else
        {
            
            try
            {
                DrawObject* drawObject = ((DrawObject**)a6)[1];
                if (drawObject != null)
                {
                    PluginLog.Log(drawObject->Object.Position.ToString());
                }
            }catch(Exception e) { PluginLog.Log(e.Message); }
        }
        PluginLog.Log("");
        return apricotListenerSoundPlayHook!.Original(a1, a2, a3, a4, a5, a6);
    }

    private void* GetResourceSyncHandler(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash,byte* path, nint resParams)
    {
        string p = Marshal.PtrToStringUTF8((IntPtr)path)!;
        if (enabled && PathValid(p)) PluginLog.Log("Path: " + p);
        return GetResourceSyncHook!.Original(resourceManager, categoryId, resourceType, resourceHash, path, resParams);
    }

    private void* GetResourceAsyncHandler(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash, byte* path, nint resParams, bool isUnknown)
    {
        string p = Marshal.PtrToStringUTF8((IntPtr)path)!;
        if (enabled && PathValid(p)) PluginLog.Log("Path Async: " + p);
        return GetResourceAsyncHook!.Original(resourceManager, categoryId, resourceType, resourceHash, path, resParams, isUnknown);
    }


    bool PathValid(string path)
    {
        if (!path.EndsWith(".scd")) return false;
        if (!path.StartsWith("sound/")) return false;
        //if (!path.StartsWith("sound/vfx/") && !path.StartsWith("sound/foot/")) return false;
        if (path.StartsWith("sound/foot")) return false;
        if (path.StartsWith("sound/system")) return false;
        return true;
    }

    /// <summary> Characters load some of their voice lines or whatever with this function. </summary>
    private delegate nint LoadCharacterSound(VfxContainer* character, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7);



    private nint LoadCharacterSoundDetour(VfxContainer* container, int unk1, int unk2, nint unk3, ulong unk4, int unk5, int unk6, ulong unk7)
    {
        if (container->OwnerObject != null)
        {
            PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new PlayerSpeechModule(13.0, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), container->OwnerObject, Vector3.Zero, "j_ago"));
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
