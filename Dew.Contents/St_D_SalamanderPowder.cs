using System;
using UnityEngine;

public class St_D_SalamanderPowder : SkillTrigger
{
	public DewCollider range;

	public AbilityTargetValidator hittable;

	public GameObject explodeEffect;

	public GameObject chargeEffectOnWeapon;

	public GameObject chargeEffectOnlyOwner;

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackFired += new Action<EventInfoAttackFired>(CheckCritical);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer)
		{
			formerOwner.EntityEvent_OnAttackFired -= new Action<EventInfoAttackFired>(CheckCritical);
		}
	}

	private void CheckCritical(EventInfoAttackFired obj)
	{
		base.fillAmount = (float)((obj.everyFourAttackIndex + 1) % 4) / 3f;
		if (obj.everyFourAttackIndex == 2)
		{
			AttackCriticalEffect be = new AttackCriticalEffect();
			be.onUse = delegate
			{
				FxStopNetworked(chargeEffectOnWeapon);
				be.parent.Destroy();
			};
			CreateBasicEffect(base.owner, be, float.PositiveInfinity, "SalamanderCrit");
			FxPlayNetworked(chargeEffectOnWeapon, base.owner);
			FxPlayNewNetworked(chargeEffectOnlyOwner, base.owner);
		}
		if (obj.everyFourAttackIndex != 3)
		{
			return;
		}
		obj.instance.ActorEvent_OnAttackHit += (Action<EventInfoAttackHit>)delegate(EventInfoAttackHit hit)
		{
			range.transform.position = hit.victim.position;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, hittable, base.owner);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity target = entities[i];
				CreateAbilityInstance<Ai_D_SalamanderPowder_Projectile>(hit.victim.Visual.GetCenterPosition(), Quaternion.identity, new CastInfo(base.owner, target));
			}
			handle.Return();
			FxPlayNewNetworked(explodeEffect, hit.victim);
		};
	}

	private void MirrorProcessed()
	{
	}
}
