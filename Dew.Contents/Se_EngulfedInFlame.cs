using System;
using UnityEngine;

public class Se_EngulfedInFlame : StatusEffect
{
	public float immolationInterval;

	public float immolationRadius;

	public GameObject fxImmolationHit;

	public ScalingValue immolationDamage;

	public float minDelay;

	private float _lastImmolationTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.dealtDamageProcessor.Add(VictimOndealtDamageProcessor, -1);
			_lastImmolationTime = Time.time + minDelay;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null))
		{
			base.victim.dealtDamageProcessor.Remove(VictimOndealtDamageProcessor);
			if (base.victim.Status.TryGetStatusEffect<Se_Elm_Fire>(out var effect))
			{
				effect.Destroy();
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if (base.victim.Status.fireStack <= 0)
		{
			Dew.GetClosestAliveHero(base.victim.position).ApplyElemental(ElementalType.Fire, base.victim);
		}
		if (Time.time - _lastImmolationTime < immolationInterval || base.victim.isSleeping || base.victim.Visual.isSpawning)
		{
			return;
		}
		_lastImmolationTime = Time.time;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.victim.position, immolationRadius, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!entity.Control.isDisplacing)
			{
				Damage(immolationDamage, 0.66f).SetElemental(ElementalType.Fire).SetOriginPosition(base.victim.position).SetAttr(DamageAttribute.DamageOverTime)
					.Dispatch(entity);
				FxPlayNewNetworked(fxImmolationHit, entity);
			}
		}
		handle.Return();
	}

	private void VictimOndealtDamageProcessor(ref DamageData data, Actor actor, Entity target)
	{
		if (!(actor is ElementalStatusEffect) && !(base.victim == target))
		{
			data.SetElemental(ElementalType.Fire);
		}
	}

	private void MirrorProcessed()
	{
	}
}
