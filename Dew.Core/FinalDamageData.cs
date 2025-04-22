using UnityEngine;

public struct FinalDamageData
{
	public float amount;

	public float discardedAmount;

	public float procCoefficient;

	public AttackEffectType attackEffectType;

	public float attackEffectStrength;

	public ElementalType? elemental;

	public Vector3? direction;

	public DamageData.SourceType type;

	public DamageAttribute attributes;

	public int? overrideElementalStacks;

	internal ActorFlags _modifyFlags;

	public bool canMerge => (attributes & (DamageAttribute.DamageOverTime | DamageAttribute.ForceMergeNumber)) != 0;

	public FinalDamageData(DamageData data, float armorAmount, Entity victim)
	{
		float maximumAmount = victim.currentHealth + ((!data.HasAttr(DamageAttribute.IgnoreShield)) ? victim.Status.currentShield : 0f);
		amount = data.currentAmount;
		if (!data.HasAttr(DamageAttribute.IgnoreArmor) && data.source != DamageData.SourceType.Pure)
		{
			float multiplier = 100f / (100f + armorAmount);
			amount *= Mathf.Max(multiplier, 0f);
		}
		if (amount > maximumAmount)
		{
			discardedAmount = amount - maximumAmount;
			amount = maximumAmount;
		}
		else
		{
			discardedAmount = 0f;
		}
		if (data.direction.HasValue)
		{
			direction = data.direction.Value.normalized;
		}
		else if (data.originPosition.HasValue)
		{
			direction = (victim.position - data.originPosition.Value).normalized;
		}
		else
		{
			direction = null;
		}
		type = data.source;
		elemental = data.elemental;
		attackEffectType = data.attackEffectType;
		attackEffectStrength = data.attackEffectStrength;
		_modifyFlags = data._modifyFlags.ShallowCopy();
		procCoefficient = data.procCoefficient;
		attributes = data.attributes;
		overrideElementalStacks = data.overrideElementalStacks;
	}

	public bool HasAttr(DamageAttribute attr)
	{
		return attributes.HasFlag(attr);
	}
}
