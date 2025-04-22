using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_Artillery : AbilityInstance
{
	public float delay;

	public float interval;

	public int count;

	public Vector2 artilleryRange;

	public float artilleryRandomMag;

	public float targetingChance;

	public DewAnimationClip clip;

	public GameObject fxCast;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 10f, "ObliviaxUnstoppable").DestroyOnDestroy(this);
		FxPlayNetworked(fxCast, base.info.caster);
		float totalTime = interval * (float)count;
		base.info.caster.Control.StartDaze(totalTime);
		base.info.caster.Animation.PlayAbilityAnimation(clip);
		float startTime = Time.time;
		int doneShots = 0;
		while (true)
		{
			float num = Time.time - startTime;
			int num2 = Mathf.RoundToInt(Mathf.Lerp(0f, count, num / totalTime));
			int num3 = num2 - doneShots;
			if (num3 <= 0 && num > totalTime)
			{
				break;
			}
			doneShots = num2;
			for (int i = 0; i < num3; i++)
			{
				Vector3 point = base.info.caster.agentPosition + global::UnityEngine.Random.insideUnitCircle.ToXZ() * global::UnityEngine.Random.Range(artilleryRange.x, artilleryRange.y);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.agentPosition, artilleryRange.y, tvDefaultHarmfulEffectTargets);
				if (global::UnityEngine.Random.value < targetingChance && readOnlySpan.Length > 0)
				{
					Entity target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
					point = AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.Range(0.5f, 1f), target, delay) + global::UnityEngine.Random.insideUnitCircle.ToXZ() * artilleryRandomMag;
				}
				handle.Return();
				CreateAbilityInstance(point, null, new CastInfo(base.info.caster, point), delegate(Ai_Mon_Special_BossObliviax_Artillery_Instance b)
				{
					b.delay = delay;
				});
			}
			yield return null;
		}
		FxStopNetworked(fxCast);
		Destroy();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && fxCast != null)
		{
			FxStopNetworked(fxCast);
		}
	}

	private void MirrorProcessed()
	{
	}
}
