using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using KianCommons;
using UnityEngine;

namespace HideUnconnectedTracks;

public class MeshTable
{
	public static MeshTable MeshLUT = new MeshTable();

	private Hashtable _meshTable = new Hashtable(1000);

	private Dictionary<string, NodeInfoFamily> _md5Table = new Dictionary<string, NodeInfoFamily>(1000);

	private const bool vertexBasedMD5_ = false;

	public NodeInfoFamily this[Mesh key]
	{
		get
		{
			if ((Object)(object)key == (Object)null)
			{
				return null;
			}
			NodeInfoFamily result = _meshTable[key] as NodeInfoFamily;
			bool flag = false;
			return result;
		}
		set
		{
			Assertion.AssertNotNull(value);
			Assertion.AssertNotNull(key);
			_meshTable[key] = value;
			bool flag = false;
		}
	}

	public static byte[] ToBytes(Vector3[] v)
	{
		byte[] array = new byte[v.Length * 12];
		for (int i = 0; i < v.Length; i++)
		{
			byte[] bytes = BitConverter.GetBytes(v[i].x);
			byte[] bytes2 = BitConverter.GetBytes(v[i].y);
			byte[] bytes3 = BitConverter.GetBytes(v[i].z);
			Buffer.BlockCopy(bytes, 0, array, i * 12, 0);
			Buffer.BlockCopy(bytes, 0, array, i * 12 + 4, 4);
			Buffer.BlockCopy(bytes, 0, array, i * 12 + 8, 4);
		}
		return array;
	}

	public static string ToMD5(Vector3[] vertices)
	{
		using MD5 mD = MD5.Create();
		byte[] buffer = ToBytes(vertices);
		byte[] array = mD.ComputeHash(buffer);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
