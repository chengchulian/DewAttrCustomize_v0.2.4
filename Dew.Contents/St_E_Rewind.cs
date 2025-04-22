using System;
using System.Runtime.InteropServices;
using Mirror;

public class St_E_Rewind : SkillTrigger
{
	[SyncVar]
	private HeroSkillLocation _targetType;

	[SyncVar]
	private bool _isSet;

	public HeroSkillLocation Network_targetType
	{
		get
		{
			return _targetType;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _targetType, 8388608uL, null);
		}
	}

	public bool Network_isSet
	{
		get
		{
			return _isSet;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _isSet, 16777216uL, null);
		}
	}

	public override void OnCastCompleteBeforePrepare(EventInfoCast cast)
	{
		base.OnCastCompleteBeforePrepare(cast);
		if (cast.instance is Se_E_Rewind se_E_Rewind)
		{
			base.owner.Skill.TryGetSkill(_targetType, out se_E_Rewind.targetSkill);
		}
	}

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			((Hero)newOwner).ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			Network_isSet = false;
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer)
		{
			((Hero)formerOwner).ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
		}
	}

	private void HeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		if ((obj.type == HeroSkillLocation.Q || obj.type == HeroSkillLocation.W || obj.type == HeroSkillLocation.E || obj.type == HeroSkillLocation.R) && !(obj.skill == this))
		{
			Network_isSet = true;
			Network_targetType = obj.type;
		}
	}

	public override bool CanBeCast()
	{
		if (!base.CanBeCast())
		{
			return false;
		}
		if (_isSet && base.owner.Skill.TryGetSkill(_targetType, out var _))
		{
			if (base.owner.Status.TryGetStatusEffect<Se_E_Rewind>(out var effect))
			{
				return effect._didConsume;
			}
			return true;
		}
		return false;
	}

	public override bool CanBeReserved()
	{
		if (!base.CanBeReserved())
		{
			return false;
		}
		if (_isSet && base.owner.Skill.TryGetSkill(_targetType, out var _))
		{
			if (base.owner.Status.TryGetStatusEffect<Se_E_Rewind>(out var effect))
			{
				return effect._didConsume;
			}
			return true;
		}
		return false;
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			GeneratedNetworkCode._Write_HeroSkillLocation(writer, _targetType);
			writer.WriteBool(_isSet);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x800000L) != 0L)
		{
			GeneratedNetworkCode._Write_HeroSkillLocation(writer, _targetType);
		}
		if ((base.syncVarDirtyBits & 0x1000000L) != 0L)
		{
			writer.WriteBool(_isSet);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _targetType, null, GeneratedNetworkCode._Read_HeroSkillLocation(reader));
			GeneratedSyncVarDeserialize(ref _isSet, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x800000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _targetType, null, GeneratedNetworkCode._Read_HeroSkillLocation(reader));
		}
		if ((num & 0x1000000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _isSet, null, reader.ReadBool());
		}
	}
}
