using UnityEngine;

namespace ObjUnity3D;

public static class Vector3Ext
{
	public static readonly Vector3 IgnoreX = new Vector3(0f, 1f, 1f);

	public static readonly Vector3 IgnoreY = new Vector3(1f, 0f, 1f);

	public static readonly Vector3 IgnoreZ = new Vector3(1f, 1f, 0f);

	public static Color ToColor(this Vector3 lVector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		return new Color(lVector.x, lVector.y, lVector.z);
	}
}
