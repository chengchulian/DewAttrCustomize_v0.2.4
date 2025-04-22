using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Hero_Bismuth_BookController : DewNetworkBehaviour
{
	private static readonly int MotionTime;

	public Transform bookTransform;

	public Animator bookAnimator;

	public AnimationCurve openAmountCurve;

	public AnimationCurve castRecoilCurve;

	[CompilerGenerated]
	[SyncVar]
	private Vector3? _003CtargetPos_003Ek__BackingField;

	private Vector3 _currentPosition;

	private Vector3 _lastShakeRandom;

	private float _lastShakeRandomTime;

	private float _lastCastTime;

	private Quaternion _lastCastRot;

	private Vector3 _cv;

	private Quaternion _rotCv;

	private Hero_Bismuth _hero;

	public Vector3? targetPos
	{
		[CompilerGenerated]
		get
		{
			return _003CtargetPos_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CtargetPos_003Ek__BackingField = value;
		}
	}

	public Vector3? targetPosClamped
	{
		get
		{
			if (!targetPos.HasValue)
			{
				return null;
			}
			return _hero.position + Vector3.ClampMagnitude(targetPos.Value - _hero.position, 2f);
		}
	}

	public Vector3? Network_003CtargetPos_003Ek__BackingField
	{
		get
		{
			return targetPos;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref targetPos, 1uL, null);
		}
	}

	private void Start()
	{
		_hero = GetComponent<Hero_Bismuth>();
		Dew.CallDelayed(delegate
		{
			bookTransform.parent = null;
		}, 2);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (base.isServer && !(_hero.Ability.attackAbility == null))
		{
			ListReturnHandle<Entity> handle;
			List<Entity> targetEntities = Hero_Bismuth.GetTargetEntities(out handle, _hero, canBeNeutral: false, 10f);
			if (targetEntities.Count > 0)
			{
				Network_003CtargetPos_003Ek__BackingField = targetEntities[0].agentPosition;
			}
			else
			{
				Network_003CtargetPos_003Ek__BackingField = null;
			}
			handle.Return();
		}
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();
		float num = 1.6f;
		if (Time.time - _lastShakeRandomTime > 0.05f)
		{
			_lastShakeRandom = Random.insideUnitSphere;
			_lastShakeRandomTime = Time.time;
		}
		Vector3 vector;
		float smoothTime;
		if (targetPosClamped.HasValue)
		{
			vector = targetPosClamped.Value;
			smoothTime = 0.125f;
		}
		else if (_hero.Control.isWalking)
		{
			vector = _hero.position + base.transform.forward * (0f - num);
			smoothTime = 0.2f;
		}
		else
		{
			vector = _hero.position + base.transform.forward * (0f - num);
			smoothTime = 0.2f;
		}
		vector += Vector3.up * (Mathf.Sin((Time.time - _hero.creationTime) * 2f) * 0.25f + 0.25f);
		Vector3 b = vector;
		Vector3 target = Vector3.Lerp(vector, b, 1f - (Time.time - _lastCastTime));
		_currentPosition = Vector3.SmoothDamp(_currentPosition, target, ref _cv, smoothTime);
		Vector3 vector2 = _lastCastRot * Vector3.back * castRecoilCurve.Evaluate(Time.time - _lastCastTime);
		vector2 += _lastShakeRandom * (castRecoilCurve.Evaluate(Time.time - _lastCastTime) * 0.55f);
		vector2 += _hero.rotation * _hero.Visual.etLocalOffset + _hero.Visual.etWorldOffset;
		bookTransform.position = _currentPosition + vector2;
		Quaternion a = Quaternion.Slerp(_lastCastRot, _lastCastRot.Flattened(), (Time.time - _lastCastTime) * 1.4f);
		Quaternion b2;
		if (targetPos.HasValue)
		{
			b2 = Quaternion.LookRotation(targetPos.Value - bookTransform.position).Flattened();
		}
		else
		{
			float num2 = Vector3.SignedAngle(bookTransform.position - _hero.position, bookTransform.position + _cv * 0.2f - _hero.position, Vector3.up);
			b2 = Quaternion.Euler(Mathf.Sin(Time.time * 0.784f) * 10f, (Mathf.Sin(Time.time) * 15f + Mathf.Sin(Time.time * 0.32f) * 12f) * (0.5f + _cv.magnitude * 0.5f) - num2, Mathf.Sin(Time.time * 0.921f) * 10f) * bookTransform.rotation.Flattened();
		}
		Quaternion target2 = Quaternion.Slerp(a, b2, (Time.time - _lastCastTime) * 2f);
		float time = Mathf.Lerp(0.05f, 0.2f, (Time.time - _lastCastTime) * 1.4f);
		bookTransform.rotation = QuaternionUtil.SmoothDamp(bookTransform.rotation, target2, ref _rotCv, time);
		bookAnimator.SetFloat(MotionTime, openAmountCurve.Evaluate(Time.time - _lastCastTime));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Object.Destroy(bookTransform.gameObject);
	}

	[ClientRpc]
	public void RpcBookCast(Quaternion rot)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteQuaternion(rot);
		SendRPCInternal("System.Void Hero_Bismuth_BookController::RpcBookCast(UnityEngine.Quaternion)", -150928420, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	public void RpcBookCast(Vector3 pos)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		SendRPCInternal("System.Void Hero_Bismuth_BookController::RpcBookCast(UnityEngine.Vector3)", -28427928, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	static Hero_Bismuth_BookController()
	{
		MotionTime = Animator.StringToHash("Motion Time");
		RemoteProcedureCalls.RegisterRpc(typeof(Hero_Bismuth_BookController), "System.Void Hero_Bismuth_BookController::RpcBookCast(UnityEngine.Quaternion)", InvokeUserCode_RpcBookCast__Quaternion);
		RemoteProcedureCalls.RegisterRpc(typeof(Hero_Bismuth_BookController), "System.Void Hero_Bismuth_BookController::RpcBookCast(UnityEngine.Vector3)", InvokeUserCode_RpcBookCast__Vector3);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcBookCast__Quaternion(Quaternion rot)
	{
		_lastCastRot = rot;
		_lastCastTime = Time.time;
	}

	protected static void InvokeUserCode_RpcBookCast__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcBookCast called on server.");
		}
		else
		{
			((Hero_Bismuth_BookController)obj).UserCode_RpcBookCast__Quaternion(reader.ReadQuaternion());
		}
	}

	protected void UserCode_RpcBookCast__Vector3(Vector3 pos)
	{
		_lastCastRot = Quaternion.LookRotation(pos - bookTransform.position).Flattened();
		_lastCastTime = Time.time;
	}

	protected static void InvokeUserCode_RpcBookCast__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcBookCast called on server.");
		}
		else
		{
			((Hero_Bismuth_BookController)obj).UserCode_RpcBookCast__Vector3(reader.ReadVector3());
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteVector3Nullable(targetPos);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteVector3Nullable(targetPos);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref targetPos, null, reader.ReadVector3Nullable());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref targetPos, null, reader.ReadVector3Nullable());
		}
	}
}
