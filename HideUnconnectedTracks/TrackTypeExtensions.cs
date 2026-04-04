using System;
using System.Collections.Generic;
using ColossalFramework;

namespace HideUnconnectedTracks;

public static class TrackTypeExtensions
{
	public static ConnectGroup GetConnectGroups(this TrackType connectType)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		return (ConnectGroup)(connectType switch
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

	public static TrackType GetTrackType(this ConnectGroup connectGroup)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)112))
		{
			return TrackType.Train;
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)133248))
		{
			return TrackType.Metro;
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)1792))
		{
			return TrackType.Monorail;
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)3932160))
		{
			return TrackType.Trolley;
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)67108879))
		{
			return TrackType.Tram;
		}
		return TrackType.None;
	}

	public static IEnumerable<TrackType> GetTrackTypes(this ConnectGroup connectGroup)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		List<TrackType> list = new List<TrackType>();
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)112))
		{
			list.Add(TrackType.Train);
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)133248))
		{
			list.Add(TrackType.Metro);
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)1792))
		{
			list.Add(TrackType.Monorail);
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)3932160))
		{
			list.Add(TrackType.Trolley);
		}
		if (EnumExtensions.IsFlagSet<ConnectGroup>(connectGroup, (ConnectGroup)67108879))
		{
			list.Add(TrackType.Tram);
		}
		return list;
	}
}
