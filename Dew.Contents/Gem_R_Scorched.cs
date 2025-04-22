using System;
using System.Collections;
using UnityEngine;

public class Gem_R_Scorched : Gem
{
	public ScalingValue maxCount;

	public float radius = 12f;

	public float randomMag;

	protected override void OnCastComplete(EventInfoCast info)
	{
		base.OnCastComplete(info);
		NotifyUse();
		StartCoroutine(ScorchedRoutine(info.instance));
	}

	private IEnumerator ScorchedRoutine(AbilityInstance source)
	{
		source.LockDestroy();
		int count = Mathf.RoundToInt(GetValue(maxCount));
		int noTargetCount = 0;
		for (int i = 0; i < count; i++)
		{
			if (!base.isValid)
			{
				yield break;
			}
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.agentPosition, radius, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				includeUncollidable = true
			});
			if (readOnlySpan.Length > 0)
			{
				Entity target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
				CreateAbilityInstanceWithSource<Ai_Gem_R_Scorched_Meteor>(source, Dew.GetPositionOnGround(AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.value, target, 0.35f) + global::UnityEngine.Random.insideUnitSphere.Flattened() * (randomMag * Mathf.Clamp((float)i / 5f, 0.5f, 1f))), Quaternion.identity, new CastInfo(base.owner));
				yield return new SI.WaitForSeconds(0.05f);
			}
			else
			{
				noTargetCount++;
				if (noTargetCount >= 10)
				{
					yield break;
				}
			}
			handle.Return();
			NotifyUse();
			yield return new SI.WaitForSeconds(0.035f);
			handle = default(ArrayReturnHandle<Entity>);
		}
		source.UnlockDestroy();
	}

	private void MirrorProcessed()
	{
	}
}
