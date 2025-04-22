using System;
using System.Collections;
using UnityEngine;

public class Gem_E_Twilight : Gem
{
	public GameObject consumeStackEffect;

	public float delay;

	public ScalingValue dmgPerStack;

	public ScalingValue healPerStack;

	public int minStack = 3;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackFiredBeforePrepare += new Action<EventInfoAttackFired>(EmpowerAttack);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && oldOwner != null)
		{
			oldOwner.EntityEvent_OnAttackFiredBeforePrepare -= new Action<EventInfoAttackFired>(EmpowerAttack);
		}
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (base.owner.CheckEnemyOrNeutral(info.victim) && !info.chain.DidReact(this))
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			info.actor.LockDestroy();
			yield return new WaitForSeconds(delay);
			info.actor.UnlockDestroy();
			if (!(info.victim == null) && !(base.owner == null))
			{
				int elementalStack = info.victim.Status.GetElementalStack(ElementalType.Dark);
				if (elementalStack >= minStack)
				{
					FxPlayNewNetworked(consumeStackEffect, info.victim);
					info.actor.DefaultDamage(GetValue(dmgPerStack) * (float)elementalStack * info.damage.procCoefficient, info.damage.procCoefficient).SetAttr(DamageAttribute.IsCrit).Dispatch(info.victim, info.chain.New(this));
					new HealData(GetValue(healPerStack) * (float)elementalStack * info.damage.procCoefficient).SetActor(info.actor).SetCrit().SetCanMerge()
						.Dispatch(base.owner, info.chain.New(this));
					if (info.victim.Status.TryGetStatusEffect<Se_Elm_Dark>(out var effect))
					{
						effect.Destroy();
					}
					NotifyUse();
				}
			}
		}
	}

	private void EmpowerAttack(EventInfoAttackFired obj)
	{
		obj.instance.dealtDamageProcessor.Add(delegate(ref DamageData data, Actor actor, Entity target)
		{
			data.SetElemental(ElementalType.Dark);
		}, 70);
	}

	private void MirrorProcessed()
	{
	}
}
