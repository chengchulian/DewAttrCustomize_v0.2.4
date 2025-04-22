using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Quest_HuntedByObliviax : DewQuest
{
	public GameObject fxStart;

	[SyncVar(hook = "OnRemainingTurnsChanged")]
	private int _remainingTurns;

	public int Network_remainingTurns
	{
		get
		{
			return _remainingTurns;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _remainingTurns, 32uL, OnRemainingTurnsChanged);
		}
	}

	private void OnRemainingTurnsChanged(int _, int __)
	{
		UpdateQuestText();
	}

	protected override void OnPrepare()
	{
		base.OnPrepare();
		Network_remainingTurns = NetworkedManagerBase<ZoneManager>.instance.GetNodeDistance(NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, NetworkedManagerBase<ZoneManager>.instance.nodes.FindIndex((WorldNodeData w) => w.type == WorldNodeType.ExitBoss)) + 1;
		Network_remainingTurns = Mathf.Clamp(_remainingTurns, 2, 5);
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		base.questTitleRaw = DewLocalization.GetUIValue("Quest_HuntedByObliviax_Title");
		UpdateQuestText();
		FxPlay(fxStart);
		if (base.isServer)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
		if (obj.isSidetrackTransition)
		{
			return;
		}
		NetworkedManagerBase<ZoneManager>.instance.CallOnReadyAfterTransition(delegate
		{
			Network_remainingTurns = _remainingTurns - 1;
			GameMod_Obliviax gameMod_Obliviax = Dew.FindActorOfType<GameMod_Obliviax>();
			if (NetworkedManagerBase<ZoneManager>.instance.currentNode.type == WorldNodeType.ExitBoss)
			{
				if (gameMod_Obliviax != null)
				{
					gameMod_Obliviax.immunityTurns = global::UnityEngine.Random.Range(2, 4);
				}
				CompleteQuest();
			}
			else if (_remainingTurns <= 0)
			{
				if (gameMod_Obliviax != null)
				{
					gameMod_Obliviax.immunityTurns = int.MaxValue;
				}
				FailQuest(QuestFailReason.NotSpecified);
				Dew.CreateActor<Actor_Obliviax_InterruptTravelAndKidnap>();
			}
		});
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && NetworkedManagerBase<ZoneManager>.instance != null)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded -= new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	private void UpdateQuestText()
	{
		if (NetworkedManagerBase<ZoneManager>.instance.currentNode.type != WorldNodeType.ExitBoss)
		{
			if (_remainingTurns <= 0)
			{
				base.questShortDescriptionRaw = DewLocalization.GetUIValue("Quest_HuntedByObliviax_Description_SheIsComingForYou");
				base.questDetailedDescriptionRaw = DewLocalization.GetUIValue("Quest_HuntedByObliviax_Description_SheIsComingForYou");
			}
			else
			{
				base.questShortDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_HuntedByObliviax_Description"), _remainingTurns);
				base.questDetailedDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_HuntedByObliviax_Tooltip"), _remainingTurns);
			}
		}
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(_remainingTurns);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteInt(_remainingTurns);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _remainingTurns, OnRemainingTurnsChanged, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _remainingTurns, OnRemainingTurnsChanged, reader.ReadInt());
		}
	}
}
