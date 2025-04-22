using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Mirror;
using UnityEngine;

public class AttrCustomizeManager
{
    private static int _lastObliviaxQuestZoneIndex = -100;
    private static bool _globalOnceFlag = false;

    private class Ad_MonsterLevelUp
    {
        public StatBonus bonus;
    }

    public static void ExecuteGlobalOnce()
    {
        _globalOnceFlag = true;
        ModifyBeneficialNodeMultiplier();
        ModifyHealRawMultiplier();
    }

    public static void ExecuteInGameOnce()
    {
        if (!_globalOnceFlag)
        {
            ExecuteGlobalOnce();
        }

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
            dewDifficultySettings.healRawMultiplier = AttrCustomizeResources.Config.healRawMultiplier *
                                                      dewDifficultySettings.healRawMultiplier;
        }
    }

    private static void ModifyBeneficialNodeMultiplier()
    {
        IEnumerable<DewDifficultySettings> difficultySettingsEnumerable =
            DewResources.FindAllByNameSubstring<DewDifficultySettings>("diff");
        foreach (var dewDifficultySettings in difficultySettingsEnumerable)
        {
            dewDifficultySettings.beneficialNodeMultiplier = AttrCustomizeResources.Config.beneficialNodeMultiplier *
                                                             dewDifficultySettings.beneficialNodeMultiplier;
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
            ConsoleCommands.WorldReveal();
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
                sb.AppendLine("伤害排行");

                // 使用 for 循环遍历伤害列表
                for (int j = 0; j < dmgList.Count; j++)
                {
                    ValueTuple<string, float, float> valueTuple = dmgList[j];
                    string playerProfileName2 = valueTuple.Item1;
                    float totalDmg2 = valueTuple.Item2;
                    float maxDmg2 = valueTuple.Item3;
                    string totalDmgFormatted = totalDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                    string maxDmgFormatted = maxDmg2.ToString("#,0", CultureInfo.InvariantCulture);
                    sb.AppendLine(playerProfileName2 + ": 总伤害 " + totalDmgFormatted + " | 最强一击 " + maxDmgFormatted);
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

    public static bool TryGetNodeIndexForNextGoal(DewQuest quest, GetNodeIndexSettings s, out int nodeIndex)
    {
        ZoneManager zoneManager = NetworkedManagerBase<ZoneManager>.instance;
        int currentNodeIndex = zoneManager.currentNodeIndex;
        SyncList<WorldNodeData> nodes = zoneManager.nodes;
        int exitNodeIndex = 0;
        for (int j = 0; j < nodes.Count; j++)
        {
            if (nodes[j].type == WorldNodeType.ExitBoss)
            {
                exitNodeIndex = j;
                break;
            }
        }

        int currentDistToExit = zoneManager.GetNodeDistance(currentNodeIndex, exitNodeIndex);
        int index = (nodeIndex = Dew.SelectBestIndexWithScore(nodes, GetScore));
        return GetScore(nodes[index], index) > -5000f;

        float GetScore(WorldNodeData data, int i)
        {
            float score = 0f;
            if (data.IsSidetrackNode())
            {
                score -= 10000f;
            }

            if (!s.allowedTypes.Contains(data.type))
            {
                score -= 10000f;
            }

            if (i == currentNodeIndex)
            {
                if (AttrCustomizeResources.Config.enableCurrentNodeGenerateLostSoul && quest is Quest_LostSoul)
                {
                    score -= -11000f;
                }
                else
                {
                    score -= 10000f;
                }
            }

            switch (data.status)
            {
                case WorldNodeStatus.Revealed:
                case WorldNodeStatus.RevealedFull:
                    score -= 2.5f;
                    break;
                case WorldNodeStatus.HasVisited:
                    score -= 10000f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                case WorldNodeStatus.Unexplored:
                    break;
            }

            int nodeDist = zoneManager.GetNodeDistance(currentNodeIndex, i);
            if (nodeDist < s.desiredDistance.x)
            {
                score -= (float)(s.desiredDistance.x - nodeDist) * 1f;
            }

            score = ((nodeDist <= s.desiredDistance.y)
                ? (score + 5f)
                : (score - (float)(nodeDist - s.desiredDistance.y) * 1f));
            if (s.preferCloserToExit)
            {
                int delta = currentDistToExit - zoneManager.GetNodeDistance(i, exitNodeIndex);
                score = ((delta <= 0) ? (score + (float)delta * 3f) : (score + (float)delta * 0.75f));
            }

            if (s.preferNoMainModifier && nodes[i].HasMainModifier())
            {
                score -= 6f;
            }

            return score + global::UnityEngine.Random.Range(-1.5f, 1.5f);
        }
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

        string startGameNotice = @$"

您正在游玩自定制版v0.2.4 改动内容如下

房间最大人数: {config.maxPlayer}  
所有敌人移动速度百分比: {config.enemyMovementSpeedPercentage}  所有敌人攻击速度百分比: {config.enemyAttackSpeedPercentage}  所有敌人技能加速百分比: {config.enemyAbilityHasteFlat}  
boss生命百分比: {config.bossHealthMultiplier}  boss伤害百分比: {config.bossDamageMultiplier}  
miniBoss生命百分比: {config.miniBossHealthMultiplier}  miniBoss伤害百分比: {config.miniBossDamageMultiplier}  
小怪生命百分比: {config.littleMonsterHealthMultiplier}  小怪伤害百分比: {config.littleMonsterDamageMultiplier}  
额外生命成长倍率(每关成长): {config.extraHealthGrowthMultiplier}  额外伤害成长倍率(每关成长): {config.extraDamageGrowthMultiplier}  
Q技能精华槽数量: {config.skillQGemCount}  W技能精华槽数量: {config.skillWGemCount}  E技能精华槽数量: {config.skillEGemCount}  R技能精华槽数量: {config.skillRGemCount}  身份技能精华槽数量: {config.skillIdentityGemCount}  位移技能精华槽数量: {config.skillMovementGemCount}  
商店增加物品数量: {config.shopAddedItems}  商店刷新次数: {config.shopRefreshes}  
boss数量: {config.bossCount}  每周目添加boss数量: {config.bossCountAddByLoop}  每关添加boss数量: {config.bossCountAddByZone}  所有boss一次性生成: {config.enableBossSpawnAllOnce}  
boss幻想化概率: {config.bossMirageChance}  boss猎手化概率: {config.bossHunterChance}  小怪产生紫皮概率倍数:{config.monsterMirageChanceMultiple}  
人口过剩人口倍数: {config.maxAndSpawnedPopulationMultiplier}  引导祭坛数量倍率: {config.beneficialNodeMultiplier}  治疗效果百分比:{config.healRawMultiplier}  
开局发放技能: {startSkillsText}  
开局发放精华: {startGemsText}  
移除技能: {removeSkillsText}  
移除精华: {removeGemsText}  
转职: {config.enableHeroSkillAddShop}  薄雾全方位招架: {config.enableMistAllowAnyDirection}  
未访问过的图发钱数量: {config.firstVisitDropGoldCount}  未访问过的图发钱数量每周目添加数量: {config.firstVisitDropGoldCountAddByLoop}  未访问过的图发钱数量每关添加数量: {config.firstVisitDropGoldCountAddByZone}  
当前节点生成迷失灵魂: {config.enableCurrentNodeGenerateLostSoul}  Boss房生成迷失灵魂: {config.enableBossRoomGenerateLostSoul}  
遗物任务: {config.enableArtifactQuest}  光辉BOSS任务: {config.enableFragmentOfRadianceBossQuest}  遗忘猎手任务可重复生成: {config.enableQuestHuntedByObliviaxRepeatable}  
幻想上限每周关增加: {config.enableHealthReduceMultiplierAddByZone}  Boss单次受伤血量百分比(限伤):{config.bossSingleInjuryHealthMultiplier}
清醒梦 拥抱死亡 (所有角色的伤害量增加100%): {config.enableLucidDreamEmbraceMortality}  清醒梦 一路顺风 (猎手不再追踪探险队): {config.enableLucidDreamBonVoyage}
清醒梦 剧痛伤口 (旅行者接受的治疗和护盾减少50%): {config.enableLucidDreamGrievousWounds}  清醒梦 极暗冲动 (所有角色的无法识别敌我) : {config.enableLucidDreamTheDarkestUrge}
清醒梦 野性本能 (除首领外所有怪物都将作为猎手被召唤): {config.enableLucidDreamWild}  清醒梦 疯狂生涯 (所有怪物都能更精准的预测旅行者的移动): {config.enableLucidDreamMadLife}  
清醒梦 泡影浮梦 (生命药水和引导祭坛不再恢复生命值,而是提供梦尘): {config.enableLucidDreamSparklingDreamFlask}  
每关发送伤害排行榜: {config.enableDamageRanking}  

声明:本改包仅供个人测试使用，禁止用于商业用途，禁止用于非法用途，禁止用于破坏游戏平衡性。因使用本改包所造成的任何后果与作者无关。
关注qq联机/改包教程群 1034195007
";

        return startGameNotice;
    }
}