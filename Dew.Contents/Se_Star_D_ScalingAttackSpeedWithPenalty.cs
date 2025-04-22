public class Se_Star_D_ScalingAttackSpeedWithPenalty : StarEffect
{
	public float atkSpdPercentageBasePenalty;

	public float[] atkSpdPercentagePerLevel;

	private StatBonus _bonus;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientHeroEventOnLevelChanged(EventInfoHeroLevelUp obj)
	{
	}

	private void UpdateStatBonus()
	{
	}

	private void MirrorProcessed()
	{
	}
}
