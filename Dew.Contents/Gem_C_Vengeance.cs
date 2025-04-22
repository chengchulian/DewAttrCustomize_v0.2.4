using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Gem_C_Vengeance : Gem
{
	public int shootCount = 2;

	public float shootInterval = 0.1f;

	public float empowerDuration = 5f;

	public ScalingValue ampAmount;

	public float targetRadius;

	public GameObject fxEmpowered;

	[SyncVar(hook = "OnDamageAmplifiedChanged")]
	private bool _isAmpActive;

	private float _lastHitTime;

	public bool Network_isAmpActive
	{
		get
		{
			return _isAmpActive;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _isAmpActive, 8192uL, OnDamageAmplifiedChanged);
		}
	}

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			base.owner.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(EntityEventOnTakeDamage);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (oldOwner != null)
			{
				oldOwner.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(EntityEventOnTakeDamage);
			}
			Network_isAmpActive = false;
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(AmplifyDamage);
			newSkill.dealtHealProcessor.Add(AmplifyHeal);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (oldSkill != null)
			{
				oldSkill.dealtDamageProcessor.Remove(AmplifyDamage);
				oldSkill.dealtHealProcessor.Remove(AmplifyHeal);
			}
			Network_isAmpActive = false;
		}
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
		StartCooldown();
		Network_isAmpActive = true;
		_lastHitTime = Time.time;
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			for (int i = 0; i < shootCount; i++)
			{
				if (!base.isValid)
				{
					break;
				}
				if (!LaunchProjectile())
				{
					break;
				}
				NotifyUse();
				yield return new WaitForSeconds(shootInterval);
			}
		}
	}

	private bool LaunchProjectile()
	{
		if (!base.isValid)
		{
			return false;
		}
		if (base.isNotReadyByRateLimit)
		{
			return false;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.owner.position, targetRadius, tvDefaultHarmfulEffectTargets);
		if (readOnlySpan.Length > 0)
		{
			CreateAbilityInstance<Ai_Gem_C_Vengence_Projectile>(base.owner.position, null, new CastInfo(base.owner, readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)]));
			handle.Return();
			return true;
		}
		handle.Return();
		return false;
	}

	private void AmplifyHeal(ref HealData data, Actor actor, Entity target)
	{
		if (base.isValid && _isAmpActive && !data.IsAmountModifiedBy(this) && base.owner.CheckEnemyOrNeutral(target))
		{
			data.SetCrit();
			data.SetAmountModifiedBy(this);
			data.ApplyAmplification(GetValue(ampAmount));
		}
	}

	private void AmplifyDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (base.isValid && _isAmpActive && !data.IsAmountModifiedBy(this) && base.owner.CheckEnemyOrNeutral(target))
		{
			data.SetAttr(DamageAttribute.IsCrit);
			data.SetAmountModifiedBy(this);
			data.ApplyAmplification(GetValue(ampAmount));
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && _isAmpActive && Time.time - _lastHitTime > empowerDuration)
		{
			Network_isAmpActive = false;
		}
	}

	private void OnDamageAmplifiedChanged(bool oldVal, bool newVal)
	{
		if (newVal)
		{
			FxPlay(fxEmpowered, base.owner);
		}
		else
		{
			FxStop(fxEmpowered);
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
			writer.WriteBool(_isAmpActive);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteBool(_isAmpActive);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _isAmpActive, OnDamageAmplifiedChanged, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _isAmpActive, OnDamageAmplifiedChanged, reader.ReadBool());
		}
	}
}
