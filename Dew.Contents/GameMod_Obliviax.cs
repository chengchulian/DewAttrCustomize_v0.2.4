using System;
using System.Collections;
using UnityEngine;

public class GameMod_Obliviax : GameModifierBase
{
	private int _lastObliviaxQuestZoneIndex = -100;

	[NonSerialized]
	public int immunityTurns;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && NetworkedManagerBase<ZoneManager>.instance != null)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded -= new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom _)
	{
		if (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex == _lastObliviaxQuestZoneIndex || NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex == _lastObliviaxQuestZoneIndex + 1 || !NetworkedManagerBase<ZoneManager>.instance.isCurrentNodeHunted)
		{
			return;
		}
		if (immunityTurns > 0)
		{
			immunityTurns--;
		}
		else if (NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel >= 5)
		{
			GameManager.CallOnReady(delegate
			{
				StartCoroutine(Routine());
			});
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(2f);
			NetworkedManagerBase<QuestManager>.instance.StartQuest<Quest_HuntedByObliviax>();
			_lastObliviaxQuestZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
		}
	}

	private void MirrorProcessed()
	{
	}
}
