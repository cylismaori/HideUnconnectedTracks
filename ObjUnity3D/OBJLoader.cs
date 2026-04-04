using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ObjUnity3D;

public class OBJLoader
{
	private static OBJData m_OBJData;

	private static OBJMaterial m_CurrentMaterial;

	private static OBJGroup m_CurrentGroup;

	private static readonly Dictionary<string, Action<string>> m_ParseOBJActionDictionary;

	private static readonly Dictionary<string, Action<string>> m_ParseMTLActionDictionary;

	public static OBJData LoadOBJ(Stream lStream)
	{
		m_OBJData = new OBJData();
		m_CurrentMaterial = null;
		m_CurrentGroup = null;
		StreamReader streamReader = new StreamReader(lStream);
		Action<string> action = null;
		while (!streamReader.EndOfStream)
		{
			string text = streamReader.ReadLine();
			if (!StringExt.IsNullOrWhiteSpace(text) && text[0] != '#')
			{
				string[] array = text.Trim().Split(null, 2);
				if (array.Length >= 2)
				{
					string text2 = array[0].Trim();
					string obj = array[1].Trim();
					action = null;
					m_ParseOBJActionDictionary.TryGetValue(text2.ToLowerInvariant(), out action);
					action?.Invoke(obj);
				}
			}
		}
		OBJData oBJData = m_OBJData;
		m_OBJData = null;
		return oBJData;
	}

