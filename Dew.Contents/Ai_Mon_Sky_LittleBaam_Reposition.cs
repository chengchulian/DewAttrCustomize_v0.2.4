using System.Collections;
using UnityEngine;

public class Ai_Mon_Sky_LittleBaam_Reposition : AbilityInstance
{
	public float dazeDuration;

	public Vector2 distanceFromTargetPoint;

	public float randomMagnitude;

	public Vector2 delay;

	public float landYOffset;

	public float landYVelocity;

	public float landEffectDelay;

	public GameObject landEffect;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			base.info.caster.Control.StartDaze(dazeDuration);
			Vector2 normalized = (base.info.point - base.info.caster.position).ToXY().normalized;
			Vector3 dest = Dew.GetValidAgentDestination_LinearSweep(end: base.info.point + normalized.ToXZ() * Random.Range(distanceFromTargetPoint.x, distanceFromTargetPoint.y) + Random.insideUnitSphere * randomMagnitude, start: base.info.caster.agentPosition);
			base.info.caster.Visual.DisableRenderers();
			yield return new SI.WaitForSeconds(Random.Range(delay.x, delay.y));
			Teleport(base.info.caster, dest);
			base.info.caster.Control.RotateTowards(base.info.point, immediately: true);
			base.info.caster.Visual.EnableRenderers();
			base.info.caster.Visual.SetYOffset(landYOffset);
			base.info.caster.Visual.SetYVelocity(landYVelocity);
			yield return new SI.WaitForSeconds(landEffectDelay);
			FxPlayNewNetworked(landEffect, Dew.GetPositionOnGround(dest), Quaternion.identity);
		}
	}

	private void MirrorProcessed()
	{
	}
}
