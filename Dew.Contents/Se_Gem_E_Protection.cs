public class Se_Gem_E_Protection : StatusEffect
{
	public ScalingValue armorDuration;

	public ScalingValue armorAmount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoArmorBoost(GetValue(armorAmount));
			DoUnstoppable();
			SetTimer(GetValue(armorDuration));
		}
	}

	private void MirrorProcessed()
	{
	}
}
