using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;

namespace KianCommons.Plugins;

internal static class PluginUtil
{
	[Flags]
	public enum SearchOptionT
	{
		None = 0,
		Contains = 1,
		StartsWidth = 2,
		[Obsolete("always active")]
		Equals = 4,
		AllModes = 3,
		CaseInsensetive = 8,
		IgnoreWhiteSpace = 0x10,
		AllOptions = 0x18,
		UserModName = 0x20,
		UserModType = 0x40,
		RootNameSpace = 0x80,
		PluginName = 0x100,
		AssemblyName = 0x200,
		AllTargets = 0x3E0
	}

	[Obsolete]
	internal static bool CSUREnabled;

	public const SearchOptionT DefaultsearchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName;

	public const SearchOptionT AssemblyEquals = SearchOptionT.AllOptions | SearchOptionT.AssemblyName;

	private static PluginManager man => Singleton<PluginManager>.instance;

	public static PluginInfo GetCurrentAssemblyPlugin()
	{
		return GetPlugin(Assembly.GetExecutingAssembly());
	}

	public static void LogPlugins(bool detailed = false)
	{
		List<PluginInfo> list = man.GetPluginsInfo().ToList();
		list.Sort(Comparison);
		string text = list.Select((PluginInfo p) => PluginToString(p)).JoinLines();
		Log.Info("Installed mods are:\n" + text, copyToGameLog: true);
		static int Comparison(PluginInfo a, PluginInfo b)
		{
			if (b == null)
			{
				return 1;
			}
			if (a == null)
			{
				return -1;
			}
			return b.isEnabled.CompareTo(a.isEnabled);
		}
		string PluginToString(PluginInfo p)
		{
			string text2 = (p.isEnabled ? "*" : " ");
			string text3 = (p.IsLocal() ? "(local)" : p.GetWorkshopID().ToString());
			text3.PadRight(12);
			if (!detailed)
			{
				return "\t" + text2 + " " + text3 + " " + p.GetModName();
			}
			return "\t" + text2 + " " + text3 + " mod-name:" + (p?.GetModName()).ToSTR() + " asm-name:" + (p?.GetMainAssembly()?.Name()).ToSTR() + " user-mod-type:" + ((p == null) ? null : p.userModInstance?.GetType()?.Name).ToSTR();
		}
	}

	public static void ReportIncomaptibleMods(IEnumerable<PluginInfo> plugins)
	{
	}

	public static PluginInfo GetCSUR()
	{
		return GetPlugin("CSUR ToolBox", 1959342332uL);
	}

	public static PluginInfo GetAdaptiveRoads()
	{
		return GetPlugin("AdaptiveNetworks");
	}

