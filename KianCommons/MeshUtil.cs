using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ObjUnity3D;
using UnityEngine;

namespace KianCommons;

internal static class MeshUtil
{
	public delegate bool IsGoodHandler(Vector3 vertex);

	public static Mesh LoadMesh(string fileName)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("HideUnconnectedTracks.Resources." + fileName);
		Mesh val = new Mesh();
		val.LoadOBJ(OBJLoader.LoadOBJ(manifestResourceStream));
		return val;
	}

	public static void DumpMesh(this Mesh mesh, string fileName)
	{
		string text = "\\/:<>|\"";
		for (int i = 0; i < text.Length; i++)
		{
			fileName = fileName.Replace(text[i].ToString(), "");
		}
		string text2 = "DC_Dumps";
		Directory.CreateDirectory(text2);
		string text3 = Path.Combine(text2, fileName + ".obj");
		Log.Info("dumping mesh " + ((Object)mesh).name + " to " + text3);
		using FileStream lStream = new FileStream(text3, FileMode.Create);
		OBJLoader.ExportOBJ(mesh.EncodeOBJ(), lStream);
	}

	public static Mesh CutMesh(this Mesh mesh, bool keepLeftSide)
	{
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		int[] array = new int[mesh.vertexCount];
		for (int i = 0; i < array.Length; i++)
		{
			if (keepLeftSide ? (mesh.vertices[i].x < 0.0001f) : (mesh.vertices[i].x > -0.0001f))
			{
				array[i] = i;
			}
			else
			{
				array[i] = -2;
			}
		}
		array = array.Where((int v) => v != -2).ToArray();
		int[] array2 = new int[mesh.vertexCount];
		for (int num = 0; num < array2.Length; num++)
		{
			array2[num] = -2;
		}
		for (int num2 = 0; num2 < array.Length; num2++)
		{
			array2[array[num2]] = num2;
		}
		List<int> list = new List<int>(mesh.triangles.Length);
		for (int num3 = 0; num3 < mesh.triangles.Length; num3 += 3)
		{
			int num4 = array2[mesh.triangles[num3]];
			int num5 = array2[mesh.triangles[num3 + 1]];
			int num6 = array2[mesh.triangles[num3 + 2]];
			if (num4 != -2 && num5 != -2 && num6 != -2)
			{
				list.Add(num4);
				list.Add(num5);
				list.Add(num6);
			}
		}
		Vector3[] array3 = (Vector3[])(object)new Vector3[array.Length];
		Vector2[] array4 = (Vector2[])(object)new Vector2[array.Length];
		Vector3[] array5 = (Vector3[])(object)new Vector3[array.Length];
		Vector4[] array6 = (Vector4[])(object)new Vector4[array.Length];
		for (int num7 = 0; num7 < array.Length; num7++)
		{
			int num8 = array[num7];
			array3[num7] = mesh.vertices[num8];
			array4[num7] = mesh.uv[num7];
			array5[num7] = mesh.normals[num8];
			array6[num7] = mesh.tangents[num8];
		}
		Mesh val = new Mesh
		{
			name = ((Object)mesh).name + (keepLeftSide ? "_CutLeftHalf" : "_CutRightHalf")
		};
		val.bounds = mesh.bounds;
		val.vertices = array3;
		val.normals = array5;
		val.tangents = array6;
		val.triangles = list.ToArray();
		return val;
	}

	public static Mesh CutMesh2(this Mesh mesh, bool keepLeftSide)
	{
		List<int> list = new List<int>(mesh.vertexCount);
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = i;
		}
		List<int> list2 = new List<int>(mesh.triangles.Length);
		for (int j = 0; j < mesh.triangles.Length; j += 3)
		{
			if (GoodSide(j) && GoodSide(j + 1) && GoodSide(j + 2))
			{
				list2.Add(mesh.triangles[j]);
				list2.Add(mesh.triangles[j + 1]);
				list2.Add(mesh.triangles[j + 2]);
			}
		}
		Mesh val = Object.Instantiate<Mesh>(mesh);
		((Object)val).name = ((Object)mesh).name + (keepLeftSide ? "_CutLeftHalf" : "_CutRightHalf");
		val.triangles = list2.ToArray();
		return val;
		bool GoodSide(int _i)
		{
			int num = mesh.triangles[_i];
			return keepLeftSide ? (mesh.vertices[num].x < 0.0001f) : (mesh.vertices[num].x > -0.0001f);
		}
	}

	public static Mesh CutMeshGeneric2(this Mesh mesh, IsGoodHandler IsGoodFunc)
	{
		List<int> list = new List<int>(mesh.vertexCount);
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = i;
		}
		List<int> list2 = new List<int>(mesh.triangles.Length);
		for (int j = 0; j < mesh.triangles.Length; j += 3)
		{
			if (IsGood(j) && IsGood(j + 1) && IsGood(j + 2))
			{
				list2.Add(mesh.triangles[j]);
				list2.Add(mesh.triangles[j + 1]);
				list2.Add(mesh.triangles[j + 2]);
			}
		}
		Mesh val = Object.Instantiate<Mesh>(mesh);
		((Object)val).name = ((Object)mesh).name + "_CutMeshGeneric";
		val.triangles = list2.ToArray();
		return val;
		bool IsGood(int _i)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			int num = mesh.triangles[_i];
			return IsGoodFunc(mesh.vertices[num]);
		}
	}

	public static Mesh CutMeshGeneric(this Mesh mesh, IsGoodHandler IsGoodFunc)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		int[] array = new int[mesh.vertexCount];
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = IsGoodFunc(mesh.vertices[i]);
			array[i] = (flag ? i : (-2));
		}
		array = array.Where((int v) => v != -2).ToArray();
		int[] array2 = new int[mesh.vertexCount];
		for (int num = 0; num < array2.Length; num++)
		{
			array2[num] = -2;
		}
		for (int num2 = 0; num2 < array.Length; num2++)
		{
			array2[array[num2]] = num2;
		}
		List<int> list = new List<int>(mesh.triangles.Length);
		for (int num3 = 0; num3 < mesh.triangles.Length; num3 += 3)
		{
			int num4 = array2[mesh.triangles[num3]];
			int num5 = array2[mesh.triangles[num3 + 1]];
			int num6 = array2[mesh.triangles[num3 + 2]];
			if (num4 != -2 && num5 != -2 && num6 != -2)
			{
				list.Add(num4);
				list.Add(num5);
				list.Add(num6);
			}
		}
		Vector3[] array3 = (Vector3[])(object)new Vector3[array.Length];
		Vector2[] array4 = (Vector2[])(object)new Vector2[array.Length];
		Vector3[] array5 = (Vector3[])(object)new Vector3[array.Length];
		Vector4[] array6 = (Vector4[])(object)new Vector4[array.Length];
		for (int num7 = 0; num7 < array.Length; num7++)
		{
			int num8 = array[num7];
			array3[num7] = mesh.vertices[num8];
			array4[num7] = mesh.uv[num7];
			array5[num7] = mesh.normals[num8];
			array6[num7] = mesh.tangents[num8];
		}
		Mesh val = new Mesh
		{
			name = ((Object)mesh).name + "_CutMeshGeneric"
		};
		val.bounds = mesh.bounds;
		val.vertices = array3;
		val.normals = array5;
		val.tangents = array6;
		val.triangles = list.ToArray();
		return val;
	}
}
