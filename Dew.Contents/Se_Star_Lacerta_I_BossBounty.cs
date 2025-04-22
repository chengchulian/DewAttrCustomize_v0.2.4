using System;

public class Se_Star_Lacerta_I_BossBounty : StarEffect
{
	public int[] bonusGold;

	public override Type heroType
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientHeroEventOnKillOrAssist(EventInfoKill obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
