using System;

namespace KianCommons;

internal static class LogExtensions
{
	internal static T LogRet<T>(this T a, string m = null)
	{
		if (m == null)
		{
			m = ReflectionHelpers.CurrentMethod(2);
		}
		return a;
	}

	internal static void Log(this Exception ex, string message, bool showInPannel = true)
	{
		ex.Exception(message, showInPannel);
	}

	internal static void Log(this Exception ex, bool showInPannel = true)
	{
		ex.Exception("", showInPannel);
	}
}
