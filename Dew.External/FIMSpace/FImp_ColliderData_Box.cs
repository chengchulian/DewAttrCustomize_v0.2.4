using UnityEngine;

namespace FIMSpace;

public class FImp_ColliderData_Box : FImp_ColliderData_Base
{
	private Vector3 boxCenter;

	private Vector3 right;

	private Vector3 up;

	private Vector3 forward;

	private Vector3 rightN;

	private Vector3 upN;

	private Vector3 forwardN;

	private Vector3 scales;

	public BoxCollider Box { get; private set; }

	public BoxCollider2D Box2D { get; private set; }

	public FImp_ColliderData_Box(BoxCollider collider)
	{
		Is2D = false;
		base.Collider = collider;
		base.Transform = collider.transform;
		Box = collider;
		base.ColliderType = EFColliderType.Box;
		RefreshColliderData();
		previousPosition = base.Transform.position + Vector3.forward * Mathf.Epsilon;
	}

	public FImp_ColliderData_Box(BoxCollider2D collider2D)
	{
		Is2D = true;
		base.Collider2D = collider2D;
		base.Transform = collider2D.transform;
		Box2D = collider2D;
		base.ColliderType = EFColliderType.Box;
		RefreshColliderData();
		previousPosition = base.Transform.position + Vector3.forward * Mathf.Epsilon;
	}

	public override void RefreshColliderData()
	{
		if (base.IsStatic)
		{
			return;
		}
		if (base.Collider2D == null)
		{
			bool diff = false;
			if (!base.Transform.position.VIsSame(previousPosition))
			{
				diff = true;
			}
			else if (!base.Transform.rotation.QIsSame(previousRotation))
			{
				diff = true;
			}
			if (diff)
			{
				right = Box.transform.TransformVector(Vector3.right / 2f * Box.size.x);
				up = Box.transform.TransformVector(Vector3.up / 2f * Box.size.y);
				forward = Box.transform.TransformVector(Vector3.forward / 2f * Box.size.z);
				rightN = right.normalized;
				upN = up.normalized;
				forwardN = forward.normalized;
				boxCenter = GetBoxCenter(Box);
				scales = Vector3.Scale(Box.size, Box.transform.lossyScale);
				scales.Normalize();
			}
		}
		else
		{
			bool diff2 = false;
			if (Vector2.Distance(base.Transform.position, previousPosition) > Mathf.Epsilon)
			{
				diff2 = true;
			}
			else if (!base.Transform.rotation.QIsSame(previousRotation))
			{
				diff2 = true;
			}
			if (diff2)
			{
				right = Box2D.transform.TransformVector(Vector3.right / 2f * Box2D.size.x);
				up = Box2D.transform.TransformVector(Vector3.up / 2f * Box2D.size.y);
				rightN = right.normalized;
				upN = up.normalized;
				boxCenter = GetBoxCenter(Box2D);
				boxCenter.z = 0f;
				Vector3 scale = base.Transform.lossyScale;
				scale.z = 1f;
				scales = Vector3.Scale(Box2D.size, scale);
				scales.Normalize();
			}
		}
		base.RefreshColliderData();
		previousPosition = base.Transform.position;
		previousRotation = base.Transform.rotation;
	}

