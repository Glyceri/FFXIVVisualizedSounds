using AstralAether.Core.Singleton;
using AstralAether.Utilization.Attributes;

namespace AstralAether.Utilization.UtilsModule;

[UtilsDeclarable]
internal class MathUtils : UtilsRegistryType, ISingletonBase<MathUtils>
{
    public static MathUtils instance { get; set; } = null!;

    public float Map(float value, float min, float max, float newMin, float newMax)
    {
        return newMin + (newMax - newMin) * ((value - min) / (max - min));
    }
}
