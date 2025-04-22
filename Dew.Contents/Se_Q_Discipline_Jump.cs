using System;
using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Se_Q_Discipline_Jump : StatusEffect
{
	public DewAnimationClip animPrepareNoJump;

	public DewAnimationClip animJump;

	public DewAnimationClip animStomp;

	public float gapDist = 2.5f;

	public float speed = 30f;

	public AnimationCurve jumpCurve;

	public float jumpDamageAmp = 2f;

	[NonSerialized]
	public bool enableBonusByDistance;

	private Vector3 _stompPos;

	private EntityTransformModifier _et;

	private float _strength = 1f;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		DoUnstoppable();
		DoUncollidable();
		_stompPos = base.info.target.agentPosition;
		base.info.caster.Ability.attackAbility.ResetCooldown();
		if (Vector3.Distance(base.info.caster.agentPosition, _stompPos) < 3.5f)
		{
			base.info.caster.Animation.PlayAbilityAnimation(animPrepareNoJump);
			base.info.caster.Control.StartDaze(0.125f);
			yield return new SI.WaitForSeconds(0.1f);
			Stomp();
			yield break;
		}
		float range = base.firstTrigger.configs[0].castMethod.targetData.range;
		Vector3 vector = base.info.target.agentPosition - base.info.caster.agentPosition;
		float t = Mathf.Clamp01((vector.magnitude - 3.5f) / (range - 3.5f) * 1.15f);
		if (enableBonusByDistance)
		{
			_strength = Mathf.Lerp(1f, 1f + jumpDamageAmp, t);
		}
		base.info.caster.Animation.PlayAbilityAnimation(animJump);
		Vector3 destination = base.info.caster.agentPosition + vector.normalized * (vector.magnitude - gapDist);
		float duration = Mathf.Clamp(vector.magnitude / (speed * base.info.caster.Status.movementSpeedMultiplier), 0.1f, 0.5f);
		base.info.caster.Control.StartDisplacement(new DispByDestination
		{
			destination = destination,
			duration = duration,
			ease = DewEase.EaseOutQuad,
			isFriendly = true,
			rotateForward = true,
			rotateSmoothly = false,
			canGoOverTerrain = true,
			affectedByMovementSpeed = false,
			isCanceledByCC = false,
			onCancel = base.DestroyIfActive,
			onFinish = Stomp
		});
		RpcStartTransform(duration);
	}

	[ClientRpc]
	private void RpcStartTransform(float duration)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(duration);
		SendRPCInternal("System.Void Se_Q_Discipline_Jump::RpcStartTransform(System.Single)", 781336865, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_et != null)
		{
			_et.Stop();
			_et = null;
		}
	}

	private void Stomp()
	{
		base.info.caster.Animation.PlayAbilityAnimation(animStomp);
		CreateAbilityInstance(_stompPos, Quaternion.LookRotation(_stompPos - base.info.caster.agentPosition), new CastInfo(base.info.caster), delegate(Ai_Q_Discipline_Stomp ai)
		{
			ai.strengthMultiplier = _strength;
		});
		base.info.caster.Control.RotateTowards(_stompPos, immediately: true, 1f);
		base.info.caster.Control.StartDaze(0.125f);
		base.info.caster.Control.Attack(base.info.target, doChase: true);
		DestroyIfActive();
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcStartTransform__Single(float duration)
	{
		if (base.isActive)
		{
			_et = base.info.caster.Visual.GetNewTransformModifier();
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			float startTime = Time.time;
			while (Time.time - startTime < duration && _et != null)
			{
				_et.localOffset = Vector3.up * (jumpCurve.Evaluate((Time.time - startTime) / duration) * Mathf.Max(duration, 0.3f) * 7f);
				yield return null;
			}
		}
	}

	protected static void InvokeUserCode_RpcStartTransform__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStartTransform called on server.");
		}
		else
		{
			((Se_Q_Discipline_Jump)obj).UserCode_RpcStartTransform__Single(reader.ReadFloat());
		}
	}

	static Se_Q_Discipline_Jump()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Se_Q_Discipline_Jump), "System.Void Se_Q_Discipline_Jump::RpcStartTransform(System.Single)", InvokeUserCode_RpcStartTransform__Single);
	}
}
