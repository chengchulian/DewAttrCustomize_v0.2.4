public class Mon_Sky_BigBaam_Rooter : Mon_Sky_BigBaam_Base, ISpawnableAsMiniBoss
{
	protected override bool MainSkillRoutine()
	{
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BigBaam_Main_Root>() && base.AI.context.targetEnemy != null)
		{
			if (base.AI.Helper_IsTargetInRange<At_Mon_Sky_BigBaam_Main_Root>())
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BigBaam_Main_Root>();
			}
			else
			{
				base.AI.Helper_ChaseTarget();
			}
			return true;
		}
		return false;
	}

	public void OnBeforeSpawnAsMiniBoss()
	{
	}

	public void OnCreateAsMiniBoss()
	{
		if (base.isServer)
		{
			ISpawnableAsMiniBoss.GiveGenericMiniBossBonus(this);
			base.Ability.GetAbility<At_Mon_Sky_BigBaam_BeamAtk>().spawnAdditionalProjectile = true;
			base.Ability.GetAbility<At_Mon_Sky_BigBaam_Main_Root>().configs[0].cooldownTime *= 0.5f;
		}
	}

	private void MirrorProcessed()
	{
	}
}
