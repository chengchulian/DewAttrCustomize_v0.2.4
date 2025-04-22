using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_Special_BossLightElemental_Beam : AbilityInstance
{
	[NonSerialized]
	[SyncVar]
	public Vector2 angle;

	[NonSerialized]
	[SyncVar]
	public float duration;

	[NonSerialized]
	[SyncVar]
	public float damageMultiplier = 1f;

	public DewCollider range;

	public DewEase ease;

	public float frontDistance;

	public float damageCheckInterval;

	public float damageInterval;

	public float yOffset;

	public ScalingValue damage;

	public GameObject beamEffect;

	public GameObject beamHitEffect;

	public GameObject destinationEffect;

	public LineRenderer lineRenderer;

	public float beamInterval;

	private EaseFunction _easeFunc;

	private float _lastDamageTime;

	private bool _atTarget;

	private float _lastBeamInterval;

	private Dictionary<Entity, float> _hitTimes = new Dictionary<Entity, float>();

	private float _currentValue => _easeFunc(0f, 1f, Mathf.Clamp01((Time.time - base.creationTime) / duration));

	public Vector2 Networkangle
	{
		get
		{
			return angle;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref angle, 32uL, null);
		}
	}

	public float Networkduration
	{
		get
		{
			return duration;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref duration, 64uL, null);
		}
	}

	public float NetworkdamageMultiplier
	{
		get
		{
			return damageMultiplier;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref damageMultiplier, 128uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		_easeFunc = EasingFunction.GetEasingFunction(ease);
		SolvePosition();
		FxPlay(beamEffect);
		FxPlay(destinationEffect);
		lineRenderer.enabled = true;
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		FxStop(beamEffect);
		FxStop(destinationEffect);
		lineRenderer.enabled = false;
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		SolvePosition();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if (Time.time - base.creationTime > duration)
		{
			Destroy();
		}
		else
		{
			if (Time.time - _lastDamageTime < damageCheckInterval)
			{
				return;
			}
			_lastDamageTime = Time.time;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				if (!_hitTimes.ContainsKey(entity) || !(Time.time - _hitTimes[entity] < damageInterval))
				{
					_hitTimes[entity] = Time.time;
					FxPlayNewNetworked(beamHitEffect, entity);
					Damage(damage).ApplyRawMultiplier(damageMultiplier).SetElemental(ElementalType.Light).SetOriginPosition(base.info.caster.position)
						.Dispatch(entity);
				}
			}
			handle.Return();
		}
	}

	private void SolvePosition()
	{
		float y = Mathf.Lerp(angle.x, angle.y, _currentValue);
		base.transform.position = base.info.caster.position + Quaternion.Euler(0f, y, 0f) * Vector3.forward * frontDistance + Vector3.up * yOffset;
		base.transform.rotation = Quaternion.Euler(0f, y, 0f);
		Vector3 vector = base.transform.position + base.transform.forward * 100f;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out var hitInfo, 100f, LayerMasks.Ground))
		{
			vector = hitInfo.point;
		}
		lineRenderer.SetPosition(0, base.transform.position);
		lineRenderer.SetPosition(1, vector);
		destinationEffect.transform.position = vector;
		if (Time.time - _lastBeamInterval > beamInterval)
		{
			_atTarget = !_atTarget;
			beamEffect.transform.position = (_atTarget ? vector : base.transform.position);
			_lastBeamInterval = Time.time;
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
			writer.WriteVector2(angle);
			writer.WriteFloat(duration);
			writer.WriteFloat(damageMultiplier);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteVector2(angle);
		}
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteFloat(duration);
		}
		if ((base.syncVarDirtyBits & 0x80L) != 0L)
		{
			writer.WriteFloat(damageMultiplier);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref angle, null, reader.ReadVector2());
			GeneratedSyncVarDeserialize(ref duration, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref damageMultiplier, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref angle, null, reader.ReadVector2());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref duration, null, reader.ReadFloat());
		}
		if ((num & 0x80L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref damageMultiplier, null, reader.ReadFloat());
		}
	}
}
