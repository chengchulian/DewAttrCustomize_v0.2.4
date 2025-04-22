using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Se_D_ConvergencePoint : StatusEffect
{
	public ScalingValue damageBonus;

	public DewCollider otherTargetRange;

	public int chainedTargets;

	public bool allowMovementSkill;

	public float empowerDuration;

	public DewBeamRenderer beamPrefab;

	public GameObject hitEffect;

	public float chainedStrength = 1f;

	private StatBonus _bonus;

	[SyncVar(hook = "OnDurationChanged")]
	private float _empowerRemainingDuration;

	[SyncVar]
	private bool _shouldBeam;

	private DewBeamRenderer[] _beamRenderers;

	private readonly SyncList<Entity> _targets = new SyncList<Entity>();

	private OnScreenTimerHandle _handle;

	private AbilityTrigger _trigger;

	public Hero heroVictim => base.victim as Hero;

	public bool isEmpowered => _empowerRemainingDuration > 0f;

	public float Network_empowerRemainingDuration
	{
		get
		{
			return _empowerRemainingDuration;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _empowerRemainingDuration, 512uL, OnDurationChanged);
		}
	}

	public bool Network_shouldBeam
	{
		get
		{
			return _shouldBeam;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _shouldBeam, 1024uL, null);
		}
	}

	protected override void OnPrepare()
	{
		base.OnPrepare();
		for (int i = 0; i < chainedTargets + 1; i++)
		{
			_targets.Add(null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		_trigger = base.firstTrigger;
		_beamRenderers = new DewBeamRenderer[chainedTargets];
		for (int i = 0; i < chainedTargets; i++)
		{
			_beamRenderers[i] = global::UnityEngine.Object.Instantiate(beamPrefab, base.transform);
		}
		if (base.isServer)
		{
			_bonus = new StatBonus();
			heroVictim.Status.AddStatBonus(_bonus);
			heroVictim.EntityEvent_OnAttackFiredBeforePrepare += new Action<EventInfoAttackFired>(EntityEventOnAttackFiredBeforePrepare);
			heroVictim.ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (_targets.Count == 0)
		{
			return;
		}
		for (int i = 1; i < _targets.Count; i++)
		{
			if (_targets[i].IsNullOrInactive() || _targets[i - 1].IsNullOrInactive())
			{
				_beamRenderers[i - 1].enabled = false;
				continue;
			}
			_beamRenderers[i - 1].SetPoints(_targets[i - 1].Visual.GetCenterPosition(), _targets[i].Visual.GetCenterPosition());
			_beamRenderers[i - 1].enabled = true;
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_handle != null)
		{
			HideOnScreenTimerLocally(_handle);
			_handle = null;
		}
		if (_beamRenderers != null)
		{
			for (int i = 0; i < _beamRenderers.Length; i++)
			{
				_beamRenderers[i].enabled = false;
			}
		}
		if (base.isServer && !(heroVictim == null))
		{
			base.victim.EntityEvent_OnAttackFiredBeforePrepare -= new Action<EventInfoAttackFired>(EntityEventOnAttackFiredBeforePrepare);
			heroVictim.ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(HeroEventOnSkillUse);
			if (base.firstTrigger != null)
			{
				base.firstTrigger.fillAmount = 0f;
			}
			if (_bonus != null)
			{
				heroVictim.Status.RemoveStatBonus(_bonus);
			}
		}
	}

	private void HeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		for (int i = 0; i < _beamRenderers.Length; i++)
		{
			_beamRenderers[i].enabled = false;
		}
		for (int j = 0; j < _targets.Count; j++)
		{
			_targets[j] = null;
		}
		if (obj.type == HeroSkillLocation.Q || obj.type == HeroSkillLocation.W || obj.type == HeroSkillLocation.E || obj.type == HeroSkillLocation.R || (allowMovementSkill && obj.type == HeroSkillLocation.Movement))
		{
			Network_empowerRemainingDuration = empowerDuration;
			base.victim.Ability.attackAbility.ResetCooldown();
		}
	}

	private void OnDurationChanged(float oldVal, float newVal)
	{
		if (base.victim == null || !base.isActive)
		{
			return;
		}
		if (base.victim.isOwned)
		{
			if (newVal <= 0f && _handle != null)
			{
				HideOnScreenTimerLocally(_handle);
				_handle = null;
			}
			else if (newVal > 0f && _handle == null)
			{
				_handle = ShowOnScreenTimerLocally(new OnScreenTimerHandle
				{
					fillAmountGetter = () => _empowerRemainingDuration / empowerDuration
				});
			}
		}
		if (base.isServer)
		{
			if (newVal <= 0f && _bonus.attackDamageFlat > 0f)
			{
				_bonus.attackDamageFlat = 0f;
			}
			else if (newVal > 0f && _bonus.attackDamageFlat <= 0f)
			{
				_bonus.attackDamageFlat = GetValue(damageBonus);
			}
			if (_trigger != null)
			{
				_trigger.fillAmount = _empowerRemainingDuration / empowerDuration;
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer)
		{
			Network_empowerRemainingDuration = Mathf.MoveTowards(_empowerRemainingDuration, 0f, dt);
			Network_shouldBeam = isEmpowered && base.victim.Control.attackTarget != null && base.victim.Ability.attackAbility.IsTargetInRange(base.victim.Control.attackTarget);
		}
		if (_shouldBeam)
		{
			return;
		}
		if (base.isServer)
		{
			for (int i = 0; i < _targets.Count; i++)
			{
				_targets[i] = null;
			}
		}
		DewBeamRenderer[] beamRenderers = _beamRenderers;
		for (int j = 0; j < beamRenderers.Length; j++)
		{
			beamRenderers[j].enabled = false;
		}
	}

	private void EntityEventOnAttackFiredBeforePrepare(EventInfoAttackFired obj)
	{
		if (!isEmpowered)
		{
			return;
		}
		int num = 0;
		_targets[0] = obj.info.target;
		FxPlayNewNetworked(hitEffect, obj.info.target);
		for (int i = 0; i < chainedTargets; i++)
		{
			otherTargetRange.transform.position = _targets[i].agentPosition;
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = otherTargetRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
			{
				sortComparer = CollisionCheckSettings.DistanceFromCenter
			});
			Entity entity = null;
			ReadOnlySpan<Entity> readOnlySpan = entities;
			for (int j = 0; j < readOnlySpan.Length; j++)
			{
				Entity entity2 = readOnlySpan[j];
				bool flag = false;
				for (int k = 0; k <= i; k++)
				{
					if (_targets[k] == entity2)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					entity = entity2;
					break;
				}
			}
			handle.Return();
			if (entity == null)
			{
				break;
			}
			_targets[i + 1] = entity;
			FxPlayNewNetworked(hitEffect, entity);
			Ai_Atk_YubarStardust ai_Atk_YubarStardust = (Ai_Atk_YubarStardust)obj.instance;
			ai_Atk_YubarStardust.chainStrength = chainedStrength;
			if (i + 1 == 1)
			{
				ai_Atk_YubarStardust.chainTarget0 = entity;
			}
			else if (i + 1 == 2)
			{
				ai_Atk_YubarStardust.chainTarget1 = entity;
			}
			else if (i + 1 == 3)
			{
				ai_Atk_YubarStardust.chainTarget2 = entity;
			}
			num++;
		}
		for (int l = num + 1; l < chainedTargets + 1; l++)
		{
			_targets[l] = null;
		}
	}

	public Se_D_ConvergencePoint()
	{
		InitSyncObject(_targets);
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(_empowerRemainingDuration);
			writer.WriteBool(_shouldBeam);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteFloat(_empowerRemainingDuration);
		}
		if ((base.syncVarDirtyBits & 0x400L) != 0L)
		{
			writer.WriteBool(_shouldBeam);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _empowerRemainingDuration, OnDurationChanged, reader.ReadFloat());
			GeneratedSyncVarDeserialize(ref _shouldBeam, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _empowerRemainingDuration, OnDurationChanged, reader.ReadFloat());
		}
		if ((num & 0x400L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _shouldBeam, null, reader.ReadBool());
		}
	}
}
