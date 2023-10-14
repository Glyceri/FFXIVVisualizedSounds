using AstralAether.Core.Handlers;
using AstralAether.Utilization.UtilsModule;
using AstralAether.Windows.Attributes;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Logging;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Common.Math;
using FFXIVClientStructs.Havok;
using FFXIVClientStructs.Interop;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AstralAether.Windows.Windows;

[PersistentAstralAetherWindow]
internal class AudioWindow : AstralAetherWindow
{
    public List<TempFootstep> footSteps = new List<TempFootstep>();

    public AudioWindow() : base("Audio Window")
    {
        SizeConstraints = new WindowSizeConstraints()
        {
            MaximumSize = new Vector2(500, 90),
            MinimumSize = new Vector2(500, 90),
        };

        Flags |= ImGuiWindowFlags.NoMove;
        Flags |= ImGuiWindowFlags.NoBackground;
        Flags |= ImGuiWindowFlags.NoInputs;
        Flags |= ImGuiWindowFlags.NoNavFocus;
        Flags |= ImGuiWindowFlags.NoResize;
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoTitleBar;
        Flags |= ImGuiWindowFlags.NoDecoration;
        Flags |= ImGuiWindowFlags.NoFocusOnAppearing;

        ForceMainWindow = true;

        IsOpen = true;
    }