	public override bool PushIfInside(ref Vector3 segmentPosition, float segmentRadius, Vector3 segmentOffset)
	{
		int inOrInt = 0;
		Vector3 interPlane = Vector3.zero;
		Vector3 segmentOffsetted = segmentPosition + segmentOffset;
		float planeDistance = PlaneDistance(boxCenter + up, upN, segmentOffsetted);
		if (SphereInsidePlane(planeDistance, segmentRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentRadius))
		{
			inOrInt++;
			interPlane = up;
		}
		planeDistance = PlaneDistance(boxCenter - up, -upN, segmentOffsetted);
		if (SphereInsidePlane(planeDistance, segmentRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentRadius))
		{
			inOrInt++;
			interPlane = -up;
		}
		planeDistance = PlaneDistance(boxCenter - right, -rightN, segmentOffsetted);
		if (SphereInsidePlane(planeDistance, segmentRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentRadius))
		{
			inOrInt++;
			interPlane = -right;
		}
		planeDistance = PlaneDistance(boxCenter + right, rightN, segmentOffsetted);
		if (SphereInsidePlane(planeDistance, segmentRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentRadius))
		{
			inOrInt++;
			interPlane = right;
		}
		bool insideOrIntersects = false;
		if (base.Collider2D == null)
		{
			planeDistance = PlaneDistance(boxCenter + forward, forwardN, segmentOffsetted);
			if (SphereInsidePlane(planeDistance, segmentRadius))
			{
				inOrInt++;
			}
			else if (SphereIntersectsPlane(planeDistance, segmentRadius))
			{
				inOrInt++;
				interPlane = forward;
			}
			planeDistance = PlaneDistance(boxCenter - forward, -forwardN, segmentOffsetted);
			if (SphereInsidePlane(planeDistance, segmentRadius))
			{
				inOrInt++;
			}
			else if (SphereIntersectsPlane(planeDistance, segmentRadius))
			{
				inOrInt++;
				interPlane = -forward;
			}
			if (inOrInt == 6)
			{
				insideOrIntersects = true;
			}
		}
		else if (inOrInt == 4)
		{
			insideOrIntersects = true;
		}
		if (insideOrIntersects)
		{
			bool inside = false;
			if (interPlane.sqrMagnitude == 0f)
			{
				inside = true;
			}
			else if (base.Collider2D == null)
			{
				if (IsInsideBoxCollider(Box, segmentOffsetted))
				{
					inside = true;
				}
			}
			else if (IsInsideBoxCollider(Box2D, segmentOffsetted))
			{
				inside = true;
			}
			Vector3 toNormal = GetNearestPoint(segmentOffsetted) - segmentOffsetted;
			if (inside)
			{
				toNormal += toNormal.normalized * segmentRadius;
			}
			else
			{
				toNormal -= toNormal.normalized * segmentRadius;
			}
			if (inside)
			{
				segmentPosition += toNormal;
			}
			else if (toNormal.sqrMagnitude > 0f)
			{
				segmentPosition += toNormal;
			}
			return true;
		}
		return false;
	}

	public static void PushOutFromBoxCollider(BoxCollider box, Collision collision, float segmentColliderRadius, ref Vector3 segmentPosition, bool is2D = false)
	{
		Vector3 right = box.transform.TransformVector(Vector3.right / 2f * box.size.x + box.center.x * Vector3.right);
		Vector3 up = box.transform.TransformVector(Vector3.up / 2f * box.size.y + box.center.y * Vector3.up);
		Vector3 forward = box.transform.TransformVector(Vector3.forward / 2f * box.size.z + box.center.z * Vector3.forward);
		Vector3 scales = Vector3.Scale(box.size, box.transform.lossyScale);
		scales.Normalize();
		PushOutFromBoxCollider(box, collision, segmentColliderRadius, ref segmentPosition, right, up, forward, scales, is2D);
	}

