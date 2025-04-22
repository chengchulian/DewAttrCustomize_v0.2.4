using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace;

public static class FEngineering
{
	private static Plane axis2DProjection;

	private static PhysicMaterial _slidingMat;

	private static PhysicMaterial _frictMat;

	private static PhysicsMaterial2D _slidingMat2D;

	private static PhysicsMaterial2D _frictMat2D;

	public static PhysicMaterial PMSliding
	{
		get
		{
			if ((bool)_slidingMat)
			{
				return _slidingMat;
			}
			_slidingMat = new PhysicMaterial("Slide");
			_slidingMat.frictionCombine = PhysicMaterialCombine.Minimum;
			_slidingMat.dynamicFriction = 0f;
			_slidingMat.staticFriction = 0f;
			return _slidingMat;
		}
	}

	public static PhysicMaterial PMFrict
	{
		get
		{
			if ((bool)_frictMat)
			{
				return _frictMat;
			}
			_frictMat = new PhysicMaterial("Friction");
			_frictMat.frictionCombine = PhysicMaterialCombine.Maximum;
			_frictMat.dynamicFriction = 10f;
			_frictMat.staticFriction = 10f;
			return _frictMat;
		}
	}

	public static PhysicsMaterial2D PMSliding2D
	{
		get
		{
			if ((bool)_slidingMat2D)
			{
				return _slidingMat2D;
			}
			_slidingMat2D = new PhysicsMaterial2D("Slide2D");
			_slidingMat2D.friction = 0f;
			return _slidingMat2D;
		}
	}

	public static PhysicsMaterial2D PMFrict2D
	{
		get
		{
			if ((bool)_frictMat2D)
			{
				return _frictMat2D;
			}
			_frictMat2D = new PhysicsMaterial2D("Friction2D");
			_frictMat2D.friction = 5f;
			return _frictMat2D;
		}
	}

	public static bool VIsZero(this Vector3 vec)
	{
		if (vec.sqrMagnitude == 0f)
		{
			return true;
		}
		return false;
	}

	public static bool VIsSame(this Vector3 vec1, Vector3 vec2)
	{
		if (vec1.x != vec2.x)
		{
			return false;
		}
		if (vec1.y != vec2.y)
		{
			return false;
		}
		if (vec1.z != vec2.z)
		{
			return false;
		}
		return true;
	}

	public static Vector3 TransformVector(this Quaternion parentRot, Vector3 parentLossyScale, Vector3 childLocalPos)
	{
		return parentRot * Vector3.Scale(childLocalPos, parentLossyScale);
	}

	public static Vector3 TransformInDirection(this Quaternion childRotation, Vector3 parentLossyScale, Vector3 childLocalPos)
	{
		return childRotation * Vector3.Scale(childLocalPos, new Vector3((parentLossyScale.x > 0f) ? 1 : (-1), (parentLossyScale.y > 0f) ? 1 : (-1), (parentLossyScale.y > 0f) ? 1 : (-1)));
	}

	public static Vector3 InverseTransformVector(this Quaternion tRotation, Vector3 tLossyScale, Vector3 worldPos)
	{
		worldPos = Quaternion.Inverse(tRotation) * worldPos;
		return new Vector3(worldPos.x / tLossyScale.x, worldPos.y / tLossyScale.y, worldPos.z / tLossyScale.z);
	}

	public static Vector3 VAxis2DLimit(this Transform parent, Vector3 parentPos, Vector3 childPos, int axis = 3)
	{
		switch (axis)
		{
		case 3:
			axis2DProjection.SetNormalAndPosition(parent.forward, parentPos);
			break;
		case 2:
			axis2DProjection.SetNormalAndPosition(parent.up, parentPos);
			break;
		default:
			axis2DProjection.SetNormalAndPosition(parent.right, parentPos);
			break;
		}
		return axis2DProjection.normal * axis2DProjection.GetDistanceToPoint(childPos);
	}

