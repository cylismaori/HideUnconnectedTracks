using System;
using System.Collections.Generic;
using ColossalFramework;

namespace HideUnconnectedTracks;

public static class TrackTypeExtensions
{
	public static NetInfo.ConnectGroup GetConnectGroups(this TrackType connectType)
	{
		return (NetInfo.ConnectGroup)(connectType switch
		{
			TrackType.Train => 112, 
			TrackType.Metro => 133248, 
			TrackType.Monorail => 1792, 
			TrackType.Trolley => 3932160, 
			TrackType.Tram => 67108879, 
			TrackType.None => 0, 
			TrackType.All => 71176191, 
			_ => throw new Exception("Unreachable code"), 
		});
	}

	public static TrackType GetTrackType(this NetInfo.ConnectGroup connectGroup)
	{
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)112))
		{
			return TrackType.Train;
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)133248))
		{
			return TrackType.Metro;
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)1792))
		{
			return TrackType.Monorail;
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)3932160))
		{
			return TrackType.Trolley;
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)67108879))
		{
			return TrackType.Tram;
		}
		return TrackType.None;
	}

	public static IEnumerable<TrackType> GetTrackTypes(this NetInfo.ConnectGroup connectGroup)
	{
		List<TrackType> list = new List<TrackType>();
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)112))
		{
			list.Add(TrackType.Train);
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)133248))
		{
			list.Add(TrackType.Metro);
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)1792))
		{
			list.Add(TrackType.Monorail);
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)3932160))
		{
			list.Add(TrackType.Trolley);
		}
		if (EnumExtensions.IsFlagSet<NetInfo.ConnectGroup>(connectGroup, (NetInfo.ConnectGroup)67108879))
		{
			list.Add(TrackType.Tram);
		}
		return list;
	}
}
