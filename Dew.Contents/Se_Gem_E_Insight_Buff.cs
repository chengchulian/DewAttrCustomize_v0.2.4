public class Se_Gem_E_Insight_Buff : StatusEffect
{
	public ScalingValue gainedAbilityHaste;

	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoStatBonus(new StatBonus
			{
				abilityHasteFlat = GetValue(gainedAbilityHaste)
			});
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
