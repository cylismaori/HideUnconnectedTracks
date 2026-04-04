using System;
using UnityEngine;

namespace KianCommons.Math;

[Obsolete("Use Vector2D or Vector3D instead")]
internal static class VectorUtil
{
	public static float UnsignedAngleRad(Vector2 v1, Vector2 v2)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((Vector2)(v1)).Normalize();
		((Vector2)(v2)).Normalize();
		float num = Vector2.Dot(v1, v2);
		return Mathf.Acos(num);
	}

	public static Vector2 RotateRadCCW(this Vector2 v, float angle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return Vector2ByAngleRadCCW(((Vector2)(v)).magnitude, angle + v.SignedAngleRadCCW());
	}

	public static Vector2 Vector2ByAngleRadCCW(float magnitude, float angle)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
	}

	public static float SignedAngleRadCCW(this Vector2 v)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		((Vector2)(v)).Normalize();
		return Mathf.Acos(v.x) * Mathf.Sign(v.y);
	}

	public static float Determinent(Vector2 v1, Vector2 v2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return v1.x * v2.y - v1.y * v2.x;
	}

	public static float DetXZ(Vector3 v1, Vector3 v2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return v1.x * v2.z - v1.z * v2.x;
	}

	public static Vector2 Vector2ByAgnleRad(float magnitude, float angle)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
	}

	public static float SignedAngleRadCCW(Vector2 v1, Vector2 v2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Dot(v1, v2);
		float num2 = Determinent(v1, v2);
		return Mathf.Atan2(num2, num);
	}

	public static bool AreApprox180(Vector2 v1, Vector2 v2, float error = 0.001f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		float a = Vector2.Dot(v1, v2);
		return MathUtil.EqualAprox(a, -1f, error);
	}

	public static Vector2 Rotate90CCW(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(0f - v.y, v.x);
	}

	public static Vector2 PerpendicularCCW(this Vector2 v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ((Vector2)(v)).normalized.Rotate90CCW();
	}

	public static Vector2 Rotate90CW(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(v.y, 0f - v.x);
	}

	public static Vector2 PerpendicularCW(this Vector2 v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ((Vector2)(v)).normalized.Rotate90CW();
	}

	public static Vector2 Extend(this Vector2 v, float magnitude)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return v.NewMagnitude(magnitude + ((Vector2)(v)).magnitude);
	}

	public static Vector2 NewMagnitude(this Vector2 v, float magnitude)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return magnitude * ((Vector2)(v)).normalized;
	}

	public static Vector3 ToCS3D(this Vector2 v2, float h = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(v2.x, h, v2.y);
	}

	public static Vector2 ToCS2D(this Vector3 v3)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(v3.x, v3.z);
	}

	public static float Height(this Vector3 v3)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return v3.y;
	}

	public static Vector3 NormalCW(this Vector3 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = new Vector3(v.z, 0f, 0f - v.x);
		return ((Vector3)(val)).normalized;
	}

	public static Vector3 NormalCCW(this Vector3 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = new Vector3(0f - v.z, 0f, v.x);
		return ((Vector3)(val)).normalized;
	}

	public static Vector3 SetI(this Vector3 v, float value, int index)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		((Vector3)(v))[index] = value;
		return v;
	}

	public static bool CompareAngles_CCW_Right(float source, float target)
	{
		if (source == 0f)
		{
			return false;
		}
		if (source > 0f)
		{
			return 0f < target && target < source;
		}
		return !CompareAngles_CCW_Right(0f - source, 0f - target);
	}
}
