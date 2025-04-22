using System;
using System.Collections;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Ai_Mon_Sky_LittleBaam_Atk : AbilityInstance
{
	public ScalingValue damage;

	public GameObject explodeEffect;

	public GameObject hitEffect;

	public DewCollider range;

	public bool stopOnCasterDeath;

	public int explodeCount;

	public float explodeInterval;

	public float maxAngle;

	public float angleDeviation;

	public AnimationCurve stepDistByDistance;

	public DewAudioSource explodeAudio;

	public AnimationCurve explodeAudioPitch;

	private int _explodeIndex;

	protected override IEnumerator OnCreateSequenced()
	{
		if (!base.isServer)
		{
			yield break;
		}
		if (stopOnCasterDeath)
		{
			DestroyOnDeath(base.info.caster);
		}
		Vector3 knownPos = base.info.target.agentPosition;
		float angle = CastInfo.GetAngle(base.info.target.agentPosition - base.info.caster.position);
		Vector3 pos = base.position;
		for (int i = 0; i < explodeCount; i++)
		{
			float target = CastInfo.GetAngle(knownPos - pos) + angleDeviation * (global::UnityEngine.Random.value * 2f - 1f);
			angle = Mathf.MoveTowardsAngle(angle, target, maxAngle);
			pos += Quaternion.Euler(0f, angle, 0f) * Vector3.forward * stepDistByDistance.Evaluate(Vector2.Distance(knownPos.ToXY(), pos.ToXY()));
			pos = Dew.GetPositionOnGround(pos);
			RpcExplode(pos);
			if (base.info.target != null && base.info.target.isActive && !base.info.target.Status.hasInvisible)
			{
				knownPos = base.info.target.agentPosition;
			}
			yield return new SI.WaitForSeconds(explodeInterval);
		}
		Destroy();
	}

	[ClientRpc]
	private void RpcExplode(Vector3 pos)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		SendRPCInternal("System.Void Ai_Mon_Sky_LittleBaam_Atk::RpcExplode(UnityEngine.Vector3)", 1700155998, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcExplode__Vector3(Vector3 pos)
	{
		base.transform.position = pos;
		explodeAudio.pitchMultiplier = explodeAudioPitch.Evaluate((float)_explodeIndex / (float)explodeCount);
		_explodeIndex++;
		FxPlayNew(explodeEffect);
		if (base.isServer)
		{
			ArrayReturnHandle<Entity> handle;
			ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
			for (int i = 0; i < entities.Length; i++)
			{
				Entity entity = entities[i];
				Damage(damage).SetElemental(ElementalType.Light).SetOriginPosition(pos).Dispatch(entity);
				FxPlayNewNetworked(hitEffect, entity);
			}
			handle.Return();
		}
	}

	protected static void InvokeUserCode_RpcExplode__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcExplode called on server.");
		}
		else
		{
			((Ai_Mon_Sky_LittleBaam_Atk)obj).UserCode_RpcExplode__Vector3(reader.ReadVector3());
		}
	}

	static Ai_Mon_Sky_LittleBaam_Atk()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Ai_Mon_Sky_LittleBaam_Atk), "System.Void Ai_Mon_Sky_LittleBaam_Atk::RpcExplode(UnityEngine.Vector3)", InvokeUserCode_RpcExplode__Vector3);
	}
}
