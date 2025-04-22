using UnityEngine;

public class Se_C_CorrosiveTrails_Poisoned : StatusEffect
{
	public ScalingValue perTickDamage;

	public float tickInterval;

	public float poisonDuration;

	public GameObject fxPerTickEffect;

	private float _lastTickTime;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	private void MirrorProcessed()
	{
	}
}
