using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_StellarDash : AbilityInstance
{
	public DewAnimationClip animToCancel;

	public DewAnimationClip endAnim;

	public float colRadius;

	public ScalingValue damage;

	public float slowAmount;

	public float slowDuration;

	public bool isSlowDecay;

	public GameObject hitEffect;

	public float speed;

	public DewEase ease;

	public bool resetSwipe = true;

	private Vector3 _lastPos;

	private List<Entity> _hitEnts = new List<Entity>();

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, base.info.point);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = true,
				canGoOverTerrain = false,
				destination = validAgentDestination_LinearSweep,
				duration = Vector2.Distance(base.info.caster.agentPosition.ToXY(), validAgentDestination_LinearSweep.ToXY()) / speed,
				ease = ease,
				isCanceledByCC = true,
				isFriendly = true,
				onCancel = base.DestroyIfActive,
				onFinish = base.DestroyIfActive,
				rotateForward = true
			});
			_lastPos = base.info.caster.position;
		}
		yield break;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		Vector3 lastPos = _lastPos;
		Vector3 vector = (_lastPos = base.info.caster.position);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.SphereCastAllEntities(out handle, lastPos, colRadius, vector - lastPos, (vector - lastPos).ToXY().magnitude, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!_hitEnts.Contains(entity))
			{
				_hitEnts.Add(entity);
				if (GetValue(damage) > 0f)
				{
					Damage(damage).SetDirection(vector - lastPos).Dispatch(entity);
				}
				FxPlayNewNetworked(hitEffect, entity);
				CreateBasicEffect(entity, new SlowEffect
				{
					decay = isSlowDecay,
					strength = slowAmount
				}, slowDuration, "stellar_slow");
			}
		}
		handle.Return();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			if (endAnim != null)
			{
				base.info.caster.Animation.PlayAbilityAnimation(endAnim);
			}
			else if (animToCancel != null)
			{
				base.info.caster.Animation.StopAbilityAnimation(animToCancel);
			}
			if (resetSwipe && base.info.caster.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Swipe>(out var trigger))
			{
				trigger.ResetCooldown();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
