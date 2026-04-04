using System.Reflection;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;

namespace KianCommons.Plugins;

internal static class PluginExtensions
{
	public static IUserMod GetUserModInstance(this PluginInfo plugin)
	{
		object obj = ((plugin != null) ? plugin.userModInstance : null);
		return (IUserMod)((obj is IUserMod) ? obj : null);
	}

	public static string GetModName(this PluginInfo plugin)
	{
		IUserMod userModInstance = plugin.GetUserModInstance();
		return (userModInstance != null) ? userModInstance.Name : null;
	}

	public static ulong GetWorkshopID(this PluginInfo plugin)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		PublishedFileId publishedFileID = plugin.publishedFileID;
		return ((PublishedFileId)(ref publishedFileID)).AsUInt64;
	}

	public static bool IsActive(this PluginInfo plugin)
	{
		return plugin != null && plugin.isEnabled;
	}

	public static Assembly GetMainAssembly(this PluginInfo plugin)
	{
		return (plugin == null) ? null : plugin.userModInstance?.GetType()?.Assembly;
	}

	public static bool IsLocal(this PluginInfo plugin)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return plugin != null && (plugin.GetWorkshopID() == 0L || plugin.publishedFileID == PublishedFileId.invalid);
	}

	public static PluginInfo GetPlugin(this Assembly assembly)
	{
		return PluginUtil.GetPlugin(assembly);
	}
}
