using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColossalFramework;
using HideUnconnectedTracks.Utils;
using KianCommons;
using UnityEngine;

namespace HideUnconnectedTracks;

public static class NodeInfoLUT
{
	public static Dictionary<Node, NodeInfoFamily> LUT = new Dictionary<Node, NodeInfoFamily>(10000);

	public static readonly string[] BuiltInFamilies = new string[4] { "Train Track,Train Oneway Track,Train Station Track", "Monorail Station Track,Monorail Track,Monorail Oneway Track", "Metro Station Track Ground 01,Metro Track Ground 01", "1574857232.R69Railway GrCo 2x2 S_Data, 1574857232.R69Railway GrCo 1x1_Data, 1574857232.R69Railway GrCo 2x2_Data" };

	public static string FamiliesPath = "track-famlilies.txt";

	public static void GenerateLUTs()
	{
		LUT.Clear();
		MeshTable.MeshLUT = new MeshTable();
		PrefabFixesAndWorkArounds();
		try
		{
			using Stream stream = new FileStream(FamiliesPath, FileMode.CreateNew, FileAccess.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			string[] builtInFamilies = BuiltInFamilies;
			foreach (string value in builtInFamilies)
			{
				streamWriter.WriteLine(value);
			}
		}
		catch (IOException)
		{
		}
		List<string> list = new List<string>();
		using (Stream stream2 = new FileStream(FamiliesPath, FileMode.OpenOrCreate, FileAccess.Read))
		{
			using StreamReader streamReader = new StreamReader(stream2);
			while (true)
			{
				string text = streamReader.ReadLine();
				if (text != null)
				{
					list.Add(text);
					continue;
				}
				break;
			}
		}
		Log.Info("families =\n" + string.Join("\n", list.ToArray()));
		foreach (string item in list)
		{
			GenerateFamilyLUT(item);
		}
		GenerateDoubleTrackLUT();
		RecycleStationTrackMesh();
	}

	private static void PrefabFixesAndWorkArounds()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		Log.Info("PrefabFixesWorkArounds() called");
		int num = PrefabCollection<NetInfo>.LoadedCount();
		for (uint num2 = 0u; num2 < num; num2++)
		{
			NetInfo loaded = PrefabCollection<NetInfo>.GetLoaded(num2);
			if (loaded.m_netAI is RoadBaseAI || (Object)(object)loaded == (Object)null || !loaded.m_requireDirectRenderers || EnumExtensions.IsFlagSet<ConnectGroup>(loaded.m_connectGroup, (ConnectGroup)3280492) || !EnumExtensions.IsFlagSet<ConnectGroup>(loaded.m_connectGroup, (ConnectGroup)67895699))
			{
				continue;
			}
			Node[] nodes = loaded.m_nodes;
			foreach (Node val in nodes)
			{
				if (val.m_directConnect && (int)val.m_connectGroup == 0)
				{
					val.m_connectGroup = loaded.m_connectGroup;
				}
			}
		}
	}

	private static void GenerateFamilyLUT(string family)
	{
		Log.Info("Generating LUT for family:" + family);
		IEnumerable<string> infoNames = from name in family.Split(',')
			select name.Trim();
		GenerateFamilyLUT(infoNames);
		static void GenerateFamilyLUT(IEnumerable<string> source2, TrackType trackType = TrackType.All)
		{
			IEnumerable<NetInfo> source = source2.Select((string name) => NetInfoUtil.GetInfo(name, throwOnError: false));
			source = source.Where((NetInfo info) => (Object)(object)info != (Object)null);
			TrackFamily trackFamily = TrackFamily.CreateFamily(source, trackType);
			foreach (NodeInfoFamily value in trackFamily.SubFamilyDict.Values)
			{
				value.GenerateExtraMeshes();
				value.AddStationsToLUT();
			}
		}
	}

