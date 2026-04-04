using System;
using System.Collections;
using System.Collections.Generic;

namespace KianCommons;

public struct LaneIDIterator(ushort segmentID) : IEnumerable<uint>, IEnumerable, IEnumerator<uint>, IDisposable, IEnumerator
{
	private ushort segmentID_ = segmentID;

	private uint laneID_ = 0u;

	private int laneIndex_ = 0;

	private int laneCount_ = ((NetSegment)(ref segmentID.ToSegment())).Info.m_lanes.Length;

	public uint Current => laneID_;

	public int CurrentLaneIndex => laneIndex_;

	object IEnumerator.Current => Current;

	public void Reset()
	{
		laneID_ = 0u;
		laneIndex_ = 0;
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		if (laneID_ == 0)
		{
			if (laneIndex_ > 0)
			{
				return false;
			}
			laneID_ = segmentID_.ToSegment().m_lanes;
		}
		else
		{
			laneID_ = laneID_.ToLane().m_nextLane;
		}
		return laneID_ != 0 && laneIndex_++ < laneCount_;
	}

	public LaneIDIterator GetEnumerator()
	{
		return this;
	}

	IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}
}
