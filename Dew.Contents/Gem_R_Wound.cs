public class Gem_R_Wound : Gem
{
	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (base.owner == null || info.actor == null || info.chain.DidReact(this) || info.victim.IsNullInactiveDeadOrKnockedOut() || !base.owner.CheckEnemyOrNeutral(info.victim))
		{
			return;
		}
		Se_Gem_R_Wound_Wounded se_Gem_R_Wound_Wounded = info.victim.Status.FindStatusEffect((Se_Gem_R_Wound_Wounded w) => w.info.caster == base.owner);
		if (se_Gem_R_Wound_Wounded != null)
		{
			se_Gem_R_Wound_Wounded.ResetTimer();
			return;
		}
		CreateStatusEffectWithSource(info.actor, info.victim, new CastInfo(base.owner), delegate(Se_Gem_R_Wound_Wounded w)
		{
			w.chain = info.chain.New(this);
		});
	}

	private void MirrorProcessed()
	{
	}
}
