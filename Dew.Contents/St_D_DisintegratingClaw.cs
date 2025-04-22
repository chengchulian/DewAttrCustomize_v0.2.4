using System;
using UnityEngine;

public class St_D_DisintegratingClaw : SkillTrigger
{
	public ScalingValue damageAmount;

	public ScalingValue healAmount;

	public ScalingValue bossAmp;

	public float maxAmpHpThreshold = 0.3f;

	public float maxAmp = 2f;

	public GameObject fxAttackHit;

	public GameObject fxHealSelf;

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer && formerOwner != null)
		{
			formerOwner.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
		FxPlayNewNetworked(fxAttackHit, obj.victim);
		FxPlayNewNetworked(fxHealSelf, base.owner);
		HealData healData = Heal(GetValue(healAmount) * obj.strength).SetCanMerge();
		DamageData damageData = Damage(damageAmount, 0.5f).ApplyStrength(obj.strength).SetOriginPosition(base.owner.agentPosition);
		if (obj.victim.IsAnyBoss())
		{
			damageData.SetAttr(DamageAttribute.IsCrit);
			damageData.ApplyAmplification(GetValue(bossAmp));
			healData.SetCrit();
			healData.ApplyAmplification(GetValue(bossAmp));
		}
		float num = Mathf.Clamp01((1f - base.owner.currentHealth / base.owner.maxHealth) / (1f - maxAmpHpThreshold));
		if (num > 0.35f)
		{
			damageData.SetAttr(DamageAttribute.IsCrit);
			healData.SetCrit();
		}
		damageData.ApplyAmplification(maxAmp * num);
		healData.ApplyAmplification(maxAmp * num);
		healData.Dispatch(base.owner, obj.chain.New(this));
		damageData.Dispatch(obj.victim, obj.chain.New(this));
	}

	private void MirrorProcessed()
	{
	}
}
