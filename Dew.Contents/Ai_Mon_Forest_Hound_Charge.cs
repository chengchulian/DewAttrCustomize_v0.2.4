using System;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Forest_Hound_Charge : AbilityInstance
{
	public float duration;

	public float afterLandDelay = 0.5f;

	public float distance;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public bool unstoppableWhileCharging;

	public Knockback knockback;

	public ScalingValue dmgFactor = new ScalingValue(0f, 1f, 0f, 0f);

	public GameObject chargeEffect;

	public EntityAnimation animation;

	public DewAnimationClip clip;

	public float animSpeed;

	public GameObject hitEffect;

	private StatusEffect _unstoppable;

	private List<Entity> entsHit = new List<Entity>();

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		animation = base.info.caster.Animation;
		animation.PlayAbilityAnimation(clip, animSpeed);
		if (unstoppableWhileCharging)
		{
			_unstoppable = CreateBasicEffect(base.info.caster, new UnstoppableEffect(), float.PositiveInfinity);
		}
		FxPlayNetworked(chargeEffect, base.info.caster);
		Vector3 destination = base.info.caster.position + base.info.forward * distance;
		base.info.caster.Visual.KnockUp(0.8f, isFriendly: true);
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			affectedByMovementSpeed = true,
			destination = destination,
			duration = duration,
			ease = DewEase.Linear,
			isFriendly = true,
			onFinish = delegate
			{
				base.info.caster.Control.StartDaze(afterLandDelay);
				Destroy();
			},
			onCancel = delegate
			{
				if (base.isActive)
				{
					base.info.caster.Control.StartDaze(afterLandDelay);
					Destroy();
				}
			},
			rotateForward = true,
			canGoOverTerrain = false,
			isCanceledByCC = true
		});
		DestroyOnDeath(base.info.caster);
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		range.transform.position = base.info.caster.position;
		range.transform.rotation = base.info.caster.rotation;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, hittable, base.info.caster);
		for (int i = 0; i < entities.Length; i++)
		{
			bool flag = true;
			Entity entity = entities[i];
			for (int j = 0; j < entsHit.Count; j++)
			{
				if (entity == entsHit[j])
				{
					flag = false;
				}
			}
			if (flag)
			{
				OnHit(entity);
				entsHit.Add(entity);
			}
		}
		handle.Return();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			animation.StopAbilityAnimation();
			FxStopNetworked(chargeEffect);
			if (_unstoppable != null && _unstoppable.isActive)
			{
				_unstoppable.Destroy();
			}
		}
	}

	private void OnHit(Entity entity)
	{
		CreateDamage(DamageData.SourceType.Default, dmgFactor).Dispatch(entity);
		knockback.ApplyWithOrigin(base.info.caster.position, entity);
		FxPlayNewNetworked(hitEffect, entity);
	}

	private void MirrorProcessed()
	{
	}
}
