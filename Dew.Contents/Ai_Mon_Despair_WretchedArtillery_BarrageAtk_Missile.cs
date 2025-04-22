using System;
using UnityEngine;

public class Ai_Mon_Despair_WretchedArtillery_BarrageAtk_Missile : StandardProjectile
{
	public float startHeight;

	public DewCollider range;

	public ScalingValue damage;

	public GameObject telegraph;

	public GameObject hitEffect;

	public GameObject explodeEffect;

	public bool spawnAoE;

	[NonSerialized]
	public bool isFirstMissile;

	protected override void OnPrepare()
	{
	}

	protected override void OnCreate()
	{
	}

	protected override void OnComplete()
	{
	}

	private void MirrorProcessed()
	{
	}
}
