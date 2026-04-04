using System.Linq;
using System.Threading;
using ColossalFramework;
using ColossalFramework.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KianCommons;

internal static class Helpers
{
	internal static string[] StartupScenes = new string[4] { "IntroScreen", "IntroScreen2", "Startup", "MainMenu" };

	internal static bool InStartupMenu
	{
		get
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			string[] startupScenes = StartupScenes;
			Scene activeScene = SceneManager.GetActiveScene();
			return startupScenes.Contains(((Scene)(activeScene)).name);
		}
	}

	internal static bool ShiftIsPressed => Input.GetKey((KeyCode)304) || Input.GetKey((KeyCode)303);

	internal static bool ControlIsPressed => Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305);

	internal static bool AltIsPressed => Input.GetKey((KeyCode)308) || Input.GetKey((KeyCode)307);

	internal static void Swap<T>(T a, T b)
	{
		T val = a;
		a = b;
		b = val;
	}

	internal static bool InSimulationThread()
	{
		return Thread.CurrentThread == Singleton<SimulationManager>.instance.m_simulationThread;
	}

	internal static bool InMainThread()
	{
		return Dispatcher.currentSafe == ThreadHelper.dispatcher;
	}
}
