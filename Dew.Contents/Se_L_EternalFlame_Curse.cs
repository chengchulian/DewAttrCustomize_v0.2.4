using UnityEngine;

public class Se_L_EternalFlame_Curse : StatusEffect
{
	public ScalingValue duration;

	public GameObject fxAddFireStack;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
