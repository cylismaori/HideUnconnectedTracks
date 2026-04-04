using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ObjUnity3D;

public static class MeshExt
{
	internal const int MESH_BINARY_HEADER_SIZE = 20;

	internal const short MESH_BINARY_SIGNATURE = 245;

	internal const short MESH_BINARY_VERSION = 1;

	public static void LoadOBJ(this Mesh lMesh, OBJData lData)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<int>[] array = new List<int>[lData.m_Groups.Count];
		Dictionary<OBJFaceVertex, int> dictionary = new Dictionary<OBJFaceVertex, int>();
		bool flag = lData.m_Normals.Count > 0;
		bool flag2 = lData.m_UVs.Count > 0;
		lMesh.subMeshCount = lData.m_Groups.Count;
		for (int i = 0; i < lData.m_Groups.Count; i++)
		{
			OBJGroup oBJGroup = lData.m_Groups[i];
			array[i] = new List<int>();
			for (int j = 0; j < oBJGroup.Faces.Count; j++)
			{
				OBJFace oBJFace = oBJGroup.Faces[j];
				for (int k = 1; k < oBJFace.Count - 1; k++)
				{
					int[] array2 = new int[3]
					{
						0,
						k,
						k + 1
					};
					foreach (int i2 in array2)
					{
						OBJFaceVertex oBJFaceVertex = oBJFace[i2];
						int value = -1;
						if (!dictionary.TryGetValue(oBJFaceVertex, out value))
						{
							dictionary[oBJFaceVertex] = list.Count;
							value = list.Count;
							list.Add(lData.m_Vertices[oBJFaceVertex.m_VertexIndex]);
							if (flag2)
							{
								list3.Add(lData.m_UVs[oBJFaceVertex.m_UVIndex]);
							}
							if (flag)
							{
								list2.Add(lData.m_Normals[oBJFaceVertex.m_NormalIndex]);
							}
						}
						array[i].Add(value);
					}
				}
			}
		}
		lMesh.triangles = new int[0];
		lMesh.vertices = list.ToArray();
		lMesh.uv = list3.ToArray();
		lMesh.normals = list2.ToArray();
		if (!flag)
		{
			lMesh.RecalculateNormals();
		}
		lMesh.RecalculateTangents();
		for (int m = 0; m < lData.m_Groups.Count; m++)
		{
			lMesh.SetTriangles(array[m].ToArray(), m);
		}
	}

	public static void LoadOBJ(this Mesh lMesh, OBJData lData, string subOject)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<int> list4 = new List<int>();
		Dictionary<OBJFaceVertex, int> dictionary = new Dictionary<OBJFaceVertex, int>();
		bool flag = lData.m_Normals.Count > 0;
		bool flag2 = lData.m_UVs.Count > 0;
		for (int i = 0; i < lData.m_Groups.Count; i++)
		{
			OBJGroup oBJGroup = lData.m_Groups[i];
			if (oBJGroup.m_Name != subOject)
			{
				continue;
			}
			for (int j = 0; j < oBJGroup.Faces.Count; j++)
			{
				OBJFace oBJFace = oBJGroup.Faces[j];
				for (int k = 1; k < oBJFace.Count - 1; k++)
				{
					int[] array = new int[3]
					{
						0,
						k,
						k + 1
					};
					foreach (int i2 in array)
					{
						OBJFaceVertex oBJFaceVertex = oBJFace[i2];
						int value = -1;
						if (!dictionary.TryGetValue(oBJFaceVertex, out value))
						{
							dictionary[oBJFaceVertex] = list.Count;
							value = list.Count;
							list.Add(lData.m_Vertices[oBJFaceVertex.m_VertexIndex]);
							if (flag2)
							{
								list3.Add(lData.m_UVs[oBJFaceVertex.m_UVIndex]);
							}
							if (flag)
							{
								list2.Add(lData.m_Normals[oBJFaceVertex.m_NormalIndex]);
							}
						}
						list4.Add(value);
					}
				}
			}
		}
		if (list4.Count != 0)
		{
			lMesh.vertices = list.ToArray();
			lMesh.triangles = list4.ToArray();
			lMesh.uv = list3.ToArray();
			lMesh.normals = list2.ToArray();
			if (!flag)
			{
				lMesh.RecalculateNormals();
			}
			lMesh.RecalculateTangents();
		}
	}

	public static OBJData EncodeOBJ(this Mesh lMesh)
	{
		OBJData oBJData = new OBJData
		{
			m_Vertices = new List<Vector3>(lMesh.vertices),
			m_UVs = new List<Vector2>(lMesh.uv),
			m_Normals = new List<Vector3>(lMesh.normals),
			m_UV2s = new List<Vector2>(lMesh.uv2),
			m_Colors = new List<Color>(lMesh.colors)
		};
		for (int i = 0; i < lMesh.subMeshCount; i++)
		{
			int[] triangles = lMesh.GetTriangles(i);
			OBJGroup oBJGroup = new OBJGroup(((Object)lMesh).name + "_" + i);
			for (int j = 0; j < triangles.Length; j += 3)
			{
				OBJFace oBJFace = new OBJFace();
				oBJFace.AddVertex(new OBJFaceVertex
				{
					m_VertexIndex = ((oBJData.m_Vertices.Count > 0) ? triangles[j] : (-1)),
					m_UVIndex = ((oBJData.m_UVs.Count > 0) ? triangles[j] : (-1)),
					m_NormalIndex = ((oBJData.m_Normals.Count > 0) ? triangles[j] : (-1)),
					m_UV2Index = ((oBJData.m_UV2s.Count > 0) ? triangles[j] : (-1)),
					m_ColorIndex = ((oBJData.m_Colors.Count > 0) ? triangles[j] : (-1))
				});
				oBJFace.AddVertex(new OBJFaceVertex
				{
					m_VertexIndex = ((oBJData.m_Vertices.Count > 0) ? triangles[j + 1] : (-1)),
					m_UVIndex = ((oBJData.m_UVs.Count > 0) ? triangles[j + 1] : (-1)),
					m_NormalIndex = ((oBJData.m_Normals.Count > 0) ? triangles[j + 1] : (-1)),
					m_UV2Index = ((oBJData.m_UV2s.Count > 0) ? triangles[j + 1] : (-1)),
					m_ColorIndex = ((oBJData.m_Colors.Count > 0) ? triangles[j + 1] : (-1))
				});
				oBJFace.AddVertex(new OBJFaceVertex
				{
					m_VertexIndex = ((oBJData.m_Vertices.Count > 0) ? triangles[j + 2] : (-1)),
					m_UVIndex = ((oBJData.m_UVs.Count > 0) ? triangles[j + 2] : (-1)),
					m_NormalIndex = ((oBJData.m_Normals.Count > 0) ? triangles[j + 2] : (-1)),
					m_UV2Index = ((oBJData.m_UV2s.Count > 0) ? triangles[j + 2] : (-1)),
					m_ColorIndex = ((oBJData.m_Colors.Count > 0) ? triangles[j + 2] : (-1))
				});
				oBJGroup.AddFace(oBJFace);
			}
			oBJData.m_Groups.Add(oBJGroup);
		}
		return oBJData;
	}

	public static bool LoadBinary(this Mesh lMesh, byte[] lData)
	{
		int num = Marshal.SizeOf(typeof(Vector2));
		int num2 = Marshal.SizeOf(typeof(Vector3));
		int num3 = Marshal.SizeOf(typeof(Vector4));
		int num4 = Marshal.SizeOf(typeof(Matrix4x4));
		int num5 = Marshal.SizeOf(typeof(BoneWeight));
		int num6 = Marshal.SizeOf(typeof(Color));
		int num7 = 20;
		if (lData == null || lData.Length < 20)
		{
			return false;
		}
		short num8 = BitConverter.ToInt16(lData, 0);
		short num9 = BitConverter.ToInt16(lData, 2);
		if (num8 != 245 || num9 != 1)
		{
			return false;
		}
		lMesh.Clear();
		int num10 = BitConverter.ToInt32(lData, 4);
		int num11 = BitConverter.ToInt32(lData, 8);
		int num12 = BitConverter.ToInt32(lData, 12);
		byte b = lData[16];
		bool flag = (b & 1) > 0;
		bool flag2 = (b & 2) > 0;
		bool flag3 = (b & 4) > 0;
		bool flag4 = (b & 8) > 0;
		bool flag5 = (b & 0x10) > 0;
		bool flag6 = (b & 0x20) > 0;
		bool flag7 = (b & 0x40) > 0;
		bool flag8 = (b & 0x80) > 0;
		Vector3[] array = (Vector3[])(object)new Vector3[num10];
		int num13 = array.Length * num2;
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
		gCHandle.Free();
		num7 += num13;
		lMesh.vertices = array;
		if (flag)
		{
			Vector2[] array2 = (Vector2[])(object)new Vector2[num10];
			num13 = array2.Length * num;
			gCHandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.uv = array2;
			Debug.Log((object)("UV Count : " + array2.Length));
		}
		if (flag2)
		{
			Vector2[] array3 = (Vector2[])(object)new Vector2[num10];
			num13 = array3.Length * num;
			gCHandle = GCHandle.Alloc(array3, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.uv2 = array3;
			Debug.Log((object)("UV1 Count : " + array3.Length));
		}
		if (flag3)
		{
			Vector2[] array4 = (Vector2[])(object)new Vector2[num10];
			num13 = array4.Length * num;
			gCHandle = GCHandle.Alloc(array4, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.uv2 = array4;
			Debug.Log((object)("UV2 Count : " + array4.Length));
		}
		if (flag4)
		{
			Vector3[] array5 = (Vector3[])(object)new Vector3[num10];
			num13 = array5.Length * num2;
			gCHandle = GCHandle.Alloc(array5, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.normals = array5;
			Debug.Log((object)("Normal Count : " + array5.Length));
		}
		if (flag5)
		{
			Vector4[] array6 = (Vector4[])(object)new Vector4[num10];
			num13 = array6.Length * num3;
			gCHandle = GCHandle.Alloc(array6, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.tangents = array6;
			Debug.Log((object)("Tangents Count : " + array6.Length));
		}
		if (flag6)
		{
			Color[] array7 = (Color[])(object)new Color[num10];
			num13 = array7.Length * num6;
			gCHandle = GCHandle.Alloc(array7, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.colors = array7;
		}
		if (flag7)
		{
			Matrix4x4[] array8 = (Matrix4x4[])(object)new Matrix4x4[num10];
			num13 = array8.Length * num4;
			gCHandle = GCHandle.Alloc(array8, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.bindposes = array8;
		}
		if (flag8)
		{
			BoneWeight[] array9 = (BoneWeight[])(object)new BoneWeight[num10];
			num13 = array9.Length * num5;
			gCHandle = GCHandle.Alloc(array9, GCHandleType.Pinned);
			Marshal.Copy(lData, num7, gCHandle.AddrOfPinnedObject(), num13);
			gCHandle.Free();
			num7 += num13;
			lMesh.boneWeights = array9;
		}
		int[] array10 = new int[num11];
		num13 = array10.Length * 4;
		Buffer.BlockCopy(lData, num7, array10, 0, num13);
		num7 += num13;
		lMesh.triangles = array10;
		for (int i = 0; i < num12; i++)
		{
			int num14 = BitConverter.ToInt32(lData, num7);
			num7 += 4;
			array10 = new int[num14];
			num13 = array10.Length * 4;
			Buffer.BlockCopy(lData, num7, array10, 0, num13);
			num7 += num13;
			if (array10.Length != 0 && array10.Length % 3 == 0)
			{
				lMesh.SetTriangles(array10, i);
			}
		}
		return true;
	}

	public static byte[] EncodeBinary(this Mesh lMesh)
	{
		int num = Marshal.SizeOf(typeof(Vector2));
		int num2 = Marshal.SizeOf(typeof(Vector3));
		int num3 = Marshal.SizeOf(typeof(Vector4));
		int num4 = Marshal.SizeOf(typeof(Matrix4x4));
		int num5 = Marshal.SizeOf(typeof(BoneWeight));
		int num6 = Marshal.SizeOf(typeof(Color));
		int num7 = 20;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		byte[] array = new byte[num7];
		Vector3[] vertices = lMesh.vertices;
		Int32Converter int32Converter = vertices.Length;
		int num8 = vertices.Length * num2;
		Array.Resize(ref array, num7 + num8);
		GCHandle gCHandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
		Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
		gCHandle.Free();
		num7 += num8;
		Vector2[] uv = lMesh.uv;
		if (uv.Length != 0)
		{
			flag = true;
			num8 = uv.Length * num;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(uv, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		uv = lMesh.uv2;
		if (uv.Length != 0)
		{
			flag2 = true;
			num8 = uv.Length * num;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(uv, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		uv = lMesh.uv2;
		if (uv.Length != 0)
		{
			flag3 = true;
			num8 = uv.Length * num;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(uv, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		Vector3[] normals = lMesh.normals;
		if (normals.Length != 0)
		{
			flag4 = true;
			num8 = normals.Length * num2;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(normals, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		Vector4[] tangents = lMesh.tangents;
		if (tangents.Length != 0)
		{
			flag5 = true;
			num8 = tangents.Length * num3;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(tangents, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		Color[] colors = lMesh.colors;
		if (colors.Length != 0)
		{
			flag6 = true;
			num8 = colors.Length * num6;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		Matrix4x4[] bindposes = lMesh.bindposes;
		if (bindposes.Length != 0)
		{
			flag7 = true;
			num8 = bindposes.Length * num4;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(bindposes, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		BoneWeight[] boneWeights = lMesh.boneWeights;
		if (boneWeights.Length != 0)
		{
			flag8 = true;
			num8 = boneWeights.Length * num5;
			Array.Resize(ref array, num7 + num8);
			gCHandle = GCHandle.Alloc(boneWeights, GCHandleType.Pinned);
			Marshal.Copy(gCHandle.AddrOfPinnedObject(), array, num7, num8);
			gCHandle.Free();
			num7 += num8;
		}
		int[] triangles = lMesh.triangles;
		Int32Converter int32Converter2 = triangles.Length;
		num8 = triangles.Length * 4;
		Array.Resize(ref array, num7 + num8);
		Buffer.BlockCopy(triangles, 0, array, num7, num8);
		num7 += num8;
		Int32Converter int32Converter3 = lMesh.subMeshCount;
		for (int i = 0; i < (int)int32Converter3; i++)
		{
			triangles = lMesh.GetTriangles(i);
			Int32Converter int32Converter4 = triangles.Length;
			num8 = 4 + triangles.Length * 4;
			Array.Resize(ref array, num7 + num8);
			array[num7] = int32Converter4.Byte1;
			array[num7 + 1] = int32Converter4.Byte2;
			array[num7 + 2] = int32Converter4.Byte3;
			array[num7 + 3] = int32Converter4.Byte4;
			Buffer.BlockCopy(triangles, 0, array, num7, num8 - 4);
			num7 += num8;
		}
		array[0] = 245;
		array[1] = 0;
		array[2] = 1;
		array[3] = 0;
		array[4] = int32Converter.Byte1;
		array[5] = int32Converter.Byte2;
		array[6] = int32Converter.Byte3;
		array[7] = int32Converter.Byte4;
		array[8] = int32Converter2.Byte1;
		array[9] = int32Converter2.Byte2;
		array[10] = int32Converter2.Byte3;
		array[11] = int32Converter2.Byte4;
		array[12] = int32Converter3.Byte1;
		array[13] = int32Converter3.Byte2;
		array[14] = int32Converter3.Byte3;
		array[15] = int32Converter3.Byte4;
		array[16] = (byte)((flag ? 1 : 0) | (flag2 ? 2 : 0) | (flag3 ? 4 : 0) | (flag4 ? 8 : 0) | (flag5 ? 16 : 0) | (flag6 ? 32 : 0) | (flag7 ? 64 : 0) | (flag8 ? 128 : 0));
		return array;
	}

	public static Mesh Clone(this Mesh mesh)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Mesh val = new Mesh();
		((Object)val).name = "clone";
		val.vertices = mesh.vertices;
		val.triangles = mesh.triangles;
		val.normals = mesh.normals;
		val.uv = mesh.uv;
		val.colors = mesh.colors;
		return val;
	}
}
