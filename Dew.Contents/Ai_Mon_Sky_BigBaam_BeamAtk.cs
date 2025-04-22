using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Sky_BigBaam_BeamAtk : AbilityInstance
{
	public AbilitySelfValidator channelValidator;

	public ScalingValue damage;

	public float beamDuration;

	public float damageInterval;

	public AnimationCurve distanceCurve;

	public DewBeamRenderer beamRenderer;

	public Vector3 startOffset;

	public float beamRadius;

	public float hitBoxLength;

	public GameObject hitEffect;

	public float endDaze;

	private Entity[] _affected;

	private ArrayReturnHandle<Entity> _affectedHandle;

	private int _affectedCount;

	private float _lastDamageCheckTime;

	[SyncVar]
	private float _beamDuration;

	public float Network_beamDuration
	{
		get
		{
			return _beamDuration;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _beamDuration, 32uL, null);
		}
	}

	protected override void OnPrepare()
	{
		base.OnPrepare();
		Network_beamDuration = beamDuration * base.info.caster.Status.attackSpeedMultiplier;
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		UpdatePosition();
		beamRenderer.enabled = true;
		if (base.isServer)
		{
			_affected = DewPool.GetArray(out _affectedHandle, 128);
			base.info.caster.Control.StartChannel(new Channel
			{
				blockedActions = Channel.BlockedAction.Everything,
				duration = _beamDuration,
				onComplete = base.Destroy,
				onCancel = base.Destroy
			}.AddValidation(channelValidator));
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		UpdatePosition();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || Time.time - _lastDamageCheckTime < damageInterval)
		{
			return;
		}
		_lastDamageCheckTime = Time.time;
		float time = Mathf.Clamp01((Time.time - base.creationTime) / _beamDuration);
		float num = distanceCurve.Evaluate(time);
		Vector3 vector = base.info.caster.position + base.info.caster.rotation * startOffset + base.info.forward * Mathf.Clamp(num - hitBoxLength, 0f, float.PositiveInfinity);
		Vector3 vector2 = base.info.caster.position + base.info.caster.rotation * startOffset + base.info.forward * num;
		Debug.DrawLine(vector, vector2, Color.red, 0.5f);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.SphereCastAllEntities(out handle, vector, beamRadius, vector2 - vector, (vector2 - vector).magnitude, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			bool flag = false;
			for (int j = 0; j < _affectedCount; j++)
			{
				if (_affected[j] == entity)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				FxPlayNewNetworked(hitEffect, entity);
				Damage(damage).SetElemental(ElementalType.Light).SetOriginPosition(base.info.caster.position).Dispatch(entity);
				if (_affectedCount < _affected.Length)
				{
					_affected[_affectedCount] = entity;
					_affectedCount++;
				}
			}
		}
		handle.Return();
	}

	private void UpdatePosition()
	{
		float time = Mathf.Clamp01((Time.time - base.creationTime) / _beamDuration);
		Vector3 vector = base.info.caster.position + base.info.caster.rotation * startOffset;
		Vector3 vector2 = vector + base.info.forward * distanceCurve.Evaluate(time);
		vector2 = Dew.GetPositionOnGround(vector2);
		beamRenderer.SetPoints(vector, vector2);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		beamRenderer.enabled = false;
		if (base.isServer)
		{
			if (_affected != null)
			{
				_affected = null;
				_affectedHandle.Return();
			}
			if (base.info.caster.isActive)
			{
				base.info.caster.Control.StartDaze(endDaze);
			}
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
			writer.WriteFloat(_beamDuration);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteFloat(_beamDuration);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _beamDuration, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _beamDuration, null, reader.ReadFloat());
		}
	}
}
