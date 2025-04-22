using UnityEngine;

public class Se_R_BaptismOfSun_Buff : StatusEffect
{
	[HideInInspector]
	public int hitCount;

	[HideInInspector]
	public float duration;

	public ScalingValue gainApPerHit;

	public ScalingValue atkDamage;

	public float procCoefficient;

	public float hitDelay;

	public GameObject fxHit;

	protected override void OnCreate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
