using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ColossalFramework;
using KianCommons;
using UnityEngine;

namespace HideUnconnectedTracks;

public class TrackFamily
{
	public Dictionary<NodeInfoClass, NodeInfoFamily> SubFamilyDict;

	public bool Empty => SubFamilyDict.Count == 0;

	public bool IsConsistent => !SubFamilyDict.Values.Any((NodeInfoFamily _subfamily) => !_subfamily.CanComplete);

	public TrackFamily()
	{
		SubFamilyDict = new Dictionary<NodeInfoClass, NodeInfoFamily>();
	}

	public IEnumerable<NodeInfoClass> GetTypes()
	{
		return SubFamilyDict?.Keys;
	}

	private NodeInfoFamily GetOrCreateSubFamily(NodeInfoClass nodeClass)
	{
		if (SubFamilyDict.TryGetValue(nodeClass, out var value))
		{
			return value;
		}
		return SubFamilyDict[nodeClass] = new NodeInfoFamily
		{
			Class = nodeClass
		};
	}

	public bool IsHopefull()
	{
		bool flag = SubFamilyDict.Values.Any((NodeInfoFamily item) => item.TwoWayDouble != null);
		flag &= SubFamilyDict.Values.Any((NodeInfoFamily item) => item.OneWay != null);
		flag &= SubFamilyDict.Values.Any((NodeInfoFamily item) => item.OneWayEnd != null);
		flag &= SubFamilyDict.Values.Any((NodeInfoFamily item) => item.OneWayStart != null);
		flag &= SubFamilyDict.Values.Any((NodeInfoFamily item) => item.StationDouble != null);
		return flag & SubFamilyDict.Values.Any((NodeInfoFamily item) => item.StationSingle != null);
	}

	public static TrackFamily CreateFamily(IEnumerable<NetInfo> infos, TrackType trackType = TrackType.All, int inconsistencyLevel = 0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Invalid comparison between Unknown and I4
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Invalid comparison between Unknown and I4
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Invalid comparison between Unknown and I4
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Invalid comparison between Unknown and I4
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		ConnectGroup connectGroups = trackType.GetConnectGroups();
		TrackFamily trackFamily = new TrackFamily();
		foreach (NetInfo info in infos)
		{
			Assertion.Assert((info.m_connectGroup & connectGroups) > 0, "(info.m_connectGroup & connectGroups) != 0");
			HashSet<NodeInfoClassMetaData> hashSet = new HashSet<NodeInfoClassMetaData>();
			for (int i = 0; i < info.m_nodes.Length; i++)
			{
				Node val = info.m_nodes[i];
				if (!EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, connectGroups))
				{
					continue;
				}
				Assertion.Assert(EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)71176191), "unexpected nodeInfo.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_nodeConnectGroups)/*cast due to .constrained prefix*/).ToString());
				int usedIndex = NodeInfoClassMetaData.Count(hashSet, val);
				NodeInfoClass nodeInfoClass = new NodeInfoClass(val, usedIndex, inconsistencyLevel);
				hashSet.Add(new NodeInfoClassMetaData
				{
					NodeInfoClass = nodeInfoClass,
					ConnectGroup = val.m_connectGroup
				});
				NodeInfoFamily orCreateSubFamily = trackFamily.GetOrCreateSubFamily(nodeInfoClass);
				ConnectGroup val2 = (ConnectGroup)(val.m_connectGroup & 0x3000);
				if (EnumExtensions.IsFlagSet<ConnectGroup>(info.m_connectGroup, (ConnectGroup)67895699))
				{
					if (!EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)67895699))
					{
						throw new Exception("(DOUBLE) unexpected nodeInfo.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(val.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
					}
					Assertion.Assert((int)val2 == 0, "(DOUBLE to DOUBLE)expected oneway=0 got " + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
					orCreateSubFamily.TwoWayDouble = val;
				}
				else if (EnumExtensions.IsFlagSet<ConnectGroup>(info.m_connectGroup, (ConnectGroup)3148332))
				{
					if ((int)val2 == 4096)
					{
						Assertion.Assert(EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)67895699), "(OnewayStart) unexpected nodeInfo.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
						orCreateSubFamily.OneWayStart = val;
					}
					else if ((int)val2 == 8192)
					{
						Assertion.Assert(EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)67895699), "(OnewayEnd) unexpected nodeInfo.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
						orCreateSubFamily.OneWayEnd = val;
					}
					else if ((int)val2 == 12288)
					{
						Assertion.Assert(EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)3148332), "(Oneway) unexpected nodeInfo.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
						orCreateSubFamily.OneWay = val;
					}
					else if (EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)3148332))
					{
						orCreateSubFamily.OneWay = val;
					}
				}
				else
				{
					if (!EnumExtensions.IsFlagSet<ConnectGroup>(info.m_connectGroup, (ConnectGroup)132160))
					{
						throw new Exception("unexpected info.m_connectGroup=" + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
					}
					Assertion.Assert((int)val2 == 0, "(STATION) expected oneway=0 got " + ((object)System.Runtime.CompilerServices.Unsafe.As<ConnectGroup, ConnectGroup>(info.m_connectGroup)/*cast due to .constrained prefix*/).ToString());
					if (EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)67895699))
					{
						orCreateSubFamily.StationDouble = val;
					}
					else if (EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)132160))
					{
						orCreateSubFamily.Station = val;
					}
					else
					{
						orCreateSubFamily.StationSingle = val;
					}
				}
			}
		}
		bool flag = !trackFamily.IsConsistent;
		bool flag2 = trackFamily.IsHopefull();
		string[] value = infos.Select((NetInfo info) => ((Object)info).name).ToArray();
		string text = "{" + string.Join(", ", value) + "}";
		Dictionary<NodeInfoClass, NodeInfoFamily>.ValueCollection values = trackFamily.SubFamilyDict.Values;
		IEnumerable<string> source = values.Select((NodeInfoFamily _subFamily) => _subFamily.ToString());
		string text2 = string.Join("\n\t", source.ToArray());
		string text3 = "prefab names=" + text + " sub-families=\n\t" + text2;
		if (flag && flag2 && inconsistencyLevel < 2)
		{
			int num = inconsistencyLevel + 1;
			Log.Info("Warning: the following family has incosnsitent nodes (ie NetInfo.Node).\n" + text3 + $"\nincreasing inconsistency level from {inconsistencyLevel} to {num} and trying again ...");
			trackFamily = CreateFamily(infos, trackType, num);
		}
		else if (flag)
		{
			Log.Info("WARNING: the following family is incomplete\n" + text3);
		}
		else
		{
			Log.Info("Sucessfully created tracks for:\n" + text3);
		}
		return trackFamily;
	}
}
