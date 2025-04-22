using UnityEngine;

public class Gem_E_Overload : Gem
{
	public ScalingValue amp;

	public bool useCurrentHealth;

	public float healthRatio;

	public GameObject activationEffect;

	protected override void OnCastCompleteBeforePrepare(EventInfoCast info)
	{
		base.OnCastCompleteBeforePrepare(info);
		if (!(base.owner == null) && info.trigger.configs[info.configIndex].canConsumeCastBonus)
		{
			float amount;
			if (useCurrentHealth)
			{
				amount = base.owner.currentHealth * healthRatio;
			}
			else
			{
				amount = base.owner.maxHealth * healthRatio;
			}
			CreateStatusEffect(base.owner, new CastInfo(base.owner), delegate(Se_HealthCost c)
			{
				c.totalAmount = amount;
			});
			info.instance.dealtDamageProcessor.Add(AmpDamage);
			info.instance.dealtHealProcessor.Add(AmpHeal);
			FxPlayNewNetworked(activationEffect, base.owner);
			NotifyUse();
		}
	}

	private void AmpHeal(ref HealData data, Actor actor, Entity target)
	{
		if (base.isValid && !data.IsAmountModifiedBy(this) && !(base.owner == null))
		{
			data.ApplyAmplification(GetValue(amp));
			data.SetCrit();
			data.SetAmountModifiedBy(this);
		}
	}

	private void AmpDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (base.isValid && !data.IsAmountModifiedBy(this) && !(base.owner == null) && base.owner.CheckEnemyOrNeutral(target))
		{
			data.ApplyAmplification(GetValue(amp));
			data.SetAttr(DamageAttribute.IsCrit);
			data.SetAmountModifiedBy(this);
		}
	}

	private void MirrorProcessed()
	{
	}
}
