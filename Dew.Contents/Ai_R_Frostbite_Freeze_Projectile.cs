using UnityEngine;

public class Ai_R_Frostbite_Freeze_Projectile : StandardProjectile
{
	public ScalingValue freezeDamage;

	public float stunDuration;

	public GameObject entityAttachEffect;

	protected override void OnEntity(EntityHit hit)
	{
	}

	private void MirrorProcessed()
	{
	}
}
