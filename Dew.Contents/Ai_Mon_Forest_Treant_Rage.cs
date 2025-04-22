public class Ai_Mon_Forest_Treant_Rage : StatusEffect
{
	public float initDuration;

	public ScalingValue moveSpeedAmpAmount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoSpeed(GetValue(moveSpeedAmpAmount));
			SetTimer(initDuration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
