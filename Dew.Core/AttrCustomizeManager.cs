using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;

public class AttrCustomizeManager
{
    private static int _lastObliviaxQuestZoneIndex = -100;
    private static bool _globalOnceFlag = false;

    private static Dictionary<String, DifficultyData> _difficultyDataMap;

    public class DifficultyData(DewDifficultySettings dewDifficultySettings)
    {
        private string name = dewDifficultySettings.name;
        public float healRawMultiplier = dewDifficultySettings.healRawMultiplier;
        public float beneficialNodeMultiplier = dewDifficultySettings.beneficialNodeMultiplier;
    }
    
    private class Ad_MonsterLevelUp
    {
        public StatBonus bonus;
    }

    public static void ExecuteGlobalOnce()
    {
        LoadDifficultyData();
        _globalOnceFlag = true;
    }

    private static void LoadDifficultyData()
    {
        if (_difficultyDataMap != null) return;
        
        IEnumerable<DewDifficultySettings> difficultySettingsEnumerable =
            DewResources.FindAllByNameSubstring<DewDifficultySettings>("diff");

        _difficultyDataMap = new Dictionary<String, DifficultyData>();
        foreach (var dewDifficultySettings in difficultySettingsEnumerable)
        {
            _difficultyDataMap.Add(dewDifficultySettings.name, new DifficultyData(dewDifficultySettings));
        }
    }

    public static void ExecuteInGameOnce()
    {
        if (!_globalOnceFlag)
        {
            ExecuteGlobalOnce();
        }

        ModifyBeneficialNodeMultiplier();
        ModifyHealRawMultiplier();
        LucidDreamEmbraceMortality();
        LucidDreamBonVoyage();
        LucidDreamGrievousWounds();
        LucidDreamTheDarkestUrge();
        LucidDreamWild();
        LucidDreamMadLife();
        LucidDreamSparklingDreamFlask();
    }

