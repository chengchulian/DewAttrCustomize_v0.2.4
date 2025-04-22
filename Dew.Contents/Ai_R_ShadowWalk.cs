using System;
using System.Collections;
using UnityEngine;

public class Ai_R_ShadowWalk : AbilityInstance
{
	public Dash dash;

	public DewCollider range;

	public int spawnCount;

	public float initDelay;

	public float spawnInterval;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		CreateStatusEffect<Se_R_ShadowWalk_Disappear>(base.info.caster);
		dash.ApplyByDirection(base.info.caster, base.info.forward);
		yield return new SI.WaitForSeconds(initDelay);
		for (int i = 0; i < spawnCount; i++)
		{
			range.transform.position = base.info.caster.position;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				sortComparer = CollisionCheckSettings.Random
			});
			if (entities.Length > 0)
			{
				Entity target = entities[global::UnityEngine.Random.Range(0, entities.Length)];
				CreateAbilityInstance<Ai_R_ShadowWalk_Projectile>(base.info.caster.position, Quaternion.identity, new CastInfo(base.info.caster, target));
			}
			handle.Return();
			yield return new SI.WaitForSeconds(spawnInterval);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
