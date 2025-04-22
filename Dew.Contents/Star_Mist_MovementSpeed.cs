using System;

public class Star_Mist_MovementSpeed : DewHeroStarItemOld
{
	public static readonly float[] BonusMovementSpeedPercentage = new float[3] { 5f, 7f, 9f };

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
			movementSpeedPercentage = BonusMovementSpeedPercentage.GetClamped(base.level - 1)
		});
	}
}
