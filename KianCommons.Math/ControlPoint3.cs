using UnityEngine;

namespace KianCommons.Math;

internal struct ControlPoint3(Vector3 point, Vector3 dir)
{
	public Vector3 Point = point;

	public Vector3 Dir = dir;

	public ControlPoint3 Reverse => new ControlPoint3(Point, -Dir);
}
