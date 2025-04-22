public class Se_D_Resolve : StatusEffect
{
	public ScalingValue healthBonus;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoStatBonus(new StatBonus
			{
				maxHealthFlat = GetValue(healthBonus)
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
