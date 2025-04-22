using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_R_PrecisionShot_Projectile : StandardProjectile
{
	[NonSerialized]
	[SyncVar]
	internal float chargeAmount;

	public ScalingValue damageMin;

	public ScalingValue damageMax;

	public float critThreshold = 0.5f;

	public float procCoefficient = 1f;

	public float dmgAmpPerHit;

	public float stunDurationMin;

	public float stunDurationMax;

	public bool doKnockback;

	public Knockback targetKnockback;

	public DewAudioSource[] adjustedSounds;

	public AnimationCurve soundPitch;

	public AnimationCurve soundVolume;

	public DewAudioSource reverb;

	public AnimationCurve reverbVolume;

	public Transform[] scaleAdjustedTransforms;

	public AnimationCurve transformScale;

	public Light[] adjustedLights;

	public AnimationCurve lightIntensity;

	public FxCameraShake[] adjustedShakes;

	public AnimationCurve shakeAmplitude;

	[NonSerialized]
	public bool isBiggerShot;

	private float _currentDmgAmp;

	public float NetworkchargeAmount
	{
		get
		{
			return chargeAmount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref chargeAmount, 65536uL, null);
		}
	}

	protected override void OnCreate()
	{
		float pitchMultiplier = soundPitch.Evaluate(chargeAmount);
		float volumeMultiplier = soundVolume.Evaluate(chargeAmount);
		DewAudioSource[] array = adjustedSounds;
		foreach (DewAudioSource dewAudioSource in array)
		{
			dewAudioSource.pitchMultiplier = pitchMultiplier;
			dewAudioSource.volumeMultiplier = volumeMultiplier;
			if (isBiggerShot)
			{
				dewAudioSource.pitchMultiplier *= 0.85f;
			}
		}
		reverb.volumeMultiplier = reverbVolume.Evaluate(chargeAmount);
		float num = transformScale.Evaluate(chargeAmount);
		Transform[] array2 = scaleAdjustedTransforms;
		foreach (Transform transform in array2)
		{
			transform.localScale *= num;
			if (isBiggerShot)
			{
				transform.localScale *= 1.3f;
			}
		}
		float num2 = lightIntensity.Evaluate(chargeAmount);
		Light[] array3 = adjustedLights;
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i].intensity *= num2;
		}
		float num3 = shakeAmplitude.Evaluate(chargeAmount);
		FxCameraShake[] array4 = adjustedShakes;
		foreach (FxCameraShake fxCameraShake in array4)
		{
			fxCameraShake.amplitude *= num3;
			if (isBiggerShot)
			{
				fxCameraShake.amplitude *= 1.2f;
			}
		}
		base.OnCreate();
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		ScalingValue value = ScalingValue.Lerp(damageMin, damageMax, chargeAmount);
		float num = Mathf.Lerp(stunDurationMin, stunDurationMax, chargeAmount);
		DamageData damageData = Damage(value, procCoefficient).SetDirection(base.info.forward);
		if (chargeAmount > critThreshold || _currentDmgAmp > 0.01f)
		{
			damageData.SetAttr(DamageAttribute.IsCrit);
		}
		damageData.ApplyAmplification(_currentDmgAmp);
		damageData.Dispatch(hit.entity);
		if (doKnockback)
		{
			targetKnockback.ApplyWithDirection(base.info.forward, hit.entity);
		}
		if (num > 0.001f)
		{
			CreateBasicEffect(hit.entity, new StunEffect(), num, "precisionshot_stun");
		}
		_currentDmgAmp += dmgAmpPerHit;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(chargeAmount);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x10000L) != 0L)
		{
			writer.WriteFloat(chargeAmount);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref chargeAmount, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x10000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref chargeAmount, null, reader.ReadFloat());
		}
	}
}
