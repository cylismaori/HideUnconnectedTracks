using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using KianCommons.Math;

namespace KianCommons;

internal static class EnumBitMaskExtensions
{
	[Obsolete("this is buggy as it assumes enum is 0,1,2,3,4 ...\nuse String2EnumValue instead")]
	internal static int String2Enum<T>(string str) where T : Enum
	{
		return Array.IndexOf(Enum.GetNames(typeof(T)), str);
	}

	internal static object String2EnumValue<T>(string str) where T : Enum
	{
		return Enum.Parse(typeof(T), str);
	}

	internal static T Max<T>() where T : Enum
	{
		return Enum.GetValues(typeof(T)).Cast<T>().Max();
	}

	internal static void SetBit(this ref byte b, int idx)
	{
		b |= (byte)(1 << idx);
	}

	internal static void ClearBit(this ref byte b, int idx)
	{
		b &= (byte)(~(1 << idx));
	}

	internal static bool GetBit(this byte b, int idx)
	{
		return (b & (byte)(1 << idx)) != 0;
	}

	internal static void SetBit(this ref byte b, int idx, bool value)
	{
		if (value)
		{
			b.SetBit(idx);
		}
		else
		{
			b.ClearBit(idx);
		}
	}

	public static int CountOnes(int value)
	{
		int num = 0;
		while (value != 0)
		{
			value >>= 1;
			num++;
		}
		return num;
	}

	public static int CountOnes(long value)
	{
		int num = 0;
		while (value != 0)
		{
			value >>= 1;
			num++;
		}
		return num;
	}

	public static int CountOnes(uint value)
	{
		int num = 0;
		while (value != 0)
		{
			value >>= 1;
			num++;
		}
		return num;
	}

	public static int CountOnes(ulong value)
	{
		int num = 0;
		while (value != 0)
		{
			value >>= 1;
			num++;
		}
		return num;
	}

