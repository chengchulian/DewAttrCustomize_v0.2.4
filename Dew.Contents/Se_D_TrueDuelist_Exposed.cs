using UnityEngine;

public class Se_D_TrueDuelist_Exposed : StatusEffect
{
	public ScalingValue lostHealthHealRatioOnKill;

	public ScalingValue damageAmp;

	public int healTicks;

	public float tickInterval;

	public float maxDistance;

	public float distCheckInterval;

	public GameObject fxKillOnCaster;

	public GameObject fxKillOnVictim;

	private float _lastDistCheckTime;

	protected override void OnCreate()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void VictimOntakenDamageProcessor(ref DamageData data, Actor actor, Entity target)
	{
	}

	private void EntityEventOnDeath(EventInfoKill obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
