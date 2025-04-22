using UnityEngine;

public class Ai_Mon_Sky_StellaMatter_Die : StandardProjectile
{
	public DewCollider range;

	public ScalingValue dmgFactor;

	public GameObject fxGib;

	public GameObject fxHit;

	public GameObject telegraph;

	public GameObject fxExpolosion;

	public float explodeDelay;

	protected override void OnComplete()
	{
	}

	private void MirrorProcessed()
	{
	}
}