	internal static T GetMaxEnumValue<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>().Max();
	}

	internal static int GetEnumCount<T>()
	{
		return Enum.GetValues(typeof(T)).Length;
	}

	[Conditional("DEBUG")]
	private static void CheckEnumWithFlags<T>() where T : struct, Enum, IConvertible
	{
	}

	[Conditional("DEBUG")]
	private static void CheckEnumWithFlags(Type type)
	{
		if (!type.IsEnum)
		{
			throw new ArgumentException("Type '" + type.FullName + "' is not an enum");
		}
		if (!Attribute.IsDefined(type, typeof(FlagsAttribute)))
		{
			throw new ArgumentException("Type '" + type.FullName + "' doesn't have the 'Flags' attribute");
		}
		if (!Enum.GetUnderlyingType(type).IsInteger())
		{
			throw new Exception("Type '" + type.FullName + "' is not integer based enum.");
		}
	}

	public static void SetFlagsRef<T>(this ref T value, T flags, bool on) where T : struct, IConvertible
	{
		value = EnumExtensions.SetFlags<T>(value, flags, on);
	}

	internal static bool IsFlagSet(this Direction value, Direction flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool IsFlagSet(this LaneType value, LaneType flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool IsFlagSet(this VehicleType value, VehicleType flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool IsFlagSet(this Flags value, Flags flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool IsFlagSet(this FlagsLong value, FlagsLong flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I8
		return (long)(value & flag) > 0L;
	}

	internal static bool IsFlagSet(this Flags value, Flags flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool IsFlagSet(this Flags value, Flags flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		return (value & flag) > 0;
	}

	internal static bool CheckFlags(this Direction value, Direction required, Direction forbidden = (Direction)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (Direction)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this LaneType value, LaneType required, LaneType forbidden = (LaneType)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (LaneType)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this VehicleType value, VehicleType required, VehicleType forbidden = (VehicleType)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (VehicleType)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this Flags value, Flags required, Flags forbidden = (Flags)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (Flags)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this FlagsLong value, FlagsLong required, FlagsLong forbidden = (FlagsLong)0L)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (FlagsLong)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this Flags value, Flags required, Flags forbidden = (Flags)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (Flags)(value & (required | forbidden)) == required;
	}

	internal static bool CheckFlags(this Flags value, Flags required, Flags forbidden = (Flags)0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return (Flags)(value & (required | forbidden)) == required;
	}

	public static bool CheckFlags<T>(this T value, T required, T forbidden) where T : struct, Enum, IConvertible
	{
		long num = value.ToInt64();
		long num2 = required.ToInt64();
		long num3 = forbidden.ToInt64();
		return (num & (num2 | num3)) == num2;
	}

	public static ulong ToUInt64(this IConvertible value)
	{
		Type type = value.GetType();
		if (type.IsEnum)
		{
			type = Enum.GetUnderlyingType(type);
		}
		if (type.IsSigned())
		{
			return (ulong)value.ToInt64(CultureInfo.InvariantCulture);
		}
		return value.ToUInt64(CultureInfo.InvariantCulture);
	}

	public static long ToInt64(this IConvertible value)
	{
		Type type = value.GetType();
		if (type.IsEnum)
		{
			type = Enum.GetUnderlyingType(type);
		}
		if (type.IsSigned())
		{
			return value.ToInt64(CultureInfo.InvariantCulture);
		}
		return (long)value.ToUInt64(CultureInfo.InvariantCulture);
	}

	public static IEnumerable<IConvertible> GetValuesSorted(Type enumType)
	{
		MemberInfo[] members = enumType.GetMembers();
		foreach (MemberInfo member in members)
		{
			if (member is FieldInfo fieldInfo && fieldInfo.FieldType == enumType)
			{
				IConvertible ret = null;
				try
				{
					ret = Enum.Parse(enumType, member.Name) as IConvertible;
				}
				catch
				{
				}
				if (ret != null)
				{
					yield return ret;
				}
			}
		}
	}

	public static IEnumerable<T> GetValuesSorted<T>() where T : struct, Enum, IConvertible
	{
		return GetValuesSorted(typeof(T)).Cast<T>();
	}

	public static IEnumerable<IConvertible> GetPow2Values(Type enumType)
	{
		return from val in GetValuesSorted(enumType)
			where MathUtil.IsPow2(val)
			select val;
	}

	public static IEnumerable<T> GetPow2Values<T>() where T : struct, Enum, IConvertible
	{
		return from val in GetValuesSorted<T>()
			where MathUtil.IsPow2(val)
			select val;
	}

	public static IEnumerable<T> ExtractPow2Flags<T>(this T flags) where T : struct, Enum, IConvertible
	{
		return from flag in GetPow2Values<T>()
			where ((IConvertible)flags).IsFlagSet((IConvertible)flag)
			select flag;
	}

	public static IEnumerable<IConvertible> ExtractPow2Flags(this IConvertible flags)
	{
		return from flag in GetPow2Values(flags.GetType())
			where flags.IsFlagSet(flag)
			select flag;
	}

	public static bool IsFlagSet(this IConvertible flags, IConvertible flag)
	{
		long num = flags.ToInt64();
		long num2 = flag.ToInt64();
		return (num & num2) != 0;
	}

	public static IEnumerable<int> GetPow2ValuesI32(Type enumType)
	{
		IEnumerable<int> source = Enum.GetValues(enumType).Cast<int>();
		return source.Where((int v) => MathUtil.IsPow2(v));
	}

	public static MemberInfo GetEnumMemberInfo(this Type enumType, object value)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		string name = Enum.GetName(enumType, value);
		if (name == null)
		{
			throw new Exception($"{enumType.GetType().Name}:{value} not found");
		}
		return enumType.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty).FirstOrDefault() ?? throw new Exception(enumType.GetType().Name + "." + name + " not found");
	}

	public static MemberInfo GetEnumMemberInfo(this Enum value)
	{
		return value.GetType().GetEnumMemberInfo(value);
	}

	public static T[] GetEnumMemberAttributes<T>(Type enumType, object value) where T : Attribute
	{
		return enumType.GetEnumMemberInfo(value).GetAttributes<T>();
	}

	public static T[] GetEnumMemberAttributes<T>(this Enum value) where T : Attribute
	{
		return GetEnumMemberAttributes<T>(value.GetType(), value);
	}

	public static T[] GetEnumValues<T>() where T : struct, Enum, IConvertible
	{
		return Enum.GetValues(typeof(T)) as T[];
	}
}
