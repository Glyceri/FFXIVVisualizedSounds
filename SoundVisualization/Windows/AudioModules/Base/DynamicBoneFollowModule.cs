using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.Havok;
using ImGuiNET;
using System;
using System.Numerics;

namespace SoundVisualization.Windows.AudioModules.Base;

public unsafe abstract class DynamicBoneFollowModule : DynamicSkeletonAudioModule
{
    readonly string boneName;
    public string BoneName => boneName;

    readonly int lockTo;
    public int LockTo => lockTo;

    public DynamicBoneFollowModule(double timer, Vector4 colour, BattleChara* player, Vector3 offset, float startSize, string boneName, int lockTo) : base(timer, colour, player, offset, startSize)
    {
        this.boneName = boneName;
        this.lockTo = lockTo;
    }

    public override sealed void Draw(ImDrawListPtr drawListPtr)
    {
        try
        {
            for (int pSkeleIndex = lockTo == -1 ? Skeleton->PartialSkeletonCount - 1 : lockTo; pSkeleIndex >= 0; pSkeleIndex--)
            {
                PartialSkeleton currentPartial = Skeleton->PartialSkeletons[pSkeleIndex];
                hkaPose* curPose = currentPartial.GetHavokPose(0);
                if (curPose == null) continue;

                hkaSkeleton* curPoseSkeleton = curPose->Skeleton;
                if (curPoseSkeleton == null) continue;
                int boneCount = curPoseSkeleton->Bones.Length;
                for (int currentBone = 0; currentBone < boneCount; currentBone++)
                {
                    if (curPoseSkeleton->Bones[currentBone].Name.String is string boneString && boneString != null && BoneName == boneString)
                    {
                        BoneDraw(drawListPtr, curPose, curPoseSkeleton, curPoseSkeleton->Bones[currentBone], currentBone);
                        return;
                    }
                }
            }
        }
        catch(Exception e) { PluginLog.Log(e.Message); blackened = true; }
    }

    public abstract void BoneDraw(ImDrawListPtr drawListPtr, hkaPose* currentPose, hkaSkeleton* curPoseSkeleton, hkaBone bone, int boneIndex);
}
