using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_NeedleAtk_AtkInstance : AbilityInstance
{
	public ScalingValue dmgFactor;

	public Knockback Knockback;

	public float delay;

	public float stunDuration;

	public GameObject fxHit;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		base.info.caster.Control.StartDaze(delay);
		yield return new SI.WaitForSeconds(delay);
		if (NetworkedManagerBase<ActorManager>.instance == null)
		{
			yield break;
		}
		foreach (Hero allHero in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			if (!allHero.IsNullInactiveDeadOrKnockedOut() && tvDefaultHarmfulEffectTargets.Evaluate(allHero) && !allHero.Status.hasUncollidable && !allHero.Status.hasInvisible)
			{
				FxPlayNewNetworked(fxHit, allHero);
				float value = GetValue(dmgFactor);
				CreateDamage(DamageData.SourceType.Default, Mathf.Min(value, allHero.currentHealth * 0.5f)).SetOriginPosition(base.info.caster.agentPosition).Dispatch(allHero);
				Knockback.ApplyWithOrigin(base.info.caster.agentPosition, allHero);
				CreateBasicEffect(allHero, new StunEffect(), stunDuration, "needleatk_stun");
			}
		}
		base.info.caster.AI.Aggro(Dew.SelectRandomAliveHero());
		yield return null;
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
