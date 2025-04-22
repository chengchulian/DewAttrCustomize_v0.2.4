using System;
using System.Collections;
using UnityEngine;

public class Se_Gem_C_Sharp_ArrowSpawner : StatusEffect
{
	public int shotArrows = 4;

	public float preDelay = 0.2f;

	public float shootInterval = 0.5f;

	public float radius = 7.5f;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		yield return new SI.WaitForSeconds(preDelay);
		for (int i = 0; i < shotArrows; i++)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.agentPosition, radius, tvDefaultHarmfulEffectTargets);
			if (readOnlySpan.Length > 0)
			{
				CreateAbilityInstance<Ai_Gem_C_Sharp_Arrow>(base.info.caster.position, null, new CastInfo(base.info.caster, readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)]));
			}
			handle.Return();
			base.gem.NotifyUse();
			yield return new SI.WaitForSeconds(shootInterval);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
