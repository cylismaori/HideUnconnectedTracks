using System;
using System.Threading;
using ColossalFramework;
using ICities;

namespace KianCommons;

[Obsolete]
internal static class HelpersExtensions
{
	internal static bool[] ALL_BOOL = new bool[2] { false, true };

	[Obsolete]
	internal static bool VERBOSE
	{
		get
		{
			return Log.VERBOSE;
		}
		set
		{
			Log.VERBOSE = value;
		}
	}

	internal static AppMode currentMode => Singleton<SimulationManager>.instance.m_ManagersWrapper.loading.currentMode;

	internal static bool InGameOrEditor => !InStartup;

	internal static bool IsActive => InGameOrEditor;

	internal static bool InStartup => Helpers.InStartupMenu;

	internal static bool InGame => CheckGameMode((AppMode)0);

	internal static bool InAssetEditor => CheckGameMode((AppMode)2);

	internal static bool ShiftIsPressed => Helpers.ShiftIsPressed;

	internal static bool ControlIsPressed => Helpers.ControlIsPressed;

	internal static bool AltIsPressed => Helpers.AltIsPressed;

	internal static bool InSimulationThread()
	{
		return Thread.CurrentThread == Singleton<SimulationManager>.instance.m_simulationThread;
	}

	internal static bool CheckGameMode(AppMode mode)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (currentMode == mode)
			{
				return true;
			}
		}
		catch
		{
		}
		return false;
	}
}
