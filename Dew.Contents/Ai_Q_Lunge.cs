using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_Q_Lunge : AbilityInstance
{
	public DewCollider targetRange;

	public DewCollider stabRange;

	public GameObject stabEffect;

	public GameObject stabHitEffect;

	public bool resetAttackCooldown;

	public bool doCalibration;

	public float maxDeviationAngle = 20f;

	public int calibrationSteps = 10;

	public bool lookAtTarget;

	public float overrideRotDuration = 1f;

	public DewAnimationClip endAnimation;

	public ScalingValue dmgFactor;

	public float speed = 10f;

	private StatusEffect _unstoppable;

	protected override void OnCreate()
	{
		base.OnCreate();
		Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, base.info.point);
		base.transform.position = base.info.caster.Visual.GetCenterPosition();
		base.transform.rotation = Quaternion.LookRotation(validAgentDestination_LinearSweep - base.info.caster.transform.position);
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
			base.info.caster.Control.StartDisplacement(new DispByDestination
			{
				canGoOverTerrain = true,
				destination = validAgentDestination_LinearSweep,
				duration = Vector3.Distance(base.info.caster.transform.position, validAgentDestination_LinearSweep) / speed,
				ease = DewEase.Linear,
				isCanceledByCC = true,
				isFriendly = true,
				rotateForward = true,
				onFinish = FinishDash,
				onCancel = base.Destroy
			});
			if (resetAttackCooldown)
			{
				base.info.caster.Ability.attackAbility.ResetCooldown();
			}
			_unstoppable = CreateBasicEffect(base.info.caster, new UnstoppableEffect(), 2f, "lunge_unstoppable");
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _unstoppable != null && _unstoppable.isActive)
		{
			_unstoppable.Destroy();
		}
	}

	protected override void ActiveFrameUpdate()
	{
		base.ActiveFrameUpdate();
		base.transform.position = base.info.caster.Visual.GetCenterPosition();
	}

	private void FinishDash()
	{
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = targetRange.GetEntities(out handle, tvDefaultHarmfulEffectTargets, new CollisionCheckSettings
		{
			sortComparer = CollisionCheckSettings.DistanceFromCenter
		});
		if (entities.Length == 0)
		{
			handle.Return();
			base.info.caster.Animation.StopAbilityAnimation();
			Destroy();
			return;
		}
		Entity entity = entities[0];
		Quaternion rot = Quaternion.LookRotation(entity.position - base.transform.position).Flattened();
		if (doCalibration)
		{
			float num = float.NegativeInfinity;
			float y = rot.eulerAngles.y;
			for (int i = 0; i < calibrationSteps; i++)
			{
				float num2 = Mathf.Lerp(0f - maxDeviationAngle, maxDeviationAngle, (float)i / (float)(calibrationSteps - 1));
				base.transform.rotation = Quaternion.Euler(0f, y + num2, 0f);
				ArrayReturnHandle<Entity> handle2;
				ReadOnlySpan<Entity> entities2 = stabRange.GetEntities(out handle2, tvDefaultHarmfulEffectTargets);
				float num3 = (float)entities2.Length - Mathf.Abs(num2 / maxDeviationAngle) * 0.5f;
				if (entities2.Contains(entity))
				{
					num3 += 2f;
				}
				if (num3 > num)
				{
					rot = base.transform.rotation;
					num = num3;
				}
				handle2.Return();
			}
		}
		base.transform.rotation = rot;
		RpcDoStabEffects(rot);
		if (lookAtTarget)
		{
			base.info.caster.Control.RotateTowards(entity, immediately: true, overrideRotDuration);
		}
		else
		{
			base.info.caster.Control.Rotate(rot, immediately: true, overrideRotDuration);
		}
		base.info.caster.Animation.PlayAbilityAnimation(endAnimation);
		ArrayReturnHandle<Entity> handle3;
		ReadOnlySpan<Entity> entities3 = stabRange.GetEntities(out handle3, tvDefaultHarmfulEffectTargets);
		for (int j = 0; j < entities3.Length; j++)
		{
			Entity entity2 = entities3[j];
			FxPlayNewNetworked(stabHitEffect, entity2);
			Damage(dmgFactor).DoAttackEffect(AttackEffectType.Others).SetOriginPosition(base.info.caster.position).Dispatch(entity2);
		}
		handle3.Return();
		Destroy();
	}

	[ClientRpc]
	private void RpcDoStabEffects(Quaternion rot)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteQuaternion(rot);
		SendRPCInternal("System.Void Ai_Q_Lunge::RpcDoStabEffects(UnityEngine.Quaternion)", 277725726, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcDoStabEffects__Quaternion(Quaternion rot)
	{
		base.transform.rotation = rot;
		FxPlay(stabEffect);
	}

	protected static void InvokeUserCode_RpcDoStabEffects__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDoStabEffects called on server.");
		}
		else
		{
			((Ai_Q_Lunge)obj).UserCode_RpcDoStabEffects__Quaternion(reader.ReadQuaternion());
		}
	}

	static Ai_Q_Lunge()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Q_Lunge), "System.Void Ai_Q_Lunge::RpcDoStabEffects(UnityEngine.Quaternion)", InvokeUserCode_RpcDoStabEffects__Quaternion);
	}
}
