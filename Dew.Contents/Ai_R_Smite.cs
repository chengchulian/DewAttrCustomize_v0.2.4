using System;
using System.Collections;
using UnityEngine;

public class Ai_R_Smite : AbilityInstance
{
	public float delay = 0.5f;

	public DewCollider range;

	public GameObject smiteEffect;

	public GameObject hitEffect;

	public ScalingValue damage;

	public float stunDuration = 2.5f;

	public float fireChance = 0.7f;

	protected override IEnumerator OnCreateSequenced()
	{
		base.transform.position = base.info.point;
		yield return new SI.WaitForSeconds(delay);
		if (!base.isServer)
		{
			yield break;
		}
		FxPlayNetworked(smiteEffect);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			Damage(damage).SetElemental(ElementalType.Light).Dispatch(entity);
			if (global::UnityEngine.Random.value < fireChance)
			{
				ApplyElemental(ElementalType.Fire, entity);
			}
			FxPlayNewNetworked(hitEffect, entity);
			CreateBasicEffect(entity, new StunEffect(), stunDuration, "SmiteStun");
		}
		handle.Return();
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
