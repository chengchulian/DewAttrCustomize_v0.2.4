public class Se_Q_SuperNova_Slow : StatusEffect
{
	public ScalingValue slowAmount;

	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoSlow(GetValue(slowAmount));
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
