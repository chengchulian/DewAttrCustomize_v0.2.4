using UnityEngine;

public class Mon_DarkCave_CaveSpider : Mon_Forest_SpiderWarrior
{
	public GameObject fxRockObject;

	public GameObject fxArmorBroken;

	public GameObject fxHit;

	public float fxHitPlayInterval;

	public float armorAmount;

	public float armorBreakHealthThreshold;

	private StatBonus _armorBonus;

	private float _lastHitTime;

	private bool _enablePlayFxHit;

	protected override void OnCreate()
	{
	}

	private void OnTakeDamage(EventInfoDamage obj)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
