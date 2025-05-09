using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mirror;
using UnityEngine;


public static class PubBuyManager_ChaosBox
{
    // 混沌盲盒 - 锻体板块
    // 记录每个玩家购买的混沌盲盒数量(小保底计数器)
    public static Dictionary<string, int> ChaosBoxPurchaseCount = new();
    
    // 记录玩家未获得红色混沌的次数(大保底计数器)
    public static Dictionary<string, int> ChaosBoxLegendaryPityCounter = new();

    // 混沌奖励类型枚举
    public enum MiracleRewardType
    {
        //主要
        MaxHealth, //生命
        AttackDamage, // 攻击
        AttackSpeed, // 攻速
        AbilityPower, // 法强
        AbilityHaste, // 冷却
        
        //次要
        AttackRange, // 攻击距离
        MovementSpeed, // 移动速度
        Armor, // 护甲
        Tenacity, // 韧性
        CritChance, // 暴击率
        CritAmp, // 暴击伤害
        MoveCooldown, // 闪避冷却
        Ascension, // 升级
        
        //其他
        Perfection, // 完美
        HealthRegen, // 回血
        fireEffectAmp, // 额外火伤
        coldEffectAmp, // 额外冰伤
        lightEffectAmp, // 额外光伤
        darkEffectAmp, // 额外暗伤
        MoveMaxCharges, // 闪避充能
        MoveRange, // 闪避距离
        RoomMagic, // 场地魔法
    }
    
