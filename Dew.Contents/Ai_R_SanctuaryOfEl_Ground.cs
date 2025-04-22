using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_R_SanctuaryOfEl_Ground : AbilityInstance
{
	public float groundDuration;

	public float radius;

	public float checkInterval;

	public GameObject changePositionEffect;

	public float positionSmoothTime;

	public float damageDelay;

	public ScalingValue startDamage;

	public ScalingValue startShield;

	public GameObject damageHitEffect;

	public float aboutToExpireEffectDelay;

	public GameObject aboutToExpireEffect;

	public float shieldDuration = 4f;

	private OnScreenTimerHandle _handle;

	private float _lastCheckTime = float.NegativeInfinity;

	[SyncVar]
	private Vector3 _desiredPosition;

	private Vector3 _cv;

	private AbilityTrigger _trigger;

	public float fillAmount => 1f - (Time.time - base.creationTime) / groundDuration;

	public Vector3 Network_desiredPosition
	{
		get
		{
			return _desiredPosition;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _desiredPosition, 32uL, null);
		}
	}

	protected override IEnumerator OnCreateSequenced()
	{
		base.transform.SetPositionAndRotation(base.info.caster.agentPosition, ManagerBase<CameraManager>.instance.entityCamAngleRotation);
		if (base.info.caster.isOwned)
		{
			_handle = ShowOnScreenTimerLocally(new OnScreenTimerHandle
			{
				fillAmountGetter = () => fillAmount
			});
		}
		if (!base.isServer)
		{
			yield break;
		}
		_trigger = base.firstTrigger;
		Network_desiredPosition = base.info.caster.agentPosition;
		GiveShield(base.info.caster, GetValue(startShield), shieldDuration);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.transform.position, radius, tvDefaultUsefulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			GiveShield(entity, GetValue(startShield), shieldDuration);
			FxPlayNewNetworked(damageHitEffect, entity);
		}
		handle.Return();
		yield return new SI.WaitForSeconds(damageDelay);
		ArrayReturnHandle<Entity> handle2;
		ReadOnlySpan<Entity> readOnlySpan2 = DewPhysics.OverlapCircleAllEntities(out handle2, base.transform.position, radius, tvDefaultHarmfulEffectTargets);
		for (int j = 0; j < readOnlySpan2.Length; j++)
		{
			Entity entity2 = readOnlySpan2[j];
			Damage(startDamage).SetElemental(ElementalType.Light).SetOriginPosition(base.transform.position).Dispatch(entity2);
			FxPlayNewNetworked(damageHitEffect, entity2);
		}
		handle2.Return();
		yield return new SI.WaitForSeconds(0.7f);
		base.firstTrigger.ChangeConfigTimedOnce(1, groundDuration, delegate
		{
			if (base.isActive)
			{
				Network_desiredPosition = base.info.caster.agentPosition;
				FxPlayNetworked(changePositionEffect);
			}
		});
		yield return new SI.WaitForSeconds(aboutToExpireEffectDelay - damageDelay - 0.7f);
		FxPlayNetworked(aboutToExpireEffect);
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		base.transform.position = Vector3.SmoothDamp(base.transform.position, _desiredPosition, ref _cv, positionSmoothTime);
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if (_trigger.IsNullOrInactive() || _trigger.owner == null)
		{
			Destroy();
		}
		else if (Time.time - base.creationTime > groundDuration)
		{
			Destroy();
		}
		else
		{
			if (!(Time.time - _lastCheckTime > checkInterval))
			{
				return;
			}
			_lastCheckTime = Time.time;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.transform.position, radius, tvDefaultUsefulEffectTargets);
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				Entity entity = readOnlySpan[i];
				if (entity.Status.TryGetStatusEffect<Se_R_SanctuaryOfEl_Buff>(out var effect))
				{
					effect.ResetTimer();
				}
				else
				{
					CreateStatusEffect<Se_R_SanctuaryOfEl_Buff>(entity);
				}
			}
			handle.Return();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_handle != null)
		{
			HideOnScreenTimerLocally(_handle);
			_handle = null;
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
			writer.WriteVector3(_desiredPosition);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteVector3(_desiredPosition);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _desiredPosition, null, reader.ReadVector3());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _desiredPosition, null, reader.ReadVector3());
		}
	}
}
