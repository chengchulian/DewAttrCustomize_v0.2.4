using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_Artillery_Instance : AbilityInstance
{
	[HideInInspector]
	public float delay;

	public float dmgDelay;

	public ScalingValue dmgFactor;

	public DewCollider range;

	public Knockback knockback;

	public GameObject fxTelegraph;

	public GameObject fxInstance;

	public GameObject fxHit;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			FxPlayNetworked(fxTelegraph, base.info.point, null);
			yield return new SI.WaitForSeconds(delay);
			FxStopNetworked(fxTelegraph);
			FxPlayNetworked(fxInstance, base.info.point, null);
			yield return new SI.WaitForSeconds(dmgDelay);
			range.transform.position = base.info.point;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				CreateDamage(DamageData.SourceType.Default, dmgFactor).SetOriginPosition(base.info.point).Dispatch(entity);
				CreateBasicEffect(entity, new SlowEffect
				{
					decay = true,
					strength = 50f
				}, 1f, "ObvArtillerySlow", DuplicateEffectBehavior.UsePrevious);
				FxPlayNewNetworked(fxHit, entity);
			}
			handle.Return();
			Destroy();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(fxTelegraph);
		}
	}

	private void MirrorProcessed()
	{
	}
}
