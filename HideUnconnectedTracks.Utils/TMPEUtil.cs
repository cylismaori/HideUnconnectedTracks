using KianCommons.Plugins;
using TrafficManager.API;
using TrafficManager.API.Manager;

namespace HideUnconnectedTracks.Utils;

public static class TMPEUtil
{
	public static bool Exists { get; private set; }

	public static IManagerFactory TMPE => Implementations.ManagerFactory;

	public static IJunctionRestrictionsManager JRMan
	{
		get
		{
			IManagerFactory tMPE = TMPE;
			return (tMPE != null) ? tMPE.JunctionRestrictionsManager : null;
		}
	}

	public static IRoutingManager RMan
	{
		get
		{
			IManagerFactory tMPE = TMPE;
			return (tMPE != null) ? tMPE.RoutingManager : null;
		}
	}

	public static ILaneConnectionManager LCM
	{
		get
		{
			IManagerFactory tMPE = TMPE;
			return (tMPE != null) ? tMPE.LaneConnectionManager : null;
		}
	}

	public static void Init()
	{
		Exists = PluginUtil.GetTrafficManager().IsActive();
	}

	public static LaneTransitionData[] GetForwardRoutings(uint laneID, bool startNode)
	{
		uint laneEndRoutingIndex = RMan.GetLaneEndRoutingIndex(laneID, startNode);
		return RMan.LaneEndForwardRoutings[laneEndRoutingIndex].transitions;
	}
}
