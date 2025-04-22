using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Q_GoldenBurst : AbilityInstance
{
	public ScalingValue damageAmount;

	public ScalingValue healAmount;

	public GameObject fxHit;

	public GameObject fxActualHit;

	public ScalingValue selfDamageBaseRatio;

	public ScalingValue selfDamagePerWitherRatio;

	public float singleTargetBonusAmp = 1f;

	public FxCameraShake shake;

	public DewAudioSource baseAudio;

	public AnimationCurve baseAudioPitch;

	public AnimationCurve baseAudioVolume;

	public DewAudioSource flavourAudio;

	public AnimationCurve flavourAudioPitch;

	public AnimationCurve flavourAudioVolume;

	public AnimationCurve shakeMultiplier;

	public DewCollider range;

	public float GetSelfDamageAmount(Entity e)
	{
		if (e.Status.TryGetStatusEffect<Se_Q_GoldenBurst_Wither>(out var effect))
		{
			return (selfDamageBaseRatio.GetValue(base.skillLevel, e) + selfDamagePerWitherRatio.GetValue(base.skillLevel, e) * (float)effect.stack) * e.maxHealth;
		}
		return selfDamageBaseRatio.GetValue(base.skillLevel, e) * e.maxHealth;
	}

	protected override void OnCreate()
	{
		int num = 0;
		if (base.info.caster.Status.TryGetStatusEffect<Se_Q_GoldenBurst_Wither>(out var effect))
		{
			num = effect.stack;
		}
		baseAudio.pitchMultiplier *= baseAudioPitch.Evaluate(num);
		baseAudio.volumeMultiplier *= baseAudioVolume.Evaluate(num);
		flavourAudio.pitchMultiplier *= flavourAudioPitch.Evaluate(num);
		flavourAudio.volumeMultiplier *= flavourAudioVolume.Evaluate(num);
		shake.amplitude *= shakeMultiplier.Evaluate(num);
		startEffectNoStop.transform.localScale *= 1f + (float)num * 0.1f;
		range.transform.localScale *= 1f + (float)num * 0.1f;
		base.OnCreate();
	}

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		float num = GetSelfDamageAmount(base.info.caster);
		if (num > base.info.caster.currentHealth - 1f)
		{
			num = base.info.caster.currentHealth - 1f;
		}
		if (num > 0f)
		{
			PureDamage(num, 0f).SetAttr(DamageAttribute.IgnoreShield).Dispatch(base.info.caster);
		}
		IEnumerable<List<Entity>> enumerable = range.SweepEntitiesFromOrigin(3);
		ListReturnHandle<Entity> handle;
		List<Entity> list = DewPool.GetList(out handle);
		foreach (List<Entity> ents in enumerable)
		{
			yield return new SI.WaitForSeconds(0.01f);
			foreach (Entity item in ents)
			{
				if (!(item == base.info.caster) && !list.Contains(item) && (tvDefaultHarmfulEffectTargets.Evaluate(item) || tvDefaultUsefulEffectTargets.Evaluate(item)))
				{
					list.Add(item);
					FxPlayNewNetworked(fxHit, item);
				}
			}
		}
		bool flag = list.Count <= 1;
		foreach (Entity item2 in list)
		{
			FxPlayNewNetworked(fxActualHit, item2);
			if (tvDefaultHarmfulEffectTargets.Evaluate(item2))
			{
				DamageData damageData = Damage(damageAmount, flag ? 1f : 0.75f).SetDirection(base.info.forward).SetElemental(ElementalType.Light);
				if (flag)
				{
					damageData.SetAttr(DamageAttribute.IsCrit);
					damageData.ApplyAmplification(singleTargetBonusAmp);
				}
				damageData.Dispatch(item2);
			}
			else if (tvDefaultUsefulEffectTargets.Evaluate(item2))
			{
				HealData healData = Heal(GetValue(healAmount));
				if (flag)
				{
					healData.SetCrit();
					healData.ApplyAmplification(singleTargetBonusAmp);
				}
				healData.Dispatch(item2);
			}
		}
		base.info.caster.Status.TryGetStatusEffect<Se_Q_GoldenBurst_Wither>(out var effect);
		if (effect != null)
		{
			effect.AddStack();
		}
		else
		{
			CreateStatusEffect<Se_Q_GoldenBurst_Wither>(base.info.caster);
		}
		Destroy();
		handle.Return();
	}

	private void MirrorProcessed()
	{
	}
}
