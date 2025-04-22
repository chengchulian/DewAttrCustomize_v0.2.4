using UnityEngine;

public class Gem_R_Rejuvenation : Gem
{
	public GameObject healEffect;

	public ScalingValue conversionRatio;

	protected override void OnCastCompleteBeforePrepare(EventInfoCast info)
	{
	}

	private void MirrorProcessed()
	{
	}
}
