using UnityEngine;

public class Ai_Mon_LavaLand_Magmadon_Charge_Projectile : StandardProjectile
{
	public ScalingValue dmgFactor;

	public DewCollider range;

	public Knockback Knockback;

	public GameObject fxExplode;

	protected override void OnComplete()
	{
	}

	private void MirrorProcessed()
	{
	}
}
