using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Sky_BossNyx_Blackhole : AbilityInstance
{
	public GameObject displaceEffect;

	public GameObject startTelegraphEffect;

	public DewAnimationClip animDisplace;

	public float displaceDuration;

	public DewEase displaceEase;

	public GameObject blackholeStartEffect;

	public GameObject blackholeRepeatedEffect;

	public float repeatedEffectInterval;

	public GameObject blackholeEndEffect;

	public float blackholeDuration;

	public float tickDamageRadius = 3.5f;

	public int blackholeStarfallCount;

	public float tickInterval;

	public float tickDamageRatio;

	public AnimationCurve dmgMultiplierOverLifetime;

	public Vector2 distanceBounds;

	public AnimationCurve attractStrengthByDist;

	public AnimationCurve attractStrengthMulOverLifetime;

	public float explodeDamageRadius = 4f;

	public float explodeDamageRatio;

	public DewAnimationClip animEnd;

	public float monsterDmgMultiplier;

	public float afterBlackholeDelay;

	[SyncVar]
	private bool _isBlackholeOn;

	[SyncVar]
	private float _blackholeStartTime;

	private float _lastTickTime;

	private bool _didTurnOffRenderer;

	private float _lastRepeatedEffectTime;

	public bool Network_isBlackholeOn
	{
		get
		{
			return _isBlackholeOn;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _isBlackholeOn, 32uL, null);
		}
	}

	public float Network_blackholeStartTime
	{
		get
		{
			return _blackholeStartTime;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _blackholeStartTime, 64uL, null);
		}
	}

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 1000f).DestroyOnDestroy(this);
		DestroyOnDeath(base.info.caster);
		if (SingletonBehaviour<Sky_BossRoomCenter>.instance != null)
		{
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = false,
				canGoOverTerrain = true,
				destination = SingletonBehaviour<Sky_BossRoomCenter>.instance.transform.position,
				duration = displaceDuration,
				ease = displaceEase,
				isCanceledByCC = false,
				isFriendly = true,
				rotateForward = false
			});
		}
		base.info.caster.Animation.PlayAbilityAnimation(animDisplace);
		base.info.caster.Control.Rotate(Vector3.back, immediately: false, 2f);
		FxPlayNetworked(displaceEffect, base.info.caster);
		FxPlayNewNetworked(startTelegraphEffect, (SingletonBehaviour<Sky_BossRoomCenter>.instance != null) ? SingletonBehaviour<Sky_BossRoomCenter>.instance.transform.position : base.info.caster.position, null);
		float num = displaceDuration + blackholeDuration + afterBlackholeDelay;
		base.info.caster.Control.StartChannel(new Channel
		{
			blockedActions = Channel.BlockedAction.Everything,
			duration = num,
			onCancel = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			}
		});
		base.info.caster.Control.Rotate(Vector3.back, immediately: false, num);
		yield return new SI.WaitForSeconds(displaceDuration);
		FxStopNetworked(displaceEffect);
		FxPlayNetworked(blackholeStartEffect, base.info.caster);
		Network_isBlackholeOn = true;
		Network_blackholeStartTime = Time.time;
		_didTurnOffRenderer = true;
		base.info.caster.Visual.DisableRenderers();
		for (int i = 0; i < blackholeStarfallCount; i++)
		{
			CreateAbilityInstance(base.info.caster.position, null, base.info, delegate(Ai_Mon_Sky_BossNyx_Starfall s)
			{
				s.starfallWavaCount = 1;
				s.starfallCount = 30;
			});
			yield return new SI.WaitForSeconds(blackholeDuration / (float)blackholeStarfallCount);
		}
		Network_isBlackholeOn = false;
		FxStopNetworked(blackholeStartEffect);
		FxPlayNetworked(blackholeEndEffect, base.info.caster);
		if (_didTurnOffRenderer)
		{
			base.info.caster.Visual.EnableRenderers();
			_didTurnOffRenderer = false;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.position, explodeDamageRadius);
		for (int j = 0; j < readOnlySpan.Length; j++)
		{
			Entity entity = readOnlySpan[j];
			if (!(entity == base.info.caster) && !entity.IsNullInactiveDeadOrKnockedOut())
			{
				float num2 = explodeDamageRatio * entity.maxHealth;
				if (entity is Monster)
				{
					num2 *= monsterDmgMultiplier;
				}
				DefaultDamage(num2).SetOriginPosition(base.position).Dispatch(entity);
			}
		}
		base.info.caster.Animation.PlayAbilityAnimation(animEnd);
		handle.Return();
		yield return new SI.WaitForSeconds(afterBlackholeDelay);
		Destroy();
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			FxStopNetworked(displaceEffect);
			FxStopNetworked(blackholeStartEffect);
			if (_didTurnOffRenderer && base.info.caster != null)
			{
				base.info.caster.Visual.EnableRenderers();
				_didTurnOffRenderer = false;
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!_isBlackholeOn)
		{
			return;
		}
		float time = Mathf.Clamp01((Time.time - _blackholeStartTime) / blackholeDuration);
		if (Time.time - _lastRepeatedEffectTime > repeatedEffectInterval)
		{
			_lastRepeatedEffectTime = Time.time;
			FxPlay(blackholeRepeatedEffect);
		}
		foreach (Entity allEntity in NetworkedManagerBase<ActorManager>.instance.allEntities)
		{
			if (!(allEntity == base.info.caster) && !allEntity.IsNullInactiveDeadOrKnockedOut() && allEntity.Control.isLocalMovementProcessor && !allEntity.Control.isDisplacing && !allEntity.Status.hasCrowdControlImmunity)
			{
				float time2 = Mathf.Clamp01((Vector2.Distance(base.position.ToXY(), allEntity.agentPosition.ToXY()) - distanceBounds.x) / (distanceBounds.y - distanceBounds.x));
				float num = attractStrengthByDist.Evaluate(time2);
				num *= attractStrengthMulOverLifetime.Evaluate(time);
				allEntity.Control.SetAgentPosition(allEntity.agentPosition + (base.position - allEntity.agentPosition).normalized * (num * dt));
			}
		}
		if (!base.isServer || !(Time.time - _lastTickTime > tickInterval))
		{
			return;
		}
		_lastTickTime = Time.time;
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.info.caster.position, tickDamageRadius);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!(entity == base.info.caster) && !entity.IsNullInactiveDeadOrKnockedOut())
			{
				float num2 = tickDamageRatio * entity.maxHealth * dmgMultiplierOverLifetime.Evaluate(time);
				if (entity is Monster)
				{
					num2 *= monsterDmgMultiplier;
				}
				DefaultDamage(num2).SetOriginPosition(base.position).Dispatch(entity);
			}
		}
		handle.Return();
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.position = base.info.caster.position;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_isBlackholeOn);
			writer.WriteFloat(_blackholeStartTime);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteBool(_isBlackholeOn);
		}
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteFloat(_blackholeStartTime);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _isBlackholeOn, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _blackholeStartTime, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _isBlackholeOn, null, reader.ReadBool());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _blackholeStartTime, null, reader.ReadFloat());
		}
	}
}
