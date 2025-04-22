using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Gem_E_Might : Gem
{
	public ScalingValue healthBonus;

	public float updateInterval;

	public float unitHealthAmount = 50f;

	public ScalingValue dmgAmpPerUnitHealth;

	[NonSerialized]
	[SyncVar]
	private float _currentDamageAmp;

	private float _lastUpdateTime;

	public float currentDamageAmp
	{
		get
		{
			if ((!NetworkServer.active && !NetworkClient.active) || !(base.netIdentity != null) || !(base.owner != null))
			{
				return GetDamageAmpFallback();
			}
			return _currentDamageAmp;
		}
	}

	public float Network_currentDamageAmp
	{
		get
		{
			return _currentDamageAmp;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _currentDamageAmp, 8192uL, null);
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(Amplify);
			statBonus.maxHealthFlat = GetValue(healthBonus);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.dealtDamageProcessor.Remove(Amplify);
		}
	}

	protected override void OnQualityChange(int oldQuality, int newQuality)
	{
		base.OnQualityChange(oldQuality, newQuality);
		if (base.isServer)
		{
			statBonus.maxHealthFlat = GetValue(healthBonus);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && base.isValid && !(Time.time - _lastUpdateTime < updateInterval))
		{
			_lastUpdateTime = Time.time;
			Network_currentDamageAmp = base.owner.maxHealth / unitHealthAmount * GetValue(dmgAmpPerUnitHealth);
			base.numberDisplay = Mathf.RoundToInt(_currentDamageAmp * 100f);
		}
	}

	private float GetDamageAmpFallback()
	{
		if (DewPlayer.local == null || DewPlayer.local.hero == null)
		{
			return 0f;
		}
		Hero hero = DewPlayer.local.hero;
		return hero.maxHealth / unitHealthAmount * dmgAmpPerUnitHealth.GetValue(base.effectiveLevel, hero);
	}

	private void Amplify(ref DamageData data, Actor actor, Entity target)
	{
		if (base.owner.CheckEnemyOrNeutral(target) && !data.IsAmountModifiedBy(this))
		{
			data.ApplyAmplification(currentDamageAmp);
			data.SetAmountModifiedBy(this);
			NotifyUse();
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
			writer.WriteFloat(_currentDamageAmp);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteFloat(_currentDamageAmp);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _currentDamageAmp, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _currentDamageAmp, null, reader.ReadFloat());
		}
	}
}
