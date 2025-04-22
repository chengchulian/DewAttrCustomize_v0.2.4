using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class RoomMod_LeafPuppies : RoomModifierBase
{
	public float spawnPopulationMultiplier = 1.4f;

	public Vector2 sizeMultiplier;

	public float bonusHealthPercentageBase = -30f;

	public float bonusHealthPercentagePerSize = 150f;

	public override void OnStartServer()
	{
		base.OnStartServer();
		SingletonDewNetworkBehaviour<Room>.instance.monsters.OverrideMonsterType(DewResources.GetByType<Mon_Forest_Hound>());
		SingletonDewNetworkBehaviour<Room>.instance.monsters.spawnedPopMultiplier *= spawnPopulationMultiplier;
		RoomMonsters monsters = SingletonDewNetworkBehaviour<Room>.instance.monsters;
		monsters.onAfterSpawn = (Action<Entity>)Delegate.Combine(monsters.onAfterSpawn, new Action<Entity>(OnAfterSpawn));
	}

	private void OnAfterSpawn(Entity obj)
	{
		float num = global::UnityEngine.Random.Range(sizeMultiplier.x, sizeMultiplier.y);
		RpcApplyTransform(obj, num);
		obj.Control.outerRadius *= num;
		obj.Status.AddStatBonus(new StatBonus
		{
			maxHealthPercentage = bonusHealthPercentageBase + bonusHealthPercentagePerSize * (num - 1f)
		});
	}

	[ClientRpc]
	private void RpcApplyTransform(Entity ent, float scale)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(ent);
		writer.WriteFloat(scale);
		SendRPCInternal("System.Void RoomMod_LeafPuppies::RpcApplyTransform(Entity,System.Single)", -211970482, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && SingletonDewNetworkBehaviour<Room>.instance != null)
		{
			SingletonDewNetworkBehaviour<Room>.instance.monsters.spawnedPopMultiplier /= spawnPopulationMultiplier;
			RoomMonsters monsters = SingletonDewNetworkBehaviour<Room>.instance.monsters;
			monsters.onAfterSpawn = (Action<Entity>)Delegate.Remove(monsters.onAfterSpawn, new Action<Entity>(OnAfterSpawn));
		}
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
			((RoomMod_LeafPuppies)obj).UserCode_RpcApplyTransform__Entity__Single(reader.ReadNetworkBehaviour<Entity>(), reader.ReadFloat());
		}
	}

	static RoomMod_LeafPuppies()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(RoomMod_LeafPuppies), "System.Void RoomMod_LeafPuppies::RpcApplyTransform(Entity,System.Single)", InvokeUserCode_RpcApplyTransform__Entity__Single);
	}
}
