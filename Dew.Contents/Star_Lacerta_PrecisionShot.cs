using System;

public class Star_Lacerta_PrecisionShot : DewHeroStarItemOld
{
	public static readonly float BonusChargeSpeed = 0.4f;

	public override Type heroType => typeof(Hero_Lacerta);

	public override Type affectedSkill => typeof(St_R_PrecisionShot);

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
		base.hero.ActorEvent_OnAbilityInstanceBeforePrepare += new Action<EventInfoAbilityInstance>(ActorEventOnAbilityInstanceBeforePrepare);
	}

	private void ActorEventOnAbilityInstanceBeforePrepare(EventInfoAbilityInstance obj)
	{
		if (obj.instance is Ai_R_PrecisionShot ai_R_PrecisionShot)
		{
			ai_R_PrecisionShot.channel.chargeFullDuration /= 1f + BonusChargeSpeed;
		}
	}
}