	public static Quaternion QToLocal(this Quaternion parentRotation, Quaternion worldRotation)
	{
		return Quaternion.Inverse(parentRotation) * worldRotation;
	}

	public static Quaternion QToWorld(this Quaternion parentRotation, Quaternion localRotation)
	{
		return parentRotation * localRotation;
	}

	public static Quaternion QRotateChild(this Quaternion offset, Quaternion parentRot, Quaternion childLocalRot)
	{
		return offset * parentRot * childLocalRot;
	}

	public static Quaternion ClampRotation(this Vector3 current, Vector3 bounds)
	{
		WrapVector(current);
		if (current.x < 0f - bounds.x)
		{
			current.x = 0f - bounds.x;
		}
		else if (current.x > bounds.x)
		{
			current.x = bounds.x;
		}
		if (current.y < 0f - bounds.y)
		{
			current.y = 0f - bounds.y;
		}
		else if (current.y > bounds.y)
		{
			current.y = bounds.y;
		}
		if (current.z < 0f - bounds.z)
		{
			current.z = 0f - bounds.z;
		}
		else if (current.z > bounds.z)
		{
			current.z = bounds.z;
		}
		return Quaternion.Euler(current);
	}

	public static Vector3 QToAngularVelocity(this Quaternion deltaRotation, bool fix = false)
	{
		deltaRotation.ToAngleAxis(out var angle, out var axis);
		if (angle != 0f)
		{
			angle = Mathf.DeltaAngle(0f, angle);
			axis *= angle * (MathF.PI / 180f);
			if (fix)
			{
				axis /= Time.fixedDeltaTime;
			}
			if (float.IsNaN(axis.x))
			{
				return Vector3.zero;
			}
			if (float.IsNaN(axis.y))
			{
				return Vector3.zero;
			}
			if (float.IsNaN(axis.z))
			{
				return Vector3.zero;
			}
			return axis;
		}
		return Vector3.zero;
	}

	public static Vector3 QToAngularVelocity(this Quaternion currentRotation, Quaternion targetRotation, bool fix = false)
	{
		return (targetRotation * Quaternion.Inverse(currentRotation)).QToAngularVelocity(fix);
	}

	public static bool QIsZero(this Quaternion rot)
	{
		if (rot.x != 0f)
		{
			return false;
		}
		if (rot.y != 0f)
		{
			return false;
		}
		if (rot.z != 0f)
		{
			return false;
		}
		return true;
	}

	public static bool QIsSame(this Quaternion rot1, Quaternion rot2)
	{
		if (rot1.x != rot2.x)
		{
			return false;
		}
		if (rot1.y != rot2.y)
		{
			return false;
		}
		if (rot1.z != rot2.z)
		{
			return false;
		}
		if (rot1.w != rot2.w)
		{
			return false;
		}
		return true;
	}

	public static float WrapAngle(float angle)
	{
		angle %= 360f;
		if (angle > 180f)
		{
			return angle - 360f;
		}
		return angle;
	}

	public static Vector3 WrapVector(Vector3 angles)
	{
		return new Vector3(WrapAngle(angles.x), WrapAngle(angles.y), WrapAngle(angles.z));
	}

	public static float UnwrapAngle(float angle)
	{
		if (angle >= 0f)
		{
			return angle;
		}
		angle = (0f - angle) % 360f;
		return 360f - angle;
	}

	public static Vector3 UnwrapVector(Vector3 angles)
	{
		return new Vector3(UnwrapAngle(angles.x), UnwrapAngle(angles.y), UnwrapAngle(angles.z));
	}

	public static Quaternion SmoothDampRotation(this Quaternion current, Quaternion target, ref Quaternion velocityRef, float duration, float delta)
	{
		return current.SmoothDampRotation(target, ref velocityRef, duration, float.PositiveInfinity, delta);
	}

