public class Se_Q_EtherealInfluence_Root : StatusEffect
{
	public float rootDuration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoRoot();
			SetTimer(rootDuration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
