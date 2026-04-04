using UnityEngine;

namespace KianCommons.Math;

internal static class LineUtil
{
	public static bool IntersectLine(Vector2 A, Vector2 B, Vector2 C, Vector2 D, out Vector2 center)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		float num = B.y - A.y;
		float num2 = A.x - B.x;
		float num3 = num * A.x + num2 * A.y;
		float num4 = D.y - C.y;
		float num5 = C.x - D.x;
		float num6 = num4 * C.x + num5 * C.y;
		float num7 = num * num5 - num4 * num2;
		if (MathUtil.EqualAprox(num7, 0f))
		{
			center = Vector2.zero;
			return false;
		}
		center.x = (num5 * num3 - num2 * num6) / num7;
		center.y = (num * num6 - num4 * num3) / num7;
		return true;
	}

	public static bool Intersect(Vector2 point1, Vector2 dir1, Vector2 point2, Vector2 dir2, out Vector2 center)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return IntersectLine(point1, point1 + dir1, point2, point2 + dir2, out center);
	}

	public static Vector3 GetClosestPoint(Vector3 A, Vector3 B, Vector3 P)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = P - A;
		Vector3 val2 = B - A;
		float num = Vector3.Dot(val, val2);
		float num2 = num / ((Vector3)(ref val2)).sqrMagnitude;
		return A + val2 * num2;
	}
}
