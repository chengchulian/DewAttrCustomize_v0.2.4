using UnityEngine;

public class Gem_L_ChaosApple : Gem
{
	public ScalingValue castPerRoomCount;

	public Color skillOverlayColor;

	public GameObject fxActivate;

	public override void OnEquipGem(Hero newOwner)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
