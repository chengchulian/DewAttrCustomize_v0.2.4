using System.Collections;

public class Ai_Mon_Special_BossObliviax_DashAtk_Spawner : AbilityInstance
{
	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		for (int i = 0; i < 3; i++)
		{
			Hero closestAliveHero = Dew.GetClosestAliveHero(base.info.caster.agentPosition);
			if (closestAliveHero.IsNullInactiveDeadOrKnockedOut())
			{
				break;
			}
			float speed = 1f + (float)i * 0.3f;
			Ai_Mon_Special_BossObliviax_DashAtk ins = CreateAbilityInstance(base.info.caster.agentPosition, null, new CastInfo(base.info.caster, AbilityTrigger.PredictAngle_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), closestAliveHero, base.info.caster.agentPosition, speed)), delegate(Ai_Mon_Special_BossObliviax_DashAtk d)
			{
				d.NetworktelegraphDuration = d.telegraphDuration / speed;
			});
			while (!ins.IsNullOrInactive())
			{
				yield return null;
			}
			if (base.info.caster.normalizedHealth > 1f - (float)(i + 1) * 0.3333f)
			{
				break;
			}
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