	public static void PushOutFromBoxCollider(BoxCollider box, float segmentColliderRadius, ref Vector3 segmentPosition, bool is2D = false)
	{
		Vector3 right = box.transform.TransformVector(Vector3.right / 2f * box.size.x + box.center.x * Vector3.right);
		Vector3 up = box.transform.TransformVector(Vector3.up / 2f * box.size.y + box.center.y * Vector3.up);
		Vector3 forward = box.transform.TransformVector(Vector3.forward / 2f * box.size.z + box.center.z * Vector3.forward);
		Vector3.Scale(box.size, box.transform.lossyScale).Normalize();
		Vector3 boxCenter = GetBoxCenter(box);
		Vector3 upN = up.normalized;
		Vector3 rightN = right.normalized;
		Vector3 forwardN = forward.normalized;
		int inOrInt = 0;
		Vector3 interPlane = Vector3.zero;
		float planeDistance = PlaneDistance(boxCenter + up, upN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = up;
		}
		planeDistance = PlaneDistance(boxCenter - up, -upN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = -up;
		}
		planeDistance = PlaneDistance(boxCenter - right, -rightN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = -right;
		}
		planeDistance = PlaneDistance(boxCenter + right, rightN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = right;
		}
		planeDistance = PlaneDistance(boxCenter + forward, forwardN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = forward;
		}
		planeDistance = PlaneDistance(boxCenter - forward, -forwardN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = -forward;
		}
		if (inOrInt == 6)
		{
			bool inside = false;
			if (interPlane.sqrMagnitude == 0f)
			{
				inside = true;
			}
			else if (IsInsideBoxCollider(box, segmentPosition))
			{
				inside = true;
			}
			Vector3 toNormal = GetNearestPoint(segmentPosition, boxCenter, right, up, forward, is2D) - segmentPosition;
			if (inside)
			{
				toNormal += toNormal.normalized * segmentColliderRadius * 1.01f;
			}
			else
			{
				toNormal -= toNormal.normalized * segmentColliderRadius * 1.01f;
			}
			if (inside)
			{
				segmentPosition += toNormal;
			}
			else if (toNormal.sqrMagnitude > 0f)
			{
				segmentPosition += toNormal;
			}
		}
	}

	public static void PushOutFromBoxCollider(BoxCollider box, Collision collision, float segmentColliderRadius, ref Vector3 pos, Vector3 right, Vector3 up, Vector3 forward, Vector3 scales, bool is2D = false)
	{
		Vector3 collisionPoint = collision.contacts[0].point;
		Vector3 pushNormal = pos - collisionPoint;
		Vector3 boxCenter = GetBoxCenter(box);
		if (pushNormal.sqrMagnitude == 0f)
		{
			pushNormal = pos - boxCenter;
		}
		float insideMul = 1f;
		if (IsInsideBoxCollider(box, pos))
		{
			float castFactor = GetBoxAverageScale(box);
			Vector3 fittingNormal = GetTargetPlaneNormal(box, pos, right, up, forward, scales);
			Vector3 fittingNormalNorm = fittingNormal.normalized;
			collisionPoint = ((!box.Raycast(new Ray(pos - fittingNormalNorm * castFactor * 3f, fittingNormalNorm), out var info, castFactor * 4f)) ? GetIntersectOnBoxFromInside(box, boxCenter, pos, fittingNormal) : info.point);
			pushNormal = collisionPoint - pos;
			insideMul = 100f;
		}
		Vector3 toNormal = pos - (pushNormal / insideMul + pushNormal.normalized * 1.15f) / 2f * segmentColliderRadius;
		toNormal = collisionPoint - toNormal;
		float pushMagn = toNormal.sqrMagnitude;
		if (pushMagn > 0f && pushMagn < segmentColliderRadius * segmentColliderRadius * insideMul)
		{
			pos += toNormal;
		}
	}

	public static void PushOutFromBoxCollider(BoxCollider2D box2D, float segmentColliderRadius, ref Vector3 segmentPosition)
	{
		Vector2 right = box2D.transform.TransformVector(Vector3.right / 2f * box2D.size.x + box2D.offset.x * Vector3.right);
		Vector2 up = box2D.transform.TransformVector(Vector3.up / 2f * box2D.size.y + box2D.offset.y * Vector3.up);
		Vector3 scale2D = box2D.transform.lossyScale;
		scale2D.z = 1f;
		((Vector2)Vector3.Scale(box2D.size, scale2D)).Normalize();
		Vector2 boxCenter = GetBoxCenter(box2D);
		Vector2 upN = up.normalized;
		Vector2 rightN = right.normalized;
		int inOrInt = 0;
		Vector3 interPlane = Vector3.zero;
		float planeDistance = PlaneDistance(boxCenter + up, upN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = up;
		}
		planeDistance = PlaneDistance(boxCenter - up, -upN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = -up;
		}
		planeDistance = PlaneDistance(boxCenter - right, -rightN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = -right;
		}
		planeDistance = PlaneDistance(boxCenter + right, rightN, segmentPosition);
		if (SphereInsidePlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
		}
		else if (SphereIntersectsPlane(planeDistance, segmentColliderRadius))
		{
			inOrInt++;
			interPlane = right;
		}
		if (inOrInt == 4)
		{
			bool inside = false;
			if (interPlane.sqrMagnitude == 0f)
			{
				inside = true;
			}
			else if (IsInsideBoxCollider(box2D, segmentPosition))
			{
				inside = true;
			}
			Vector3 toNormal = GetNearestPoint2D(segmentPosition, boxCenter, right, up) - segmentPosition;
			if (inside)
			{
				toNormal += toNormal.normalized * segmentColliderRadius * 1.01f;
			}
			else
			{
				toNormal -= toNormal.normalized * segmentColliderRadius * 1.01f;
			}
			if (inside)
			{
				segmentPosition += toNormal;
			}
			else if (toNormal.sqrMagnitude > 0f)
			{
				segmentPosition += toNormal;
			}
		}
	}

