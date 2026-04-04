using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using KianCommons;
using KianCommons.Patches;

namespace HideUnconnectedTracks.Patches.NetNodePatches;

[HarmonyPatch]
public static class RenderInstance
{
	private static string _logprefix = "NetNode.RenderInstance.Transpiler: ";

	private static bool VERBOSE => Log.VERBOSE;

	private static MethodInfo Target => typeof(NetNode).GetMethod("RenderInstance", BindingFlags.Instance | BindingFlags.NonPublic);

	private static MethodBase TargetMethod()
	{
		MethodInfo target = Target;
		Assertion.Assert(target != null, "did not manage to find original function to patch");
		if (VERBOSE)
		{
		}
		return target;
	}

	public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
	{
		try
		{
			List<CodeInstruction> list = instructions.ToCodeList();
			CheckTracksCommons.ApplyCheckTracks(list, original, 1);
			return list;
		}
		catch (Exception ex)
		{
			if (VERBOSE)
			{
				Log.Error(ex.ToString(), copyToGameLog: false);
			}
			throw ex;
		}
	}
}
