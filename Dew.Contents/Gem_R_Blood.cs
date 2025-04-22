using UnityEngine;

public class Gem_R_Blood : Gem
{
	public GameObject fxActivate;

	public float activateHealthThreshold;

	public int sacrificedQuality;

	public float healMaxHpRatio;

	public override void OnEquipGem(Hero newOwner)
	{
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	public override bool IsReady()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
