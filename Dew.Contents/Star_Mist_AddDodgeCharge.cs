using System;

public class Star_Mist_AddDodgeCharge : DewHeroStarItemOld
{
	public static readonly int AddedMaxCharges = 1;

	public override Type heroType => typeof(Hero_Mist);

	public override Type affectedSkill => typeof(St_M_FastFeet);

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
		base.hero.Skill.Movement.configs[0].maxCharges += AddedMaxCharges;
	}
}
