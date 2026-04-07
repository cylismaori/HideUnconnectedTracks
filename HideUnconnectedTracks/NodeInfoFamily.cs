using System.Collections.Generic;
using KianCommons;
using UnityEngine;

namespace HideUnconnectedTracks;

public class NodeInfoFamily
{
	public NodeInfoClass Class;

	public NetInfo.Node TwoWayDouble;

	public NetInfo.Node TwoWayRight;

	public NetInfo.Node TwoWayLeft;

	public NetInfo.Node OneWay;

	public NetInfo.Node OneWayEnd;

	public NetInfo.Node OneWayStart;

	public NetInfo.Node StationDouble;

	public NetInfo.Node StationSingle;

	public NetInfo.Node Station;

	public bool IsComplete => CanComplete && TwoWayRight != null && TwoWayLeft != null && TwoWayRight != null;

	public bool CanComplete => TwoWayDouble != null && OneWay != null && OneWayEnd != null && OneWayStart != null && StationDouble != null && StationSingle != null;

	public override string ToString()
	{
		return "TwoWayDouble:" + T(TwoWayDouble) + " TwoWayRight:" + T(TwoWayRight) + " TwoWayLeft:" + T(TwoWayLeft) + " | OneWay:" + T(OneWay) + " OneWayEnd:" + T(OneWayEnd) + " OneWayStart:" + T(OneWayStart) + " | " + $"StationDouble:{T(StationDouble)} StationSingle:{T(StationSingle)} Station:{T(Station)} | Class={Class}  ";
		static string T(NetInfo.Node nodeInfo)
		{
			if (nodeInfo == null)
			{
				return "<EMPTY>";
			}
			Mesh nodeMesh = nodeInfo.m_nodeMesh;
			string arg = ((nodeMesh != null) ? ((UnityEngine.Object)nodeMesh).name : null) ?? "<MESH=null>";
			return $"{arg,-20}";
		}
	}

	public bool GenerateExtraMeshes()
	{
		if ((UnityEngine.Object)(object)TwoWayDouble?.m_nodeMesh == (UnityEngine.Object)null)
		{
			return false;
		}
		NodeInfoFamily nodeInfoFamily = MeshTable.MeshLUT[TwoWayDouble.m_nodeMesh];
		if (TwoWayRight == null)
		{
			TwoWayRight = CopyNodeInfo_shallow(TwoWayDouble);
			if (nodeInfoFamily != null)
			{
				TwoWayRight.m_nodeMesh = nodeInfoFamily.TwoWayRight.m_nodeMesh;
			}
			else
			{
				TwoWayRight.m_nodeMesh =   TwoWayDouble.m_nodeMesh.CutMesh2(keepLeftSide: false);
			}
		}
		if (TwoWayLeft == null)
		{
			TwoWayLeft = CopyNodeInfo_shallow(TwoWayDouble);
			if (nodeInfoFamily != null)
			{
				TwoWayLeft.m_nodeMesh = nodeInfoFamily.TwoWayLeft.m_nodeMesh;
			}
			else
			{
				TwoWayLeft.m_nodeMesh = TwoWayDouble.m_nodeMesh.CutMesh2(keepLeftSide: true);
			}
		}
		MeshTable.MeshLUT[TwoWayDouble.m_nodeMesh] = this;
		if (StationDouble != null)
		{
			MeshTable.MeshLUT[StationDouble.m_nodeMesh] = this;
		}
		if (StationSingle != null)
		{
			MeshTable.MeshLUT[StationSingle.m_nodeMesh] = this;
		}
		if (Station != null)
		{
			MeshTable.MeshLUT[Station.m_nodeMesh] = this;
		}
		return nodeInfoFamily == null;
	}

	public void FillInTheBlanks(NodeInfoFamily source, bool station = false)
	{
		TwoWayDouble = TwoWayDouble ?? source.TwoWayDouble;
		TwoWayRight = TwoWayRight ?? source.TwoWayRight;
		TwoWayLeft = TwoWayLeft ?? source.TwoWayLeft;
		OneWay = OneWay ?? source.OneWay;
		OneWayEnd = OneWayEnd ?? source.OneWayEnd;
		OneWayStart = OneWayStart ?? source.OneWayStart;
		if (station)
		{
			StationDouble = StationDouble ?? source.StationDouble;
			StationSingle = StationSingle ?? source.StationSingle;
			Station = Station ?? source.Station;
		}
	}

	public void AddStationsToLUT()
	{
		if (StationDouble != null && TwoWayDouble != null)
		{
			Dictionary<NetInfo.Node, NodeInfoFamily> lUT = NodeInfoLUT.LUT;
			NetInfo.Node stationDouble = StationDouble;
			NodeInfoFamily value = (NodeInfoLUT.LUT[TwoWayDouble] = this);
			lUT[stationDouble] = value;
			if (StationSingle != null && OneWayStart != null)
			{
				NodeInfoLUT.LUT[StationSingle] = this;
			}
			if (Station != null)
			{
				NodeInfoLUT.LUT[Station] = this;
			}
		}
	}

	public static NetInfo.Node CopyNodeInfo_shallow(NetInfo.Node nodeInfo)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		Assertion.Assert(nodeInfo != null, "nodeInfo==null");
		NetInfo.Node val = new NetInfo.Node();
		ReflectionHelpers.CopyProperties<NetInfo.Node>(val, nodeInfo);
		Assertion.Assert((Object)(object)nodeInfo.m_material != (Object)null, "nodeInfo m_material is null");
		return val;
	}
}
