using UnityEngine;

public class Mon_Special_BossLightElemental : BossMonster
{
	public Transform[] scaleTransforms;

	public Vector3[] localScales;

	public Transform[] translateTransforms;

	public Vector3[] localPositions;

	public bool beamAtkEnabled;

	public bool summonEnabled;

	public float startMainSkillDelayTime;

	public float mainSkillIntervalTime;

	public bool lightningEnabled;

	public float lightningHpThreshold;

	public bool beamBarrageEnabled;

	public float beamBarrageHpThreshold;

	private float _lastMainSkillUseTime;

	private float _skillChanceMultiplier;

	public override void OnStartServer()
	{
		base.OnStartServer();
		CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity, "boss_unstoppable");
		_lastMainSkillUseTime = Time.time - mainSkillIntervalTime + startMainSkillDelayTime;
		_skillChanceMultiplier = NetworkedManagerBase<GameManager>.instance.GetSpecialSkillChanceMultiplier();
	}

	private void LateUpdate()
	{
		for (int i = 0; i < scaleTransforms.Length; i++)
		{
			scaleTransforms[i].localScale = localScales[i];
		}
		for (int j = 0; j < translateTransforms.Length; j++)
		{
			translateTransforms[j].localPosition += localPositions[j];
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (context.targetEnemy == null)
		{
			return;
		}
		if (summonEnabled && base.AI.Helper_CanBeCast<At_Mon_Special_BossLightElemental_Summon>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossLightElemental_Summon>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossLightElemental_Summon>();
			return;
		}
		if (Time.time - _lastMainSkillUseTime > mainSkillIntervalTime - 12f * _skillChanceMultiplier)
		{
			float num = base.currentHealth / base.maxHealth;
			if (Random.value < 0.75f && beamBarrageEnabled && num < beamBarrageHpThreshold + 0.1f * _skillChanceMultiplier && base.AI.Helper_CanBeCast<At_Mon_Special_BossLightElemental_BeamBarrage>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossLightElemental_BeamBarrage>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossLightElemental_BeamBarrage>();
				_lastMainSkillUseTime = Time.time;
				return;
			}
			if (lightningEnabled && num < lightningHpThreshold && base.AI.Helper_CanBeCast<At_Mon_Special_BossLightElemental_Lightning>() && base.AI.Helper_IsTargetInRange<At_Mon_Special_BossLightElemental_Lightning>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossLightElemental_Lightning>();
				_lastMainSkillUseTime = Time.time;
				return;
			}
		}
		if (beamAtkEnabled && base.AI.Helper_CanBeCast<At_Mon_Special_BossLightElemental_BeamAtk>() && base.AI.Helper_IsTargetInRangeOfAttack())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Special_BossLightElemental_BeamAtk>();
		}
	}

	protected override void OnBossSoulBeforeSpawn(Shrine_BossSoul soul)
	{
		base.OnBossSoulBeforeSpawn(soul);
		soul.SetSkillReward<St_L_WorldCracker>(1f);
	}

	private void MirrorProcessed()
	{
	}
}
