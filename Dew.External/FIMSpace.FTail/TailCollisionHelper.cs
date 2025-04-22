using UnityEngine;
using UnityEngine.Tilemaps;

namespace FIMSpace.FTail;

[AddComponentMenu("FImpossible Creations/Hidden/Tail Collision Helper")]
public class TailCollisionHelper : MonoBehaviour
{
	public TailAnimator2 ParentTail;

	public Collider TailCollider;

	public Collider2D TailCollider2D;

	public int Index;

	private Transform previousCollision;

	internal Rigidbody RigBody { get; private set; }

	internal Rigidbody2D RigBody2D { get; private set; }

	internal TailCollisionHelper Init(bool addRigidbody = true, float mass = 1f, bool kinematic = false)
	{
		if (TailCollider2D == null)
		{
			if (addRigidbody)
			{
				Rigidbody rig = GetComponent<Rigidbody>();
				if (!rig)
				{
					rig = base.gameObject.AddComponent<Rigidbody>();
				}
				rig.interpolation = RigidbodyInterpolation.Interpolate;
				rig.useGravity = false;
				rig.isKinematic = kinematic;
				rig.constraints = RigidbodyConstraints.FreezeAll;
				rig.mass = mass;
				RigBody = rig;
			}
			else
			{
				RigBody = GetComponent<Rigidbody>();
				if ((bool)RigBody)
				{
					RigBody.mass = mass;
				}
			}
		}
		else if (addRigidbody)
		{
			Rigidbody2D rig2 = GetComponent<Rigidbody2D>();
			if (!rig2)
			{
				rig2 = base.gameObject.AddComponent<Rigidbody2D>();
			}
			rig2.interpolation = RigidbodyInterpolation2D.Interpolate;
			rig2.gravityScale = 0f;
			rig2.isKinematic = kinematic;
			rig2.constraints = RigidbodyConstraints2D.FreezeAll;
			rig2.mass = mass;
			RigBody2D = rig2;
		}
		else
		{
			RigBody2D = GetComponent<Rigidbody2D>();
			if ((bool)RigBody2D)
			{
				RigBody2D.mass = mass;
			}
		}
		return this;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (ParentTail == null)
		{
			Object.Destroy(this);
			return;
		}
		TailCollisionHelper helper = collision.transform.GetComponent<TailCollisionHelper>();
		if ((!helper || (ParentTail.CollideWithOtherTails && !(helper.ParentTail == ParentTail))) && !ParentTail._TransformsGhostChain.Contains(collision.transform) && !ParentTail.IgnoredColliders.Contains(collision.collider))
		{
			ParentTail.CollisionDetection(Index, collision);
			previousCollision = collision.transform;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.transform == previousCollision)
		{
			ParentTail.ExitCollision(Index);
			previousCollision = null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && (!ParentTail.IgnoreMeshColliders || !(other is MeshCollider)) && !(other is CharacterController) && !ParentTail._TransformsGhostChain.Contains(other.transform) && !ParentTail.IgnoredColliders.Contains(other) && (ParentTail.CollideWithOtherTails || !other.transform.GetComponent<TailCollisionHelper>()))
		{
			ParentTail.AddCollider(other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (ParentTail.IncludedColliders.Contains(other) && !ParentTail.DynamicAlwaysInclude.Contains(other))
		{
			ParentTail.IncludedColliders.Remove(other);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.isTrigger && !(other is CompositeCollider2D) && !(other is TilemapCollider2D) && !(other is EdgeCollider2D) && !ParentTail._TransformsGhostChain.Contains(other.transform) && !ParentTail.IgnoredColliders2D.Contains(other) && (ParentTail.CollideWithOtherTails || !other.transform.GetComponent<TailCollisionHelper>()))
		{
			ParentTail.AddCollider(other);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (ParentTail.IncludedColliders2D.Contains(other) && !ParentTail.DynamicAlwaysInclude.Contains(other))
		{
			ParentTail.IncludedColliders2D.Remove(other);
		}
	}
}
