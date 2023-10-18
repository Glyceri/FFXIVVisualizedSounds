namespace SoundVisualization.Core.Singleton;

internal interface ISingletonBase<T> 
{
    public abstract static T instance { get; set; }
}
