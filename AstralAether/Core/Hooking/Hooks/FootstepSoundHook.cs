using AstralAether.Core.Attributes;
using AstralAether.Core.Hooking.Attributes;
using AstralAether.Core.Sound;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using static AstralAether.Core.Hooking.Delegates;

namespace AstralAether.Core.Hooking.Hooks;

[Hook]
internal unsafe class FootstepSoundHook : HookableElement
{
    [Unused]
    [Signature("48 83 EC 48 4D 63 D0")]
    readonly Hook<OnFootstep>? onFootstepHook;

    [Unused]
    [Signature("E8 ?? ?? ?? ?? 83 3D ?? ?? ?? ?? ?? 73 6C ")]
    readonly Hook<FootstepLocation>? footstepLocation;

    [Signature("E9 ?? ?? ?? ?? 49 8B CE E8 ?? ?? ?? ?? 8B F8 ", DetourName = nameof(EarlyFootstepCatch))]
    readonly Hook<PreFootstep>? preFootstepHook;

    internal override void OnInit()
    {
        preFootstepHook?.Enable();
    }

    internal override void OnDispose()
    {
        onFootstepHook?.Dispose();
    }

    public void EarlyFootstepCatch(GameObject* a1, uint a2, int a3)
    {
        SoundStorage.RegisterLastFootstep(a1);
        preFootstepHook!.Original(a1, a2, a3);
    }
}
