public class Rev_PlayCoopTwice : DewReverieItem
{
	[AchPersistentVar]
	private int _playCount;

	public override int grantedStardust => 30;

	public override int GetCurrentProgress()
	{
		return _playCount;
	}

	public override int GetMaxProgress()
	{
		return 2;
	}

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		if (DewPlayer.humanPlayers.Count > 1)
		{
			_playCount++;
			if (_playCount >= 2)
			{
				Complete();
			}
		}
	}
}
