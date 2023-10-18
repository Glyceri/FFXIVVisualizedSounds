using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System;
using System.Security.AccessControl;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.Character;

namespace SoundVisualization.Core.Hooking;

public static unsafe class Delegates
{
    public delegate IntPtr EarlyBattleSound(VfxContainer* vfxContainer, int a2, uint a3, IntPtr* a4, byte a5, uint a6, int a7, char a8);
    public delegate IntPtr BattleSound(IntPtr a1, float soundX, float soundY, float soundZ, uint a5, int a6, uint a7, uint a8, int a9, uint a10, byte a11, uint a12, char a13);
    public delegate void** BattleMonsterSoundDelegate(IntPtr a1, float soundX, float soundY, float soundZ, uint a5, int a6, uint a7, char a8);

    public delegate void* GetResourceSync(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash, byte* path, nint resParams);
    public delegate void* GetResourceAsync(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash, byte* path, nint resParams, bool isUnknown);

    public delegate IntPtr PlayAudioSource(IntPtr a1, IntPtr fileName, float a3, int posXAsHex, int posYAsHex, int posZAsHex, int a7, int a8, int a9, int a10, byte a11, uint a12, char a13, int a14, char a15, char a16);

    public delegate char OnFootstep(IntPtr a1, char* a2, int a3, int a4, int a5, int a6, int a7);
    public delegate void FootstepLocation(IntPtr a1, float a2, float a3, float a4, uint a5, int a6, uint a7, uint a8, uint a9, int a10, int a11, int a12, int a13, float a14, int a15, uint a16, char a17);
    public delegate void PreFootstep(GameObject* a1, uint a2, int a3);
}
