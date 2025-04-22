using System;
using System.Collections;
using UnityEngine;

public class Ai_R_GreatFrostSword : AbilityInstance
{
	public ScalingValue dmgFactor;

	public Knockback knockback;

	public DewCollider range;

	public float singleTargetAmp = 1.5f;

	public GameObject fxSingleTargetAmp;

	public GameObject swordEffect;

	public GameObject swingEffect;

	public GameObject hitEffect;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	public float castingDuration;

	public float postDelay;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		base.info.caster.Control.StartDaze(postDelay + castingDuration);
		base.info.caster.Animation.PlayAbilityAnimation(startAnim);
		FxPlayNetworked(swordEffect, base.info.caster);
		yield return new SI.WaitForSeconds(castingDuration);
		base.info.caster.Animation.PlayAbilityAnimation(endAnim);
		FxPlayNetworked(swingEffect);
		yield return new SI.WaitForSeconds(0.1f);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			FxPlayNewNetworked(hitEffect, entity);
			DamageData damageData = Damage(dmgFactor).SetElemental(ElementalType.Cold);
			if (entities.Length == 1)
			{
				damageData.ApplyAmplification(singleTargetAmp);
				damageData.SetAttr(DamageAttribute.IsCrit);
				FxPlayNewNetworked(fxSingleTargetAmp, entity);
			}
			damageData.Dispatch(entity);
			knockback.ApplyWithDirection(base.info.forward, entity);
		}
		handle.Return();
		FxStopNetworked(swordEffect);
		yield return new SI.WaitForSeconds(postDelay);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
