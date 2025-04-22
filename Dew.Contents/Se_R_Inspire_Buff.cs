using UnityEngine;

public class Se_R_Inspire_Buff : StatusEffect
{
	public GameObject fxHit;

	public float duration;

	public ScalingValue haste;

	public ScalingValue damage;

	public float selfMultiplier;

	protected override void OnCreate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