	private Vector3 GetNearestPoint(Vector3 point)
	{
		Vector3 pointOnBox = point;
		Vector3 distancesPositive = Vector3.one;
		distancesPositive.x = PlaneDistance(boxCenter + right, rightN, point);
		distancesPositive.y = PlaneDistance(boxCenter + up, upN, point);
		if (base.Collider2D == null)
		{
			distancesPositive.z = PlaneDistance(boxCenter + forward, forwardN, point);
		}
		Vector3 distancesNegative = Vector3.one;
		distancesNegative.x = PlaneDistance(boxCenter - right, -rightN, point);
		distancesNegative.y = PlaneDistance(boxCenter - up, -upN, point);
		if (base.Collider2D == null)
		{
			distancesNegative.z = PlaneDistance(boxCenter - forward, -forwardN, point);
		}
		float negX = 1f;
		float negY = 1f;
		float negZ = 1f;
		float nearestX;
		if (distancesPositive.x > distancesNegative.x)
		{
			nearestX = distancesPositive.x;
			negX = -1f;
		}
		else
		{
			nearestX = distancesNegative.x;
			negX = 1f;
		}
		float nearestY;
		if (distancesPositive.y > distancesNegative.y)
		{
			nearestY = distancesPositive.y;
			negY = -1f;
		}
		else
		{
			nearestY = distancesNegative.y;
			negY = 1f;
		}
		if (base.Collider2D == null)
		{
			float nearestZ;
			if (distancesPositive.z > distancesNegative.z)
			{
				nearestZ = distancesPositive.z;
				negZ = -1f;
			}
			else
			{
				nearestZ = distancesNegative.z;
				negZ = 1f;
			}
			if (nearestX > nearestZ)
			{
				if (nearestX > nearestY)
				{
					return ProjectPointOnPlane(right * negX, point, nearestX);
				}
				return ProjectPointOnPlane(up * negY, point, nearestY);
			}
			if (nearestZ > nearestY)
			{
				return ProjectPointOnPlane(forward * negZ, point, nearestZ);
			}
			return ProjectPointOnPlane(up * negY, point, nearestY);
		}
		if (nearestX > nearestY)
		{
			return ProjectPointOnPlane(right * negX, point, nearestX);
		}
		return ProjectPointOnPlane(up * negY, point, nearestY);
	}

