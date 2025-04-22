using UnityEngine;

public class Mon_Special_BossObliviax : BossMonster
{
	public float skillChance = 0.2f;

	public float shadowWalkFarChance = 0.25f;

	public float shadowWalkCloseChance = 0.25f;

	public float shadowWalkDistanceThresholdOffset = 1f;

	public Vector2 shadowWalkInterval;

	public float backStepChance;

	internal float _chainAttackRequestTime = float.NegativeInfinity;

	private float _currentShadowWalkInterval;

	private float _lastShadowWalkTime;

	public bool isDashAtkAvailable => true;

	public bool isTurnAtkAvailable => true;

	public bool isTurretModeAvailable => true;

	public bool isNeedleAtkAvailable => base.normalizedHealth < 0.6666f;

	public bool isArtilleryAvailable => base.normalizedHealth < 0.7111f;

	public bool isBackstepAvailable => base.normalizedHealth < 0.4555f;

	public override void OnStartServer()
	{
		base.OnStartServer();
		CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
		_lastShadowWalkTime = Time.time;
		_currentShadowWalkInterval = Random.Range(shadowWalkInterval.x, shadowWalkInterval.y);
		base.isHunter = true;
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_Laugh>() && NetworkedManagerBase<GameManager>.instance.isGameConcluded)
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_Laugh>();
		}
		else
		{
			if (context.targetEnemy == null)
			{
				return;
			}
			float num = skillChance * context.deltaTime;
			if (Time.time - _chainAttackRequestTime < 1.5f && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_ShadowWalk>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_ShadowWalk>())
			{
				base.Ability.attackAbility.ResetCooldown();
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_ShadowWalk>();
				_chainAttackRequestTime = float.NegativeInfinity;
				return;
			}
			if (isDashAtkAvailable && Random.value < num && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_DashAtk>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_DashAtk>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_DashAtk>();
				return;
			}
			if (isTurnAtkAvailable && Random.value < num && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_TurnAtk>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_TurnAtk>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_TurnAtk>();
				return;
			}
			if (isTurretModeAvailable && Random.value < num && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_TurretMode>())
			{
				Entity entity = null;
				float num2 = 0f;
				foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
				{
					if (!humanPlayer.hero.IsNullInactiveDeadOrKnockedOut())
					{
						float num3 = Vector3.Distance(humanPlayer.hero.position, base.position);
						if (num3 > num2)
						{
							num2 = num3;
							entity = humanPlayer.hero;
						}
					}
				}
				if (!entity.IsNullInactiveDeadOrKnockedOut())
				{
					base.AI.Helper_CastAbility<At_Mon_Special_BossObliviax_TurretMode>(new CastInfo(this, entity));
				}
				return;
			}
			if (isNeedleAtkAvailable && Random.value < num && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_NeedleAtk>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_NeedleAtk>() && base.normalizedHealth > 0.125f)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_NeedleAtk>();
				return;
			}
			if (isArtilleryAvailable && Random.value < num && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_Artillery>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_Artillery>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_Artillery>();
				return;
			}
			if (isBackstepAvailable && Random.value < num * 2f && base.AI.Helper_CanBeCast<At_Mon_Special_BossObliviax_BackStep>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossObliviax_BackStep>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossObliviax_BackStep>();
				return;
			}
			Entity targetEnemy = context.targetEnemy;
			if (Time.time - _lastShadowWalkTime > _currentShadowWalkInterval)
			{
				_lastShadowWalkTime = Time.time;
				_currentShadowWalkInterval = Random.Range(shadowWalkInterval.x, shadowWalkInterval.y);
				float num4 = base.Ability.attackAbility.currentConfig.effectiveRange + shadowWalkDistanceThresholdOffset;
				Vector3 b = targetEnemy.agentPosition;
				if (Vector3.Distance(base.agentPosition, b) < num4)
				{
					if (Random.value > shadowWalkCloseChance)
					{
						return;
					}
				}
				else if (Random.value > shadowWalkFarChance)
				{
					return;
				}
				base.AI.Helper_CastAbility<At_Mon_Special_BossObliviax_ShadowWalk>(new CastInfo(this, targetEnemy));
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
		soul.SetSkillReward<St_L_ShoutOfOblivion>(1f);
	}

	private void MirrorProcessed()
	{
	}
}
