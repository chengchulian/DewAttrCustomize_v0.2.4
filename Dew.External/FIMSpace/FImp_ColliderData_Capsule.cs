using UnityEngine;

namespace FIMSpace;

public class FImp_ColliderData_Capsule : FImp_ColliderData_Base
{
	private Vector3 Top;

	private Vector3 Bottom;

	private Vector3 Direction;

	private float radius;

	private float scaleFactor;

	private float preRadius;

	public CapsuleCollider Capsule { get; private set; }

	public CapsuleCollider2D Capsule2D { get; private set; }

	public FImp_ColliderData_Capsule(CapsuleCollider collider)
	{
		Is2D = false;
		base.Transform = collider.transform;
		base.Collider = collider;
		base.Transform = collider.transform;
		Capsule = collider;
		base.ColliderType = EFColliderType.Capsule;
		CalculateCapsuleParameters(Capsule, ref Direction, ref radius, ref scaleFactor);
		RefreshColliderData();
	}

	public FImp_ColliderData_Capsule(CapsuleCollider2D collider)
	{
		Is2D = true;
		base.Transform = collider.transform;
		base.Collider2D = collider;
		base.Transform = collider.transform;
		Capsule2D = collider;
		base.ColliderType = EFColliderType.Capsule;
		CalculateCapsuleParameters(Capsule2D, ref Direction, ref radius, ref scaleFactor);
		RefreshColliderData();
	}

	public override void RefreshColliderData()
	{
		if (base.IsStatic)
		{
			return;
		}
		bool diff = false;
		if (!previousPosition.VIsSame(base.Transform.position))
		{
			diff = true;
		}
		else if (!base.Transform.rotation.QIsSame(previousRotation))
		{
			diff = true;
		}
		else if (!Is2D)
		{
			if (preRadius != Capsule.radius || !previousScale.VIsSame(base.Transform.lossyScale))
			{
				CalculateCapsuleParameters(Capsule, ref Direction, ref radius, ref scaleFactor);
			}
		}
		else if (preRadius != GetCapsule2DRadius(Capsule2D) || !previousScale.VIsSame(base.Transform.lossyScale))
		{
			CalculateCapsuleParameters(Capsule2D, ref Direction, ref radius, ref scaleFactor);
		}
		if (diff)
		{
			if (!Is2D)
			{
				GetCapsuleHeadsPositions(Capsule, ref Top, ref Bottom, Direction, radius, scaleFactor);
			}
			else
			{
				GetCapsuleHeadsPositions(Capsule2D, ref Top, ref Bottom, Direction, radius, scaleFactor);
			}
		}
		base.RefreshColliderData();
		previousPosition = base.Transform.position;
		previousRotation = base.Transform.rotation;
		previousScale = base.Transform.lossyScale;
		if (!Is2D)
		{
			preRadius = Capsule.radius;
		}
		else
		{
			preRadius = GetCapsule2DRadius(Capsule2D);
		}
	}

	public override bool PushIfInside(ref Vector3 point, float pointRadius, Vector3 pointOffset)
	{
		return PushOutFromCapsuleCollider(pointRadius, ref point, Top, Bottom, radius, pointOffset, Is2D);
	}

	public static bool PushOutFromCapsuleCollider(CapsuleCollider capsule, float segmentColliderRadius, ref Vector3 pos, Vector3 segmentOffset)
	{
		Vector3 direction = Vector3.zero;
		float capsuleRadius = capsule.radius;
		float scalerFactor = 1f;
		CalculateCapsuleParameters(capsule, ref direction, ref capsuleRadius, ref scalerFactor);
		Vector3 up = Vector3.zero;
		Vector3 bottom = Vector3.zero;
		GetCapsuleHeadsPositions(capsule, ref up, ref bottom, direction, capsuleRadius, scalerFactor);
		return PushOutFromCapsuleCollider(segmentColliderRadius, ref pos, up, bottom, capsuleRadius, segmentOffset);
	}

	public static bool PushOutFromCapsuleCollider(float segmentColliderRadius, ref Vector3 segmentPos, Vector3 capSphereCenter1, Vector3 capSphereCenter2, float capsuleRadius, Vector3 segmentOffset, bool is2D = false)
	{
		float radius = capsuleRadius + segmentColliderRadius;
		Vector3 capsuleUp = capSphereCenter2 - capSphereCenter1;
		Vector3 fromCenter = segmentPos + segmentOffset - capSphereCenter1;
		if (is2D)
		{
			capsuleUp.z = 0f;
			fromCenter.z = 0f;
		}
		float orientationDot = Vector3.Dot(fromCenter, capsuleUp);
		if (orientationDot <= 0f)
		{
			float sphereRefDistMagn = fromCenter.sqrMagnitude;
			if (sphereRefDistMagn > 0f && sphereRefDistMagn < radius * radius)
			{
				segmentPos = capSphereCenter1 - segmentOffset + fromCenter * (radius / Mathf.Sqrt(sphereRefDistMagn));
				return true;
			}
		}
		else
		{
			float upRefMagn = capsuleUp.sqrMagnitude;
			if (orientationDot >= upRefMagn)
			{
				fromCenter = segmentPos + segmentOffset - capSphereCenter2;
				float sphereRefDistMagn2 = fromCenter.sqrMagnitude;
				if (sphereRefDistMagn2 > 0f && sphereRefDistMagn2 < radius * radius)
				{
					segmentPos = capSphereCenter2 - segmentOffset + fromCenter * (radius / Mathf.Sqrt(sphereRefDistMagn2));
					return true;
				}
			}
			else if (upRefMagn > 0f)
			{
				fromCenter -= capsuleUp * (orientationDot / upRefMagn);
				float sphericalRefDistMagn = fromCenter.sqrMagnitude;
				if (sphericalRefDistMagn > 0f && sphericalRefDistMagn < radius * radius)
				{
					float projectedDistance = Mathf.Sqrt(sphericalRefDistMagn);
					segmentPos += fromCenter * ((radius - projectedDistance) / projectedDistance);
					return true;
				}
			}
		}
		return false;
	}

