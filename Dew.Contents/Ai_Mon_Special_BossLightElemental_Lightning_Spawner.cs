using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossLightElemental_Lightning_Spawner : AbilityInstance
{
	public DewCollider targetRange;

	public int fallCount;

	public float fallInterval;

	public float randomMagnitude;

	public float fallPredictionDelay;

	public float postDelay;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		float duration = postDelay + (float)fallCount * fallInterval;
		base.info.caster.Control.StartDaze(duration);
		Vector3 fallPos = base.info.caster.position + base.info.caster.transform.forward * 3f;
		for (int i = 0; i < fallCount; i++)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = targetRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				includeUncollidable = true
			});
			if (entities.Length > 0)
			{
				Entity target = entities[global::UnityEngine.Random.Range(0, entities.Length)];
				fallPos = AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.value, target, fallPredictionDelay) + global::UnityEngine.Random.insideUnitSphere.Flattened() * randomMagnitude;
				fallPos = Dew.GetPositionOnGround(fallPos);
			}
			handle.Return();
			CreateAbilityInstance<Ai_Mon_Special_BossLightElemental_Lightning>(fallPos, null, new CastInfo(base.info.caster));
			yield return new SI.WaitForSeconds(fallInterval);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
