using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_MiniBoss_SpinningArrow : MiniBossEffect
{
	public float startDelay = 2f;

	public float angleStep = 13f;

	public float shootInterval = 0.2f;

	public bool disableOnDash;

	public float disableArrowsDuration = 0.5f;

	public GameObject fxAfterFirstShoot;

	public Transform angleTransform;

	private bool _didSetup;

	[SyncVar(hook = "OnAngleChanged")]
	private float _angle;

	private float _lastShootTime;

	private float _disableTime;

	public float Network_angle
	{
		get
		{
			return _angle;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _angle, 512uL, OnAngleChanged);
		}
	}

	private void OnAngleChanged(float oldV, float newV)
	{
		angleTransform.rotation = Quaternion.Euler(0f, _angle, 0f);
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			base.victim.Control.ClientEvent_OnTeleport += new Action<Vector3, Vector3>(ClientEventOnTeleport);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (!base.isServer && base.victim != null)
		{
			base.victim.Control.ClientEvent_OnTeleport -= new Action<Vector3, Vector3>(ClientEventOnTeleport);
		}
		FxStop(fxAfterFirstShoot);
	}

	private void ClientEventOnTeleport(Vector3 arg1, Vector3 arg2)
	{
		DisableArrows();
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || Time.time - base.creationTime < startDelay)
		{
			return;
		}
		if (!_didSetup)
		{
			Network_angle = CastInfo.GetAngle(base.victim.position - Dew.GetClosestAliveHero(base.victim.position).position);
			_didSetup = true;
			FxPlayNetworked(fxAfterFirstShoot);
		}
		if (!(Time.time - _lastShootTime < shootInterval))
		{
			Network_angle = _angle + angleStep;
			_lastShootTime = Time.time;
			if (base.victim.Visual.isSpawning || (disableOnDash && base.victim.Control.isDashing))
			{
				DisableArrows();
			}
			else if (!(Time.time < _disableTime))
			{
				CreateAbilityInstance<Ai_MiniBoss_SpinningArrow_Arrow>(base.victim.position, null, new CastInfo(base.victim, _angle));
			}
		}
	}

	private void DisableArrows()
	{
		_disableTime = Time.time + disableArrowsDuration;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_angle);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteFloat(_angle);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _angle, OnAngleChanged, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _angle, OnAngleChanged, reader.ReadFloat());
		}
	}
}
