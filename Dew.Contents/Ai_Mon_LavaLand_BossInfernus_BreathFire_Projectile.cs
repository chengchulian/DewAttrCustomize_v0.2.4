using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_BreathFire_Projectile : StandardProjectile
{
	public class Ad_FlameJet
	{
		public float hitTime;
	}

	public float startHeight;

	public float procCoefficient;

	public float minHitInterval;

	public GameObject hitEffect;

	public ScalingValue damage;

	public Vector2 collisionRadiusRange;

	protected override void OnPrepare()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnEntity(EntityHit hit)
	{
	}

	private void MirrorProcessed()
	{
	}
}
