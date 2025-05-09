using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Mirror;
using UnityEngine;  

public class PubBuyManager
{
    // 映射玩家名称到其生成的实体
    public static Dictionary<string, Dictionary<string, Entity>> SpawnMap = new();

    // 映射输入按键到实体名称
    public static Dictionary<string, string> SpawnKeyEntityNameMap = new()
    {
        { "1", "Hero_Lacerta" },
        { "2", "Hero_Mist" },
        { "3", "Hero_Yubar" },
        { "4", "Hero_Vesper" },
        { "5", "Hero_Aurena" },
        { "6", "Mon_Forest_BossDemon" },
        { "7", "Mon_SnowMountain_BossSkoll" },
        { "8", "Mon_Sky_BossNyx" },
        { "9", "Mon_Special_BossLightElemental" },
        { "0", "Mon_Special_BossObliviax" },
        { "-", "PropEnt_Merchant_Jonas" },
    };
    

    // 验证聊天输入并处理命令
    public static void ValidateInput(ChatManager.Message obj)
    {
        string content = obj.content;
        if (content.StartsWith("<noparse>") && content.EndsWith("</noparse>") &&
            content.Length >= "<noparse>".Length + "</noparse>".Length)
        {
            string text = content.Substring("<noparse>".Length,
                content.Length - "<noparse>".Length - "</noparse>".Length);
            string[] array = text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!(text != string.Join(" ", array)) && array.Length != 0)
            {
                HandleCommand(obj, array);
            }
        }
    }

    // 路由命令到相应的处理程序
    private static void HandleCommand(ChatManager.Message obj, string[] parts)
    {
        string text = parts[0].ToLower();
        int num = parts.Length;

        if (text == "b" && num >= 2) // 购买命令
        {
            HandleBuy(obj, parts);
        }
        else if (text == "s" && num >= 2) // 切换命令
        {
            HandleSwitch(obj, parts);
        }
        else if (text == "c" && num >= 2) // 控制命令
        {
            HandleControl(obj, parts);
        }
        else if (text == "r") // 重置视角命令
        {
            HandleResetAngleOfView(obj, parts);
        }
        else if (text == "d") // 销毁命令
        {
            HandleDestroy(obj, parts);
        }
        else if (text == "h") // 帮助命令
        {
            HandleHelp(obj);
        }
        else if (text == "p") // 月球商店命令
        {
            HandleMoonShopHelp(obj);
        }
        else if (text == "pb") // 月球商店命令
        {
            HandleMoonShopBuy(obj, parts);
        }
        else if (text == "cs10086")
        {
            // HandleCheat(obj);
        }
        else if (text == "+" || text.StartsWith("++") || text == "+i") // 锻体
        {
            // if (AttrCustomizeResources.Config.enablePubBuyManager)
            // {
                if (text == "+")
                {
                    // 单次快速购买
                    PubBuyManager_ChaosBox.HandleChaosBox(obj, parts);
                }
                if (text == "+i")
                {
                    ChaosBoxHelp(obj);
                }
                else 
                {
                    // 批量购买
                    PubBuyManager_ChaosBox.HandleBulkChaosBox(obj, parts);
                }
                
            // }
        }
        else if (text == "i") // 显示当前属性命令
        {
            HandleShowStats(obj);
        }
    }
    
    //处理测试用作弊码命令
    private static void HandleCheat(ChatManager.Message obj)
    {
        DewPlayer player = PubBuyUtil.GetPlayer(obj);
        if (PubBuyUtil.DeductGold(PubBuyUtil.GetPlayer(obj), -999999, "你连一块都没有...看这个又有什么用呢?"))
        {
            PubBuyUtil.BroadcastMessage(
                @"启动测试用作弊码");
        }
    }

    // 销毁玩家所有拥有的实体
    // 处理销毁消息
    public static void HandleDestroy(ChatManager.Message message, string[] parts)
    {
        // 获取消息发送者
        DewPlayer dewPlayer = PubBuyUtil.GetPlayer(message);
        HandleDestroy(dewPlayer);
    }

    public static void HandleDestroy(DewPlayer dewPlayer)
    {
        if (dewPlayer == null) return;

        if (SpawnMap.TryGetValue(dewPlayer.name, out var dictionary))
        {
            for (var i = 0; i < dictionary.Values.Count; i++)
            {
                dictionary.Values.ElementAt(i).Destroy();
                dictionary.Remove(dictionary.Keys.ElementAt(i));
            }
        }

        // 重置混沌盲盒计数器
        PubBuyManager_ChaosBox.ChaosBoxPurchaseCount[dewPlayer.name] = 0;
        PubBuyManager_ChaosBox.ChaosBoxLegendaryPityCounter[dewPlayer.name] = 0;
    }

    // 重置玩家视角到其英雄
    private static void HandleResetAngleOfView(ChatManager.Message message, string[] parts)
    {
        DewPlayer dewPlayer = PubBuyUtil.GetPlayer(message);
        if (dewPlayer != null)
        {
            dewPlayer.controllingEntity = dewPlayer.hero;
        }
    }

    // 处理控制已生成实体的命令
    private static void HandleControl(ChatManager.Message message, string[] parts)
    {
        DewPlayer dewPlayer = PubBuyUtil.GetPlayer(message);
        if (!PubBuyUtil.ValidatePlayer(dewPlayer, checkAlive: true))
        {
            return;
        }

        string text = parts[1].ToLower();
        if (text.Length != 1)
        {
            return;
        }

        if (SpawnKeyEntityNameMap.TryGetValue(text, out var entityName))
        {
            HandleEntityControl(dewPlayer, entityName);
            return;
        }

        PubBuyUtil.BroadcastMessage("你应该重新选择一下!");
    }

    // 控制特定实体
    private static void HandleEntityControl(DewPlayer dewPlayer, string entityName)
    {
        if (SpawnMap.TryGetValue(dewPlayer.name, out var playerSpawnMap))
        {
            if (playerSpawnMap.TryGetValue(entityName, out Entity existingEntity))
            {
                dewPlayer.controllingEntity = existingEntity;
                return;
            }
        }

        PubBuyUtil.BroadcastMessage("老伙计,你是不是应该先雇佣这位呢!");
    }

    // 处理切换英雄的命令
    private static void HandleSwitch(ChatManager.Message message, string[] parts)
    {
        DewPlayer dewPlayer = PubBuyUtil.GetPlayer(message);
        if (!PubBuyUtil.ValidatePlayer(dewPlayer, checkAlive: true))
        {
            return;
        }

        string text = parts[1].ToLower();
        if (text.Length != 1)
        {
            return;
        }

        if (SpawnKeyEntityNameMap.TryGetValue(text, out var entityName))
        {
            HandleHeroSwitch(dewPlayer, entityName);
            return;
        }

        PubBuyUtil.BroadcastMessage("你应该重新选择一下!");
    }

    // 将玩家英雄切换到不同类型
    private static void HandleHeroSwitch(DewPlayer dewPlayer, string entityName)
    {
        int num = 500;
        if (!PubBuyUtil.DeductGold(dewPlayer, num, $"老伙计,我知道你换身份是干什么用的,但没有钱是绝对不行的! 需要{num}金币"))
        {
            return;
        }

        Hero sourceHero = dewPlayer.hero;
        Hero entity = DewResources.FindOneByTypeSubstring<Hero>(entityName);
        if (entity == null)
        {
            PubBuyUtil.BroadcastMessage("你应该重新选择一下!");
            return;
        }

        ConsoleCommands.DebugGameSave.HeroData data = new ConsoleCommands.DebugGameSave.HeroData(sourceHero);

        Hero hero = Dew.SpawnHero(entity, sourceHero.position, sourceHero.rotation, sourceHero.owner,
            sourceHero.level, null);
        Dew.Destroy(sourceHero.gameObject);

        dewPlayer.hero = hero;
        dewPlayer.controllingEntity = hero;
        hero.Status.SetHealth(data.heroHealth);
        hero.StartCoroutine(Routine());

        // 协程处理技能和宝石转移
        IEnumerator Routine()
        {
            yield return null;
            yield return null;
            if (data.chaosStats != null)
            {
                hero.AddData(new Shrine_Chaos.Ad_ChaosStats
                {
                    bonus = data.chaosStats
                });
                hero.Status.AddStatBonus(data.chaosStats);
            }

            foreach (ConsoleCommands.DebugGameSave.SkillData s in data.skills)
            {
                if (s.location == HeroSkillLocation.Movement)
                {
                    continue;
                }

                SkillTrigger newSkillPrefab = DewResources.GetByShortTypeName<SkillTrigger>(s.name);

                if (newSkillPrefab == null)
                {
                    Debug.LogWarning("替换技能 " + s.name + " 不可用");
                }
                else
                {
                    if (hero.Ability.abilities.ContainsKey((int)s.location))
                    {
                        hero.Skill.UnequipSkill(s.location, hero.position, ignoreCanReplace: true).Destroy();
                    }

                    SkillTrigger newSkill = Dew.CreateSkillTrigger(newSkillPrefab, hero.position, s.level);
                    hero.Skill.EquipSkill(s.location, newSkill, ignoreCanReplace: true);
                }
            }

            foreach (ConsoleCommands.DebugGameSave.GemData g in data.gems)
            {
                Gem newGemPrefab = DewResources.GetByShortTypeName<Gem>(g.name);
                if (newGemPrefab == null)
                {
                    Debug.LogWarning("宝石 " + g.name + " 不可用");
                }
                else
                {
                    Gem newGem = Dew.CreateGem(newGemPrefab, hero.position, g.quality);
                    hero.Skill.EquipGem(g.location, newGem);
                }
            }
        }
    }

    // 处理购买实体的命令
    private static void HandleBuy(ChatManager.Message obj, string[] parts)
    {
        DewPlayer dewPlayer = PubBuyUtil.GetPlayer(obj);
        if (!PubBuyUtil.ValidatePlayer(dewPlayer, checkAlive: true))
        {
            return;
        }

        string text = parts[1].ToLower();
        if (text.Length != 1)
        {
            return;
        }

        if (SpawnKeyEntityNameMap.TryGetValue(text, out var entityName))
        {
            HandleEntitySpawn(dewPlayer, entityName);
            return;
        }

        PubBuyUtil.BroadcastMessage("酒馆没有这个佣兵,你到别处看看吧!");
    }

    // 为玩家生成实体
    private static void HandleEntitySpawn(DewPlayer dewPlayer, string entityName)
    {
        // 如果不存在则初始化玩家的生成映射
        if (!SpawnMap.ContainsKey(dewPlayer.name))
        {
            SpawnMap[dewPlayer.name] = new Dictionary<string, Entity>();
        }

        Dictionary<string, Entity> playerSpawnMap = SpawnMap[dewPlayer.name];

        // 如果存在则移除现有实体
        if (playerSpawnMap.TryGetValue(entityName, out Entity existingEntity))
        {
            existingEntity.Destroy();
            playerSpawnMap.Remove(entityName);
        }

        // 生成新实体
        Entity entity = DewResources.FindOneByTypeSubstring<Entity>(entityName);
        Vector3 position = dewPlayer.hero.transform.position;
        Quaternion spawnRot = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
        Entity newEntity = Dew.SpawnEntity(
            entity,
            position,
            spawnRot,
            null,
            dewPlayer,
            dewPlayer.hero.level
        );

        // 根据不同实体类型处理相应的成本和加成
        if (newEntity is Hero)
        {
            int num = 150;
            if (!PubBuyUtil.DeductGold(dewPlayer, num, $"老伙计,你是不是应该先攒点钱! 需要{num}金币"))
            {
                newEntity.Destroy();
                return;
            }

            newEntity.Status.baseStats.maxHealth *= 5f;
            newEntity.Status.baseStats.attackDamage *= 2f;
            newEntity.Status.baseStats.abilityPower *= 2f;
            newEntity.Status.level++;
            PubBuyUtil.BroadcastMessage("你的选择一如既往的明智!");
        }
        else if (newEntity is Monster)
        {
            int num = 300;
            if (!PubBuyUtil.DeductGold(dewPlayer, num, $"要想雇佣这位你可要掂量掂量钱袋子! 需要{num}金币"))
            {
                newEntity.Destroy();
                return;
            }

            newEntity.Status.baseStats.maxHealth /= 10f;
            newEntity.Status.baseStats.attackDamage *= 2f;
            newEntity.Status.baseStats.abilityPower *= 2f;
            PubBuyUtil.BroadcastMessage("BOSS是很强力的佣兵,真的!");
        }
        else if (newEntity is PropEnt_Merchant_Jonas)
        {
            int num = (int)(998.0 * (0.615 + (double)DewPlayer.humanPlayers.Count * 0.385));
            if (!PubBuyUtil.DeductGold(dewPlayer, num, $"商人要{num}块的出场费！"))
            {
                newEntity.Destroy();
                return;
            }

            PubBuyUtil.BroadcastMessage("乔纳斯会出现在任何地方,前提是有利可图!");
        }

        playerSpawnMap.Add(entityName, newEntity);
    }

    //月球商店
    public enum MoonShopRewardType
    {
        HooksofHeresy,
        LightFluxPauldron,
        StoneFluxPauldron,
        // Transcendence
    }
    // 处理购买塑形玻璃的命令
    private static void HandleMoonShopBuy(ChatManager.Message obj, string[] parts)
    {
        DewPlayer player = PubBuyUtil.GetPlayer(obj);
        if (!PubBuyUtil.ValidatePlayer(player, checkAlive: true))
        {
            return;
        }
        
        int cost = 648;
        if (!PubBuyUtil.DeductGold(player, cost, $"抽取月球装备需要{cost}金币!"))
        {
            return;
        }
        ApplyMoonShopReward(player);
    }

    // 记录玩家已获得的月球道具类型
    public static Dictionary<string, HashSet<MoonShopRewardType>> PlayerMoonEquips = new();
    private static void ApplyMoonShopReward(DewPlayer player)
    {
        // 动态获取 MoonShopRewardType 的枚举总数
        int rewardTypeCount = Enum.GetValues(typeof(MoonShopRewardType)).Length;
        MoonShopRewardType rewardType = (MoonShopRewardType)UnityEngine.Random.Range(0, rewardTypeCount);
        
        Hero hero = player.hero;
        
        // 初始化装备记录
        if (!PlayerMoonEquips.ContainsKey(player.name)) 
        {
            PlayerMoonEquips[player.name] = new HashSet<MoonShopRewardType>();
        }
        
        // 检查是否已经拥有该类型的装备，如果已拥有则效果减半
        bool hasEquip = PlayerMoonEquips[player.name].Contains(rewardType);
        float effectivenessMultiplier = hasEquip ? 0.5f : 1.0f;
        
        // 记录获得的装备类型
        PlayerMoonEquips[player.name].Add(rewardType);

        StatBonus statBonus = new StatBonus();
        
        if (hero == null || hero.isDead)
        {
            PubBuyUtil.BroadcastMessage("奇迹无法降临于死者!");
            return;
        }
        
        switch (rewardType)
        {
            // 处理塑形玻璃购买
            case MoonShopRewardType.HooksofHeresy:
                if (player.hero.Status.maxHealth < 10)
                {
                    PubBuyUtil.BroadcastMessage("你的生命值太低，无法获得塑形玻璃！");
                }
                hero.Status.AddStatBonus(new StatBonus
                {
                    //最大生命值-50%
                    maxHealthFlat = - hero.Status.maxHealth * 0.5f * effectivenessMultiplier,
                    //转换攻击力
                    attackDamageFlat = Math.Min(hero.Status.maxHealth * 0.016f * effectivenessMultiplier, 320f),
                    //转换攻击力法强
                    abilityPowerFlat = Math.Min(hero.Status.abilityPower * 0.016f * 0.67f * effectivenessMultiplier, 320f* 0.67f),
                });
                AddDmg(hero, 0.5f * effectivenessMultiplier);
                PubBuyUtil.BroadcastMessage(player.name + "您已购买<b><color=#FFD700>塑形玻璃</color></b>，永久减少当前" + 
                                            (50 * effectivenessMultiplier) + "%最大生命值（" +
                                            hero.Status.maxHealth + "），永久转化为攻击（" + 
                                            Math.Min(hero.Status.maxHealth * 0.016f * effectivenessMultiplier, 320f) + "）和法强（" +
                                            Math.Min(hero.Status.abilityPower * 0.016f * 0.67f * effectivenessMultiplier, 320f* 0.67f) + 
                                            "）和" + (50 * effectivenessMultiplier) + "%增伤（当前额外增伤：" + 
                                            (GetDmg(hero)-1f) * 100 + "%）！");
                
                break;
                
            // 处理光通量肩甲购买
            case MoonShopRewardType.LightFluxPauldron:
                hero.Status.AddStatBonus(new StatBonus
                {
                    //技能极速+100
                    abilityHasteFlat = 100 * effectivenessMultiplier,
                    //攻速-50%
                    attackSpeedPercentage = -hero.Status.attackSpeedPercentage * 0.5f * effectivenessMultiplier
                });
                PubBuyUtil.BroadcastMessage(player.name + "您已购买<b><color=#FFD700>光通量肩甲</color></b>，技能极速+" + 
                                            (100 * effectivenessMultiplier) + "，攻速-" + 
                                            (50 * effectivenessMultiplier) + "%（" +
                                            hero.Status.attackSpeedPercentage*0.01f / effectivenessMultiplier + "）！");
                break;
            
            // 处理石通量肩甲购买
            case MoonShopRewardType.StoneFluxPauldron:
                hero.Status.AddStatBonus(new StatBonus
                {
                    //最大生命值增加50%
                    maxHealthFlat = hero.Status.maxHealth * 1.0f * effectivenessMultiplier,
                    //移动速度-50
                    movementSpeedPercentage = -hero.Status.movementSpeedPercentage * 0.5f * effectivenessMultiplier
                });
                
                PubBuyUtil.BroadcastMessage(player.name + "您已购买<b><color=#FFD700>石通量肩甲</color></b>，最大生命值+" + 
                                            (100 * effectivenessMultiplier) + "%（" +
                                            hero.Status.maxHealth * 0.5f * effectivenessMultiplier + "），当前移动速率-" + 
                                            (50 * effectivenessMultiplier) + "%（" +
                                            hero.Status.movementSpeedPercentage/effectivenessMultiplier + "）！");
                break;
        }
    }
        
    /**
     * 设置伤害加成
     */
    public static void SetDmg(Entity ent, float multiplier)
    {
        ent.AddData(new Ad_DealtDmg
        {
            multiplier = multiplier
        });
        ent.dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
        {
            if (!data.IsAmountModifiedBy(typeof(Ad_DealtDmg)) && ent.TryGetData<Ad_DealtDmg>(out var data2))
            {
                data.SetAmountModifiedBy(typeof(Ad_DealtDmg));
                data.ApplyRawMultiplier(data2.multiplier);
            }
        });
    }
    /**
     * 追加伤害加成
     */
    public static void AddDmg(Entity ent, float multiplier)
    {
        float currentDmg = GetDmg(ent);
        multiplier += currentDmg;
        SetDmg(ent, multiplier);
    }

    /**
     * 获取伤害加成
     */
    public static float GetDmg(Entity ent)
    {
        if (ent.TryGetData<Ad_DealtDmg>(out var data))
        {
            return data.multiplier;
        }

        return 1f;
    }

    private struct Ad_DealtDmg
    {
        public float multiplier;
    }
    
    
    // 处理显示当前属性的命令
    private static void HandleShowStats(ChatManager.Message message)
    {
        DewPlayer player = PubBuyUtil.GetPlayer(message);
        if (!PubBuyUtil.ValidatePlayer(player, checkAlive: true)) return;

        Hero hero = player.hero;
        if (hero == null) return;

        // 构建属性信息字符串
        string statsInfo = $"<pos=0%>{player.name} 当前属性:\n" +
                           $"<pos=0%>■等级: <pos=30%>{hero.Status.level}\n" +
                           $"<pos=0%>■生命: <pos=30%>{hero.Status.maxHealth}\n" +
                           $"<pos=0%>■攻击: <pos=30%>{hero.Status.attackDamage}\n" + 
                           $"<pos=0%>■法强: <pos=30%>{hero.Status.abilityPower}\n" +
                           $"<pos=0%>■攻速: <pos=30%>{hero.Status.attackSpeedPercentage*0.01}<pos=40%>丨{hero.Status.attackSpeedPercentage}%\n" +
                           $"<pos=0%>■冷却缩减: <pos=30%>{hero.Status.abilityHaste}<pos=40%>丨"+PubBuyUtil.Percentage(hero.Status.abilityHaste)+ "%\n" +
                           $"<pos=0%>■移动速率: <pos=30%>{hero.Status.movementSpeedPercentage}%\n" +
                           $"<pos=0%>■护甲: <pos=30%>{hero.Status.armorFromStats}<pos=40%>丨"+PubBuyUtil.RePercentage(hero.Status.armorFromStats)+"%\n" +
                           $"<pos=0%>■攻击距离: <pos=30%>{hero.Ability?.attackAbility?.configs[0].castMethod._range ?? 0}\n" +
                           $"<pos=0%>■韧性: <pos=30%>{hero.Status.tenacity}<pos=40%>丨"+PubBuyUtil.Percentage(hero.Status.tenacity)+"%\n" +
                           $"<pos=0%>■暴击率: <pos=30%>{hero.Status.critChance * 100}%\n" +
                           $"<pos=0%>■暴击伤害: <pos=30%>150%+{hero.Status.critAmp * 100}%\n" +
                           $"<pos=0%>■生命恢复: <pos=30%>{hero.Status.healthRegen}HP/s\n" +
                           $"<pos=0%>■额外火伤: <pos=30%>{hero.Status.fireEffectAmp * 100}%\n" +
                           $"<pos=0%>■额外冰伤: <pos=30%>{hero.Status.coldEffectAmp * 100}%\n" +
                           $"<pos=0%>■额外光伤: <pos=30%>{hero.Status.lightEffectAmp * 100}%\n" +
                           $"<pos=0%>■额外暗伤: <pos=30%>{hero.Status.darkEffectAmp * 100}%\n";

        if (hero.Skill?.Movement != null)
        {
            statsInfo += $"<pos=0%>■闪避冷却: <pos=30%>{hero.Skill.Movement.configs[0].cooldownTime}s\n" +
                         $"<pos=0%>■闪避充能: <pos=30%>{hero.Skill.Movement.configs[0].maxCharges}次\n" +
                         $"<pos=0%>■闪避距离: <pos=30%>{hero.Skill.Movement.configs[0].castMethod._range}\n";
        }

        // 添加增伤信息
        statsInfo += $"<pos=0%>■增伤: <pos=30%>{GetDmg(hero) * 100}%\n";

        PubBuyUtil.BroadcastMessage(statsInfo);
    }

    // 显示帮助信息
    private static void HandleHelp(ChatManager.Message obj)
    {
        if (PubBuyUtil.DeductGold(PubBuyUtil.GetPlayer(obj), 1, "你连一块都没有...看这个又有什么用呢?"))
        {
            PubBuyUtil.BroadcastMessage(
                @"欢迎来到酒馆！有下列佣兵可供各位选择.
当然你要有足够的金币他们才会为你卖命！
<pos=0%>丨序号<pos=10%>丨名称<pos=35%>丨花费<pos=60%>丨切换价格<pos=75%>丨
------------------------------------
<pos=0%>丨1<pos=10%>丨拉塞尔塔<pos=35%>丨150<pos=60%>丨500<pos=75%>丨
<pos=0%>丨2<pos=10%>丨薄雾<pos=35%>丨150<pos=60%>丨500<pos=75%>丨
<pos=0%>丨3<pos=10%>丨尤巴尔<pos=35%>丨150<pos=60%>丨500<pos=75%>丨
<pos=0%>丨4<pos=10%>丨维斯珀<pos=35%>丨150<pos=60%>丨500<pos=75%>丨
<pos=0%>丨5<pos=10%>丨奥雷娜<pos=35%>丨150<pos=60%>丨500<pos=75%>丨
<pos=0%>丨6<pos=10%>丨第一关BOSS<pos=35%>丨300<pos=60%>丨N<pos=75%>丨
<pos=0%>丨7<pos=10%>丨第二关BOSS<pos=35%>丨300<pos=60%>丨N<pos=75%>丨
<pos=0%>丨8<pos=10%>丨第三关BOSS<pos=35%>丨300<pos=60%>丨N<pos=75%>丨
<pos=0%>丨9<pos=10%>丨光辉BOSS<pos=35%>丨300<pos=60%>丨N<pos=75%>丨
<pos=0%>丨0<pos=10%>丨遗忘猎手BOSS<pos=35%>丨300<pos=60%>丨N<pos=75%>丨
<pos=0%>丨-<pos=10%>丨商人<pos=35%>丨998+<pos=60%>丨N<pos=75%>丨
------------------------------------
<pos=0%>丨i<pos=10%>丨显示属性<pos=35%>丨免费<pos=60%>丨N<pos=75%>丨
<pos=0%>丨+<pos=10%>丨锻体<pos=35%>丨100<pos=60%>丨N<pos=75%>丨
<pos=0%>丨+i<pos=10%>丨锻体概率公式<pos=35%>丨100<pos=60%>丨N<pos=75%>丨
<pos=0%>丨l<pos=10%>丨彩票（禁用）<pos=35%>丨100<pos=60%>丨N<pos=75%>丨
<pos=0%>丨p<pos=10%>丨月球商店说明<pos=35%>丨648<pos=60%>丨N<pos=75%>丨
------------------------------------
b [] 雇佣丨 c [] 控制丨 s [] 切换丨 r 重置视角丨 d 销毁当前玩家所有雇佣单位丨 h 帮助
例: b 1 (雇佣拉塞尔塔)
仅雇佣、切换角色和购买盲盒收费,其余选项皆免费
------------------------------------
购买锻体会获得不同品质的混沌奖励以及额外奖励，价格每周目提高15.4%
+ 购买一次丨++x 购买x次丨++9 购买9次丨+++ 购买最多
虽然购买次数没有限制，但是狗屎算法负载很大，尤其不建议在战斗中购买
保底机制：连续12次未获得传说品质时，第13次必定获得<color=#EE3830>传说</color>及其以上。
");
        }
    }
    
    private static void HandleMoonShopHelp(ChatManager.Message obj)
    {
        if (PubBuyUtil.DeductGold(PubBuyUtil.GetPlayer(obj), 1, "你连一块都没有...看这个又有什么用呢?"))
        {
            PubBuyUtil.BroadcastMessage(
                @"欢迎来到月亮商店！有下列月亮装备可供各位抽取.
友情提醒：以下装备都有来自【无为之主】的诅咒。输入【pb】抽奖，无法解除。
获得重复装备只会有基础的66%效果
<pos=0%>丨实装<pos=5%>丨名称<pos=15%>丨说明<pos=90%>丨
------------------------------------
<pos=0%>丨1<pos=5%>丨塑形玻璃<pos=15%>丨最大生命值-50%，转化为攻击、法强、增伤<pos=100%>丨
<pos=0%>丨1<pos=5%>丨光通量肩甲<pos=15%>丨技能极速+100，当前攻速-50%<pos=90%>丨
<pos=0%>丨1<pos=5%>丨石通量肩甲<pos=15%>丨最大生命值+100%，当前移动速率-50%<pos=90%>丨
<pos=0%>丨0<pos=5%>丨变化叶脊<pos=15%>丨周围16米所有单位+50%法强<pos=90%>丨
<pos=0%>丨0<pos=5%>丨尸爆<pos=15%>丨治疗+100%，但是每秒只接受单次治疗的20效果<pos=90%>丨
<pos=0%>丨0<pos=5%>丨超绝<pos=15%>丨最大生命值降低为1，但是扣除的生命会转化为75%可再生的护盾<pos=90%>丨
<pos=0%>丨0<pos=5%>丨地狱火㓅剂<pos=15%>丨点燃周围16米所有目标，每秒造成你10%的生命值伤害，英雄-75%<pos=90%>丨
<pos=0%>丨0<pos=5%>丨悲伤石像<pos=15%>丨周围16米所有单位移速-45%，同时护甲降低25<pos=90%>丨
<pos=0%>丨0<pos=5%>丨发光的陨石<pos=15%>丨聆听猎手的轰鸣<pos=90%>丨
------------------------------------
0代表还没有时装
");
        }
    }
    
    private static void ChaosBoxHelp(ChatManager.Message obj)
    {
        if (PubBuyUtil.DeductGold(PubBuyUtil.GetPlayer(obj), 1, "你连一块都没有...看这个又有什么用呢?"))
        {
            PubBuyUtil.BroadcastMessage(
                @"----------------------------锻体概率公示--------------------------
<pos=0%>丨品质·普通<pos=20%>丨15.4%<pos=40%>丨<color=#31EFF1>品质·稀有</color><pos=60%>丨54.6%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#B654E1>品质·史诗</color><pos=20%>丨23.4%<pos=40%>丨<color=#EE3830>品质·传说</color><pos=60%>丨6.6%<pos=80%>丨<pos=100%>
------------------------------主要奖励------------------------------
<pos=0%>丨最大生命<pos=20%>丨13.28%<pos=40%>丨攻击力<pos=60%>丨13.28%<pos=80%>丨<pos=100%>
<pos=0%>丨攻击速度<pos=20%>丨13.28%<pos=40%>丨法术强度<pos=60%>丨13.28%<pos=80%>丨<pos=100%>
<pos=0%>丨冷却缩减<pos=20%>丨13.28%<pos=40%>丨----<pos=60%>丨----<pos=80%>丨<pos=100%>
------------------------------次要奖励------------------------------
<pos=0%>丨移动速度<pos=20%>丨8.85%<pos=40%>丨护甲<pos=60%>丨8.85%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>攻击距离</color><pos=20%>丨1.56%<pos=40%>丨<color=#F1C13E>韧性</color><pos=60%>丨1.56%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>暴击率</color><pos=20%>丨1.56%<pos=40%>丨<color=#F1C13E>暴击伤害</color><pos=60%>丨1.56%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>闪避冷却</color><pos=20%>丨1.56%<pos=40%>丨<color=#F1C13E>飞升等级</color><pos=60%>丨1.56%<pos=80%>丨<pos=100%>
------------------------------其他奖励------------------------------
<pos=0%>丨<color=#F1C13E>完美宝石</color><pos=20%>丨0.78%<pos=40%>丨<color=#F1C13E>生命恢复</color><pos=60%>丨0.78%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>火伤加成</color><pos=20%>丨0.78%<pos=40%>丨<color=#F1C13E>冰伤加成</color><pos=60%>丨0.78%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>光伤加成</color><pos=20%>丨0.78%<pos=40%>丨<color=#F1C13E>暗伤加成</color><pos=60%>丨0.78%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>闪避距离</color><pos=20%>丨0.78%<pos=40%>丨场地魔法<pos=60%>丨0.94%<pos=80%>丨<pos=100%>
<pos=0%>丨<color=#F1C13E>闪避充能</color><pos=20%>丨0.17%<pos=40%>丨----<pos=60%>丨----<pos=80%>丨<pos=100%>
");
        }
    }

    // 静态构造函数 - 初始化玩家数据
    static PubBuyManager()
    {
        foreach (DewPlayer player in DewPlayer.humanPlayers)
        {
            if (!SpawnMap.ContainsKey(player.name))
            {
                SpawnMap[player.name] = new Dictionary<string, Entity>();
            }
        }
    }
}