using UnityEngine;

public class Gem_R_Shock : Gem
{
	public GameObject fxActivate;

	public int baseHitCount;

	public float critChanceForAdditionalHit;

	public int currentHitCount
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override void OnEquipGem(Hero newOwner)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	private void CheckCritical(EventInfoAttackFired obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
