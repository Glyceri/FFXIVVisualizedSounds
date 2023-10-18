using SoundVisualization.Core.Attributes;
using SoundVisualization.Core.Hooking.Attributes;
using SoundVisualization.Core.Sound;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System;
using static SoundVisualization.Core.Hooking.Delegates;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace SoundVisualization.Core.Hooking.Hooks;

// Battle sounds include a lot of player battle sounds,
// but it also includes stuff like emotes.

[Hook]
internal unsafe class BattleSoundHook : HookableElement
{
    [Signature("E8 ?? ?? ?? ?? 0F B6 06 3C 05", DetourName = nameof(EarlyBattleSoundDetour))]
    readonly Hook<EarlyBattleSound>? earlyBattleSoundHook;

    [Unused]
    [Signature("E8 ?? ?? ?? ?? 4C 8B A4 24 ?? ?? ?? ?? 48 8B AC 24 ?? ?? ?? ?? 4C 8B AC 24 ?? ?? ?? ?? 48 8B 9C 24 ?? ?? ?? ??")]
    readonly Hook<BattleSound>? battleSoundHook;

    [Signature("E8 ?? ?? ?? ?? 44 8B 8C 24 ?? ?? ?? ?? 45 85 C9", DetourName = nameof(BattleMonsterSoundDelegateDetour))]
    readonly Hook<BattleMonsterSoundDelegate>? battleMonsterSoundHook;

    internal override void OnInit()
    {
        earlyBattleSoundHook?.Enable();
        battleMonsterSoundHook?.Enable();
    }

    internal override void OnDispose()
    {
        earlyBattleSoundHook?.Dispose();
        battleMonsterSoundHook?.Dispose();
        battleSoundHook?.Dispose();
    }

    IntPtr EarlyBattleSoundDetour(VfxContainer* vfxContainer, int a2, uint a3, IntPtr* a4, byte a5, uint a6, int a7, char a8)
    {
        GameObject* targetObject = (GameObject*)vfxContainer->OwnerObject;
        SoundStorage.RegisterLastAccessedGameObject(targetObject);
        return earlyBattleSoundHook!.Original(vfxContainer, a2, a3, a4, a5, a6, a7, a8);
    }

    // This is mostly footsteps, but I'll have to keep testing
    void** BattleMonsterSoundDelegateDetour(IntPtr a1, float x, float y, float z, uint a5, int a6, uint a7, char a8)
    {
        return battleMonsterSoundHook!.Original(a1, x, y, z, a5, a6, a7, a8);
    }
}
