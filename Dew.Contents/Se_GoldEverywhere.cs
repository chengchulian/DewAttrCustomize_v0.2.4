using System;
using UnityEngine;

public class Se_GoldEverywhere : StatusEffect
{
	public StatBonus statBonus;

	public GameObject fxGoldExplosion;

	public bool hideModelOnDeath;

	public Vector2Int goldDropAmount;

	public float lesserGoldMultiplier;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoStatBonus(statBonus);
			base.victim.EntityEvent_OnDeath += new Action<EventInfoKill>(EntityEventOnDeath);
		}
	}

	private void EntityEventOnDeath(EventInfoKill obj)
	{
		FxPlayNewNetworked(fxGoldExplosion, base.victim);
		if (hideModelOnDeath)
		{
			base.victim.Visual.DisableRenderers();
		}
		float num = global::UnityEngine.Random.Range(goldDropAmount.x, goldDropAmount.y + 1);
		if (base.victim is Monster { type: Monster.MonsterType.Lesser })
		{
			num *= lesserGoldMultiplier;
		}
		NetworkedManagerBase<PickupManager>.instance.DropGold(isKillGold: true, isGivenByOtherPlayer: false, DewMath.RandomRoundToInt(num), base.victim.position);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null))
		{
			base.victim.EntityEvent_OnDeath -= new Action<EventInfoKill>(EntityEventOnDeath);
		}
	}

	private void MirrorProcessed()
	{
	}
}
