using UnityEngine;

public class Se_R_Frostbite_EmpowerAttacks : StatusEffect
{
	public GameObject firstHitEffect;

	public GameObject secondHitEffect;

	public float hasteStrength = 50f;

	public float duration = 8f;

	public float shieldDuration = 1.5f;

	public ScalingValue firstDmg;

	public ScalingValue firstShieldAmount;

	public ScalingValue secondDmg;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		if (base.victim.Ability.attackAbility != null)
		{
			base.victim.Ability.attackAbility.ResetCooldown();
		}
		SetTimer(duration);
		ShowOnScreenTimer();
		DoHaste(hasteStrength);
		DoAttackEmpower(delegate(EventInfoAttackEffect effect, int i)
		{
			switch (i)
			{
			case 0:
				if (effect.type != AttackEffectType.BasicAttackSub)
				{
					GiveShield(base.victim, GetValue(firstShieldAmount) * effect.strength, shieldDuration);
				}
				Damage(firstDmg).ApplyStrength(effect.strength).SetElemental(ElementalType.Cold).SetOriginPosition(base.victim.position)
					.Dispatch(effect.victim);
				FxPlayNewNetworked(firstHitEffect, effect.victim);
				break;
			case 1:
				Damage(secondDmg).ApplyStrength(effect.strength).SetElemental(ElementalType.Cold).SetOriginPosition(base.victim.position)
					.SetAttr(DamageAttribute.IsCrit)
					.Dispatch(effect.victim);
				FxPlayNewNetworked(secondHitEffect, effect.victim);
				break;
			}
		}, 2, base.DestroyIfActive);
	}

	private void MirrorProcessed()
	{
	}
}
