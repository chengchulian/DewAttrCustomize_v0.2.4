using System;
using UnityEngine;

public class Se_M_Charge : StatusEffect
{
	public float speed = 12f;

	public DewEase ease;

	public float minDistance = 2f;

	public float animSpeedMultiplier = 1f;

	public bool rotateForward;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	public DewCollider knockbackRange;

	public float knockbackDamageHpRatio;

	public Vector3 colCheckOffset;

	public float colCheckRadius;

	public Knockback knockback;

	public GameObject hitEffect;

	public GameObject hitOnVictimEffect;

	public float knockupAmount;

	public float postDelay;

	public bool resetAtk;

	public float gainedShieldMaxHpRatio = 0.1f;

	public float shieldDuration = 1.5f;

	private bool _didCollide;

	private Vector3 _dir;

	private Quaternion _dirRot;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		_dir = (base.info.point - base.info.caster.agentPosition).Flattened().normalized;
		if (_dir.sqrMagnitude < 0.001f)
		{
			_dir = base.info.caster.transform.forward;
		}
		_dirRot = Quaternion.LookRotation(_dir);
		DoUncollidable();
		GiveShield(base.info.caster, gainedShieldMaxHpRatio * base.info.caster.maxHealth, shieldDuration);
		CreateBasicEffect(base.info.caster, new UncollidableEffect(), 0.3f, "ChargeMinUncollidable", DuplicateEffectBehavior.UsePrevious);
		Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, base.info.point);
		Vector3 vector = validAgentDestination_LinearSweep - base.info.caster.agentPosition;
		float magnitude = vector.magnitude;
		if (magnitude < minDistance)
		{
			vector = vector.normalized * minDistance;
			validAgentDestination_LinearSweep = base.info.caster.agentPosition + vector;
			validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, validAgentDestination_LinearSweep);
			magnitude = minDistance;
		}
		float num = magnitude / speed;
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			destination = validAgentDestination_LinearSweep,
			duration = num,
			ease = ease,
			isFriendly = true,
			onCancel = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			},
			onFinish = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			},
			rotateForward = rotateForward,
			canGoOverTerrain = false,
			isCanceledByCC = false
		});
		base.info.caster.Animation.PlayAbilityAnimation(startAnim, animSpeedMultiplier * startAnim.entries[0].duration / num);
		base.transform.rotation = Quaternion.LookRotation(validAgentDestination_LinearSweep - base.info.caster.agentPosition).Flattened();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !_didCollide)
		{
			DoCollisionCheck();
		}
	}

	private void DoCollisionCheck()
	{
		Vector3 center = base.info.caster.agentPosition + _dirRot * colCheckOffset;
		if (DewPhysics.OverlapCircleAllEntities(out var handle, center, colCheckRadius, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			sortComparer = CollisionCheckSettings.Random
		}).Length > 0)
		{
			DoCollision();
		}
		handle.Return();
	}

	private void DoCollision()
	{
		_didCollide = true;
		knockbackRange.transform.SetPositionAndRotation(base.info.caster.position, _dirRot);
		FxPlayNewNetworked(hitEffect, base.info.caster.position, base.info.caster.rotation);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = knockbackRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			knockback.ApplyWithDirection(_dirRot, entity);
			DefaultDamage(knockbackDamageHpRatio * base.info.caster.maxHealth).SetDirection(base.info.caster.rotation).Dispatch(entity);
			FxPlayNewNetworked(hitOnVictimEffect, entity);
			entity.Visual.KnockUp(knockupAmount, isFriendly: false);
		}
		handle.Return();
		base.info.caster.Control.CancelOngoingDisplacement();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !base.info.caster.IsNullOrInactive())
		{
			if (!_didCollide)
			{
				DoCollisionCheck();
			}
			base.info.caster.Animation.StopAbilityAnimation(startAnim);
			base.info.caster.Animation.PlayAbilityAnimation(endAnim);
			if (postDelay > 0f)
			{
				base.info.caster.Control.StartDaze(postDelay);
			}
			if (resetAtk)
			{
				base.info.caster.Ability.attackAbility.ResetCooldown();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
