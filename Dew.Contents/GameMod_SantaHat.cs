using UnityEngine;

public class GameMod_SantaHat : GameModifierBase
{
	public override bool IsAvailableInGame()
	{
		return Dew.IsNearChristmas();
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			if (Random.value < 0.1f)
			{
				CreateStatusEffect<Se_SantaHat>(humanPlayer.hero, default(CastInfo));
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
