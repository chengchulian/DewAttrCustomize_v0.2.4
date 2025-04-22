using UnityEngine;

namespace FIMSpace;

public abstract class FImp_ColliderData_Base
{
	public enum EFColliderType
	{
		Box,
		Sphere,
		Capsule,
		Mesh,
		Terrain
	}

	public bool Is2D;

	protected Vector3 previousPosition = Vector3.zero;

	protected Quaternion previousRotation = Quaternion.identity;

	protected Vector3 previousScale = Vector3.one;

	public Transform Transform { get; protected set; }

	public Collider Collider { get; protected set; }

	public Collider2D Collider2D { get; protected set; }

	public bool IsStatic { get; private set; }

	public EFColliderType ColliderType { get; protected set; }

	public static FImp_ColliderData_Base GetColliderDataFor(Collider collider)
	{
		SphereCollider s = collider as SphereCollider;
		if ((bool)s)
		{
			return new FImp_ColliderData_Sphere(s);
		}
		CapsuleCollider c = collider as CapsuleCollider;
		if ((bool)c)
		{
			return new FImp_ColliderData_Capsule(c);
		}
		BoxCollider b = collider as BoxCollider;
		if ((bool)b)
		{
			return new FImp_ColliderData_Box(b);
		}
		MeshCollider m = collider as MeshCollider;
		if ((bool)m)
		{
			return new FImp_ColliderData_Mesh(m);
		}
		TerrainCollider t = collider as TerrainCollider;
		if ((bool)t)
		{
			return new FImp_ColliderData_Terrain(t);
		}
		return null;
	}

	public static FImp_ColliderData_Base GetColliderDataFor(Collider2D collider)
	{
		CircleCollider2D s = collider as CircleCollider2D;
		if ((bool)s)
		{
			return new FImp_ColliderData_Sphere(s);
		}
		CapsuleCollider2D c = collider as CapsuleCollider2D;
		if ((bool)c)
		{
			return new FImp_ColliderData_Capsule(c);
		}
		BoxCollider2D b = collider as BoxCollider2D;
		if ((bool)b)
		{
			return new FImp_ColliderData_Box(b);
		}
		PolygonCollider2D m = collider as PolygonCollider2D;
		if ((bool)m)
		{
			return new FImp_ColliderData_Mesh(m);
		}
		return null;
	}

	public virtual void RefreshColliderData()
	{
		if (Transform.gameObject.isStatic)
		{
			IsStatic = true;
		}
		else
		{
			IsStatic = false;
		}
	}

	public virtual bool PushIfInside(ref Vector3 point, float pointRadius, Vector3 pointOffset)
	{
		if ((bool)(Collider as SphereCollider))
		{
			Debug.Log("It shouldn't appear");
		}
		return false;
	}

	public virtual bool PushIfInside2D(ref Vector3 point, float pointRadius, Vector3 pointOffset)
	{
		return PushIfInside(ref point, pointRadius, pointOffset);
	}

	public static bool VIsSame(Vector3 vec1, Vector3 vec2)
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
}
