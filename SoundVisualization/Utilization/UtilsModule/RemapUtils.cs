using SoundVisualization.Core.Singleton;
using SoundVisualization.Utilization.Attributes;
using System.Collections.Generic;
using System.Numerics;

namespace SoundVisualization.Utilization.UtilsModule;

[UtilsDeclarable]
internal class RemapUtils : UtilsRegistryType, ISingletonBase<RemapUtils>
{
    public static RemapUtils instance { get; set; } = null!;

    public readonly Dictionary<string, SkeletonOffset[]> offsetSkeleton = new Dictionary<string, SkeletonOffset[]>()
    {
        { "j_ago", new SkeletonOffset[] {
                   new SkeletonOffset(Gender.Female, Tribe.SeekerOfTheSun, new Vector3(0.07f, -0.005f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.KeeperOfTheMoon, new Vector3(0.07f, -0.005f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.SeekerOfTheSun, new Vector3(0.09f, 0.014f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.KeeperOfTheMoon, new Vector3(0.09f, 0.014f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Plainsfolk, new Vector3(0.1f, -0.004f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Dunesfolk, new Vector3(0.1f, -0.004f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Duskwight, new Vector3(0.082f, 0.011f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Wildwood, new Vector3(0.082f, 0.011f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Duskwight, new Vector3(0.091f, 0.011f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Wildwood, new Vector3(0.091f, 0.011f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Highlander, new Vector3(0.091f, 0.013f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Midlander, new Vector3(0.091f, 0.013f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Highlander, new Vector3(0.088f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Midlander, new Vector3(0.088f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.SeaWolf, new Vector3(0.11f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Hellsguard, new Vector3(0.11f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.SeaWolf, new Vector3(0.135f, 0.007f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Hellsguard, new Vector3(0.135f, 0.007f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Raen, new Vector3(0.08f, 0.01f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Xaela, new Vector3(0.08f, 0.01f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Raen, new Vector3(0.12f, 0.012f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Xaela, new Vector3(0.12f, 0.012f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Helions, new Vector3(0.136f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.TheLost, new Vector3(0.136f, 0.02f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Rava, new Vector3(0.11f, 0.012f, 0)),
                   new SkeletonOffset(Gender.Female, Tribe.Veena, new Vector3(0.11f, 0.012f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Rava, new Vector3(0.095f, 0.012f, 0)),
                   new SkeletonOffset(Gender.Male, Tribe.Veena, new Vector3(0.095f, 0.012f, 0)),
        } }
    };

    public Vector3 GetOffset(string bone, Gender gender, Tribe tribe)
    {
        if (!offsetSkeleton.ContainsKey(bone)) return Vector3.Zero;
        foreach(SkeletonOffset offset in offsetSkeleton[bone])
            if (offset.gender == gender && offset.tribe == tribe) 
                return offset.offset;

        return Vector3.Zero;
    }
}

public struct SkeletonOffset
{
    public readonly Vector3 offset;
    public readonly Gender gender;
    public readonly Tribe tribe;

    public SkeletonOffset(Gender gender, Tribe tribe, Vector3 offset)
    {
        this.offset = offset;
        this.gender = gender;
        this.tribe = tribe;
    }
}

public enum Gender
{
    Male,
    Female
}

public enum Tribe
{
    Midlander = 1,
    Highlander = 2,
    Wildwood = 3,
    Duskwight = 4,
    Plainsfolk = 5,
    Dunesfolk = 6,
    SeekerOfTheSun = 7,
    KeeperOfTheMoon = 8,
    SeaWolf = 9,
    Hellsguard = 10,
    Raen = 11,
    Xaela = 12,
    Helions = 13,
    TheLost = 14,
    Rava = 15,
    Veena = 16
}
