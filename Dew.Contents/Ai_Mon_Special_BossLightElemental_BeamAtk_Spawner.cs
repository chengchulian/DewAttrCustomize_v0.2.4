using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Special_BossLightElemental_BeamAtk_Spawner : AbilityInstance
{
	[Serializable]
	public struct AttackType
	{
		public float hpThreshold;

		public float[] angles;
	}

	public DewCollider range;

	public AttackType[] attacks;

	public float dazeDuration;

	public GameObject perBeamTelegraph;

	public GameObject perBeam;

	public GameObject beamCast;

	public GameObject telegraphingEffect;

	public DewAnimationClip animChannel;

	public DewAnimationClip animCast;

	public float damageMultiplier = 2f;

	public float duration = 0.15f;

	public float telegraphDelay;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		int num = attacks.Length - 1;
		for (int i = 0; i < attacks.Length; i++)
		{
			if (!(base.info.caster.normalizedHealth > attacks[i].hpThreshold))
			{
				num = i;
				break;
			}
		}
		AttackType attackType = attacks[num];
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			includeUncollidable = true,
			sortComparer = CollisionCheckSettings.DistanceFromCenter
		});
		List<float> angles = new List<float>();
		for (int j = 0; j < entities.Length; j++)
		{
			Entity entity = entities[j];
			if (entity is Hero e && !e.IsNullInactiveDeadOrKnockedOut() && base.info.caster.CheckEnemyOrNeutral(entity))
			{
				float num2 = AbilityTrigger.PredictAngle_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), entity, base.info.caster.position, telegraphDelay);
				float[] angles2 = attackType.angles;
				foreach (float num3 in angles2)
				{
					angles.Add(num2 + num3);
				}
			}
		}
		handle.Return();
		if (angles.Count <= 0)
		{
			Destroy();
			yield break;
		}
		float clipSelectValue = global::UnityEngine.Random.value;
		base.info.caster.Control.StartDaze(dazeDuration);
		base.info.caster.Control.Rotate(angles[0], immediately: false);
		if (animChannel != null)
		{
			base.info.caster.Animation.PlayAbilityAnimation(animChannel, 1f, clipSelectValue);
		}
		FxPlayNetworked(telegraphingEffect);
		for (int l = 0; l < angles.Count; l++)
		{
			FxPlayNewNetworked(perBeamTelegraph, base.info.caster.position, Quaternion.Euler(0f, angles[l], 0f));
		}
		yield return new SI.WaitForSeconds(telegraphDelay);
		FxPlayNetworked(beamCast);
		if (animCast != null)
		{
			base.info.caster.Animation.PlayAbilityAnimation(animCast, 1f, clipSelectValue);
		}
		FxStopNetworked(telegraphingEffect);
		int m;
		for (m = 0; m < angles.Count; m++)
		{
			CreateAbilityInstance(base.info.caster.position, null, new CastInfo(base.info.caster), delegate(Ai_Mon_Special_BossLightElemental_Beam b)
			{
				b.Networkangle = Vector2.one * angles[m];
				b.Networkduration = duration;
				b.NetworkdamageMultiplier = damageMultiplier;
			});
			FxPlayNewNetworked(perBeam, base.info.caster.position, Quaternion.Euler(0f, angles[m], 0f));
		}
		Destroy();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		FxStop(telegraphingEffect);
	}

	private void MirrorProcessed()
	{
	}
}
