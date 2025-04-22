public class Se_Q_CruelSun_Armored : StatusEffect
{
	public ScalingValue armorAmount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoUnstoppable();
			DoArmorBoost(GetValue(armorAmount));
		}
	}

	private void MirrorProcessed()
	{
	}
}
