using System.Linq;
using UnityEngine;

public class Mon_Forest_BossDemon : BossMonster
{
    public float startAbilityCooldown;

    public float delayedAtkChance = 0.35f;

    public float dropHysteriaChance = 0.1f;

    protected override void OnCreate()
    {
        base.OnCreate();
        if (base.isServer)
        {
            CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
            delayedAtkChance *= NetworkedManagerBase<GameManager>.instance.GetSpecialSkillChanceMultiplier();
        }
    }

    public override void OnLateStartServer()
    {
        base.OnLateStartServer();
        At_Mon_Forest_BossDemon_MainSkill ability = base.Ability.GetAbility<At_Mon_Forest_BossDemon_MainSkill>();
        ability.SetChargeAll(0);
        ability.SetCooldownTimeAll(startAbilityCooldown);
    }

    protected override void AIUpdate(ref EntityAIContext context)
    {
        base.AIUpdate(ref context);
        if (!(context.targetEnemy == null))
        {
            if (!base.AI.Helper_IsTargetInRangeOfAttack() && base.AI.Helper_CanBeCast<At_Mon_Forest_BossDemon_Blink>())
            {
                base.AI.Helper_CastAbilityAuto<At_Mon_Forest_BossDemon_Blink>();
            }
            else if (base.AI.Helper_IsTargetInRange<At_Mon_Forest_BossDemon_MainSkill>() && base.AI.Helper_CanBeCast<At_Mon_Forest_BossDemon_MainSkill>())
            {
                base.AI.Helper_CastAbilityAuto<At_Mon_Forest_BossDemon_MainSkill>();
            }
            else if (base.AI.Helper_IsTargetInRange<At_Mon_Forest_BossDemon_DelayedAtk>() && base.AI.Helper_CanBeCast<At_Mon_Forest_BossDemon_DelayedAtk>() && Random.value < delayedAtkChance)
            {
                base.AI.Helper_CastAbilityAuto<At_Mon_Forest_BossDemon_DelayedAtk>();
            }
            else
            {
                base.AI.Helper_ChaseTarget();
            }
        }
    }

    protected override void OnBossSoulBeforeSpawn(Shrine_BossSoul soul)
    {
        base.OnBossSoulBeforeSpawn(soul);
        if (!AttrCustomizeResources.Config.removeSkills.Contains("St_L_Hysteria"))
        {
            soul.SetSkillReward<St_L_Hysteria>(dropHysteriaChance);
        }
    }

    private void MirrorProcessed()
    {
    }
}