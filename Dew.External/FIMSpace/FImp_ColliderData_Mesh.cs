using UnityEngine;

namespace FIMSpace;

public class FImp_ColliderData_Mesh : FImp_ColliderData_Base
{
	private ContactFilter2D filter;

	private RaycastHit2D[] r;

	public MeshCollider Mesh { get; private set; }

	public PolygonCollider2D Poly2D { get; private set; }

	public FImp_ColliderData_Mesh(MeshCollider collider)
	{
		Is2D = false;
		base.Transform = collider.transform;
		base.Collider = collider;
		Mesh = collider;
		base.ColliderType = EFColliderType.Mesh;
	}

	public FImp_ColliderData_Mesh(PolygonCollider2D collider)
	{
		Is2D = true;
		base.Transform = collider.transform;
		Poly2D = collider;
		base.Collider2D = collider;
		base.ColliderType = EFColliderType.Mesh;
		filter = default(ContactFilter2D);
		filter.useTriggers = false;
		filter.useDepth = false;
		r = new RaycastHit2D[1];
	}

	public override bool PushIfInside(ref Vector3 segmentPosition, float segmentRadius, Vector3 segmentOffset)
	{
		if (!Is2D)
		{
			if (!Mesh.convex)
			{
				float plus = 0f;
				Vector3 positionOffsetted = segmentPosition + segmentOffset;
				Vector3 closest = Mesh.ClosestPointOnBounds(positionOffsetted);
				plus = (closest - Mesh.transform.position).magnitude;
				bool inside = false;
				float insideMul = 1f;
				if (closest == positionOffsetted)
				{
					inside = true;
					insideMul = 7f;
					closest = Mesh.transform.position;
				}
				Vector3 targeting = closest - positionOffsetted;
				Vector3 rayDirection = targeting.normalized;
				Vector3 rayOrigin = positionOffsetted - rayDirection * (segmentRadius * 2f + Mesh.bounds.extents.magnitude);
				float rayDistance = targeting.magnitude + segmentRadius * 2f + plus + Mesh.bounds.extents.magnitude;
				if ((positionOffsetted - closest).magnitude < segmentRadius * insideMul)
				{
					Ray ray = new Ray(rayOrigin, rayDirection);
					if (Mesh.Raycast(ray, out var hit, rayDistance) && (positionOffsetted - hit.point).magnitude < segmentRadius * insideMul)
					{
						Vector3 toNormal = hit.point - positionOffsetted;
						Vector3 pushNormal = ((!inside) ? (toNormal - toNormal.normalized * segmentRadius) : (toNormal + toNormal.normalized * segmentRadius));
						float dot = Vector3.Dot((hit.point - positionOffsetted).normalized, rayDirection);
						if (inside && dot > 0f)
						{
							pushNormal = toNormal - toNormal.normalized * segmentRadius;
						}
						segmentPosition += pushNormal;
						return true;
					}
				}
				return false;
			}
			Vector3 positionOffsetted2 = segmentPosition + segmentOffset;
			float castMul = 1f;
			Vector3 closest2 = Physics.ClosestPoint(positionOffsetted2, Mesh, Mesh.transform.position, Mesh.transform.rotation);
			if (Vector3.Distance(closest2, positionOffsetted2) > segmentRadius * 1.01f)
			{
				return false;
			}
			Vector3 dir = closest2 - positionOffsetted2;
			if (dir == Vector3.zero)
			{
				return false;
			}
			Mesh.Raycast(new Ray(positionOffsetted2, dir.normalized), out var meshHit, segmentRadius * castMul);
			if ((bool)meshHit.transform)
			{
				segmentPosition = meshHit.point + meshHit.normal * segmentRadius;
				return true;
			}
		}
		else
		{
			Vector2 positionOffsetted3 = segmentPosition + segmentOffset;
			Vector2 closest3;
			if (Poly2D.OverlapPoint(positionOffsetted3))
			{
				Vector3 indir = Poly2D.bounds.center - (Vector3)positionOffsetted3;
				indir.z = 0f;
				Ray r = new Ray(Poly2D.bounds.center - indir * Poly2D.bounds.max.magnitude, indir);
				float dist = 0f;
				Poly2D.bounds.IntersectRay(r, out dist);
				closest3 = ((!(dist > 0f)) ? Poly2D.ClosestPoint(positionOffsetted3) : Poly2D.ClosestPoint(r.GetPoint(dist)));
			}
			else
			{
				closest3 = Poly2D.ClosestPoint(positionOffsetted3);
			}
			Vector2 dir2 = (closest3 - positionOffsetted3).normalized;
			if (Physics2D.Raycast(positionOffsetted3, dir2, filter, this.r, segmentRadius) > 0 && this.r[0].transform == base.Transform)
			{
				segmentPosition = closest3 + this.r[0].normal * segmentRadius;
				return true;
			}
		}
		return false;
	}

