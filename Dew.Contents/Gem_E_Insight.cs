using System;
using UnityEngine;

public class Gem_E_Insight : Gem
{
	public float explodeRadius;

	public ScalingValue maxHitCount;

	public GameObject activateEffect;

	public bool spawnOnBase;

	public float startDelay;

	public float interval;

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (!base.isValid || !IsReady())
		{
			return;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, info.victim.position, explodeRadius, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			sortComparer = CollisionCheckSettings.DistanceFromCenter
		});
		if (readOnlySpan.Length < 0)
		{
			handle.Return();
			return;
		}
		int num = Mathf.RoundToInt(GetValue(maxHitCount));
		float delay = startDelay;
		for (int i = 0; i < readOnlySpan.Length && i < num; i++)
		{
			Entity entity = readOnlySpan[i];
			CreateAbilityInstanceWithSource(info.actor, spawnOnBase ? entity.position : entity.Visual.GetCenterPosition(), Quaternion.identity, new CastInfo(base.owner, entity), delegate(Ai_Gem_E_Insight_Damage p)
			{
				p.chain = info.chain.New(this);
				p._delay = delay;
			});
			delay += interval;
		}
		if (base.owner.Status.TryGetStatusEffect<Se_Gem_E_Insight_Buff>(out var effect))
		{
			effect.Destroy();
		}
		CreateStatusEffectWithSource<Se_Gem_E_Insight_Buff>(info.actor, base.owner, new CastInfo(base.owner));
		FxPlayNewNetworked(activateEffect, info.victim);
		NotifyUse();
		StartCooldown();
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
