using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KianCommons;

internal static class Assertion
{
	[Conditional("DEBUG")]
	internal static void AssertDebug(bool con, string m = "")
	{
		Assert(con, m);
	}

	[Conditional("DEBUG")]
	internal static void AssertNotNullDebug(object obj, string m = "")
	{
		AssertNotNull(obj, m);
	}

	[Conditional("DEBUG")]
	internal static void AssertEqualDebug<T>(T a, T b, string m = "") where T : IComparable
	{
		AssertEqual(a, b, m);
	}

	[Conditional("DEBUG")]
	internal static void AssertNeqDebug<T>(T a, T b, string m = "") where T : IComparable
	{
		AssertNeq(a, b, m);
	}

	[Conditional("DEBUG")]
	internal static void AssertGTDebug<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) > 0, $"expected {a} > {b} | " + m);
	}

	[Conditional("DEBUG")]
	internal static void AssertGTEqDebug<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) >= 0, $"expected {a} >= {b} | " + m);
	}

	internal static void AssertNotNull(object obj, string m = "")
	{
		Assert(obj != null, " unexpected null " + m);
	}

	internal static void AssertEqual<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) == 0, $"expected {a} == {b} | " + m);
	}

	internal static void AssertNeq<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) != 0, $"expected {a} != {b} | " + m);
	}

	internal static void AssertGT<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) > 0, $"expected {a} > {b} | " + m);
	}

	internal static void AssertGTEq<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) >= 0, $"expected {a} >= {b} | " + m);
	}

	internal static void NotNull(object obj, string m = "")
	{
		Assert(obj != null, " unexpected null " + m);
	}

	internal static void Equal<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) == 0, $"expected {a} == {b} | " + m);
	}

	internal static void Neq<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) != 0, $"expected {a} != {b} | " + m);
	}

	internal static void GT<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) > 0, $"expected {a} > {b} | " + m);
	}

	internal static void GTEq<T>(T a, T b, string m = "") where T : IComparable
	{
		Assert(a.CompareTo(b) >= 0, $"expected {a} >= {b} | " + m);
	}

	internal static void Assert(bool con, string m = "")
	{
		if (!con)
		{
			m = "Assertion failed: " + m;
			Log.Error(m);
			throw new Exception(m);
		}
	}

	internal static void NotNaNOrInf(float f, string m = "")
	{
		if (float.IsNaN(f) || float.IsInfinity(f))
		{
			m = "Assertion failed: " + m;
			Log.Error(m);
			throw new Exception(m);
		}
	}

	internal static void NotNaNOrInf(Vector3 v, string m = "")
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (m != "")
		{
			m = " - " + m;
		}
		m = $"vector={v}" + m;
		NotNaNOrInf(v.x, m);
		NotNaNOrInf(v.y, m);
		NotNaNOrInf(v.z, m);
	}

	internal static void InRange(IList list, int index)
	{
		NotNull(list);
		Assert(index >= 0 && index < list.Count, $"index={index} Count={list.Count}");
	}

	internal static void AssertStack()
	{
		int frameCount = new StackTrace().FrameCount;
		if (frameCount > 200)
		{
			Exception ex = new StackOverflowException("stack frames=" + frameCount);
			Log.Error(ex.ToString());
			throw ex;
		}
	}
}
