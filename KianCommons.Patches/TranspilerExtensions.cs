using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace KianCommons.Patches;

internal static class TranspilerExtensions
{
	public static void InsertInstructions(this List<CodeInstruction> codes, int index, CodeInstruction[] insertion, bool moveLabels = true)
	{
		TranspilerUtils.InsertInstructions(codes, insertion, index, moveLabels);
	}

	public static void InsertInstructions(this List<CodeInstruction> codes, int index, IEnumerable<CodeInstruction> insertion, bool moveLabels = true)
	{
		TranspilerUtils.InsertInstructions(codes, insertion.ToArray(), index, moveLabels);
	}

	public static void InsertInstructions(this List<CodeInstruction> codes, int index, CodeInstruction insertion, bool moveLabels = true)
	{
		TranspilerUtils.InsertInstructions(codes, (CodeInstruction[])(object)new CodeInstruction[1] { insertion }, index, moveLabels);
	}

	public static void ReplaceInstruction(this List<CodeInstruction> codes, int index, CodeInstruction[] insertion)
	{
		TranspilerUtils.ReplaceInstructions(codes, insertion, index);
	}

	public static void ReplaceInstruction(this List<CodeInstruction> codes, int index, IEnumerable<CodeInstruction> insertion)
	{
		TranspilerUtils.ReplaceInstructions(codes, insertion.ToArray(), index);
	}

	public static void ReplaceInstruction(this List<CodeInstruction> codes, int index, CodeInstruction insertion)
	{
		TranspilerUtils.ReplaceInstructions(codes, (CodeInstruction[])(object)new CodeInstruction[1] { insertion }, index);
	}

	public static bool IsLdLoc(this CodeInstruction code, out int loc)
	{
		if (code.opcode == OpCodes.Ldloc_0)
		{
			loc = 0;
		}
		else if (code.opcode == OpCodes.Ldloc_1)
		{
			loc = 1;
		}
		else if (code.opcode == OpCodes.Ldloc_2)
		{
			loc = 2;
		}
		else if (code.opcode == OpCodes.Ldloc_3)
		{
			loc = 3;
		}
		else
		{
			if (!(code.opcode == OpCodes.Ldloc_S) && !(code.opcode == OpCodes.Ldloc))
			{
				loc = -1;
				return false;
			}
			if (code.operand is LocalBuilder localBuilder)
			{
				loc = localBuilder.LocalIndex;
			}
			else
			{
				loc = (int)code.operand;
			}
		}
		return true;
	}

	public static bool IsLdLoc(this CodeInstruction code, int loc)
	{
		if (!code.IsLdLoc(out var loc2))
		{
			return false;
		}
		return loc == loc2;
	}

	public static bool IsStLoc(this CodeInstruction code, out int loc)
	{
		if (code.opcode == OpCodes.Stloc_0)
		{
			loc = 0;
		}
		else if (code.opcode == OpCodes.Stloc_1)
		{
			loc = 1;
		}
		else if (code.opcode == OpCodes.Stloc_2)
		{
			loc = 2;
		}
		else if (code.opcode == OpCodes.Stloc_3)
		{
			loc = 3;
		}
		else
		{
			if (!(code.opcode == OpCodes.Stloc_S) && !(code.opcode == OpCodes.Stloc))
			{
				loc = -1;
				return false;
			}
			if (code.operand is LocalBuilder localBuilder)
			{
				loc = localBuilder.LocalIndex;
			}
			else
			{
				loc = (int)code.operand;
			}
		}
		return true;
	}

	public static bool IsStLoc(this CodeInstruction code, int loc)
	{
		if (!code.IsStLoc(out var loc2))
		{
			return false;
		}
		return loc == loc2;
	}

	public static bool IsLdLoc(this CodeInstruction code, Type type, MethodBase method)
	{
		if (!CodeInstructionExtensions.IsLdloc(code, (LocalBuilder)null))
		{
			return false;
		}
		if (code.opcode == OpCodes.Ldloc_0)
		{
			return method.GetMethodBody().LocalVariables[0].LocalType == type;
		}
		if (code.opcode == OpCodes.Ldloc_1)
		{
			return method.GetMethodBody().LocalVariables[1].LocalType == type;
		}
		if (code.opcode == OpCodes.Ldloc_2)
		{
			return method.GetMethodBody().LocalVariables[2].LocalType == type;
		}
		if (code.opcode == OpCodes.Ldloc_3)
		{
			return method.GetMethodBody().LocalVariables[3].LocalType == type;
		}
		return code.operand is LocalBuilder localBuilder && localBuilder.LocalType == type;
	}

	public static bool IsLdLocA(this CodeInstruction code, Type type, out int loc)
	{
		if ((code.opcode == OpCodes.Ldloca || code.opcode == OpCodes.Ldloca_S) && code.operand is LocalBuilder localBuilder && localBuilder.LocalType == type)
		{
			loc = localBuilder.LocalIndex;
			return true;
		}
		loc = -1;
		return false;
	}

	public static bool IsStLoc(this CodeInstruction code, Type type, MethodBase method)
	{
		if (!CodeInstructionExtensions.IsStloc(code, (LocalBuilder)null))
		{
			return false;
		}
		if (code.opcode == OpCodes.Stloc_0)
		{
			return method.GetMethodBody().LocalVariables[0].LocalType == type;
		}
		if (code.opcode == OpCodes.Stloc_1)
		{
			return method.GetMethodBody().LocalVariables[1].LocalType == type;
		}
		if (code.opcode == OpCodes.Stloc_2)
		{
			return method.GetMethodBody().LocalVariables[2].LocalType == type;
		}
		if (code.opcode == OpCodes.Stloc_3)
		{
			return method.GetMethodBody().LocalVariables[3].LocalType == type;
		}
		return code.operand is LocalBuilder localBuilder && localBuilder.LocalType == type;
	}

	public static int GetLoc(this CodeInstruction code)
	{
		int num = -1;
		if (code.opcode == OpCodes.Ldloc_0 || code.opcode == OpCodes.Stloc_0)
		{
			return 0;
		}
		if (code.opcode == OpCodes.Ldloc_1 || code.opcode == OpCodes.Stloc_1)
		{
			return 1;
		}
		if (code.opcode == OpCodes.Ldloc_2 || code.opcode == OpCodes.Stloc_2)
		{
			return 2;
		}
		if (code.opcode == OpCodes.Ldloc_3 || code.opcode == OpCodes.Stloc_3)
		{
			return 3;
		}
		if (code.opcode == OpCodes.Ldloc_S || code.opcode == OpCodes.Ldloc || code.opcode == OpCodes.Ldloca_S || code.opcode == OpCodes.Ldloca || code.opcode == OpCodes.Stloc_S || code.opcode == OpCodes.Stloc)
		{
			if (!(code.operand is LocalBuilder { LocalIndex: var localIndex }))
			{
				return (int)code.operand;
			}
			return localIndex;
		}
		throw new Exception($"{code} is not stloc, ldloc or ldlocA");
	}

	public static bool LoadsConstant(this CodeInstruction code, string value)
	{
		return code.opcode == OpCodes.Ldstr && code.operand is string text && text == value;
	}

	public static bool Calls(this CodeInstruction code, string method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (code.opcode != OpCodes.Call && code.opcode != OpCodes.Callvirt)
		{
			return false;
		}
		return (code.operand as MethodBase)?.Name == method;
	}

	public static bool Calls(this CodeInstruction code, MethodBase method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (code.opcode != OpCodes.Call && code.opcode != OpCodes.Callvirt)
		{
			return false;
		}
		return object.Equals(code.operand, method);
	}
}
