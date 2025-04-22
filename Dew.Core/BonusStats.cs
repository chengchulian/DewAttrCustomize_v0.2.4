using System;
using UnityEngine.Serialization;

[Serializable]
public struct BonusStats : IEquatable<BonusStats>
{
	public static readonly BonusStats Default;

	public float attackDamageFlat;

	public float attackDamagePercentage;

	public float abilityPowerFlat;

	public float abilityPowerPercentage;

	public float maxHealthFlat;

	public float maxHealthPercentage;

	public float maxManaFlat;

	public float maxManaPercentage;

	public float healthRegenFlat;

	public float healthRegenPercentage;

	public float manaRegenFlat;

	public float manaRegenPercentage;

	public float attackSpeedPercentage;

	public float critAmpFlat;

	public float critAmpPercentage;

	public float critChanceFlat;

	public float critChancePercentage;

	public float abilityHasteFlat;

	public float abilityHastePercentage;

	public float tenacityFlat;

	public float tenacityPercentage;

	public float movementSpeedPercentage;

	public float fireEffectAmpFlat;

	public float coldEffectAmpFlat;

	public float lightEffectAmpFlat;

	public float darkEffectAmpFlat;

	public float armorFlat;

	[FormerlySerializedAs("armorPercetnage")]
	public float armorPercentage;

	public static BonusStats operator +(BonusStats x, BonusStats y)
	{
		BonusStats result = default(BonusStats);
		result.attackDamageFlat = x.attackDamageFlat + y.attackDamageFlat;
		result.attackDamagePercentage = DewMath.MultiplyPercentageBonuses(x.attackDamagePercentage, y.attackDamagePercentage);
		result.abilityPowerFlat = x.abilityPowerFlat + y.abilityPowerFlat;
		result.abilityPowerPercentage = DewMath.MultiplyPercentageBonuses(x.abilityPowerPercentage, y.abilityPowerPercentage);
		result.maxHealthFlat = x.maxHealthFlat + y.maxHealthFlat;
		result.maxHealthPercentage = DewMath.MultiplyPercentageBonuses(x.maxHealthPercentage, y.maxHealthPercentage);
		result.maxManaFlat = x.maxManaFlat + y.maxManaFlat;
		result.maxManaPercentage = DewMath.MultiplyPercentageBonuses(x.maxManaPercentage, y.maxManaPercentage);
		result.healthRegenFlat = x.healthRegenFlat + y.healthRegenFlat;
		result.healthRegenPercentage = DewMath.MultiplyPercentageBonuses(x.healthRegenPercentage, y.healthRegenPercentage);
		result.manaRegenFlat = x.manaRegenFlat + y.manaRegenFlat;
		result.manaRegenPercentage = DewMath.MultiplyPercentageBonuses(x.manaRegenPercentage, y.manaRegenPercentage);
		result.attackSpeedPercentage = x.attackSpeedPercentage + y.attackSpeedPercentage;
		result.critAmpFlat = x.critAmpFlat + y.critAmpFlat;
		result.critAmpPercentage = x.critAmpPercentage + y.critAmpPercentage;
		result.critChanceFlat = x.critChanceFlat + y.critChanceFlat;
		result.critChancePercentage = DewMath.MultiplyPercentageBonuses(x.critChancePercentage, y.critChancePercentage);
		result.abilityHasteFlat = x.abilityHasteFlat + y.abilityHasteFlat;
		result.abilityHastePercentage = DewMath.MultiplyPercentageBonuses(x.abilityHastePercentage, y.abilityHastePercentage);
		result.tenacityFlat = x.tenacityFlat + y.tenacityFlat;
		result.tenacityPercentage = DewMath.MultiplyPercentageBonuses(x.tenacityPercentage, y.tenacityPercentage);
		result.movementSpeedPercentage = x.movementSpeedPercentage + y.movementSpeedPercentage;
		result.fireEffectAmpFlat = x.fireEffectAmpFlat + y.fireEffectAmpFlat;
		result.coldEffectAmpFlat = x.coldEffectAmpFlat + y.coldEffectAmpFlat;
		result.lightEffectAmpFlat = x.lightEffectAmpFlat + y.lightEffectAmpFlat;
		result.darkEffectAmpFlat = x.darkEffectAmpFlat + y.darkEffectAmpFlat;
		result.armorFlat = x.armorFlat + y.armorFlat;
		result.armorPercentage = DewMath.MultiplyPercentageBonuses(x.armorPercentage, y.armorPercentage);
		return result;
	}

