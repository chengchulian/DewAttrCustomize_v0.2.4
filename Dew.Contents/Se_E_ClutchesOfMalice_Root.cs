using UnityEngine;

public class Se_E_ClutchesOfMalice_Root : StatusEffect
{
	[HideInInspector]
	public float duration;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoRoot();
			SetTimer(duration);
		}
	}

	private void MirrorProcessed()
	{
	}
}
