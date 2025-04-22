public class Star_Global_DodgeAbilityCooldown : DewStarItemOld
{
	public static readonly float[] CooldownMultiplier = new float[4] { 0.93f, 0.88f, 0.82f, 0.75f };

	public override int maxLevel => 4;

	public override bool affectsMovementSkill => true;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		if (!(base.hero.Skill.Movement == null))
		{
			base.hero.Skill.Movement.configs[0].cooldownTime *= CooldownMultiplier.GetClamped(base.level - 1);
		}
	}
}
