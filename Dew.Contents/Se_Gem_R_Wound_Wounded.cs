using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_Gem_R_Wound_Wounded : StatusEffect
{
	public float duration;

	public DewCollider explodeRange;

	public ScalingValue hitDamage;

	public ScalingValue explodeDamage;

	public float hitProcCoefficient = 0.5f;

	public float explodeProcCoefficient = 1f;

	public int explodeStage;

	public GameObject[] stageEffect;

	public GameObject hitEffect;

	public GameObject explodeEffect;

	[SyncVar(hook = "OnStageChanged")]
	private int _stage;

	public int Network_stage
	{
		get
		{
			return _stage;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _stage, 512uL, OnStageChanged);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		OnStageChanged(-1, _stage);
		if (base.isServer)
		{
			SetTimer(duration);
			base.info.caster.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(OwnerAttacked);
		}
	}

	private void OwnerAttacked(EventInfoAttackEffect obj)
	{
		if (!base.isActive || _stage >= explodeStage || obj.victim != base.victim || base.info.caster.IsNullInactiveDeadOrKnockedOut())
		{
			return;
		}
		ResetTimer();
		Network_stage = _stage + 1;
		if (_stage >= explodeStage)
		{
			FxPlayNewNetworked(explodeEffect, base.victim);
			explodeRange.transform.position = base.victim.position;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = explodeRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				Damage(explodeDamage, explodeProcCoefficient).ApplyStrength(obj.strength).SetOriginPosition(base.info.caster.position).SetAttr(DamageAttribute.IsCrit)
					.Dispatch(entity, chain);
			}
			handle.Return();
			DestroyIfActive();
		}
		else
		{
			FxPlayNewNetworked(hitEffect, base.victim);
			Damage(hitDamage, hitProcCoefficient).SetOriginPosition(base.info.caster.position).Dispatch(base.victim, chain);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_stage < stageEffect.Length)
		{
			FxStop(stageEffect[_stage]);
		}
		if (base.isServer && base.info.caster != null)
		{
			base.info.caster.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(OwnerAttacked);
		}
	}

	private void OnStageChanged(int oldVal, int newVal)
	{
		if (oldVal >= 0)
		{
			FxStop(stageEffect[oldVal]);
		}
		if (base.isActive && newVal < stageEffect.Length)
		{
			FxPlay(stageEffect[newVal], base.victim);
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
			writer.WriteInt(_stage);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteInt(_stage);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _stage, OnStageChanged, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _stage, OnStageChanged, reader.ReadInt());
		}
	}
}
