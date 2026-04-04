using System;
using System.Collections;
using System.Collections.Generic;

namespace KianCommons;

public struct FastSegmentList : IEnumerable<ushort>, IEnumerable
{
	public struct Iterator(FastSegmentList list) : IEnumerator<ushort>, IDisposable, IEnumerator
	{
		private int i_ = 0;

		private FastSegmentList list_ = list;

		private ushort current_ = 0;

		public ushort Current => current_;

		object IEnumerator.Current => Current;

		public bool MoveNext()
		{
			if (i_ < list_.Count)
			{
				current_ = list_[i_++];
				return true;
			}
			return false;
		}

		public void Reset()
		{
			i_ = (current_ = 0);
		}

		public Iterator GetEnumerator()
		{
			return this;
		}

		public void Dispose()
		{
			Reset();
		}
	}

	private const int MAX_SIZE = 8;

	private int size_;

	private unsafe fixed ushort segments_[8];

	public int Count => size_;

	public unsafe ushort this[int index]
	{
		get
		{
			if (index < size_ && index >= 0)
			{
				return segments_[index];
			}
			throw new IndexOutOfRangeException($"index:{index} size:{size_}");
		}
		set
		{
			if (index < size_)
			{
				segments_[index] = value;
				return;
			}
			throw new IndexOutOfRangeException($"index:{index} size:{size_}");
		}
	}

	public unsafe void Add(ushort value)
	{
		if (size_ >= 8)
		{
			throw new Exception("List grows too big (max size is 10)");
		}
		segments_[size_++] = value;
	}

	public void Clear()
	{
		size_ = 0;
	}

	IEnumerator<ushort> IEnumerable<ushort>.GetEnumerator()
	{
		return new Iterator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Iterator(this);
	}
}
