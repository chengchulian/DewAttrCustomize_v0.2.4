using System;
using UnityEngine;

public class Mon_SnowMountain_SnowWolf : Monster, ISpawnableAsMiniBoss
{
	private Se_Mon_SnowMountain_SnowWolf_SlowApproach _slowApproach;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (_slowApproach == null)
		{
			_slowApproach = DewResources.GetByType<Se_Mon_SnowMountain_SnowWolf_SlowApproach>();
		}
		if (!base.Status.HasStatusEffect<Se_Mon_SnowMountain_SnowWolf_SlowApproach>() && !(context.targetEnemy == null))
		{
			float num = Vector3.Distance(context.targetEnemy.position, base.position);
			if (base.AI.Helper_CanBeCast<At_Mon_SnowMountain_SnowWolf_SlowApproach>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_SnowWolf_SlowApproach>() && num >= _slowApproach.distanceRange.x && num <= _slowApproach.distanceRange.y)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_SnowWolf_SlowApproach>();
			}
			else if (base.AI.Helper_CanBeCast<At_Mon_SnowMountain_SnowWolf_Pounce>() && base.AI.Helper_IsTargetInRange<At_Mon_SnowMountain_SnowWolf_Pounce>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_SnowMountain_SnowWolf_Pounce>();
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
		At_Mon_SnowMountain_SnowWolf_Pounce ability = base.Ability.GetAbility<At_Mon_SnowMountain_SnowWolf_Pounce>();
		ability.configs[0].cooldownTime *= 0.35f;
		ability.configs[0].castMethod._length *= 2f;
		ActorEvent_OnAbilityInstanceBeforePrepare += (Action<EventInfoAbilityInstance>)delegate(EventInfoAbilityInstance ins)
		{
			if (ins.instance is Ai_Mon_SnowMountain_SnowWolf_Pounce ai_Mon_SnowMountain_SnowWolf_Pounce)
			{
				ai_Mon_SnowMountain_SnowWolf_Pounce.dash.distance *= 1.5f;
			}
		};
	}

	private void MirrorProcessed()
	{
	}
}
