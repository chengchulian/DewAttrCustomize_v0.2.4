using UnityEngine;

public class Mon_Forest_SpiderSpitter : Monster, ISpawnableAsMiniBoss
{
	public float triggerBackdashDist = 5f;

	public float backdashChancePerSecond = 0.5f;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (context.targetEnemy == null)
		{
			return;
		}
		if (Random.value < backdashChancePerSecond * context.deltaTime && base.AI.Helper_CanBeCast<At_Mon_Forest_SpiderSpitter_Backdash>() && Vector3.Distance(base.position, context.targetEnemy.position) < triggerBackdashDist)
		{
			base.AI.Helper_CastAbility<At_Mon_Forest_SpiderSpitter_Backdash>(new CastInfo(this, context.targetEnemy));
		}
		else if (base.AI.Helper_IsTargetInRange<At_Mon_Forest_SpiderSpitter_Melee>())
		{
			if (base.AI.Helper_CanBeCast<At_Mon_Forest_SpiderSpitter_Melee>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Forest_SpiderSpitter_Melee>();
			}
		}
		else
		{
			base.AI.Helper_ChaseTarget();
		}
	}

	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (base.isServer)
		{
			ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
			At_Mon_Forest_SpiderSpitter_Atk ability = base.Ability.GetAbility<At_Mon_Forest_SpiderSpitter_Atk>();
			ability.spawnAdditionalProjectile = true;
			ability.configs[0].castMethod._range += 5f;
			base.Ability.GetAbility<At_Mon_Forest_SpiderSpitter_Melee>().configs[0].maxCharges = 0;
			base.Status.AddStatBonus(new StatBonus
			{
				attackSpeedPercentage = 15f
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
