using SoundVisualization.Core.Attributes;
using SoundVisualization.Core.Hooking.Attributes;
using SoundVisualization.Core.Sound;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System;
using static SoundVisualization.Core.Hooking.Delegates;

namespace SoundVisualization.Core.Hooking.Hooks;

[Hook]
internal unsafe class FootstepSoundHook : HookableElement
{
    [Unused]
    [Signature("48 83 EC 48 4D 63 D0")]
    readonly Hook<OnFootstep>? onFootstepHook;

    [Signature("E8 ?? ?? ?? ?? 83 3D ?? ?? ?? ?? ?? 73 6C ", DetourName = nameof(OnFootstep))]
    readonly Hook<FootstepLocation>? footstepLocation;

    [Signature("E9 ?? ?? ?? ?? 49 8B CE E8 ?? ?? ?? ?? 8B F8 ", DetourName = nameof(EarlyFootstepCatch))]
    readonly Hook<PreFootstep>? preFootstepHook;

    internal override void OnInit()
    {
        preFootstepHook?.Enable();
        footstepLocation?.Enable();
    }

    internal override void OnDispose()
    {
        preFootstepHook?.Dispose();
        footstepLocation?.Dispose(); 
        onFootstepHook?.Dispose();
    }

    public void OnFootstep(IntPtr a1, float a2, float a3, float a4, uint a5, int a6, uint a7, uint a8, uint a9, int a10, int a11, int a12, int a13, float a14, int a15, uint a16, char a17)
    {
        SoundStorage.footstepCount = 2;
        footstepLocation!.Original(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17);
    }

    public void EarlyFootstepCatch(GameObject* a1, uint a2, int a3)
    {
        SoundStorage.RegisterLastAccessedGameObject(a1);
        preFootstepHook!.Original(a1, a2, a3);
    }
}
