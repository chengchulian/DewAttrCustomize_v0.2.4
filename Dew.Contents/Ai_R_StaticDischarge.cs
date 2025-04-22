using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_R_StaticDischarge : AbilityInstance
{
	public DewAnimationClip startAnim;

	public DewAnimationClip dischargeAnim;

	public ChargingChannelData channel;

	public ScalingValue minDamage;

	public ScalingValue maxDamage;

	public DewCollider range;

	public float slowAmount;

	public float slowDuration;

	public GameObject chargeEffect;

	public GameObject explodeEffect;

	public GameObject hitEffect;

	public float procCoefficient;

	public DewAudioSource[] adjustedAudios;

	public Transform[] adjustedTransforms;

	public AnimationCurve pitchMultiplier;

	public AnimationCurve volumeMultiplier;

	public AnimationCurve scaleMultiplier;

	public FxCameraShake shake;

	public AnimationCurve shakeMultiplier;

	public float endDaze;

	public bool doKnockback;

	public Knockback knockback;

	public Vector2 knockbackDist;

	public float knockbackDistFarThreshold;

	public AnimationCurve knockbackMultiplier;

	public FxPointLight explodeLight;

	public AnimationCurve intensityMultiplier;

	[SyncVar]
	private float _currentCharge;

	private ChargingChannel _channel;

	private OnScreenTimerHandle _handle;

	private float _originalRadius;

	public float Network_currentCharge
	{
		get
		{
			return _currentCharge;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _currentCharge, 32uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.info.caster.isOwned)
		{
			_handle = ShowOnScreenTimerLocally(new OnScreenTimerHandle
			{
				fillAmountGetter = () => _currentCharge
			});
		}
		if (base.isServer)
		{
			base.info.caster.Animation.PlayAbilityAnimation(startAnim);
			FxPlayNetworked(chargeEffect, base.info.caster);
			_channel = channel.Get(base.netIdentity).OnCancel(DoDischarge).OnCast(DoDischarge)
				.OnComplete(DoDischarge);
			_originalRadius = _channel.castMethod._radius;
			_channel.castMethod._radius *= scaleMultiplier.Evaluate(0f);
			_channel.Dispatch(base.info.caster, base.firstTrigger);
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (!(base.info.caster == null))
		{
			base.transform.position = base.info.caster.position;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && _channel != null)
		{
			Network_currentCharge = _channel.chargeAmount;
			_channel.castMethod._radius = _originalRadius * scaleMultiplier.Evaluate(_currentCharge);
			_channel.UpdateCastMethod();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_handle != null)
		{
			HideOnScreenTimerLocally(_handle);
		}
		if (base.isServer)
		{
			FxStopNetworked(chargeEffect);
		}
	}

	private void DoDischarge(ChargingChannel c)
	{
		Vector3 vector = base.info.caster.position;
		float num = c.chargeAmount;
		if ((double)num > 0.9)
		{
			num = 1f;
		}
		RpcPlayExplodeEffect(vector, num);
		range.transform.localScale *= scaleMultiplier.Evaluate(num);
		ScalingValue value = ScalingValue.Lerp(minDamage, maxDamage, num);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		float num2 = knockbackMultiplier.Evaluate(num);
		ReadOnlySpan<Entity> readOnlySpan = entities;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			FxPlayNewNetworked(hitEffect, entity);
			Damage(value, procCoefficient).SetOriginPosition(vector).SetElemental(ElementalType.Light).Dispatch(entity);
			if (slowAmount > 0f)
			{
				CreateBasicEffect(entity, new SlowEffect
				{
					strength = slowAmount
				}, slowDuration, "staticdischarge_slow");
			}
			if (doKnockback)
			{
				float t = Mathf.Clamp01(Vector2.Distance(vector.ToXY(), entity.agentPosition.ToXY()) / knockbackDistFarThreshold);
				knockback.distance = Mathf.Lerp(knockbackDist.x, knockbackDist.y, t);
				knockback.distance *= num2;
				knockback.ApplyWithOrigin(vector, entity);
			}
		}
		handle.Return();
		base.info.caster.Animation.PlayAbilityAnimation(dischargeAnim);
		if (endDaze > 0f)
		{
			base.info.caster.Control.StartDaze(endDaze);
		}
		Destroy();
	}

	[ClientRpc]
	private void RpcPlayExplodeEffect(Vector3 pos, float strength)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		writer.WriteFloat(strength);
		SendRPCInternal("System.Void Ai_R_StaticDischarge::RpcPlayExplodeEffect(UnityEngine.Vector3,System.Single)", -1594235088, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcPlayExplodeEffect__Vector3__Single(Vector3 pos, float strength)
	{
		shake.amplitude *= shakeMultiplier.Evaluate(strength);
		float num = scaleMultiplier.Evaluate(strength);
		Transform[] array = adjustedTransforms;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].localScale *= num;
		}
		float num2 = pitchMultiplier.Evaluate(strength);
		float num3 = volumeMultiplier.Evaluate(strength);
		DewAudioSource[] array2 = adjustedAudios;
		foreach (DewAudioSource obj in array2)
		{
			obj.pitchMultiplier *= num2;
			obj.volumeMultiplier *= num3;
		}
		explodeLight.intensityMultiplier *= intensityMultiplier.Evaluate(strength);
		FxPlay(explodeEffect, pos, Quaternion.identity);
	}

	protected static void InvokeUserCode_RpcPlayExplodeEffect__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayExplodeEffect called on server.");
		}
		else
		{
			((Ai_R_StaticDischarge)obj).UserCode_RpcPlayExplodeEffect__Vector3__Single(reader.ReadVector3(), reader.ReadFloat());
		}
	}

	static Ai_R_StaticDischarge()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_R_StaticDischarge), "System.Void Ai_R_StaticDischarge::RpcPlayExplodeEffect(UnityEngine.Vector3,System.Single)", InvokeUserCode_RpcPlayExplodeEffect__Vector3__Single);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_currentCharge);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteFloat(_currentCharge);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
		}
	}
}
