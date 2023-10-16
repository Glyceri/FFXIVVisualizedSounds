using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System.Numerics;

namespace AstralAether.Core.Sound;

public static unsafe class SoundStorage
{
    public static GameObject* LastBattleSound { get; private set; } = null!;
    public static GameObject* LastApricot { get; private set; } = null!;
    public static GameObject* LastFootstep { get; private set; } = null!;

    public static void RegisterBattleSound(GameObject* lastBattleSound) => LastBattleSound = lastBattleSound;
    public static void RegisterLastApricot(GameObject* apricot) => LastApricot = apricot;
    public static void RegisterLastFootstep(GameObject* footstep) => LastFootstep = footstep;

    public static void ClearLastApricot()           => LastApricot = null;
    public static void ClearLastBattleSound()       => LastBattleSound = null;
    public static void ClearLastFootstep()          => LastFootstep = null;

    public static void ClearAll()
    {
        ClearLastApricot();
        ClearLastBattleSound();
        ClearLastFootstep();
    }
}
