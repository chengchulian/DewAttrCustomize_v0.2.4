using UnityEngine;

public class Ai_R_Fireball : StandardProjectile
{
	public AbilityTargetValidator explodeHittable;

	public DewCollider explodeRange;

	public GameObject explodeEffect;

	public GameObject hitEffect;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public float stunDuration;

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
