using System;
using System.Collections;
using UnityEngine;

public class Ai_Mon_SnowMountain_IceElemental_DoubleSwipe : AbilityInstance
{
	public int swipeCount;

	public DewAnimationClip[] swipeAnimations;

	public GameObject[] swipeEffects;

	public GameObject[] hitEffects;

	public Dash[] swipeDashes;

	public float[] swipeIntervals;

	public float[] animationOffsetTimes;

	public DewCollider[] swipeRanges;

	public Knockback[] swipeKnockbacks;

	public float coldChance;

	public ScalingValue damage;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DestroyOnDeath(base.info.caster);
		float num = 0f;
		float[] array = swipeIntervals;
		foreach (float num2 in array)
		{
			num += num2;
		}
		base.info.caster.Control.StartChannel(new Channel
		{
			blockedActions = Channel.BlockedAction.Everything,
			duration = num,
			onCancel = delegate
			{
				if (base.isActive)
				{
					Destroy();
				}
			}
		});
		StartSequence(AnimationSequence());
		for (int j = 0; j < swipeCount; j++)
		{
			FxPlayNetworked(swipeEffects[j]);
			swipeDashes[j].ApplyByDirection(base.info.caster, base.info.forward, delegate(DispByDestination d)
			{
				d.affectedByMovementSpeed = true;
			});
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = swipeRanges[j].GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int k = 0; k < entities.Length; k++)
			{
				Entity entity = entities[k];
				FxPlayNewNetworked(hitEffects[j], entity);
				swipeKnockbacks[j].ApplyWithDirection(base.info.forward, entity);
				DamageData damageData = DefaultDamage(damage);
				if (global::UnityEngine.Random.value < coldChance)
				{
					damageData.SetElemental(ElementalType.Cold);
				}
				damageData.Dispatch(entity);
			}
			handle.Return();
			yield return new SI.WaitForSeconds(swipeIntervals[j]);
		}
		Destroy();
	}

	private IEnumerator AnimationSequence()
	{
		for (int i = 0; i < swipeCount; i++)
		{
			base.info.caster.Animation.PlayAbilityAnimation(swipeAnimations[i]);
			yield return new SI.WaitForSeconds(swipeIntervals[i] + animationOffsetTimes[i]);
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = base.info.caster.position;
	}

	private void MirrorProcessed()
	{
	}
}
