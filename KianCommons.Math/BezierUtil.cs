using ColossalFramework.Math;
using UnityEngine;

namespace KianCommons.Math;

internal static class BezierUtil
{
	public static float ArcLength(this Bezier3 beizer, float step = 0.1f)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2;
		Vector3 val;
		for (num2 = step; num2 < 1f; num2 += step)
		{
			val = ((Bezier3)(ref beizer)).Position(num2) - ((Bezier3)(ref beizer)).Position(num2 - step);
			float magnitude = ((Vector3)(ref val)).magnitude;
			num += magnitude;
		}
		val = beizer.d - ((Bezier3)(ref beizer)).Position(num2 - step);
		float magnitude2 = ((Vector3)(ref val)).magnitude;
		return num + magnitude2;
	}

	public static float ArcTravel(this Bezier3 beizer, float distance, float step = 0.1f)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = step;
		while (true)
		{
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector3 val = ((Bezier3)(ref beizer)).Position(num2) - ((Bezier3)(ref beizer)).Position(num2 - step);
			float magnitude = ((Vector3)(ref val)).magnitude;
			num += magnitude;
			if (num >= distance)
			{
				return ((Bezier3)(ref beizer)).Travel(num2, distance - num);
			}
			if (num2 >= 1f)
			{
				break;
			}
			num2 += step;
		}
		return 1f;
	}

	internal static Vector3 DirA(this in Bezier3 bezier)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return bezier.b - bezier.a;
	}

	internal static Vector3 DirD(this in Bezier3 bezier)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return bezier.c - bezier.d;
	}

	public static Vector3 Travel2(this Bezier3 beizer, float distance, out Vector3 tangent)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		float num = ((Bezier3)(ref beizer)).Travel(0f, distance);
		Vector3 val = ((Bezier3)(ref beizer)).Tangent(num);
		tangent = ((Vector3)(ref val)).normalized;
		return ((Bezier3)(ref beizer)).Position(num);
	}

	public static Vector2 Travel2(this Bezier2 beizer, float distance, out Vector2 tangent)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val;
		if (beizer.IsStraight())
		{
			val = beizer.d - beizer.a;
			tangent = ((Vector2)(ref val)).normalized;
			return beizer.TravelStraight(distance);
		}
		float num = ((Bezier2)(ref beizer)).Travel(0f, distance);
		val = ((Bezier2)(ref beizer)).Tangent(num);
		tangent = ((Vector2)(ref val)).normalized;
		return ((Bezier2)(ref beizer)).Position(num);
	}

	private static Vector2 TravelStraight(this Bezier2 beizer, float length)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		float num = length / beizer.ArcLength();
		return beizer.a + num * (beizer.d - beizer.a);
	}

	public static bool IsStraight(this Bezier2 beizer)
	{
		return false;
	}

	public static float ArcLength(this Bezier2 bezier, float step = 0.1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val;
		if (bezier.IsStraight())
		{
			val = bezier.d - bezier.a;
			return ((Vector2)(ref val)).magnitude;
		}
		float num = 0f;
		float num2;
		for (num2 = step; num2 < 1f; num2 += step)
		{
			val = ((Bezier2)(ref bezier)).Position(num2) - ((Bezier2)(ref bezier)).Position(num2 - step);
			float magnitude = ((Vector2)(ref val)).magnitude;
			num += magnitude;
		}
		val = bezier.d - ((Bezier2)(ref bezier)).Position(num2 - step);
		float magnitude2 = ((Vector2)(ref val)).magnitude;
		num += magnitude2;
		if (MathUtil.EqualAprox(num, 0f))
		{
			return 0f;
		}
		return num;
	}

	public static float ArcLength(this Bezier2 bezier, Vector2 point, float step = 0.1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val;
		if (bezier.IsStraight())
		{
			val = point - bezier.a;
			return ((Vector2)(ref val)).magnitude;
		}
		float num = 0f;
		for (float num2 = step; num2 <= 1.001f; num2 += step)
		{
			Vector2 val2 = ((Bezier2)(ref bezier)).Position(num2 - step);
			val = ((Bezier2)(ref bezier)).Position(num2) - val2;
			float magnitude = ((Vector2)(ref val)).magnitude;
			val = point - val2;
			float magnitude2 = ((Vector2)(ref val)).magnitude;
			if (magnitude2 <= magnitude + 0.001f)
			{
				return num + magnitude2;
			}
			num += magnitude;
		}
		return num;
	}

	public static Bezier2 ToCSBezier2(this Bezier3 bezier)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		return new Bezier2
		{
			a = bezier.a.ToCS2D(),
			b = bezier.b.ToCS2D(),
			c = bezier.c.ToCS2D(),
			d = bezier.d.ToCS2D()
		};
	}

	public static Bezier2 Bezier2ByDir(Vector2 startPos, Vector2 startDir, Vector2 endPos, Vector2 endDir)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 v = default(Vector3);
		Vector3 v2 = default(Vector3);
		NetSegment.CalculateMiddlePoints(startPos.ToCS3D(), startDir.ToCS3D(), endPos.ToCS3D(), endDir.ToCS3D(), false, false, ref v, ref v2);
		return new Bezier2
		{
			a = startPos,
			d = endPos,
			b = v.ToCS2D(),
			c = v2.ToCS2D()
		};
	}

	public static Bezier3 Bezier3ByDir(Vector3 startPos, Vector3 startDir, Vector3 endPos, Vector3 endDir, bool startSmooth = false, bool endSmooth = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 b = default(Vector3);
		Vector3 c = default(Vector3);
		NetSegment.CalculateMiddlePoints(startPos, startDir, endPos, endDir, startSmooth, endSmooth, ref b, ref c);
		return new Bezier3
		{
			a = startPos,
			d = endPos,
			b = b,
			c = c
		};
	}

	public static void NormalTangent(this ref Bezier2 bezier, float t, bool lefSide, out Vector2 normal, out Vector2 tangent)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (MathUtil.EqualAprox(t, 0f))
		{
			tangent = bezier.b - bezier.a;
		}
		else if (MathUtil.EqualAprox(t, 0f))
		{
			tangent = bezier.d - bezier.c;
		}
		else
		{
			tangent = ((Bezier2)(ref bezier)).Tangent(t);
		}
		((Vector2)(ref tangent)).Normalize();
		normal = tangent.Rotate90CW();
		if (lefSide)
		{
			normal = -normal;
		}
	}

	public static void NormalTangent(this ref Bezier3 bezier, float t, bool lefSide, out Vector3 normal, out Vector3 tangent)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (MathUtil.EqualAprox(t, 0f))
		{
			tangent = bezier.b - bezier.a;
		}
		else if (MathUtil.EqualAprox(t, 0f))
		{
			tangent = bezier.d - bezier.c;
		}
		else
		{
			tangent = ((Bezier3)(ref bezier)).Tangent(t);
		}
		((Vector3)(ref tangent)).Normalize();
		normal = tangent.NormalCW();
		if (lefSide)
		{
			normal = -normal;
		}
	}

	public static float GetClosestT(this ref Bezier3 bezier, Vector3 pos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		float num = 1E+11f;
		float num2 = 0f;
		Vector3 val = bezier.a;
		float num4 = default(float);
		for (int i = 1; i <= 16; i++)
		{
			Vector3 val2 = ((Bezier3)(ref bezier)).Position((float)i / 16f);
			float num3 = Segment3.DistanceSqr(val, val2, pos, ref num4);
			if (num3 < num)
			{
				num = num3;
				num2 = ((float)(i - 1) + num4) / 16f;
			}
			val = val2;
		}
		float num5 = 1f / 32f;
		float num7 = default(float);
		float num9 = default(float);
		for (int j = 0; j < 4; j++)
		{
			Vector3 val3 = ((Bezier3)(ref bezier)).Position(Mathf.Max(0f, num2 - num5));
			Vector3 val4 = ((Bezier3)(ref bezier)).Position(num2);
			Vector3 val5 = ((Bezier3)(ref bezier)).Position(Mathf.Min(1f, num2 + num5));
			float num6 = Segment3.DistanceSqr(val3, val4, pos, ref num7);
			float num8 = Segment3.DistanceSqr(val4, val5, pos, ref num9);
			num2 = ((!(num6 < num8)) ? Mathf.Min(1f, num2 + num5 * num9) : Mathf.Max(0f, num2 - num5 * (1f - num7)));
			num5 *= 0.5f;
		}
		return num2;
	}

	public static Bezier2 CalculateParallelBezier(this Bezier2 bezier, float sideDistance, bool bLeft)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		bezier.NormalTangent(0f, bLeft, out var normal, out var tangent);
		bezier.NormalTangent(1f, bLeft, out var normal2, out var tangent2);
		return Bezier2ByDir(bezier.a + sideDistance * normal, tangent, bezier.d + sideDistance * normal2, -tangent2);
	}

	public static Bezier3 CalculateParallelBezier(this Bezier3 bezier, float sideDistance, bool bLeft)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		bezier.NormalTangent(0f, bLeft, out var normal, out var tangent);
		bezier.NormalTangent(1f, bLeft, out var normal2, out var tangent2);
		return Bezier3ByDir(bezier.a + sideDistance * normal, tangent, bezier.d + sideDistance * normal2, -tangent2);
	}

	public static Bezier3 ToCSBezier3(this Bezier2 bezier)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		return new Bezier3
		{
			a = NetUtil.Get3DPos(bezier.a),
			b = NetUtil.Get3DPos(bezier.b),
			c = NetUtil.Get3DPos(bezier.c),
			d = NetUtil.Get3DPos(bezier.d)
		};
	}

	public static Bezier3 ToCSBezier3(this Bezier2 bezier, float h)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		return new Bezier3
		{
			a = bezier.a.ToCS3D(h),
			b = bezier.b.ToCS3D(h),
			c = bezier.c.ToCS3D(h),
			d = bezier.d.ToCS3D(h)
		};
	}

	public static Vector3 Mirror(Vector3 pos, Bezier3 bezier)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		float closestT = bezier.GetClosestT(pos);
		Vector3 val = ((Bezier3)(ref bezier)).Position(closestT);
		Vector3 val2 = val - pos;
		return val + val2;
	}

	public static Vector2 Mirror(Vector2 point, Bezier2 bezier)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return Mirror(point.ToCS3D(), bezier.ToCSBezier3(0f)).ToCS2D();
	}

	public static Vector3 CalcShift(Vector3 pos, Vector3 dir, float shift)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return pos + dir.NormalCW() * shift;
	}

	public static Bezier3 SimpleShiftRight(this Bezier3 bezier, float shift, float vshift = 0f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = bezier.d - bezier.a;
		float magnitude = ((Vector3)(ref val)).magnitude;
		Vector3 val2 = bezier.b - bezier.a;
		bezier.a = CalcShift(bezier.a, val2, shift);
		bezier.a.y += vshift;
		Vector3 val3 = bezier.c - bezier.d;
		bezier.d = CalcShift(bezier.d, -val3, shift);
		bezier.d.y += vshift;
		val = bezier.d - bezier.a;
		float magnitude2 = ((Vector3)(ref val)).magnitude;
		float num = magnitude2 / magnitude;
		bezier.b = bezier.a + val2 * num;
		bezier.c = bezier.d + val3 * num;
		return bezier;
	}
}