	public static Quaternion SmoothDampRotation(this Quaternion current, Quaternion target, ref Quaternion velocityRef, float duration, float maxSpeed, float delta)
	{
		float sign = ((Quaternion.Dot(current, target) > 0f) ? 1f : (-1f));
		target.x *= sign;
		target.y *= sign;
		target.z *= sign;
		target.w *= sign;
		Vector4 smoothVal = new Vector4(Mathf.SmoothDamp(current.x, target.x, ref velocityRef.x, duration, maxSpeed, delta), Mathf.SmoothDamp(current.y, target.y, ref velocityRef.y, duration, maxSpeed, delta), Mathf.SmoothDamp(current.z, target.z, ref velocityRef.z, duration, maxSpeed, delta), Mathf.SmoothDamp(current.w, target.w, ref velocityRef.w, duration, maxSpeed, delta)).normalized;
		Vector4 correction = Vector4.Project(new Vector4(velocityRef.x, velocityRef.y, velocityRef.z, velocityRef.w), smoothVal);
		velocityRef.x -= correction.x;
		velocityRef.y -= correction.y;
		velocityRef.z -= correction.z;
		velocityRef.w -= correction.w;
		return new Quaternion(smoothVal.x, smoothVal.y, smoothVal.z, smoothVal.w);
	}

