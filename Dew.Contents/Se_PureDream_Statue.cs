using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Se_PureDream_Statue : Se_PureDream
{
	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DoUnstoppable();
			RpcApplyTransform(base.victim, Random.Range(1.2f, 1.3f));
		}
	}

	[ClientRpc]
	private void RpcApplyTransform(Entity ent, float scale)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(ent);
		writer.WriteFloat(scale);
		SendRPCInternal("System.Void Se_PureDream_Statue::RpcApplyTransform(Entity,System.Single)", -640710747, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcApplyTransform__Entity__Single(Entity ent, float scale)
	{
		ent.Visual.GetNewTransformModifier().scaleMultiplier = Vector3.one * scale;
	}

	protected static void InvokeUserCode_RpcApplyTransform__Entity__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcApplyTransform called on server.");
		}
		else
		{
			((Se_PureDream_Statue)obj).UserCode_RpcApplyTransform__Entity__Single(reader.ReadNetworkBehaviour<Entity>(), reader.ReadFloat());
		}
	}

	static Se_PureDream_Statue()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Se_PureDream_Statue), "System.Void Se_PureDream_Statue::RpcApplyTransform(Entity,System.Single)", InvokeUserCode_RpcApplyTransform__Entity__Single);
	}
}
