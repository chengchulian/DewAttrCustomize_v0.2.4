using System;
using UnityEngine;

[Serializable]
public struct FloatCost
{
	public float gold;

	public float dreamDust;

	public float stardust;

	public float healthPercentage;

	public static implicit operator FloatCost(Cost cost)
	{
		FloatCost result = default(FloatCost);
		result.gold = cost.gold;
		result.dreamDust = cost.dreamDust;
		result.stardust = cost.stardust;
		result.healthPercentage = cost.healthPercentage;
		return result;
	}

	public static explicit operator Cost(FloatCost floatCost)
	{
		Cost result = default(Cost);
		result.gold = Mathf.RoundToInt(floatCost.gold);
		result.dreamDust = Mathf.RoundToInt(floatCost.dreamDust);
		result.stardust = Mathf.RoundToInt(floatCost.stardust);
		result.healthPercentage = Mathf.RoundToInt(floatCost.healthPercentage);
		return result;
	}

	public static FloatCost operator +(FloatCost a, FloatCost b)
	{
		FloatCost result = default(FloatCost);
		result.gold = a.gold + b.gold;
		result.dreamDust = a.dreamDust + b.dreamDust;
		result.stardust = a.stardust + b.stardust;
		result.healthPercentage = a.healthPercentage + b.healthPercentage;
		return result;
	}

	public static FloatCost operator +(FloatCost a, Cost b)
	{
		return a + (FloatCost)b;
	}

	public static FloatCost operator -(FloatCost a, FloatCost b)
	{
		FloatCost result = default(FloatCost);
		result.gold = a.gold - b.gold;
		result.dreamDust = a.dreamDust - b.dreamDust;
		result.stardust = a.stardust - b.stardust;
		result.healthPercentage = a.healthPercentage - b.healthPercentage;
		return result;
	}

	public static FloatCost operator -(FloatCost a, Cost b)
	{
		return a - (FloatCost)b;
	}

	public static FloatCost operator *(FloatCost a, float scalar)
	{
		FloatCost result = default(FloatCost);
		result.gold = a.gold * scalar;
		result.dreamDust = a.dreamDust * scalar;
		result.stardust = a.stardust * scalar;
		result.healthPercentage = a.healthPercentage * scalar;
		return result;
	}

	public static FloatCost operator *(float scalar, FloatCost a)
	{
		return a * scalar;
	}

	public static FloatCost operator /(FloatCost a, float scalar)
	{
		FloatCost result = default(FloatCost);
		result.gold = a.gold / scalar;
		result.dreamDust = a.dreamDust / scalar;
		result.stardust = a.stardust / scalar;
		result.healthPercentage = a.healthPercentage / scalar;
		return result;
	}
}
