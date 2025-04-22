using System;
using UnityEngine;

public class St_D_Resolve : SkillTrigger
{
	public GameObject chargeEffectOnWeapon;

	public GameObject chargeEffectOnlyOwner;

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackFired += new Action<EventInfoAttackFired>(CheckFourth);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer)
		{
			formerOwner.EntityEvent_OnAttackFired -= new Action<EventInfoAttackFired>(CheckFourth);
		}
	}

	private void CheckFourth(EventInfoAttackFired obj)
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
			CreateBasicEffect(base.owner, be, float.PositiveInfinity, "ResolveCrit");
			FxPlayNetworked(chargeEffectOnWeapon, base.owner);
			FxPlayNewNetworked(chargeEffectOnlyOwner, base.owner);
		}
		if (obj.everyFourAttackIndex == 3)
		{
			Vector3 forward = ((obj.info.target != null) ? (obj.info.target.position - base.owner.position).Flattened().normalized : (Quaternion.Euler(0f, obj.info.angle, 0f) * Vector3.forward));
			if (forward.sqrMagnitude < 0.001f)
			{
				forward = base.owner.transform.forward;
			}
			Quaternion value = Quaternion.LookRotation(forward);
			CreateAbilityInstance(base.owner.position, value, new CastInfo(base.owner, value.eulerAngles.y), delegate(Ai_D_Resolve_Push p)
			{
				p.attack = obj.instance as MeleeAttackInstance;
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
