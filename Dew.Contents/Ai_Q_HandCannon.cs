using System;
using UnityEngine;

public class Ai_Q_HandCannon : AbilityInstance
{
	public ScalingValue factor;

	public ScalingValue closeFactor;

	public DewCollider farRange;

	public DewCollider closeRange;

	public GameObject hitEffect;

	public bool enableKnockbackCloseRange;

	public Knockback knockback;

	public bool enableRecoil = true;

	public float recoilDuration = 0.5f;

	public float recoilDistance = 0.5f;

	public DewEase recoilEase = DewEase.EaseOutQuad;

	[NonSerialized]
	public bool alwaysKnockback;

	[NonSerialized]
	public float nearDamageAmp;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		if (enableRecoil)
		{
			DispByDestination disp = new DispByDestination
			{
				destination = base.info.caster.position - base.info.forward * recoilDistance,
				duration = recoilDuration,
				ease = recoilEase,
				isFriendly = true,
				rotateForward = false,
				canGoOverTerrain = false,
				isCanceledByCC = true
			};
			base.info.caster.Control.StartDisplacement(disp);
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = closeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		ArrayReturnHandle<Entity> handle2;
		ReadOnlySpan<Entity> entities2 = farRange.GetEntities(out handle2, tvDefaultHarmfulEffectTargets);
		ReadOnlySpan<Entity> readOnlySpan = entities;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			Damage(closeFactor).SetElemental(ElementalType.Fire).SetAttr(DamageAttribute.IsCrit).ApplyAmplification(nearDamageAmp)
				.Dispatch(entity);
			if (enableKnockbackCloseRange)
			{
				knockback.ApplyWithDirection(base.info.forward, entity);
			}
			FxPlayNewNetworked(hitEffect, entity);
		}
		readOnlySpan = entities2;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity2 = readOnlySpan[i];
			if (!entities.Contains(entity2))
			{
				Damage(factor).SetElemental(ElementalType.Fire).Dispatch(entity2);
				if (alwaysKnockback)
				{
					knockback.ApplyWithDirection(base.info.forward, entity2);
				}
				FxPlayNewNetworked(hitEffect, entity2);
			}
		}
		handle.Return();
		handle2.Return();
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
