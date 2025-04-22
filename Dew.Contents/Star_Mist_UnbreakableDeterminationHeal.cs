using System;

public class Star_Mist_UnbreakableDeterminationHeal : DewHeroStarItemOld
{
	public static readonly float LostHealthRatio = 0.25f;

	public override Type heroType => typeof(Hero_Mist);

	public override Type affectedSkill => typeof(St_R_UnbreakableDetermination);

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
		base.hero.ActorEvent_OnAbilityInstanceCreated += new Action<EventInfoAbilityInstance>(CreateHealEffect);
	}

	private void CreateHealEffect(EventInfoAbilityInstance obj)
	{
		if (obj.instance is Se_R_UnbreakableDetermination)
		{
			obj.instance.CreateStatusEffect(base.hero, new CastInfo(base.hero, base.hero), delegate(Se_GenericHealOverTime heal)
			{
				heal.tickInterval = 0.1f;
				heal.ticks = 6;
				heal.totalAmount = LostHealthRatio * base.hero.Status.missingHealth;
			});
		}
	}
}
