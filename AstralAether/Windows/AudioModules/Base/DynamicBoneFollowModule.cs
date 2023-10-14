using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.Havok;
using ImGuiNET;
using System.Numerics;

namespace AstralAether.Windows.AudioModules.Base;

public unsafe abstract class DynamicBoneFollowModule : DynamicSkeletonAudioModule
{
    readonly string boneName;
    public string BoneName => boneName;

    public DynamicBoneFollowModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, float startSize, string boneName) : base(timer, colour, player, offset, startSize)
    {
        this.boneName = boneName;
    }

    public override sealed void Draw(ImDrawListPtr drawListPtr)
    {
        for (int pSkeleIndex = Skeleton->PartialSkeletonCount - 1; pSkeleIndex >= 0; pSkeleIndex--)
        {
            PartialSkeleton currentPartial = Skeleton->PartialSkeletons[pSkeleIndex];
            hkaPose* curPose = currentPartial.GetHavokPose(0);
            if (curPose == null) return;

            hkaSkeleton* curPoseSkeleton = curPose->Skeleton;
            if (curPoseSkeleton == null) return;
            int boneCount = curPoseSkeleton->Bones.Length;
            for (int currentBone = 0; currentBone < boneCount; currentBone++)
            {
                PluginLog.Log("pre: " + BoneName + " : " + curPoseSkeleton->Bones[currentBone].Name.String);
                if (curPoseSkeleton->Bones[currentBone].Name.String is string boneString && boneString != null && BoneName == boneString)
                {
                    PluginLog.Log(BoneName);
                    BoneDraw(drawListPtr, curPose, curPoseSkeleton, curPoseSkeleton->Bones[currentBone], currentBone);
                    break;
                }
            }
        }
    }

    public abstract void BoneDraw(ImDrawListPtr drawListPtr, hkaPose* currentPose, hkaSkeleton* curPoseSkeleton, hkaBone bone, int boneIndex);
}
