using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace KianCommons.Patches;

public static class TranspilerUtils
{
	public class InstructionNotFoundException : Exception
	{
		public InstructionNotFoundException()
		{
		}

		public InstructionNotFoundException(string m)
			: base(m)
		{
		}
	}

	public const BindingFlags ALL = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

	public static int PeekBefore = 10;

	public static int PeekAfter = 15;

	private static bool VERBOSE => KianCommons.Log.VERBOSE;

	private static void Log(object message)
	{
		KianCommons.Log.Info("TRANSPILER " + message);
	}

	public static string FullName(MethodBase m)
	{
		return m.DeclaringType.FullName + "::" + m.Name;
	}

	internal static Type[] GetParameterTypes<TDelegate>(bool instance = false) where TDelegate : Delegate
	{
		IEnumerable<Type> source = from p in typeof(TDelegate).GetMethod("Invoke").GetParameters()
			select p.ParameterType;
		if (instance)
		{
			source = source.Skip(1);
		}
		return source.ToArray();
	}

	internal static MethodInfo DeclaredMethod<TDelegate>(Type type, bool throwOnError = true, bool instance = false) where TDelegate : Delegate
	{
		return DeclaredMethod<TDelegate>(type, typeof(TDelegate).Name, throwOnError, instance);
	}

	internal static MethodInfo DeclaredMethod<TDelegate>(Type type, string name, bool throwOnError = false, bool instance = false) where TDelegate : Delegate
	{
		return type.GetMethod(name, GetParameterTypes<TDelegate>(instance), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty, throwOnError);
	}

