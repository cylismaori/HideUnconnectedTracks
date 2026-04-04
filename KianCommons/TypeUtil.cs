using System;

namespace KianCommons;

internal static class TypeUtil
{
	private static bool IsIntegerType(Type type)
	{
		TypeCode typeCode = Type.GetTypeCode(type);
		TypeCode typeCode2 = typeCode;
		if ((uint)(typeCode2 - 5) <= 7u)
		{
			return true;
		}
		return type.IsArray && IsIntegerType(type.GetElementType());
	}

	public static bool IsNumeric(this Type type)
	{
		return type == typeof(byte) || type == typeof(sbyte) || type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort) || type == typeof(long) || type == typeof(ulong) || type == typeof(float) || type == typeof(double) || type == typeof(decimal);
	}

	public static bool IsFloatingPoint(this Type type)
	{
		TypeCode typeCode = Type.GetTypeCode(type);
		TypeCode typeCode2 = typeCode;
		if ((uint)(typeCode2 - 13) <= 2u)
		{
			return true;
		}
		return false;
	}

	public static bool IsSigned(this Type type)
	{
		switch (Type.GetTypeCode(type))
		{
		case TypeCode.SByte:
		case TypeCode.Int16:
		case TypeCode.Int32:
		case TypeCode.Int64:
		case TypeCode.Single:
		case TypeCode.Double:
		case TypeCode.Decimal:
			return true;
		default:
			return false;
		}
	}

	public static bool IsInteger(this Type type)
	{
		return type.IsPrimitive && IsIntegerType(type);
	}

	public static bool IsSignedInteger(this Type type)
	{
		return type.IsSigned() && type.IsInteger();
	}
}
