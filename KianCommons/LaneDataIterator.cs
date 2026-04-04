using System;
using System.Collections;
using System.Collections.Generic;

namespace KianCommons;

public struct LaneDataIterator(ushort segmentID, bool? startNode = null, LaneType? laneType = null, VehicleType? vehicleType = null) : IEnumerable<LaneData>, IEnumerable, IEnumerator<LaneData>, IDisposable, IEnumerator
{
	private ushort segmentID_ = segmentID;

	private bool? startNode_ = startNode;

	private LaneType? laneType_ = laneType;

	private VehicleType? vehicleType_ = vehicleType;

	private int nLanes_ = ((NetSegment)(ref segmentID.ToSegment())).Info.m_lanes.Length;

	private LaneData current_ = default(LaneData);

	public int Count
	{
		get
		{
			int num = 0;
			try
			{
				while (MoveNext())
				{
					num++;
				}
			}
			finally
			{
				Reset();
			}
			return num;
		}
	}

	public LaneData Current => current_;

	object IEnumerator.Current => Current;

	public bool MoveNext()
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		uint num;
		int num2;
		if (current_.LaneID != 0)
		{
			num = current_.Lane.m_nextLane;
			num2 = current_.LaneIndex + 1;
		}
		else
		{
			num = segmentID_.ToSegment().m_lanes;
			num2 = 0;
		}
		if (num == 0)
		{
			return false;
		}
		if (num2 >= nLanes_)
		{
			if (Log.VERBOSE)
			{
				Log.Warning($"lane count mismatch! segment:{segmentID_} laneID:{num} laneIndex:{num2}", copyToGameLog: false);
				Log.Warning(NetUtil.PrintSegmentLanes(segmentID_), copyToGameLog: false);
			}
			return false;
		}
		if (num.ToLane().m_segment != segmentID_)
		{
			if (Log.VERBOSE)
			{
				Log.Warning($"lane has different segment:{num.ToLane().m_segment}! segment:{segmentID_} laneID:{num} laneIndex:{num2}", copyToGameLog: false);
				Log.Warning(NetUtil.PrintSegmentLanes(segmentID_), copyToGameLog: false);
			}
			return false;
		}
		try
		{
			current_ = new LaneData(num, num2);
		}
		catch (Exception ex)
		{
			ex.Log($"bad lane! segment:{segmentID_} laneID:{num} laneIndex:{num2}", showInPannel: false);
		}
		if (startNode_.HasValue && startNode_.Value != current_.HeadsToStartNode)
		{
			return MoveNext();
		}
		if (laneType_.HasValue && !current_.LaneInfo.m_laneType.IsFlagSet(laneType_.Value))
		{
			return MoveNext();
		}
		if (vehicleType_.HasValue && !current_.LaneInfo.m_vehicleType.IsFlagSet(vehicleType_.Value))
		{
			return MoveNext();
		}
		return true;
	}

	public void Reset()
	{
		current_ = default(LaneData);
	}

	public LaneDataIterator GetEnumerator()
	{
		return this;
	}

	public void Dispose()
	{
	}

	IEnumerator<LaneData> IEnumerable<LaneData>.GetEnumerator()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}
}
