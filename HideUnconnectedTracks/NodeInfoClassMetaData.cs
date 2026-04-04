using System.Collections.Generic;
using System.Linq;

namespace HideUnconnectedTracks;

public struct NodeInfoClassMetaData
{
	public NodeInfoClass NodeInfoClass;

	public ConnectGroup ConnectGroup;

	public bool Matches(Node nodeInfo)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		NodeInfoClass nodeInfoClass = new NodeInfoClass(nodeInfo, NodeInfoClass.UsedIndex, NodeInfoClass.InconsistencyLevel);
		return ConnectGroup == nodeInfo.m_connectGroup && NodeInfoClass.Equals(nodeInfoClass);
	}

	public static int Count(IEnumerable<NodeInfoClassMetaData> nodeClasses, Node nodeInfo)
	{
		return nodeClasses.Count((NodeInfoClassMetaData nodeInfoClassMetaData) => nodeInfoClassMetaData.Matches(nodeInfo));
	}
}
