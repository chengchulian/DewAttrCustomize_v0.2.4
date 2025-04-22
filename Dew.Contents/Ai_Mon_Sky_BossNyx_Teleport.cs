using System.Collections;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_Teleport : AbilityInstance
{
	public float delay;

	public float awayDis;

	public float teleportDuration;

	public GameObject teleportEffect;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			base.info.caster.Visual.DisableRenderers();
			Hero hero = Dew.SelectRandomAliveHero();
			Vector3 agentPosition = hero.agentPosition;
			Vector3 normalized = (agentPosition - base.info.caster.agentPosition).normalized;
			Vector3 vector = agentPosition + normalized * awayDis;
			vector = Dew.GetPositionOnGround(vector);
			vector = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, vector);
			Teleport(base.info.caster, vector);
			base.info.caster.Control.RotateTowards(hero, immediately: true, 0.5f);
			base.info.caster.AI.Aggro(hero);
			yield return new SI.WaitForSeconds(teleportDuration);
			FxPlayNetworked(teleportEffect, base.info.caster);
			base.info.caster.Visual.EnableRenderers();
			yield return new SI.WaitForSeconds(delay);
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