	private static void GenerateDoubleTrackLUT()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Log.Info("GenerateDoubleTrackLUT() called");
		int num = PrefabCollection<NetInfo>.LoadedCount();
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (uint num5 = 0u; num5 < num; num5++)
		{
			NetInfo loaded = PrefabCollection<NetInfo>.GetLoaded(num5);
			if ((Object)(object)loaded == (Object)null || !EnumExtensions.IsFlagSet<ConnectGroup>(loaded.m_connectGroup, (ConnectGroup)67895699))
			{
				continue;
			}
			bool flag = false;
			Node[] nodes = loaded.m_nodes;
			foreach (Node val in nodes)
			{
				if (val.m_directConnect && !LUT.ContainsKey(val) && EnumExtensions.IsFlagSet<ConnectGroup>(val.m_connectGroup, (ConnectGroup)67895699) && DCUtil.IsTrack(val, loaded))
				{
					if (GenerateDoubleTrackLUT(val))
					{
						num2++;
					}
					else
					{
						num3++;
					}
					flag = true;
				}
			}
			if (flag)
			{
				num4++;
			}
		}
		Log.Info("GenerateDoubleTrackLUT() successful.\n" + $"generated:{num2} recycled:{num3} pairs of half tracks for {num4} track prefabs");
		static bool GenerateDoubleTrackLUT(Node nodeInfo)
		{
			NodeInfoFamily nodeInfoFamily = new NodeInfoFamily
			{
				TwoWayDouble = nodeInfo
			};
			bool result = nodeInfoFamily.GenerateExtraMeshes();
			LUT[nodeInfoFamily.TwoWayDouble] = nodeInfoFamily;
			return result;
		}
	}

	public static void RecycleStationTrackMesh()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Log.Info("RecycleStationTrackMesh() called");
		int num = PrefabCollection<NetInfo>.LoadedCount();
		for (uint num2 = 0u; num2 < num; num2++)
		{
			NetInfo loaded = PrefabCollection<NetInfo>.GetLoaded(num2);
			if ((Object)(object)loaded == (Object)null || !EnumExtensions.IsFlagSet<ConnectGroup>(loaded.m_connectGroup, (ConnectGroup)132160))
			{
				continue;
			}
			bool flag = false;
			Node[] nodes = loaded.m_nodes;
			foreach (Node val in nodes)
			{
				if (val.m_directConnect && !LUT.ContainsKey(val) && DCUtil.IsTrack(val, loaded))
				{
					flag |= Recycle(loaded, val);
				}
			}
			if (flag)
			{
				Log.Info("Recycled half track meshes for station track: " + ((Object)loaded).name);
			}
		}
		static bool Recycle(NetInfo info, Node nodeInfo)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			NodeInfoFamily nodeInfoFamily = MeshTable.MeshLUT[nodeInfo.m_nodeMesh];
			if (nodeInfoFamily == null)
			{
				return false;
			}
			NodeInfoFamily nodeInfoFamily2 = new NodeInfoFamily();
			nodeInfoFamily2.FillInTheBlanks(nodeInfoFamily);
			if (EnumExtensions.IsFlagSet<ConnectGroup>(nodeInfo.m_connectGroup, (ConnectGroup)67895699))
			{
				nodeInfoFamily2.StationDouble = nodeInfo;
			}
			else if (EnumExtensions.IsFlagSet<ConnectGroup>(nodeInfo.m_connectGroup, (ConnectGroup)3148332))
			{
				nodeInfoFamily2.StationSingle = nodeInfo;
			}
			else
			{
				if (!EnumExtensions.IsFlagSet<ConnectGroup>(nodeInfo.m_connectGroup, (ConnectGroup)132160))
				{
					return false;
				}
				nodeInfoFamily2.Station = nodeInfo;
			}
			nodeInfoFamily2.GenerateExtraMeshes();
			nodeInfoFamily2.AddStationsToLUT();
			return true;
		}
	}
}