	private static Vector3 GetNearestPoint(Vector3 point, Vector3 boxCenter, Vector3 right, Vector3 up, Vector3 forward, bool is2D = false)
	{
		Vector3 pointOnBox = point;
		Vector3 distancesPositive = Vector3.one;
		distancesPositive.x = PlaneDistance(boxCenter + right, right.normalized, point);
		distancesPositive.y = PlaneDistance(boxCenter + up, up.normalized, point);
		if (!is2D)
		{
			distancesPositive.z = PlaneDistance(boxCenter + forward, forward.normalized, point);
		}
		Vector3 distancesNegative = Vector3.one;
		distancesNegative.x = PlaneDistance(boxCenter - right, -right.normalized, point);
		distancesNegative.y = PlaneDistance(boxCenter - up, -up.normalized, point);
		if (!is2D)
		{
			distancesNegative.z = PlaneDistance(boxCenter - forward, -forward.normalized, point);
		}
		float negX = 1f;
		float negY = 1f;
		float negZ = 1f;
		float nearestX;
		if (distancesPositive.x > distancesNegative.x)
		{
			nearestX = distancesPositive.x;
			negX = -1f;
		}
		else
		{
			nearestX = distancesNegative.x;
			negX = 1f;
		}
		float nearestY;
		if (distancesPositive.y > distancesNegative.y)
		{
			nearestY = distancesPositive.y;
			negY = -1f;
		}
		else
		{
			nearestY = distancesNegative.y;
			negY = 1f;
		}
		if (!is2D)
		{
			float nearestZ;
			if (distancesPositive.z > distancesNegative.z)
			{
				nearestZ = distancesPositive.z;
				negZ = -1f;
			}
			else
			{
				nearestZ = distancesNegative.z;
				negZ = 1f;
			}
			if (nearestX > nearestZ)
			{
				if (nearestX > nearestY)
				{
					return ProjectPointOnPlane(right * negX, point, nearestX);
				}
				return ProjectPointOnPlane(up * negY, point, nearestY);
			}
			if (nearestZ > nearestY)
			{
				return ProjectPointOnPlane(forward * negZ, point, nearestZ);
			}
			return ProjectPointOnPlane(up * negY, point, nearestY);
		}
		if (nearestX > nearestY)
		{
			return ProjectPointOnPlane(right * negX, point, nearestX);
		}
		return ProjectPointOnPlane(up * negY, point, nearestY);
	}

	private static Vector3 GetNearestPoint2D(Vector2 point, Vector2 boxCenter, Vector2 right, Vector2 up)
	{
		Vector3 pointOnBox = point;
		Vector3 distancesPositive = Vector3.one;
		distancesPositive.x = PlaneDistance(boxCenter + right, right.normalized, point);
		distancesPositive.y = PlaneDistance(boxCenter + up, up.normalized, point);
		Vector3 distancesNegative = Vector3.one;
		distancesNegative.x = PlaneDistance(boxCenter - right, -right.normalized, point);
		distancesNegative.y = PlaneDistance(boxCenter - up, -up.normalized, point);
		float negX = 1f;
		float negY = 1f;
		float nearestX;
		if (distancesPositive.x > distancesNegative.x)
		{
			nearestX = distancesPositive.x;
			negX = -1f;
		}
		else
		{
			nearestX = distancesNegative.x;
			negX = 1f;
		}
		float nearestY;
		if (distancesPositive.y > distancesNegative.y)
		{
			nearestY = distancesPositive.y;
			negY = -1f;
		}
		else
		{
			nearestY = distancesNegative.y;
			negY = 1f;
		}
		if (nearestX > nearestY)
		{
			return ProjectPointOnPlane(right * negX, point, nearestX);
		}
		return ProjectPointOnPlane(up * negY, point, nearestY);
	}

