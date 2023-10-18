using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace AstralAether.Core.Sound;

public static unsafe class SoundStorage
{
    public static GameObject* LastAccessedGameObject { get; private set; } = null!;
    public static void RegisterLastAccessedGameObject(GameObject* lastAccessedGameObject) => LastAccessedGameObject = lastAccessedGameObject;
    public static int footstepCount = 0;
}
