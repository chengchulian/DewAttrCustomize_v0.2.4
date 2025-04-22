using System;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Mon_Sky_BossNyx : BossMonster
{
	public float forceTeleportDistance;

	public float doubleTeleportChance;

	public float dashMinDistance = 11f;

	public GameObject[] phaseObjects;

	[SyncVar(hook = "OnCurrentPhaseChanged")]
	internal int _currentPhase;

	private int _currentPhaseBeforeChange;

	private float _starfallChance = 0.5f;

	public int Network_currentPhase
	{
		get
		{
			return _currentPhase;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _currentPhase, 256uL, OnCurrentPhaseChanged);
		}
	}

	private void ActorEventOnAbilityInstanceBeforePrepareSwipe(EventInfoAbilityInstance obj)
	{
		if (obj.instance is Ai_Mon_Sky_BossNyx_Swipe ai_Mon_Sky_BossNyx_Swipe)
		{
			ai_Mon_Sky_BossNyx_Swipe.NetworkenableImprovedSwipe = true;
		}
	}

	private void OnCurrentPhaseChanged(int oldVal, int newVal)
	{
		if (!base.isActive)
		{
			return;
		}
		for (int i = 0; i < phaseObjects.Length; i++)
		{
			if (!(phaseObjects[i] == null))
			{
				phaseObjects[i].SetActive(newVal > i);
			}
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Swipe>(out var trigger))
		{
			trigger.configs[0].effectOnCast = ((newVal >= 1) ? trigger.improvedCastEffect : trigger.configs[0].effectOnCast);
		}
		if (!base.isServer)
		{
			return;
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Starfall>(out var trigger2))
		{
			trigger2.configs[0].maxCharges = ((newVal >= 1) ? 1 : 0);
			trigger2.ResetCooldown();
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_CreateSeeds>(out var trigger3))
		{
			trigger3.configs[0].maxCharges = ((newVal >= 1) ? 1 : 0);
			trigger3.ResetCooldown();
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Teleport>(out var trigger4))
		{
			float cooldownTime = trigger4.configs[0].cooldownTime;
			trigger4.configs[0].cooldownTime = ((newVal < 1) ? cooldownTime : (cooldownTime -= cooldownTime / 3f));
			trigger4.configs[0].maxCharges = ((newVal < 1) ? 1 : 2);
			trigger4.ResetCooldown();
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Swipe>(out var trigger5))
		{
			if (newVal >= 1)
			{
				ActorEvent_OnAbilityInstanceBeforePrepare += new Action<EventInfoAbilityInstance>(ActorEventOnAbilityInstanceBeforePrepareSwipe);
			}
			trigger5.configs[0].maxCharges = ((newVal < 1) ? 1 : 2);
			trigger5.configs[0].addedCharges = ((newVal < 2) ? 1 : 2);
			trigger5.ResetCooldown();
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_StellarDash>(out var trigger6))
		{
			trigger6.configs[0].maxCharges = ((newVal < 2) ? 1 : 3);
			trigger6.configs[0].addedCharges = ((newVal < 2) ? 1 : 3);
			trigger6.ResetCooldown();
		}
		if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Blackhole>(out var trigger7))
		{
			trigger7.configs[0].maxCharges = ((newVal >= 2) ? 1 : 0);
			trigger7.ResetCooldown();
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		OnCurrentPhaseChanged(0, 0);
		if (base.isServer)
		{
			CreateBasicEffect(this, new UnstoppableEffect(), float.PositiveInfinity);
			EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(EntityEventOnTakeDamage);
		}
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
		if (_currentPhaseBeforeChange < 2 && base.normalizedHealth < 1f / 3f)
		{
			_starfallChance = 1f;
			_currentPhaseBeforeChange = 2;
			CreateStatusEffect<Se_Mon_Sky_BossNyx_PhaseChange>(this, new CastInfo(this));
		}
		else if (_currentPhaseBeforeChange < 1 && base.normalizedHealth < 2f / 3f)
		{
			_starfallChance = 0.7f;
			_currentPhaseBeforeChange = 1;
			CreateStatusEffect<Se_Mon_Sky_BossNyx_PhaseChange>(this, new CastInfo(this));
		}
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		if (context.targetEnemy == null)
		{
			return;
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_Blackhole>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Blackhole>();
			return;
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_CreateSeeds>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_CreateSeeds>();
			return;
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_Starfall>() && global::UnityEngine.Random.value <= _starfallChance)
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Starfall>();
			return;
		}
		if (Vector3.Distance(context.targetEnemy.agentPosition, base.agentPosition) >= forceTeleportDistance)
		{
			if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Teleport>(out var trigger))
			{
				trigger.ResetCooldown();
			}
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Teleport>();
			return;
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_Teleport>())
		{
			if (!(global::UnityEngine.Random.value <= doubleTeleportChance * NetworkedManagerBase<GameManager>.instance.GetSpecialSkillChanceMultiplier()) || _currentPhase < 1)
			{
				base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Teleport>();
				if (!(global::UnityEngine.Random.value > 0.7f) && base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Swipe>(out var trigger2))
				{
					trigger2.ResetCooldown();
				}
				return;
			}
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Teleport>();
			if (base.Ability.TryGetAbility<At_Mon_Sky_BossNyx_Teleport>(out var trigger3))
			{
				trigger3.ResetCooldown();
			}
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Teleport>();
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_StellarDash>() && base.AI.Helper_IsTargetInRange<At_Mon_Sky_BossNyx_StellarDash>() && base.AI.Helper_TryGetCastInfoAuto<At_Mon_Sky_BossNyx_StellarDash>(out var info))
		{
			if (Vector2.Distance(base.agentPosition.ToXY(), info.point.ToXY()) > dashMinDistance)
			{
				base.AI.Helper_CastAbility<At_Mon_Sky_BossNyx_StellarDash>(info);
				return;
			}
			Vector3 validAgentDestination_Closest = Dew.GetValidAgentDestination_Closest(base.agentPosition, base.agentPosition + dashMinDistance * (info.point - base.agentPosition).normalized);
			if (Vector2.Distance(base.agentPosition.ToXY(), validAgentDestination_Closest.ToXY()) > dashMinDistance - 2f)
			{
				info.point = validAgentDestination_Closest;
				base.AI.Helper_CastAbility<At_Mon_Sky_BossNyx_StellarDash>(info);
				return;
			}
		}
		if (base.AI.Helper_CanBeCast<At_Mon_Sky_BossNyx_Swipe>())
		{
			base.AI.Helper_CastAbilityAuto<At_Mon_Sky_BossNyx_Swipe>();
		}
		else
		{
			base.AI.Helper_ChaseTarget();
		}
	}

	protected override void OnBossSoulBeforeSpawn(Shrine_BossSoul soul)
	{
		base.OnBossSoulBeforeSpawn(soul);
		
		if (!AttrCustomizeResources.Config.removeSkills.Contains("St_L_HerWorld"))
		{
			soul.SetSkillReward<St_L_HerWorld>(0.1f);
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
			writer.WriteInt(_currentPhase);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteInt(_currentPhase);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _currentPhase, OnCurrentPhaseChanged, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _currentPhase, OnCurrentPhaseChanged, reader.ReadInt());
		}
	}
}
