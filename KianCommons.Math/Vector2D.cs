using System.Runtime.CompilerServices;
using UnityEngine;

namespace KianCommons.Math;

internal struct Vector2D
{
	private Vector2 _vector2;

	public const float kEpsilon = 1E-05f;

	public float x => _vector2.x;

	public float z => _vector2.y;

	public float this[int index]
	{
		get
		{
			return ((Vector2)(ref _vector2))[index];
		}
		set
		{
			((Vector2)(ref _vector2))[index] = value;
		}
	}

	public static Vector2D down => Vector2.down;

	public static Vector2D up => Vector2.up;

	public static Vector2D one => Vector2.one;

	public static Vector2D zero => Vector2.zero;

	public static Vector2D left => Vector2.left;

	public static Vector2D right => Vector2.right;

	public float magnitude => ((Vector2)(ref _vector2)).magnitude;

	public float sqrMagnitude => ((Vector2)(ref _vector2)).sqrMagnitude;

	public Vector2D normalized => ((Vector2)(ref _vector2)).normalized;

	public Vector2D(float x, float z)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_vector2 = new Vector2(x, z);
	}

	public Vector2D(Vector2 v)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_vector2 = v;
	}

	public void Set(float newX, float newY)
	{
		((Vector2)(ref _vector2)).Set(newX, newY);
	}

	public Vector3D ToVector3D(float h = 0f)
	{
		return new Vector3D(this, h);
	}

	public static implicit operator Vector2D(Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2D(v);
	}

	public static implicit operator Vector2(Vector2D v)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return v._vector2;
	}

	public override bool Equals(object other)
	{
		if (!(other is Vector2D vector2D))
		{
			return false;
		}
		return x == vector2D.x && z == vector2D.z;
	}

	public override int GetHashCode()
	{
		return ((object)System.Runtime.CompilerServices.Unsafe.As<Vector2, Vector2>(ref _vector2)/*cast due to .constrained prefix*/).GetHashCode();
	}

	public override string ToString()
	{
		return $"Vector2D({x},{z})";
	}

	public string ToString(string format)
	{
		return "Vector2D(" + x.ToString(format) + "," + z.ToString(format) + ")";
	}

	public static Vector2D operator +(Vector2D a, Vector2D b)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return a._vector2 + b._vector2;
	}

	public static Vector2D operator -(Vector2D a)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return -a._vector2;
	}

	public static Vector2D operator -(Vector2D a, Vector2D b)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return a._vector2 - b._vector2;
	}

	public static Vector2D operator *(float d, Vector2D a)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return d * a._vector2;
	}

	public static Vector2D operator *(Vector2D a, float d)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return a._vector2 * d;
	}

	public static Vector2D operator /(Vector2D a, float d)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return a._vector2 / d;
	}

	public static bool operator ==(Vector2D lhs, Vector2D rhs)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return lhs._vector2 == rhs._vector2;
	}

	public static bool operator !=(Vector2D lhs, Vector2D rhs)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return lhs._vector2 != rhs._vector2;
	}

	public Vector2D Scale(Vector2D scale)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Scale((Vector2)this, (Vector2)scale);
	}

	public Vector2D Extend(float magnitude)
	{
		return NewMagnitude(magnitude + this.magnitude);
	}

	public Vector2D NewMagnitude(float magnitude)
	{
		return magnitude * normalized;
	}

	public static float UnsignedAngleRad(Vector2D v1, Vector2D v2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Dot((Vector2)v1, (Vector2)v2);
		float num2 = Mathf.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude);
		return Mathf.Acos(num / num2);
	}

	public static float Determinent(Vector2D v1, Vector2D v2)
	{
		return v1.x * v2.z - v1.z * v2.x;
	}

	public static Vector2D Vector2ByAgnleRad(float magnitude, float angle)
	{
		return new Vector2D(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
	}

	public float SignedAngleRadCCW()
	{
		return Mathf.Atan2(x, z);
	}

	public static float SignedAngleRadCCW(Vector2D v1, Vector2D v2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Dot((Vector2)v1, (Vector2)v2);
		float num2 = Determinent(v1, v2);
		return Mathf.Atan2(num2, num);
	}

	public Vector2D Rotate90CCW()
	{
		return new Vector2D(0f - z, x);
	}

	public Vector2D PerpendicularCCW()
	{
		return normalized.Rotate90CCW();
	}

	public Vector2D Rotate90CW()
	{
		return new Vector2D(z, 0f - x);
	}

	public Vector2D PerpendicularCC()
	{
		return normalized.Rotate90CW();
	}
}
