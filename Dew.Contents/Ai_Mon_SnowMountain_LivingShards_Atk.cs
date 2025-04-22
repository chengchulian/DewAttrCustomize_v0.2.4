using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_LivingShards_Atk : AbilityInstance
{
	public float castDelay;

	public ScalingValue dmgFactor;

	public GameObject telegraph;

	public GameObject fxAtk;

	public GameObject fxHit;

	public DewCollider range;

	protected override IEnumerator OnCreateSequenced()
	{
		base.transform.position = base.info.point;
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			FxPlayNetworked(telegraph, base.info.caster);
			yield return new SI.WaitForSeconds(castDelay);
			FxStopNetworked(telegraph);
			FxPlayNetworked(fxAtk, base.info.point, null);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				CreateDamage(DamageData.SourceType.Default, dmgFactor).SetElemental(ElementalType.Cold).SetOriginPosition(base.transform.position).Dispatch(entity);
				FxPlayNetworked(fxHit, entity);
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
			FxStopNetworked(telegraph);
		}
	}

	private void MirrorProcessed()
	{
	}
}
