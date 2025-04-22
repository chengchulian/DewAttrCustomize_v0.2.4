using System;
using UnityEngine;

public class Mon_SnowMountain_IceElemental : Monster, ISpawnableAsMiniBoss
{
	public float doubleSwipeChancePerSecond;

	public float iceBreakerChancePerSecond;

	protected override StaggerSettings GetStaggerSettings()
	{
		StaggerSettings staggerSettings = base.GetStaggerSettings();
		staggerSettings.canStaggerWhileChanneling = false;
		return staggerSettings;
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (base.AI.Helper_CanBeCast<At_Mon_SnowMountain_IceElemental_DoubleSwipe>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_IceElemental_DoubleSwipe>() && global::UnityEngine.Random.value < context.deltaTime * doubleSwipeChancePerSecond)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_IceElemental_DoubleSwipe>();
			}
			else if (base.AI.Helper_CanBeCast<At_Mon_SnowMountain_IceElemental_IceBreaker>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_IceElemental_IceBreaker>() && global::UnityEngine.Random.value < context.deltaTime * iceBreakerChancePerSecond)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_IceElemental_IceBreaker>();
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
		}
	}

	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (!base.isServer)
		{
			return;
		}
		ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
		TriggerConfig obj2 = base.Ability.attackAbility.configs[0];
		obj2.castMethod._length = 9f;
		AbilityTrigger.PredictionSettings predictionSettings = obj2.predictionSettings;
		predictionSettings.useCustomParameters = true;
		predictionSettings.type = AbilityTrigger.PredictionSettings.ModelType.SpeedAcceleration;
		predictionSettings.initSpeed = 15f;
		predictionSettings.targetSpeed = 15f;
		predictionSettings.acceleration = 0f;
		obj2.predictionSettings = predictionSettings;
		ActorEvent_OnAbilityInstanceCreated += (Action<EventInfoAbilityInstance>)delegate(EventInfoAbilityInstance obj)
		{
			if (obj.instance is Ai_Mon_SnowMountain_IceElemental_Atk)
			{
				obj.instance.CreateAbilityInstance<Ai_Mon_SnowMountain_IceElemental_MiniBossAtkFlavour>(obj.instance.position, null, obj.instance.info);
			}
		};
	}

	private void MirrorProcessed()
	{
	}
}
