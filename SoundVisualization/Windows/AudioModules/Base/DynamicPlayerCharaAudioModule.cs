using SoundVisualization.Utilization.UtilsModule;
using Dalamud.Game.ClientState.Objects.Enums;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using ImGuiNET;
using System.Numerics;

namespace SoundVisualization.Windows.AudioModules.Base;

public unsafe abstract class DynamicPlayerCharaAudioModule : DynamicBattleCharaAudioModule
{
    readonly CharacterBase* characterBase;
    public CharacterBase* CharacterBase => characterBase; 

    readonly Character* character;
    public Character* Character => character;

    readonly Gender gender;
    public Gender Gender => gender;

    readonly Tribe tribe;
    public Tribe Tribe => tribe;

    protected DynamicPlayerCharaAudioModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, float startSize) : base(timer, colour, player, offset, startSize)
    {
        characterBase = (CharacterBase*)FollowMe->GetDrawObject();
        character = (Character*)player;
        gender = (Gender)character->DrawData.CustomizeData[(int)CustomizeIndex.Gender];
        tribe = (Tribe)character->DrawData.CustomizeData[(int)CustomizeIndex.Tribe];
    }

    public override bool Tick(double time)
    {
        if (characterBase == null) return true;
        return base.Tick(time);
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
