using System;
using UnityEngine;

[Serializable]
public struct Cost
{
	public int gold;

	public int dreamDust;

	public int stardust;

	public int healthPercentage;

	public AffordType CanAfford(Entity entity)
	{
		if (entity.owner.gold < gold)
		{
			return AffordType.NoGold;
		}
		if (entity.currentHealth / entity.maxHealth < (float)healthPercentage / 100f)
		{
			return AffordType.NoHealth;
		}
		return AffordType.Yes;
	}

	public static Cost Gold(int amount)
	{
		Cost result = default(Cost);
		result.gold = amount;
		return result;
	}

	public static Cost DreamDust(int amount)
	{
		Cost result = default(Cost);
		result.dreamDust = amount;
		return result;
	}

	public static Cost Stardust(int amount)
	{
		Cost result = default(Cost);
		result.stardust = amount;
		return result;
	}

	public static Cost HealthPercentage(int amount)
	{
		Cost result = default(Cost);
		result.healthPercentage = amount;
		return result;
	}

	public override string ToString()
	{
		if (gold == 0 && dreamDust == 0 && stardust == 0 && healthPercentage == 0)
		{
			return DewLocalization.GetUIValue("Generic_Free_NoCost");
		}
		string res = "";
		if (gold != 0)
		{
			if (res != "")
			{
				res += ", ";
			}
			res += string.Format(DewLocalization.GetUIValue("Currency_Template_Gold"), Mathf.Abs(gold).ToString("#,##0"));
		}
		if (dreamDust != 0)
		{
			if (res != "")
			{
				res += ", ";
			}
			res += string.Format(DewLocalization.GetUIValue("Currency_Template_DreamDust"), Mathf.Abs(dreamDust).ToString("#,##0"));
		}
		if (stardust != 0)
		{
			if (res != "")
			{
				res += ", ";
			}
			res += string.Format(DewLocalization.GetUIValue("Currency_Template_Stardust"), Mathf.Abs(stardust).ToString("#,##0"));
		}
		if (healthPercentage != 0)
		{
			if (res != "")
			{
				res += ", ";
			}
			res += string.Format(DewLocalization.GetUIValue("Currency_Template_HealthPercentage"), Mathf.Abs(healthPercentage).ToString("#,##0"));
		}
		return res;
	}

	public static Cost operator +(Cost a, Cost b)
	{
		Cost result = default(Cost);
		result.gold = a.gold + b.gold;
		result.dreamDust = a.dreamDust + b.dreamDust;
		result.stardust = a.stardust + b.stardust;
		result.healthPercentage = a.healthPercentage + b.healthPercentage;
		return result;
	}

	public static FloatCost operator +(Cost a, FloatCost b)
	{
		FloatCost result = default(FloatCost);
		result.gold = (float)a.gold + b.gold;
		result.dreamDust = (float)a.dreamDust + b.dreamDust;
		result.stardust = (float)a.stardust + b.stardust;
		result.healthPercentage = (float)a.healthPercentage + b.healthPercentage;
		return result;
	}

	public static Cost operator -(Cost a, Cost b)
	{
		Cost result = default(Cost);
		result.gold = a.gold - b.gold;
		result.dreamDust = a.dreamDust - b.dreamDust;
		result.stardust = a.stardust - b.stardust;
		result.healthPercentage = a.healthPercentage - b.healthPercentage;
		return result;
	}

	public static FloatCost operator -(Cost a, FloatCost b)
	{
		FloatCost result = default(FloatCost);
		result.gold = (float)a.gold - b.gold;
		result.dreamDust = (float)a.dreamDust - b.dreamDust;
		result.stardust = (float)a.stardust - b.stardust;
		result.healthPercentage = (float)a.healthPercentage - b.healthPercentage;
		return result;
	}

	public static Cost operator *(Cost a, int scalar)
	{
		Cost result = default(Cost);
		result.gold = a.gold * scalar;
		result.dreamDust = a.dreamDust * scalar;
		result.stardust = a.stardust * scalar;
		result.healthPercentage = a.healthPercentage * scalar;
		return result;
	}

	public static Cost operator *(int scalar, Cost a)
	{
		return a * scalar;
	}

	public static FloatCost operator *(Cost a, float scalar)
	{
		FloatCost result = default(FloatCost);
		result.gold = (float)a.gold * scalar;
		result.dreamDust = (float)a.dreamDust * scalar;
		result.stardust = (float)a.stardust * scalar;
		result.healthPercentage = (float)a.healthPercentage * scalar;
		return result;
	}

	public static FloatCost operator *(float scalar, Cost a)
	{
		return a * scalar;
	}

	public static FloatCost operator /(Cost a, float scalar)
	{
		FloatCost result = default(FloatCost);
		result.gold = (float)a.gold / scalar;
		result.dreamDust = (float)a.dreamDust / scalar;
		result.stardust = (float)a.stardust / scalar;
		result.healthPercentage = (float)a.healthPercentage / scalar;
		return result;
	}

	public static Cost operator /(Cost a, int scalar)
	{
		Cost result = default(Cost);
		result.gold = a.gold / scalar;
		result.dreamDust = a.dreamDust / scalar;
		result.stardust = a.stardust / scalar;
		result.healthPercentage = a.healthPercentage / scalar;
		return result;
	}
}
