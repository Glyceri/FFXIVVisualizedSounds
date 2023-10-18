using static AstralAether.Core.Hooking.Delegates;
using AstralAether.Core.Hooking.Attributes;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System;
using Dalamud.Logging;

namespace AstralAether.Core.Hooking.Hooks;

[Hook]
public unsafe class ResourceRequestHook : HookableElement
{
    [Signature("E8 ?? ?? 00 00 48 8D 8F ?? ?? 00 00 48 89 87 ?? ?? 00 00", DetourName = nameof(GetResourceSyncHandler))]
    readonly Hook<GetResourceSync>? getResourceSyncHook;

    [Signature("E8 ?? ?? ?? 00 48 8B D8 EB ?? F0 FF 83 ?? ?? 00 00", DetourName = nameof(GetResourceAsyncHandler))]
    readonly Hook<GetResourceAsync>? getResourceAsyncHook;

    internal override void OnInit()
    {
        getResourceSyncHook?.Enable();
        getResourceAsyncHook?.Enable();
    }

    internal override void OnDispose()
    {
        getResourceSyncHook?.Dispose();
        getResourceAsyncHook?.Dispose();
    }

    void* GetResourceSyncHandler(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash, byte* path, nint resParams)
    {
        Handle(path, false);
        return getResourceSyncHook!.Original(resourceManager, categoryId, resourceType, resourceHash, path, resParams);
    }

    void* GetResourceAsyncHandler(IntPtr resourceManager, uint* categoryId, ResourceType* resourceType, int* resourceHash, byte* path, nint resParams, bool isUnknown)
    {
        Handle(path, true);
        return getResourceAsyncHook!.Original(resourceManager, categoryId, resourceType, resourceHash, path, resParams, isUnknown);
    }

    void Handle(byte* path, bool asAsync)
    {
        string p = Marshal.PtrToStringUTF8((IntPtr)path)!;
        if (!p.EndsWith(".scd")) return;
       //PluginLog.LogError($"[{asAsync}] {p}");
    }
}
