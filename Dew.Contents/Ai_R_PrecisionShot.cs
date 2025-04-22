using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_R_PrecisionShot : AbilityInstance
{
	public ChargingChannelData channel;

	public float lengthMin;

	public float lengthMax;

	public float cooldownRefundRatioOnCancel = 0.7f;

	public float fullGraceThreshold;

	public float cameraOffsetMultiplier = 1f;

	public int cameraZoomIndex;

	public float cameraRevertDelay = 0.1f;

	public float cameraRevertTime = 0.5f;

	public bool doUnstoppable;

	private ChargingChannel _channel;

	private CameraModifierOffset _offset;

	private CameraModifierZoom _zoom;

	private StatusEffect _unstoppable;

	[SyncVar]
	private float _chargeAmount;

	[SyncVar]
	private float _angle;

	public float Network_chargeAmount
	{
		get
		{
			return _chargeAmount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _chargeAmount, 32uL, null);
		}
	}

	public float Network_angle
	{
		get
		{
			return _angle;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _angle, 64uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_channel = channel.Get(base.netIdentity).SetInitialInfo(base.info).OnCast(OnCast)
				.OnCancel(delegate
				{
					ShootCanceled();
				})
				.OnComplete(delegate
				{
					ShootCanceled();
				})
				.Dispatch(base.info.caster, base.firstTrigger);
			if (doUnstoppable)
			{
				_unstoppable = CreateBasicEffect(base.info.caster, new UnstoppableEffect(), channel.completeDuration, "precision_unstoppable");
			}
		}
		if (base.info.caster.isOwned)
		{
			_offset = new CameraModifierOffset
			{
				offset = Vector3.zero
			}.Apply();
			_zoom = new CameraModifierZoom
			{
				zoomIndex = cameraZoomIndex
			}.Apply();
		}
	}

	private void ShootCanceled()
	{
		if (base.isActive)
		{
			Destroy();
		}
		if (base.firstTrigger != null)
		{
			base.firstTrigger.ApplyCooldownReductionByRatio(cooldownRefundRatioOnCancel);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		CameraModifierOffset o;
		if (_offset != null)
		{
			o = _offset;
			_offset = null;
			ManagerBase<CameraManager>.instance.StartCoroutine(Routine());
		}
		CameraModifierZoom z;
		if (_zoom != null)
		{
			z = _zoom;
			_zoom = null;
			ManagerBase<CameraManager>.instance.StartCoroutine(Routine());
		}
		if (base.isServer && _unstoppable != null && _unstoppable.isActive)
		{
			_unstoppable.Destroy();
			_unstoppable = null;
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(cameraRevertDelay);
			Vector3 startOffset = o.offset;
			for (float t = 0f; t < cameraRevertTime; t += Time.deltaTime)
			{
				o.offset = startOffset * ((cameraRevertTime - t) / cameraRevertTime);
				yield return null;
			}
			o.Remove();
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(cameraRevertDelay);
			z.Remove();
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			Network_chargeAmount = _channel.chargeAmount;
			Network_angle = _channel.castInfo.angle;
			_channel.castMethod._length = GetLength();
			_channel.UpdateCastMethod();
		}
		if (_offset != null && _angle != 0f)
		{
			_offset.offset = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward * GetLength() * cameraOffsetMultiplier;
		}
	}

	private void OnCast(ChargingChannel obj)
	{
		CreateAbilityInstance(base.info.caster.position, Quaternion.identity, new CastInfo(base.info.caster, obj.castInfo.angle), delegate(Ai_R_PrecisionShot_Projectile p)
		{
			p.NetworkchargeAmount = obj.chargeAmount;
			if (p.chargeAmount > fullGraceThreshold)
			{
				p.NetworkchargeAmount = 1f;
			}
			p.endDistance = GetLength();
		});
		Destroy();
	}

	private float GetLength()
	{
		return Mathf.Lerp(lengthMin, lengthMax, _chargeAmount);
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_chargeAmount);
			writer.WriteFloat(_angle);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteFloat(_chargeAmount);
		}
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteFloat(_angle);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _chargeAmount, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _angle, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _chargeAmount, null, reader.ReadFloat());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _angle, null, reader.ReadFloat());
		}
	}
}
