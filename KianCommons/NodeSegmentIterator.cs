using System;
using System.Collections;
using System.Collections.Generic;

namespace KianCommons;

public struct NodeSegmentIterator(ushort nodeId) : IEnumerable<ushort>, IEnumerable, IEnumerator<ushort>, IDisposable, IEnumerator
{
	private int i_ = 0;

	private ushort segmentId_ = 0;

	private ushort nodeId_ = nodeId;

	public ushort Current => segmentId_;

	object IEnumerator.Current => Current;

	public bool MoveNext()
	{
		while (i_ < 8)
		{
			segmentId_ = ((NetNode)(ref nodeId_.ToNode())).GetSegment(i_++);
			if (segmentId_ != 0)
			{
				return true;
			}
		}
		segmentId_ = 0;
		return false;
	}

	public NodeSegmentIterator GetEnumerator()
	{
		return this;
	}

	public void Reset()
	{
		i_ = (segmentId_ = 0);
	}

	public void Dispose()
	{
		Reset();
	}

	IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}
}
