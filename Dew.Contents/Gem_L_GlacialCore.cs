using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem_L_GlacialCore : Gem
{
	public float shootRadius;

	public GameObject healEffect;

	public GameObject healCritEffect;

	public ScalingValue healMaxHealthRatio;

	public float chillAmp = 1f;

	public float delay = 0.5f;

	public float shootMinDamage;

	public float shootMaxDamageMaxHpRatio;

	public AnimationCurve normalizedShootInterval;

	public float maxShootCount;

	public Vector2 procCoefficient;

	public ScalingValue healToDamageRatio;

	private float _lastShootTime;

	private Dictionary<ReactionChain, float> _remainingDamages = new Dictionary<ReactionChain, float>(new ReactionChainComparer());

	private List<ReactionChain> _keysToRemove = new List<ReactionChain>();

	private List<ReactionChain> _keysToDecrease = new List<ReactionChain>();

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded += new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(NetworkedManagerBase<ZoneManager>.instance == null))
		{
			NetworkedManagerBase<ZoneManager>.instance.ClientEvent_OnRoomLoaded -= new Action<EventInfoLoadRoom>(ClientEventOnRoomLoaded);
		}
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom _)
	{
		_remainingDamages.Clear();
		base.numberDisplay = 0;
	}

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnTakeHeal += new Action<EventInfoHeal>(EntityEventOnTakeHeal);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (oldOwner != null)
			{
				oldOwner.EntityEvent_OnTakeHeal -= new Action<EventInfoHeal>(EntityEventOnTakeHeal);
			}
			_remainingDamages.Clear();
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			_remainingDamages.Clear();
		}
	}

	private void EntityEventOnTakeHeal(EventInfoHeal obj)
	{
		if (!base.isValid)
		{
			return;
		}
		float num = (obj.amount + obj.discardedAmount) * GetValue(healToDamageRatio);
		if (_remainingDamages.ContainsKey(obj.chain))
		{
			_remainingDamages[obj.chain] += num;
		}
		else if (_remainingDamages.Count > 100)
		{
			ReactionChain key = default(ReactionChain);
			foreach (KeyValuePair<ReactionChain, float> remainingDamage in _remainingDamages)
			{
				key = remainingDamage.Key;
			}
			_remainingDamages[key] += num;
		}
		else
		{
			_remainingDamages[obj.chain] = num;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || !base.isValid)
		{
			return;
		}
		if (_remainingDamages.Count <= 0)
		{
			base.numberDisplay = 0;
			return;
		}
		float num = 0f;
		foreach (KeyValuePair<ReactionChain, float> remainingDamage in _remainingDamages)
		{
			num += remainingDamage.Value;
		}
		float num2 = shootMinDamage;
		float num3 = shootMaxDamageMaxHpRatio * base.owner.maxHealth;
		float num4 = Mathf.Max(num3, num / maxShootCount);
		_keysToRemove.Clear();
		_keysToDecrease.Clear();
		bool flag = false;
		ReactionChain chain = default(ReactionChain);
		float damageToShoot = 0f;
		foreach (KeyValuePair<ReactionChain, float> remainingDamage2 in _remainingDamages)
		{
			if (!flag)
			{
				flag = true;
				chain = remainingDamage2.Key;
			}
			if (remainingDamage2.Value > num4)
			{
				_keysToDecrease.Add(remainingDamage2.Key);
				damageToShoot = num4;
				break;
			}
			damageToShoot += remainingDamage2.Value;
			_keysToRemove.Add(remainingDamage2.Key);
			if (damageToShoot >= num4)
			{
				break;
			}
		}
		float num5 = (damageToShoot - num2) / (num3 - num2);
		float t = Mathf.Clamp01(num5);
		float num6 = normalizedShootInterval.Evaluate(num5);
		if (Time.time - _lastShootTime <= num6)
		{
			base.numberDisplay = Mathf.RoundToInt(num);
			return;
		}
		foreach (ReactionChain item in _keysToRemove)
		{
			_remainingDamages.Remove(item);
		}
		foreach (ReactionChain item2 in _keysToDecrease)
		{
			_remainingDamages[item2] -= num4;
		}
		base.numberDisplay = Mathf.RoundToInt(num - damageToShoot);
		_lastShootTime = Time.time;
		float procCoeff = Mathf.Lerp(procCoefficient.x, procCoefficient.y, t);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.position, shootRadius, tvDefaultHarmfulEffectTargets);
		if (readOnlySpan.Length > 0)
		{
			Entity target = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
			CreateAbilityInstance(base.owner.position, Quaternion.identity, new CastInfo(base.owner, target), delegate(Ai_Gem_L_GlacialCore_Projectile p)
			{
				p._damageAmount = damageToShoot;
				p.Network_procCoefficient = procCoeff;
				p.chain = chain.New(this);
			});
			handle.Return();
			NotifyUse();
		}
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (base.isValid)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			info.actor.LockDestroy();
			yield return new WaitForSeconds(delay);
			if (!base.isValid || !IsReady() || info.chain.DidReact(this) || base.owner.IsNullOrInactive() || !base.owner.CheckEnemyOrNeutral(info.victim))
			{
				info.actor.UnlockDestroy();
			}
			else
			{
				HealData heal = new HealData(GetValue(healMaxHealthRatio) * base.owner.maxHealth);
				if (info.damage.elemental == ElementalType.Cold)
				{
					FxPlayNewNetworked(healCritEffect, base.owner);
					heal.ApplyAmplification(chillAmp);
					heal.SetCrit();
				}
				else
				{
					FxPlayNewNetworked(healEffect, base.owner);
				}
				info.actor.DoHeal(heal, base.owner, info.chain.New(this));
				NotifyUse();
				StartCooldown();
				info.actor.UnlockDestroy();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
