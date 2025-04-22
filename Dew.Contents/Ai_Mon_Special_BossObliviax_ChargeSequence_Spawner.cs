using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_ChargeSequence_Spawner : AbilityInstance
{
	public float initDelay;

	public float duration;

	public GameObject fxDisappear;

	public float fsDelay;

	public float fsDuration;

	public GameObject fsTelegraphEffect;

	public float ssDelay;

	public int ssWaveCount;

	public int ssAtkCountPerWave;

	public float ssStartPointRadius;

	public int ssStartPointCount;

	public float ssAtkDelay;

	public float ssAtkInterval;

	public float ssWaveInterval;

	public GameObject ssTelegraphEffect;

	public GameObject ssStartEffect;

	public GameObject ssCloneEffect;

	public DewAnimationClip ssCloneAnimation;

	public float tsDelay;

	public float tsTelegraphInterval;

	public float tsAtkDelay;

	public GameObject tsStartTelegraphEffect;

	public GameObject tsFirstAtkEffect;

	public ScalingValue tsFirstAtkDmgFactor;

	public DewAnimationClip tsLandingAnimation;

	public float tsPostDelay;

	[Space(15f)]
	[Header("Donut Atk")]
	public int donutRadiusAdder = 4;

	public int donutCount;

	public DewCollider tsDonutRange;

	public GameObject tsDonutTelegraphEffect;

	public GameObject tsDonutAtkEffect;

	public ScalingValue tsDonutDmgFactor;

	private Vector3 _roomCenterPos;

	private BoxTelegraphController _boxTelegraphController;

	private ArcTelegraphController _arcTelegraphController;

	private DewCollider _donutCollider;

	private float _donutStartRadius;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		if (SingletonBehaviour<Obliviax_BossRoomCenter>.instance != null)
		{
			_roomCenterPos = SingletonBehaviour<Obliviax_BossRoomCenter>.instance.transform.position;
		}
		else
		{
			_roomCenterPos = base.info.caster.section.transform.position;
		}
		_roomCenterPos = Dew.GetPositionOnGround(_roomCenterPos);
		_boxTelegraphController = ssTelegraphEffect.GetComponentInChildren<BoxTelegraphController>();
		_arcTelegraphController = tsDonutTelegraphEffect.GetComponentInChildren<ArcTelegraphController>();
		_donutCollider = tsDonutRange.GetComponent<DewCollider>();
		_donutStartRadius = _donutCollider.radius;
		duration = initDelay + fsDelay + ssDelay + ssAtkInterval * (float)ssAtkCountPerWave * ssWaveInterval + ssWaveInterval * (float)ssWaveCount + tsDelay + tsAtkDelay;
		FxPlayNetworked(fxDisappear, base.info.caster);
		CreateBasicEffect(base.info.caster, new InvisibleEffect(), duration);
		CreateBasicEffect(base.info.caster, new InvulnerableEffect(), duration);
		CreateBasicEffect(base.info.caster, new UncollidableEffect(), duration);
		base.info.caster.Visual.DisableRenderers();
		base.info.caster.Control.freeMovement = true;
		base.info.caster.Control.StartDaze(duration);
		yield return new SI.WaitForSeconds(initDelay);
		List<(Hero, Vector3)> heros = new List<(Hero, Vector3)>();
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			Hero hero = humanPlayer.hero;
			if (!hero.IsNullInactiveDeadOrKnockedOut())
			{
				Vector3 agentPosition = hero.agentPosition;
				heros.Add((hero, agentPosition));
				FxPlayNewNetworked(fsTelegraphEffect, agentPosition, null);
			}
		}
		base.info.caster.Control.StartDaze(fsDelay);
		yield return new SI.WaitForSeconds(fsDelay);
		foreach (var item in heros)
		{
			var (hero2, pos) = item;
			if (!hero2.IsNullInactiveDeadOrKnockedOut())
			{
				CreateAbilityInstance(hero2.agentPosition, null, new CastInfo(base.info.caster, hero2), delegate(Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk b)
				{
					b.initPoint = pos;
					b.duration = fsDuration;
				});
			}
		}
		yield return new SI.WaitForSeconds(ssDelay);
		ssAtkCountPerWave += DewPlayer.humanPlayers.Count - 1;
		float ssAngle = 360f / (float)ssStartPointCount;
		List<int> availableIndices = new List<int>();
		for (int i = 0; i < ssWaveCount; i++)
		{
			for (int j = 0; j < ssStartPointCount; j++)
			{
				availableIndices.Add(j);
			}
			for (int k = 0; k < ssAtkCountPerWave; k++)
			{
				int num = global::UnityEngine.Random.Range(0, availableIndices.Count);
				availableIndices.RemoveAt(num);
				Vector3 normalized = (Quaternion.AngleAxis((float)num * ssAngle, Vector3.up) * Vector3.forward).Flattened().normalized;
				Vector3 startPos = _roomCenterPos + normalized * ssStartPointRadius;
				startPos = Dew.GetValidAgentDestination_LinearSweep(_roomCenterPos, startPos);
				Quaternion finalRot = default(Quaternion);
				float finalAngle = 0f;
				if (k < DewPlayer.humanPlayers.Count && !DewPlayer.humanPlayers[k].hero.IsNullInactiveDeadOrKnockedOut())
				{
					normalized = (AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.Range(0.7f, 1f), DewPlayer.humanPlayers[k].hero, ssAtkDelay) - startPos).normalized;
					finalRot = Quaternion.AngleAxis(finalAngle = Vector3.Angle(Vector3.forward, normalized), Vector3.up);
				}
				else
				{
					normalized = Quaternion.Euler(0f, global::UnityEngine.Random.Range(-20f, 20f), 0f) * -normalized;
					finalAngle = Vector3.Angle(Vector3.forward, normalized);
					finalRot = Quaternion.LookRotation(normalized);
				}
				SpawnEntity(startPos, finalRot, DewPlayer.creep, base.info.caster.level, delegate(Mon_Special_ObliviaxHallucination b)
				{
					StartCoroutine(Routine());
					IEnumerator Routine()
					{
						b.Visual.spawnDuration = ssAtkDelay;
						b.Control.StartDaze(3f);
						yield return new WaitForSeconds(ssAtkDelay);
						CreateAbilityInstance<Ai_Mon_Special_BossObliviax_ChargeSequence_LinearAtk>(startPos, finalRot, new CastInfo(base.info.caster, finalAngle));
						FxPlayNewNetworked(ssCloneEffect, b);
						b.Animation.PlayAbilityAnimation(ssCloneAnimation);
						b.Control.StartDisplacement(new DispByDestination
						{
							affectedByMovementSpeed = false,
							canGoOverTerrain = true,
							destination = b.agentPosition + b.transform.forward * _boxTelegraphController.height,
							duration = 0.35f,
							isFriendly = true,
							isCanceledByCC = false,
							onFinish = delegate
							{
								FxStopNetworked(ssCloneEffect);
								b.Destroy();
							}
						});
						yield return new WaitForSeconds(0.6f);
					}
				});
				FxPlayNewNetworked(ssTelegraphEffect, startPos + normalized * (_boxTelegraphController.height / 2f), finalRot);
				yield return new SI.WaitForSeconds(ssAtkInterval);
			}
			availableIndices.Clear();
			yield return new SI.WaitForSeconds(ssWaveInterval);
		}
		yield return new SI.WaitForSeconds(tsDelay);
		Hero hero3 = Dew.SelectRandomAliveHero();
		Vector3 targetPos = Dew.GetPositionOnGround(hero3.agentPosition);
		base.info.caster.Teleport(base.info.caster, targetPos);
		FxPlayNetworked(tsStartTelegraphEffect, targetPos, null);
		yield return new SI.WaitForSeconds(tsAtkDelay);
		FxPlayNetworked(tsFirstAtkEffect, targetPos, null);
		base.info.caster.Visual.EnableRenderers();
		base.info.caster.Animation.PlayAbilityAnimation(tsLandingAnimation);
		base.info.caster.Control.StartDaze(tsPostDelay);
		base.info.caster.Control.freeMovement = false;
		tsDonutRange.transform.position = targetPos;
		ReadOnlySpan<Entity> entities = tsDonutRange.GetEntities(out var handle, tvDefaultHarmfulEffectTargets);
		for (int l = 0; l < entities.Length; l++)
		{
			Entity entity = entities[l];
			if (!entity.IsNullInactiveDeadOrKnockedOut())
			{
				CreateDamage(DamageData.SourceType.Default, tsFirstAtkDmgFactor).SetOriginPosition(targetPos).Dispatch(entity);
				entity.Visual.KnockUp(KnockUpStrength.Big, isFriendly: false);
			}
		}
		handle.Return();
		yield return new SI.WaitForSeconds(0.35f);
		for (int i = 0; i < donutCount; i++)
		{
			_arcTelegraphController.innerRadius = _arcTelegraphController.outerRadius;
			_arcTelegraphController.outerRadius = _arcTelegraphController.innerRadius + (float)donutRadiusAdder;
			FxPlayNewNetworked(tsDonutTelegraphEffect, targetPos, null);
			yield return new SI.WaitForSeconds(tsTelegraphInterval);
		}
		yield return new SI.WaitForSeconds(1f);
		int effectCount = 12;
		float effectScale = 0.1f;
		float angle = 360f / (float)effectCount;
		float outerRadius = _donutStartRadius;
		for (int i = 0; i < donutCount; i++)
		{
			float num2 = outerRadius;
			outerRadius += (float)donutRadiusAdder;
			tsDonutAtkEffect.transform.localScale *= 1f + effectScale * (float)i;
			float num3 = num2 + (outerRadius - num2) / 2f;
			for (int m = 0; m < effectCount + i * 3; m++)
			{
				Vector3 normalized2 = (Quaternion.AngleAxis(angle * (float)m, Vector3.up) * Vector3.forward).Flattened().normalized;
				Vector3 vector = targetPos + normalized2 * num3;
				FxPlayNewNetworked(tsDonutAtkEffect, vector, null);
			}
			_donutCollider.GeneratePolygonPoints_Donut(num2, outerRadius);
			_donutCollider.UpdateProxyCollider();
			entities = tsDonutRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int n = 0; n < entities.Length; n++)
			{
				Entity entity2 = entities[n];
				if (!entity2.IsNullInactiveDeadOrKnockedOut())
				{
					CreateDamage(DamageData.SourceType.Default, tsDonutDmgFactor).SetOriginPosition(_roomCenterPos).Dispatch(entity2);
				}
			}
			handle.Return();
			yield return new SI.WaitForSeconds(tsTelegraphInterval / 2f);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
