using System;

public class Star_Yubar_IncreaseFlickerRange : DewHeroStarItemOld
{
	public static readonly float RangeMultiplier = 1.35f;

	public override Type heroType => typeof(Hero_Yubar);

	public override Type affectedSkill => typeof(St_M_Flicker);

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
		base.hero.Skill.Movement.configs[0].castMethod._range *= RangeMultiplier;
		base.hero.Skill.Movement.SyncCastMethodChanges(0);
	}
}