	public static Vector3 GetNearestPointOnBox(BoxCollider boxCollider, Vector3 point, bool is2D = false)
	{
		Vector3 right = boxCollider.transform.TransformVector(Vector3.right / 2f);
		Vector3 up = boxCollider.transform.TransformVector(Vector3.up / 2f);
		Vector3 forward = Vector3.forward;
		if (!is2D)
		{
			forward = boxCollider.transform.TransformVector(Vector3.forward / 2f);
		}
		Vector3 pointOnBox = point;
		Vector3 center = GetBoxCenter(boxCollider);
		Vector3 rightN = right.normalized;
		Vector3 upN = up.normalized;
		Vector3 forwardN = forward.normalized;
		Vector3 distancesPositive = Vector3.one;
		distancesPositive.x = PlaneDistance(center + right, rightN, point);
		distancesPositive.y = PlaneDistance(center + up, upN, point);
		if (!is2D)
		{
			distancesPositive.z = PlaneDistance(center + forward, forwardN, point);
		}
		Vector3 distancesNegative = Vector3.one;
		distancesNegative.x = PlaneDistance(center - right, -rightN, point);
		distancesNegative.y = PlaneDistance(center - up, -upN, point);
		if (!is2D)
		{
			distancesNegative.z = PlaneDistance(center - forward, -forwardN, point);
		}
		float negX = 1f;
		float negY = 1f;
		float negZ = 1f;
		float nearestX;
		if (distancesPositive.x > distancesNegative.x)
		{
			nearestX = distancesPositive.x;
			negX = -1f;
		}
		else
		{
			nearestX = distancesNegative.x;
			negX = 1f;
		}
		float nearestY;
		if (distancesPositive.y > distancesNegative.y)
		{
			nearestY = distancesPositive.y;
			negY = -1f;
		}
		else
		{
			nearestY = distancesNegative.y;
			negY = 1f;
		}
		if (!is2D)
		{
			float nearestZ;
			if (distancesPositive.z > distancesNegative.z)
			{
				nearestZ = distancesPositive.z;
				negZ = -1f;
			}
			else
			{
				nearestZ = distancesNegative.z;
				negZ = 1f;
			}
			if (nearestX > nearestZ)
			{
				if (nearestX > nearestY)
				{
					return ProjectPointOnPlane(right * negX, point, nearestX);
				}
				return ProjectPointOnPlane(up * negY, point, nearestY);
			}
			if (nearestZ > nearestY)
			{
				return ProjectPointOnPlane(forward * negZ, point, nearestZ);
			}
			return ProjectPointOnPlane(up * negY, point, nearestY);
		}
		if (nearestX > nearestY)
		{
			return ProjectPointOnPlane(right * negX, point, nearestX);
		}
		return ProjectPointOnPlane(up * negY, point, nearestY);
	}

	private static float PlaneDistance(Vector3 planeCenter, Vector3 planeNormal, Vector3 point)
	{
		return Vector3.Dot(point - planeCenter, planeNormal);
	}

