using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Se_C_IceBlock : StatusEffect
{
	public GameObject fxStart;

	public GameObject fxEnd;

	public ScalingValue dmgFactor;

	public ScalingValue shieldFactor;

	public ScalingValue perHealFactor;

	public Knockback knockback;

	public DewCollider range;

	public float duration;

	public GameObject hitEffect;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		ListReturnHandle<Se_C_IceBlock> handle;
		List<Se_C_IceBlock> list = DewPool.GetList(out handle);
		foreach (StatusEffect statusEffect in base.victim.Status.statusEffects)
		{
			if (statusEffect is Se_C_IceBlock item && !(statusEffect == this))
			{
				list.Add(item);
			}
		}
		list.Sort((Se_C_IceBlock x, Se_C_IceBlock y) => x.creationTime.CompareTo(y.creationTime));
		int num = list.Count - 4;
		for (int i = 0; i < num; i++)
		{
			list[i].DestroyIfActive();
		}
		handle.Return();
		ShowOnScreenTimer();
		if (base.info.target != base.info.caster)
		{
			CreateAbilityInstance<Ai_C_IceBlock_Projectile>(base.info.caster.position, null, base.info);
		}
		FxPlayNetworked(fxStart, base.info.target);
		DoShield(GetValue(shieldFactor), delegate(EventInfoDamageNegatedByShield obj)
		{
			if (obj.shield.amount < 0.001f)
			{
				DestroyIfActive();
			}
		});
		SetTimer(duration);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (!base.isServer)
		{
			return;
		}
		FxStopNetworked(fxStart);
		int len;
		if (!(base.info.target == null))
		{
			FxPlayNetworked(fxEnd, base.info.target);
			range.transform.position = base.info.target.position;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			ReadOnlySpan<Entity> readOnlySpan = entities;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				Entity entity = readOnlySpan[i];
				FxPlayNewNetworked(hitEffect, entity);
				Damage(dmgFactor).SetElemental(ElementalType.Cold).Dispatch(entity);
				knockback.ApplyWithOrigin(base.info.target.position, entity);
			}
			len = entities.Length;
			StartCoroutine(Routine());
			handle.Return();
		}
		IEnumerator Routine()
		{
			yield return null;
			Heal(GetValue(perHealFactor) * (float)len).Dispatch(base.info.target);
		}
	}

	private void MirrorProcessed()
	{
	}
}
