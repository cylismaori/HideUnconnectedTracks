using System.Diagnostics;
using System.Linq;

namespace KianCommons;

internal static class StackHelpers
{
	public static string ToStringPretty(this StackTrace st, bool fullPath = false, bool nameSpace = false, bool showArgs = false)
	{
		string text = "";
		StackFrame[] frames = st.GetFrames();
		foreach (StackFrame stackFrame in frames)
		{
			string text2 = stackFrame.GetFileName();
			if (text2 == null)
			{
				text = text + "    at " + stackFrame?.ToString() + "\n";
				continue;
			}
			if (!fullPath)
			{
				text2 = text2.Split('\\').LastOrDefault();
			}
			int fileLineNumber = stackFrame.GetFileLineNumber();
			text += $"    at {stackFrame.GetMethod()} in {text2}:{fileLineNumber}\n";
		}
		return text;
	}
}