	public static PluginInfo GetHideCrossings()
	{
		return GetPlugin("HideCrosswalks", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetHideUnconnectedTracks()
	{
		return GetPlugin("HideUnconnectedTracks", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetDirectConnectRoads()
	{
		return GetPlugin("DirectConnectRoads", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetTrafficManager()
	{
		return GetPlugin("TrafficManager", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetNetworkDetective()
	{
		return GetPlugin("NetworkDetective", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetNetworkSkins()
	{
		return GetPlugin("NetworkSkins", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	public static PluginInfo GetNodeController()
	{
		return GetPlugin("Node Controller");
	}

	public static PluginInfo GetPedestrianBridge()
	{
		return GetPlugin("Pedestrian Bridge");
	}

	public static PluginInfo GetIMT()
	{
		return GetPlugin("Intersection Marking", new ulong[2] { 2140418403uL, 2159934925uL });
	}

	public static PluginInfo GetRAB()
	{
		return GetPlugin("Roundabout Builder");
	}

	public static PluginInfo GetLoadOrderMod()
	{
		return GetPlugin("LoadOrderMod", null, SearchOptionT.AllOptions | SearchOptionT.AssemblyName);
	}

	[Obsolete]
	private static bool IsCSUR(PluginInfo current)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		int result;
		if (!current.name.Contains("CSUR ToolBox"))
		{
			PublishedFileId publishedFileID = current.publishedFileID;
			result = ((1959342332 == (int)((PublishedFileId)(publishedFileID)).AsUInt64) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	[Obsolete]
	public static void Init()
	{
		CSUREnabled = false;
		foreach (PluginInfo item in man.GetPluginsInfo())
		{
			if (!item.isEnabled || !IsCSUR(item))
			{
				continue;
			}
			CSUREnabled = true;
			break;
		}
	}

	public static PluginInfo GetPlugin(this IUserMod userMod)
	{
		foreach (PluginInfo item in man.GetPluginsInfo())
		{
			if (userMod == item.userModInstance)
			{
				return item;
			}
		}
		return null;
	}

	public static PluginInfo GetPlugin<UserModT>() where UserModT : IUserMod
	{
		foreach (PluginInfo item in man.GetPluginsInfo())
		{
			if (item.userModInstance is UserModT)
			{
				return item;
			}
		}
		return null;
	}

	public static PluginInfo GetPlugin(Assembly assembly = null)
	{
		if (assembly == null)
		{
			assembly = Assembly.GetExecutingAssembly();
		}
		foreach (PluginInfo item in man.GetPluginsInfo())
		{
			if (item.ContainsAssembly(assembly))
			{
				return item;
			}
		}
		return null;
	}

	public static PluginInfo GetPlugin(string searchName, ulong searchId, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
	{
		return GetPlugin(searchName, new ulong[1] { searchId }, searchOptions);
	}

	public static PluginInfo GetPlugin(string searchName, ulong[] searchIds = null, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
	{
		foreach (PluginInfo item in Singleton<PluginManager>.instance.GetPluginsInfo())
		{
			if (item == null)
			{
				continue;
			}
			bool flag = Matches(item, searchIds);
			object userModInstance = item.userModInstance;
			IUserMod val = (IUserMod)((userModInstance is IUserMod) ? userModInstance : null);
			if (val != null)
			{
				if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.UserModName))
				{
					flag = flag || Match(val.Name, searchName, searchOptions);
				}
				Type type = ((object)val).GetType();
				if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.UserModType))
				{
					flag = flag || Match(type.Name, searchName, searchOptions);
				}
				if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.RootNameSpace))
				{
					string text = type.Namespace;
					string name = text.Split('.')[0];
					flag = flag || Match(name, searchName, searchOptions);
				}
				if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.PluginName))
				{
					flag = flag || Match(item.name, searchName, searchOptions);
				}
				if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.AssemblyName))
				{
					Assembly mainAssembly = item.GetMainAssembly();
					flag = flag || Match(mainAssembly?.Name(), searchName, searchOptions);
				}
				if (flag)
				{
					Log.Info("Found plug-in:" + item.GetModName());
					return item;
				}
			}
		}
		Log.Info($"plug-in not found: keyword={searchName} options={searchOptions}");
		return null;
	}

	public static bool Match(string name1, string name2, SearchOptionT searchOptions = SearchOptionT.AllOptions | SearchOptionT.Contains | SearchOptionT.UserModName)
	{
		if (string.IsNullOrEmpty(name1))
		{
			return false;
		}
		Assertion.Assert((searchOptions & SearchOptionT.AllTargets) != 0);
		if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.CaseInsensetive))
		{
			name1 = name1.ToLower();
			name2 = name2.ToLower();
		}
		if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.IgnoreWhiteSpace))
		{
			name1 = name1.Replace(" ", "");
			name2 = name2.Replace(" ", "");
		}
		if (Log.VERBOSE)
		{
		}
		if (name1 == name2)
		{
			return true;
		}
		if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.Contains) && name1.Contains(name2))
		{
			return true;
		}
		if (EnumExtensions.IsFlagSet<SearchOptionT>(searchOptions, SearchOptionT.StartsWidth) && name1.StartsWith(name2))
		{
			return true;
		}
		return false;
	}

	public static bool Matches(PluginInfo plugin, ulong[] searchIds)
	{
		Assertion.AssertNotNull(plugin);
		if (searchIds == null)
		{
			return false;
		}
		foreach (ulong num in searchIds)
		{
			if (num == 0)
			{
				Log.Error("unexpected 0 as mod search id");
			}
			else if (num == plugin.GetWorkshopID())
			{
				return true;
			}
		}
		return false;
	}
}