	private static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 point, float distance)
	{
		Vector3 translationVector = planeNormal.normalized * distance;
		return point + translationVector;
	}

	private static bool SphereInsidePlane(float planeDistance, float pointRadius)
	{
		return 0f - planeDistance > pointRadius;
	}

	private static bool SphereOutsidePlane(float planeDistance, float pointRadius)
	{
		return planeDistance > pointRadius;
	}

	private static bool SphereIntersectsPlane(float planeDistance, float pointRadius)
	{
		return Mathf.Abs(planeDistance) <= pointRadius;
	}

	public static bool IsInsideBoxCollider(BoxCollider collider, Vector3 point, bool is2D = false)
	{
		point = collider.transform.InverseTransformPoint(point) - collider.center;
		float xExtend = collider.size.x * 0.5f;
		float yExtend = collider.size.y * 0.5f;
		float zExtend = collider.size.z * 0.5f;
		if (point.x < xExtend && point.x > 0f - xExtend && point.y < yExtend && point.y > 0f - yExtend && point.z < zExtend)
		{
			return point.z > 0f - zExtend;
		}
		return false;
	}

	public static bool IsInsideBoxCollider(BoxCollider2D collider, Vector3 point)
	{
		point = (Vector2)collider.transform.InverseTransformPoint(point) - collider.offset;
		float xExtend = collider.size.x * 0.5f;
		float yExtend = collider.size.y * 0.5f;
		if (point.x < xExtend && point.x > 0f - xExtend && point.y < yExtend)
		{
			return point.y > 0f - yExtend;
		}
		return false;
	}

	protected static float GetBoxAverageScale(BoxCollider box)
	{
		Vector3 scales = box.transform.lossyScale;
		scales = Vector3.Scale(scales, box.size);
		return (scales.x + scales.y + scales.z) / 3f;
	}

	protected static Vector3 GetBoxCenter(BoxCollider box)
	{
		return box.transform.position + box.transform.TransformVector(box.center);
	}

	protected static Vector3 GetBoxCenter(BoxCollider2D box)
	{
		return box.transform.position + box.transform.TransformVector(box.offset);
	}

	protected static Vector3 GetTargetPlaneNormal(BoxCollider boxCollider, Vector3 point, bool is2D = false)
	{
		Vector3 right = boxCollider.transform.TransformVector(Vector3.right / 2f * boxCollider.size.x);
		Vector3 up = boxCollider.transform.TransformVector(Vector3.up / 2f * boxCollider.size.y);
		Vector3 forward = Vector3.forward;
		if (!is2D)
		{
			forward = boxCollider.transform.TransformVector(Vector3.forward / 2f * boxCollider.size.z);
		}
		Vector3 scales = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale);
		scales.Normalize();
		return GetTargetPlaneNormal(boxCollider, point, right, up, forward, scales, is2D);
	}

	protected static Vector3 GetTargetPlaneNormal(BoxCollider boxCollider, Vector3 point, Vector3 right, Vector3 up, Vector3 forward, Vector3 scales, bool is2D = false)
	{
		Vector3 rayDirection = (GetBoxCenter(boxCollider) - point).normalized;
		Vector3 dots = default(Vector3);
		dots.x = Vector3.Dot(rayDirection, right.normalized);
		dots.y = Vector3.Dot(rayDirection, up.normalized);
		dots.x = dots.x * scales.y * scales.z;
		dots.y = dots.y * scales.x * scales.z;
		if (!is2D)
		{
			dots.z = Vector3.Dot(rayDirection, forward.normalized);
			dots.z = dots.z * scales.y * scales.x;
		}
		else
		{
			dots.z = 0f;
		}
		dots.Normalize();
		Vector3 dotsAbs = dots;
		if (dots.x < 0f)
		{
			dotsAbs.x = 0f - dots.x;
		}
		if (dots.y < 0f)
		{
			dotsAbs.y = 0f - dots.y;
		}
		if (dots.z < 0f)
		{
			dotsAbs.z = 0f - dots.z;
		}
		if (dotsAbs.x > dotsAbs.y)
		{
			if (dotsAbs.x > dotsAbs.z || is2D)
			{
				return right * Mathf.Sign(dots.x);
			}
			return forward * Mathf.Sign(dots.z);
		}
		if (dotsAbs.y > dotsAbs.z || is2D)
		{
			return up * Mathf.Sign(dots.y);
		}
		return forward * Mathf.Sign(dots.z);
	}

	protected static Vector3 GetTargetPlaneNormal(BoxCollider2D boxCollider, Vector2 point, Vector2 right, Vector2 up, Vector2 scales)
	{
		Vector2 rayDirection = ((Vector2)GetBoxCenter(boxCollider) - point).normalized;
		Vector2 dots = default(Vector2);
		dots.x = Vector3.Dot(rayDirection, right.normalized);
		dots.y = Vector3.Dot(rayDirection, up.normalized);
		dots.x *= scales.y;
		dots.y *= scales.x;
		dots.Normalize();
		Vector2 dotsAbs = dots;
		if (dots.x < 0f)
		{
			dotsAbs.x = 0f - dots.x;
		}
		if (dots.y < 0f)
		{
			dotsAbs.y = 0f - dots.y;
		}
		if (dotsAbs.x > dotsAbs.y)
		{
			return right * Mathf.Sign(dots.x);
		}
		return up * Mathf.Sign(dots.y);
	}

	protected static Vector3 GetIntersectOnBoxFromInside(BoxCollider boxCollider, Vector3 from, Vector3 to, Vector3 planeNormal)
	{
		Vector3 rayDirection = to - from;
		Plane plane = new Plane(-planeNormal, GetBoxCenter(boxCollider) + planeNormal);
		Vector3 intersectionPoint = to;
		float enter = 0f;
		Ray ray = new Ray(from, rayDirection);
		if (plane.Raycast(ray, out enter))
		{
			intersectionPoint = ray.GetPoint(enter);
		}
		return intersectionPoint;
	}
}
