using Mirror;

public class Se_Curse_LossOfIdentity : CurseStatusEffect
{
	private SkillTrigger _unequippedIdentity;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	[Server]
	private void Unequip()
	{
	}

	[Server]
	private void Equip()
	{
	}

	private void ClientEventOnRoomLoadStarted(EventInfoLoadRoom obj)
	{
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
