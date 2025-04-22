using UnityEngine;

public class Ai_Mon_DarkCave_NightOlm_Atk_Projectile : StandardProjectile
{
	public class Ad_NightOlmAtk
	{
		public Entity caster;

		public float hitTime;
	}

	public float minHitInterval;

	public GameObject fxHit;

	public ScalingValue dmgFactor;

	public Knockback Knockback;

	protected override void OnEntity(EntityHit hit)
	{
	}

	private void MirrorProcessed()
	{
	}
}
