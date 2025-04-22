using System;
using UnityEngine;

public class Se_R_Parry_Start : StatusEffect
{
	public DewCollider range;

	public GameObject parryEffect;

	public float duration = 2.5f;

	public float selfSlowAmount = 50f;

	public bool cancelable;

	public float uncancellableTime = 0.5f;

	public DewAnimationClip parryAnim;

	public float reducedCooldownPercentage = 0.5f;

	public float parrySuccessLookDuration = 1f;

	public ScalingValue maxEnemiesHit = "4";

	public float hitboxMultiplier = 2f;

	public bool resetMeister = true;

	[NonSerialized]
	public bool allowAnyDirection;

	private Channel _channel;

	private bool _didParry;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		base.victim.Control.Rotate(base.info.rotation, immediately: true, 0.25f);
		DoSlow(selfSlowAmount);
		DoUnstoppable();
		SetTimer(duration);
		ShowOnScreenTimer();
		_channel = new Channel
		{
			duration = duration,
			blockedActions = (Channel.BlockedAction)(7 | (cancelable ? 128 : 0)),
			uncancellableTime = uncancellableTime,
			onCancel = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			},
			onComplete = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			}
		};
		base.victim.Control.StartChannel(_channel);
		base.victim.takenDamageProcessor.Add(BlockDamage, -100);
		base.victim.Control.outerRadius = Mathf.Round(base.victim.Control.outerRadius * hitboxMultiplier * 100f) / 100f;
		base.victim.Ability.attackAbility.ResetCooldown();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			if (base.victim != null)
			{
				base.victim.takenDamageProcessor.Remove(BlockDamage);
				base.victim.Control.outerRadius = Mathf.Round(base.victim.Control.outerRadius / hitboxMultiplier * 100f) / 100f;
			}
			if (_channel.isAlive)
			{
				_channel.Cancel();
			}
		}
	}

	private void BlockDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (data.HasAttr(DamageAttribute.IgnoreDamageImmunity) && !actor.IsDescendantOf<Gem_E_Overload>())
		{
			return;
		}
		if (!allowAnyDirection)
		{
			Vector2 lhs = base.victim.transform.forward.ToXY();
			if ((!data.direction.HasValue || Vector2.Dot(lhs, (-data.direction.Value).ToXY()) <= 0f) && (!data.originPosition.HasValue || Vector2.Dot(lhs, (data.originPosition.Value - base.victim.position).ToXY()) <= 0f))
			{
				Entity entity = actor.firstEntity;
				if (entity == null || Vector2.Dot(lhs, (entity.position - base.victim.position).ToXY()) <= 0f)
				{
					return;
				}
			}
		}
		data.BlockWithImmunity();
		if (actor is ElementalStatusEffect || _didParry)
		{
			return;
		}
		_didParry = true;
		SetTimer(0.1f);
		HideOnScreenTimer();
		FxPlayNetworked(parryEffect, base.victim);
		range.transform.SetPositionAndRotation(base.victim.position, base.info.rotation);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			sortComparer = CollisionCheckSettings.DistanceFromCenter
		});
		int num = 0;
		int num2 = Mathf.RoundToInt(GetValue(maxEnemiesHit));
		ReadOnlySpan<Entity> readOnlySpan = entities;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity2 = readOnlySpan[i];
			CreateAbilityInstance<Ai_R_Parry_Projectile>(base.victim.position, Quaternion.identity, new CastInfo(base.victim, entity2));
			if (resetMeister)
			{
				Se_D_AstridsMasterpiece_Exposed se_D_AstridsMasterpiece_Exposed = entity2.Status.FindStatusEffect((Se_D_AstridsMasterpiece_Exposed e) => e.info.caster == base.victim);
				if (se_D_AstridsMasterpiece_Exposed != null)
				{
					se_D_AstridsMasterpiece_Exposed.chargeCount = se_D_AstridsMasterpiece_Exposed.maxCharge;
				}
			}
			num++;
			if (num >= num2)
			{
				break;
			}
		}
		handle.Return();
		CreateStatusEffect<Se_R_Parry_End>(base.victim, new CastInfo(base.info.caster));
		AbilityTrigger abilityTrigger = base.firstTrigger;
		if (abilityTrigger != null)
		{
			abilityTrigger.ApplyCooldownReductionByRatio(reducedCooldownPercentage);
		}
		base.victim.Animation.PlayAbilityAnimation(parryAnim);
		base.victim.Control.Rotate(base.info.rotation, immediately: true, parrySuccessLookDuration);
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			base.victim.Control.Rotate(base.info.rotation, immediately: true, 0.25f);
		}
	}

	private void MirrorProcessed()
	{
	}
}
