using UnityEngine;

public class Mon_LavaLand_FireElemental : Monster, ISpawnableAsMiniBoss
{
	public float backdashChance;

	public float triggerBackdashDist;

	public float triggerHealthDestruct;

	public override void OnStartServer()
	{
		base.OnStartServer();
		AddData(new LavaLand_Lava.Ad_Lava
		{
			isImmune = true
		});
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (!(context.targetEnemy == null))
		{
			if (LavaLand_Lava.instance != null && LavaLand_Lava.instance.IsEntityOnLava(this) && !LavaLand_Lava.instance.IsEntityOnLava(context.targetEnemy))
			{
				base.Control.MoveToDestination(context.targetEnemy.position, immediately: false);
			}
			else if (Random.value < backdashChance && base.AI.Helper_CanBeCast<At_Mon_LavaLand_FireElemental_BackDash>() && Vector3.Distance(base.position, context.targetEnemy.position) < triggerBackdashDist)
			{
				base.AI.Helper_CastAbility<At_Mon_LavaLand_FireElemental_BackDash>(new CastInfo(this, context.targetEnemy));
			}
			else if (base.normalizedHealth <= triggerHealthDestruct && base.AI.Helper_CanBeCast<At_Mon_LavaLand_FireElemental_Destruct>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_LavaLand_FireElemental_Destruct>();
			}
			else if (base.AI.Helper_CanBeCast<At_Mon_LavaLand_FireElemental_Explosion>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_LavaLand_FireElemental_Explosion>();
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
		if (base.isServer)
		{
			ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
			base.Ability.GetAbility<At_Mon_LavaLand_FireElemental_Explosion>().configs[0].cooldownTime *= 0.5f;
			base.Ability.GetAbility<At_Mon_LavaLand_FireElemental_Destruct>().configs[0].maxCharges = 0;
			base.Ability.attackAbility.configs[0].postDelay = 0.25f;
			base.Status.AddStatBonus(new StatBonus
			{
				attackSpeedPercentage = 50f
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
