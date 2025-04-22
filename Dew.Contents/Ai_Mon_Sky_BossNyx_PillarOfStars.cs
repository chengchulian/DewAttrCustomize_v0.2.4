using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_PillarOfStars : AbilityInstance
{
	public float initDelay;

	public GameObject fxTelegraph;

	public GameObject fxPillar;

	public GameObject fxHit;

	public DewCollider range;

	public int tickCount;

	public float tickInterval;

	public ScalingValue firstDamage;

	public ScalingValue afterFirstDamage;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		FxPlayNetworked(fxTelegraph);
		yield return new SI.WaitForSeconds(initDelay);
		FxStopNetworked(fxTelegraph);
		FxPlayNetworked(fxPillar);
		List<Entity> hitEnts = new List<Entity>();
		for (int i = 0; i < tickCount; i++)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int j = 0; j < entities.Length; j++)
			{
				Entity entity = entities[j];
				if (hitEnts.Contains(entity))
				{
					Damage(afterFirstDamage).SetOriginPosition(base.position).Dispatch(entity);
				}
				else
				{
					hitEnts.Add(entity);
					Damage(firstDamage).SetOriginPosition(base.position).Dispatch(entity);
				}
				FxPlayNewNetworked(fxHit, entity);
			}
			handle.Return();
			yield return new SI.WaitForSeconds(tickInterval);
		}
		Destroy();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(fxTelegraph);
			FxStopNetworked(fxPillar);
		}
	}

	private void MirrorProcessed()
	{
	}
}
