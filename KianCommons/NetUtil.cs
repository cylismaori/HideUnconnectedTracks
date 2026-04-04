using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.Math;
using KianCommons.Math;
using UnityEngine;

namespace KianCommons;

public static class NetUtil
{
	public struct NodeSegments
	{
		public ushort[] segments = new ushort[8];

		public int count = 0;

		private void Add(ushort segmentID)
		{
			segments[count++] = segmentID;
		}

		public NodeSegments(ushort nodeID)
		{
			ushort num = GetFirstSegment(nodeID);
			Add(num);
			while (true)
			{
				num = ((NetSegment)(ref num.ToSegment())).GetLeftSegment(nodeID);
				if (num == segments[0])
				{
					break;
				}
				Add(num);
			}
		}
	}

	public static Dictionary<string, int> kTags = ReflectionHelpers.GetFieldValue<NetInfo>("kTags") as Dictionary<string, int>;

	public const float SAFETY_NET = 0.02f;

	public static NetManager netMan = Singleton<NetManager>.instance;

	public const float MPU = 8f;

	private static NetNode[] nodeBuffer_ = netMan.m_nodes.m_buffer;

	private static NetSegment[] segmentBuffer_ = netMan.m_segments.m_buffer;

	private static NetLane[] laneBuffer_ = netMan.m_lanes.m_buffer;

	public static NetTool netTool => ToolsModifierControl.GetTool<NetTool>();

	public static SimulationManager simMan => Singleton<SimulationManager>.instance;

	public static TerrainManager terrainMan => Singleton<TerrainManager>.instance;

	public static bool LHT => TrafficDrivesOnLeft;

	public static bool RHT => !LHT;

