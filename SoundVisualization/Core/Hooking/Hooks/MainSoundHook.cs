using SoundVisualization.Core.Hooking.Attributes;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using System.Runtime.InteropServices;
using System;
using static SoundVisualization.Core.Hooking.Delegates;
using System.Numerics;
using SoundVisualization.Utilization.UtilsModule;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using SoundVisualization.Core.Sound;
using System.Globalization;
using SoundVisualization.Core.Sound.SoundTypes.BaseTypes;
using SoundVisualization.Core.Handlers;
using SoundVisualization.Windows.Windows;
using SoundVisualization.Windows.AudioModules;
using SoundVisualization.Core.Sound.SoundTypes.FootstepSoundTypes;
using SoundVisualization.Core.Sound.SoundTypes;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

namespace SoundVisualization.Core.Hooking.Hooks;

[Hook]
internal class MainSoundHook : HookableElement
{
    [Signature("E8 ?? ?? ?? ?? 48 8B 7D B0", DetourName = nameof(PlayAudioSourceDetour))]
    readonly Hook<PlayAudioSource>? playAudioSourceHook;

    internal override void OnInit()
    {
        playAudioSourceHook?.Enable();
    }

    internal override void OnDispose()
    {
        playAudioSourceHook?.Dispose();
    }

    unsafe IntPtr PlayAudioSourceDetour(IntPtr a1, IntPtr fileName, float a3, int a4, int posXAsHex, int posYAsHex, int posZAsHex, int a8, int a9, int a10, byte a11, uint a12, char a13, int a14, char a15, char a16)
    {
        try
        {
            string path = Marshal.PtrToStringUTF8(fileName)!;
            Vector3 position = MathUtils.instance.MakeFromSLODWORDPOS(posXAsHex, posYAsHex, posZAsHex);
            SoundType soundType = SoundParser.ParseSound(path);
            GetForSoundType(ref soundType, out GameObject* lastGameObject);

            PrintSoundType(ref soundType, position, lastGameObject);
            if ((soundType is BaseFootstepSoundType || SoundStorage.footstepCount > 0) && lastGameObject != null)
            {
                int minusMe = soundType is BaseFootstepSoundType ? 1 : 2;
                if (soundType is not BaseFootstepSoundType) SoundStorage.footstepCount -= minusMe;
                PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.4, new Vector4(0.4f, 0.4f, 1.0f, 1.0f), position, 60 * lastGameObject->HitboxRadius));
            }
            if (soundType is VoEmoteSoundType && lastGameObject != null) PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new PlayerSpeechModule(3.0, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), (BattleChara*)lastGameObject, Vector3.Zero, "j_ago"));

            //if (soundType is UnimplementedSoundType) PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(10.0, new Vector4(0, 1, 1, 1), position, 200, 12, 6));
            //if (soundType is BaseBattleSoundType) PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(10.0, new Vector4(1, 0, 1, 1), position, 200, 12, 6));
        }
        catch (Exception e) { PluginLog.Log(e.Message); }

        return playAudioSourceHook!.Original(a1, fileName, a3, a4, posXAsHex, posYAsHex, posZAsHex, a8, a9, a10, a11, a12, a13, a14, a15, a16);
    }

    unsafe GameObject* GetForSoundType(ref SoundType soundType, out GameObject* gameObject) 
    {
        return gameObject = SoundStorage.LastAccessedGameObject;
    }

    unsafe void PrintSoundType(ref SoundType soundType, Vector3 position, GameObject* gameObject)
    {
        return;
        string posString = $"[X:{position.X.ToString("0.00", CultureInfo.InvariantCulture)}, Y:{position.Y.ToString("0.00", CultureInfo.InvariantCulture)}, Z:{position.Z.ToString("0.00", CultureInfo.InvariantCulture)}]";
        string gameObjectName = "[]";
        if (gameObject != null) gameObjectName = $"[{Marshal.PtrToStringUTF8((IntPtr)gameObject->GetName())}]";
        else gameObjectName = "[NULL GAMEOBJECT]";
        int gameObjectIndex = -1;
        if (gameObject != null) gameObjectIndex = gameObject->ObjectIndex;
        string finalString = string.Format("{2,-6} {3,-32} {1,-40} {0,-15}", soundType.ToString(), posString, $"[{gameObjectIndex}]", gameObjectName);
        if (soundType is UnimplementedSoundType or InvalidSoundType or UnparsedSoundType || gameObject == null) PluginLog.LogError(finalString);
        else PluginLog.LogWarning(finalString);
    }

}
