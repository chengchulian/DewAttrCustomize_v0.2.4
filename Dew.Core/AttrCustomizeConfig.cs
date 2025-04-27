public class AttrCustomizeConfig
{
    public static readonly AttrCustomizeConfig DefaultConfig = new AttrCustomizeConfig()
    {
        maxPlayer = 4,
        enemyMovementSpeedPercentage = 1,
        enemyAttackSpeedPercentage = 1,
        enemyAbilityHasteFlat = 1,
        bossHealthMultiplier = 1,
        bossDamageMultiplier = 1,
        miniBossHealthMultiplier = 1,
        miniBossDamageMultiplier = 1,
        littleMonsterHealthMultiplier = 1,
        littleMonsterDamageMultiplier = 1,
        extraHealthGrowthMultiplier = 0,
        extraDamageGrowthMultiplier = 0,
        beneficialNodeMultiplier = 1,
        skillQGemCount = 3,
        skillWGemCount = 3,
        skillEGemCount = 3,
        skillRGemCount = 3,
        skillIdentityGemCount = 0,
        skillMovementGemCount = 0,
        shopAddedItems = 1,
        shopRefreshes = 1,
        bossCount = 1,
        bossCountAddByLoop = 0,
        bossCountAddByZone = 0,
        maxAndSpawnedPopulationMultiplier = 1.5f,
        startSkills = new string[] { },
        startSkillsLevel = new int[] { },
        startGems = new string[] { },
        startGemsQuality = new int[] { },
        enableHeroSkillAddShop = false,
        removeSkills = new string[] { },
        removeGems = new string[] { },
        enableMistAllowAnyDirection = true,
        firstVisitDropGoldCount = 0,
        firstVisitDropGoldCountAddByLoop = 0,
        firstVisitDropGoldCountAddByZone = 0,
        enableHealthReduceMultiplierAddByZone = false,
        enableCurrentNodeGenerateLostSoul = false,
        enableBossRoomGenerateLostSoul = false,
        enableArtifactQuest = true,
        enableFragmentOfRadianceBossQuest = true,
        enableQuestHuntedByObliviaxRepeatable = false,
        enableDamageRanking = false,
        enableBossSpawnAllOnce = false,
        bossMirageChance = 0f,
        bossHunterChance = 0f,
        monsterMirageChanceMultiple = 1f,
        enableWorldReveal = false,
        bossSingleInjuryHealthMultiplier = 1f,
        healRawMultiplier = 1f,
        enableLucidDreamEmbraceMortality = false,
        enableLucidDreamBonVoyage = false,
        enableLucidDreamGrievousWounds = false,
        enableLucidDreamTheDarkestUrge = false,
        enableLucidDreamWild = false,
        enableLucidDreamMadLife = false,
        enableLucidDreamSparklingDreamFlask = false,
        enableGemMerge = true,
    };

    /**
     * 房间最大人数
     */
    public int maxPlayer;

    /**
     * 所有敌人移动速度百分比
     */
    public float enemyMovementSpeedPercentage;

    /**
     * 所有敌人攻击速度百分比
     */
    public float enemyAttackSpeedPercentage;

    /**
     * 所有敌人技能加速百分比
     */
    public float enemyAbilityHasteFlat;

    /**
     * boss生命百分比
     */
    public float bossHealthMultiplier;

    /**
     * boss伤害百分比
     */
    public float bossDamageMultiplier;

    /**
     * miniBoss生命百分比
     */
    public float miniBossHealthMultiplier;

    /**
     * miniBoss伤害百分比
     */
    public float miniBossDamageMultiplier;

    /**
     * 小怪生命百分比
     */
    public float littleMonsterHealthMultiplier;

    /**
     * 小怪伤害百分比
     */
    public float littleMonsterDamageMultiplier;

    /**
     * 额外生命成长倍率(此数值为 n^当前关卡数 例: 怪物原始血量为100 当前关卡数为10 值为2 血量为100*2^10)
     */
    public float extraHealthGrowthMultiplier;

    /**
     * 额外伤害成长倍率(此数值为 n^当前关卡数)
     */
    public float extraDamageGrowthMultiplier;

    /**
     * 引导祭坛数量倍数
     */
    public float beneficialNodeMultiplier;

    /**
     * Q技能精华槽数量
     */
    public int skillQGemCount;

    /**
     * W技能精华槽数量
     */
    public int skillWGemCount;

    /**
     * E技能精华槽数量
     */
    public int skillEGemCount;

    /**
     * R技能精华槽数量
     */
    public int skillRGemCount;

    /**
     * 身份技能精华槽数量
     */
    public int skillIdentityGemCount;

    /**
     * 位移技能精华槽数量
     */
    public int skillMovementGemCount;

    /**
     * 商店增加物品数量
     */
    public int shopAddedItems;

    /**
     * 商店刷新次数
     */
    public int shopRefreshes;

    /**
     * boss数量
     */
    public int bossCount;

    /**
     * 每周目添加boss数量
     */
    public int bossCountAddByLoop;

    /**
     * 每关添加boss数量
     */
    public int bossCountAddByZone;

    /**
     * 人口过剩人口倍数
     */
    public float maxAndSpawnedPopulationMultiplier;

    /**
     * 开局发放技能
     */
    public string[] startSkills;

    /**
     * 开局发放技能等级
     */
    public int[] startSkillsLevel;

    /**
     * 开局发放精华
     */
    public string[] startGems;

    /**
     * 开局发放精华品质
     */
    public int[] startGemsQuality;

    /**
     * 开启转职
     */
    public bool enableHeroSkillAddShop;

    /**
     * 移除技能
     */
    public string[] removeSkills;

    /**
     * 移除精华
     */
    public string[] removeGems;

    /**
     * 薄雾全方位招架
     */
    public bool enableMistAllowAnyDirection;

    /**
     * 未访问过的图发钱数量
     */
    public int firstVisitDropGoldCount;


    /**
     * 未访问过的图发钱数量每周目添加数量
     */
    public int firstVisitDropGoldCountAddByLoop;

    /**
     * 未访问过的图发钱数量每关添加数量
     */
    public int firstVisitDropGoldCountAddByZone;

    /**
     * 开启幻想上限每周关增加
     */
    public bool enableHealthReduceMultiplierAddByZone;

    /**
     * 开启当前节点生成迷失灵魂
     */
    public bool enableCurrentNodeGenerateLostSoul;

    /**
     * 开启Boss房生成迷失灵魂
     */
    public bool enableBossRoomGenerateLostSoul;

    /**
     * 开启遗物任务
     */
    public bool enableArtifactQuest;

    /**
     * 开启光辉BOSS任务
     */
    public bool enableFragmentOfRadianceBossQuest;

    /**
     * 开启遗忘猎手任务可重复生成
     */
    public bool enableQuestHuntedByObliviaxRepeatable;

    /**
     * 开启每关发送伤害排行榜
     */
    public bool enableDamageRanking;

    /**
     * 开启所有boss一次性生成
     */
    public bool enableBossSpawnAllOnce;

    /**
     * boss幻想化概率
     */
    public float bossMirageChance;

    /**
     * boss猎手化概率
     */
    public float bossHunterChance;

    /**
     * 小怪产生紫皮概率倍数
     */
    public float monsterMirageChanceMultiple;

    /**
     * 开启所有节点透视
     */
    public bool enableWorldReveal;

    /**
     * Boss单次受伤血量百分比(限伤)
     */
    public float bossSingleInjuryHealthMultiplier;

    /**
     * 治疗效果百分比
     */
    public float healRawMultiplier;

    /**
     * 清醒梦 拥抱死亡 (所有角色的伤害量增加100%)
     */
    public bool enableLucidDreamEmbraceMortality;

    /**
     * 清醒梦 一路顺风 (猎手不再追踪探险队)
     */
    public bool enableLucidDreamBonVoyage;

    /**
     * 清醒梦 剧痛伤口 (旅行者接受的治疗和护盾减少50%)
     */
    public bool enableLucidDreamGrievousWounds;

    /**
     * 清醒梦 极暗冲动 (所有角色的无法识别敌我)
     */
    public bool enableLucidDreamTheDarkestUrge;

    /**
     * 清醒梦 野性本能 (除首领外所有怪物都将作为猎手被召唤)
     */
    public bool enableLucidDreamWild;

    /**
     * 清醒梦 疯狂生涯 (所有怪物都能更精准的预测旅行者的移动)
     */
    public bool enableLucidDreamMadLife;

    /**
     * 清醒梦 泡影浮梦 (生命药水和引导祭坛不再恢复生命值,而是提供梦尘)
     */
    public bool enableLucidDreamSparklingDreamFlask;
    
    /**
     * 精华合并
     */
    public bool enableGemMerge;
}