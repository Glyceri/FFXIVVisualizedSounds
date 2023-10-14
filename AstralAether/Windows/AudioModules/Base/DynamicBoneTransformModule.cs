using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.Havok;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules.Base;

public unsafe abstract class DynamicBoneTransformModule : DynamicBoneFollowModule
{
    hkQsTransformf boneTransform;
    public hkQsTransformf BoneTransform => boneTransform;

    Vector3 boneTranslation;
    public Vector3 BoneTranslation => boneTranslation;

    Vector3 scaledBone;
    public Vector3 ScaledBone => scaledBone;

    Vector3 scaledBoneTranslation;
    public Vector3 ScaledBoneTranslation => scaledBoneTranslation;

    public DynamicBoneTransformModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, float startSize, string boneName) : base(timer, colour, player, offset, startSize, boneName) { }

    public override sealed void BoneDraw(ImDrawListPtr drawListPtr, hkaPose* currentPose, hkaSkeleton* curPoseSkeleton, hkaBone bone, int boneIndex)
    {
        boneTransform = currentPose->ModelPose.Data[boneIndex];
        boneTranslation = new Vector3(boneTransform.Translation.X, boneTransform.Translation.Y, boneTransform.Translation.Z);
        scaledBone = boneTranslation * SkeletonScale;
        scaledBoneTranslation = FollowedMixedTranslation(scaledBone);
        BoneTransformDraw(drawListPtr);
    }

    public Vector3 MatrixedTranslation(Vector3 translation) => Vector3.Transform(translation, RotationMatrix);
    public Vector3 FollowedMixedTranslation(Vector3 translation) => (Vector3)FollowedPosition + MatrixedTranslation(translation);

    public abstract void BoneTransformDraw(ImDrawListPtr drawListPtr);
}