    private static string GetRarityColor(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => "<color=#E4F0F1>[普通]</color>",
            Rarity.Rare => "<color=#31EFF1>[稀有]</color>",
            Rarity.Epic => "<color=#B654E1>[英雄]</color>",
            Rarity.Legendary => "<color=#EE3830>[传说]</color>",
            _ => ""
        };
    }
    
    public static Dictionary<string, string> roomModName = new Dictionary<string, string> {
        {"RoomMod_ArcticTerritory", "卧槽，冰！"},
        {"RoomMod_AcceleratedTime", "最后再说一遍，时间要开始加速了！"},
        {"RoomMod_DistantMemories", "那一天，人们想起了。"},
        {"RoomMod_EngulfedInFlame", "这不比博燃？"},
        {"RoomMod_GoldEverywhere", "说好模型镀金，你真给我上纯金？"},
        {"RoomMod_GravityTraining", "坂本式缓动！"},
        {"RoomMod_LeafPuppies", "有狗！"},
        {"RoomMod_LingeringAuraOfGuidance", "圣光の诈骗"},
        {"RoomMod_PureDream", "噗噗！列车出发~"},
        {"RoomMod_StarCookie", "V你50买星辰饼干"},
        {"RoomMod_Hunted", "苏醒了,猎杀时刻！"}
    };
    
    //盲盒的价格声明，每周目涨价15.4%
    public static int chaosBoxPrice = (int)(100 *(1+NetworkedManagerBase<ZoneManager>.instance.loopIndex *0.154f) ) ;
    
    // 混沌盲盒处理逻辑
    public static void HandleChaosBox(ChatManager.Message obj, string[] parts)
    {
        DewPlayer player = PubBuyUtil.GetPlayer(obj);
        if (!PubBuyUtil.ValidatePlayer(player, checkAlive: true)) return;
    
        int cost = chaosBoxPrice;
        if (!PubBuyUtil.DeductGold(player, cost, $"购买混沌盲盒需要{cost}金币!")) return;
    
        // 初始化计数器
        if (!ChaosBoxPurchaseCount.ContainsKey(player.name))
        {
            ChaosBoxPurchaseCount[player.name] = 0;
            ChaosBoxLegendaryPityCounter[player.name] = 0;
        }
    
        // 更新计数器
        ChaosBoxPurchaseCount[player.name]++;
        ChaosBoxLegendaryPityCounter[player.name]++;
    
        // 确定品质
        Rarity miracleRarity = DetermineMiracleRarity(player.name);
    
        // 应用混沌奖励
        ApplyMiracleReward(player, miracleRarity);
    }
    
    
    // 批量购买方法
    public static void HandleBulkChaosBox(ChatManager.Message message, string[] parts)
    {
        DewPlayer player = PubBuyUtil.GetPlayer(message);
        if (!PubBuyUtil.ValidatePlayer(player, checkAlive: true)) return;

        string command = parts[0].ToLower();
    
        // 情况1: "++x" 购买指定数量
        if (command.Length > 2 && int.TryParse(command.Substring(2), out int specifiedCount))
        {
            BuySpecifiedCount(player, specifiedCount);
        }
        // 情况2: "+++" 购买最大数量
        else if (command == "+++")
        {
            BuyMaxAffordable(player);
        }
        else if (command == "++dogma")
        {
            // PubBuyManager_ChaosBox_Test.HandleDogmaTest(player);
        }
        // 情况3: "++" 默认购买1个
        else
        {
            HandleChaosBox(message, new string[] { "+" });
        }
    }
    

    

    // 购买指定数量的盲盒
    private static void BuySpecifiedCount(DewPlayer player, int count)
    {
        if (count <= 0)
        {
            PubBuyUtil.BroadcastMessage("购买数量必须大于0!");
            return;
        }

        int totalCost = chaosBoxPrice * count;
        if (!PubBuyUtil.DeductGold(player, totalCost, $"购买{count}个混沌盲盒需要{totalCost}金币!"))
        {
            return;
        }

        // 初始化计数器
        if (!ChaosBoxPurchaseCount.ContainsKey(player.name))
        {
            ChaosBoxPurchaseCount[player.name] = 0;
            ChaosBoxLegendaryPityCounter[player.name] = 0;
        }

        // 记录初始属性
        Hero hero = player.hero;
        var initialStats = new
        {
            MaxHealth = hero.Status.maxHealth,
            AttackDamage = hero.Status.attackDamage,
            AbilityPower = hero.Status.abilityPower,
            AttackSpeed = hero.Status.attackSpeedPercentage,
            AbilityHaste = hero.Status.abilityHaste,
            MovementSpeed = hero.Status.movementSpeedPercentage,
            Armor = hero.Status.armorFromStats,
            AttackRange = hero.Ability?.attackAbility?.configs[0].castMethod._range ?? 0,
            Tenacity = hero.Status.tenacity,
            CritChance = hero.Status.critChance,
            CritAmp = hero.Status.critAmp,
            HealthRegen = hero.Status.healthRegen,
            FireEffectAmp = hero.Status.fireEffectAmp,
            ColdEffectAmp = hero.Status.coldEffectAmp,
            LightEffectAmp = hero.Status.lightEffectAmp,
            DarkEffectAmp = hero.Status.darkEffectAmp,
            MoveCooldown = hero.Skill?.Movement?.configs[0].cooldownTime ?? 0f, 
            MoveMaxCharges = hero.Skill?.Movement?.configs[0].maxCharges ?? 0,
            MoveRange = hero.Skill?.Movement?.configs[0].castMethod._range ?? 0f,
            Level = hero.Status.level, // 当前英雄等级
        };

        // 记录获得的奖励
        int legendaryCount = 0;
        int epicCount = 0;
        int rareCount = 0;
        int commonCount = 0;
        
        // 初始化奖励统计字典
        Dictionary<MiracleRewardType, int> rewards = new Dictionary<MiracleRewardType, int>();

        // 标记是否已经触发过场地魔法
        bool roomMagicTriggered = false;
        // 存储触发的场地魔法名称
        string triggeredRoomMagicName = "";
        
        for (int i = 0; i < count; i++)
        {
            // 更新计数器
            ChaosBoxPurchaseCount[player.name]++;
            ChaosBoxLegendaryPityCounter[player.name]++;

            // 确定品质
            Rarity miracleRarity = DetermineMiracleRarity(player.name);
            
            // 统计品质数量
            switch (miracleRarity)
            {
                case Rarity.Legendary: legendaryCount++; break;
                case Rarity.Epic: epicCount++; break;
                case Rarity.Rare: rareCount++; break;
                case Rarity.Common: commonCount++; break;
            }

            // 应用混沌奖励并记录
            MiracleRewardType rewardType = ApplyMiracleReward(player, miracleRarity, false);
            
            // 如果是场地魔法且已经触发过，则强制改为其他奖励类型
            if (roomMagicTriggered && rewardType == MiracleRewardType.RoomMagic)
            {
                // 获取可用的奖励类型列表（排除场地魔法）
                var availableTypes = GetAvailableRewardTypes(miracleRarity)
                    .Where(t => t != MiracleRewardType.RoomMagic)
                    .ToList();
            
                // 随机选择一个非场地魔法的奖励类型
                rewardType = availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
            }
            else
            {
                rewardType = ApplyMiracleReward(player, miracleRarity, false);
            
                // 如果是场地魔法，标记为已触发
                if (rewardType == MiracleRewardType.RoomMagic)
                {
                    roomMagicTriggered = true;
                    // 获取对应的场地魔法名称
                    List<string> roomMods = roomModName.Keys.ToList();
                    string selectedMod = roomMods[UnityEngine.Random.Range(0, roomMods.Count)];
                    triggeredRoomMagicName = roomModName[selectedMod];
                }
            }
            
            if (rewards.ContainsKey(rewardType))
            {
                rewards[rewardType]++;
            }
            else
            {
                rewards[rewardType] = 1;
            }
        }
        
        // 计算赠送次数
        int freeCount = count / 9;
        if (freeCount > 0)
        {
            // 执行赠送的盲盒
            for (int i = 0; i < freeCount; i++)
            {
                // 更新计数器
                ChaosBoxPurchaseCount[player.name]++;
                ChaosBoxLegendaryPityCounter[player.name]++;

                // 确定品质（赠送的盲盒品质与正常购买相同）
                Rarity miracleRarity = DetermineMiracleRarity(player.name);
            
                // 统计品质数量
                switch (miracleRarity)
                {
                    case Rarity.Legendary: legendaryCount++; break;
                    case Rarity.Epic: epicCount++; break;
                    case Rarity.Rare: rareCount++; break;
                    case Rarity.Common: commonCount++; break;
                }

                // 应用混沌奖励并记录
                MiracleRewardType rewardType = ApplyMiracleReward(player, miracleRarity, false);
                
                    // 如果是场地魔法，则强制改为其他奖励类型
                if (rewardType == MiracleRewardType.RoomMagic)
                {
                    // 获取可用的奖励类型列表（排除场地魔法）
                    var availableTypes = GetAvailableRewardTypes(miracleRarity)
                        .Where(t => t != MiracleRewardType.RoomMagic)
                        .ToList();
            
                    // 随机选择一个非场地魔法的奖励类型
                    rewardType = availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
                }
                
                if (rewards.ContainsKey(rewardType))
                {
                    rewards[rewardType]++;
                }
                else
                {
                    rewards[rewardType] = 1;
                }
            }
        }

        // 计算属性变化
        var statChanges = new
        {
            MaxHealth = hero.Status.maxHealth - initialStats.MaxHealth,
            AttackDamage = hero.Status.attackDamage - initialStats.AttackDamage,
            AbilityPower = hero.Status.abilityPower - initialStats.AbilityPower,
            AttackSpeed = hero.Status.attackSpeedPercentage - initialStats.AttackSpeed,
            AbilityHaste = hero.Status.abilityHaste - initialStats.AbilityHaste,
            MovementSpeed = hero.Status.movementSpeedPercentage - initialStats.MovementSpeed,
            Armor = hero.Status.armorFromStats - initialStats.Armor,
            AttackRange = (hero.Ability?.attackAbility?.configs[0].castMethod._range ?? 0) - initialStats.AttackRange,
            Tenacity = hero.Status.tenacity - initialStats.Tenacity,
            CritChance = hero.Status.critChance - initialStats.CritChance,
            CritAmp = hero.Status.critAmp - initialStats.CritAmp,
            HealthRegen = hero.Status.healthRegen - initialStats.HealthRegen,
            FireEffectAmp = hero.Status.fireEffectAmp - initialStats.FireEffectAmp,
            ColdEffectAmp = hero.Status.coldEffectAmp - initialStats.ColdEffectAmp,
            LightEffectAmp = hero.Status.lightEffectAmp - initialStats.LightEffectAmp,
            DarkEffectAmp = hero.Status.darkEffectAmp - initialStats.DarkEffectAmp,
            MoveCooldown = (hero.Skill?.Movement?.configs[0].cooldownTime ?? 0f) - initialStats.MoveCooldown,
            MoveMaxCharges = (hero.Skill?.Movement?.configs[0].maxCharges ?? 0) - initialStats.MoveMaxCharges,
            MoveRange = (hero.Skill?.Movement?.configs[0].castMethod._range ?? 0f) - initialStats.MoveRange,
            Level = hero.Status.level - initialStats.Level,
        };
        
        // 构建属性变化信息
        string statChangeInfo = "";
        if (statChanges.MaxHealth != 0) statChangeInfo += $"■ 最大生命: {statChanges.MaxHealth:+0;-#}\n";
        if (statChanges.AttackDamage != 0) statChangeInfo += $"■ 攻击: {statChanges.AttackDamage:+0;-#}\n";
        if (statChanges.AbilityPower != 0) statChangeInfo += $"■ 法强: {statChanges.AbilityPower:+0;-#}\n";
        if (statChanges.AttackSpeed != 0) statChangeInfo += $"■ 攻速: {statChanges.AttackSpeed:+0;-#}%\n";
        if (statChanges.AbilityHaste != 0) statChangeInfo += $"■ 冷却缩减: {statChanges.AbilityHaste:+0;-#}\n";
        if (statChanges.MovementSpeed != 0) statChangeInfo += $"■ 移速: {statChanges.MovementSpeed:+0;-#}%\n";
        if (statChanges.Armor != 0) statChangeInfo += $"■ 护甲: {statChanges.Armor:+0;-#}\n";
        if (statChanges.AttackRange != 0) statChangeInfo += $"■ 攻击距离: {statChanges.AttackRange:+0;-#}\n";
        if (statChanges.Tenacity != 0) statChangeInfo += $"■ 韧性: {statChanges.Tenacity:+0;-#}\n";
        if (statChanges.CritChance != 0) statChangeInfo += $"■ 暴击率: {statChanges.CritChance*100:+0.##;-#.##}%\n";
        if (statChanges.CritAmp != 0) statChangeInfo += $"■ 暴击伤害: {statChanges.CritAmp*100:+0.##;-#.##}%\n";
        if (statChanges.HealthRegen != 0) statChangeInfo += $"■ 生命恢复: {statChanges.HealthRegen:+0;-#}\n";
        if (statChanges.FireEffectAmp != 0) statChangeInfo += $"■ 火伤加成: {statChanges.FireEffectAmp*100:+0.##;-#.##}%\n";
        if (statChanges.ColdEffectAmp != 0) statChangeInfo += $"■ 冰伤加成: {statChanges.ColdEffectAmp*100:+0.##;-#.##}%\n";
        if (statChanges.LightEffectAmp != 0) statChangeInfo += $"■ 光伤加成: {statChanges.LightEffectAmp*100:+0.##;-#.##}%\n";
        if (statChanges.DarkEffectAmp != 0) statChangeInfo += $"■ 暗伤加成: {statChanges.DarkEffectAmp*100:+0.##;-#.##}%\n";
        if (statChanges.MoveCooldown != 0) statChangeInfo += $"■ 闪避冷却: {statChanges.MoveCooldown*100:+0.##;-#.##}%\n";
        if (statChanges.MoveMaxCharges != 0) statChangeInfo += $"■ 闪避充能: {statChanges.MoveMaxCharges:+0;-#}\n";
        if (statChanges.MoveRange != 0) statChangeInfo += $"■ 闪避距离: {statChanges.MoveRange:+0.##;-#.##}\n";
        if (statChanges.Level != 0) statChangeInfo += $"■ 飞升等级: {statChanges.Level:+0;-#}\n";
        
        // 发送综合消息
        int totalCount = legendaryCount + epicCount + rareCount + commonCount;

        string legendaryPercent = (totalCount > 0) ? (legendaryCount * 100f / totalCount).ToString("0.0") : "0.0";
        string epicPercent = (totalCount > 0) ? (epicCount * 100f / totalCount).ToString("0.0") : "0.0";
        string rarePercent = (totalCount > 0) ? (rareCount * 100f / totalCount).ToString("0.0") : "0.0";
        string commonPercent = (totalCount > 0) ? (commonCount * 100f / totalCount).ToString("0.0") : "0.0";

        string freeCountMessage = freeCount > 0 ? $" (赠送{freeCount}次)" : "";    

        string roomMagicInfo = roomMagicTriggered ? $"触发场地魔法：【{triggeredRoomMagicName}】" : "";
        
        PubBuyUtil.BroadcastMessage(
            $"{player.name} 批量购买了 {count} 个混沌盲盒{freeCountMessage}!\n" +
            $"品质统计:\n" +
            $"<color=#EE3830>传说</color>: {legendaryCount}({legendaryPercent}%)丨<color=#B654E1>史诗</color>: {epicCount}({epicPercent}%)丨<color=#31EFF1>稀有</color>: {rareCount}({rarePercent}%)丨<color=#E4F0F1>普通</color>: {commonCount}({commonPercent}%)\n" +
            $"属性变化:\n{statChangeInfo}" +
            $"额外:{roomMagicInfo}"
        );
    }

    // 购买当前金币能买的最大数量
    private static void BuyMaxAffordable(DewPlayer player)
    {
        int maxAffordable = player.gold / 100;
        if (maxAffordable <= 0)
        {
            PubBuyUtil.BroadcastMessage("你的金币不足以购买锻体!");
            return;
        }
        // 添加反馈消息
        PubBuyUtil.BroadcastMessage($"{player.name} 的金币最多可以购买 {maxAffordable} 个混沌盲盒，正在购买...");

        // 调用批量购买方法
        BuySpecifiedCount(player, maxAffordable);
    }

    // 确定混沌品质
    private static Rarity DetermineMiracleRarity(string playerName)
    {
        int purchaseCount = ChaosBoxPurchaseCount[playerName];
        int pityCount = ChaosBoxLegendaryPityCounter[playerName];
    
        // 大保底机制
        if (pityCount >= 13)
        {
            ChaosBoxLegendaryPityCounter[playerName] = 0;
            ChaosBoxPurchaseCount[playerName] = 0;
            return Rarity.Legendary;
        }
    
        // 中保底机制（每6次）
        if (purchaseCount >= 6)
        {
            ChaosBoxPurchaseCount[playerName] = 0;
            return UnityEngine.Random.value < 0.83f ? Rarity.Epic : Rarity.Legendary;
        }
    
        // 常规概率
        // float roll = UnityEngine.Random.value;
        // if (roll < 0.21f) return Rarity.Common;
        // if (roll < 0.7f) return Rarity.Rare;
        // if (roll < 0.9f) return Rarity.Epic;
        // return Rarity.Legendary;
        float roll = UnityEngine.Random.value;
        // 15.4% 普通品质
        if (roll < 0.154f) return Rarity.Common;
        // 54.6% 稀有品质
        if (roll < 0.700f) return Rarity.Rare;
        // 23.4% 英雄品质
        if (roll < 0.934f) return Rarity.Epic;
        // 6.6%  传说品质
        return Rarity.Legendary;                      
    }

    // 应用混沌奖励并返回奖励类型
    // private static MiracleRewardType ApplyMiracleReward(DewPlayer player, Rarity rarity, bool showMessage = true)
    public static MiracleRewardType ApplyMiracleReward(
        DewPlayer player, 
        Rarity rarity, 
        bool showMessage = true,    
        MiracleRewardType? forceType = null)
    {
        // 获取品质系数
        float rarityMultiplier = GetRarityMultiplier(rarity);
        
        MiracleRewardType rewardType;
        if (forceType.HasValue)
        {
            rewardType = forceType.Value;
        }
        else
        {
            var availableRewards = GetAvailableRewardTypes(rarity);
            rewardType = availableRewards[UnityEngine.Random.Range(0, availableRewards.Count)];
        }

        Hero hero = player.hero;
        if (hero == null || hero.isDead) return rewardType;

        switch (rewardType)
        {
            // 主要奖励
            case MiracleRewardType.MaxHealth:
                float healthBonus = 400 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { maxHealthFlat = healthBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}生命 +{healthBonus}");
                break;

            case MiracleRewardType.AttackDamage:
                float adBonus = 27 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { attackDamageFlat = adBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}攻击 +{adBonus}");
                break;

            case MiracleRewardType.AttackSpeed:
                float atkSpeedBonus = 30 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { attackSpeedPercentage = atkSpeedBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}攻速 +{atkSpeedBonus}%");
                break;

            case MiracleRewardType.AbilityPower:
                float apBonus = 20 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { abilityPowerFlat = apBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}法强 +{apBonus}");
                break;

            case MiracleRewardType.AbilityHaste:
                float hasteBonus = 30 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { abilityHasteFlat = hasteBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}冷却缩减 +{hasteBonus}");
                break;
                
            case MiracleRewardType.MovementSpeed:
                float speedBonus = 24 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { movementSpeedPercentage = speedBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}移动速率 +{speedBonus}");
                break;

            case MiracleRewardType.Armor:
                float armorBonus = 40 * rarityMultiplier;
                hero.Status.AddStatBonus(new StatBonus { armorFlat = armorBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}护甲 +{armorBonus}");
                break;

            // 次要奖励
            case MiracleRewardType.AttackRange:
                int rangeBonus = rarity == Rarity.Legendary ? 2 : 1;
                if (hero.Ability?.attackAbility != null)
                {
                    hero.Ability.attackAbility.configs[0].castMethod._range += rangeBonus;
                    hero.Ability.attackAbility.configs[1].castMethod._range += rangeBonus;
                    hero.Ability.attackAbility.SyncCastMethodChanges(0);
                    hero.Ability.attackAbility.SyncCastMethodChanges(1);
                }
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}攻击距离 +{rangeBonus}");
                break;
                
            case MiracleRewardType.Tenacity:
                int tenacityBonus = rarity == Rarity.Legendary ? 42 : 31;
                hero.Status.AddStatBonus(new StatBonus { tenacityFlat = tenacityBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}韧性 +{tenacityBonus}");
                break;
                
            case MiracleRewardType.CritChance:
                float critChanceBonus = rarity == Rarity.Legendary ? 0.16f : 0.08f;
                hero.Status.AddStatBonus(new StatBonus { critChanceFlat = critChanceBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}暴击率 +{critChanceBonus*100}%");
                break;
        
            case MiracleRewardType.CritAmp:
                float critAmpBonus = rarity == Rarity.Legendary ? 0.24f : 0.12f;
                hero.Status.AddStatBonus(new StatBonus { critAmpFlat = critAmpBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}暴击伤害 +{critAmpBonus*100}%");
                break;
                
            case MiracleRewardType.MoveCooldown:
                float moveCooldownBonus = rarity == Rarity.Legendary ? 0.16f : 0.08f;
                if (hero.Skill.Movement != null && hero.Skill.Movement.configs[0].cooldownTime > 0)
                {
                    hero.Skill.Movement.configs[0].cooldownTime -= moveCooldownBonus;
                    // hero.Skill.Movement.configs[1].cooldownTime -= moveCooldownBonus;
                    // hero.Skill.Movement.SyncCastMethodChanges(0);
                    // hero.Skill.Movement.SyncCastMethodChanges(1);
                }
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}闪避冷却 -{moveCooldownBonus*100}%");
                break;
            
            case MiracleRewardType.Ascension:
                int ascensionBonus = rarity == Rarity.Legendary ? 3 : 1;
                if (hero.level <= 30)
                {
                    hero.Status.level += ascensionBonus;
                }
                else
                {
                    NetworkedManagerBase<GameManager>.instance.gss.maxHeroLevel += ascensionBonus;
                    hero.Status.level += ascensionBonus;
                }
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}飞升！等级+{ascensionBonus}");
                break;

            // 其他奖励
            case MiracleRewardType.Perfection:
                int quality = rarity == Rarity.Legendary ? 80 : 160;
                Vector3 spawnPos = hero.agentPosition + 
                                 (Rift.instance.transform.position - hero.agentPosition).normalized * 2.5f;
                Dew.CreateGem<Gem>(
                    DewResources.GetByShortTypeName<Gem>("Gem_L_Perfect"), 
                    spawnPos, 
                    quality, 
                    player
                );
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}获得完美宝石（品质 {quality}）!");
                break;
                
            case MiracleRewardType.HealthRegen:
                int healthRegenBonus = rarity == Rarity.Legendary ? 32 : 16;
                hero.Status.AddStatBonus(new StatBonus { healthRegenFlat = healthRegenBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}生命恢复 +{healthRegenBonus}");
                break;
                
            case MiracleRewardType.fireEffectAmp:
                float fireEffectAmpBonus = rarity == Rarity.Legendary ? 0.36f : 0.18f;
                hero.Status.AddStatBonus(new StatBonus { fireEffectAmpFlat = fireEffectAmpBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}火伤加成 +{fireEffectAmpBonus*100}%");
                break;
        
            case MiracleRewardType.coldEffectAmp:
                float coldEffectAmpBonus = rarity == Rarity.Legendary ? 0.16f : 0.08f;
                hero.Status.AddStatBonus(new StatBonus { coldEffectAmpFlat = coldEffectAmpBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}冰伤加成 +{coldEffectAmpBonus*100}%");
                break;
        
            case MiracleRewardType.lightEffectAmp:
                float lightEffectAmpBonus = rarity == Rarity.Legendary ? 0.24f : 0.12f;
                hero.Status.AddStatBonus(new StatBonus { lightEffectAmpFlat = lightEffectAmpBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}光伤加成 +{lightEffectAmpBonus*100}%");
                break;
        
            case MiracleRewardType.darkEffectAmp:
                float darkEffectAmpBonus = rarity == Rarity.Legendary ? 0.24f : 0.12f;
                hero.Status.AddStatBonus(new StatBonus { darkEffectAmpFlat = darkEffectAmpBonus });
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}暗伤加成 +{darkEffectAmpBonus*100}%");
                break;
            
            case MiracleRewardType.MoveMaxCharges:
                int moveMaxChargesBonus = rarity == Rarity.Legendary ? 1 : 1;
                if (hero.Skill.Movement != null && hero.Skill.Movement.configs[0].cooldownTime > 0)
                {
                    hero.Skill.Movement.configs[0].maxCharges += moveMaxChargesBonus;
                    // hero.Skill.Movement.configs[1].maxCharges += moveMaxChargesBonus;
                    // hero.Skill.Movement.SyncCastMethodChanges(0);
                    // hero.Skill.Movement.SyncCastMethodChanges(1);
                }
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}闪避充能 +{moveMaxChargesBonus}");
                break;
            
            case MiracleRewardType.MoveRange: 
                float moveRangeBonus = rarity == Rarity.Legendary ? 1.6f : 0.8f; 
                if (hero.Skill.Movement != null && hero.Skill.Movement.configs[0].castMethod._range > 0) 
                { 
                    hero.Skill.Movement.configs[0].castMethod._range += moveRangeBonus; 
                    // hero.Skill.Movement.configs[1].castMethod._range += moveRangeBonus; 
                    hero.Skill.Movement.SyncCastMethodChanges(0); 
                    // hero.Skill.Movement.SyncCastMethodChanges(1);
                    
                }
                if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}闪避距离 +{moveRangeBonus}");
                break;
            
            case MiracleRewardType.RoomMagic:
                List<string> roomMods = roomModName.Keys.ToList();
                if (NetworkedManagerBase<ZoneManager>.instance != null) {
                    // 随机选择一个Mod
                    string selectedMod = roomMods[UnityEngine.Random.Range(0, roomMods.Count)];
            
                    // 添加Mod
                    NetworkedManagerBase<ZoneManager>.instance.AddModifier(
                        NetworkedManagerBase<ZoneManager>.instance.currentNodeIndex, 
                        new ModifierData {
                            type = selectedMod,
                            clientData = null
                        });
                    
                    if(showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}发动场地魔法：【{roomModName[selectedMod]}】");
                }
                break;
        }
                
            
            // case MiracleRewardType.MoveDuration:
            //     float moveDurationBonus = rarity == Rarity.Legendary ? 4f : 2f; 
            //     if (hero.Skill?.Movement != null)
            //     {
            //        // 闪避速度最基础为12f，
            //     }
            //     if (showMessage) PubBuyUtil.BroadcastMessage($"{GetRarityColor(rarity)}闪避速度 +{moveDurationBonus}");
            //     break;

        return rewardType;
    }

    // 辅助方法
    private static float GetRarityMultiplier(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => 0.4f,
            Rarity.Rare => 0.5f,
            Rarity.Epic => 0.6f,
            Rarity.Legendary => 0.8f,
            _ => 0.4f
        };
    }
        
    private static List<MiracleRewardType> GetAvailableRewardTypes(Rarity rarity) 
    {
        var rewards = new List<MiracleRewardType>();

        // 主要奖励类型（57%概率）
        var primaryTypes = new List<MiracleRewardType>
        {
            MiracleRewardType.MaxHealth,
            MiracleRewardType.AttackDamage,
            MiracleRewardType.AttackSpeed,
            MiracleRewardType.AbilityPower,
            MiracleRewardType.AbilityHaste
        };
        foreach (var type in primaryTypes)
        {
            rewards.Add(type);
            rewards.Add(type); 
            rewards.Add(type);
        }

        // 次要奖励类型（29%概率）
        var secondaryTypes = new List<MiracleRewardType>
        {
            MiracleRewardType.MovementSpeed,
            MiracleRewardType.Armor
        };
        if (rarity >= Rarity.Epic)
        {
            secondaryTypes.Add(MiracleRewardType.AttackRange);
            secondaryTypes.Add(MiracleRewardType.Tenacity);
            secondaryTypes.Add(MiracleRewardType.CritChance);
            secondaryTypes.Add(MiracleRewardType.CritAmp);
            secondaryTypes.Add(MiracleRewardType.MoveCooldown);
            secondaryTypes.Add(MiracleRewardType.Ascension);
        }
        if (rarity == Rarity.Legendary)
        {
            // 仅在传说品质时添加
        }
        foreach (var type in secondaryTypes)
        {
            rewards.Add(type);
            rewards.Add(type); 
        }

        // 其他奖励（14%概率）
        if (rarity == Rarity.Common || rarity == Rarity.Legendary)
        {
            rewards.Add(MiracleRewardType.RoomMagic);
        }
        if (rarity >= Rarity.Epic)
        {
            rewards.Add(MiracleRewardType.Perfection);
            rewards.Add(MiracleRewardType.HealthRegen);
            rewards.Add(MiracleRewardType.fireEffectAmp);
            rewards.Add(MiracleRewardType.coldEffectAmp);
            rewards.Add(MiracleRewardType.lightEffectAmp);
            rewards.Add(MiracleRewardType.darkEffectAmp);
            rewards.Add(MiracleRewardType.MoveRange);
        }
        if (rarity == Rarity.Legendary)
        {
            rewards.Add(MiracleRewardType.MoveMaxCharges);
        }

        
        return rewards; 
    }
}

