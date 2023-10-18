using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using ImGuiNET;
using System.Numerics;

namespace SoundVisualization.Windows.AudioModules.Base;

public unsafe abstract class DynamicSkeletonAudioModule : DynamicPlayerCharaAudioModule
{
    readonly Skeleton* skeleton;
    public Skeleton* Skeleton => skeleton;

    readonly Vector3 skeletonScale;
    public Vector3 SkeletonScale => skeletonScale;

    public DynamicSkeletonAudioModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, float startSize) : base(timer, colour, player, offset, startSize)
    {
        skeleton = CharacterBase->Skeleton;
        skeletonScale = skeleton->Transform.Scale;
    }

    public override bool Tick(double time)
    {
        if (skeleton == null) return true;
        return base.Tick(time);
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