	public static bool TrafficDrivesOnLeft
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			SimulationManager instance = Singleton<SimulationManager>.instance;
			return instance != null && instance.m_metaData?.m_invertTraffic == (MetaBool?)2;
		}
	}

	public static string[] GetTags(DynamicFlags flags)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>();
		foreach (string key in kTags.Keys)
		{
			DynamicFlags flags2 = NetInfo.GetFlags(new string[1] { key });
			DynamicFlags val = flags2 & flags;
			if (!((DynamicFlags)(ref val)).IsEmpty)
			{
				list.Add(key);
			}
		}
		return list.ToArray();
	}

	public static ref NetNode ToNode(this ushort id)
	{
		return ref nodeBuffer_[id];
	}

	public static ref NetSegment ToSegment(this ushort id)
	{
		return ref segmentBuffer_[id];
	}

	public static ref NetLane ToLane(this uint id)
	{
		return ref laneBuffer_[id];
	}

	internal static Flags Flags(this ref NetLane lane)
	{
		return (Flags)lane.m_flags;
	}

	public static Lane GetLaneInfo(uint laneId)
	{
		return ((NetSegment)(ref laneId.ToLane().m_segment.ToSegment())).Info.m_lanes[GetLaneIndex(laneId)];
	}

	public static IEnumerable<NetInfo> IterateLoadedNetInfos()
	{
		int n = PrefabCollection<NetInfo>.LoadedCount();
		for (uint i = 0u; i < n; i++)
		{
			NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);
			if (!((Object)(object)info == (Object)null))
			{
				yield return info;
			}
		}
	}

	public static LaneData GetLaneData(uint laneId)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		Assertion.Assert(laneId != 0, "laneId!=0");
		Flags val = laneId.ToLane().Flags();
		bool con = ((val & 1) | 2) != 1;
		Assertion.Assert(con, "valid");
		ushort segment = laneId.ToLane().m_segment;
		foreach (LaneData item in IterateSegmentLanes(segment))
		{
			if (item.LaneID == laneId)
			{
				return item;
			}
		}
		throw new Exception("Unreachable code. " + $"lane:{laneId} segment:{segment} info:{((NetSegment)(ref segment.ToSegment())).Info}");
	}

	public static bool IsCSUR(this NetInfo info)
	{
		if ((Object)(object)info == (Object)null || (((object)info.m_netAI).GetType() != typeof(RoadAI) && ((object)info.m_netAI).GetType() != typeof(RoadBridgeAI) && ((object)info.m_netAI).GetType() != typeof(RoadTunnelAI)))
		{
			return false;
		}
		return ((Object)info).name.Contains(".CSUR ");
	}

	public static ToolErrors InsertNode(ControlPoint controlPoint, out ushort nodeId, bool test = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		ushort num = default(ushort);
		int num2 = default(int);
		int num3 = default(int);
		ToolErrors result = NetTool.CreateNode(((NetSegment)(ref controlPoint.m_segment.ToSegment())).Info, controlPoint, controlPoint, controlPoint, NetTool.m_nodePositionsSimulation, 0, test, false, true, false, false, false, (ushort)0, ref nodeId, ref num, ref num2, ref num3);
		if (!test)
		{
			ref Flags flags = ref nodeId.ToNode().m_flags;
			flags = (Flags)((uint)flags | 0x120u);
		}
		return result;
	}

	public static int CountPedestrianLanes(this NetInfo info)
	{
		return info.m_lanes.Count((Lane lane) => (int)lane.m_laneType == 2);
	}

	private static bool CheckID(this ref NetNode node1, ushort nodeId2)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		ref NetNode reference = ref nodeId2.ToNode();
		return node1.m_buildIndex == reference.m_buildIndex && node1.m_position == reference.m_position;
	}

	private static bool CheckID(this ref NetSegment segment1, ushort segmentId2)
	{
		ref NetSegment reference = ref segmentId2.ToSegment();
		return (segment1.m_startNode == reference.m_startNode) & (segment1.m_endNode == reference.m_endNode);
	}

	public static ushort GetID(this ref NetNode node)
	{
		ref NetSegment reference = ref node.GetFirstSegment().ToSegment();
		return node.CheckID(reference.m_startNode) ? reference.m_startNode : reference.m_endNode;
	}

	public static ushort GetID(this ref NetSegment segment)
	{
		ref NetNode reference = ref segment.m_startNode.ToNode();
		for (int i = 0; i < 8; i++)
		{
			ushort segment2 = ((NetNode)(ref reference)).GetSegment(i);
			if (segment.CheckID(segment2))
			{
				return segment2;
			}
		}
		return 0;
	}

	public static ushort GetFirstSegment(ushort nodeID)
	{
		return nodeID.ToNode().GetFirstSegment();
	}

	public static ushort GetFirstSegment(this ref NetNode node)
	{
		ushort num = 0;
		for (int i = 0; i < 8; i++)
		{
			num = ((NetNode)(ref node)).GetSegment(i);
			if (num != 0)
			{
				break;
			}
		}
		return num;
	}

	public static Vector3 GetSegmentDir(ushort segmentID, ushort nodeID)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		bool flag = IsStartNode(segmentID, nodeID);
		ref NetSegment reference = ref segmentID.ToSegment();
		return flag ? reference.m_startDirection : reference.m_endDirection;
	}

	internal static float MaxNodeHW(ushort nodeId)
	{
		float num = 0f;
		foreach (ushort item in IterateNodeSegments(nodeId))
		{
			float halfWidth = ((NetSegment)(ref item.ToSegment())).Info.m_halfWidth;
			if (halfWidth > num)
			{
				num = halfWidth;
			}
		}
		return num;
	}

	internal static Bezier3 CalculateSegmentBezier3(this ref NetSegment seg, bool bStartNode = true)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		ref NetNode reference = ref seg.m_startNode.ToNode();
		ref NetNode reference2 = ref seg.m_endNode.ToNode();
		Bezier3 val = new Bezier3
		{
			a = reference.m_position,
			d = reference2.m_position
		};
		NetSegment.CalculateMiddlePoints(val.a, seg.m_startDirection, val.d, seg.m_endDirection, reference.m_flags.IsFlagSet((Flags)32), reference2.m_flags.IsFlagSet((Flags)32), ref val.b, ref val.c);
		if (!bStartNode)
		{
			val = ((Bezier3)(ref val)).Invert();
		}
		return val;
	}

	internal static Bezier2 CalculateSegmentBezier2(ushort segmentId, bool startNode)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Bezier3 bezier = segmentId.ToSegment().CalculateSegmentBezier3(startNode);
		return bezier.ToCSBezier2();
	}

	internal static Bezier2 CalculateSegmentBezier2(ushort segmentId, ushort endNodeID)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		bool startNode = !IsStartNode(segmentId, endNodeID);
		return CalculateSegmentBezier2(segmentId, startNode);
	}

	internal static float GetClosestT(this ref NetSegment segment, Vector3 position)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Bezier3 bezier = segment.CalculateSegmentBezier3();
		return bezier.GetClosestT(position);
	}

	internal static void CalculateCorner(ushort segmentID, ushort nodeID, bool bLeft2, out Vector2 cornerPoint, out Vector2 cornerDir)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 v = default(Vector3);
		Vector3 v2 = default(Vector3);
		bool flag = default(bool);
		((NetSegment)(ref segmentID.ToSegment())).CalculateCorner(segmentID, true, IsStartNode(segmentID, nodeID), !bLeft2, ref v, ref v2, ref flag);
		cornerPoint = v.ToCS2D();
		Vector2 val = v2.ToCS2D();
		cornerDir = ((Vector2)(ref val)).normalized;
	}

	internal static void CalculateOtherCorner(ushort segmentID, ushort nodeID, bool bLeft2, out Vector2 cornerPoint, out Vector2 cornerDir)
	{
		ushort segmentID2 = (bLeft2 ? ((NetSegment)(ref segmentID.ToSegment())).GetLeftSegment(nodeID) : ((NetSegment)(ref segmentID.ToSegment())).GetRightSegment(nodeID));
		CalculateCorner(segmentID2, nodeID, !bLeft2, out cornerPoint, out cornerDir);
	}

	internal static void CalculateCorner(ushort segmentID, ushort nodeID, bool bLeft2, out Vector3 cornerPos, out Vector3 cornerDirection)
	{
		bool flag = default(bool);
		((NetSegment)(ref segmentID.ToSegment())).CalculateCorner(segmentID, true, IsStartNode(segmentID, nodeID), !bLeft2, ref cornerPos, ref cornerDirection, ref flag);
	}

	internal static void CalculateSegEndCenter(ushort segmentID, ushort nodeID, out Vector3 pos, out Vector3 dir)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		CalculateCorner(segmentID, nodeID, false, out Vector3 cornerPos, out Vector3 cornerDirection);
		CalculateCorner(segmentID, nodeID, true, out Vector3 cornerPos2, out Vector3 cornerDirection2);
		pos = (cornerPos + cornerPos2) * 0.5f;
		dir = (cornerDirection + cornerDirection2) * 0.5f;
	}

	public static float SampleHeight(Vector2 point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return terrainMan.SampleDetailHeightSmooth(point.ToCS3D());
	}

	public static Vector3 Get3DPos(Vector2 point)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return point.ToCS3D(SampleHeight(point));
	}

	public static bool CanConnectPathToSegment(ushort segmentID)
	{
		return segmentID.ToSegment().CanConnectPath();
	}

	public static bool CanConnectPath(this ref NetSegment segment)
	{
		return (((NetSegment)(ref segment)).Info.m_netAI is RoadAI) & ((NetSegment)(ref segment)).Info.m_hasPedestrianLanes;
	}

	public static bool CanConnectPath(this NetInfo info)
	{
		return (info.m_netAI is RoadAI) & info.m_hasPedestrianLanes;
	}

	internal static bool IsInvert(this ref NetSegment segment)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return segment.m_flags.IsFlagSet((Flags)16);
	}

	internal static bool IsMiddle(this ref NetNode node)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return node.m_flags.IsFlagSet((Flags)32);
	}

	internal static bool IsSmooth(this ref NetNode node)
	{
		return node.IsMiddle();
	}

	internal static bool IsSmooth(this ref NetSegment segment, bool start)
	{
		return segment.GetNode(start).ToNode().IsSmooth();
	}

	internal static bool SmoothStart(this ref NetSegment segment)
	{
		return segment.IsSmooth(start: true);
	}

	internal static bool SmoothEnd(this ref NetSegment segment)
	{
		return segment.IsSmooth(start: false);
	}

	internal static bool IsJunction(this ref NetNode node)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return node.m_flags.IsFlagSet((Flags)128);
	}

	internal static Direction Invert(this Direction direction, bool invert = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (invert)
		{
			direction = NetInfo.InvertDirection(direction);
		}
		return direction;
	}

	public static bool IsGoingBackward(this Direction direction)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		return (direction & 3) == 2 || (direction & 0xF) == 11;
	}

	public static bool IsGoingForward(this Direction direction)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		return (direction & 3) == 1 || (direction & 0xF) == 7;
	}

	public static bool IsGoingBackward(this Lane laneInfo, bool invertDirection = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return laneInfo.m_finalDirection.Invert(invertDirection).IsGoingBackward();
	}

	public static bool IsGoingForward(this Lane laneInfo, bool invertDirection = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return laneInfo.m_finalDirection.Invert(invertDirection).IsGoingForward();
	}

	public static bool IsStartNode(ushort segmentId, ushort nodeId)
	{
		return segmentId.ToSegment().m_startNode == nodeId;
	}

	public static bool IsStartNode(this ref NetSegment segment, ushort nodeId)
	{
		return segment.m_startNode == nodeId;
	}

	public static ushort GetSegmentNode(ushort segmentID, bool startNode)
	{
		return segmentID.ToSegment().GetNode(startNode);
	}

	public static ushort GetNode(this ref NetSegment segment, bool startNode)
	{
		return startNode ? segment.m_startNode : segment.m_endNode;
	}

	public static bool HasNode(ushort segmentId, ushort nodeId)
	{
		return segmentId.ToSegment().m_startNode == nodeId || segmentId.ToSegment().m_endNode == nodeId;
	}

	public static ushort GetSharedNode(ushort segmentID1, ushort segmentID2)
	{
		return ((NetSegment)(ref segmentID1.ToSegment())).GetSharedNode(segmentID2);
	}

	public static bool IsSegmentValid(ushort segmentId)
	{
		if (segmentId == 0 || segmentId >= 36864)
		{
			return false;
		}
		return segmentId.ToSegment().IsValid();
	}

	public static bool IsValid(this ref NetSegment segment)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)((NetSegment)(ref segment)).Info))
		{
			return false;
		}
		return segment.m_flags.CheckFlags((Flags)1, (Flags)2);
	}

	public static bool IsValid(this InstanceID instance)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		if (((InstanceID)(ref instance)).IsEmpty)
		{
			return false;
		}
		if ((int)((InstanceID)(ref instance)).Type == 5)
		{
			return IsNodeValid(((InstanceID)(ref instance)).NetNode);
		}
		if ((int)((InstanceID)(ref instance)).Type == 6)
		{
			return IsSegmentValid(((InstanceID)(ref instance)).NetSegment);
		}
		if ((int)((InstanceID)(ref instance)).Type == 13)
		{
			return IsLaneValid(((InstanceID)(ref instance)).NetLane);
		}
		return true;
	}

	public static void AssertSegmentValid(ushort segmentId)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Assertion.AssertNeq(segmentId, 0, "segmentId");
		Assertion.AssertGT(36864, segmentId);
		Assertion.AssertNotNull(((NetSegment)(ref segmentId.ToSegment())).Info, $"segment:{segmentId} info");
		Flags flags = segmentId.ToSegment().m_flags;
		bool con = flags.CheckFlags((Flags)1, (Flags)2);
		Assertion.Assert(con, $"segment {segmentId} {((NetSegment)(ref segmentId.ToSegment())).Info} has bad flags: {flags}");
	}

	public static bool IsNodeValid(ushort nodeId)
	{
		if (nodeId == 0 || nodeId >= 32768)
		{
			return false;
		}
		return nodeId.ToNode().IsValid();
	}

	public static bool IsValid(this ref NetNode node)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)((NetNode)(ref node)).Info == (Object)null)
		{
			return false;
		}
		return node.m_flags.CheckFlags((Flags)1, (Flags)2);
	}

	public static bool IsLaneValid(uint laneId)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (laneId != 0 && laneId < 262144)
		{
			return laneId.ToLane().Flags().CheckFlags((Flags)1, (Flags)2);
		}
		return false;
	}

	public static ushort GetHeadNode(this ref NetSegment segment)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		bool flag = (segment.m_flags & 0x10) > 0;
		if (flag ^ LHT)
		{
			return segment.m_startNode;
		}
		return segment.m_endNode;
	}

	public static ushort GetHeadNode(ushort segmentId)
	{
		return segmentId.ToSegment().GetHeadNode();
	}

	public static ushort GetTailNode(this ref NetSegment segment)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		bool flag = (segment.m_flags & 0x10) > 0;
		if (!(flag ^ LHT))
		{
			return segment.m_startNode;
		}
		return segment.m_endNode;
	}

	public static ushort GetTailNode(ushort segmentId)
	{
		return segmentId.ToSegment().GetTailNode();
	}

	public static bool CalculateIsOneWay(ushort segmentId)
	{
		int num = 0;
		int num2 = 0;
		((NetSegment)(ref segmentId.ToSegment())).CountLanes(segmentId, (LaneType)33, (VehicleType)2119, (VehicleCategory)(-1), ref num, ref num2);
		return (num == 0) ^ (num2 == 0);
	}

	public static IEnumerable<ushort> GetCCSegList(ushort nodeID)
	{
		ushort segmentID0 = GetFirstSegment(nodeID);
		Assertion.Assert(segmentID0 != 0, "GetFirstSegment!=0");
		yield return segmentID0;
		ushort segmentID1 = segmentID0;
		while (true)
		{
			segmentID1 = ((NetSegment)(ref segmentID1.ToSegment())).GetRightSegment(nodeID);
			if (segmentID1 == 0 || segmentID1 == segmentID0)
			{
				break;
			}
			yield return segmentID1;
		}
	}

	public static IEnumerable<ushort> GetCWSegList(ushort nodeID)
	{
		ushort segmentID0 = GetFirstSegment(nodeID);
		Assertion.Assert(segmentID0 != 0, "GetFirstSegment!=0");
		yield return segmentID0;
		ushort segmentID1 = segmentID0;
		while (true)
		{
			segmentID1 = ((NetSegment)(ref segmentID1.ToSegment())).GetLeftSegment(nodeID);
			if (segmentID1 == 0 || segmentID1 == segmentID0)
			{
				break;
			}
			yield return segmentID1;
		}
	}

	public static IEnumerable<ushort> IterateNodeSegments(ushort nodeID)
	{
		int i = 0;
		while (i < 8)
		{
			ushort segmentID = ((NetNode)(ref nodeID.ToNode())).GetSegment(i);
			if (segmentID != 0)
			{
				yield return segmentID;
			}
			int num = i + 1;
			i = num;
		}
	}

	public static NodeSegmentIterator IterateSegments(this ref NetNode node)
	{
		return new NodeSegmentIterator(node.GetID());
	}

	public static ushort GetAnotherSegment(this ref NetNode node, ushort segmentId0)
	{
		for (int i = 0; i < 8; i++)
		{
			ushort segment = ((NetNode)(ref node)).GetSegment(i);
			if (segment != segmentId0 && segment != 0)
			{
				return segment;
			}
		}
		return 0;
	}

	[Obsolete("use IterateNodeSegments instead")]
	internal static IEnumerable<ushort> GetSegmentsCoroutine(ushort nodeID)
	{
		return IterateNodeSegments(nodeID);
	}

	public static void LaneTest(ushort segmentId)
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		string text = "STRANGE LANE ISSUE: lane count mismatch for " + $"segment:{segmentId} Info:{((NetSegment)(ref segmentId.ToSegment())).Info} IsSegmentValid={IsSegmentValid(segmentId)}\n";
		if ((Object)(object)((NetSegment)(ref segmentId.ToSegment())).Info != (Object)null)
		{
			List<uint> list = new List<uint>();
			for (uint num = segmentId.ToSegment().m_lanes; num != 0; num = num.ToLane().m_nextLane)
			{
				list.Add(num);
			}
			Lane[] lanes = ((NetSegment)(ref segmentId.ToSegment())).Info.m_lanes;
			if (list.Count == lanes.Length)
			{
				return;
			}
			string text2 = "laneIDs=\n";
			foreach (uint item in list)
			{
				text2 += $"\tlaneID:{item} flags:{item.ToLane().m_flags} segment:{item.ToLane().m_segment} bezier.a={item.ToLane().m_bezier.a}\n";
			}
			string text3 = "laneInfoss=\n";
			for (int i = 0; i < lanes.Length; i++)
			{
				text3 += $"\tlaneID:{lanes[i]} dir:{lanes[i].m_direction} laneType:{lanes[i].m_laneType} vehicleType:{lanes[i].m_vehicleType} pos:{lanes[i].m_position}\n";
			}
			text = text + text2 + text3;
		}
		Log.Error(text);
		Log.LogToFileSimple("NodeControler.Strange.log", text);
	}

	public static IEnumerable<uint> IterateNodeLanes(ushort nodeId)
	{
		int idx = 0;
		if ((Object)(object)((NetNode)(ref nodeId.ToNode())).Info == (Object)null)
		{
			Log.Error("null info: potentially caused by missing assets");
			yield break;
		}
		uint laneID = nodeId.ToNode().m_lane;
		while (laneID != 0)
		{
			yield return laneID;
			laneID = laneID.ToLane().m_nextLane;
			idx++;
		}
	}

	public static LaneDataIterator IterateSegmentLanes(ushort segmentId)
	{
		return new LaneDataIterator(segmentId);
	}

	public static LaneDataIterator IterateLanes(ushort segmentId, bool? startNode = null, LaneType? laneType = null, VehicleType? vehicleType = null)
	{
		return new LaneDataIterator(segmentId, startNode, laneType, vehicleType);
	}

	public static Lane SortedLane(this NetInfo info, int index)
	{
		int num = info.m_sortedLanes[index];
		return info.m_lanes[num];
	}

	public static IEnumerable<Lane> SortedLanes(this NetInfo info)
	{
		int i = 0;
		while (i < info.m_sortedLanes.Length)
		{
			int sortedIndex = info.m_sortedLanes[i];
			yield return info.m_lanes[sortedIndex];
			int num = i + 1;
			i = num;
		}
	}

	public static LaneData[] GetSortedLanes(ushort segmentId, bool? startNode = null, LaneType? laneType = null, VehicleType? vehicleType = null)
	{
		LaneDataIterator laneDataIterator = new LaneDataIterator(segmentId, startNode, laneType, vehicleType);
		return laneDataIterator.OrderBy((LaneData lane) => lane.LaneInfo.m_position).ToArray();
	}

	public static int GetLaneIndex(uint laneID)
	{
		ushort segment = laneID.ToLane().m_segment;
		uint num = segment.ToSegment().m_lanes;
		int num2 = ((NetSegment)(ref segment.ToSegment())).Info.m_lanes.Length;
		for (int i = 0; i < num2; i++)
		{
			if (num == 0)
			{
				break;
			}
			if (num == laneID)
			{
				return i;
			}
			num = num.ToLane().m_nextLane;
		}
		return -1;
	}

	public static uint GetlaneID(ushort segmentID, int laneIndex)
	{
		uint num = segmentID.ToSegment().m_lanes;
		int num2 = ((NetSegment)(ref segmentID.ToSegment())).Info.m_lanes.Length;
		for (int i = 0; i < num2; i++)
		{
			if (num == 0)
			{
				break;
			}
			if (i == laneIndex)
			{
				return num;
			}
			num = num.ToLane().m_nextLane;
		}
		return 0u;
	}

	public static string PrintSegmentLanes(ushort segmentID)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		ref NetSegment reference = ref segmentID.ToSegment();
		List<string> list = new List<string>();
		list.Add($"ushort segment:{segmentID} info:{((NetSegment)(ref reference)).Info}");
		Lane[] lanes = ((NetSegment)(ref reference)).Info.m_lanes;
		int num = 0;
		for (uint num2 = reference.m_lanes; num2 != 0; num2 = num2.ToLane().m_nextLane)
		{
			if (num < lanes.Length)
			{
				Lane val = lanes[num];
				list.Add($"lane[{num}]:{num2} {val.m_laneType}:{val.m_vehicleType}");
			}
			else
			{
				list.Add($"WARNING: laneID:{num2} laneIndex:{num} exceeds laneCount:{lanes.Length} lane.segment:{num2.ToLane().m_segment}");
			}
			num++;
		}
		return list.Join("\n");
	}
}
