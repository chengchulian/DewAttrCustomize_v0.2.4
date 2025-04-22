using UnityEngine;

public class Se_Gem_Regeneration : StatusEffect
{
	public ScalingValue healPerTick;

	public int totalTicks;

	public float tickInterval;

	private int _remainingTicks;

	private float _lastHealTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_remainingTicks = totalTicks;
			_lastHealTime = Time.time;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			if (_remainingTicks <= 0)
			{
				Destroy();
			}
			else if (Time.time - _lastHealTime > tickInterval)
			{
				_lastHealTime = Time.time;
				_remainingTicks--;
				HealData heal = new HealData(GetValue(healPerTick));
				heal.SetCanMerge();
				DoHeal(heal, base.victim, chain);
			}
		}
	}

	public void ResetTicks()
	{
		_remainingTicks = totalTicks;
	}

	private void MirrorProcessed()
	{
	}
}
