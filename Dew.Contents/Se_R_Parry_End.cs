public class Se_R_Parry_End : StatusEffect
{
	public float duration;

	private float _lastEffectTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			SetTimer(duration);
			DoInvulnerable();
		}
	}

	private void MirrorProcessed()
	{
	}
}
