using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_L_PureWhite : Gem
{
	public GameObject castEffect;

	public float initDelay;

	public float interval;

	public float maxShootTime;

	public int minProjectiles;

	protected override void OnCastCompleteBeforePrepare(EventInfoCast info)
	{
		base.OnCastCompleteBeforePrepare(info);
		if (!IsReady() || !info.trigger.configs[info.configIndex].canConsumeCastBonus)
		{
			return;
		}
		List<Entity> affected = new List<Entity>();
		info.instance.ActorEvent_OnDealDamage += (Action<EventInfoDamage>)delegate(EventInfoDamage obj)
		{
			if (base.isValid && !obj.chain.DidReact(this) && base.owner.CheckEnemyOrNeutral(obj.victim))
			{
				StartCoroutine(Routine());
			}
			IEnumerator Routine()
			{
				obj.actor.LockDestroy();
				yield return new WaitForSeconds(initDelay);
				if (base.isValid && minProjectiles + obj.victim.Status.lightStack > 0 && !affected.Contains(obj.victim))
				{
					obj.actor.UnlockDestroy();
					affected.Add(obj.victim);
					int count = minProjectiles + obj.victim.Status.lightStack;
					float adjustedInterval = Mathf.Min(interval, maxShootTime / (float)count);
					for (int i = 0; i < count; i++)
					{
						if (!base.isValid)
						{
							break;
						}
						if (obj.victim.IsNullInactiveDeadOrKnockedOut())
						{
							break;
						}
						CreateAbilityInstanceWithSource(obj.actor, base.owner.position, Quaternion.identity, new CastInfo(base.owner, obj.victim), delegate(Ai_Gem_L_PureWhite_Projectile p)
						{
							p.damageData = obj.damage;
							p.chain = obj.chain.New(this);
						});
						yield return new WaitForSeconds(adjustedInterval);
					}
				}
			}
		};
		NotifyUse();
		StartCooldown();
		FxPlayNewNetworked(castEffect, base.owner);
	}

	private void MirrorProcessed()
	{
	}
}
