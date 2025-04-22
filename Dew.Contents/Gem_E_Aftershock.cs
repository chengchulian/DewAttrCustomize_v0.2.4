using UnityEngine;

public class Gem_E_Aftershock : Gem
{
	public GameObject fxBeforeActivate;

	public float delay;

	private int _version;

	protected override void OnCastComplete(EventInfoCast info)
	{
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
	}

	private void MirrorProcessed()
	{
	}
}
