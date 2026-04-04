using System.IO;
using System.Runtime.CompilerServices;
using CitiesHarmony.API;
using ICities;
using JetBrains.Annotations;
using KianCommons;

namespace HideUnconnectedTracks.LifeCycle;

public class KianModInfo : IUserMod
{
	private const string HarmonyId = "CS.kian.HideUnconnectedTracks";

	public string Name => "RM Unconnected Tracks ";

	public string Description => "Automatically hide unconnected track textures";

	[MethodImpl(MethodImplOptions.NoInlining)]
	[UsedImplicitly]
	public void OnEnabled()
	{
		File.WriteAllText("mod.debug.log", "");
		HarmonyHelper.DoOnHarmonyReady(delegate
		{
			HarmonyUtil.InstallHarmony("CS.kian.HideUnconnectedTracks");
		});
		if (HelpersExtensions.InGame)
		{
			LifeCycle.Load();
		}
	}

	[UsedImplicitly]
	public void OnDisabled()
	{
		LifeCycle.Release();
		HarmonyUtil.UninstallHarmony("CS.kian.HideUnconnectedTracks");
	}
}
