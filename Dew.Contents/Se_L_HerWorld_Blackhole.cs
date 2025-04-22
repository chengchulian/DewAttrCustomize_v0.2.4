using System;
using UnityEngine;

public class Se_L_HerWorld_Blackhole : StatusEffect
{
	public ScalingValue armorAmount;

	public GameObject blackholeRepeatedEffect;

	public float repeatedEffectInterval;

	public AnimationCurve attractStrengthByDist;

	public Vector2 distanceBounds;

	public float duration;

	public float tickInterval = 0.25f;

	public ScalingValue perTickDamage;

	public float tickDamageRadius;

	public DewAnimationClip endClip;

	public float endDaze;

	private float _lastRepeatedEffectTime;

	private float _lastTickTime;

	private AbilityLockHandle _handle;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoDeathInterrupt(delegate
			{
				base.victim.Status.SetHealth(1f);
			}, -9999);
			DoUnstoppable();
			DoSpeed(-30f);
			DoArmorBoost(GetValue(armorAmount));
			base.victim.Visual.DisableRenderers();
			SetTimer(duration);
			ShowOnScreenTimer();
			_handle = base.info.caster.Ability.GetNewAbilityLockHandle();
			_handle.LockAllAbilitiesCast();
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = base.info.caster.position;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (Time.time - _lastRepeatedEffectTime > repeatedEffectInterval)
		{
			_lastRepeatedEffectTime = Time.time;
			FxPlay(blackholeRepeatedEffect);
		}
		foreach (Entity allEntity in NetworkedManagerBase<ActorManager>.instance.allEntities)
		{
			if (!(allEntity == base.info.caster) && !allEntity.IsNullInactiveDeadOrKnockedOut() && allEntity.Control.isLocalMovementProcessor && !allEntity.Control.isDisplacing && !allEntity.Status.hasCrowdControlImmunity && base.info.caster.CheckEnemyOrNeutral(allEntity))
			{
				float time = Mathf.Clamp01((Vector2.Distance(base.position.ToXY(), allEntity.agentPosition.ToXY()) - distanceBounds.x) / (distanceBounds.y - distanceBounds.x));
				float num = attractStrengthByDist.Evaluate(time);
				allEntity.Control.SetAgentPosition(allEntity.agentPosition + (base.position - allEntity.agentPosition).normalized * (num * dt));
			}
		}
		if (!base.isServer || !(Time.time - _lastTickTime > tickInterval))
		{
			return;
		}
		_lastTickTime = Time.time;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.position, tickDamageRadius, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!(entity == base.info.caster) && !entity.IsNullInactiveDeadOrKnockedOut())
			{
				Damage(perTickDamage, 0.5f).SetElemental(ElementalType.Light).SetAttr(DamageAttribute.DamageOverTime).SetOriginPosition(base.position)
					.Dispatch(entity);
			}
		}
		handle.Return();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			if (_handle != null)
			{
				_handle.Stop();
				_handle = null;
			}
			if (base.victim != null)
			{
				base.victim.Visual.EnableRenderers();
			}
			if (!base.victim.IsNullInactiveDeadOrKnockedOut())
			{
				base.victim.Animation.PlayAbilityAnimation(endClip);
				base.victim.Control.StartDaze(endDaze);
				CreateBasicEffect(base.victim, new SlowEffect
				{
					strength = 50f,
					decay = true
				}, 1f);
			}
			CreateAbilityInstance<Ai_L_HerWorld_Explosion>(base.info.caster.position, null, new CastInfo(base.info.caster));
		}
	}

	private void MirrorProcessed()
	{
	}
}
