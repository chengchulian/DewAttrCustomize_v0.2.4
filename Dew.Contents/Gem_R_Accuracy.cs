using UnityEngine;

public class Gem_R_Accuracy : Gem
{
	public GameObject fxRange;

	public float radius;

	public override void OnEquipGem(Hero newOwner)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
	}

	private void MirrorProcessed()
	{
	}
}
