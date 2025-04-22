using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_TurnAtk_DelayedAtk : AbilityInstance
{
	public DewCollider range;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public float delay;

	public GameObject fxTelegraph;

	public GameObject fxExplode;

	public GameObject fxHit;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			FxPlayNetworked(fxTelegraph);
			yield return new SI.WaitForSeconds(delay);
			FxPlayNetworked(fxExplode);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				FxPlayNewNetworked(fxHit, entity);
				knockback.ApplyWithOrigin(base.info.point, entity);
				entity.Visual.KnockUp(KnockUpStrength.Big, isFriendly: false);
				DefaultDamage(dmgFactor).SetOriginPosition(base.position).Dispatch(entity);
				CreateBasicEffect(entity, new SlowEffect
				{
					decay = true,
					strength = 80f
				}, 2.5f, "TurnAtkSlow", DuplicateEffectBehavior.UsePrevious);
			}
			handle.Return();
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
