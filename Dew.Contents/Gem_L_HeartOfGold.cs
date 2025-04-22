using UnityEngine;

public class Gem_L_HeartOfGold : Gem
{
	public ScalingValue ampPerHundredGold;

	public ScalingValue spentGoldRatio;

	public GameObject castEffect;

	public GameObject hitEffect;

	public float currentAmp
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected override void OnCastCompleteBeforePrepare(EventInfoCast info)
	{
	}

	private void MirrorProcessed()
	{
	}
}
