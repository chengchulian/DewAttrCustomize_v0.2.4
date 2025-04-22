public class Hero_Vesper : Hero
{
	public override void OnStartClient()
	{
		base.OnStartClient();
		GetComponentInChildren<Vesper_LobbyHammer>().lobbyHammer.SetActive(value: false);
	}

	private void MirrorProcessed()
	{
	}
}
