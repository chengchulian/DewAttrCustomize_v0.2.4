public class Rev_KillHeroicBosses : DewReverieItem
{
	[AchPersistentVar]
	private int _killCount;

	public override int grantedStardust => 30;

	public override int GetCurrentProgress()
	{
		return _killCount;
	}

	public override int GetMaxProgress()
	{
		return 5;
	}

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		AchOnKillOrAssist(delegate(EventInfoKill k)
		{
			if (k.victim is BossMonster)
			{
				_killCount++;
				if (_killCount >= 5)
				{
					Complete();
				}
			}
		});
	}
}
