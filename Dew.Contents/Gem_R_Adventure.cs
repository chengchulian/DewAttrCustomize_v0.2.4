using UnityEngine;

public class Gem_R_Adventure : Gem
{
	public GameObject fxReward;

	public ScalingValue upgradeGemAmount;

	public ScalingValue goldAmount;

	public override void OnEquipGem(Hero newOwner)
	{
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	private void MirrorProcessed()
	{
	}
}