	public static void PushOutFromMeshCollider(MeshCollider mesh, Collision collision, float segmentColliderRadius, ref Vector3 pos)
	{
		Vector3 collisionPoint = collision.contacts[0].point;
		Vector3 pushNormal = collision.contacts[0].normal;
		if (mesh.Raycast(new Ray(pos + pushNormal * segmentColliderRadius * 2f, -pushNormal), out var info, segmentColliderRadius * 5f))
		{
			pushNormal = info.point - pos;
			float pushMagn = pushNormal.sqrMagnitude;
			if (pushMagn > 0f && pushMagn < segmentColliderRadius * segmentColliderRadius)
			{
				pos = info.point - pushNormal * (segmentColliderRadius / Mathf.Sqrt(pushMagn)) * 0.9f;
			}
		}
		else
		{
			pushNormal = collisionPoint - pos;
			float pushMagn2 = pushNormal.sqrMagnitude;
			if (pushMagn2 > 0f && pushMagn2 < segmentColliderRadius * segmentColliderRadius)
			{
				pos = collisionPoint - pushNormal * (segmentColliderRadius / Mathf.Sqrt(pushMagn2)) * 0.9f;
			}
		}
	}

	public static void PushOutFromMesh(MeshCollider mesh, Collision collision, float pointRadius, ref Vector3 point)
	{
		float plus = 0f;
		Vector3 closest = mesh.ClosestPointOnBounds(point);
		plus = (closest - mesh.transform.position).magnitude;
		bool inside = false;
		float insideMul = 1f;
		if (closest == point)
		{
			inside = true;
			insideMul = 7f;
			closest = mesh.transform.position;
		}
		Vector3 targeting = closest - point;
		Vector3 rayDirection = targeting.normalized;
		Vector3 rayOrigin = point - rayDirection * (pointRadius * 2f + mesh.bounds.extents.magnitude);
		float rayDistance = targeting.magnitude + pointRadius * 2f + plus + mesh.bounds.extents.magnitude;
		if (!((point - closest).magnitude < pointRadius * insideMul))
		{
			return;
		}
		Vector3 collisionPoint;
		if (!inside)
		{
			collisionPoint = collision.contacts[0].point;
		}
		else
		{
			Ray ray = new Ray(rayOrigin, rayDirection);
			collisionPoint = ((!mesh.Raycast(ray, out var hit, rayDistance)) ? collision.contacts[0].point : hit.point);
		}
		if ((point - collisionPoint).magnitude < pointRadius * insideMul)
		{
			Vector3 toNormal = collisionPoint - point;
			Vector3 pushNormal = ((!inside) ? (toNormal - toNormal.normalized * pointRadius) : (toNormal + toNormal.normalized * pointRadius));
			float dot = Vector3.Dot((collisionPoint - point).normalized, rayDirection);
			if (inside && dot > 0f)
			{
				pushNormal = toNormal - toNormal.normalized * pointRadius;
			}
			point += pushNormal;
		}
	}
}
