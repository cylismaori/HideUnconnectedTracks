using System;
using UnityEngine;

namespace KianCommons.Math;

internal static class MathUtil
{
	public const float Epsilon = 0.001f;

	public static bool EqualAprox(float a, float b, float error = 0.001f)
	{
		float num = a - b;
		return num > 0f - error && num < error;
	}

	public static bool IsPow2(IConvertible x)
	{
		return Type.GetTypeCode(x.GetType()) switch
		{
			TypeCode.Byte => IsPow2((ulong)(byte)(object)x), 
			TypeCode.UInt16 => IsPow2((ulong)(ushort)(object)x), 
			TypeCode.UInt32 => IsPow2((ulong)(uint)(object)x), 
			TypeCode.UInt64 => IsPow2((ulong)(object)x), 
			TypeCode.SByte => IsPow2((sbyte)(object)x), 
			TypeCode.Int16 => IsPow2((short)(object)x), 
			TypeCode.Int32 => IsPow2((int)(object)x), 
			TypeCode.Int64 => IsPow2((long)(object)x), 
			_ => throw new ArgumentException("expected x to be integer. got " + x), 
		};
	}

	public static bool IsPow2(ulong x)
	{
		return x != 0L && (x & (x - 1)) == 0;
	}

	public static bool IsPow2(long x)
	{
		return x != 0L && (x & (x - 1)) == 0;
	}

	public static bool IsPow2(int x)
	{
		return x != 0 && (x & (x - 1)) == 0;
	}

	public static bool IsPow2(short x)
	{
		return x != 0 && (x & (x - 1)) == 0;
	}

	public static bool IsPow2(sbyte x)
	{
		return x != 0 && (x & (x - 1)) == 0;
	}

	internal static ushort Clamp2U16(int value)
	{
		return (ushort)Mathf.Clamp(value, 0, 65535);
	}
}
