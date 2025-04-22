using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_BossSkoll_AuraBlade : AbilityInstance
{
	public DewCollider range;

	public ScalingValue dmgFactor;

	public float delay;

	public GameObject cutEffect;

	public GameObject hitEffect;

	public KnockUpStrength knockUpStrength;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			yield return new SI.WaitForSeconds(delay);
			FxPlayNewNetworked(cutEffect);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				entity.Visual.KnockUp(knockUpStrength, isFriendly: true);
				CreateDamage(DamageData.SourceType.Default, dmgFactor).Dispatch(entity);
				FxPlayNewNetworked(hitEffect, entity);
			}
			handle.Return();
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