	public static float PerlinNoise3D(float x, float y, float z)
	{
		y += 1f;
		z += 2f;
		float num = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(x, y));
		float xz = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(x, z));
		float yz = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(y, z));
		float yx = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(y, x));
		float zx = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(z, x));
		float zy = Mathf.Sin(MathF.PI * Mathf.PerlinNoise(z, y));
		return num * xz * yz * yx * zx * zy;
	}

	public static float PerlinNoise3D(Vector3 pos)
	{
		return PerlinNoise3D(pos.x, pos.y, pos.z);
	}

	public static bool SameDirection(this float a, float b)
	{
		if (!(a > 0f) || !(b > 0f))
		{
			if (a < 0f)
			{
				return b < 0f;
			}
			return false;
		}
		return true;
	}

	public static float PointDisperse01(int index, int baseV = 2)
	{
		float sum = 0f;
		float functionV = 1f / (float)baseV;
		int i = index;
		while (i > 0)
		{
			sum += functionV * (float)(i % baseV);
			i = Mathf.FloorToInt(i / baseV);
			functionV /= (float)baseV;
		}
		return sum;
	}

	public static float PointDisperse(int index, int baseV = 2)
	{
		float sum = 0f;
		float functionV = 1f / (float)baseV;
		int i = index;
		while (i > 0)
		{
			sum += functionV * (float)(i % baseV);
			i = Mathf.FloorToInt(i / baseV);
			functionV /= (float)baseV;
		}
		return sum - 0.5f;
	}

	public static float GetScaler(this Transform transform)
	{
		if (transform.lossyScale.x > transform.lossyScale.y)
		{
			if (transform.lossyScale.y > transform.lossyScale.z)
			{
				return transform.lossyScale.y;
			}
			return transform.lossyScale.z;
		}
		return transform.lossyScale.x;
	}

	public static Vector3 PosFromMatrix(this Matrix4x4 m)
	{
		return m.GetColumn(3);
	}

	public static Quaternion RotFromMatrix(this Matrix4x4 m)
	{
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}

	public static Vector3 ScaleFromMatrix(this Matrix4x4 m)
	{
		return new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
	}

	public static Bounds TransformBounding(Bounds b, Transform by)
	{
		return TransformBounding(b, by.localToWorldMatrix);
	}

	public static Bounds TransformBounding(Bounds b, Matrix4x4 mx)
	{
		Vector3 min = mx.MultiplyPoint(b.min);
		Vector3 max = mx.MultiplyPoint(b.max);
		Vector3 minB = mx.MultiplyPoint(new Vector3(b.max.x, b.center.y, b.min.z));
		Vector3 maxB = mx.MultiplyPoint(new Vector3(b.min.x, b.center.y, b.max.z));
		b = new Bounds(min, Vector3.zero);
		b.Encapsulate(min);
		b.Encapsulate(max);
		b.Encapsulate(minB);
		b.Encapsulate(maxB);
		return b;
	}

	public static Bounds RotateBoundsByMatrix(this Bounds b, Quaternion rotation)
	{
		if (rotation.QIsZero())
		{
			return b;
		}
		Matrix4x4 rot = Matrix4x4.Rotate(rotation);
		Bounds newB = default(Bounds);
		Vector3 fr1 = rot.MultiplyPoint(new Vector3(b.max.x, b.min.y, b.max.z));
		Vector3 br1 = rot.MultiplyPoint(new Vector3(b.max.x, b.min.y, b.min.z));
		Vector3 bl1 = rot.MultiplyPoint(new Vector3(b.min.x, b.min.y, b.min.z));
		Vector3 fl1 = rot.MultiplyPoint(new Vector3(b.min.x, b.min.y, b.max.z));
		newB.Encapsulate(fr1);
		newB.Encapsulate(br1);
		newB.Encapsulate(bl1);
		newB.Encapsulate(fl1);
		Vector3 fr2 = rot.MultiplyPoint(new Vector3(b.max.x, b.max.y, b.max.z));
		Vector3 br2 = rot.MultiplyPoint(new Vector3(b.max.x, b.max.y, b.min.z));
		Vector3 bl2 = rot.MultiplyPoint(new Vector3(b.min.x, b.max.y, b.min.z));
		Vector3 fl2 = rot.MultiplyPoint(new Vector3(b.min.x, b.max.y, b.max.z));
		newB.Encapsulate(fr2);
		newB.Encapsulate(br2);
		newB.Encapsulate(bl2);
		newB.Encapsulate(fl2);
		return newB;
	}

	public static Bounds RotateLocalBounds(this Bounds b, Quaternion rotation)
	{
		float angle = Quaternion.Angle(rotation, Quaternion.identity);
		if (angle > 45f && angle < 135f)
		{
			b.size = new Vector3(b.size.z, b.size.y, b.size.x);
		}
		if (angle < 315f && angle > 225f)
		{
			b.size = new Vector3(b.size.z, b.size.y, b.size.x);
		}
		return b;
	}

	public static int[] GetLayermaskValues(int mask, int optionsCount)
	{
		List<int> masks = new List<int>();
		for (int i = 0; i < optionsCount; i++)
		{
			int layer = 1 << i;
			if ((mask & layer) != 0)
			{
				masks.Add(i);
			}
		}
		return masks.ToArray();
	}

	public static LayerMask GetLayerMaskUsingPhysicsProjectSettingsMatrix(int maskForLayer)
	{
		LayerMask layerMask = 0;
		for (int i = 0; i < 32; i++)
		{
			if (!Physics.GetIgnoreLayerCollision(maskForLayer, i))
			{
				layerMask = (int)layerMask | (1 << i);
			}
		}
		return layerMask;
	}

	public static float DistanceTo_2D(Vector3 aPos, Vector3 bPos)
	{
		return Vector2.Distance(new Vector2(aPos.x, aPos.z), new Vector2(bPos.x, bPos.z));
	}

	public static float DistanceTo_2DSqrt(Vector3 aPos, Vector3 bPos)
	{
		return Vector2.SqrMagnitude(new Vector2(aPos.x, aPos.z) - new Vector2(bPos.x, bPos.z));
	}

	public static Vector2 GetAngleDirection2D(float angle)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector2(Mathf.Sin(degToRad), Mathf.Cos(degToRad));
	}

	public static Vector3 GetAngleDirection(float angle)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(Mathf.Sin(degToRad), 0f, Mathf.Cos(degToRad));
	}

	public static Vector3 GetAngleDirectionXZ(float angle)
	{
		return GetAngleDirection(angle);
	}

	public static Vector3 GetAngleDirectionZX(float angle)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(Mathf.Cos(degToRad), 0f, Mathf.Sin(degToRad));
	}

	public static Vector3 GetAngleDirectionXY(float angle, float radOffset = 0f, float secAxisRadOffset = 0f)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(Mathf.Sin(degToRad + radOffset), Mathf.Cos(degToRad + secAxisRadOffset), 0f);
	}

	public static Vector3 GetAngleDirectionYX(float angle, float firstAxisRadOffset = 0f, float secAxisRadOffset = 0f)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(Mathf.Cos(degToRad + secAxisRadOffset), Mathf.Sin(degToRad + firstAxisRadOffset), 0f);
	}

	public static Vector3 GetAngleDirectionYZ(float angle)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(0f, Mathf.Sin(degToRad), Mathf.Cos(degToRad));
	}

	public static Vector3 GetAngleDirectionZY(float angle)
	{
		float degToRad = angle * (MathF.PI / 180f);
		return new Vector3(0f, Mathf.Cos(degToRad), Mathf.Sin(degToRad));
	}

	public static Vector3 V2ToV3TopDown(Vector2 v)
	{
		return new Vector3(v.x, 0f, v.y);
	}

	public static Vector2 V3ToV2(Vector3 a)
	{
		return new Vector2(a.x, a.z);
	}

	public static Vector2 V3TopDownDiff(Vector3 target, Vector3 me)
	{
		return V3ToV2(target) - V3ToV2(me);
	}

	public static float GetAngleDeg(Vector3 v)
	{
		return GetAngleDeg(v.x, v.z);
	}

	public static float GetAngleDeg(Vector2 v)
	{
		return GetAngleDeg(v.x, v.y);
	}

	public static float GetAngleDeg(float x, float z)
	{
		return GetAngleRad(x, z) * 57.29578f;
	}

	public static float GetAngleRad(float x, float z)
	{
		return Mathf.Atan2(x, z);
	}

	public static float Rnd(float val, int dec = 0)
	{
		if (dec <= 0)
		{
			return Mathf.Round(val);
		}
		return (float)Math.Round(val, dec);
	}

	internal static float ManhattanTopDown2D(Vector3 probePos, Vector3 worldPosition)
	{
		float d1 = probePos.x - worldPosition.x;
		if (d1 < 0f)
		{
			d1 = 0f - d1;
		}
		float d2 = probePos.z - worldPosition.z;
		if (d2 < 0f)
		{
			d2 = 0f - d2;
		}
		return d1 + d2;
	}

	internal static bool IsInSqureBounds2D(Vector3 probePos, Vector3 boundsPos, float boundsRange)
	{
		if (boundsRange <= 0f)
		{
			return false;
		}
		if (probePos.x > boundsPos.x - boundsRange && probePos.x < boundsPos.x + boundsRange && probePos.z > boundsPos.z - boundsRange && probePos.z < boundsPos.z + boundsRange)
		{
			return true;
		}
		return false;
	}

	internal static bool IsInSqureBounds2D(Vector3 boundsAPos, float boundsAHalfRange, Vector3 boundsBPos, float boundsBHRange)
	{
		if (boundsAPos.x - boundsAHalfRange <= boundsBPos.x + boundsBHRange && boundsAPos.x + boundsAHalfRange >= boundsBPos.x - boundsBHRange && boundsAPos.z - boundsAHalfRange <= boundsBPos.z + boundsBHRange)
		{
			return boundsAPos.z + boundsAHalfRange >= boundsBPos.z - boundsBHRange;
		}
		return false;
	}

	internal static Vector3 GetDirectionTowards(Vector3 me, Vector3 target)
	{
		return new Vector3(target.x - me.x, 0f, target.z - me.z);
	}
}
