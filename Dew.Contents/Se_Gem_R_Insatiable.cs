using UnityEngine;

public class Se_Gem_R_Insatiable : StatusEffect
{
	public ScalingValue healAmount;

	public GameObject healEffect;

	public GameObject hitEffect;

	public float duration;

	public ScalingValue bonusAttackDamagePercentage;

	private StatBonus _bonus;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_bonus = new StatBonus
			{
				attackDamagePercentage = GetValue(bonusAttackDamagePercentage)
			};
			base.victim.Status.AddStatBonus(_bonus);
			SetTimer(duration);
			DoAttackEmpower(delegate(EventInfoAttackEffect effect, int i)
			{
				FxPlayNewNetworked(healEffect, base.victim);
				FxPlayNewNetworked(hitEffect, effect.victim);
				DoHeal(new HealData(GetValue(healAmount) * effect.strength), base.victim, effect.chain.New(this));
			});
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null))
		{
			base.victim.Status.RemoveStatBonus(_bonus);
		}
	}

	private void MirrorProcessed()
	{
	}
}
