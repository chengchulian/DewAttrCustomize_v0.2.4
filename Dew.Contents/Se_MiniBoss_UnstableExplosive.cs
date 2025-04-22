using UnityEngine;

public class Se_MiniBoss_UnstableExplosive : MiniBossEffect
{
	public float maxHealthBonusPercentage = 25f;

	public float explodeInterval;

	public float startDelay;

	private float _lastExplodeTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoStatBonus(new StatBonus
			{
				maxHealthPercentage = maxHealthBonusPercentage
			});
			_lastExplodeTime = Time.time - explodeInterval + startDelay;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(Time.time - _lastExplodeTime < explodeInterval))
		{
			CreateStatusEffect<Se_MiniBoss_UnstableExplosive_Explosion>(base.victim, new CastInfo(base.victim));
			_lastExplodeTime = Time.time;
		}
	}

	private void MirrorProcessed()
	{
	}
}
