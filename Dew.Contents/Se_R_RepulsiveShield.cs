using System;
using UnityEngine;

public class Se_R_RepulsiveShield : StatusEffect
{
	public DewAnimationClip stoppedClip;

	public DewCollider range;

	public GameObject blockEffect;

	public ScalingValue duration;

	public float selfSlowAmount = 50f;

	public bool cancelable;

	public float rateLimitTime = 0.5f;

	public float rateLimitCount = 2.5f;

	public float hitboxMultiplier = 2f;

	public float uncancellableTime = 0.5f;

	public int targetLimit = 4;

	private float _remainingRate;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		base.victim.Control.LockGamepadRotation();
		base.victim.Control.Rotate(base.info.rotation, immediately: true, 0.25f);
		DoSlow(selfSlowAmount);
		DoUnstoppable();
		float value = GetValue(duration);
		SetTimer(value);
		ShowOnScreenTimer();
		Channel channel = new Channel
		{
			duration = value,
			blockedActions = (Channel.BlockedAction)(6 | (cancelable ? 128 : 0)),
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
		base.victim.Control.StartChannel(channel);
		base.victim.takenDamageProcessor.Add(BlockDamage, -100);
		base.victim.Control.outerRadius = Mathf.Round(base.victim.Control.outerRadius * hitboxMultiplier * 100f) / 100f;
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.Control.UnlockGamepadRotation();
			base.victim.takenDamageProcessor.Remove(BlockDamage);
			base.victim.Control.outerRadius = Mathf.Round(base.victim.Control.outerRadius / hitboxMultiplier * 100f) / 100f;
			base.victim.Animation.StopAbilityAnimation(stoppedClip);
		}
	}

	private void BlockDamage(ref DamageData data, Actor actor, Entity target)
	{
		Entity entity = actor.firstEntity;
		if (entity == null || !base.victim.CheckEnemyOrNeutral(entity))
		{
			return;
		}
		Vector2 lhs = base.victim.transform.forward.ToXY();
		if (Vector2.Dot(lhs, (entity.position - base.victim.position).ToXY()) <= 0f && (!data.direction.HasValue || Vector2.Dot(lhs, (-data.direction.Value).ToXY()) <= 0f) && (!data.originPosition.HasValue || Vector2.Dot(lhs, (data.originPosition.Value - base.victim.position).ToXY()) <= 0f))
		{
			return;
		}
		data.BlockWithImmunity();
		FxPlayNewNetworked(blockEffect, base.victim);
		if (_remainingRate < 1f)
		{
			return;
		}
		_remainingRate -= 1f;
		if (actor.FindFirstAncestorOfType<Se_R_RepulsiveShield>() != null)
		{
			return;
		}
		range.transform.SetPositionAndRotation(base.victim.position, base.victim.rotation);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			sortComparer = CollisionCheckSettings.DistanceFromCenter
		});
		int num = 0;
		ReadOnlySpan<Entity> readOnlySpan = entities;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity target2 = readOnlySpan[i];
			if (num < targetLimit)
			{
				num++;
				CreateAbilityInstance<Ai_R_RepulsiveShield_Projectile>(base.victim.position, Quaternion.identity, new CastInfo(base.victim, target2));
			}
		}
		handle.Return();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			base.victim.Control.Rotate(base.info.rotation, immediately: true, 0.25f);
			_remainingRate = Mathf.MoveTowards(_remainingRate, rateLimitCount, dt * rateLimitCount / rateLimitTime);
		}
	}

	private void MirrorProcessed()
	{
	}
}
