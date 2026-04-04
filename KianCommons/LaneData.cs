using System;
using ColossalFramework.Math;

namespace KianCommons;

[Serializable]
public struct LaneData
{
	public uint LaneID;

	public int LaneIndex;

	public Lane LaneInfo
	{
		get
		{
			NetInfo info = ((NetSegment)(Segment)).Info;
			object result;
			if (info == null)
			{
				result = null;
			}
			else
			{
				Lane[] lanes = info.m_lanes;
				result = ((lanes != null) ? lanes[LaneIndex] : null);
			}
			return (Lane)result;
		}
	}

	public bool IsValid => LaneID != 0 && Segment.IsValid() && LaneInfo != null;

	public bool HeadsToStartNode => Segment.IsInvert() ^ LaneInfo.IsGoingBackward();

	public ushort HeadNodeID => HeadsToStartNode ? Segment.m_startNode : Segment.m_endNode;

	public readonly ushort SegmentID => Lane.m_segment;

	public readonly NetSegment Segment => SegmentID.ToSegment();

	public readonly NetLane Lane => LaneID.ToLane();

	public readonly Flags Flags
	{
		get
		{
			return (Flags)Lane.m_flags;
		}
		set
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			LaneID.ToLane().m_flags = (ushort)value;
		}
	}

	public bool LeftSide => LaneInfo.m_position < 0f != Segment.m_flags.IsFlagSet((Flags)16);

	public bool RightSide => !LeftSide;

	public Bezier3 Bezier => Lane.m_bezier;

	public bool SmoothA => Segment.SmoothStart();

	public bool SmoothD => Segment.SmoothEnd();

	public LaneData(uint laneID, int laneIndex = -1)
	{
		LaneID = laneID;
		if (laneIndex < 0)
		{
			laneIndex = NetUtil.GetLaneIndex(laneID);
		}
		LaneIndex = laneIndex;
	}

	public Bezier3 GetBezier(bool startNode)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Bezier3 result;
		if (!startNode)
		{
			Bezier3 bezier = Bezier;
			result = ((Bezier3)(bezier)).Invert();
		}
		else
		{
			result = Bezier;
		}
		return result;
	}

	public Bezier3 GetBezier(ushort nodeId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return GetBezier(Segment.IsStartNode(nodeId));
	}

	public override string ToString()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return $"LaneData:[segment:{SegmentID} segmentInfo:{((NetSegment)(Segment)).Info} node:{HeadNodeID} laneID:{LaneID} Index={LaneIndex} {LaneInfo?.m_laneType} {LaneInfo?.m_vehicleType}]";
		}
		catch (NullReferenceException)
		{
			return $"LaneData:[segment:{SegmentID} segmentInfo:{((NetSegment)(Segment)).Info} node:{HeadNodeID} lane ID:{LaneID} null";
		}
	}
}