	public static void ExportOBJ(OBJData lData, Stream lStream)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		StreamWriter streamWriter = new StreamWriter(lStream);
		streamWriter.WriteLine($"# File exported by Unity3D version {Application.unityVersion}");
		for (int i = 0; i < lData.m_Vertices.Count; i++)
		{
			TextWriter textWriter = streamWriter;
			string format = "v {0} {1} {2}";
			float x = lData.m_Vertices[i].x;
			object arg = x.ToString("n8");
			float y = lData.m_Vertices[i].y;
			object arg2 = y.ToString("n8");
			float z = lData.m_Vertices[i].z;
			textWriter.WriteLine(string.Format(format, arg, arg2, z.ToString("n8")));
		}
		for (int j = 0; j < lData.m_UVs.Count; j++)
		{
			TextWriter textWriter2 = streamWriter;
			string format2 = "vt {0} {1}";
			float x2 = lData.m_UVs[j].x;
			object arg3 = x2.ToString("n5");
			float y2 = lData.m_UVs[j].y;
			textWriter2.WriteLine(string.Format(format2, arg3, y2.ToString("n5")));
		}
		for (int k = 0; k < lData.m_UV2s.Count; k++)
		{
			TextWriter textWriter3 = streamWriter;
			string format3 = "vt2 {0} {1}";
			float x3 = lData.m_UVs[k].x;
			object arg4 = x3.ToString("n5");
			float y3 = lData.m_UVs[k].y;
			textWriter3.WriteLine(string.Format(format3, arg4, y3.ToString("n5")));
		}
		for (int l = 0; l < lData.m_Normals.Count; l++)
		{
			TextWriter textWriter4 = streamWriter;
			string format4 = "vn {0} {1} {2}";
			float x4 = lData.m_Normals[l].x;
			object arg5 = x4.ToString("n8");
			float y4 = lData.m_Normals[l].y;
			object arg6 = y4.ToString("n8");
			float z2 = lData.m_Normals[l].z;
			textWriter4.WriteLine(string.Format(format4, arg5, arg6, z2.ToString("n8")));
		}
		for (int m = 0; m < lData.m_Colors.Count; m++)
		{
			TextWriter textWriter5 = streamWriter;
			string format5 = "vc {0} {1} {2} {3}";
			object[] array = new object[4];
			object[] array2 = array;
			int num = 0;
			float r = lData.m_Colors[m].r;
			array2[num] = r.ToString("n8");
			object[] array3 = array;
			int num2 = 1;
			float g = lData.m_Colors[m].g;
			array3[num2] = g.ToString("n8");
			object[] array4 = array;
			int num3 = 2;
			float b = lData.m_Colors[m].b;
			array4[num3] = b.ToString("n8");
			object[] array5 = array;
			int num4 = 3;
			float a = lData.m_Colors[m].a;
			array5[num4] = a.ToString("n8");
			textWriter5.WriteLine(string.Format(format5, array));
		}
		for (int n = 0; n < lData.m_Groups.Count; n++)
		{
			streamWriter.WriteLine($"g {lData.m_Groups[n].m_Name}");
			for (int num5 = 0; num5 < lData.m_Groups[n].Faces.Count; num5++)
			{
				streamWriter.WriteLine($"f {lData.m_Groups[n].Faces[num5].ToString(0)} {lData.m_Groups[n].Faces[num5].ToString(1)} {lData.m_Groups[n].Faces[num5].ToString(2)}");
			}
		}
		streamWriter.Flush();
	}

	private static void PushOBJMaterial(string lMaterialName)
	{
		m_CurrentMaterial = new OBJMaterial(lMaterialName);
		m_OBJData.m_Materials.Add(m_CurrentMaterial);
	}

	private static void PushOBJGroup(string lGroupName)
	{
		m_CurrentGroup = new OBJGroup(lGroupName);
		m_OBJData.m_Groups.Add(m_CurrentGroup);
	}

	private static void PushOBJGroupIfNeeded()
	{
		if (m_CurrentGroup == null)
		{
			PushOBJGroup("default");
		}
	}

	private static void PushOBJFace(string lFaceLine)
	{
		PushOBJGroupIfNeeded();
		string[] array = lFaceLine.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		OBJFace oBJFace = new OBJFace();
		string[] array2 = array;
		foreach (string lVertexString in array2)
		{
			oBJFace.ParseVertex(lVertexString);
		}
		m_CurrentGroup.AddFace(oBJFace);
	}

	static OBJLoader()
	{
		m_OBJData = null;
		m_CurrentMaterial = null;
		m_CurrentGroup = null;
		m_ParseOBJActionDictionary = new Dictionary<string, Action<string>>
		{
			{
				"mtllib",
				delegate
				{
				}
			},
			{
				"usemtl",
				delegate(string lEntry)
				{
					PushOBJGroupIfNeeded();
					m_CurrentGroup.m_Material = m_OBJData.m_Materials.SingleOrDefault((OBJMaterial lX) => lX.m_Name.EqualsInvariantCultureIgnoreCase(lEntry));
				}
			},
			{
				"v",
				delegate(string lEntry)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					m_OBJData.m_Vertices.Add(Utils.ParseVector3String(lEntry));
				}
			},
			{
				"vn",
				delegate(string lEntry)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					m_OBJData.m_Normals.Add(Utils.ParseVector3String(lEntry));
				}
			},
			{
				"vt",
				delegate(string lEntry)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					m_OBJData.m_UVs.Add(Utils.ParseVector2String(lEntry));
				}
			},
			{
				"vt2",
				delegate(string lEntry)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					m_OBJData.m_UV2s.Add(Utils.ParseVector2String(lEntry));
				}
			},
			{
				"vc",
				delegate(string lEntry)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					m_OBJData.m_Colors.Add(Utils.ParseVector4String(lEntry).ToColor());
				}
			},
			{ "f", PushOBJFace },
			{ "g", PushOBJGroup }
		};
		m_ParseMTLActionDictionary = new Dictionary<string, Action<string>>
		{
			{ "newmtl", PushOBJMaterial },
			{
				"Ka",
				delegate(string lEntry)
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					m_CurrentMaterial.m_AmbientColor = Utils.ParseVector3String(lEntry).ToColor();
				}
			},
			{
				"Kd",
				delegate(string lEntry)
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					m_CurrentMaterial.m_DiffuseColor = Utils.ParseVector3String(lEntry).ToColor();
				}
			},
			{
				"Ks",
				delegate(string lEntry)
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					m_CurrentMaterial.m_SpecularColor = Utils.ParseVector3String(lEntry).ToColor();
				}
			},
			{
				"Ns",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_SpecularCoefficient = lEntry.ParseInvariantFloat();
				}
			},
			{
				"d",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_Transparency = lEntry.ParseInvariantFloat();
				}
			},
			{
				"Tr",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_Transparency = lEntry.ParseInvariantFloat();
				}
			},
			{
				"illum",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_IlluminationModel = lEntry.ParseInvariantInt();
				}
			},
			{
				"map_Ka",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_AmbientTextureMap = lEntry;
				}
			},
			{
				"map_Kd",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_DiffuseTextureMap = lEntry;
				}
			},
			{
				"map_Ks",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_SpecularTextureMap = lEntry;
				}
			},
			{
				"map_Ns",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_SpecularHighlightTextureMap = lEntry;
				}
			},
			{
				"map_d",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_AlphaTextureMap = lEntry;
				}
			},
			{
				"map_bump",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_BumpMap = lEntry;
				}
			},
			{
				"bump",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_BumpMap = lEntry;
				}
			},
			{
				"disp",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_DisplacementMap = lEntry;
				}
			},
			{
				"decal",
				delegate(string lEntry)
				{
					m_CurrentMaterial.m_StencilDecalMap = lEntry;
				}
			}
		};
	}
}
