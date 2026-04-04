using System.Collections.Generic;
using UnityEngine;

namespace KianCommons;

internal static class GridUtil
{
	public const float GRID_SIZE = 64f;

	public const int GRID_LENGTH = 270;

	public static int ConvertGrid(float a)
	{
		return Mathf.Clamp((int)(a / 64f + 135f), 0, 269);
	}

	public static float ConvertStartPoint(int xy)
	{
		return ((float)xy - 135f) * 64f;
	}

	public static IEnumerable<ushort> ScanDirSegment(Vector2 start, Vector2 dir, float dist)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		foreach (GridVector item in ScaDir(start, dir, dist))
		{
			int index = item.Index;
			ushort segmentID = 0;
			if (index >= 0 && index < NetUtil.netMan.m_segmentGrid.Length)
			{
				segmentID = NetUtil.netMan.m_segmentGrid[index];
			}
			while (segmentID != 0)
			{
				yield return segmentID;
				segmentID = segmentID.ToSegment().m_nextGridSegment;
			}
		}
	}

	public static IEnumerable<GridVector> ScaDir(Vector2 start, Vector2 dir, float dist)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GridVector startGrid = new GridVector(start);
		if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
		{
			foreach (GridVector item in scanX(startGrid, dir, dist))
			{
				yield return item;
			}
			yield break;
		}
		foreach (GridVector item2 in scanY(startGrid, dir, dist))
		{
			yield return item2;
		}
	}

	private static IEnumerable<GridVector> scanX(GridVector start, Vector2 dir, float dist)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		float ratioYX = Mathf.Abs(dir.y / dir.x);
		dist /= 64f;
		int distx = Mathf.CeilToInt(dist / Mathf.Sqrt(1f + ratioYX * ratioYX));
		int signx = (int)Mathf.Sign(dir.x);
		int signy = (int)Mathf.Sign(dir.y);
		int x = 0;
		while (x <= distx)
		{
			int y = Mathf.RoundToInt((float)x * ratioYX);
			GridVector grid = new GridVector(start.x + x * signx, start.y + y * signy);
			yield return grid;
			yield return grid + GridVector.up;
			yield return grid + GridVector.down;
			int num = x + 1;
			x = num;
		}
	}

	private static IEnumerable<GridVector> scanY(GridVector start, Vector2 dir, float dist)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		float ratioXY = Mathf.Abs(dir.x / dir.y);
		dist /= 64f;
		int disty = Mathf.CeilToInt(dist / Mathf.Sqrt(1f + ratioXY * ratioXY));
		int signx = (int)Mathf.Sign(dir.x);
		int signy = (int)Mathf.Sign(dir.y);
		int y = 0;
		while (y <= disty)
		{
			int x = Mathf.RoundToInt((float)y * ratioXY);
			GridVector grid = new GridVector(start.x + x * signx, start.y + y * signy);
			yield return grid;
			yield return grid + GridVector.left;
			yield return grid + GridVector.right;
			int num = y + 1;
			y = num;
		}
	}

	public static IEnumerable<ushort> ScanSegmentsInArea(Vector2 point)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		GridVector start = new GridVector(point);
		foreach (GridVector item in ScanArea(start))
		{
			int index = item.Index;
			ushort segmentID = 0;
			if (index >= 0 && index < NetUtil.netMan.m_segmentGrid.Length)
			{
				segmentID = NetUtil.netMan.m_segmentGrid[index];
			}
			while (segmentID != 0)
			{
				yield return segmentID;
				segmentID = segmentID.ToSegment().m_nextGridSegment;
			}
		}
	}

	private static IEnumerable<GridVector> ScanArea(GridVector start)
	{
		int i = -1;
		while (i <= 1)
		{
			int num;
			for (int j = -1; j <= 1; j = num)
			{
				yield return new GridVector(i, j);
				num = j + 1;
			}
			num = i + 1;
			i = num;
		}
	}
}