    public unsafe override void OnDraw()
    {
       
        foreach(TempFootstep footstep in footSteps)
        {
            List<Line> lines = new List<Line>();

            Vector3 basePos = Vector3.Zero;
            if (footstep.gObject != null)
            {
                // HOLY FOCKING CHEERS MATE :DD https://github.com/XIV-Tools/CustomizePlus/blob/main/CustomizePlus/Data/Armature/Armature.cs#L321
                CharacterBase* charaBase = (CharacterBase*)footstep.gObject->GetDrawObject();
                if (charaBase == null) continue;

                System.Numerics.Matrix4x4 matrix = System.Numerics.Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromEuler(new Vector3(0, footstep.gObject->Rotation * (float)(180.0 / Math.PI), 0)));
                

                try
                {
                    Skeleton* curSkeleton = charaBase->Skeleton;
                    if (curSkeleton == null) continue;
                    for (int pSkeleIndex = 0; pSkeleIndex < curSkeleton->PartialSkeletonCount; ++pSkeleIndex)
                    {
                        PartialSkeleton currentPartial = curSkeleton->PartialSkeletons[pSkeleIndex];
                        hkaPose* curPose = currentPartial.GetHavokPose(0);

                        if (curPose == null) continue;

                        hkaSkeleton* curPoseSkeleton = curPose->Skeleton;
                        int boneCount = curPoseSkeleton->Bones.Length;

                        for (int b = 0; b < boneCount; ++b)
                        {
                            if (curPoseSkeleton->Bones[b].Name.String is string boneName && boneName != null && (boneName == "j_f_dlip_b" || boneName == "j_m_dlip_b"))
                            {
                                hkQsTransformf transform = curPose->ModelPose.Data[b];
                                Character* c = (Character*)footstep.gObject;

                                //Race race = SheetUtils.instance.GetRace(c->DrawData.CustomizeData[(int)CustomizeIndex.Race])!;
                                Tribe tribe = SheetUtils.instance.GetTribe(c->DrawData.CustomizeData[(int)CustomizeIndex.Tribe])!;
                                int height = c->DrawData.CustomizeData[(int)CustomizeIndex.Height];
                                
                                Gender gender = (Gender)c->DrawData.CustomizeData[(int)CustomizeIndex.Gender];

                                //PluginLog.Log($"Race: {(gender == 0 ? tribe.Masculine : tribe.Feminine)}, Height: {height}, Gender: {gender}");

                                // Highlander   : 1
                                // Midlander    : 2
                                // Elezen       : 3, 4
                                // Lalafel      : 5, 6
                                // Miqote       : 7, 8
                                // Roegadyn     : 9, 10
                                // Au Ra        : 11, 12
                                // Hrothgar     : 13, 14
                                // Viera        : 15, 16


                                float outcome = (tribe.RowId, gender) switch
                                {
                                    (7, Gender.Female) => MathUtils.instance.Map(height, 0, 100, 0.96f, 1.04f),
                                    (8, Gender.Female) => MathUtils.instance.Map(height, 0, 100, 0.96f, 1.04f),
                                    (11, Gender.Female) => MathUtils.instance.Map(height, 0, 100, 0.925f, 1f),
                                    (12, Gender.Female) => MathUtils.instance.Map(height, 0, 100, 0.925f, 1f),
                                    _ => 1
                                };

                                Vector3 translated = Vector3.Transform(new Vector3(transform.Translation.X, transform.Translation.Y, transform.Translation.Z) * outcome, matrix);
                                //* 1.040f, matrix);

                                Vector3 end = Vector3.Transform(new Vector3(0, 0, 0.05f), matrix);
                                Vector3 start = Vector3.Transform(new Vector3(0, 0, 0.02f), matrix);

                                Vector3 camVec = CameraManager.Instance()->CurrentCamera->Vector_1;
                                camVec.Y = 0;
                                camVec = camVec.Normalized;

                                Vector3 endVec = end;
                                endVec.Y = 0;
                                endVec = endVec.Normalized;

                                float dot = Vector3.Dot(camVec, endVec);
                                PluginLog.Log(dot.ToString());
                                //if (dot > -0.95f && dot < 0.95f)
                                {

                                    lines.Add(new Line(footstep.gObject->Position + translated + start, footstep.gObject->Position + translated + end));

                                    lines.Add(new Line(footstep.gObject->Position + translated + start + new Vector3(0, 0.01f, 0), footstep.gObject->Position + translated + end + new Vector3(0, 0.015f, 0)));

                                    lines.Add(new Line(footstep.gObject->Position + translated + start - new Vector3(0, 0.01f, 0), footstep.gObject->Position + translated + end - new Vector3(0, 0.015f, 0)));
                                }
                                break;
                            }
                        }
                        

                        

                        /*
                        hkaSkeleton* curPoseSkeleton = curPose->Skeleton;
                        PluginLog.Log(curPoseSkeleton->Name.String!);
                        for (int boneIndex = 0; boneIndex < curPoseSkeleton->Bones.Length; boneIndex++)
                        {
                            if (curPoseSkeleton->Bones[boneIndex].Name.String is string boneName && boneName != null)
                            {
                                PluginLog.Log("-----" + boneName);
                            }
                        }*/
                    }
                    


                }
                catch(Exception e) 
                { 
                    PluginLog.Log(e.Message); 
                }
                
                //basePos = footstep.gObject->Position;
            }

            

            
           

            ImDrawListPtr drawlist = ImGui.GetBackgroundDrawList();
            if (footstep.gObject != null)
            {
                uint colour = Color(new Vector4(1, 0.4f, 0.4f, 1));

                int counter = 0;
                foreach(Line line in lines)
                {
                    drawlist.AddLine(WorldToScreen(line.start), WorldToScreen(line.end), colour, 5f);
                    counter++;
                }
            } else {
                Vector2 screenPos = WorldToScreen(basePos + footstep.position);
                if (screenPos == Vector2.Zero) continue;

                float size = footstep.startSize * 60 * (float)footstep.timer;
                if (size > 18)
                    size = 18;
                drawlist.AddCircle(screenPos, size, Color(new Vector4(0.4f, 0.4f, 1, 1)), 12, 3f);
            }
        }
    }

    public Vector2 WorldToScreen(Dalamud.Game.ClientState.Objects.Types.GameObject? obj)
    {
        if (obj == null) return Vector2.Zero;
        return PluginHandlers.GameGui.WorldToScreen(obj.Position, out var screenPos) ? screenPos : Vector2.Zero;
    }

    public Vector2 WorldToScreen(Vector3 pos)
    {
        return PluginHandlers.GameGui.WorldToScreen(pos, out var screenPos) ? screenPos : Vector2.Zero;
    }

    public static uint Color(Vector4 color)
    {
        uint ret = (byte)(color.W * 255);
        ret <<= 8;
        ret += (byte)(color.Z * 255);
        ret <<= 8;
        ret += (byte)(color.Y * 255);
        ret <<= 8;
        ret += (byte)(color.X * 255);
        return ret;
    }
}

public class TempFootstep 
{
    public Vector3 position;
    public double timer;
    public float startSize;
    public unsafe GameObject* gObject;

    public TempFootstep(Vector3 position, float timer, float startSize)
    {
        this.position = position;
        this.timer = timer;
        this.startSize = startSize;
    }

    public unsafe TempFootstep(GameObject* gObject, Vector3 offset, float timer, float startSize)
    {
        this.gObject = gObject;
        this.position = offset;
        this.timer = timer;
        this.startSize = startSize;
    }
}

public struct Line
{
    public Vector3 start;
    public Vector3 end;
    public Line(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }
}

public enum Gender 
{ 
    Male,
    Female
}
