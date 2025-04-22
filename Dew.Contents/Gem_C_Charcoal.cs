using System.Collections;
using UnityEngine;

public class Gem_C_Charcoal : Gem
{
	public float delayMin = 0.15f;

	public float delayMax = 0.35f;

	public bool useProcChance;

	public float procChanceMultiplier = 2f;

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (!info.chain.DidReact(this) && base.owner.CheckEnemyOrNeutral(info.victim) && (!useProcChance || !(Random.value > info.damage.procCoefficient * procChanceMultiplier)))
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			info.actor.LockDestroy();
			yield return new WaitForSeconds(Random.Range(delayMin, delayMax));
			info.actor.UnlockDestroy();
			if (IsReady() && !(base.owner == null) && !(info.victim == null))
			{
				CreateAbilityInstanceWithSource(info.actor, base.owner.position, Quaternion.identity, new CastInfo(base.owner, info.victim), delegate(Ai_Gem_C_Charcoal_Projectile a)
				{
					a.chain = info.chain.New(this);
					if (!useProcChance)
					{
						a._strength = info.damage.procCoefficient;
					}
				});
				NotifyUse();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
