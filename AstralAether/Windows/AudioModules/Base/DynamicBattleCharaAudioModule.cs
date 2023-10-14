using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules.Base;

public unsafe abstract class DynamicBattleCharaAudioModule : DynamicScalableAudioModule
{
    readonly BattleChara* battleChara;
    public BattleChara* BattleChara => battleChara;

    protected DynamicBattleCharaAudioModule(double timer, Vector4 colour, BattleChara* followMe, Vector3 offset, float startSize) : base(timer, colour, (GameObject*)followMe, offset, startSize)
    {
        battleChara = followMe;
    }

    public override bool Tick(double time)
    {
        if (battleChara == null) return true;
        return base.Tick(time);
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
