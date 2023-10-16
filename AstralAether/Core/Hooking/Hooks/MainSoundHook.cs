using AstralAether.Core.Hooking.Attributes;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using System.Runtime.InteropServices;
using System;
using static AstralAether.Core.Hooking.Delegates;
using System.Numerics;
using AstralAether.Utilization.UtilsModule;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using AstralAether.Core.Sound;
using AstralAether.Core.Sound.SoundTypes.FootstepSoundTypes;
using AstralAether.Core.Sound.SoundTypes;
using System.Globalization;
using AstralAether.Core.Sound.SoundTypes.BaseTypes;
using AstralAether.Core.Handlers;
using AstralAether.Windows.Windows;
using AstralAether.Windows.AudioModules;

namespace AstralAether.Core.Hooking.Hooks;

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
            GameObject* lastGameObject = SoundStorage.LastApricot!;
            SoundType soundType = SoundParser.ParseSound(path);
            if (lastGameObject == null)
            {
                // Get other game objects here depending on type
                GetForSoundType(ref soundType, ref lastGameObject);
            }

            if (soundType is BaseFootstepSoundType && lastGameObject != null) PluginLink.WindowHandler.GetWindow<AudioWindow>().audioModules.Add(new StaticCircleModule(0.4, new Vector4(0.4f, 0.4f, 1.0f, 1.0f), position, 60 * lastGameObject->HitboxRadius));

            PrintSoundType(ref soundType, position, lastGameObject);
        }
        catch (Exception e) { PluginLog.Log(e.Message); }

        SoundStorage.ClearAll();
        return playAudioSourceHook!.Original(a1, fileName, a3, a4, posXAsHex, posYAsHex, posZAsHex, a8, a9, a10, a11, a12, a13, a14, a15, a16);
    }

    unsafe void GetForSoundType(ref SoundType soundType, ref GameObject* gameObject) 
    {
        if (soundType is BaseBattleSoundType)       gameObject = SoundStorage.LastBattleSound;
        if (soundType is BaseFootstepSoundType)     gameObject = SoundStorage.LastFootstep;
    }

    unsafe void PrintSoundType(ref SoundType soundType, Vector3 position, GameObject* gameObject)
    {
        string posString = $"[X:{position.X.ToString("0.00", CultureInfo.InvariantCulture)}, Y:{position.Y.ToString("0.00", CultureInfo.InvariantCulture)}, Z:{position.Z.ToString("0.00", CultureInfo.InvariantCulture)}]";
        string gameObjectName = "[]";
        if (gameObject != null) gameObjectName = $"[{Marshal.PtrToStringUTF8((IntPtr)gameObject->GetName())}]";
        PluginLog.LogWarning(string.Format("{0,-15}{1,-15} {2,-32}", soundType.ToString(), posString, gameObjectName));
    }

}
