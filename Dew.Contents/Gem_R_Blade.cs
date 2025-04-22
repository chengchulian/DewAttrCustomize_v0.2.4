using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_R_Blade : Gem
{
	private struct Ad_BladeImmunity
	{
		public Dictionary<Entity, float> applyTime;
	}

	public ScalingValue baseCount;

	public float delay;

	public AnimationCurve intervalCurve;

	public float perEnemyCooldown;

	public DewAudioClip castVoice;

	private float _reducedCooldownTime;

	protected override void Awake()
	{
		base.Awake();
		ClientGemEvent_OnCooldownReduced += new Action<float>(ClientGemEventOnCooldownReduced);
		ClientGemEvent_OnCooldownReducedByRatio += new Action<float>(ClientGemEventOnCooldownReducedByRatio);
	}

	private void ClientGemEventOnCooldownReduced(float obj)
	{
		_reducedCooldownTime += obj;
	}

	private void ClientGemEventOnCooldownReducedByRatio(float obj)
	{
		_reducedCooldownTime += obj * perEnemyCooldown;
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		int count;
		float interval;
		if (!info.chain.DidReact(this) && base.owner.CheckEnemyOrNeutral(info.victim))
		{
			count = Mathf.RoundToInt(GetValue(baseCount));
			interval = intervalCurve.Evaluate(count);
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			info.actor.LockDestroy();
			if (IsReady() && !(base.owner == null) && !(info.victim == null))
			{
				if (!IsReady() || !base.isValid || !base.owner.CheckEnemyOrNeutral(info.victim) || info.victim == null || base.owner == null)
				{
					info.actor.UnlockDestroy();
				}
				else
				{
					if (info.victim.TryGetData<Ad_BladeImmunity>(out var data))
					{
						if (data.applyTime.TryGetValue(base.owner, out var value) && Time.time + _reducedCooldownTime - value < perEnemyCooldown)
						{
							info.actor.UnlockDestroy();
							yield break;
						}
						data.applyTime[base.owner] = Time.time + _reducedCooldownTime;
					}
					else
					{
						info.victim.AddData(new Ad_BladeImmunity
						{
							applyTime = new Dictionary<Entity, float> { 
							{
								base.owner,
								Time.time + _reducedCooldownTime
							} }
						});
					}
					base.owner.Sound.Say(castVoice, interruptPrevious: true);
					yield return new WaitForSeconds(delay);
					for (int i = 0; i < count; i++)
					{
						if (info.victim.IsNullInactiveDeadOrKnockedOut())
						{
							break;
						}
						CreateAbilityInstanceWithSource(info.actor, base.owner.position, Quaternion.identity, new CastInfo(base.owner, info.victim), delegate(Ai_Gem_R_Blade a)
						{
							a.chain = info.chain.New(this);
						});
						NotifyUse();
						yield return new WaitForSeconds(interval);
					}
					info.actor.UnlockDestroy();
				}
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
