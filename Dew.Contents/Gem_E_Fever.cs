public class Gem_E_Fever : Gem
{
	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (base.owner.CheckEnemyOrNeutral(info.victim) && info.damage.elemental == ElementalType.Fire && !info.chain.DidReact(this) && !info.victim.Status.FindStatusEffect((Se_Gem_E_Fever_LivingBomb b) => b.info.caster == base.owner))
		{
			CreateStatusEffect(info.victim, new CastInfo(base.owner), delegate(Se_Gem_E_Fever_LivingBomb se)
			{
				se.chain = info.chain.New(this);
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