	[Obsolete("use reflection helpers instead")]
	internal static MethodInfo GetMethod(Type type, string name)
	{
		return type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty) ?? throw new Exception("Method not found: " + type.Name + "." + name);
	}

	internal static MethodInfo GetCoroutineMoveNext(Type declaringType, string name)
	{
		try
		{
			Type type = declaringType.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).Single((Type _t) => _t.Name.Contains("<" + name + ">"));
			return ReflectionHelpers.GetMethod(type, "MoveNext");
		}
		catch (Exception ex)
		{
			IEnumerable<string> list = (from _t in declaringType?.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty)
				where _t.Name.Contains("<" + name + ">")
				select _t.FullName);
			ex.Exception("the following types contain '<" + name + ">': " + ((IEnumerable)list).ToSTR());
			return null;
		}
	}

	public static List<CodeInstruction> ToCodeList(this IEnumerable<CodeInstruction> instructions)
	{
		List<CodeInstruction> collection = new List<CodeInstruction>(instructions);
		return new List<CodeInstruction>(collection);
	}

	public static CodeInstruction GetLDArg(MethodBase method, string argName, bool throwOnError = true)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		if (!throwOnError && !HasParameter(method, argName))
		{
			return null;
		}
		byte argLoc = method.GetArgLoc(argName);
		return (CodeInstruction)(argLoc switch
		{
			0 => (object)new CodeInstruction(OpCodes.Ldarg_0, (object)null), 
			1 => (object)new CodeInstruction(OpCodes.Ldarg_1, (object)null), 
			2 => (object)new CodeInstruction(OpCodes.Ldarg_2, (object)null), 
			3 => (object)new CodeInstruction(OpCodes.Ldarg_3, (object)null), 
			_ => (object)new CodeInstruction(OpCodes.Ldarg_S, (object)argLoc), 
		});
	}

	public static byte GetArgLoc(this MethodBase method, string argName)
	{
		byte b = GetParameterLoc(method, argName);
		if (!method.IsStatic)
		{
			b++;
		}
		return b;
	}

	public static bool IsLdarg(this CodeInstruction code, MethodBase method, string argName)
	{
		byte argLoc = method.GetArgLoc(argName);
		return CodeInstructionExtensions.IsLdarg(code, (int?)argLoc);
	}

	public static byte GetParameterLoc(MethodBase method, string name)
	{
		ParameterInfo[] parameters = method.GetParameters();
		for (byte b = 0; b < parameters.Length; b++)
		{
			if (parameters[b].Name == name)
			{
				return b;
			}
		}
		throw new Exception("did not found parameter with name:<" + name + ">");
	}

	public static bool HasParameter(MethodBase method, string name)
	{
		ParameterInfo[] parameters = method.GetParameters();
		foreach (ParameterInfo parameterInfo in parameters)
		{
			if (parameterInfo.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsSameInstruction(this CodeInstruction a, CodeInstruction b)
	{
		if (a.opcode == b.opcode)
		{
			if (a.operand == b.operand)
			{
				return true;
			}
			return a.operand is byte b2 && b.operand is byte b3 && b2 == b3;
		}
		return false;
	}

	[Obsolete("use harmony extension instead")]
	public static bool IsLdLoc(CodeInstruction instruction)
	{
		return instruction.opcode == OpCodes.Ldloc_0 || instruction.opcode == OpCodes.Ldloc_1 || instruction.opcode == OpCodes.Ldloc_2 || instruction.opcode == OpCodes.Ldloc_3 || instruction.opcode == OpCodes.Ldloc_S || instruction.opcode == OpCodes.Ldloc;
	}

	[Obsolete("use harmony extension instead")]
	public static bool IsStLoc(CodeInstruction instruction)
	{
		return instruction.opcode == OpCodes.Stloc_0 || instruction.opcode == OpCodes.Stloc_1 || instruction.opcode == OpCodes.Stloc_2 || instruction.opcode == OpCodes.Stloc_3 || instruction.opcode == OpCodes.Stloc_S || instruction.opcode == OpCodes.Stloc;
	}

	public static CodeInstruction BuildLdLocFromStLoc(this CodeInstruction instruction)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		if (instruction.opcode == OpCodes.Stloc_0)
		{
			return new CodeInstruction(OpCodes.Ldloc_0, (object)null);
		}
		if (instruction.opcode == OpCodes.Stloc_1)
		{
			return new CodeInstruction(OpCodes.Ldloc_1, (object)null);
		}
		if (instruction.opcode == OpCodes.Stloc_2)
		{
			return new CodeInstruction(OpCodes.Ldloc_2, (object)null);
		}
		if (instruction.opcode == OpCodes.Stloc_3)
		{
			return new CodeInstruction(OpCodes.Ldloc_3, (object)null);
		}
		if (instruction.opcode == OpCodes.Stloc_S)
		{
			return new CodeInstruction(OpCodes.Ldloc_S, instruction.operand);
		}
		if (instruction.opcode == OpCodes.Stloc)
		{
			return new CodeInstruction(OpCodes.Ldloc, instruction.operand);
		}
		throw new Exception("instruction is not stloc! : " + (object)instruction);
	}

	public static CodeInstruction BuildStLocFromLdLoc(this CodeInstruction instruction)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		if (instruction.opcode == OpCodes.Ldloc_0)
		{
			return new CodeInstruction(OpCodes.Stloc_0, (object)null);
		}
		if (instruction.opcode == OpCodes.Ldloc_1)
		{
			return new CodeInstruction(OpCodes.Stloc_1, (object)null);
		}
		if (instruction.opcode == OpCodes.Ldloc_2)
		{
			return new CodeInstruction(OpCodes.Stloc_2, (object)null);
		}
		if (instruction.opcode == OpCodes.Ldloc_3)
		{
			return new CodeInstruction(OpCodes.Stloc_3, (object)null);
		}
		if (instruction.opcode == OpCodes.Ldloc_S)
		{
			return new CodeInstruction(OpCodes.Stloc_S, instruction.operand);
		}
		if (instruction.opcode == OpCodes.Ldloc)
		{
			return new CodeInstruction(OpCodes.Stloc, instruction.operand);
		}
		throw new Exception("instruction is not ldloc! : " + (object)instruction);
	}

	internal static string IL2STR(this IEnumerable<CodeInstruction> instructions)
	{
		string text = "";
		foreach (CodeInstruction instruction in instructions)
		{
			text = text + ((object)instruction)?.ToString() + "\n";
		}
		return text;
	}

	public static int Search(this List<CodeInstruction> codes, Func<CodeInstruction, bool> predicate, int startIndex = 0, int count = 1, bool throwOnError = true)
	{
		return codes.Search((int i) => predicate(codes[i]), startIndex, count, throwOnError);
	}

	public static int Search(this List<CodeInstruction> codes, Func<int, bool> predicate, int startIndex = 0, int count = 1, bool throwOnError = true)
	{
		if (count == 0)
		{
			throw new ArgumentOutOfRangeException("count can't be zero");
		}
		int num = ((count > 0) ? 1 : (-1));
		int num2 = System.Math.Abs(count);
		int num3 = 0;
		int i;
		for (i = startIndex; 0 <= i && i < codes.Count; i += num)
		{
			if (predicate(i) && ++num3 == num2)
			{
				break;
			}
		}
		if (num3 != num2)
		{
			if (throwOnError)
			{
				throw new InstructionNotFoundException($"count: found={num3} requested={count} predicate={predicate.Method}");
			}
			if (VERBOSE)
			{
				Log("Did not found instruction[s].\n" + Environment.StackTrace);
			}
			return -1;
		}
		if (VERBOSE)
		{
			Log("Found : \n" + ((IEnumerable<CodeInstruction>)(object)new CodeInstruction[2]
			{
				codes[i],
				codes[i + 1]
			}).IL2STR());
		}
		return i;
	}

	[Obsolete]
	public static int SearchInstruction(List<CodeInstruction> codes, CodeInstruction instruction, int index, int dir = 1, int counter = 1)
	{
		try
		{
			return SearchGeneric(codes, (int idx) => codes[idx].IsSameInstruction(instruction), index, dir, counter);
		}
		catch (InstructionNotFoundException)
		{
			throw new InstructionNotFoundException(" Did not found instruction: " + (object)instruction);
		}
	}

	public static int SearchGeneric(List<CodeInstruction> codes, Func<int, bool> predicate, int index, int dir = 1, int counter = 1, bool throwOnError = true)
	{
		int num = 0;
		while (0 <= index && index < codes.Count && (!predicate(index) || ++num != counter))
		{
			index += dir;
		}
		if (num != counter)
		{
			if (throwOnError)
			{
				throw new InstructionNotFoundException(" Did not found instruction[s].");
			}
			if (VERBOSE)
			{
				Log("Did not found instruction[s].\n" + Environment.StackTrace);
			}
			return -1;
		}
		if (VERBOSE)
		{
			Log("Found : \n" + ((IEnumerable<CodeInstruction>)(object)new CodeInstruction[2]
			{
				codes[index],
				codes[index + 1]
			}).IL2STR());
		}
		return index;
	}

	[Obsolete("unreliable")]
	public static Label GetContinueLabel(List<CodeInstruction> codes, int index, int counter = 1, int dir = -1)
	{
		index = SearchGeneric(codes, (int idx) =>
		{
			Label? label = default(Label?);
			return CodeInstructionExtensions.Branches(codes[idx], ref label);
		}, index, dir, counter);
		return (Label)codes[index].operand;
	}

	[Obsolete("use harmony extension Branches() instead")]
	public static bool IsBR32(OpCode opcode)
	{
		return opcode == OpCodes.Br || opcode == OpCodes.Brtrue || opcode == OpCodes.Brfalse || opcode == OpCodes.Beq;
	}

	public static void MoveLabels(CodeInstruction source, CodeInstruction target)
	{
		List<Label> labels = source.labels;
		target.labels.AddRange(labels);
		labels.Clear();
	}

	public static void ReplaceInstructions(List<CodeInstruction> codes, CodeInstruction[] insertion, int index)
	{
		foreach (CodeInstruction val in insertion)
		{
			if (val == null)
			{
				throw new Exception("Bad Instructions:\n" + insertion.IL2STR());
			}
		}
		if (VERBOSE)
		{
			Log($"replacing <{codes[index]}>\nInsert between: <{codes[index - 1]}>  and  <{codes[index + 1]}>");
		}
		MoveLabels(codes[index], insertion[0]);
		codes.RemoveAt(index);
		codes.InsertRange(index, insertion);
		if (!VERBOSE)
		{
			return;
		}
		Log("Replacing with\n" + insertion.IL2STR());
		string text = "PEEK\n";
		for (int j = index - PeekBefore; j <= index + PeekAfter && j < codes.Count; j++)
		{
			if (j == index)
			{
				text += " *** REPLACEMENT START ***\n";
			}
			text = text + ((object)codes[j])?.ToString() + "\n";
			if (j == index + insertion.Length - 1)
			{
				text += " *** REPLACEMENT END ***\n";
			}
		}
		Log(text);
	}

	public static void InsertInstructions(List<CodeInstruction> codes, CodeInstruction[] insertion, int index, bool moveLabels = true)
	{
		foreach (CodeInstruction val in insertion)
		{
			if (val == null)
			{
				throw new Exception("Bad Instructions:\n" + insertion.IL2STR());
			}
		}
		if (VERBOSE)
		{
			Log($"Insert point:\n between: <{codes[index - 1]}>  and  <{codes[index]}>");
		}
		if (moveLabels)
		{
			MoveLabels(codes[index], insertion[0]);
		}
		codes.InsertRange(index, insertion);
		if (!VERBOSE)
		{
			return;
		}
		Log("Insertion is:\n" + insertion.IL2STR());
		string text = "PEEK\n";
		for (int j = index - PeekBefore; j <= index + PeekAfter && j < codes.Count; j++)
		{
			if (j == index)
			{
				text += " *** INJECTION START ***\n";
			}
			text = text + ((object)codes[j])?.ToString() + "\n";
			if (j == index + insertion.Length - 1)
			{
				text += " *** INJECTION END ***\n";
			}
		}
		Log(text);
	}
}
