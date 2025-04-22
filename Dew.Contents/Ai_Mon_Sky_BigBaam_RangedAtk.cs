using UnityEngine;

public class Ai_Mon_Sky_BigBaam_RangedAtk : StandardProjectile
{
	public GameObject hitEffect;

	public GameObject explodeEffect;

	public ScalingValue damage;

	public DewCollider range;

	protected override void OnComplete()
	{
	}

	private void MirrorProcessed()
	{
	}
}
