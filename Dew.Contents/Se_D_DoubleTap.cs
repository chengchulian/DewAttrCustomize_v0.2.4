using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Se_D_DoubleTap : StackedStatusEffect
{
	public GameObject chargeEffect;

	public GameObject firstShotEffect;

	public GameObject secondShotEffect;

	public ScalingValue critChanceBonus;

	public ScalingValue attackDamageBonus;

	public bool allowMovementSkill;

	public float shootDelay;

	private StatBonus _bonus;

	public Hero heroVictim => base.victim as Hero;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			heroVictim.EntityEvent_OnAttackFired += new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
			heroVictim.ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			_bonus = new StatBonus
			{
				critChanceFlat = GetValue(critChanceBonus),
				attackDamageFlat = GetValue(attackDamageBonus)
			};
			heroVictim.Status.AddStatBonus(_bonus);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(heroVictim == null))
		{
			base.victim.EntityEvent_OnAttackFired -= new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
			heroVictim.ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			if (base.firstTrigger != null)
			{
				base.firstTrigger.fillAmount = 0f;
			}
			if (_bonus != null)
			{
				heroVictim.Status.RemoveStatBonus(_bonus);
			}
		}
	}

	private void HeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		if ((obj.type == HeroSkillLocation.Q || obj.type == HeroSkillLocation.W || obj.type == HeroSkillLocation.E || obj.type == HeroSkillLocation.R || (allowMovementSkill && obj.type == HeroSkillLocation.Movement)) && base.stack < maxStack)
		{
			AddStack();
			FxPlayNewNetworked(chargeEffect, base.victim);
			base.victim.Ability.attackAbility.ResetCooldown();
		}
	}

	private void EntityEventOnAttackFired(EventInfoAttackFired obj)
	{
		if (base.stack > 0 && !(Vector3.Magnitude(obj.instance.info.point - At_Atk_LacertaRifle.MagicNumber) < 0.01f))
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			RemoveStack();
			FxPlayNewNetworked(firstShotEffect, base.victim);
			yield return new WaitForSeconds(shootDelay * base.victim.Ability.attackAbility.GetCooldownTimeMultiplier(base.victim.Ability.attackAbility.currentConfigIndex));
			if (base.victim.Ability.attackAbility is At_Atk_LacertaRifle at_Atk_LacertaRifle)
			{
				Entity entity = obj.info.target;
				if (entity.IsNullInactiveDeadOrKnockedOut() || entity.currentHealth + entity.Status.currentShield < base.victim.Status.attackDamage)
				{
					Vector3 pivot = ((entity == null) ? base.victim.owner.cursorWorldPos : entity.position);
					float radius = base.victim.Ability.attackAbility.currentConfig.effectiveRange + 1.5f;
					ArrayReturnHandle<Entity> handle;
					ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.victim.agentPosition, radius, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
					{
						sortComparer = Comparer<Entity>.Create((Entity x, Entity y) => Vector2.Distance(pivot.ToXY(), x.agentPosition.ToXY()).CompareTo(Vector2.Distance(pivot.ToXY(), y.agentPosition.ToXY())))
					});
					if (readOnlySpan.Length > 0)
					{
						entity = readOnlySpan[0];
					}
					handle.Return();
				}
				at_Atk_LacertaRifle.ResetCooldown();
				if (entity != null)
				{
					at_Atk_LacertaRifle.Shoot(entity);
					FxPlayNewNetworked(secondShotEffect, base.victim);
				}
				else
				{
					at_Atk_LacertaRifle.Shoot((obj.info.angle != 0f) ? obj.info.angle : CastInfo.GetAngle(base.victim.owner.cursorWorldPos - base.victim.agentPosition));
				}
			}
		}
	}

	protected override void OnStackChange(int oldStack, int newStack)
	{
		base.OnStackChange(oldStack, newStack);
		if (base.isServer)
		{
			base.firstTrigger.fillAmount = (float)newStack / (float)maxStack;
		}
	}

	private void MirrorProcessed()
	{
	}
}
