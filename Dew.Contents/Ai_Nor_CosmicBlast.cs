using UnityEngine;

public class Ai_Nor_CosmicBlast : StandardProjectile
{
	public AbilityTargetValidator explodeHittable;

	public DewCollider explodeRange;

	public GameObject explodeEffect;

	public GameObject hitEffect;

	public ScalingValue dmgFactor;

	protected override void OnEntity(EntityHit hit)
	{
	}

	protected override void OnComplete()
	{
	}

	private void Explode()
	{
	}

	private void MirrorProcessed()
	{
	}
}
