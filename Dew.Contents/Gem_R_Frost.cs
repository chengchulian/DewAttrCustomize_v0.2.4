using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Gem_R_Frost : Gem
{
	private struct Ad_FrostStat
	{
		public StatBonus bonus;
	}

	private struct Ad_FrostImmunity
	{
		public Dictionary<Entity, float> applyTime;
	}

	public ScalingValue damageRatio;

	public Vector2 delay;

	public float stunDuration;

	public float perEnemyCooldown;

	public ScalingValue gainedMaxHp;

	public GameObject activateEffect;

	[NonSerialized]
	[SyncVar]
	public float gainedHp;

	private float _reducedCooldownTime;

	public float NetworkgainedHp
	{
		get
		{
			return gainedHp;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref gainedHp, 8192uL, null);
		}
	}

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

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			if (!newOwner.TryGetData<Ad_FrostStat>(out var data))
			{
				StatBonus bonus = new StatBonus();
				newOwner.Status.AddStatBonus(bonus);
				data.bonus = bonus;
				newOwner.AddData(data);
			}
			NetworkgainedHp = data.bonus.maxHealthFlat;
		}
	}

	protected override void OnDealDamage(EventInfoDamage obj)
	{
		base.OnDealDamage(obj);
		if (base.isValid && obj.damage.elemental == ElementalType.Cold)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			obj.actor.LockDestroy();
			yield return new WaitForSeconds(global::UnityEngine.Random.Range(delay.x, delay.y));
			if (!base.isValid || !base.owner.CheckEnemyOrNeutral(obj.victim) || obj.victim == null)
			{
				obj.actor.UnlockDestroy();
			}
			else
			{
				if (obj.victim.TryGetData<Ad_FrostImmunity>(out var data))
				{
					if (data.applyTime.TryGetValue(base.owner, out var value) && Time.time + _reducedCooldownTime - value < perEnemyCooldown)
					{
						obj.actor.UnlockDestroy();
						yield break;
					}
					data.applyTime[base.owner] = Time.time + _reducedCooldownTime;
				}
				else
				{
					obj.victim.AddData(new Ad_FrostImmunity
					{
						applyTime = new Dictionary<Entity, float> { 
						{
							base.owner,
							Time.time + _reducedCooldownTime
						} }
					});
				}
				FxPlayNewNetworked(activateEffect, obj.victim);
				CreateBasicEffect(obj.victim, new StunEffect(), stunDuration, "frost_stun");
				float maxHealth = base.owner.Status.maxHealth;
				obj.actor.DefaultDamage(maxHealth * GetValue(damageRatio)).SetElemental(ElementalType.Cold).Dispatch(obj.victim);
				if (!base.owner.TryGetData<Ad_FrostStat>(out var data2))
				{
					StatBonus bonus = new StatBonus();
					base.owner.Status.AddStatBonus(bonus);
					data2.bonus = bonus;
					base.owner.AddData(data2);
				}
				data2.bonus.maxHealthFlat += Mathf.RoundToInt(GetValue(gainedMaxHp));
				NetworkgainedHp = data2.bonus.maxHealthFlat;
				NotifyUse();
				obj.actor.UnlockDestroy();
			}
		}
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(gainedHp);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteFloat(gainedHp);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref gainedHp, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref gainedHp, null, reader.ReadFloat());
		}
	}
}
