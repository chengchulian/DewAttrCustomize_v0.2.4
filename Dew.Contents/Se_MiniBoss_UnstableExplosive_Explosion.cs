using System;
using UnityEngine;

public class Se_MiniBoss_UnstableExplosive_Explosion : StatusEffect
{
	public float explodeDelay;

	public float maxSlow = 50f;

	public DewCollider explodeRange;

	public GameObject fxExplode;

	public GameObject fxExplodeHit;

	public ScalingValue explodeDamage;

	public Knockback explodeKnockback;

	public float selfDamageCurrentHpRatio;

	public float selfDamageMaxHpRatio;

	public float selfStunDuration;

	private SlowEffect _slow;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.Control.StartChannel(new Channel
			{
				blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack),
				duration = explodeDelay
			});
			SetTimer(explodeDelay);
			_slow = DoSlow(0f);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.normalizedDuration.HasValue)
		{
			float t = 1f - base.normalizedDuration.Value;
			if (_slow != null)
			{
				_slow.strength = Mathf.Lerp(0f, maxSlow, t);
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !base.victim.IsNullInactiveDeadOrKnockedOut())
		{
			FxPlayNetworked(fxExplode, base.victim);
			explodeRange.transform.position = base.victim.agentPosition;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = explodeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				DefaultDamage(explodeDamage).SetOriginPosition(base.victim.position).Dispatch(entity);
				FxPlayNewNetworked(fxExplodeHit, entity);
				explodeKnockback.ApplyWithOrigin(base.victim.position, entity);
			}
			handle.Return();
			DefaultDamage(selfDamageCurrentHpRatio * base.victim.currentHealth + selfDamageMaxHpRatio * base.victim.maxHealth).Dispatch(base.victim);
			CreateBasicEffect(base.victim, new StunEffect(), selfStunDuration, "unstablestun", DuplicateEffectBehavior.UsePrevious);
		}
	}

	private void MirrorProcessed()
	{
	}
}
