using UnityEngine;

public class Gem_E_Fangs : Gem
{
	public ScalingValue cooldownReductionOnCrit;

	public float searchRange;

	public GameObject fxActivate;

	public override void OnEquipGem(Hero newOwner)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	private void CheckCritical(EventInfoAttackFired obj)
	{
	}

	private void EntityEventOnAttackHit(EventInfoAttackHit obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
