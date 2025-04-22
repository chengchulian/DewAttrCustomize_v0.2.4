public class Se_Gem_C_Wind : StatusEffect
{
	public ScalingValue hasteAmount;

	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoHaste(GetValue(hasteAmount));
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
