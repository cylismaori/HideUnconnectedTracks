using KianCommons.Plugins;

namespace KianCommons;

internal static class DCUtil
{
	public const NetInfo.ConnectGroup DOUBLE = (NetInfo.ConnectGroup)67895699;

	public const NetInfo.ConnectGroup SINGLE = (NetInfo.ConnectGroup)3148332;

	public const NetInfo.ConnectGroup STATION = (NetInfo.ConnectGroup)132160;

	public const NetInfo.ConnectGroup TRAM = (NetInfo.ConnectGroup)67108879;

	public const NetInfo.ConnectGroup TRAIN = (NetInfo.ConnectGroup)112;

	public const NetInfo.ConnectGroup MONORAIL = (NetInfo.ConnectGroup)1792;

	public const NetInfo.ConnectGroup METRO = (NetInfo.ConnectGroup)133248;

	public const NetInfo.ConnectGroup TROLLEY = (NetInfo.ConnectGroup)3932160;

	public static bool HasLane(ushort segmentID, VehicleInfo.VehicleType vehicleType)
	{
		return (((NetSegment)(segmentID.ToSegment())).Info.m_vehicleTypes & vehicleType) > 0;
	}

	public static bool IsMedian(NetInfo.Node nodeInfo, NetInfo netInfo)
	{
		VehicleInfo.VehicleType vehicleType = nodeInfo.GetVehicleType();
		return !netInfo.m_vehicleTypes.IsFlagSet(vehicleType);
	}

	public static VehicleInfo.VehicleType GetVehicleType(this NetInfo.Node nodeInfo)
	{
		VehicleInfo.VehicleType val = AdaptiveRoadsUtil.ARVehicleTypes(nodeInfo);
		if ((int)val == 0)
		{
			val = GetVehicleType(nodeInfo.m_connectGroup);
		}
		return val;
	}

	public static bool IsTrack(NetInfo.Node nodeInfo, NetInfo info)
	{
		VehicleInfo.VehicleType vehicleType = nodeInfo.GetVehicleType();
		return info.m_vehicleTypes.IsFlagSet(vehicleType);
	}

	internal static VehicleInfo.VehicleType GetVehicleType(NetInfo.ConnectGroup flags)
	{
		VehicleInfo.VehicleType val = (VehicleInfo.VehicleType)0;
		if (((int)flags & 0x400000F) > 0)
		{
			val = (VehicleInfo.VehicleType)((int)val | 0x40);
		}
		if (((int)flags & 0x20880) > 0)
		{
			val = (VehicleInfo.VehicleType)((int)val | 2);
		}
		if (((int)flags & 0x70) > 0)
		{
			val = (VehicleInfo.VehicleType)((int)val | 4);
		}
		if (((int)flags & 0x700) > 0)
		{
			val = (VehicleInfo.VehicleType)((int)val | 0x800);
		}
		if (((int)flags & 0x3C0000) > 0)
		{
			val = (VehicleInfo.VehicleType)((int)val | 0x10000);
		}
		return val;
	}
}
