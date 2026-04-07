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
			if (((UnityEngine.Object)loaded).name.Trim() == name.Trim())
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

	public static bool IsSingleBidireactional(this NetInfo info, VehicleInfo.VehicleType vehicleType)
	{
		int num = 0;
		NetInfo.Lane[] lanes = info.m_lanes;
		foreach (NetInfo.Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				NetInfo.Direction direction = val.m_direction;
				if (!direction.CheckFlags((NetInfo.Direction)3, (NetInfo.Direction)0))
				{
					return false;
				}
				num++;
			}
		}
		return num == 1;
	}

	public static bool IsDoubleOneWay(this NetInfo info, VehicleInfo.VehicleType vehicleType)
	{
		int num = 0;
		int num2 = 0;
		NetInfo.Lane[] lanes = info.m_lanes;
		foreach (NetInfo.Lane val in lanes)
		{
			if (!val.m_vehicleType.IsFlagSet(vehicleType))
			{
				continue;
			}
			NetInfo.Direction direction = val.m_direction;
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

	public static bool IsDoubleBidirectional(this NetInfo info, VehicleInfo.VehicleType vehicleType)
	{
		int num = 0;
		NetInfo.Lane[] lanes = info.m_lanes;
		foreach (NetInfo.Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				NetInfo.Direction direction = val.m_direction;
				if (!direction.CheckFlags((NetInfo.Direction)3, (NetInfo.Direction)0))
				{
					return false;
				}
				num++;
			}
		}
		return num == 2;
	}

	public static bool IsSingleNormal(this NetInfo info, VehicleInfo.VehicleType vehicleType)
	{
		int num = 0;
		NetInfo.Lane[] lanes = info.m_lanes;
		foreach (NetInfo.Lane val in lanes)
		{
			if (val.m_vehicleType.IsFlagSet(vehicleType))
			{
				NetInfo.Direction direction = val.m_direction;
				if ((int)direction != 1 && (int)direction != 2)
				{
					return false;
				}
				num++;
			}
		}
		return num == 1;
	}

	public static bool IsDoubleNormal(this NetInfo info, VehicleInfo.VehicleType vehicleType)
	{
		int num = 0;
		int num2 = 0;
		NetInfo.Lane[] lanes = info.m_lanes;
		foreach (NetInfo.Lane val in lanes)
		{
			if (!val.m_vehicleType.IsFlagSet(vehicleType))
			{
				continue;
			}
			NetInfo.Direction direction = val.m_direction;
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
