using System.Collections;
using UnityEngine;

public class Ai_Mon_Forest_BossDemon_MainSkill_SpawnMissiles : AbilityInstance
{
	public float initialDelay;

	public DewAnimationClip eachWaveAnim;

	public int waves;

	public float waveInterval;

	public int perWaveMissileCountStart;

	public int perWaveMissileCountAdded;

	public GameObject spawnEffect;

	public float postDelay;

	protected override IEnumerator OnCreateSequenced()
	{
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			base.info.caster.Control.StartDaze(initialDelay + waveInterval * (float)waves + postDelay);
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), initialDelay + waveInterval * (float)waves, "SpawnMissilesUnstoppable");
			yield return new SI.WaitForSeconds(initialDelay);
			int missileCount = perWaveMissileCountStart;
			for (int i = 0; i < waves; i++)
			{
				base.info.caster.Animation.PlayAbilityAnimation(eachWaveAnim);
				FxPlayNewNetworked(spawnEffect);
				SpawnMissiles(this, base.info.caster, missileCount);
				missileCount += perWaveMissileCountAdded;
				yield return new SI.WaitForSeconds(waveInterval);
			}
		}
	}

	public static void SpawnMissiles(Actor parent, Entity caster, int count)
	{
		float num = Random.Range(0f, 360f);
		for (int i = 0; i < count; i++)
		{
			parent.CreateAbilityInstance<Ai_Mon_Forest_BossDemon_MainSkill_SpawnMissiles_Missile>(caster.position, null, new CastInfo(caster, num));
			num += 360f / (float)count;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !base.hasOngoingSequences)
		{
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
