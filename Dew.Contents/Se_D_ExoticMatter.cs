using System;
using System.Collections;
using UnityEngine;

public class Se_D_ExoticMatter : StackedStatusEffect
{
	public float attackCooldownAfterTrigger;

	public DewAnimationClip shootAnim;

	public float directionAtkSphereCastRadius = 1f;

	private St_D_ExoticMatter _trigger;

	private Hero _hero;

	protected override void OnCreate()
	{
		base.OnCreate();
		_trigger = (St_D_ExoticMatter)base.firstTrigger;
		_hero = (Hero)base.victim;
		if (base.isServer)
		{
			_trigger.fillAmount = (float)base.stack / (float)maxStack;
			_hero.ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			_hero.EntityEvent_OnAttackFired += new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
			_hero.Visual.genericStackIndicatorMax = maxStack;
		}
	}

	private void EntityEventOnAttackFired(EventInfoAttackFired obj)
	{
		if (base.stack <= 0)
		{
			return;
		}
		Vector3 point;
		if (obj.info.target == null)
		{
			point = obj.info.point;
			float maxDistance = Vector3.Distance(obj.info.point, base.victim.agentPosition);
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.SphereCastAllEntities(out handle, base.victim.agentPosition, directionAtkSphereCastRadius, obj.info.forward, maxDistance, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				sortComparer = CollisionCheckSettings.DistanceFromCenter
			});
			if (readOnlySpan.Length > 0)
			{
				point = readOnlySpan[0].agentPosition;
			}
			handle.Return();
		}
		else
		{
			point = obj.info.target.agentPosition;
		}
		CreateAbilityInstance(base.info.caster.Visual.GetBonePosition(HumanBodyBones.RightHand), Quaternion.identity, new CastInfo(base.info.caster, point), delegate(Ai_D_ExoticMatter_Projectile p)
		{
			p.explosionStack = base.stack;
		});
		RemoveStack(base.stack);
		if (attackCooldownAfterTrigger > 0f)
		{
			StartCoroutine(Routine());
			base.info.caster.Ability.attackAbility.SetCooldownTimeAll(attackCooldownAfterTrigger, scaled: false);
		}
		IEnumerator Routine()
		{
			yield return null;
			if (!(base.info.caster == null) && base.info.caster.isActive && !((Hero)base.info.caster).isKnockedOut)
			{
				if (base.info.caster.Ability.attackAbility is At_Atk_YubarStardust at_Atk_YubarStardust)
				{
					at_Atk_YubarStardust.ClearTarget();
				}
				base.info.caster.Animation.PlayAbilityAnimation(shootAnim);
			}
		}
	}

	private void HeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		if ((obj.type == HeroSkillLocation.Q || obj.type == HeroSkillLocation.W || obj.type == HeroSkillLocation.E || obj.type == HeroSkillLocation.R || obj.type == HeroSkillLocation.Movement) && base.stack < maxStack)
		{
			AddStack();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _hero != null)
		{
			_hero.ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			_hero.EntityEvent_OnAttackFired -= new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
			_hero.Visual.genericStackIndicatorMax = 0;
			_hero.Visual.genericStackIndicatorValue = 0;
		}
	}

	protected override void OnStackChange(int oldStack, int newStack)
	{
		base.OnStackChange(oldStack, newStack);
		if (base.isServer)
		{
			_trigger.fillAmount = (float)newStack / (float)maxStack;
			_hero.Visual.genericStackIndicatorValue = base.stack;
		}
	}

	private void MirrorProcessed()
	{
	}
}
