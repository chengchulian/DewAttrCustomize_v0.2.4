using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Shrine_Hatred : Shrine
{
	public float[] hatredStrengthChances;

	public int choiceCount = 3;

	public int bonusSkillLevel = 1;

	public int bonusEssenceQuality = 50;

	public float goldWeight = 1f;

	public float dreamDustWeight = 1f;

	public float skillWeight = 1f;

	public float gemWeight = 1f;

	public float chaosWeight = 1f;

	[Space(16f)]
	public float[] goldMultipliers;

	public float[] dreamDustMultipliers;

	public float currencyDeviation = 0.15f;

	public readonly SyncList<HatredStrengthType> choices = new SyncList<HatredStrengthType>();

	private readonly SyncList<uint> _usedPlayers = new SyncList<uint>();

	private CurseStatusEffect[] _curseStatusEffects;

	private int GetIndexOfStrength(HatredStrengthType strength)
	{
		return strength switch
		{
			HatredStrengthType.None => 0, 
			HatredStrengthType.Mild => 1, 
			HatredStrengthType.Potent => 2, 
			HatredStrengthType.Powerful => 3, 
			_ => throw new ArgumentOutOfRangeException("strength", strength, null), 
		};
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		_curseStatusEffects = (from c in DewResources.FindAllByTypeSubstring<CurseStatusEffect>("Se_Curse_")
			where Dew.IsCurseIncludedInGame(c.GetType().Name)
			select c).ToArray();
		choices.Add(HatredStrengthType.None);
		choices.Add(HatredStrengthType.Mild);
		choices.Add(HatredStrengthType.Potent);
		choices.Add(HatredStrengthType.Powerful);
		while (choices.Count > choiceCount)
		{
			HatredStrengthType item = Dew.SelectRandomWeightedInList(choices, (HatredStrengthType t) => 1f / hatredStrengthChances[GetIndexOfStrength(t)]);
			choices.Remove(item);
		}
	}

	protected override bool OnUse(Entity entity)
	{
		TpcOpenWindow(entity.owner.connectionToClient);
		return false;
	}

	public override bool CanInteract(Entity entity)
	{
		if (base.CanInteract(entity) && entity.owner != null)
		{
			return !_usedPlayers.Contains(entity.owner.netId);
		}
		return false;
	}

	[TargetRpc]
	private void TpcOpenWindow(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void Shrine_Hatred::TpcOpenWindow(Mirror.NetworkConnectionToClient)", -1077927823, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Command(requiresAuthority = false)]
	public void CmdChoose(int index, NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(index);
		SendCommandInternal("System.Void Shrine_Hatred::CmdChoose(System.Int32,Mirror.NetworkConnectionToClient)", 1341025742, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	private void UpdateAvailability()
	{
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			if (!_usedPlayers.Contains(humanPlayer.netId) && !humanPlayer.hero.IsNullInactiveDeadOrKnockedOut())
			{
				return;
			}
		}
		base.currentUseCount = maxUseCount;
	}

	private void DoCurse(HatredStrengthType strength, Hero hero)
	{
		if (strength == HatredStrengthType.None)
		{
			return;
		}
		CurseStatusEffect curseStatusEffect = Dew.SelectRandomWeightedInList((IList<CurseStatusEffect>)_curseStatusEffects, (Func<CurseStatusEffect, float>)delegate(CurseStatusEffect effect)
		{
			if (!effect.availableStrengths.HasFlag(strength))
			{
				return 0f;
			}
			return (!effect.IsViable(hero)) ? 0f : effect.chanceWeight;
		});
		if (curseStatusEffect == null)
		{
			return;
		}
		hero.CreateStatusEffect(curseStatusEffect, hero, new CastInfo(hero, hero), delegate(CurseStatusEffect curse)
		{
			curse.currentStrength = strength;
			if (global::UnityEngine.Random.value < 0.5f)
			{
				curse.progressType = QuestProgressType.Kills;
				switch (strength)
				{
				case HatredStrengthType.Mild:
					curse.requiredAmount = 23 + NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex * 2;
					break;
				case HatredStrengthType.Potent:
					curse.requiredAmount = 27 + NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex * 3;
					break;
				case HatredStrengthType.Powerful:
					curse.requiredAmount = 42 + NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex * 3;
					break;
				default:
					throw new ArgumentOutOfRangeException("strength", strength, null);
				}
			}
			else
			{
				curse.progressType = QuestProgressType.Travel;
				switch (strength)
				{
				case HatredStrengthType.Mild:
					curse.requiredAmount = 3;
					break;
				case HatredStrengthType.Potent:
					curse.requiredAmount = 4;
					break;
				case HatredStrengthType.Powerful:
					curse.requiredAmount = 4;
					break;
				default:
					throw new ArgumentOutOfRangeException("strength", strength, null);
				}
			}
		});
	}

	public static float GetBaseRewardAmount_DreamDust()
	{
		int currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
		Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
		lootInstance.gemQualityMinByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		float f = lootInstance.gemQualityMaxByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		return (float)NetworkedManagerBase<GameManager>.instance.GetGemUpgradeDreamDustCost(Mathf.RoundToInt(f)) * (1f + (float)NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex * 0.1f) * (1f + (float)currentZoneIndex * 0.125f);
	}

	public static float GetBaseRewardAmount_Gold()
	{
		int currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
		Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
		float num = lootInstance.gemQualityMinByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		float num2 = lootInstance.gemQualityMaxByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		return (float)Gem.GetBuyGold(null, Rarity.Rare, Mathf.RoundToInt((num + num2) / 2f)) * (1f + (float)currentZoneIndex * 0.075f);
	}

	private void DoReward(HatredStrengthType type, Hero hero)
	{
		Vector3 pos = Dew.GetGoodRewardPosition(base.position + (hero.agentPosition - base.position).normalized * 2.5f);
		if (type == HatredStrengthType.None)
		{
			switch (Dew.SelectRandomWeightedIndexInParams(goldWeight, dreamDustWeight, skillWeight, gemWeight))
			{
			case 0:
				DropGoldReward();
				break;
			case 1:
				DropDreamDustReward();
				break;
			case 2:
				DropSkillReward(isHigh: false, 0, new Rarity[1]);
				break;
			case 3:
				DropGemReward(isHigh: false, 0, new Rarity[1]);
				break;
			}
		}
		else if (type == HatredStrengthType.Mild)
		{
			switch (Dew.SelectRandomWeightedIndexInParams(goldWeight, dreamDustWeight, skillWeight, gemWeight, chaosWeight))
			{
			case 0:
				DropGoldReward();
				break;
			case 1:
				DropDreamDustReward();
				break;
			case 2:
				DropSkillReward(isHigh: false, bonusSkillLevel, new Rarity[2]
				{
					Rarity.Common,
					Rarity.Rare
				});
				break;
			case 3:
				DropGemReward(isHigh: false, bonusEssenceQuality, new Rarity[2]
				{
					Rarity.Common,
					Rarity.Rare
				});
				break;
			case 4:
				DropChaosReward(isHigh: false, new Rarity[2]
				{
					Rarity.Common,
					Rarity.Rare
				});
				break;
			}
		}
		else if (type == HatredStrengthType.Potent)
		{
			switch (Dew.SelectRandomWeightedIndexInParams(goldWeight, dreamDustWeight, skillWeight, gemWeight, chaosWeight))
			{
			case 0:
				DropGoldReward();
				break;
			case 1:
				DropDreamDustReward();
				break;
			case 2:
				DropSkillReward(isHigh: true, bonusSkillLevel, new Rarity[2]
				{
					Rarity.Rare,
					Rarity.Epic
				});
				break;
			case 3:
				DropGemReward(isHigh: true, bonusEssenceQuality, new Rarity[2]
				{
					Rarity.Rare,
					Rarity.Epic
				});
				break;
			case 4:
				DropChaosReward(isHigh: true, new Rarity[2]
				{
					Rarity.Rare,
					Rarity.Epic
				});
				break;
			}
		}
		else
		{
			switch (Dew.SelectRandomWeightedIndexInParams(goldWeight, dreamDustWeight, skillWeight, gemWeight, chaosWeight))
			{
			case 0:
				DropGoldReward();
				break;
			case 1:
				DropDreamDustReward();
				break;
			case 2:
				DropSkillReward(isHigh: true, bonusSkillLevel, new Rarity[2]
				{
					Rarity.Epic,
					Rarity.Legendary
				});
				break;
			case 3:
				DropGemReward(isHigh: true, bonusEssenceQuality, new Rarity[2]
				{
					Rarity.Epic,
					Rarity.Legendary
				});
				break;
			case 4:
				DropChaosReward(isHigh: true, new Rarity[2]
				{
					Rarity.Epic,
					Rarity.Legendary
				});
				break;
			}
		}
		void DropChaosReward(bool isHigh, Rarity[] rarities)
		{
			Dew.CreateActor(pos, null, null, delegate(Shrine_Chaos chaos)
			{
				chaos.playersOverride = new DewPlayer[1] { hero.owner };
				(float, Rarity)[] array3 = new(float, Rarity)[rarities.Length];
				for (int k = 0; k < rarities.Length; k++)
				{
					array3[k] = ((isHigh ? chaos.highChance : chaos.chance).Get(rarities[k]), rarities[k]);
				}
				Rarity rarity3 = Dew.SelectRandomWeightedInParams(array3);
				chaos.rarity = rarity3;
			});
			NetworkedManagerBase<ChatManager>.instance.SendChatMessage((NetworkConnectionToClient)hero.owner, new ChatManager.Message
			{
				type = ChatManager.MessageType.Notice,
				content = "Chat_Hatred_RewardedChaos"
			});
		}
		void DropDreamDustReward()
		{
			float floatAmount = GetBaseRewardAmount_DreamDust() * dreamDustMultipliers[GetIndexOfStrength(type)] * global::UnityEngine.Random.Range(1f - currencyDeviation, 1f + currencyDeviation);
			CreateAbilityInstance(base.transform.position, null, new CastInfo(hero, pos), delegate(Ai_Shrine_Hatred_CurrencyReward r)
			{
				r.dreamDust = DewMath.RandomRoundToInt(floatAmount);
			});
			NetworkedManagerBase<ChatManager>.instance.SendChatMessage((NetworkConnectionToClient)hero.owner, new ChatManager.Message
			{
				type = ChatManager.MessageType.Notice,
				content = "Chat_Hatred_RewardedDreamDust"
			});
		}
		void DropGemReward(bool isHigh, int addedQuality, Rarity[] rarities)
		{
			Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
			(float, Rarity)[] array = new(float, Rarity)[rarities.Length];
			for (int i = 0; i < rarities.Length; i++)
			{
				array[i] = ((isHigh ? lootInstance.gemRarityChanceHigh : lootInstance.gemRarityChance).Get(rarities[i]), rarities[i]);
			}
			Rarity rarity = Dew.SelectRandomWeightedInParams(array);
			lootInstance.SelectGemAndQuality(rarity, out var gem, out var quality);
			Dew.CreateGem(gem, pos, quality + addedQuality, hero.owner);
			NetworkedManagerBase<ChatManager>.instance.SendChatMessage((NetworkConnectionToClient)hero.owner, new ChatManager.Message
			{
				type = ChatManager.MessageType.Notice,
				content = "Chat_Hatred_RewardedEssence"
			});
		}
		void DropGoldReward()
		{
			float floatAmount2 = GetBaseRewardAmount_Gold() * goldMultipliers[GetIndexOfStrength(type)] * global::UnityEngine.Random.Range(1f - currencyDeviation, 1f + currencyDeviation);
			CreateAbilityInstance(base.transform.position, null, new CastInfo(hero, pos), delegate(Ai_Shrine_Hatred_CurrencyReward r)
			{
				r.gold = DewMath.RandomRoundToInt(floatAmount2);
			});
			NetworkedManagerBase<ChatManager>.instance.SendChatMessage((NetworkConnectionToClient)hero.owner, new ChatManager.Message
			{
				type = ChatManager.MessageType.Notice,
				content = "Chat_Hatred_RewardedGold"
			});
		}
		void DropSkillReward(bool isHigh, int addedLevel, Rarity[] rarities)
		{
			Loot_Skill lootInstance2 = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Skill>();
			(float, Rarity)[] array2 = new(float, Rarity)[rarities.Length];
			for (int j = 0; j < rarities.Length; j++)
			{
				array2[j] = ((isHigh ? lootInstance2.skillRarityChanceHigh : lootInstance2.skillRarityChance).Get(rarities[j]), rarities[j]);
			}
			Rarity rarity2 = Dew.SelectRandomWeightedInParams(array2);
			lootInstance2.SelectSkillAndLevel(rarity2, out var skill, out var level);
			Dew.CreateSkillTrigger(skill, pos, level + addedLevel, hero.owner);
			NetworkedManagerBase<ChatManager>.instance.SendChatMessage((NetworkConnectionToClient)hero.owner, new ChatManager.Message
			{
				type = ChatManager.MessageType.Notice,
				content = "Chat_Hatred_RewardedMemory"
			});
		}
	}

	public override void OnSaveActor(Dictionary<string, object> data)
	{
		base.OnSaveActor(data);
		data["choices"] = new List<HatredStrengthType>(choices);
		data["_usedPlayers"] = new List<uint>(_usedPlayers);
	}

	public override void OnLoadActor(Dictionary<string, object> data)
	{
		base.OnLoadActor(data);
		choices.Clear();
		choices.AddRange((List<HatredStrengthType>)data["choices"]);
		_usedPlayers.Clear();
		_usedPlayers.AddRange((List<uint>)data["_usedPlayers"]);
	}

	public Shrine_Hatred()
	{
		InitSyncObject(choices);
		InitSyncObject(_usedPlayers);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_TpcOpenWindow__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		ManagerBase<FloatingWindowManager>.instance.SetTarget(this);
	}

	protected static void InvokeUserCode_TpcOpenWindow__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TpcOpenWindow called on server.");
		}
		else
		{
			((Shrine_Hatred)obj).UserCode_TpcOpenWindow__NetworkConnectionToClient((NetworkConnectionToClient)NetworkClient.connection);
		}
	}

	protected void UserCode_CmdChoose__Int32__NetworkConnectionToClient(int index, NetworkConnectionToClient sender)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			if (index >= 0 && index < choices.Count)
			{
				DewPlayer player = sender.GetPlayer();
				if (!(player == null) && !player.hero.IsNullInactiveDeadOrKnockedOut() && !_usedPlayers.Contains(player.netId))
				{
					_usedPlayers.Add(player.netId);
					UpdateAvailability();
					DoPostUseRoutines(player.hero);
					if (choices[index] != 0)
					{
						yield return new WaitForSeconds(0.75f);
						DoCurse(choices[index], player.hero);
					}
					yield return new WaitForSeconds(0.8f);
					DoReward(choices[index], player.hero);
				}
			}
		}
	}

	protected static void InvokeUserCode_CmdChoose__Int32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChoose called on client.");
		}
		else
		{
			((Shrine_Hatred)obj).UserCode_CmdChoose__Int32__NetworkConnectionToClient(reader.ReadInt(), senderConnection);
		}
	}

	static Shrine_Hatred()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(Shrine_Hatred), "System.Void Shrine_Hatred::CmdChoose(System.Int32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdChoose__Int32__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterRpc(typeof(Shrine_Hatred), "System.Void Shrine_Hatred::TpcOpenWindow(Mirror.NetworkConnectionToClient)", InvokeUserCode_TpcOpenWindow__NetworkConnectionToClient);
	}
}
