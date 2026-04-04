using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ObjUnity3D;

public static class Utils
{
	public static bool HasKeys(Dictionary<string, object> lData, params string[] lKeys)
	{
		if (lKeys != null)
		{
			for (int i = 0; i < lKeys.Length; i++)
			{
				if (!lData.ContainsKey(lKeys[i]))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static void ClearChildren(GameObject lGo, string lTarget)
	{
		if (!((Object)(object)lGo != (Object)null))
		{
			return;
		}
		for (int num = lGo.transform.childCount - 1; num > -1; num--)
		{
			Transform child = lGo.transform.GetChild(num);
			if (((Object)child).name.Contains(lTarget))
			{
				child.parent = null;
				Object.Destroy((Object)(object)((Component)child).gameObject);
			}
		}
	}

	public static void ClearChildrenRegex(GameObject lGo, string lPattern)
	{
		if (!((Object)(object)lGo != (Object)null))
		{
			return;
		}
		Regex regex = new Regex(lPattern);
		for (int num = lGo.transform.childCount - 1; num > -1; num--)
		{
			Transform child = lGo.transform.GetChild(num);
			if (regex.IsMatch(((Object)child).name))
			{
				child.parent = null;
				Object.Destroy((Object)(object)((Component)child).gameObject);
			}
		}
	}

	public static void VerifyObjects(string lMsg, params object[] lObjects)
	{
		for (int i = 0; i < lObjects.Length; i++)
		{
			if (lObjects[i] == null)
			{
				Debug.LogError((object)lMsg);
				break;
			}
		}
	}

	public static bool JSONCheck(string lText)
	{
		return !string.IsNullOrEmpty(lText) && lText[0] == '{';
	}

	public static Vector3 ParseVector3Json(string lJsonData)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lJsonData.Replace("(", "").Replace(")", "").Replace(" ", "")
			.Split(',');
		Vector3 zero = Vector3.zero;
		if (!float.TryParse(array[0], out zero.x))
		{
			return Vector3.zero;
		}
		if (!float.TryParse(array[1], out zero.y))
		{
			return Vector3.zero;
		}
		if (!float.TryParse(array[2], out zero.z))
		{
			return Vector3.zero;
		}
		return zero;
	}

	public static Vector4 ParseVector4Json(string lJsonData)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lJsonData.Replace("(", "").Replace(")", "").Replace(" ", "")
			.Split(',');
		Vector4 zero = Vector4.zero;
		if (!float.TryParse(array[0], out zero.x))
		{
			return Vector4.zero;
		}
		if (!float.TryParse(array[1], out zero.y))
		{
			return Vector4.zero;
		}
		if (!float.TryParse(array[2], out zero.z))
		{
			return Vector4.zero;
		}
		if (!float.TryParse(array[3], out zero.w))
		{
			return Vector4.zero;
		}
		return zero;
	}

	public static Vector2 ParseVector2String(string lData, char lSeperator = ' ')
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lData.Split(new char[1] { lSeperator }, StringSplitOptions.RemoveEmptyEntries);
		float num = array[0].ParseInvariantFloat();
		float num2 = array[1].ParseInvariantFloat();
		return new Vector2(num, num2);
	}

	public static Vector3 ParseVector3String(string lData, char lSeperator = ' ')
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lData.Split(new char[1] { lSeperator }, StringSplitOptions.RemoveEmptyEntries);
		float num = array[0].ParseInvariantFloat();
		float num2 = array[1].ParseInvariantFloat();
		float num3 = array[2].ParseInvariantFloat();
		return new Vector3(num, num2, num3);
	}

	public static Vector4 ParseVector4String(string lData, char lSeperator = ' ')
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lData.Split(new char[1] { lSeperator }, StringSplitOptions.RemoveEmptyEntries);
		float num = array[0].ParseInvariantFloat();
		float num2 = array[1].ParseInvariantFloat();
		float num3 = array[2].ParseInvariantFloat();
		float num4 = array[3].ParseInvariantFloat();
		return new Vector4(num, num2, num3, num4);
	}

	public static Quaternion ParseQuaternion(string lJsonData)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		string[] array = lJsonData.Replace("(", "").Replace(")", "").Replace(" ", "")
			.Split(',');
		Quaternion identity = Quaternion.identity;
		if (!float.TryParse(array[0], out identity.x))
		{
			return Quaternion.identity;
		}
		if (!float.TryParse(array[1], out identity.y))
		{
			return Quaternion.identity;
		}
		if (!float.TryParse(array[2], out identity.z))
		{
			return Quaternion.identity;
		}
		if (!float.TryParse(array[3], out identity.w))
		{
			return Quaternion.identity;
		}
		return identity;
	}

	public static string Vector3String(Vector3 lVector3)
	{
		return "(" + lVector3.x.ToString("f3") + "," + lVector3.y.ToString("f3") + "," + lVector3.z.ToString("f3") + ")";
	}

	public static string Vector4String(Vector4 lVector4)
	{
		return "(" + lVector4.x.ToString("f3") + "," + lVector4.y.ToString("f3") + "," + lVector4.z.ToString("f3") + "," + lVector4.w.ToString("f3") + ")";
	}

	public static string QuaternionString(Quaternion lQuaternion)
	{
		return "(" + lQuaternion.x.ToString("f3") + "," + lQuaternion.y.ToString("f3") + "," + lQuaternion.z.ToString("f3") + "," + lQuaternion.w.ToString("f3") + ")";
	}

	public static int FirstInt(string lJsonData)
	{
		string text = "";
		for (int i = 0; i < lJsonData.Length && char.IsDigit(lJsonData[i]); i++)
		{
			text += lJsonData[i];
		}
		return int.Parse(text);
	}
}
