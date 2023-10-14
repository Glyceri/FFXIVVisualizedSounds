using AstralAether.Utilization.UtilsModule;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace AstralAether.Windows.AudioModules.Base;

public unsafe abstract class DynamicAudioModule : AudioModule
{
    readonly Vector3 offset;
    public Vector3 Offset => offset;

    readonly GameObject* followMe;
    public GameObject* FollowMe => followMe;

    public float HitboxRadius => followMe->HitboxRadius;

    public Vector3 FollowedPosition => followMe->Position;

    public Matrix4x4 RotationMatrix => Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromEuler(new Vector3(0, followMe->Rotation * MathUtils.rad2deg, 0)));

    public DynamicAudioModule(double timer, Vector4 colour, GameObject* followMe, Vector3 offset) : base(timer, colour)
    {
        this.followMe = followMe;
        this.offset = offset;
    }

    public override bool Tick(double time)
    {
        if (followMe == null) return true;
        return base.Tick(time);
    }

    public override abstract void Draw(ImDrawListPtr drawListPtr);
}
