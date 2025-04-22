using System;

public class Star_Yubar_AbilityPower : DewHeroStarItemOld
{
	public static readonly int[] AbilityPowerBonus = new int[3] { 5, 10, 15 };

	public override int maxLevel => 3;

	public override Type heroType => typeof(Hero_Yubar);

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
			abilityPowerFlat = AbilityPowerBonus.GetClamped(base.level - 1)
		});
	}
}
