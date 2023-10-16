using AstralAether.Core.Hooking.Attributes;
using AstralAether.Core.Sound;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static AstralAether.Core.Hooking.Delegates;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace AstralAether.Core.Hooking.Hooks;

// Battle sounds include a lot of player battle sounds,
// but it also includes stuff like emotes.

[Hook]
internal unsafe class BattleSoundHook : HookableElement
{
    [Signature("E8 ?? ?? ?? ?? 0F B6 06 3C 05", DetourName = nameof(EarlyBattleSoundDetour))]
    readonly Hook<EarlyBattleSound>? earlyBattleSoundHook;

    [Signature("E8 ?? ?? ?? ?? 4C 8B A4 24 ?? ?? ?? ?? 48 8B AC 24 ?? ?? ?? ?? 4C 8B AC 24 ?? ?? ?? ?? 48 8B 9C 24 ?? ?? ?? ??", DetourName = nameof(BattleSoundDetour))]
    readonly Hook<BattleSound>? battleSoundHook;

    [Signature("E8 ?? ?? ?? ?? 44 8B 8C 24 ?? ?? ?? ?? 45 85 C9", DetourName = nameof(BattleMonsterSoundDelegateDetour))]
    readonly Hook<BattleMonsterSoundDelegate>? battleMonsterSoundHook;

    internal override void OnInit()
    {
        earlyBattleSoundHook?.Enable();
        battleSoundHook?.Enable();
        battleMonsterSoundHook?.Enable();
    }

    internal override void OnDispose()
    {
        earlyBattleSoundHook?.Dispose();
        battleSoundHook?.Dispose();
        battleMonsterSoundHook?.Dispose();
    }

    IntPtr EarlyBattleSoundDetour(VfxContainer* vfxContainer, int a2, uint a3, IntPtr* a4, byte a5, uint a6, int a7, char a8)
    {
      //  PluginLog.Log("Early Battle Sound!");
        GameObject* targetObject = (GameObject*)vfxContainer->OwnerObject;
        if (targetObject != null)
        { 
            SoundStorage.RegisterBattleSound(targetObject);
            //PluginLog.Log(Marshal.PtrToStringUTF8((IntPtr)(targetObject->GetName())!)!);
        }
        return earlyBattleSoundHook!.Original(vfxContainer, a2, a3, a4, a5, a6, a7, a8);
    }

    IntPtr BattleSoundDetour(IntPtr a1, float soundX, float soundY, float soundZ, uint a5, int a6, uint a7, uint a8, int a9, uint a10, byte a11, uint a12, char a13)
    {
        //SoundStorage.RegisterLastBattleSoundLocation(new Vector3(soundX, soundY, soundZ));
        return battleSoundHook!.Original(a1, soundX, soundY, soundZ, a5, a6, a7, a8, a9, a10, a11, a12, a13);
    }

    // This is mostly footsteps, but I'll have to keep testing
    void** BattleMonsterSoundDelegateDetour(IntPtr a1, float x, float y, float z, uint a5, int a6, uint a7, char a8)
    {
        if (SoundStorage.LastBattleSound == null) SoundStorage.RegisterBattleSound(SoundStorage.LastFootstep);
        //PluginLog.Log("battle sound!");
        return battleMonsterSoundHook!.Original(a1, x, y, z, a5, a6, a7, a8);
    }
}
