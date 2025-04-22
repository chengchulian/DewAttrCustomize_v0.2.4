using UnityEngine;

public class Mon_Sky_StarSeed : Monster, ISpawnableAsMiniBoss
{
	public float selfDestructTime = 6f;

	public bool canSelfDestruct;

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (canSelfDestruct && Time.time - base.creationTime > selfDestructTime && base.AI.Helper_CanBeCast<At_Mon_Sky_StarSeed_SelfDestruct>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_StarSeed_SelfDestruct>();
		}
		else if (!(context.targetEnemy == null))
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
			At_Mon_Sky_StarSeed_Atk ability = base.Ability.GetAbility<At_Mon_Sky_StarSeed_Atk>();
			ability.configs[0].channel.duration *= 1.55f;
			ability.NetworkisMegaExplosion = true;
			base.Status.AddStatBonus(new StatBonus
			{
				movementSpeedPercentage = 100f
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
