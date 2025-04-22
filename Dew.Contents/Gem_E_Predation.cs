using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Gem_E_Predation : Gem
{
	public ScalingValue amp;

	public float killGraceTime;

	public int bossMultiplier = 3;

	[NonSerialized]
	[SyncVar]
	public float boostedAp;

	[NonSerialized]
	[SyncVar]
	public float boostedAd;

	private KillTracker _tracker;

	public float NetworkboostedAp
	{
		get
		{
			return boostedAp;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref boostedAp, 8192uL, null);
		}
	}

	public float NetworkboostedAd
	{
		get
		{
			return boostedAd;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref boostedAd, 16384uL, null);
		}
	}

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			_tracker = newOwner.TrackKills(killGraceTime, OnKill);
			UpdateStack();
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (_tracker != null)
			{
				_tracker.Stop();
				_tracker = null;
			}
			UpdateStack();
		}
	}

	private void OnKill(EventInfoKill obj)
	{
		if (!(obj.victim is Monster monster) || !base.isValid || base.owner.IsNullInactiveDeadOrKnockedOut())
		{
			return;
		}
		if (monster.IsAnyBoss())
		{
			for (int i = 0; i < bossMultiplier; i++)
			{
				CreateAbilityInstance(monster.position, null, new CastInfo(base.owner), delegate(Ai_Gem_E_Predation_Pickup p)
				{
					p._targetHero = base.owner;
				});
			}
			NotifyUse();
		}
		else if (monster.isHunter)
		{
			CreateAbilityInstance(monster.position, null, new CastInfo(base.owner), delegate(Ai_Gem_E_Predation_Pickup p)
			{
				p._targetHero = base.owner;
			});
			NotifyUse();
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(GiveAmpDmg);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.dealtDamageProcessor.Remove(GiveAmpDmg);
		}
	}

	private void GiveAmpDmg(ref DamageData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && target is Monster monster && (monster.type == Monster.MonsterType.Boss || monster.isHunter || monster.type == Monster.MonsterType.MiniBoss))
		{
			data.SetAttr(DamageAttribute.IsCrit);
			data.ApplyAmplification(GetValue(amp));
			data.SetAmountModifiedBy(this);
			NotifyUse();
		}
	}

	[Server]
	internal void UpdateStack()
	{
		Ai_Gem_E_Predation_Pickup.Ad_Predation data;
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Gem_E_Predation::UpdateStack()' called when server was not active");
		}
		else if (base.owner == null || !base.owner.TryGetData<Ai_Gem_E_Predation_Pickup.Ad_Predation>(out data))
		{
			NetworkboostedAd = 0f;
			NetworkboostedAp = 0f;
		}
		else
		{
			NetworkboostedAd = data.bonus.attackDamageFlat;
			NetworkboostedAp = data.bonus.abilityPowerFlat;
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
			writer.WriteFloat(boostedAp);
			writer.WriteFloat(boostedAd);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteFloat(boostedAp);
		}
		if ((base.syncVarDirtyBits & 0x4000L) != 0L)
		{
			writer.WriteFloat(boostedAd);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref boostedAp, null, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref boostedAd, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref boostedAp, null, reader.ReadFloat());
		}
		if ((num & 0x4000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref boostedAd, null, reader.ReadFloat());
		}
	}
}
