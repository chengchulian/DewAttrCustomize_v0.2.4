using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Q_SuperNova : AbilityInstance
{
	public float maxChannelTime;

	public float chargeFullTime;

	public float selfSlowAmount;

	public bool blockAttacksAndAbility;

	public bool continuousRotateOverride;

	public float chargeFullGraceThreshold;

	public DewAnimationClip chargeAnimation;

	public DewAnimationClip endAnimation;

	public GameObject endEffectOnCaster;

	public ScalingValue damage;

	public ScalingValue maxDamageMultiplier;

	public DewCollider range;

	public AnimationCurve scaleCurve;

	public AnimationCurve explodePitchCurve;

	public AnimationCurve explodeVolumeCurve;

	public AnimationCurve shakeMagnitudeCurve;

	public Transform[] scaledTransforms;

	public GameObject fullyChargedEffect;

	public GameObject explodeEffect;

	public GameObject critExplodeEffect;

	public GameObject explodeHitEffect;

	public FxCameraShake shake;

	private float _originalShake;

	[NonSerialized]
	[SyncVar]
	public float scaleMultiplier = 1f;

	[SyncVar]
	private float _currentCharge;

	private St_Q_SuperNova _trigger;

	private StatusEffect _selfSlow;

	private Vector3[] _originalScales;

	private DewAudioSource[] _explodeAudioSources;

	private Channel _channel;

	private OnScreenTimerHandle _handle;

	public float NetworkscaleMultiplier
	{
		get
		{
			return scaleMultiplier;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref scaleMultiplier, 32uL, null);
		}
	}

	public float Network_currentCharge
	{
		get
		{
			return _currentCharge;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _currentCharge, 64uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		_trigger = (St_Q_SuperNova)base.firstTrigger;
		base.transform.position = base.info.point;
		_originalScales = new Vector3[scaledTransforms.Length];
		_explodeAudioSources = explodeEffect.GetComponentsInChildren<DewAudioSource>();
		for (int i = 0; i < scaledTransforms.Length; i++)
		{
			_originalScales[i] = scaledTransforms[i].localScale * scaleMultiplier;
		}
		_originalShake = shake.amplitude;
		if (base.info.caster.isOwned)
		{
			_handle = ShowOnScreenTimerLocally(new OnScreenTimerHandle
			{
				fillAmountGetter = () => _currentCharge
			});
		}
		if (base.isServer)
		{
			_trigger.ChangeConfigTimedOnce(1, maxChannelTime, OnUse, OnExpire, setFillAmount: false);
			if (selfSlowAmount > 0f)
			{
				_selfSlow = CreateBasicEffect(base.info.caster, new SlowEffect
				{
					strength = selfSlowAmount
				}, maxChannelTime, "supernova_selfslow");
			}
			if (blockAttacksAndAbility)
			{
				base.info.caster.Control.IncrementBlockCounters(Channel.BlockedAction.Ability | Channel.BlockedAction.Attack);
			}
		}
	}

	private void OnExpire()
	{
		Finish();
	}

	private void OnUse(EventInfoAbilityInstance obj)
	{
		Finish();
	}

	private void Finish()
	{
		if (!base.isActive)
		{
			return;
		}
		FxStopNetworked(fullyChargedEffect);
		FxPlayNetworked(explodeEffect);
		if (_currentCharge > chargeFullGraceThreshold)
		{
			Network_currentCharge = 1f;
		}
		if (_currentCharge > 0.99f)
		{
			FxPlayNetworked(critExplodeEffect);
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			DamageData damageData = Damage(damage).ApplyRawMultiplier(Mathf.Lerp(1f, GetValue(maxDamageMultiplier), _currentCharge)).SetElemental(ElementalType.Light).SetOriginPosition(base.transform.position);
			if (_currentCharge > 0.99f)
			{
				damageData.SetAttr(DamageAttribute.IsCrit);
			}
			damageData.Dispatch(entity);
			FxPlayNewNetworked(explodeHitEffect, entity);
		}
		handle.Return();
		if (base.info.caster != null && base.info.caster.isActive)
		{
			base.info.caster.Animation.StopAbilityAnimation(chargeAnimation);
			if (endAnimation != null)
			{
				base.info.caster.Animation.PlayAbilityAnimation(endAnimation);
			}
			FxPlayNetworked(endEffectOnCaster, base.info.caster);
		}
		Destroy();
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
			if (blockAttacksAndAbility)
			{
				base.info.caster.Control.DecrementBlockCounters(Channel.BlockedAction.Ability | Channel.BlockedAction.Attack);
			}
			if (_trigger != null)
			{
				_trigger.fillAmount = 0f;
			}
			if (_selfSlow != null && _selfSlow.isActive)
			{
				_selfSlow.Destroy();
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && _trigger != null)
		{
			float num = Mathf.MoveTowards(_currentCharge, 1f, dt / chargeFullTime);
			if (_currentCharge < 0.999f && num > 0.999f)
			{
				FxPlayNetworked(fullyChargedEffect);
			}
			Network_currentCharge = num;
			_trigger.fillAmount = _currentCharge;
		}
		for (int i = 0; i < scaledTransforms.Length; i++)
		{
			scaledTransforms[i].localScale = _originalScales[i] * scaleCurve.Evaluate(_currentCharge);
		}
		DewAudioSource[] explodeAudioSources = _explodeAudioSources;
		foreach (DewAudioSource obj in explodeAudioSources)
		{
			obj.pitchMultiplier = explodePitchCurve.Evaluate(_currentCharge);
			obj.volumeMultiplier = explodeVolumeCurve.Evaluate(_currentCharge);
		}
		shake.amplitude = shakeMagnitudeCurve.Evaluate(_currentCharge) * _originalShake;
		if (!base.isServer)
		{
			return;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int j = 0; j < entities.Length; j++)
		{
			Entity entity = entities[j];
			if (!entity.Status.hasUnstoppable)
			{
				if (entity.Status.TryGetStatusEffect<Se_Q_SuperNova_Slow>(out var effect))
				{
					effect.ResetTimer();
				}
				else
				{
					CreateStatusEffect<Se_Q_SuperNova_Slow>(entity);
				}
			}
		}
		handle.Return();
		if (continuousRotateOverride && base.info.caster != null && base.info.caster.isActive)
		{
			base.info.caster.Control.RotateTowards(base.info.point, immediately: false, 0.5f);
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
			writer.WriteFloat(scaleMultiplier);
			writer.WriteFloat(_currentCharge);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteFloat(scaleMultiplier);
		}
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteFloat(_currentCharge);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref scaleMultiplier, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref scaleMultiplier, null, reader.ReadFloat());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _currentCharge, null, reader.ReadFloat());
		}
	}
}
