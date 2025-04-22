using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_Mon_Special_BossObliviax_TurnAtk : AbilityInstance
{
	public float maxRotation;

	public float rotDuration;

	public float allowMarginDistance;

	public DewCollider range;

	public float angleDeviation;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public GameObject fxMain;

	public GameObject fxHit;

	public int projectileCount;

	public Vector2 projectileRange;

	private EntityTransformModifier _entTransform;

	private float _initialTime;

	private float _rotSpeed;

	[SyncVar]
	private bool _isComplete;

	public bool Network_isComplete
	{
		get
		{
			return _isComplete;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _isComplete, 32uL, null);
		}
	}

	protected override IEnumerator OnCreateSequenced()
	{
		_entTransform = base.info.caster.Visual.GetNewTransformModifier();
		if (!base.isServer)
		{
			yield break;
		}
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			FxPlayNewNetworked(fxHit, entity);
			CreateDamage(DamageData.SourceType.Default, dmgFactor).SetOriginPosition(base.info.caster.agentPosition).Dispatch(entity);
			knockback.ApplyWithOrigin(base.info.caster.agentPosition, entity);
		}
		handle.Return();
		base.info.caster.Control.Rotate(ManagerBase<CameraManager>.instance.entityCamAngle + 220f, immediately: false);
		_rotSpeed = maxRotation / rotDuration;
		RpcRotate(_rotSpeed);
		FxPlayNetworked(fxMain, base.info.caster);
		yield return new SI.WaitForSeconds(0.05f);
		projectileCount = Mathf.CeilToInt((float)projectileCount * NetworkedManagerBase<GameManager>.instance.GetSpecialSkillChanceMultiplier());
		if (projectileCount <= 8)
		{
			projectileCount = 8;
		}
		float initialAngle = global::UnityEngine.Random.Range(0f, 360f);
		int addedAngle = 360 / projectileCount;
		for (int j = 0; j < projectileCount; j++)
		{
			float y = initialAngle + (float)(addedAngle * j) + global::UnityEngine.Random.Range(0f - angleDeviation, angleDeviation);
			Vector3 vector = (Quaternion.Euler(0f, y, 0f) * base.info.forward).normalized * global::UnityEngine.Random.Range(projectileRange.x, projectileRange.y);
			Vector3 end = base.info.caster.agentPosition + vector;
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(base.info.caster.agentPosition, end);
			if (!(end.sqrMagnitude - validAgentDestination_LinearSweep.sqrMagnitude >= allowMarginDistance))
			{
				CreateAbilityInstance<Ai_Mon_Special_BossObliviax_TurnAtk_Projectile>(validAgentDestination_LinearSweep, null, new CastInfo(base.info.caster, validAgentDestination_LinearSweep));
				yield return new SI.WaitForSeconds(0.05f);
			}
		}
		Destroy();
	}

	[ClientRpc]
	private void RpcRotate(float rotSpeed)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteFloat(rotSpeed);
		SendRPCInternal("System.Void Ai_Mon_Special_BossObliviax_TurnAtk::RpcRotate(System.Single)", -194085383, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_entTransform != null)
		{
			_entTransform.Stop();
			_entTransform = null;
		}
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcRotate__Single(float rotSpeed)
	{
		StartSequence(Routine());
		IEnumerator Routine()
		{
			float angle = 0f;
			for (float t = 0f; t < rotDuration; t += 1f / 30f)
			{
				float num = t / rotDuration;
				angle += _rotSpeed * num;
				_entTransform.rotation = Quaternion.Euler(0f, 0f - angle, 0f);
				if (angle >= maxRotation)
				{
					break;
				}
				yield return null;
			}
			if (base.isServer)
			{
				Network_isComplete = true;
			}
		}
	}

	protected static void InvokeUserCode_RpcRotate__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRotate called on server.");
		}
		else
		{
			((Ai_Mon_Special_BossObliviax_TurnAtk)obj).UserCode_RpcRotate__Single(reader.ReadFloat());
		}
	}

	static Ai_Mon_Special_BossObliviax_TurnAtk()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Mon_Special_BossObliviax_TurnAtk), "System.Void Ai_Mon_Special_BossObliviax_TurnAtk::RpcRotate(System.Single)", InvokeUserCode_RpcRotate__Single);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_isComplete);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteBool(_isComplete);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _isComplete, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _isComplete, null, reader.ReadBool());
		}
	}
}
