using UnityEngine;

public class Se_MorasDomain_MorasCreation : StatusEffect
{
	private static readonly int EmissionColor;

	public GameObject fxDeathEffect;

	public GameObject fxTakeDamage;

	public GameObject fxTakeDamageDoT;

	public float takeDamageInterval;

	private float _lastTakeDamage;

	private float _lastTakeDamageDoT;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	protected virtual void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
	}

	private void EntityEventOnDeath(EventInfoKill obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
