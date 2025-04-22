using System;
using UnityEngine;

public class Ai_C_BeamOfLight : AbilityInstance
{
	public float targetRadius = 8f;

	public float beamInterval = 0.01666667f;

	public GameObject beamEffect;

	public GameObject targetEffect;

	public GameObject handEffect;

	public GameObject healEffect;

	public LineRenderer lineRenderer;

	public DewAnimationClip canceledClip;

	public float maxDistance;

	public float selfSlow;

	public AbilityTargetValidator hittable;

	public float tickInterval;

	public ScalingValue ticks;

	public ScalingValue tickDamage;

	public ScalingValue tickHeal;

	public float procCoefficient = 0.25f;

	private float _lastTickTime;

	private int _ticksDone;

	private int _maxTicks;

	private bool _atTarget;

	private float _lastBeamInterval;

	private StatusEffect _slow;

	private AbilityTrigger _firstTrigger;

	protected override void OnCreate()
	{
		base.OnCreate();
		beamEffect.transform.position = base.info.caster.Visual.GetBonePosition(HumanBodyBones.RightHand);
		_atTarget = false;
		_lastBeamInterval = Time.time;
		lineRenderer.SetPosition(0, base.info.caster.Visual.GetBonePosition(HumanBodyBones.RightHand));
		lineRenderer.SetPosition(1, base.info.target.Visual.GetCenterPosition());
		FxPlay(beamEffect);
		FxPlay(targetEffect, base.info.target);
		FxPlay(handEffect, base.info.caster);
		lineRenderer.enabled = true;
		if (!base.isServer)
		{
			return;
		}
		_firstTrigger = base.firstTrigger;
		DestroyOnDeath(base.info.caster);
		_maxTicks = Mathf.RoundToInt(GetValue(ticks));
		base.info.caster.Control.StartChannel(new Channel
		{
			blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack | Channel.BlockedAction.Cancelable),
			duration = tickInterval * (float)(_maxTicks - 1),
			onCancel = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			},
			uncancellableTime = 0.5f
		}.AddValidation(AbilitySelfValidator.Default));
		if (selfSlow > 0f)
		{
			_slow = CreateBasicEffect(base.info.caster, new SlowEffect
			{
				strength = selfSlow
			}, tickInterval * (float)(_maxTicks - 1), "purification");
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (!(base.info.target == null))
		{
			if (Time.time - _lastBeamInterval > beamInterval)
			{
				_atTarget = !_atTarget;
				beamEffect.transform.position = (_atTarget ? base.info.target.Visual.GetCenterPosition() : base.info.caster.Visual.GetBonePosition(HumanBodyBones.RightHand));
				_lastBeamInterval = Time.time;
			}
			lineRenderer.SetPosition(0, base.info.caster.Visual.GetBonePosition(HumanBodyBones.RightHand));
			lineRenderer.SetPosition(1, base.info.target.Visual.GetCenterPosition());
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if (_firstTrigger != null)
		{
			_firstTrigger.fillAmount = 1f - (float)_ticksDone / (float)_maxTicks;
		}
		if (base.info.target != null)
		{
			base.info.caster.Control.RotateTowards(base.info.target.position, immediately: false, 0.1f);
		}
		if (Time.time - _lastTickTime < tickInterval)
		{
			return;
		}
		_lastTickTime = Time.time;
		_ticksDone++;
		if (base.info.target.IsNullInactiveDeadOrKnockedOut() || !hittable.Evaluate(base.info.caster, base.info.target))
		{
			if (_ticksDone >= _maxTicks)
			{
				Destroy();
				return;
			}
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.agentPosition, targetRadius, tvDefaultHarmfulEffectTargets);
			if (readOnlySpan.Length <= 0)
			{
				handle.Return();
				Destroy();
				return;
			}
			float target = ((base.info.target != null) ? CastInfo.GetAngle(base.info.target.position - base.info.caster.position) : base.info.caster.rotation.eulerAngles.y);
			Entity entity = null;
			float num = float.NegativeInfinity;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				float num2 = 0f - Mathf.Abs(Mathf.DeltaAngle(CastInfo.GetAngle(readOnlySpan[i].position - base.info.caster.position), target)) + ((base.info.caster.GetRelation(readOnlySpan[i]) == EntityRelation.Ally) ? (-1000f) : 0f);
				if (!(num > num2))
				{
					num = num2;
					entity = readOnlySpan[i];
				}
			}
			if (entity == null)
			{
				handle.Return();
				Destroy();
				return;
			}
			CastInfo castInfo = base.info;
			castInfo.target = entity;
			base.info = castInfo;
			handle.Return();
			FxStopNetworked(targetEffect);
			FxPlayNetworked(targetEffect, base.info.target);
		}
		Damage(tickDamage, procCoefficient).SetElemental(ElementalType.Light).SetOriginPosition(base.info.caster.position).SetAttr(DamageAttribute.DamageOverTime)
			.Dispatch(base.info.target);
		ArrayReturnHandle<Entity> handle2;
		ReadOnlySpan<Entity> readOnlySpan2 = DewPhysics.OverlapCircleAllEntities(out handle2, base.info.caster.agentPosition, 15f, tvDefaultUsefulEffectTargets, new CollisionCheckSettings
		{
			includeUncollidable = true,
			sortComparer = CollisionCheckSettings.Random
		});
		Entity entity2 = base.info.caster;
		ReadOnlySpan<Entity> readOnlySpan3 = readOnlySpan2;
		for (int j = 0; j < readOnlySpan3.Length; j++)
		{
			Entity entity3 = readOnlySpan3[j];
			if (!(entity3.normalizedHealth - 0.0001f >= entity2.normalizedHealth))
			{
				entity2 = entity3;
			}
		}
		if (!entity2.IsNullInactiveDeadOrKnockedOut())
		{
			Heal(tickHeal).Dispatch(entity2);
			FxPlayNewNetworked(healEffect, entity2);
		}
		handle2.Return();
		if (_ticksDone >= _maxTicks)
		{
			Destroy();
		}
		else if (Vector3.Distance(base.info.caster.position, base.info.target.position) > maxDistance)
		{
			Destroy();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(beamEffect);
			FxStopNetworked(targetEffect);
			FxStopNetworked(handEffect);
		}
		lineRenderer.enabled = false;
		if (base.isServer && base.info.caster != null)
		{
			base.info.caster.Animation.StopAbilityAnimation(canceledClip);
			if (_slow != null && _slow.isActive)
			{
				_slow.Destroy();
			}
		}
		if (base.isServer && _firstTrigger != null)
		{
			_firstTrigger.fillAmount = 0f;
		}
	}

	private void MirrorProcessed()
	{
	}
}
