using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class At_Atk_YubarStardust : AttackTrigger
{
	public DewBeamRenderer beamRenderer;

	[SyncVar(hook = "OnTargetChanged")]
	private Entity _target;

	private Vector3 _targetPos;

	protected NetworkBehaviourSyncVar ____targetNetId;

	public Entity Network_target
	{
		get
		{
			return GetSyncVarNetworkBehaviour(____targetNetId, ref _target);
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter_NetworkBehaviour(value, ref _target, 256uL, OnTargetChanged, ref ____targetNetId);
		}
	}

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			Hero hero = newOwner as Hero;
			if (!(hero == null))
			{
				hero.ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			}
		}
	}

	[Server]
	public void ClearTarget()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void At_Atk_YubarStardust::ClearTarget()' called when server was not active");
		}
		else
		{
			Network_target = null;
		}
	}

	private void HeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		Network_target = null;
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer)
		{
			Hero hero = base.owner as Hero;
			if (!(hero == null))
			{
				hero.ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			}
		}
	}

	public override AbilityInstance OnCastComplete(int configIndex, CastInfo info)
	{
		AbilityInstance result = base.OnCastComplete(configIndex, info);
		if (info.target != null)
		{
			Network_target = info.target;
		}
		return result;
	}

	private void OnTargetChanged(Entity oldv, Entity newv)
	{
		if (newv == null || !newv.isActive)
		{
			beamRenderer.enabled = false;
			return;
		}
		beamRenderer.SetPoints(base.owner.Visual.GetBonePosition(HumanBodyBones.RightHand), Network_target.Visual.GetCenterPosition());
		beamRenderer.enabled = true;
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (!(Network_target == null))
		{
			beamRenderer.SetPoints(base.owner.Visual.GetBonePosition(HumanBodyBones.RightHand), Network_target.Visual.GetCenterPosition());
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (beamRenderer != null)
		{
			beamRenderer.enabled = false;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Network_target != null)
		{
			AbilityTrigger attackAbility = base.owner.Ability.attackAbility;
			if (base.owner.Control.attackTarget != Network_target || !attackAbility.IsTargetInRange(Network_target) || !attackAbility.currentConfig.selfValidator.Evaluate(base.owner) || !attackAbility.currentConfig.targetValidator.Evaluate(base.owner, Network_target))
			{
				Network_target = null;
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
			writer.WriteNetworkBehaviour(Network_target);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteNetworkBehaviour(Network_target);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref _target, OnTargetChanged, reader, ref ____targetNetId);
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref _target, OnTargetChanged, reader, ref ____targetNetId);
		}
	}
}