    private static void LucidDreamSparklingDreamFlask()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamSparklingDreamFlask)
        {
            NetworkedManagerBase<ActorManager>.instance.onActorAdd +=
                new Action<Actor>(LucidDreamSparklingDreamFlaskOnActorAdd);
            foreach (Actor a in NetworkedManagerBase<ActorManager>.instance.allActors)
            {
                LucidDreamSparklingDreamFlaskOnActorAdd(a);
            }
        }
    }

    private static void LucidDreamSparklingDreamFlaskOnActorAdd(Actor obj)
    {
        if (obj is Ai_RegenOrb_Projectile potion)
        {
            potion.actionOverride = delegate(Entity target)
            {
                float value = GetBaseRewardAmount_DreamDust() * global::UnityEngine.Random.Range(0.9f, 1.1f) * 0.4f;
                NetworkedManagerBase<PickupManager>.instance.DropDreamDust(isGivenByOtherPlayer: false,
                    DewMath.RandomRoundToInt(value), target.agentPosition, (Hero)target);
            };
        }

        Type type = obj.GetType();
        if (type.ToString() == "Shrine_Guidance")
        {
            // 获取 TargetClass 的 Type 对象
            Type targetType = type;

            // 获取 actionOverride 字段的 FieldInfo 对象
            FieldInfo actionOverrideField =
                targetType.GetField("actionOverride", BindingFlags.Public | BindingFlags.Instance);
            if (actionOverrideField != null)
            {
                actionOverrideField.SetValue(obj, delegate(Entity target)
                {
                    float value2 = GetBaseRewardAmount_DreamDust() * global::UnityEngine.Random.Range(0.9f, 1.1f) *
                                   1.2f;
                    NetworkedManagerBase<PickupManager>.instance.DropDreamDust(isGivenByOtherPlayer: false,
                        DewMath.RandomRoundToInt(value2), obj.position, (Hero)target);
                });
            }
        }
    }

    public static float GetBaseRewardAmount_DreamDust()
    {
        int currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
        Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
        lootInstance.gemQualityMinByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
        float f = lootInstance.gemQualityMaxByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
        return (float)NetworkedManagerBase<GameManager>.instance.GetGemUpgradeDreamDustCost(Mathf.RoundToInt(f)) *
               (1f + (float)NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex * 0.1f) *
               (1f + (float)currentZoneIndex * 0.125f);
    }

    private static void LucidDreamMadLife()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamMadLife)
        {
            NetworkedManagerBase<GameManager>.instance.predictionStrengthOverride =
                () => 0.8f + UnityEngine.Random.value * 0.2f;
        }
    }

    private static void LucidDreamWild()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamWild)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd += new Action<Entity>(LucidDreamWildOnEntityAdd);
        }
    }

    private static void LucidDreamWildOnEntityAdd(Entity obj)
    {
        if (obj is Monster && !(obj.owner != DewPlayer.creep) && !obj.IsAnyBoss() &&
            !obj.Status.HasStatusEffect<Se_HunterBuff>())
        {
            NetworkedManagerBase<ActorManager>.instance.serverActor
                .CreateStatusEffect<Se_HunterBuff>(obj, new CastInfo(obj, obj)).enableGoldAndExpDrops = true;
            DewResources.GetByType<RoomMod_Hunted>()
                .ApplyHunterStatBonusAndAIPrediction(obj, NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel);
        }
    }

    private static void LucidDreamTheDarkestUrge()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamTheDarkestUrge)
        {
            DewPlayer.creep.enemies.Add(DewPlayer.creep);
            foreach (DewPlayer x in DewPlayer.humanPlayers)
            {
                foreach (DewPlayer y in DewPlayer.humanPlayers)
                {
                    if (!(x == y))
                    {
                        x.neutrals.Add(y);
                    }
                }
            }

            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamTheDarkestUrgeOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamTheDarkestUrgeOnEntityAdd(e);
            }
        }
    }

    private static void LucidDreamTheDarkestUrgeOnEntityAdd(Entity entity)
    {
        if (entity is Monster m)
        {
            m.ActorEvent_OnKill += new Action<EventInfoKill>(LucidDreamTheDarkestUrgeActorEventOnKill);
        }
    }

    private static void LucidDreamTheDarkestUrgeActorEventOnKill(EventInfoKill obj)
    {
        Entity killer = obj.actor.firstEntity;
        if (!(killer == null) && !(obj.victim == null) && obj.victim is Monster)
        {
            if (!killer.TryGetData<Ad_MonsterLevelUp>(out var data))
            {
                data = new Ad_MonsterLevelUp
                {
                    bonus = killer.Status.AddStatBonus(new StatBonus())
                };
                killer.AddData(data);
            }

            if (obj.victim.TryGetData<Ad_MonsterLevelUp>(out var victimData))
            {
                data.bonus.attackDamagePercentage += victimData.bonus.attackDamagePercentage;
                data.bonus.abilityPowerPercentage += victimData.bonus.abilityPowerPercentage;
                data.bonus.attackSpeedPercentage += victimData.bonus.attackSpeedPercentage;
                data.bonus.movementSpeedPercentage += victimData.bonus.movementSpeedPercentage;
                data.bonus.abilityHasteFlat += victimData.bonus.abilityHasteFlat;
            }

            data.bonus.maxHealthFlat += obj.victim.maxHealth * 0.8f;
            data.bonus.attackDamagePercentage += 25f;
            data.bonus.abilityPowerPercentage += 25f;
            data.bonus.attackSpeedPercentage += 10f;
            data.bonus.movementSpeedPercentage += 10f;
            data.bonus.abilityHasteFlat += 15f;
            killer.Heal(obj.victim.maxHealth * 0.8f).Dispatch(killer);
        }
    }

    private static void LucidDreamGrievousWounds()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamGrievousWounds)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamGrievousWoundsOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamGrievousWoundsOnEntityAdd(e);
            }
        }
    }

    private static void LucidDreamGrievousWoundsOnEntityAdd(Entity entity)
    {
        if (entity is Hero)
        {
            entity.takenHealProcessor.Add(LucidDreamGrievousWoundsHealProcessor, 200);
            entity.takenShieldProcessor.Add(LucidDreamGrievousWoundsShieldProcessor);
        }
    }

    private static void LucidDreamGrievousWoundsHealProcessor(ref HealData data, Actor actor, Entity target)
    {
        data.ApplyRawMultiplier(0.5f);
    }

    private static void HealProcessor(ref HealData data, Actor actor, Entity target)
    {
        data.ApplyRawMultiplier(0.5f);
    }

    private static void LucidDreamGrievousWoundsShieldProcessor(ref HealData data, Actor actor, Entity target)
    {
        data.ApplyRawMultiplier(0.5f);
    }

    private static void LucidDreamBonVoyage()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamBonVoyage)
        {
            NetworkedManagerBase<ZoneManager>.instance.isHuntAdvanceDisabled = true;
            NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex = -1;
            NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded +=
                new Action<EventInfoLoadRoom>(LucidDreamBonVoyageClientEventOnRoomLoaded);
        }
    }

    private static void LucidDreamBonVoyageClientEventOnRoomLoaded(EventInfoLoadRoom obj)
    {
        NetworkedManagerBase<ZoneManager>.instance.isHuntAdvanceDisabled = true;
        NetworkedManagerBase<ZoneManager>.instance.hunterStartNodeIndex = -1;
    }

    private static void LucidDreamEmbraceMortality()
    {
        if (AttrCustomizeResources.Config.enableLucidDreamEmbraceMortality)
        {
            NetworkedManagerBase<ActorManager>.instance.onEntityAdd +=
                new Action<Entity>(LucidDreamEmbraceMortalityOnEntityAdd);
            foreach (Entity e in NetworkedManagerBase<ActorManager>.instance.allEntities)
            {
                LucidDreamEmbraceMortalityOnEntityAdd(e);
            }
        }
    }

    private static void LucidDreamEmbraceMortalityOnEntityAdd(Entity entity)
    {
        entity.dealtDamageProcessor.Add(LucidDreamEmbraceMortalityProcessor);
    }

    private static void LucidDreamEmbraceMortalityProcessor(ref DamageData data, Actor actor, Entity target)
    {
        Entity attacker = actor.firstEntity;
        if (!(attacker == null) && !(target == null) && !(attacker == target))
        {
            data.ApplyAmplification(1f);
        }
    }

    private static void ModifyHealRawMultiplier()
    {
        
        IEnumerable<DewDifficultySettings> difficultySettingsEnumerable =
            DewResources.FindAllByNameSubstring<DewDifficultySettings>("diff");
        foreach (var dewDifficultySettings in difficultySettingsEnumerable)
        {
            var difficultyData = _difficultyDataMap[dewDifficultySettings.name];
            dewDifficultySettings.healRawMultiplier = AttrCustomizeResources.Config.healRawMultiplier *
                                                      difficultyData.healRawMultiplier;
        }
    }


    private static void ModifyBeneficialNodeMultiplier()
    {
        
        IEnumerable<DewDifficultySettings> difficultySettingsEnumerable =
            DewResources.FindAllByNameSubstring<DewDifficultySettings>("diff");
        foreach (var dewDifficultySettings in difficultySettingsEnumerable)
        {
            var difficultyData = _difficultyDataMap[dewDifficultySettings.name];
            dewDifficultySettings.beneficialNodeMultiplier = AttrCustomizeResources.Config.beneficialNodeMultiplier *
                                                             difficultyData.beneficialNodeMultiplier;
        }
    }

    public static void ExecuteInGameOnceAtGameLoaded()
    {
        SendStartGameNotice();

        ResetObliviaxQuestZone();
    }

    private static void ResetObliviaxQuestZone()
    {
        _lastObliviaxQuestZoneIndex = -100;
    }

    public static void ExecuteLoopOnce()
    {
    }

    public static void ExecuteZoneOnce()
    {
        if (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex == 0)
        {
            ExecuteInGameOnceAtGameLoaded();
        }

        WorldReveal();

        RemoveRoomMod();

        DamageRanking();
    }

    private static void WorldReveal()
    {
        if (AttrCustomizeResources.Config.enableWorldReveal)
        {
            ConsoleCommands.WorldRevealFull();
        }
    }

    private static void DamageRanking()
    {
        if (AttrCustomizeResources.Config.enableDamageRanking)
        {
            DewGameResult tracked = (DewGameResult)typeof(GameResultManager)
                .GetField("_tracked",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(NetworkedManagerBase<GameResultManager>.instance);

            if (tracked != null)
            {
                List<ValueTuple<string, float, float>> dmgList = new List<ValueTuple<string, float, float>>();

                // 使用 for 循环遍历玩家数据
                for (int i = 0; i < tracked.players.Count; i++)
                {
                    DewGameResult.PlayerData playerData = tracked.players[i];
                    string playerProfileName = playerData.playerProfileName;
                    float totalDmg = playerData.dealtDamageToEnemies;
                    float maxDmg = playerData.maxDealtSingleDamageToEnemy;
                    dmgList.Add(ValueTuple.Create(playerProfileName, totalDmg, maxDmg));
                }

                // 按总伤害降序排序（显式委托）
                dmgList.Sort(delegate(ValueTuple<string, float, float> a, ValueTuple<string, float, float> b)
                {
                    return b.Item2.CompareTo(a.Item2); // 降序比较
                });

                StringBuilder sb = new StringBuilder();
                sb.Append("伤害排行");

                // 使用 for 循环遍历伤害列表
                for (int j = 0; j < dmgList.Count; j++)
                {
                    ValueTuple<string, float, float> valueTuple = dmgList[j];
                    string playerProfileName2 = valueTuple.Item1;
                    float totalDmg2 = valueTuple.Item2;
                    float maxDmg2 = valueTuple.Item3;
                    string totalDmgFormatted = totalDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                    string maxDmgFormatted = maxDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                    sb.Append(playerProfileName2 + ": 总伤害 " + totalDmgFormatted + " | 最强一击 " + maxDmgFormatted);
                }

                // 延迟发送消息（使用显式委托）
                Dew.CallDelayed(new Action(delegate
                {
                    ChatManager.Message message = new ChatManager.Message();
                    message.type = ChatManager.MessageType.Raw;
                    message.content = sb.ToString();
                    NetworkedManagerBase<ChatManager>.instance.BroadcastChatMessage(message);
                }), 100);
            }
        }
    }

    private static void QuestHuntedByObliviaxRepeatable()
    {
        List<ModifierData> modifiers = NetworkedManagerBase<ZoneManager>.instance.currentNode.modifiers;
        bool flag = false;
        for (int i = 0; i < modifiers.Count; i++)
        {
            if (modifiers[i].type == "RoomMod_Hunted")
            {
                flag = true;
            }
        }

        int currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;

        if (flag)
        {
            IReadOnlyList<DewQuest> activeQuests = NetworkedManagerBase<QuestManager>.instance.activeQuests;
            for (int k = 0; k < activeQuests.Count; k++)
            {
                if (activeQuests[k].GetType().Name == "Quest_HuntedByObliviax")
                {
                    _lastObliviaxQuestZoneIndex = currentZoneIndex;
                }
            }

            if (_lastObliviaxQuestZoneIndex > -1 && _lastObliviaxQuestZoneIndex != currentZoneIndex)
            {
                if (AttrCustomizeResources.Config.enableQuestHuntedByObliviaxRepeatable)
                {
                    DewQuest dewQuest = DewResources.FindOneByTypeSubstring<DewQuest>("Quest_HuntedByObliviax");
                    NetworkedManagerBase<QuestManager>.instance.StartQuest<DewQuest>(dewQuest, null);
                }
            }
        }
    }

    private static void RemoveRoomMod()
    {
        List<WorldNodeData> worldNodeDatas = NetworkedManagerBase<ZoneManager>.instance.nodes.ToList();

        if (!AttrCustomizeResources.Config.enableArtifactQuest)
        {
            for (int i = 0; i < worldNodeDatas.Count; i++)
            {
                NetworkedManagerBase<ZoneManager>.instance.RemoveModifier<RoomMod_Artifact>(i);
            }
        }

        if (!AttrCustomizeResources.Config.enableFragmentOfRadianceBossQuest)
        {
            for (int i = 0; i < worldNodeDatas.Count; i++)
            {
                int id = worldNodeDatas[i].FindModifierIndex("RoomMod_FragmentOfRadiance_StartProp");

                if (id != 0)
                {
                    NetworkedManagerBase<ZoneManager>.instance.RemoveModifier(i, id);
                }
            }
        }
    }

    private static void SendStartGameNotice()
    {
        Dew.CallDelayed(delegate
        {
            NetworkedManagerBase<ChatManager>.instance.BroadcastChatMessage(new ChatManager.Message
            {
                type = ChatManager.MessageType.Raw,
                content = BuildStartGameNotice()
            });
        }, 200);
    }

    public static void ExecuteRoomOnce()
    {
        DropGold();

        QuestHuntedByObliviaxRepeatable();
    }

    private static void DropGold()
    {
        Room room = SingletonDewNetworkBehaviour<Room>.instance;

        if (!room.isRevisit)
        {
            Vector3 pos = room.GetHeroSpawnPosition();
            int zoneAddCount = (NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex) *
                               AttrCustomizeResources.Config.firstVisitDropGoldCountAddByZone;
            int loopAddCount = (NetworkedManagerBase<ZoneManager>.instance.loopIndex) *
                               AttrCustomizeResources.Config.firstVisitDropGoldCountAddByLoop;
            int count = AttrCustomizeResources.Config.firstVisitDropGoldCount;
            int value = (count + loopAddCount + zoneAddCount) * DewPlayer.humanPlayers.Count;


            NetworkedManagerBase<PickupManager>.instance.DropGold(false, false, value, pos);
        }
    }


    public static float ExponentialGrowth(int x, double initialY, double multiplier)
    {
        if (multiplier - 0 < 0.00001)
        {
            return (float)initialY;
        }

        return (float)(initialY * Math.Pow(multiplier, x));
    }

    public static int GetMaxGemCount(HeroSkillLocation type)
    {
        switch (type)
        {
            case HeroSkillLocation.Q:
                return AttrCustomizeResources.Config.skillQGemCount;
            case HeroSkillLocation.W:
                return AttrCustomizeResources.Config.skillWGemCount;
            case HeroSkillLocation.E:
                return AttrCustomizeResources.Config.skillEGemCount;
            case HeroSkillLocation.R:
                return AttrCustomizeResources.Config.skillRGemCount;
            case HeroSkillLocation.Identity:
                return AttrCustomizeResources.Config.skillIdentityGemCount;
            case HeroSkillLocation.Movement:
                return AttrCustomizeResources.Config.skillMovementGemCount;
            default:
                return 0;
        }
    }

    private static string BuildStartGameNotice()
    {
        AttrCustomizeConfig config = AttrCustomizeResources.Config;

        string generateStartGameNotice = GenerateStartGameNotice(config);
        string startGameNotice = @$"

您正在游玩自定制版v0.2.5 改动内容如下

{generateStartGameNotice}
声明:本改包仅供个人测试使用，禁止用于商业用途，禁止用于非法用途，禁止用于破坏游戏平衡性。因使用本改包所造成的任何后果与作者无关。
关注qq联机/改包教程群 1034195007
";

        return startGameNotice;
    }

    private static bool IsFloatDifferent(float a, float b, float epsilon = 0.0001f)
    {
        return Math.Abs(a - b) > epsilon;
    }

    public static string GenerateStartGameNotice(AttrCustomizeConfig config)
    {
        String startSkillsText = "";
        for (int i = 0; i < config.startSkills.Length; i++)
        {
            startSkillsText += config.startSkills[i] + "->" + config.startSkillsLevel[i] + "级 ";
        }

        String startGemsText = "";
        for (int i = 0; i < config.startGems.Length; i++)
        {
            startGemsText += config.startGems[i] + "->" + config.startGemsQuality[i] + "品质 ";
        }

        String removeSkillsText = "";
        for (int i = 0; i < config.removeSkills.Length; i++)
        {
            removeSkillsText += config.removeSkills[i] + " ";
        }

        String removeGemsText = "";
        for (int i = 0; i < config.removeGems.Length; i++)
        {
            removeGemsText += config.removeGems[i] + " ";
        }


        bool appendFlag = false;

        var sb = new StringBuilder();

        if (config.maxPlayer != AttrCustomizeConfig.DefaultConfig.maxPlayer)
        {
            sb.Append($"房间最大人数：{AttrCustomizeConfig.DefaultConfig.maxPlayer} → {config.maxPlayer}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (IsFloatDifferent(config.enemyMovementSpeedPercentage,
                AttrCustomizeConfig.DefaultConfig.enemyMovementSpeedPercentage))
        {
            sb.Append(
                $"所有敌人移动速度百分比：{AttrCustomizeConfig.DefaultConfig.enemyMovementSpeedPercentage} → {config.enemyMovementSpeedPercentage}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.enemyAttackSpeedPercentage,
                AttrCustomizeConfig.DefaultConfig.enemyAttackSpeedPercentage))
        {
            sb.Append(
                $"所有敌人攻击速度百分比：{AttrCustomizeConfig.DefaultConfig.enemyAttackSpeedPercentage} → {config.enemyAttackSpeedPercentage}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.enemyAbilityHasteFlat, AttrCustomizeConfig.DefaultConfig.enemyAbilityHasteFlat))
        {
            sb.Append(
                $"所有敌人技能加速百分比：{AttrCustomizeConfig.DefaultConfig.enemyAbilityHasteFlat} → {config.enemyAbilityHasteFlat}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (IsFloatDifferent(config.bossHealthMultiplier, AttrCustomizeConfig.DefaultConfig.bossHealthMultiplier))
        {
            sb.Append(
                $"boss生命百分比：{AttrCustomizeConfig.DefaultConfig.bossHealthMultiplier} → {config.bossHealthMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.bossDamageMultiplier, AttrCustomizeConfig.DefaultConfig.bossDamageMultiplier))
        {
            sb.Append(
                $"boss伤害百分比：{AttrCustomizeConfig.DefaultConfig.bossDamageMultiplier} → {config.bossDamageMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.miniBossHealthMultiplier,
                AttrCustomizeConfig.DefaultConfig.miniBossHealthMultiplier))
        {
            sb.Append(
                $"miniBoss生命百分比：{AttrCustomizeConfig.DefaultConfig.miniBossHealthMultiplier} → {config.miniBossHealthMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.miniBossDamageMultiplier,
                AttrCustomizeConfig.DefaultConfig.miniBossDamageMultiplier))
        {
            sb.Append(
                $"miniBoss伤害百分比：{AttrCustomizeConfig.DefaultConfig.miniBossDamageMultiplier} → {config.miniBossDamageMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.littleMonsterHealthMultiplier,
                AttrCustomizeConfig.DefaultConfig.littleMonsterHealthMultiplier))
        {
            sb.Append(
                $"小怪生命百分比：{AttrCustomizeConfig.DefaultConfig.littleMonsterHealthMultiplier} → {config.littleMonsterHealthMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.littleMonsterDamageMultiplier,
                AttrCustomizeConfig.DefaultConfig.littleMonsterDamageMultiplier))
        {
            sb.Append(
                $"小怪伤害百分比：{AttrCustomizeConfig.DefaultConfig.littleMonsterDamageMultiplier} → {config.littleMonsterDamageMultiplier}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (IsFloatDifferent(config.extraHealthGrowthMultiplier,
                AttrCustomizeConfig.DefaultConfig.extraHealthGrowthMultiplier))
        {
            sb.Append(
                $"怪物额外生命成长倍率：{AttrCustomizeConfig.DefaultConfig.extraHealthGrowthMultiplier} → {config.extraHealthGrowthMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.extraDamageGrowthMultiplier,
                AttrCustomizeConfig.DefaultConfig.extraDamageGrowthMultiplier))
        {
            sb.Append(
                $"怪物额外伤害成长倍率：{AttrCustomizeConfig.DefaultConfig.extraDamageGrowthMultiplier} → {config.extraDamageGrowthMultiplier}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.skillQGemCount != AttrCustomizeConfig.DefaultConfig.skillQGemCount)
        {
            sb.Append($"Q技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillQGemCount} → {config.skillQGemCount}    ");
            appendFlag = true;
        }

        if (config.skillWGemCount != AttrCustomizeConfig.DefaultConfig.skillWGemCount)
        {
            sb.Append($"W技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillWGemCount} → {config.skillWGemCount}    ");
            appendFlag = true;
        }

        if (config.skillEGemCount != AttrCustomizeConfig.DefaultConfig.skillEGemCount)
        {
            sb.Append($"E技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillEGemCount} → {config.skillEGemCount}    ");
            appendFlag = true;
        }

        if (config.skillRGemCount != AttrCustomizeConfig.DefaultConfig.skillRGemCount)
        {
            sb.Append($"R技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillRGemCount} → {config.skillRGemCount}    ");
            appendFlag = true;
        }

        if (config.skillIdentityGemCount != AttrCustomizeConfig.DefaultConfig.skillIdentityGemCount)
        {
            sb.Append(
                $"身份技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillIdentityGemCount} → {config.skillIdentityGemCount}    ");
            appendFlag = true;
        }

        if (config.skillMovementGemCount != AttrCustomizeConfig.DefaultConfig.skillMovementGemCount)
        {
            sb.Append(
                $"位移技能精华槽数量：{AttrCustomizeConfig.DefaultConfig.skillMovementGemCount} → {config.skillMovementGemCount}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.shopAddedItems != AttrCustomizeConfig.DefaultConfig.shopAddedItems)
        {
            sb.Append($"商店增加物品数量：{AttrCustomizeConfig.DefaultConfig.shopAddedItems} → {config.shopAddedItems}    ");
            appendFlag = true;
        }

        if (config.shopRefreshes != AttrCustomizeConfig.DefaultConfig.shopRefreshes)
        {
            sb.Append($"商店刷新次数：{AttrCustomizeConfig.DefaultConfig.shopRefreshes} → {config.shopRefreshes}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.startSkills.Length != AttrCustomizeConfig.DefaultConfig.startSkills.Length)
        {
            sb.Append($"开局发放技能：{startSkillsText}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.startGems.Length != AttrCustomizeConfig.DefaultConfig.startGems.Length)
        {
            sb.Append($"开局发放精华：{startGemsText}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.removeSkills.Length != AttrCustomizeConfig.DefaultConfig.removeSkills.Length)
        {
            sb.Append($"移除技能：{removeSkillsText}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.removeGems.Length != AttrCustomizeConfig.DefaultConfig.removeGems.Length)
        {
            sb.Append($"移除精华：{removeGemsText}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (IsFloatDifferent(config.firstVisitDropGoldCount, AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCount))
        {
            sb.Append(
                $"首次访问节点发钱数量：{AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCount} → {config.firstVisitDropGoldCount}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.firstVisitDropGoldCountAddByZone,
                AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCountAddByZone))
        {
            sb.Append(
                $"首次访问节点每关增加发钱数量：{AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCountAddByZone} → {config.firstVisitDropGoldCountAddByZone}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.firstVisitDropGoldCountAddByLoop,
                AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCountAddByLoop))
        {
            sb.Append(
                $"首次访问节点每周目增加发钱数量：{AttrCustomizeConfig.DefaultConfig.firstVisitDropGoldCountAddByLoop} → {config.firstVisitDropGoldCountAddByLoop}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }


        if (config.bossCount != AttrCustomizeConfig.DefaultConfig.bossCount)
        {
            sb.Append($"boss数量：{AttrCustomizeConfig.DefaultConfig.bossCount} → {config.bossCount}    ");
            appendFlag = true;
        }

        if (config.bossCountAddByLoop != AttrCustomizeConfig.DefaultConfig.bossCountAddByLoop)
        {
            sb.Append(
                $"每周目添加boss数量：{AttrCustomizeConfig.DefaultConfig.bossCountAddByLoop} → {config.bossCountAddByLoop}    ");
            appendFlag = true;
        }

        if (config.bossCountAddByZone != AttrCustomizeConfig.DefaultConfig.bossCountAddByZone)
        {
            sb.Append(
                $"每关添加boss数量：{AttrCustomizeConfig.DefaultConfig.bossCountAddByZone} → {config.bossCountAddByZone}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.bossHunterChance, AttrCustomizeConfig.DefaultConfig.bossHunterChance))
        {
            sb.Append(
                $"boss猎手化概率：{AttrCustomizeConfig.DefaultConfig.bossHunterChance} → {config.bossHunterChance}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.bossMirageChance, AttrCustomizeConfig.DefaultConfig.bossMirageChance))
        {
            sb.Append(
                $"boss幻想化概率：{AttrCustomizeConfig.DefaultConfig.bossMirageChance} → {config.bossMirageChance}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.bossSingleInjuryHealthMultiplier,
                AttrCustomizeConfig.DefaultConfig.bossSingleInjuryHealthMultiplier))
        {
            sb.Append(
                $"对boss限伤：{AttrCustomizeConfig.DefaultConfig.bossSingleInjuryHealthMultiplier} → {config.bossSingleInjuryHealthMultiplier}    ");
            appendFlag = true;
        }


        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (IsFloatDifferent(config.monsterMirageChanceMultiple,
                AttrCustomizeConfig.DefaultConfig.monsterMirageChanceMultiple))
        {
            sb.Append(
                $"小怪幻想化概率倍数：{AttrCustomizeConfig.DefaultConfig.monsterMirageChanceMultiple} → {config.monsterMirageChanceMultiple}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.beneficialNodeMultiplier,
                AttrCustomizeConfig.DefaultConfig.beneficialNodeMultiplier))
        {
            sb.Append(
                $"引导祭坛数量倍数：{AttrCustomizeConfig.DefaultConfig.beneficialNodeMultiplier} → {config.beneficialNodeMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.maxAndSpawnedPopulationMultiplier,
                AttrCustomizeConfig.DefaultConfig.maxAndSpawnedPopulationMultiplier))
        {
            sb.Append(
                $"人口过剩人口倍数：{AttrCustomizeConfig.DefaultConfig.maxAndSpawnedPopulationMultiplier} → {config.maxAndSpawnedPopulationMultiplier}    ");
            appendFlag = true;
        }

        if (IsFloatDifferent(config.healRawMultiplier,
                AttrCustomizeConfig.DefaultConfig.healRawMultiplier))
        {
            sb.Append(
                $"治疗效果百分比：{AttrCustomizeConfig.DefaultConfig.healRawMultiplier} → {config.healRawMultiplier}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        // bool类型（开启/关闭）
        if (config.enableHeroSkillAddShop != AttrCustomizeConfig.DefaultConfig.enableHeroSkillAddShop)
        {
            sb.Append(
                $"转职：{(AttrCustomizeConfig.DefaultConfig.enableHeroSkillAddShop ? "开启" : "关闭")} → {(config.enableHeroSkillAddShop ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableGemMerge != AttrCustomizeConfig.DefaultConfig.enableGemMerge)
        {
            sb.Append(
                $"精华合并：{(AttrCustomizeConfig.DefaultConfig.enableGemMerge ? "开启" : "关闭")} → {(config.enableGemMerge ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableMistAllowAnyDirection != AttrCustomizeConfig.DefaultConfig.enableMistAllowAnyDirection)
        {
            sb.Append(
                $"薄雾全方位招架：{(AttrCustomizeConfig.DefaultConfig.enableMistAllowAnyDirection ? "开启" : "关闭")} → {(config.enableMistAllowAnyDirection ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableHealthReduceMultiplierAddByZone !=
            AttrCustomizeConfig.DefaultConfig.enableHealthReduceMultiplierAddByZone)
        {
            sb.Append(
                $"幻想上限每周关增加：{(AttrCustomizeConfig.DefaultConfig.enableHealthReduceMultiplierAddByZone ? "开启" : "关闭")} → {(config.enableHealthReduceMultiplierAddByZone ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.enableCurrentNodeGenerateLostSoul !=
            AttrCustomizeConfig.DefaultConfig.enableCurrentNodeGenerateLostSoul)
        {
            sb.Append(
                $"当前节点生成迷失灵魂：{(AttrCustomizeConfig.DefaultConfig.enableCurrentNodeGenerateLostSoul ? "开启" : "关闭")} → {(config.enableCurrentNodeGenerateLostSoul ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableBossRoomGenerateLostSoul != AttrCustomizeConfig.DefaultConfig.enableBossRoomGenerateLostSoul)
        {
            sb.Append(
                $"Boss房生成迷失灵魂：{(AttrCustomizeConfig.DefaultConfig.enableBossRoomGenerateLostSoul ? "开启" : "关闭")} → {(config.enableBossRoomGenerateLostSoul ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableArtifactQuest != AttrCustomizeConfig.DefaultConfig.enableArtifactQuest)
        {
            sb.Append(
                $"遗物任务：{(AttrCustomizeConfig.DefaultConfig.enableArtifactQuest ? "开启" : "关闭")} → {(config.enableArtifactQuest ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableFragmentOfRadianceBossQuest !=
            AttrCustomizeConfig.DefaultConfig.enableFragmentOfRadianceBossQuest)
        {
            sb.Append(
                $"光辉BOSS任务：{(AttrCustomizeConfig.DefaultConfig.enableFragmentOfRadianceBossQuest ? "开启" : "关闭")} → {(config.enableFragmentOfRadianceBossQuest ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableQuestHuntedByObliviaxRepeatable !=
            AttrCustomizeConfig.DefaultConfig.enableQuestHuntedByObliviaxRepeatable)
        {
            sb.Append(
                $"遗忘猎手任务可重复生成：{(AttrCustomizeConfig.DefaultConfig.enableQuestHuntedByObliviaxRepeatable ? "开启" : "关闭")} → {(config.enableQuestHuntedByObliviaxRepeatable ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.enableBossSpawnAllOnce != AttrCustomizeConfig.DefaultConfig.enableBossSpawnAllOnce)
        {
            sb.Append(
                $"所有boss一次性生成：{(AttrCustomizeConfig.DefaultConfig.enableBossSpawnAllOnce ? "开启" : "关闭")} → {(config.enableBossSpawnAllOnce ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableWorldReveal != AttrCustomizeConfig.DefaultConfig.enableWorldReveal)
        {
            sb.Append(
                $"所有节点透视：{(AttrCustomizeConfig.DefaultConfig.enableWorldReveal ? "开启" : "关闭")} → {(config.enableWorldReveal ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.enableLucidDreamEmbraceMortality !=
            AttrCustomizeConfig.DefaultConfig.enableLucidDreamEmbraceMortality)
        {
            sb.Append(
                $"清醒梦 拥抱死亡：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamEmbraceMortality ? "开启" : "关闭")} → {(config.enableLucidDreamEmbraceMortality ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamBonVoyage != AttrCustomizeConfig.DefaultConfig.enableLucidDreamBonVoyage)
        {
            sb.Append(
                $"清醒梦 一路顺风：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamBonVoyage ? "开启" : "关闭")} → {(config.enableLucidDreamBonVoyage ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamGrievousWounds != AttrCustomizeConfig.DefaultConfig.enableLucidDreamGrievousWounds)
        {
            sb.Append(
                $"清醒梦 剧痛伤口：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamGrievousWounds ? "开启" : "关闭")} → {(config.enableLucidDreamGrievousWounds ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamTheDarkestUrge != AttrCustomizeConfig.DefaultConfig.enableLucidDreamTheDarkestUrge)
        {
            sb.Append(
                $"清醒梦 极暗冲动：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamTheDarkestUrge ? "开启" : "关闭")} → {(config.enableLucidDreamTheDarkestUrge ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamWild != AttrCustomizeConfig.DefaultConfig.enableLucidDreamWild)
        {
            sb.Append(
                $"清醒梦 野性本能：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamWild ? "开启" : "关闭")} → {(config.enableLucidDreamWild ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamMadLife != AttrCustomizeConfig.DefaultConfig.enableLucidDreamMadLife)
        {
            sb.Append(
                $"清醒梦 疯狂生涯：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamMadLife ? "开启" : "关闭")} → {(config.enableLucidDreamMadLife ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (config.enableLucidDreamSparklingDreamFlask !=
            AttrCustomizeConfig.DefaultConfig.enableLucidDreamSparklingDreamFlask)
        {
            sb.Append(
                $"清醒梦 泡影浮梦：{(AttrCustomizeConfig.DefaultConfig.enableLucidDreamSparklingDreamFlask ? "开启" : "关闭")} → {(config.enableLucidDreamSparklingDreamFlask ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        if (config.enableDamageRanking != AttrCustomizeConfig.DefaultConfig.enableDamageRanking)
        {
            sb.Append(
                $"每关发送伤害排行榜：{(AttrCustomizeConfig.DefaultConfig.enableDamageRanking ? "开启" : "关闭")} → {(config.enableDamageRanking ? "开启" : "关闭")}    ");
            appendFlag = true;
        }

        if (appendFlag)
        {
            sb.Append('\n');
            appendFlag = false;
        }

        return sb.ToString();
    }
}