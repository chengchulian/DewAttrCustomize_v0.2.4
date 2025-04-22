using System.Collections.Generic;
using UnityEngine;

public class Gem_E_Flexibility : Gem
{
	private struct PostponedDamage
	{
		public float damagePerTick;

		public int remainingTicks;
	}

	public int dmgTickCount;

	public float tickInterval;

	public ScalingValue dmgDivideAmount;

	public GameObject hitEffect;

	public float dmgReduceAmount;

	private float _lastCheckTime;

	private List<PostponedDamage> _postponedDamages = new List<PostponedDamage>();

	public float divMultiplier => 1f - 1f / (GetValue(dmgDivideAmount) + 1f);

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.takenDamageProcessor.Add(OnTakenDamage, 1000);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && !(oldOwner == null))
		{
			oldOwner.takenDamageProcessor.Remove(OnTakenDamage);
			float num = 0f;
			for (int num2 = _postponedDamages.Count - 1; num2 >= 0; num2--)
			{
				PostponedDamage postponedDamage = _postponedDamages[num2];
				num += postponedDamage.damagePerTick * (float)postponedDamage.remainingTicks;
				_postponedDamages.RemoveAt(num2);
			}
			if (!(num <= 0f))
			{
				PureDamage(num).SetAttr(DamageAttribute.DamageOverTime).Dispatch(oldOwner);
				FxPlayNewNetworked(hitEffect, oldOwner);
			}
		}
	}

	private void OnTakenDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (!base.isValid || data.isBlockedByImmunity || target.Status.hasDamageImmunity || base.skill == null || base.skill.currentCharges[0] >= base.skill.configs[0].maxCharges || data.actor == this)
		{
			return;
		}
		float currentAmount = data.currentAmount;
		data.ApplyRawMultiplier(1f - divMultiplier);
		float num = currentAmount * divMultiplier * dmgReduceAmount;
		if (!(num < 0.7f))
		{
			float num2 = num / (float)dmgTickCount;
			if (_postponedDamages.Count != 0 && _postponedDamages[^1].remainingTicks == dmgTickCount)
			{
				PostponedDamage value = _postponedDamages[^1];
				value.damagePerTick += num2;
				_postponedDamages[^1] = value;
			}
			else
			{
				_postponedDamages.Add(new PostponedDamage
				{
					damagePerTick = num2,
					remainingTicks = dmgTickCount
				});
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || _postponedDamages.Count <= 0)
		{
			return;
		}
		if (base.owner.IsNullInactiveDeadOrKnockedOut())
		{
			_postponedDamages.Clear();
		}
		else
		{
			if (!(Time.time - _lastCheckTime > tickInterval))
			{
				return;
			}
			_lastCheckTime = Time.time;
			float num = 0f;
			for (int num2 = _postponedDamages.Count - 1; num2 >= 0; num2--)
			{
				PostponedDamage value = _postponedDamages[num2];
				if (value.remainingTicks == 0)
				{
					_postponedDamages.RemoveAt(num2);
				}
				else
				{
					value.remainingTicks--;
					_postponedDamages[num2] = value;
					num += value.damagePerTick;
				}
			}
			PureDamage(num).SetAttr(DamageAttribute.DamageOverTime).Dispatch(base.owner);
			FxPlayNewNetworked(hitEffect, base.owner);
		}
	}

	private void MirrorProcessed()
	{
	}
}
