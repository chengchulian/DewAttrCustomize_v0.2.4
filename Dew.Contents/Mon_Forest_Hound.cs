using System;

public class Mon_Forest_Hound : Monster, ISpawnableAsMiniBoss
{
	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (base.AI.Helper_CanBeCast<At_Mon_Forest_Hound_Charge>() && base.AI.Helper_IsTargetInRange<At_Mon_Forest_Hound_Charge>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Forest_Hound_Charge>();
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
		At_Mon_Forest_Hound_Charge ability = base.Ability.GetAbility<At_Mon_Forest_Hound_Charge>();
		TriggerConfig[] configs = ability.configs;
		foreach (TriggerConfig obj in configs)
		{
			obj.maxCharges = 3;
			obj.addedCharges = 3;
		}
		ability.TriggerEvent_OnCastCompleteBeforePrepare = (Action<EventInfoCast>)Delegate.Combine(ability.TriggerEvent_OnCastCompleteBeforePrepare, (Action<EventInfoCast>)delegate(EventInfoCast cast)
		{
			if (cast.instance is Ai_Mon_Forest_Hound_Charge ai_Mon_Forest_Hound_Charge)
			{
				ai_Mon_Forest_Hound_Charge.duration *= 0.7f;
			}
		});
	}

	private void MirrorProcessed()
	{
	}
}
