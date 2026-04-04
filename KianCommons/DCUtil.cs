using KianCommons.Plugins;

namespace KianCommons;

internal static class DCUtil
{
	public const ConnectGroup DOUBLE = (ConnectGroup)67895699;

	public const ConnectGroup SINGLE = (ConnectGroup)3148332;

	public const ConnectGroup STATION = (ConnectGroup)132160;

	public const ConnectGroup TRAM = (ConnectGroup)67108879;

	public const ConnectGroup TRAIN = (ConnectGroup)112;

	public const ConnectGroup MONORAIL = (ConnectGroup)1792;

	public const ConnectGroup METRO = (ConnectGroup)133248;

	public const ConnectGroup TROLLEY = (ConnectGroup)3932160;

	public static bool HasLane(ushort segmentID, VehicleType vehicleType)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		return (((NetSegment)(ref segmentID.ToSegment())).Info.m_vehicleTypes & vehicleType) > 0;
	}

	public static bool IsMedian(Node nodeInfo, NetInfo netInfo)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		VehicleType vehicleType = nodeInfo.GetVehicleType();
		return !netInfo.m_vehicleTypes.IsFlagSet(vehicleType);
	}

	public static VehicleType GetVehicleType(this Node nodeInfo)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		VehicleType val = nodeInfo.ARVehicleTypes();
		if ((int)val == 0)
		{
			val = GetVehicleType(nodeInfo.m_connectGroup);
		}
		return val;
	}

	public static bool IsTrack(Node nodeInfo, NetInfo info)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		VehicleType vehicleType = nodeInfo.GetVehicleType();
		return info.m_vehicleTypes.IsFlagSet(vehicleType);
	}

	internal static VehicleType GetVehicleType(ConnectGroup flags)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		VehicleType val = (VehicleType)0;
		if ((flags & 0x400000F) > 0)
		{
			val = (VehicleType)(val | 0x40);
		}
		if ((flags & 0x20880) > 0)
		{
			val = (VehicleType)(val | 2);
		}
		if ((flags & 0x70) > 0)
		{
			val = (VehicleType)(val | 4);
		}
		if ((flags & 0x700) > 0)
		{
			val = (VehicleType)(val | 0x800);
		}
		if ((flags & 0x3C0000) > 0)
		{
			val = (VehicleType)(val | 0x10000);
		}
		return val;
	}
}
