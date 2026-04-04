using System;
using System.Reflection;
using ColossalFramework;
using ColossalFramework.Plugins;

namespace KianCommons.Plugins;

internal static class AdaptiveRoadsUtil
{
	private delegate bool IsAdaptive(NetInfo info);

	private delegate VehicleType NodeVehicleTypes(Node node);

	private delegate LaneType NodeLaneTypes(Node node);

	private delegate bool HideBrokenMedians(Node node);

	private delegate bool GetSharpCorners(NetInfo info);

	private static IsAdaptive isAdaptive_;

	private static NodeVehicleTypes nodeVehicleTypes_;

	private static NodeLaneTypes nodeLaneTypes_;

	private static HideBrokenMedians hideBrokenMedians_;

	private static GetSharpCorners getSharpCorners_;

	public static PluginInfo Plugin { get; private set; }

	public static bool IsActive { get; private set; }

	public static Assembly asm { get; private set; }

	public static Type API { get; private set; }

	static AdaptiveRoadsUtil()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		Init();
		Singleton<PluginManager>.instance.eventPluginsStateChanged += new PluginsChangedHandler(Init);
		Singleton<PluginManager>.instance.eventPluginsChanged += new PluginsChangedHandler(Init);
		Singleton<LoadingManager>.instance.m_levelPreLoaded += new LevelPreLoadedHandler(Init);
	}

	private static void Init()
	{
		Log.Info("AdaptiveRoadsUtil.Init() called");
		Plugin = PluginUtil.GetAdaptiveRoads();
		IsActive = Plugin.IsActive();
		if (IsActive)
		{
			asm = Plugin.GetMainAssembly();
			API = asm.GetType("AdaptiveRoads.API", throwOnError: true, ignoreCase: true);
			Log.Info("AR Version=" + (Plugin.userModInstance.VersionOf() ?? new Version(0, 0)));
			nodeVehicleTypes_ = CreateDelegate<NodeVehicleTypes>();
			nodeLaneTypes_ = CreateDelegate<NodeLaneTypes>();
			hideBrokenMedians_ = CreateDelegate<HideBrokenMedians>();
			getSharpCorners_ = CreateDelegate<GetSharpCorners>();
			isAdaptive_ = CreateDelegate<IsAdaptive>();
		}
		else
		{
			Log.Info("AR not found.");
			asm = null;
			API = null;
		}
	}

	private static MethodInfo GetMethod(string name)
	{
		MethodInfo method = API.GetMethod(name);
		if (method == null)
		{
			Log.Warning("AdaptiveRoadsUtil: method " + name + " not found!");
		}
		return method;
	}

	private static object Invoke(string methodName, params object[] args)
	{
		return GetMethod(methodName)?.Invoke(null, args);
	}

	private static TDelegate CreateDelegate<TDelegate>() where TDelegate : Delegate
	{
		return DelegateUtil.CreateDelegate<TDelegate>(API);
	}

	public static object GetARSegmentFlags(ushort id)
	{
		if (!IsActive)
		{
			return null;
		}
		return Invoke("GetARSegmentFlags", id);
	}

	public static object GetARNodeFlags(ushort id)
	{
		if (!IsActive)
		{
			return null;
		}
		return Invoke("GetARNodeFlags", id);
	}

	public static object GetARSegmentEndFlags(ushort segmentID, ushort nodeID)
	{
		if (!IsActive)
		{
			return null;
		}
		return Invoke("GetARSegmentEndFlags", segmentID, nodeID);
	}

	public static object GetARSegmentEndFlags(ushort segmentID, bool startNode)
	{
		if (!IsActive)
		{
			return null;
		}
		ushort node = segmentID.ToSegment().GetNode(startNode);
		return Invoke("GetARSegmentEndFlags", segmentID, node);
	}

	public static object GetARLaneFlags(uint laneId)
	{
		if (!IsActive)
		{
			return null;
		}
		return Invoke("GetARLaneFlags", laneId);
	}

	public static void OverrideARSharpner(bool value = true)
	{
		if (IsActive)
		{
			Invoke("OverrideSharpner", value);
		}
	}

	public static bool GetIsAdaptive(this NetInfo info)
	{
		if (isAdaptive_ == null)
		{
			return false;
		}
		return isAdaptive_(info);
	}

	public static VehicleType ARVehicleTypes(this Node node)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (nodeVehicleTypes_ == null)
		{
			return (VehicleType)0;
		}
		return nodeVehicleTypes_(node);
	}

	public static LaneType LaneTypes(this Node node)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (nodeLaneTypes_ == null)
		{
			return (LaneType)0;
		}
		return nodeLaneTypes_(node);
	}

	public static bool HideBrokenARMedians(this Node node)
	{
		if (hideBrokenMedians_ == null)
		{
			return true;
		}
		return hideBrokenMedians_(node);
	}

	public static bool GetARSharpCorners(this NetInfo info)
	{
		if (getSharpCorners_ == null)
		{
			return false;
		}
		return getSharpCorners_(info);
	}
}
