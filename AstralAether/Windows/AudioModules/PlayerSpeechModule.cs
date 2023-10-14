using AstralAether.Utilization.UtilsModule;
using AstralAether.Windows.AudioModules.Base;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules;

public unsafe class PlayerSpeechModule : DynamicBoneTransformModule
{
    public PlayerSpeechModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, string boneName, int lockTo = -1) : base(timer, colour, player, offset, 0, boneName, lockTo) { }

    public override void BoneTransformDraw(ImDrawListPtr drawListPtr)
    {
        //drawListPtr.AddCircle(WorldToScreen(ScaledBoneTranslation), 80, Colour, 12, 5.0f);

        new StaticLineModule(0, UnbakedColour, GetFor(), GetFor(0, 0.10f)).Draw(drawListPtr);
        new StaticLineModule(0, UnbakedColour, GetFor(0.005f), GetFor(0.015f, 0.10f)).Draw(drawListPtr);
        new StaticLineModule(0, UnbakedColour, GetFor(-0.005f), GetFor(-0.015f, 0.10f)).Draw(drawListPtr);
    }

    Vector3 GetFor(float offset = 0, float distance = 0) => FollowedMixedTranslation(ScaledBone + Vector3.Transform(GetOffsetAt(offset) + new Vector3(Distance(distance), 0, 0), BoneMatrix));
    Vector3 GetOffsetAt(float offset) => RemapUtils.instance.GetOffset(BoneName, Gender, Tribe) + new Vector3(0, offset * SkeletonScale.Y, 0);
    float Distance(float distance) => HitboxRadius * distance;
}
