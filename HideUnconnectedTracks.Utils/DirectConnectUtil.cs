using System;
using System.Collections.Generic;
using KianCommons;
using TrafficManager.API.Manager;

namespace HideUnconnectedTracks.Utils;

public static class DirectConnectUtil
{
	[Flags]
	private enum ConnectionT
	{
		None = 0,
		Right = 1,
		Left = 2,
		Both = 3,
		RL = 4,
		LR = 8,
		CrissCross = 0xC
	}

	private static List<LaneData> sourceLanes_ = new List<LaneData>(2);

	private static List<LaneData> targetLanes_ = new List<LaneData>(2);

	public static bool IsSegmentConnectedToSegment(ushort sourceSegmentId, ushort targetSegmentId, ushort nodeId, NetInfo.LaneType laneType, VehicleInfo.VehicleType vehicleType)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		NetSegment reference = sourceSegmentId.ToSegment();
		bool startNode = reference.IsStartNode(nodeId);
		NetInfo.Lane[] lanes = ((NetSegment)(reference)).Info.m_lanes;
		int num = lanes.Length;
		int num2 = 0;
		uint num3 = reference.m_lanes;
		while (num2 < num && num3 != 0)
		{
			NetInfo.Lane val = lanes[num2];
			if (Log.VERBOSE)
			{
			}
			if ((val.m_laneType.IsFlagSet(laneType) || val.m_vehicleType.IsFlagSet(vehicleType)) && IsLaneConnectedToSegment(num3, val, targetSegmentId, startNode))
			{
				return true;
			}
			num2++;
			num3 = num3.ToLane().m_nextLane;
		}
		return false;
	}

	public static bool IsLaneConnectedToSegment(uint sourceLaneId, NetInfo.Lane sourceLaneInfo, ushort targetSegmentID, bool startNode)
	{
		LaneTransitionData[] forwardRoutings = TMPEUtil.GetForwardRoutings(sourceLaneId, startNode);
		if (forwardRoutings == null)
		{
			return false;
		}
		
		bool flag = HasTrolley(sourceLaneInfo);
		NetInfo.Lane[] lanes = ((NetSegment)(targetSegmentID.ToSegment())).Info.m_lanes;
		LaneTransitionData[] array = forwardRoutings;
		foreach (LaneTransitionData val in array)
		{
			if ((int)val.type != 0 && val.segmentId == targetSegmentID)
			{
				if (val.HasTrack())
				{
					return true;
				}
				if (flag && lanes[val.laneIndex].HasTrolley())
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool HasTrolley(this NetInfo.Lane laneInfo)
	{
		return laneInfo.m_vehicleType.IsFlagSet((VehicleInfo.VehicleType)65536);
	}

	private static bool HasTrack(this LaneTransitionData transition)
	{
		return ((int)transition.group & 2) > 0;
	}

	public static bool IsLaneConnectedToLane(uint sourceLaneId, NetInfo.Lane sourceLaneInfo, bool sourceStartNode, uint targetLaneId, NetInfo.Lane targetLaneInfo)
	{
		LaneTransitionData[] forwardRoutings = TMPEUtil.GetForwardRoutings(sourceLaneId, sourceStartNode);
		if (forwardRoutings == null)
		{
			return false;
		}
		bool flag = sourceLaneInfo.HasTrolley() && targetLaneInfo.HasTrolley();
		LaneTransitionData[] array = forwardRoutings;
		foreach (LaneTransitionData val in array)
		{
			if ((int)val.type != 0 && val.laneId == targetLaneId && (val.HasTrack() || flag))
			{
				return true;
			}
		}
		return false;
	}

	private static bool AreLanesConnected(uint laneId1, NetInfo.Lane laneInfo1, bool startNode1, uint laneId2, NetInfo.Lane laneInfo2, bool startNode2)
	{
		return IsLaneConnectedToLane(laneId1, laneInfo1, startNode1, laneId2, laneInfo2) || IsLaneConnectedToLane(laneId2, laneInfo2, startNode2, laneId1, laneInfo1);
	}

	internal static bool HasDirectConnect(ushort segmentId1, ushort segmentId2, ushort nodeId, int nodeInfoIDX)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			NetInfo.Node nodeInfo = ((NetSegment)(segmentId1.ToSegment())).Info.m_nodes[nodeInfoIDX];
			VehicleInfo.VehicleType vehicleType = DCUtil.GetVehicleType(nodeInfo);
			if (!DCUtil.HasLane(segmentId1, vehicleType))
			{
				return true;
			}
			return HasDirectConnect(segmentId1, segmentId2, nodeId, (LaneType)255, vehicleType);
		}
		catch (Exception ex)
		{
			ex.Log();
			return false;
		}
	}

	internal static bool HasDirectConnect(ushort sourceSegmentId, ushort targetSegmentId, ushort nodeId, LaneType laneType, VehicleType vehicleType)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (Log.VERBOSE)
		{
			Log.Called(sourceSegmentId, targetSegmentId, nodeId, laneType, vehicleType);
		}
		try
		{
			return IsSegmentConnectedToSegment(sourceSegmentId, targetSegmentId, nodeId, laneType, vehicleType) || IsSegmentConnectedToSegment(targetSegmentId, sourceSegmentId, nodeId, laneType, vehicleType);
		}
		catch (Exception ex)
		{
			ex.Log();
		}
		return false;
	}

	internal static bool DetermineDirectConnect(ushort segmentId1, ushort segmentId2, ushort nodeId, NetInfo.Node nodeInfo, out bool flipMesh)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		flipMesh = false;
		try
		{
			VehicleType vehicleType = nodeInfo.GetVehicleType();
			if (!DCUtil.HasLane(segmentId1, vehicleType))
			{
				return true;
			}
			Node val = DetermineDirectConnect(nodeInfo, segmentId1, segmentId2, nodeId, (LaneType)255, vehicleType, out flipMesh);
			if (val == null)
			{
				return false;
			}
			nodeInfo = val;
			return true;
		}
		catch (Exception ex)
		{
			ex.Log();
			return false;
		}
	}

	private static void Repopulate(this List<LaneData> list, LaneDataIterator it)
	{
		list.Clear();
		while (it.MoveNext())
		{
			list.Add(it.Current);
		}
	}

	internal static Node DetermineDirectConnect(Node nodeInfo, ushort sourceSegmentId, ushort targetSegmentId, ushort nodeId, LaneType laneType, VehicleType vehicleType, out bool flipMesh)
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		flipMesh = false;
		try
		{
			NetSegment reference = sourceSegmentId.ToSegment();
			NetSegment reference2 = targetSegmentId.ToSegment();
			NetInfo info = ((NetSegment)(reference)).Info;
			NetInfo info2 = ((NetSegment)(reference2)).Info;
			if (Log.VERBOSE)
			{
			}
			if (!TMPEUtil.LCM.HasNodeConnections(nodeId))
			{
				return nodeInfo;
			}
			if (!NodeInfoLUT.LUT.ContainsKey(nodeInfo))
			{
				if (Log.VERBOSE)
				{
				}
				return HasDirectConnect(sourceSegmentId, targetSegmentId, nodeId, (LaneType)255, vehicleType) ? nodeInfo : null;
			}
			ConnectionT connectionT = ConnectionT.None;
			bool flag = reference.IsStartNode(nodeId);
			bool flag2 = reference.IsInvert();
			bool flag3 = reference2.IsStartNode(nodeId);
			bool flag4 = reference2.IsInvert();
			LaneType? laneType2 = laneType;
			VehicleType? vehicleType2 = vehicleType;
			LaneDataIterator it = new LaneDataIterator(sourceSegmentId, null, laneType2, vehicleType2);
			laneType2 = laneType;
			vehicleType2 = vehicleType;
			LaneDataIterator it2 = new LaneDataIterator(targetSegmentId, null, laneType2, vehicleType2);
			int count = it.Count;
			int count2 = it2.Count;
			if (count <= 1 && count2 <= 1)
			{
				return nodeInfo;
			}
			if (count > 2 || count2 > 2)
			{
				return HasDirectConnect(sourceSegmentId, targetSegmentId, nodeId, (LaneType)255, vehicleType) ? nodeInfo : null;
			}
			bool flag5 = count == 1;
			bool flag6 = count2 == 1;
			sourceLanes_.Repopulate(it);
			targetLanes_.Repopulate(it2);
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count2; j++)
				{
					LaneData laneData = sourceLanes_[i];
					LaneData laneData2 = targetLanes_[j];
					LaneData laneData3 = ((!flag5) ? sourceLanes_[(i == 0) ? 1 : 0] : laneData);
					LaneData laneData4 = ((!flag6) ? targetLanes_[(j == 0) ? 1 : 0] : laneData2);
					if (AreLanesConnected(laneData.LaneID, laneData.LaneInfo, flag, laneData2.LaneID, laneData2.LaneInfo, flag3))
					{
						bool flag7 = laneData.LaneInfo.m_position < laneData3.LaneInfo.m_position;
						bool flag8 = flag == !flag2 == flag7;
						bool flag9 = laneData2.LaneInfo.m_position < laneData4.LaneInfo.m_position;
						bool flag10 = flag3 == !flag4 == !flag9;
						connectionT = ((!flag6) ? ((!flag5) ? ((!(flag8 && flag10)) ? ((!(!flag8 && !flag10)) ? ((!(flag8 && !flag10)) ? (connectionT | ConnectionT.LR) : (connectionT | ConnectionT.RL)) : (connectionT | ConnectionT.Left)) : (connectionT | ConnectionT.Right)) : ((!flag10) ? (connectionT | ConnectionT.Left) : (connectionT | ConnectionT.Right))) : ((!flag8) ? (connectionT | ConnectionT.Left) : (connectionT | ConnectionT.Right)));
					}
				}
			}
			NodeInfoFamily nodeInfoFamily = NodeInfoLUT.LUT[nodeInfo];
			if (Log.VERBOSE)
			{
			}
			if (flag6)
			{
				switch (connectionT)
				{
				case ConnectionT.None:
					return null;
				case ConnectionT.Left:
					return nodeInfoFamily.OneWayEnd;
				case ConnectionT.Right:
					flipMesh = true;
					return nodeInfoFamily.OneWayStart;
				case ConnectionT.Both:
					return nodeInfo;
				default:
					throw new Exception("Unreachable code");
				}
			}
			return (Node)(connectionT switch
			{
				ConnectionT.None => null, 
				ConnectionT.Left => nodeInfoFamily.TwoWayLeft, 
				ConnectionT.Right => nodeInfoFamily.TwoWayRight, 
				ConnectionT.Both => nodeInfoFamily.TwoWayDouble, 
				_ => nodeInfo, 
			});
		}
		catch (Exception ex)
		{
			ex.Log();
			return nodeInfo;
		}
	}
}