	public static BonusStats operator +(BonusStats x, StatBonus y)
	{
		BonusStats result = default(BonusStats);
		result.attackDamageFlat = x.attackDamageFlat + y.attackDamageFlat;
		result.attackDamagePercentage = DewMath.MultiplyPercentageBonuses(x.attackDamagePercentage, y.attackDamagePercentage);
		result.abilityPowerFlat = x.abilityPowerFlat + y.abilityPowerFlat;
		result.abilityPowerPercentage = DewMath.MultiplyPercentageBonuses(x.abilityPowerPercentage, y.abilityPowerPercentage);
		result.maxHealthFlat = x.maxHealthFlat + y.maxHealthFlat;
		result.maxHealthPercentage = DewMath.MultiplyPercentageBonuses(x.maxHealthPercentage, y.maxHealthPercentage);
		result.maxManaFlat = x.maxManaFlat + y.maxManaFlat;
		result.maxManaPercentage = DewMath.MultiplyPercentageBonuses(x.maxManaPercentage, y.maxManaPercentage);
		result.healthRegenFlat = x.healthRegenFlat + y.healthRegenFlat;
		result.healthRegenPercentage = DewMath.MultiplyPercentageBonuses(x.healthRegenPercentage, y.healthRegenPercentage);
		result.manaRegenFlat = x.manaRegenFlat + y.manaRegenFlat;
		result.manaRegenPercentage = DewMath.MultiplyPercentageBonuses(x.manaRegenPercentage, y.manaRegenPercentage);
		result.attackSpeedPercentage = x.attackSpeedPercentage + y.attackSpeedPercentage;
		result.critAmpFlat = x.critAmpFlat + y.critAmpFlat;
		result.critAmpPercentage = x.critAmpPercentage + y.critAmpPercentage;
		result.critChanceFlat = x.critChanceFlat + y.critChanceFlat;
		result.critChancePercentage = DewMath.MultiplyPercentageBonuses(x.critChancePercentage, y.critChancePercentage);
		result.abilityHasteFlat = x.abilityHasteFlat + y.abilityHasteFlat;
		result.abilityHastePercentage = DewMath.MultiplyPercentageBonuses(x.abilityHastePercentage, y.abilityHastePercentage);
		result.tenacityFlat = x.tenacityFlat + y.tenacityFlat;
		result.tenacityPercentage = DewMath.MultiplyPercentageBonuses(x.tenacityPercentage, y.tenacityPercentage);
		result.movementSpeedPercentage = x.movementSpeedPercentage + y.movementSpeedPercentage;
		result.fireEffectAmpFlat = x.fireEffectAmpFlat + y.fireEffectAmpFlat;
		result.coldEffectAmpFlat = x.coldEffectAmpFlat + y.coldEffectAmpFlat;
		result.lightEffectAmpFlat = x.lightEffectAmpFlat + y.lightEffectAmpFlat;
		result.darkEffectAmpFlat = x.darkEffectAmpFlat + y.darkEffectAmpFlat;
		result.armorFlat = x.armorFlat + y.armorFlat;
		result.armorPercentage = DewMath.MultiplyPercentageBonuses(x.armorPercentage, y.armorPercentage);
		return result;
	}

	public bool Equals(BonusStats other)
	{
		if (attackDamageFlat.Equals(other.attackDamageFlat) && attackDamagePercentage.Equals(other.attackDamagePercentage) && abilityPowerFlat.Equals(other.abilityPowerFlat) && abilityPowerPercentage.Equals(other.abilityPowerPercentage) && maxHealthFlat.Equals(other.maxHealthFlat) && maxHealthPercentage.Equals(other.maxHealthPercentage) && maxManaFlat.Equals(other.maxManaFlat) && maxManaPercentage.Equals(other.maxManaPercentage) && healthRegenFlat.Equals(other.healthRegenFlat) && healthRegenPercentage.Equals(other.healthRegenPercentage) && manaRegenFlat.Equals(other.manaRegenFlat) && manaRegenPercentage.Equals(other.manaRegenPercentage) && attackSpeedPercentage.Equals(other.attackSpeedPercentage) && critAmpFlat.Equals(other.critAmpFlat) && critAmpPercentage.Equals(other.critAmpPercentage) && critChanceFlat.Equals(other.critChanceFlat) && critChancePercentage.Equals(other.critChancePercentage) && abilityHasteFlat.Equals(other.abilityHasteFlat) && abilityHastePercentage.Equals(other.abilityHastePercentage) && tenacityFlat.Equals(other.tenacityFlat) && tenacityPercentage.Equals(other.tenacityPercentage) && movementSpeedPercentage.Equals(other.movementSpeedPercentage) && fireEffectAmpFlat.Equals(other.fireEffectAmpFlat) && coldEffectAmpFlat.Equals(other.coldEffectAmpFlat) && lightEffectAmpFlat.Equals(other.lightEffectAmpFlat) && darkEffectAmpFlat.Equals(other.darkEffectAmpFlat) && armorFlat.Equals(other.armorFlat))
		{
			return armorPercentage.Equals(other.armorPercentage);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is BonusStats other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(attackDamageFlat);
		hashCode.Add(attackDamagePercentage);
		hashCode.Add(abilityPowerFlat);
		hashCode.Add(abilityPowerPercentage);
		hashCode.Add(maxHealthFlat);
		hashCode.Add(maxHealthPercentage);
		hashCode.Add(maxManaFlat);
		hashCode.Add(maxManaPercentage);
		hashCode.Add(healthRegenFlat);
		hashCode.Add(healthRegenPercentage);
		hashCode.Add(manaRegenFlat);
		hashCode.Add(manaRegenPercentage);
		hashCode.Add(attackSpeedPercentage);
		hashCode.Add(critAmpFlat);
		hashCode.Add(critAmpPercentage);
		hashCode.Add(critChanceFlat);
		hashCode.Add(critChancePercentage);
		hashCode.Add(abilityHasteFlat);
		hashCode.Add(abilityHastePercentage);
		hashCode.Add(tenacityFlat);
		hashCode.Add(tenacityPercentage);
		hashCode.Add(movementSpeedPercentage);
		hashCode.Add(fireEffectAmpFlat);
		hashCode.Add(coldEffectAmpFlat);
		hashCode.Add(lightEffectAmpFlat);
		hashCode.Add(darkEffectAmpFlat);
		hashCode.Add(armorFlat);
		hashCode.Add(armorPercentage);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(BonusStats left, BonusStats right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(BonusStats left, BonusStats right)
	{
		return !left.Equals(right);
	}
}
