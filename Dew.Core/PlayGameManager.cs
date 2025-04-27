using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

[DewResourceLink(ResourceLinkBy.Type)]
public class PlayGameManager : GameManager
{
	public new static PlayGameManager instance => NetworkedManagerBase<GameManager>.instance as PlayGameManager;

	public override void OnStartServer()
	{
		AttrCustomizeResources.ResetConfig();
		base.OnStartServer();
		AttrCustomizeManager.ExecuteInGameOnce();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		GameManager.CallOnReady(delegate
		{
			DewSave.profile.preferredHero = DewPlayer.local.hero.GetType().Name;
			if (base.isServer)
			{
				DewSave.profile.preferredLucidDreams.Clear();
				DewSave.profile.preferredLucidDreams.AddRange(NetworkedManagerBase<GameSettingsManager>.instance.lucidDreams);
			}
			if (base.isServer)
			{
				NetworkedManagerBase<QuestManager>.instance.StartQuest<Quest_ShapeOfDreams>();
			}
		});
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			GameManager.CallOnReady(delegate
			{
				Dew.GetControlPresetWindow().Show(showCancel: true);
			});
		}
		StartCoroutine(IncrementPlaytime());
		static IEnumerator IncrementPlaytime()
		{
			while (NetworkClient.active)
			{
				yield return new WaitForSeconds(60f);
				DewSave.profile.totalPlayTimeMinutes++;
			}
		}
	}

	private void SpawnHero(DewPlayer player, Hero hero, HeroLoadoutData loadout, List<string> accs)
	{
		if (player.hero != null)
		{
			Dew.Destroy(player.hero.gameObject);
		}
		Vector3 spawnPos = ((NetworkedManagerBase<ZoneManager>.instance.currentRoom == null) ? Vector3.zero : NetworkedManagerBase<ZoneManager>.instance.currentRoom.GetHeroSpawnPosition());
		Hero newHero = Dew.SpawnHero(hero, spawnPos, Quaternion.identity, player, 1, loadout, delegate(Hero h)
		{
			h.accessories.AddRange(accs);
		});
		if (DewBuildProfile.current.bonusMemoryHaste > 0.1f)
		{
			newHero.Status.AddStatBonus(new StatBonus
			{
				abilityHasteFlat = DewBuildProfile.current.bonusMemoryHaste
			});
		}
		player.hero = newHero;
		player.controllingEntity = newHero;
	}

	public override void OnLateStartServer()
	{
		base.OnLateStartServer();
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return Dew.WaitForClientsReadyRoutine();
			foreach (DewPlayer h in DewPlayer.humanPlayers)
			{
				SpawnHero(h, DewResources.GetByShortTypeName<Hero>(h.selectedHeroType), h.selectedLoadout, h.selectedAccessories.ToList());
			}
			yield return new WaitForSecondsRealtime(0.1f);
			LoadNextZone();
			if (DewBuildProfile.current.buildType != BuildType.DemoLite)
			{
				NetworkedManagerBase<ZoneManager>.instance.CallOnReadyAfterTransition(DoDejavuSpawns);
			}
		}
	}

	private void DoDejavuSpawns()
	{
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			try
			{
				if (h.hero == null || string.IsNullOrEmpty(h.selectedDejavuItem))
				{
					continue;
				}
				Vector3 spawnPos = Dew.GetGoodRewardPosition(h.hero.agentPosition);
				if (h.selectedDejavuItem.StartsWith("St_"))
				{
					if (Dew.IsSkillIncludedInGame(h.selectedDejavuItem))
					{
						SkillTrigger target = DewResources.GetByShortTypeName<SkillTrigger>(h.selectedDejavuItem);
						Rarity rarity = target.rarity;
						if (rarity != Rarity.Character && rarity != Rarity.Identity)
						{
							Dew.CreateSkillTrigger(target, spawnPos, 1, h);
							goto IL_00d6;
						}
					}
				}
				else if (h.selectedDejavuItem.StartsWith("Gem_") && Dew.IsGemIncludedInGame(h.selectedDejavuItem))
				{
					Dew.CreateGem(DewResources.GetByShortTypeName<Gem>(h.selectedDejavuItem), spawnPos, 100, h);
					goto IL_00d6;
				}
				goto end_IL_0017;
				IL_00d6:
				h.TpcNotifyDejavuUse();
				end_IL_0017:;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	public override void LoadNextZone()
	{
		base.LoadNextZone();
		if (NetworkedManagerBase<ZoneManager>.instance.currentZone != null && DewBuildProfile.current.buildType == BuildType.DemoLite && DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			int sumOfZones = DewBuildProfile.current.content.zoneCountByTier.Sum();
			if (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex >= sumOfZones - 1)
			{
				ConcludeDemo();
				return;
			}
		}
		NetworkedManagerBase<ZoneManager>.instance.LoadNextZoneByContentSettings();
	}

	protected override DewDifficultySettings GetDifficulty()
	{
		return DewResources.GetByName<DewDifficultySettings>(NetworkedManagerBase<GameSettingsManager>.instance.difficulty);
	}

	private void MirrorProcessed()
	{
	}
}
