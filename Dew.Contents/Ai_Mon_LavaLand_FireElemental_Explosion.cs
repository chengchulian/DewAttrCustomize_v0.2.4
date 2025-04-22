using System.Collections;
using UnityEngine;

public class Ai_Mon_LavaLand_FireElemental_Explosion : InstantDamageInstance
{
	public float slowDuration = 1.5f;

	public float slowStrength;

	public float knockupStrength = 1.5f;

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		CreateBasicEffect(entity, new SlowEffect
		{
			strength = slowStrength
		}, slowDuration);
		if (!entity.Status.hasUnstoppable)
		{
			entity.Visual.KnockUp(knockupStrength, isFriendly: false);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		base.transform.position = base.info.point;
	}

	protected override IEnumerator OnCreateSequenced()
	{
		yield return base.OnCreateSequenced();
		if (base.isServer)
		{
			Vector3 positionOnGround = Dew.GetPositionOnGround(base.transform.position);
			CreateAbilityInstance<Ai_Mon_LavaLand_FireElemental_ExplosionSub>(positionOnGround, null, new CastInfo(base.info.caster));
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
