using System;
using UnityEngine;

public class Se_Mon_SnowMountain_SnowWolf_SlowApproach : StatusEffect
{
	public AnimationClip walkAnimation;

	public float walkAnimationSpeed;

	public AnimationClip runAnimation;

	public float runAnimationSpeed;

	public float timeout;

	public float slowAmount = 40f;

	public Vector2 distanceRange;

	public bool endOnDamage;

	public bool resetPounceOnEnd;

	public float speedOnEnd;

	public float speedDuration;

	public bool isSpeedDecay;

	public bool castPounceAtEnd;

	protected override void OnCreate()
	{
		base.OnCreate();
		base.victim.Animation.ReplaceAnimationLocal(EntityAnimation.ReplaceableAnimationType.RunForward, walkAnimation);
		base.victim.Animation.walkAnimationSpeed = walkAnimationSpeed;
		if (base.isServer)
		{
			if (base.info.target.IsNullInactiveDeadOrKnockedOut())
			{
				Destroy();
				return;
			}
			SetTimer(timeout);
			DoSlow(slowAmount);
			base.victim.Control.Attack(base.info.target, doChase: true);
			base.victim.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(EntityEventOnTakeDamage);
		}
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
		if (base.isActive && !(obj.actor is ElementalStatusEffect) && endOnDamage)
		{
			Entity entity = obj.actor.firstEntity;
			if (entity != null && entity != base.info.target)
			{
				base.victim.AI.Aggro(entity);
				base.victim.Control.Attack(entity, doChase: true);
			}
			Destroy();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.victim != null)
		{
			base.victim.Animation.ReplaceAnimationLocal(EntityAnimation.ReplaceableAnimationType.RunForward, runAnimation);
			base.victim.Animation.walkAnimationSpeed = runAnimationSpeed;
		}
		if (base.isServer && base.victim != null && base.victim.isActive)
		{
			base.victim.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(EntityEventOnTakeDamage);
			At_Mon_SnowMountain_SnowWolf_Pounce ability = base.victim.Ability.GetAbility<At_Mon_SnowMountain_SnowWolf_Pounce>();
			if (resetPounceOnEnd && ability != null)
			{
				ability.ResetCooldown();
			}
			if (speedOnEnd > 0f)
			{
				CreateBasicEffect(base.victim, new SpeedEffect
				{
					decay = isSpeedDecay,
					strength = speedOnEnd
				}, speedDuration, "snowwolf_speed");
			}
			if (castPounceAtEnd && ability.CanBeCast() && base.victim.AI.context.targetEnemy != null && ability.IsTargetInRange(base.victim.AI.context.targetEnemy))
			{
				base.victim.Control.Cast(ability, ability.GetPredictedCastInfoToTarget(base.victim.AI.context.targetEnemy));
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
		if (base.victim.Control.attackTarget.IsNullInactiveDeadOrKnockedOut() || base.victim.Control.ongoingChannels.Count > 0)
		{
			Destroy();
			return;
		}
		if (base.info.target.IsNullInactiveDeadOrKnockedOut() || base.info.target.Status.hasInvisible)
		{
			Destroy();
			return;
		}
		float num = Vector3.Distance(base.victim.position, base.info.target.position);
		if (num < distanceRange.x || num > distanceRange.y)
		{
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
