using AstralAether.Windows.AudioModules.Base;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules;

public unsafe class PlayerSpeechModule : DynamicBoneTransformModule
{
    public PlayerSpeechModule(double timer, float radius, Vector4 colour, BattleChara* player, Vector3 offset, string boneName) : base(timer, colour, player, offset, radius, boneName) { }

    public override void BoneTransformDraw(ImDrawListPtr drawListPtr)
    {
        drawListPtr.AddCircle(WorldToScreen(ScaledBoneTranslation), StartSize, Colour, 12);

        new StaticLineModule(0, UnbakedColour, FollowedMixedTranslation(ScaledBone + new Vector3(0, -0.02f * SkeletonScale.Y, HitboxRadius * 0.15f)), FollowedMixedTranslation(ScaledBone + new Vector3(0, -0.02f * SkeletonScale.Y, HitboxRadius * 0.25f))).Draw(drawListPtr);
        new StaticLineModule(0, UnbakedColour, FollowedMixedTranslation(ScaledBone + new Vector3(0, SkeletonScale.Y, HitboxRadius * 0.15f)), FollowedMixedTranslation(ScaledBone + new Vector3(0, -0.01f * SkeletonScale.Y, HitboxRadius * 0.25f))).Draw(drawListPtr);
        new StaticLineModule(0, UnbakedColour, FollowedMixedTranslation(ScaledBone + new Vector3(0, -0.04f * SkeletonScale.Y, HitboxRadius * 0.15f)), FollowedMixedTranslation(ScaledBone + new Vector3(0, -0.03f * SkeletonScale.Y, HitboxRadius * 0.25f))).Draw(drawListPtr);
    }
}
