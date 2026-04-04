using System;
using KianCommons;
using UnityEngine;

namespace HideUnconnectedTracks.Utils;

internal static class NetInfoUtil
{
	public static NetInfo GetInfo(string name, bool throwOnError = true)
	{
		int num = PrefabCollection<NetInfo>.LoadedCount();
		for (uint num2 = 0u; num2 < num; num2++)
		{
			NetInfo loaded = PrefabCollection<NetInfo>.GetLoaded(num2);
			if (((Object)loaded).name.Trim() == name.Trim())
			{
				return loaded;
			}
		}
		if (throwOnError)
		{
			throw new Exception("NetInfo " + name + " not found!");
		}
		return null;
	}

	public static bool IsSingleBidireactional(this NetInfo info, VehicleType vehicleType)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Lane[] lanes = info.m_lanes;
		foreach (Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				Direction direction = val.m_direction;
				if (!direction.CheckFlags((Direction)3, (Direction)0))
				{
					return false;
				}
				num++;
			}
		}
		return num == 1;
	}

	public static bool IsDoubleOneWay(this NetInfo info, VehicleType vehicleType)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		int num = 0;
		int num2 = 0;
		Lane[] lanes = info.m_lanes;
		foreach (Lane val in lanes)
		{
			if (!val.m_vehicleType.IsFlagSet(vehicleType))
			{
				continue;
			}
			Direction direction = val.m_direction;
			if ((int)direction == 1)
			{
				num++;
				continue;
			}
			if ((int)direction == 2)
			{
				num2++;
				continue;
			}
			return false;
		}
		return (num == 2 && num2 == 0) || (num == 0 && num2 == 2);
	}

	public static bool IsDoubleBidirectional(this NetInfo info, VehicleType vehicleType)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Lane[] lanes = info.m_lanes;
		foreach (Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				Direction direction = val.m_direction;
				if (!direction.CheckFlags((Direction)3, (Direction)0))
				{
					return false;
				}
				num++;
			}
		}
		return num == 2;
	}

	public static bool IsSingleNormal(this NetInfo info, VehicleType vehicleType)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		int num = 0;
		Lane[] lanes = info.m_lanes;
		foreach (Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				Direction direction = val.m_direction;
				if ((int)direction != 1 && (int)direction != 2)
				{
					return false;
				}
				num++;
			}
		}
		return num == 1;
	}

	public static bool IsDoubleNormal(this NetInfo info, VehicleType vehicleType)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		int num = 0;
		int num2 = 0;
		Lane[] lanes = info.m_lanes;
		foreach (Lane val in lanes)
		{
			if (!val.m_vehicleType.IsFlagSet(vehicleType))
			{
				continue;
			}
			Direction direction = val.m_direction;
			if ((int)direction == 1)
			{
				num++;
				continue;
			}
			if ((int)direction == 2)
			{
				num2++;
				continue;
			}
			return false;
		}
		return num == 1 && num2 == 1;
	}
}
