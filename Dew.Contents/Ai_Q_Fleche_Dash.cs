using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Ai_Q_Fleche_Dash : AbilityInstance
{
	public GameObject dashEffect;

	public GameObject lightningStartEffect;

	public GameObject lightningDashEffect;

	public GameObject lightningHitEffect;

	public float channelMinDist;

	public float channelDuration;

	public DewAnimationClip channelAnim;

	public DewAnimationClip dashAnim;

	public DewAnimationClip hitAnim;

	public GameObject landEffect;

	public GameObject hitEffectOnTarget;

	public bool doAttackEffects;

	public bool resetAttackCooldown;

	public bool doChase;

	public ScalingValue damage;

	public float speed;

	public float goalDistance;

	public float maxTime;

	public float unstoppableTime;

	public float speedAmount;

	public float speedDuration;

	public bool isSpeedDecay;

	public float trackerAttachTime;

	[NonSerialized]
	[SyncVar]
	public bool empoweredWithLightning;

	private Se_Q_Fleche_VictimTracker _tracker;

	public bool NetworkempoweredWithLightning
	{
		get
		{
			return empoweredWithLightning;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref empoweredWithLightning, 32uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		base.transform.SetPositionAndRotation(base.info.caster.position, Quaternion.LookRotation(base.info.target.position - base.info.caster.position).Flattened());
		if (base.isServer)
		{
			if (Vector3.Distance(base.info.caster.position, base.info.target.position) > channelMinDist)
			{
				base.info.caster.Control.StartChannel(new Channel
				{
					duration = (empoweredWithLightning ? (channelDuration * 0.5f) : channelDuration),
					blockedActions = Channel.BlockedAction.Everything,
					onCancel = base.Destroy,
					onComplete = DoDash
				});
				base.info.caster.Animation.PlayAbilityAnimation(channelAnim);
			}
			else
			{
				DoDash();
			}
			if (empoweredWithLightning)
			{
				FxPlayNetworked(lightningStartEffect, base.info.caster);
			}
			base.info.caster.Control.RotateTowards(base.info.target, immediately: true, 1f);
		}
	}

	private void DoDash()
	{
		if (base.firstTrigger is St_Q_Fleche { isActive: not false } st_Q_Fleche)
		{
			st_Q_Fleche.refundList[base.info.target] = Time.time;
		}
		if (resetAttackCooldown)
		{
			base.info.caster.Ability.attackAbility.ResetCooldown();
		}
		if (base.info.target.Status.TryGetStatusEffect<Se_Q_Fleche_VictimTracker>(out var effect))
		{
			effect._isBeingReplaced = true;
			effect.Destroy();
		}
		_tracker = CreateStatusEffect<Se_Q_Fleche_VictimTracker>(base.info.target, new CastInfo(base.info.caster));
		if (unstoppableTime > 0f)
		{
			CreateBasicEffect(base.info.caster, new UnstoppableEffect(), unstoppableTime, "fleche_unstoppable");
		}
		float goalDist = goalDistance;
		float num = Vector2.Distance(base.info.caster.agentPosition.ToXY(), base.info.target.agentPosition.ToXY()) - base.info.caster.Control.outerRadius - base.info.target.Control.outerRadius;
		if (goalDist > num)
		{
			goalDist = num;
		}
		if (goalDist < 0.1f)
		{
			goalDist = 0.1f;
		}
		if (empoweredWithLightning)
		{
			FxPlayNetworked(lightningDashEffect, base.info.caster);
			base.info.caster.Control.StartChannel(new Channel
			{
				duration = 0.02f,
				blockedActions = Channel.BlockedAction.Everything,
				onCancel = base.Destroy,
				onComplete = delegate
				{
					Vector3 end = base.info.target.agentPosition + (base.info.caster.agentPosition - base.info.target.agentPosition).normalized * (goalDist + base.info.target.Control.outerRadius + base.info.caster.Control.outerRadius);
					end = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, end);
					Teleport(base.info.caster, end);
					Finish();
				}
			});
			return;
		}
		FxPlayNetworked(dashEffect, base.info.caster);
		base.info.caster.Animation.PlayAbilityAnimation(dashAnim);
		if (Dew.GetNavMeshPathStatus(base.info.caster.agentPosition, base.info.target.agentPosition) == NavMeshPathStatus.PathComplete)
		{
			base.info.caster.Control.StartDisplacement(new DispByTarget
			{
				affectedByMovementSpeed = true,
				speed = speed,
				cancelTime = maxTime,
				goalDistance = goalDist,
				isCanceledByCC = false,
				isFriendly = true,
				onCancel = base.Destroy,
				onFinish = Finish,
				rotateForward = true,
				target = base.info.target
			});
		}
		else
		{
			Vector3 end2 = base.info.target.agentPosition + (base.info.caster.agentPosition - base.info.target.agentPosition).normalized * goalDist;
			end2 = Dew.GetValidAgentDestination_Closest(base.info.caster.agentPosition, end2);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = true,
				duration = Vector3.Distance(end2, base.info.caster.agentPosition) / speed,
				isCanceledByCC = false,
				isFriendly = true,
				onCancel = base.Destroy,
				onFinish = Finish,
				rotateForward = true,
				destination = end2
			});
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.SetPositionAndRotation(base.info.caster.position, Quaternion.LookRotation(base.info.target.position - base.info.caster.position).Flattened());
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			base.info.caster.Animation.StopAbilityAnimation(dashAnim);
			FxStopNetworked(dashEffect);
		}
	}

	private void Finish()
	{
		base.info.caster.Animation.PlayAbilityAnimation(hitAnim);
		FxPlayNetworked(landEffect);
		FxPlayNetworked(hitEffectOnTarget, base.info.target);
		if (empoweredWithLightning)
		{
			FxPlayNetworked(lightningHitEffect, base.info.target);
		}
		DamageData damageData = Damage(damage).SetOriginPosition(base.info.caster.position).DoAttackEffect(AttackEffectType.Others, doAttackEffects ? 1f : 0f);
		if (empoweredWithLightning)
		{
			damageData.SetElemental(ElementalType.Light);
		}
		damageData.Dispatch(base.info.target);
		if (doChase && !base.info.target.IsNullInactiveDeadOrKnockedOut() && base.info.caster.Ability.attackAbility.configs[0].targetValidator.Evaluate(base.info.caster, base.info.target))
		{
			StartCoroutine(Routine());
		}
		if (speedAmount > 0f && speedDuration > 0f)
		{
			CreateBasicEffect(base.info.caster, new SpeedEffect
			{
				decay = isSpeedDecay,
				strength = speedAmount
			}, speedDuration, "fleche_speed");
		}
		if (base.firstTrigger is St_Q_Fleche { isActive: not false } st_Q_Fleche && st_Q_Fleche.refundList.ContainsKey(base.info.target))
		{
			st_Q_Fleche.refundList[base.info.target] = Time.time;
		}
		if (_tracker != null && _tracker.isActive)
		{
			_tracker.SetTimer(trackerAttachTime);
		}
		Destroy();
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(base.info.caster.Ability.attackAbility.configs[0].channel.duration / base.info.caster.Status.attackSpeedMultiplier);
			if (base.info.caster.Ability.attackAbility is AttackTrigger attackTrigger)
			{
				attackTrigger.UpdateConfigIndexForCrit();
			}
			base.info.caster.Ability.attackAbility.OnCastComplete(base.info.caster.Ability.attackAbility.currentConfigIndex, new CastInfo(base.info.caster, base.info.target));
		}
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(empoweredWithLightning);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteBool(empoweredWithLightning);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref empoweredWithLightning, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref empoweredWithLightning, null, reader.ReadBool());
		}
	}
}
