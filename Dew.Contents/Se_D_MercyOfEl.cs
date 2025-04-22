using System;
using UnityEngine;

public class Se_D_MercyOfEl : StackedStatusEffect
{
	public ScalingValue hastePerStack;

	public ScalingValue adBonus;

	public GameObject fxFullyCharged;

	public GameObject fxChargeChanged;

	public ScalingValue lostHpHealRatio;

	private HasteEffect _haste;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.Visual.genericStackIndicatorMax = maxStack;
			base.victim.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
			DoStatBonus(new StatBonus
			{
				attackDamageFlat = GetValue(adBonus)
			});
			_haste = DoHaste(0f);
		}
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
		if (base.stack == 0 || base.stack == maxStack || global::UnityEngine.Random.value < obj.strength)
		{
			AddStack();
		}
		if (base.stack >= maxStack)
		{
			float value = GetValue(lostHpHealRatio);
			DoHeal(new HealData(Mathf.Max(value * 100f, base.victim.Status.missingHealth * value * obj.strength)), base.victim);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(fxFullyCharged);
			if (base.victim != null)
			{
				base.victim.Visual.genericStackIndicatorMax = 0;
				base.victim.Visual.genericStackIndicatorValue = 0;
				base.victim.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
			}
		}
	}

	protected override void OnStackChange(int oldStack, int newStack)
	{
		base.OnStackChange(oldStack, newStack);
		if (base.isServer && base.isActive)
		{
			FxPlayNetworked(fxChargeChanged, base.victim);
			if (base.stack == maxStack)
			{
				FxPlayNetworked(fxFullyCharged, base.victim);
			}
			else
			{
				FxStopNetworked(fxFullyCharged);
			}
			base.victim.Visual.genericStackIndicatorValue = base.stack;
			_haste.strength = GetValue(hastePerStack) * (float)base.stack;
		}
	}

	private void MirrorProcessed()
	{
	}
}
