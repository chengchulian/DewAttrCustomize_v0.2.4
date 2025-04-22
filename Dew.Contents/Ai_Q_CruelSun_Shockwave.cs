using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Q_CruelSun_Shockwave : InstantDamageInstance
{
	public DewAudioSource[] audios;

	public FxCameraShake[] shakes;

	public AnimationCurve audioPitch;

	public AnimationCurve audioVolume;

	public Transform[] effectTransforms;

	public float stunDurationMin = 0.5f;

	public float stunDurationMax = 2f;

	public ScalingValue damageMin;

	public ScalingValue damageMax;

	[SyncVar]
	internal float _length;

	[SyncVar]
	internal float _width;

	[SyncVar]
	internal float _chargeAmount;

	public float Network_length
	{
		get
		{
			return _length;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _length, 64uL, null);
		}
	}

	public float Network_width
	{
		get
		{
			return _width;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _width, 128uL, null);
		}
	}

	public float Network_chargeAmount
	{
		get
		{
			return _chargeAmount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _chargeAmount, 256uL, null);
		}
	}

	protected override void OnCreate()
	{
		Vector3 b = new Vector3(_width, 1f, _length);
		if (base.isServer)
		{
			range.transform.localScale = Vector3.Scale(range.transform.localScale, b);
			dmgFactor = ScalingValue.Lerp(damageMin, damageMax, _chargeAmount);
			knockupAmount *= Mathf.Lerp(0.2f, 1f, _chargeAmount);
		}
		DewAudioSource[] array = audios;
		foreach (DewAudioSource obj in array)
		{
			obj.pitchMultiplier *= audioPitch.Evaluate(_chargeAmount);
			obj.volumeMultiplier *= audioVolume.Evaluate(_chargeAmount);
		}
		FxCameraShake[] array2 = shakes;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].amplitude *= Mathf.Lerp(0.25f, 1f, _chargeAmount);
		}
		for (int j = 0; j < effectTransforms.Length; j++)
		{
			Transform obj2 = effectTransforms[j];
			obj2.localScale *= _width * 0.5f;
			obj2.localPosition = Vector3.forward * _length * ((float)(j + 1) / (float)(effectTransforms.Length + 1));
		}
		FxPointLight[] componentsInChildren = GetComponentsInChildren<FxPointLight>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].intensityMultiplier *= _chargeAmount;
		}
		startEffectNoStop.transform.localScale *= Mathf.Lerp(0.9f, 1.5f, _chargeAmount);
		base.OnCreate();
	}

	protected override void OnBeforeDispatchDamage(ref DamageData dmg, Entity target)
	{
		base.OnBeforeDispatchDamage(ref dmg, target);
		if (_chargeAmount > 0.99f)
		{
			dmg.SetAttr(DamageAttribute.IsCrit);
		}
	}

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		CreateBasicEffect(entity, new StunEffect(), Mathf.Lerp(stunDurationMin, stunDurationMax, _chargeAmount), "SuppressionStun", DuplicateEffectBehavior.UsePrevious);
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_length);
			writer.WriteFloat(_width);
			writer.WriteFloat(_chargeAmount);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteFloat(_length);
		}
		if ((base.syncVarDirtyBits & 0x80L) != 0L)
		{
			writer.WriteFloat(_width);
		}
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteFloat(_chargeAmount);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _length, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _width, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _chargeAmount, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _length, null, reader.ReadFloat());
		}
		if ((num & 0x80L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _width, null, reader.ReadFloat());
		}
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _chargeAmount, null, reader.ReadFloat());
		}
	}
}
