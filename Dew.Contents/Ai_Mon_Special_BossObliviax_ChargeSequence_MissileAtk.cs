using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk : AbilityInstance
{
	private class Ad_ObliviaxMissile
	{
		public float time;
	}

	[HideInInspector]
	public Vector3 initPoint;

	[HideInInspector]
	public float duration;

	public float projectileSpeed;

	public float damageInterval;

	public ScalingValue dmgFactor;

	public float collisionRadius;

	public GameObject fxTargetEntity;

	public GameObject fxHit;

	public GameObject fxStart;

	private float _initTime;

	protected override void OnPrepare()
	{
		base.OnPrepare();
		FxPlayNetworked(fxTargetEntity, base.info.target);
	}

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_initTime = Time.time;
			base.info.target.AddData(new Ad_ObliviaxMissile
			{
				time = _initTime
			});
		}
		base.transform.position = initPoint;
		FxPlay(fxStart);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		FxStop(fxStart);
		if (base.isServer)
		{
			FxStopNetworked(fxTargetEntity);
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer)
		{
			return;
		}
		if (Time.time - _initTime >= duration)
		{
			DestroyIfActive();
		}
		SyncProjectilePosition(dt, base.transform.position, base.info.target.agentPosition);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, base.transform.position, collisionRadius, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Entity entity = readOnlySpan[i];
			if (!entity.IsNullInactiveDeadOrKnockedOut())
			{
				if (!entity.HasData<Ad_ObliviaxMissile>())
				{
					entity.AddData(new Ad_ObliviaxMissile
					{
						time = Time.time
					});
				}
				entity.TryGetData<Ad_ObliviaxMissile>(out var data);
				if (!(Time.time - data.time <= damageInterval))
				{
					CreateDamage(DamageData.SourceType.Default, dmgFactor).SetOriginPosition(base.transform.position).Dispatch(entity);
					FxPlayNewNetworked(fxHit, entity);
					data.time = Time.time;
				}
			}
		}
		handle.Return();
	}

	[ClientRpc]
	private void SyncProjectilePosition(float dt, Vector3 startPos, Vector3 targetPos)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(dt);
		writer.WriteVector3(startPos);
		writer.WriteVector3(targetPos);
		SendRPCInternal("System.Void Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk::SyncProjectilePosition(System.Single,UnityEngine.Vector3,UnityEngine.Vector3)", -770393773, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_SyncProjectilePosition__Single__Vector3__Vector3(float dt, Vector3 startPos, Vector3 targetPos)
	{
		base.transform.position = startPos + (targetPos - base.transform.position).Flattened().normalized * (dt * projectileSpeed);
	}

	protected static void InvokeUserCode_SyncProjectilePosition__Single__Vector3__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC SyncProjectilePosition called on server.");
		}
		else
		{
			((Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk)obj).UserCode_SyncProjectilePosition__Single__Vector3__Vector3(reader.ReadFloat(), reader.ReadVector3(), reader.ReadVector3());
		}
	}

	static Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk), "System.Void Ai_Mon_Special_BossObliviax_ChargeSequence_MissileAtk::SyncProjectilePosition(System.Single,UnityEngine.Vector3,UnityEngine.Vector3)", InvokeUserCode_SyncProjectilePosition__Single__Vector3__Vector3);
	}
}
