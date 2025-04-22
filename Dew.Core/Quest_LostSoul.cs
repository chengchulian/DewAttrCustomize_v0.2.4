using System;
using System.Runtime.InteropServices;
using Mirror;

public class Quest_LostSoul : DewQuest
{
	[NonSerialized]
	[SyncVar]
	public Hero targetHero;

	protected NetworkBehaviourSyncVar ___targetHeroNetId;

	public Hero NetworktargetHero
	{
		get
		{
			return GetSyncVarNetworkBehaviour(___targetHeroNetId, ref targetHero);
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter_NetworkBehaviour(value, ref targetHero, 32uL, null, ref ___targetHeroNetId);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		UpdateQuestText();
		NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		if (base.isServer)
		{
			if (NetworktargetHero.IsNullOrInactive())
			{
				RemoveQuest();
				return;
			}
			if (!NetworktargetHero.isKnockedOut)
			{
				CompleteQuest();
				return;
			}
			NetworktargetHero.ClientActorEvent_OnDestroyed += new Action<Actor>(ClientActorEventOnDestroyed);
			NetworktargetHero.ClientHeroEvent_OnRevive += new Action<Hero>(ClientHeroEventOnRevive);
			SetNextGoal_ReachNode(new NextGoalSettings
			{
				nodeIndexSettings = new GetNodeIndexSettings
				{
					desiredDistance = NetworkedManagerBase<GameManager>.instance.difficulty.lostSoulDistance
				},
				modifierData = NetworktargetHero.netId.ToString(),
				addedModifier = "RoomMod_HeroSoul",
				revertModifierOnRemove = true,
				failQuestOnFail = true,
				dontChangeDescription = true,
				dontChangeTitle = true,
				ignoreSuboptimalSituation = true
			});
		}
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
		NetworkedManagerBase<ZoneManager>.instance.CallOnReadyAfterTransition(UpdateQuestText);
	}

	private void ClientActorEventOnDestroyed(Actor obj)
	{
		if (base.isActive)
		{
			RemoveQuest();
		}
	}

	private void ClientHeroEventOnRevive(Hero obj)
	{
		if (base.isActive)
		{
			CompleteQuest();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (NetworkedManagerBase<ZoneManager>.instance != null)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded -= new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
		if (base.isServer && NetworktargetHero != null)
		{
			NetworktargetHero.ClientActorEvent_OnDestroyed -= new Action<Actor>(ClientActorEventOnDestroyed);
			NetworktargetHero.ClientHeroEvent_OnRevive -= new Action<Hero>(ClientHeroEventOnRevive);
		}
	}

	private void UpdateQuestText()
	{
		bool num = NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers.FindIndex((ModifierData m) => m.type == "RoomMod_HeroSoul" && m.clientData == NetworktargetHero.netId.ToString()) != -1;
		string descName = ChatManager.GetDescribedPlayerName(NetworktargetHero.owner);
		base.questTitleRaw = string.Format(DewLocalization.GetUIValue("Quest_LostSoul_Title"), descName);
		if (num)
		{
			base.questShortDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_LostSoul_Description_FindAndSave"), descName);
			base.questDetailedDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_LostSoul_Tooltip_FindAndSave"), descName);
		}
		else
		{
			base.questShortDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_LostSoul_Description_Move"), descName);
			base.questDetailedDescriptionRaw = string.Format(DewLocalization.GetUIValue("Quest_LostSoul_Tooltip_Move"), descName);
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
			writer.WriteNetworkBehaviour(NetworktargetHero);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteNetworkBehaviour(NetworktargetHero);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref targetHero, null, reader, ref ___targetHeroNetId);
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref targetHero, null, reader, ref ___targetHeroNetId);
		}
	}
}
