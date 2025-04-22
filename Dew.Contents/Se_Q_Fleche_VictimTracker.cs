using UnityEngine;

public class Se_Q_Fleche_VictimTracker : StatusEffect
{
	public GameObject healEffect;

	public ScalingValue healAmount;

	public float cooldownReduction = 1.5f;

	internal bool _isBeingReplaced;

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !_isBeingReplaced && !base.info.caster.IsNullInactiveDeadOrKnockedOut() && base.victim.IsNullInactiveDeadOrKnockedOut())
		{
			if (GetValue(healAmount) > 0f)
			{
				FxPlayNetworked(healEffect, base.info.caster);
				DoHeal(new HealData(GetValue(healAmount)), base.info.caster);
			}
			if (base.firstTrigger != null)
			{
				base.firstTrigger.ApplyCooldownReductionByRatio(cooldownReduction);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
