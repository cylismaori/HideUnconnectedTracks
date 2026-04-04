using UnityEngine;

namespace KianCommons;

internal struct GridVector
{
	public int x;

	public int y;

	public static GridVector up;

	public static GridVector down;

	public static GridVector left;

	public static GridVector right;

	public static GridVector zero;

	public int Index => y * 270 + x;

	public int MangitudeSquare => x * x + y * y;

	public GridVector(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public GridVector(Vector2 v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		x = GridUtil.ConvertGrid(v.x);
		y = GridUtil.ConvertGrid(v.y);
	}

	public GridVector(Vector3 v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		x = GridUtil.ConvertGrid(v.x);
		y = GridUtil.ConvertGrid(v.z);
	}

	public static GridVector CreateFromSegment(ushort segmentID)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = segmentID.ToSegment().m_startNode.ToNode().m_position;
		Vector3 position2 = segmentID.ToSegment().m_endNode.ToNode().m_position;
		Vector3 v = (position + position2) * 0.5f;
		return new GridVector(v);
	}

	public static GridVector CreateFromNode(ushort nodeID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = nodeID.ToNode().m_position;
		return new GridVector(position);
	}

	public Vector2 GetGirdStartCorner()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(GridUtil.ConvertStartPoint(x), GridUtil.ConvertStartPoint(y));
	}

	public static GridVector operator +(GridVector lhs, GridVector rhs)
	{
		return new GridVector(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static GridVector operator -(GridVector lhs, GridVector rhs)
	{
		return new GridVector(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public override string ToString()
	{
		return $"GridVector<{Index}>({x}, {y})";
	}

	static GridVector()
	{
		up = new GridVector(0, 1);
		down = new GridVector(0, -1);
		left = new GridVector(-1, 0);
		right = new GridVector(1, 0);
		zero = new GridVector(0, 0);
	}
}
