public class Star_Global_FatalHitProtection : DewStarItemOld
{
	public static readonly float InvulnerableTime = 3.5f;

	public override int maxLevel => 1;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.CreateStatusEffect(base.hero, new CastInfo(base.hero), delegate(Se_FatalHitProtection_Interrupt se)
		{
			se.SetStack(1);
			se.invulTime = InvulnerableTime;
		});
	}
}
