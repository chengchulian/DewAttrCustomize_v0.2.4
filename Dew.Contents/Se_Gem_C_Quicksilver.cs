public class Se_Gem_C_Quicksilver : StatusEffect
{
	public ScalingValue speedAmount;

	public bool isDecay;

	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoSpeed(GetValue(speedAmount)).decay = isDecay;
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
