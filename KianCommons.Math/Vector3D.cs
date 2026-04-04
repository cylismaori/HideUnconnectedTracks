using System;
using UnityEngine;

namespace KianCommons.Math;

internal struct Vector3D
{
	public float x;

	public float z;

	public float h;

	public float this[int index]
	{
		get
		{
			return index switch
			{
				0 => x, 
				1 => z, 
				2 => h, 
				_ => throw new IndexOutOfRangeException("Invalid Vector3D index!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				x = value;
				break;
			case 1:
				z = value;
				break;
			case 2:
				h = value;
				break;
			default:
				throw new IndexOutOfRangeException("Invalid Vector3D index!");
			}
		}
	}

	public Vector3D normalized => Normalize(this);

	public float magnitude => Mathf.Sqrt(x * x + z * z + h * h);

	public float sqrMagnitude => x * x + z * z + h * h;

	public static Vector3D zero => Vector3.zero;

	public static Vector3D one => Vector3.one;

	public static Vector3D forward => Vector3.forward;

	public static Vector3D fwd => forward;

	public static Vector3D back => Vector3.back;

	public static Vector3D up => Vector3.up;

	public static Vector3D down => Vector3.down;

	public static Vector3D left => Vector3.left;

	public static Vector3D right => Vector3.right;

	public Vector3D(float x, float z, float h)
	{
		this.x = x;
		this.z = z;
		this.h = h;
	}

	public Vector3D(Vector2D vector2d, float h)
	{
		x = vector2d.x;
		z = vector2d.z;
		this.h = h;
	}

	public Vector3D(Vector3 v)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		x = v.x;
		z = v.z;
		h = v.y;
	}

	public void Set(float new_x, float new_y, float new_z)
	{
		x = new_x;
		h = new_y;
		z = new_z;
	}

	public Vector2D ToVector2D()
	{
		return new Vector2D(x, z);
	}

	public static implicit operator Vector3D(Vector3 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3D(v);
	}

	public static implicit operator Vector3(Vector3D v)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(v.x, v.h, v.z);
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ (h.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector3D vector3D))
		{
			return false;
		}
		return x.Equals(vector3D.x) && h.Equals(vector3D.h) && z.Equals(vector3D.z);
	}

	public static Vector3D Scale(Vector3D a, Vector3D b)
	{
		return new Vector3D(a.x * b.x, a.h * b.h, a.z * b.z);
	}

	public Vector3D Scale(Vector3D scale)
	{
		return Scale(this, scale);
	}

	public static Vector3D Cross(Vector3D lhs, Vector3D rhs)
	{
		return new Vector3D(lhs.h * rhs.z - lhs.z * rhs.h, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.h - lhs.h * rhs.x);
	}

	public static Vector3D Reflect(Vector3D inDirection, Vector3D inNormal)
	{
		return -2f * Dot(inNormal, inDirection) * inNormal + inDirection;
	}

	public static Vector3D Normalize(Vector3D value)
	{
		float num = value.magnitude;
		if (num > 1E-05f)
		{
			return value / num;
		}
		return zero;
	}

	public static float Dot(Vector3D lhs, Vector3D rhs)
	{
		return lhs.x * rhs.x + lhs.h * rhs.h + lhs.z * rhs.z;
	}

	public static Vector3D Project(Vector3D vector, Vector3D onNormal)
	{
		float num = Dot(onNormal, onNormal);
		if (num < Mathf.Epsilon)
		{
			return zero;
		}
		return onNormal * Dot(vector, onNormal) / num;
	}

	public static Vector3D ProjectOnPlane(Vector3D vector, Vector3D planeNormal)
	{
		return vector - Project(vector, planeNormal);
	}

	public static float Distance(Vector3D a, Vector3D b)
	{
		Vector3D vector3D = new Vector3D(a.x - b.x, a.h - b.h, a.z - b.z);
		return Mathf.Sqrt(vector3D.x * vector3D.x + vector3D.h * vector3D.h + vector3D.z * vector3D.z);
	}

	public static Vector3D ClampMagnitude(Vector3D vector, float maxLength)
	{
		if (vector.sqrMagnitude > maxLength * maxLength)
		{
			return vector.normalized * maxLength;
		}
		return vector;
	}

	public static Vector3D Min(Vector3D lhs, Vector3D rhs)
	{
		return new Vector3D(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.h, rhs.h), Mathf.Min(lhs.z, rhs.z));
	}

	public static Vector3D Max(Vector3D lhs, Vector3D rhs)
	{
		return new Vector3D(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.h, rhs.h), Mathf.Max(lhs.z, rhs.z));
	}

	public static Vector3D operator +(Vector3D a, Vector3D b)
	{
		return new Vector3D(a.x + b.x, a.h + b.h, a.z + b.z);
	}

	public static Vector3D operator -(Vector3D a, Vector3D b)
	{
		return new Vector3D(a.x - b.x, a.h - b.h, a.z - b.z);
	}

	public static Vector3D operator -(Vector3D a)
	{
		return new Vector3D(0f - a.x, 0f - a.h, 0f - a.z);
	}

	public static Vector3D operator *(Vector3D a, float d)
	{
		return new Vector3D(a.x * d, a.h * d, a.z * d);
	}

	public static Vector3D operator *(float d, Vector3D a)
	{
		return new Vector3D(a.x * d, a.h * d, a.z * d);
	}

	public static Vector3D operator /(Vector3D a, float d)
	{
		return new Vector3D(a.x / d, a.h / d, a.z / d);
	}

	public static bool operator ==(Vector3D lhs, Vector3D rhs)
	{
		return (lhs - rhs).magnitude < 9.9999994E-11f;
	}

	public static bool operator !=(Vector3D lhs, Vector3D rhs)
	{
		return !(lhs == rhs);
	}

	public override string ToString()
	{
		return $"(x:{x}, z:{z}, h:{h})";
	}

	public static float UnsignedAngleRad(Vector3D from, Vector3D to)
	{
		return Mathf.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f));
	}

	public static float UnsighedAngleRad(Vector3D from, Vector3D to)
	{
		return Mathf.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f));
	}
}
