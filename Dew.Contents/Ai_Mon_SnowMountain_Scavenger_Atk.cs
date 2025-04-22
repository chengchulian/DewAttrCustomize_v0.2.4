using UnityEngine;

public class Ai_Mon_SnowMountain_Scavenger_Atk : InstantDamageInstance
{
	protected override void OnCreate()
	{
		if (DewAnimationClip.GetEntryIndex(base.info.animSelectValue, 2) == 1)
		{
			Vector3 localScale = startEffectNoStop.transform.localScale;
			localScale.x *= -1f;
			startEffectNoStop.transform.localScale = localScale;
		}
		base.OnCreate();
	}

	private void MirrorProcessed()
	{
	}
}