	protected static void CalculateCapsuleParameters(CapsuleCollider capsule, ref Vector3 direction, ref float trueRadius, ref float scalerFactor)
	{
		Transform cTransform = capsule.transform;
		float radiusScaler;
		if (capsule.direction == 1)
		{
			direction = Vector3.up;
			scalerFactor = cTransform.lossyScale.y;
			radiusScaler = ((cTransform.lossyScale.x > cTransform.lossyScale.z) ? cTransform.lossyScale.x : cTransform.lossyScale.z);
		}
		else if (capsule.direction == 0)
		{
			direction = Vector3.right;
			scalerFactor = cTransform.lossyScale.x;
			radiusScaler = ((cTransform.lossyScale.y > cTransform.lossyScale.z) ? cTransform.lossyScale.y : cTransform.lossyScale.z);
		}
		else
		{
			direction = Vector3.forward;
			scalerFactor = cTransform.lossyScale.z;
			radiusScaler = ((cTransform.lossyScale.y > cTransform.lossyScale.x) ? cTransform.lossyScale.y : cTransform.lossyScale.x);
		}
		trueRadius = capsule.radius * radiusScaler;
	}

	private static float GetCapsule2DRadius(CapsuleCollider2D capsule)
	{
		if (capsule.direction == CapsuleDirection2D.Vertical)
		{
			return capsule.size.x / 2f;
		}
		return capsule.size.y / 2f;
	}

	private static float GetCapsule2DHeight(CapsuleCollider2D capsule)
	{
		if (capsule.direction == CapsuleDirection2D.Vertical)
		{
			return capsule.size.y / 2f;
		}
		return capsule.size.x / 2f;
	}

	protected static void CalculateCapsuleParameters(CapsuleCollider2D capsule, ref Vector3 direction, ref float trueRadius, ref float scalerFactor)
	{
		Transform cTransform = capsule.transform;
		if (capsule.direction == CapsuleDirection2D.Vertical)
		{
			direction = Vector3.up;
			scalerFactor = cTransform.lossyScale.y;
			float radiusScaler = ((cTransform.lossyScale.x > cTransform.lossyScale.z) ? cTransform.lossyScale.x : cTransform.lossyScale.z);
			trueRadius = capsule.size.x / 2f * radiusScaler;
		}
		else if (capsule.direction == CapsuleDirection2D.Horizontal)
		{
			direction = Vector3.right;
			scalerFactor = cTransform.lossyScale.x;
			float radiusScaler = ((cTransform.lossyScale.y > cTransform.lossyScale.z) ? cTransform.lossyScale.y : cTransform.lossyScale.z);
			trueRadius = capsule.size.y / 2f * radiusScaler;
		}
	}

	protected static void GetCapsuleHeadsPositions(CapsuleCollider capsule, ref Vector3 upper, ref Vector3 bottom, Vector3 direction, float radius, float scalerFactor)
	{
		Vector3 upCapCenter = direction * (capsule.height / 2f * scalerFactor - radius);
		upper = capsule.transform.position + capsule.transform.TransformDirection(upCapCenter) + capsule.transform.TransformVector(capsule.center);
		Vector3 downCapCenter = -direction * (capsule.height / 2f * scalerFactor - radius);
		bottom = capsule.transform.position + capsule.transform.TransformDirection(downCapCenter) + capsule.transform.TransformVector(capsule.center);
	}

	protected static void GetCapsuleHeadsPositions(CapsuleCollider2D capsule, ref Vector3 upper, ref Vector3 bottom, Vector3 direction, float radius, float scalerFactor)
	{
		Vector3 upCapCenter = direction * (GetCapsule2DHeight(capsule) * scalerFactor - radius);
		upper = capsule.transform.position + capsule.transform.TransformDirection(upCapCenter) + capsule.transform.TransformVector(capsule.offset);
		upper.z = 0f;
		Vector3 downCapCenter = -direction * (GetCapsule2DHeight(capsule) * scalerFactor - radius);
		bottom = capsule.transform.position + capsule.transform.TransformDirection(downCapCenter) + capsule.transform.TransformVector(capsule.offset);
		bottom.z = 0f;
	}
}
