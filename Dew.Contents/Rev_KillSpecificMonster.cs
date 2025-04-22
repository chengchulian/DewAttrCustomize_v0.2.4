using System.Collections.Generic;
using UnityEngine;

public class Rev_KillSpecificMonster : DewReverieItem
{
	[AchPersistentVar]
	private string _monsterType;

	[AchPersistentVar]
	private int _killCount;

	public override int grantedStardust => 35;

	public string monsterName => DewLocalization.GetUIValue(_monsterType + "_Name");

	public override int GetCurrentProgress()
	{
		return _killCount;
	}

	public override int GetMaxProgress()
	{
		return 60;
	}

	public override void OnSetupReverie()
	{
		base.OnSetupReverie();
		List<string> list = new List<string> { "Mon_Forest_Hound", "Mon_Forest_SpiderWarrior", "Mon_Forest_SpiderSpitter", "Mon_SnowMountain_Scavenger", "Mon_SnowMountain_IceElemental", "Mon_SnowMountain_SnowWolf", "Mon_Sky_Baam", "Mon_Sky_StarSeed" };
		_monsterType = list[Random.Range(0, list.Count)];
	}

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		AchOnKillOrAssist(delegate(EventInfoKill k)
		{
			if (!(k.victim.GetType().Name != _monsterType))
			{
				_killCount++;
				if (_killCount >= 60)
				{
					Complete();
				}
			}
		});
	}
}
