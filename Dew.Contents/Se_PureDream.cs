using System;
using UnityEngine;

public class Se_PureDream : StatusEffect
{
	public StatBonus statBonus;

	public GameObject fxdreamExplosion;

	public bool hideModelOnDeath;

	public Vector2Int dreamDustDropAmount;

	public float lesserDreamDustMultiplier;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.victim.Visual.deathEffect != null)
		{
			base.victim.Visual.deathEffect = null;
		}
		if (base.isServer)
		{
			DoStatBonus(statBonus);
			base.victim.EntityEvent_OnDeath += new Action<EventInfoKill>(EntityEventOnDeath);
		}
	}

	private void EntityEventOnDeath(EventInfoKill obj)
	{
		FxPlayNewNetworked(fxdreamExplosion, base.victim);
		if (hideModelOnDeath)
		{
			base.victim.Visual.DisableRenderers();
		}
		float num = global::UnityEngine.Random.Range(dreamDustDropAmount.x, dreamDustDropAmount.y + 1);
		if (base.victim is Monster { type: Monster.MonsterType.Lesser })
		{
			num *= lesserDreamDustMultiplier;
		}
		NetworkedManagerBase<PickupManager>.instance.DropDreamDust(isGivenByOtherPlayer: false, DewMath.RandomRoundToInt(num), base.victim.position);
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
