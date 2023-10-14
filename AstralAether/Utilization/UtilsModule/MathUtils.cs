using AstralAether.Core.Singleton;
using AstralAether.Utilization.Attributes;
using System;

namespace AstralAether.Utilization.UtilsModule;

[UtilsDeclarable]
internal class MathUtils : UtilsRegistryType, ISingletonBase<MathUtils>
{
    public const float deg2rad = (float)(Math.PI / 180.0);
    public const float rad2deg = (float)(180.0 / Math.PI);
    public static MathUtils instance { get; set; } = null!;

    public float Map(float value, float min, float max, float newMin, float newMax)
    {
        return newMin + (newMax - newMin) * ((value - min) / (max - min));
    }
}
