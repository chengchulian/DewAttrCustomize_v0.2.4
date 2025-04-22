using System;

public class Star_Vesper_MaxHealth : DewHeroStarItemOld
{
	public static readonly int MaxHealthBonus = 80;

	public override Type heroType => typeof(Hero_Vesper);

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
			maxHealthFlat = MaxHealthBonus
		});
	}
}
