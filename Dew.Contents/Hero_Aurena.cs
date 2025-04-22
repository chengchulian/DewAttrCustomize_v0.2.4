using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Hero_Aurena : Hero
{
	private enum ClawState
	{
		Hidden,
		Shown,
		ShownBig
	}

	private enum ClawAnimateState
	{
		Hidden,
		Starting,
		Looping,
		Ending
	}

	public Transform medalTransform;

	public Transform clawL;

	public Transform clawR;

	public Transform handL;

	public Transform handR;

	public Transform[] animatedJoints;

	private Quaternion[] _animatedJointsRots;

	public GameObject fxClawLStarting;

	public GameObject fxClawRStarting;

	public GameObject fxClawLLoop;

	public GameObject fxClawRLoop;

	public GameObject fxClawLEnding;

	public GameObject fxClawREnding;

	[SyncVar]
	private ClawState _clawState;

	private ClawAnimateState _clawAnimateState;

	private float _clawOutTime = -1000f;

	private float _angle = -37f;

	private float _angularVelocity;

	public ClawState Network_clawState
	{
		get
		{
			return _clawState;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _clawState, 256uL, null);
		}
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		_animatedJointsRots = new Quaternion[animatedJoints.Length];
		for (int i = 0; i < animatedJoints.Length; i++)
		{
			_animatedJointsRots[i] = animatedJoints[i].localRotation;
		}
		base.Sound.ClientEvent_OnFootstep += (Action)delegate
		{
			_angularVelocity = global::UnityEngine.Random.Range(100f, 400f);
		};
		clawL.localScale = Vector3.zero;
		clawR.localScale = Vector3.zero;
		if (!base.isServer)
		{
			return;
		}
		AbilityTrigger attackAbility = base.Ability.attackAbility;
		attackAbility.TriggerEvent_OnCastStart = (Action<EventInfoCast>)Delegate.Combine(attackAbility.TriggerEvent_OnCastStart, (Action<EventInfoCast>)delegate(EventInfoCast cast)
		{
			Network_clawState = ((cast.configIndex == 0) ? ClawState.Shown : ClawState.ShownBig);
			_clawOutTime = Time.time;
		});
		ClientHeroEvent_OnSkillUse += (Action<EventInfoSkillUse>)delegate(EventInfoSkillUse info)
		{
			if (info.skill is St_R_DangerousTheory)
			{
				Network_clawState = ClawState.ShownBig;
				_clawOutTime = Time.time + 4f;
			}
			else if (info.type != HeroSkillLocation.Movement && !info.skill.currentConfig.postponeBasicCommand)
			{
				Network_clawState = ClawState.Hidden;
			}
		};
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		if (base.isServer && _clawState > ClawState.Hidden && Time.time - _clawOutTime > 1f)
		{
			Network_clawState = ClawState.Hidden;
		}
		if (medalTransform != null)
		{
			_angle += _angularVelocity * Time.deltaTime;
			if (_angle < -37f)
			{
				_angle = -37f;
				_angularVelocity *= -0.4f;
			}
			else if (_angle > 50f)
			{
				_angle = 50f;
				_angularVelocity = 0f;
			}
			else
			{
				_angularVelocity += -1400f * Time.deltaTime;
			}
			medalTransform.localRotation = Quaternion.Euler(-0.143f, -90.423f, _angle);
		}
		clawL.position = handL.position;
		clawL.rotation = handL.rotation * Quaternion.Euler(0f, -40f, 0f);
		clawR.position = handR.position;
		clawR.rotation = handR.rotation * Quaternion.Euler(0f, 40f, 0f);
		float num = _clawState switch
		{
			ClawState.Hidden => 0f, 
			ClawState.Shown => 0.9f, 
			ClawState.ShownBig => 1.1f, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		clawL.localScale = Mathf.MoveTowards(clawL.localScale.x, num, (float)((_clawState != 0) ? 20 : 3) * Time.deltaTime) * Vector3.one;
		clawR.localScale = Mathf.MoveTowards(clawR.localScale.x, num, (float)((_clawState != 0) ? 20 : 3) * Time.deltaTime) * Vector3.one;
		if (_clawState != 0)
		{
			for (int i = 0; i < animatedJoints.Length; i++)
			{
				animatedJoints[i].localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 3f + (float)i * 0.5f) * 25f) * _animatedJointsRots[i];
			}
		}
		ClawAnimateState clawAnimateState = ((_clawState == ClawState.Hidden) ? ((clawL.localScale.x > 0.01f) ? ClawAnimateState.Ending : ClawAnimateState.Hidden) : ((clawL.localScale.x + 0.01f < num) ? ClawAnimateState.Starting : ((!(clawL.localScale.x - 0.01f > num)) ? ClawAnimateState.Looping : ClawAnimateState.Ending)));
		if (clawAnimateState != _clawAnimateState)
		{
			switch (_clawAnimateState)
			{
			case ClawAnimateState.Starting:
				FxStop(fxClawLStarting);
				FxStop(fxClawRStarting);
				break;
			case ClawAnimateState.Looping:
				FxStop(fxClawLLoop);
				FxStop(fxClawRLoop);
				break;
			case ClawAnimateState.Ending:
				FxStop(fxClawLEnding);
				FxStop(fxClawREnding);
				break;
			}
			switch (clawAnimateState)
			{
			case ClawAnimateState.Starting:
				FxPlay(fxClawLStarting);
				FxPlay(fxClawRStarting);
				break;
			case ClawAnimateState.Looping:
				FxPlay(fxClawLLoop);
				FxPlay(fxClawRLoop);
				break;
			case ClawAnimateState.Ending:
				FxPlay(fxClawLEnding);
				FxPlay(fxClawREnding);
				break;
			}
			_clawAnimateState = clawAnimateState;
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
			GeneratedNetworkCode._Write_Hero_Aurena_002FClawState(writer, _clawState);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			GeneratedNetworkCode._Write_Hero_Aurena_002FClawState(writer, _clawState);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _clawState, null, GeneratedNetworkCode._Read_Hero_Aurena_002FClawState(reader));
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _clawState, null, GeneratedNetworkCode._Read_Hero_Aurena_002FClawState(reader));
		}
	}
}
