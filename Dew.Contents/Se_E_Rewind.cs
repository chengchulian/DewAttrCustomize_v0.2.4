using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_E_Rewind : StatusEffect
{
	public float duration = 8f;

	public ScalingValue reduceAmount;

	public GameObject useEffect;

	[NonSerialized]
	public SkillTrigger targetSkill;

	private Hero _casterHero;

	[SyncVar]
	internal bool _didConsume;

	public bool Network_didConsume
	{
		get
		{
			return _didConsume;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _didConsume, 512uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		if (targetSkill is St_E_Rewind)
		{
			Destroy();
			return;
		}
		SetTimer(duration);
		ShowOnScreenTimer();
		_casterHero = (Hero)base.info.caster;
		_casterHero.HeroEvent_OnAbilityInstanceBeforePrepareFromSkill += new Action<EventInfoSkillAbilityInstance>(AttachHandlerToNewInstance);
		if (!(targetSkill == null))
		{
			targetSkill.ApplyCooldownReduction(GetValue(reduceAmount));
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _casterHero != null)
		{
			_casterHero.HeroEvent_OnAbilityInstanceBeforePrepareFromSkill -= new Action<EventInfoSkillAbilityInstance>(AttachHandlerToNewInstance);
		}
	}

	private void AttachHandlerToNewInstance(EventInfoSkillAbilityInstance info)
	{
		if ((info.type != 0 && info.type != HeroSkillLocation.W && info.type != HeroSkillLocation.E && info.type != HeroSkillLocation.R) || info.skill is St_E_Rewind)
		{
			return;
		}
		_casterHero.HeroEvent_OnAbilityInstanceBeforePrepareFromSkill -= new Action<EventInfoSkillAbilityInstance>(AttachHandlerToNewInstance);
		if (this == null || !base.isActive)
		{
			return;
		}
		List<Entity> damaged = new List<Entity>();
		info.instance.ActorEvent_OnDealDamage += (Action<EventInfoDamage>)delegate(EventInfoDamage damage)
		{
			if (!damage.chain.DidReact(this) && !damaged.Contains(damage.victim) && base.victim.CheckEnemyOrNeutral(damage.victim))
			{
				damaged.Add(damage.victim);
				CreateStatusEffect(damage.victim, delegate(Se_E_Rewind_Damage c)
				{
					c.chain = damage.chain.New(this);
				});
			}
		};
		FxPlayNetworked(useEffect, base.info.caster);
		DestroyOnDestroy(info.instance);
		StopTimer();
		HideOnScreenTimer();
		Network_didConsume = true;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_didConsume);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteBool(_didConsume);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _didConsume, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _didConsume, null, reader.ReadBool());
		}
	}
}
