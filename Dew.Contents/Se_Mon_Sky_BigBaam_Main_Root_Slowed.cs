public class Se_Mon_Sky_BigBaam_Main_Root_Slowed : StatusEffect
{
	public float duration;

	public float slowAmount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoSlow(slowAmount);
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
