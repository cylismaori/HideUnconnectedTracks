using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using HideUnconnectedTracks.Utils;
using KianCommons;
using KianCommons.Patches;
using UnityEngine;

namespace HideUnconnectedTracks.Patches;

public static class CheckTracksCommons
{
	private static MethodInfo mShouldConnectTracks => typeof(CheckTracksCommons).GetMethod("ShouldConnectTracks", true);

	private static MethodInfo mCheckRenderDistance => typeof(RenderManager.CameraInfo).GetMethod("CheckRenderDistance", true);

	private static FieldInfo fNodes => typeof(NetInfo).GetField("m_nodes") ?? throw new Exception("fNodes is null");

	private static FieldInfo fDataVector0 => typeof(RenderManager.Instance).GetField("m_dataVector0") ?? throw new Exception("fDataVector0 is null");

	public static bool ShouldConnectTracks(ushort nodeId, RenderManager.Instance data, NetInfo.Node nodeInfo, Vector4 dataVector0)
	{
		ushort segment = ((NetNode)(nodeId.ToNode())).GetSegment(data.m_dataInt0 & 7);
		int num = data.m_dataInt0 >> 4;
		ushort segment2 = ((NetNode)(nodeId.ToNode())).GetSegment(num);
		if (TMPEUtil.Exists)
		{
			try
			{
				bool flipMesh;
				bool result = DirectConnectUtil.DetermineDirectConnect(segment, segment2, nodeId, nodeInfo, out flipMesh);
				if (flipMesh)
				{
					dataVector0.x = 0f - dataVector0.x;
					dataVector0.y = 0f - dataVector0.y;
				}
				return result;
			}
			catch (Exception ex)
			{
				ex.Log(showInPannel: false);
				throw;
			}
		}
		return true;
	}


	public static void ApplyCheckTracks(List<CodeInstruction> codes, MethodBase method, int occurance)
	{
		int startIndex = 0;
		startIndex = codes.Search((CodeInstruction _c) => _c.Calls(mCheckRenderDistance), startIndex, occurance);
		Assertion.Assert(startIndex != 0, "index!=0");
		CodeInstruction val = Build_LDLocA_NodeInfo(codes, startIndex, 1, -1);
		CodeInstruction val2 = Build_LDLocA_DataVector0(codes, startIndex, 1, -1);
		int loc_cameraInfo = method.GetArgLoc("cameraInfo");
		startIndex = codes.Search((CodeInstruction _c) => CodeInstructionExtensions.IsLdarg(_c, (int?)loc_cameraInfo), startIndex, -1);
		Label? continueIndex = null;
		codes.Search((CodeInstruction _c) => CodeInstructionExtensions.Branches(_c, out continueIndex), startIndex, -1);
		CodeInstruction[] insertion = (CodeInstruction[])(object)new CodeInstruction[6]
		{
			TranspilerUtils.GetLDArg(method, "nodeID"),
			TranspilerUtils.GetLDArg(method, "data"),
			val,
			val2,
			new CodeInstruction(OpCodes.Call, (object)mShouldConnectTracks),
			new CodeInstruction(OpCodes.Brfalse, (object)continueIndex)
		};
		TranspilerUtils.InsertInstructions(codes, insertion, startIndex);
	}

	public static CodeInstruction Build_LDLocA_NodeInfo(List<CodeInstruction> codes, int index, int counter, int dir)
	{
        index = TranspilerUtils.SearchGeneric(codes, idx => TranspilerUtils.IsSameInstruction(codes[idx], new CodeInstruction(OpCodes.Ldfld, (object)fNodes)), index, dir, counter);
        //index = TranspilerUtils.SearchInstruction(codes, new CodeInstruction(OpCodes.Ldfld, (object)fNodes), index, dir, counter);
        CodeInstruction val = codes[index + 3];
		Assertion.Assert(CodeInstructionExtensions.IsStloc(val, (LocalBuilder)null), $"IsStLoc(code) | code={val}");
		return new CodeInstruction(OpCodes.Ldloca_S, val.operand);
	}

	public static CodeInstruction Build_LDLocA_DataVector0(List<CodeInstruction> codes, int index, int counter, int dir)
	{
        index = TranspilerUtils.SearchGeneric(codes, idx => TranspilerUtils.IsSameInstruction(codes[idx], new CodeInstruction(OpCodes.Ldfld, (object)fDataVector0)), index, dir, counter);
        //index = TranspilerUtils.SearchInstruction(codes, new CodeInstruction(OpCodes.Ldfld, (object)fDataVector0), index, dir, counter);
		CodeInstruction val = codes[index + 1];
		Assertion.Assert(CodeInstructionExtensions.IsStloc(val, (LocalBuilder)null), $"IsStLoc(code) | code={val}");
		return new CodeInstruction(OpCodes.Ldloca_S, val.operand);
	}
}
