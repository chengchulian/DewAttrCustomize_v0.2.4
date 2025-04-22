public class Ai_Mon_Special_BossObliviax_TurnAtk_Projectile : StandardProjectile
{
	protected override void OnComplete()
	{
		base.OnComplete();
		CreateAbilityInstance<Ai_Mon_Special_BossObliviax_TurnAtk_DelayedAtk>(base.info.point, null, base.info);
	}

	private void MirrorProcessed()
	{
	}
}
