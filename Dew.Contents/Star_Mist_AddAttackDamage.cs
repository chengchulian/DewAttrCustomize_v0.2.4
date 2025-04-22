using System;

public class Star_Mist_AddAttackDamage : DewHeroStarItemOld
{
	public static readonly int[] BonusAttackDamage = new int[3] { 3, 6, 12 };

	public override int maxLevel => 3;

	public override Type heroType => typeof(Hero_Mist);

	public override bool ShouldInitInGame()
	{
		if (base.ShouldInitInGame())
		{
			return base.isServer;
		}
		return false;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.Status.AddStatBonus(new StatBonus
		{
			attackDamageFlat = BonusAttackDamage.GetClamped(base.level - 1)
		});
	}
}
