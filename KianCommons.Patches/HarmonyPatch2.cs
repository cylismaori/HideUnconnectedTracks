using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace KianCommons.Patches;

public class HarmonyPatch2 : HarmonyPatch
{
	public HarmonyPatch2(Type delcaringType, Type delegateType, bool instance = false)
	{
		((HarmonyAttribute)this).info.declaringType = delcaringType;
		((HarmonyAttribute)this).info.methodName = delegateType.Name;
		IEnumerable<Type> source = from p in delegateType.GetMethod("Invoke").GetParameters()
			select p.ParameterType;
		if (instance)
		{
			source = source.Skip(1);
		}
		((HarmonyAttribute)this).info.argumentTypes = source.ToArray();
	}
}
