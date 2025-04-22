using System.Collections.Generic;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class RoomMod_PureDream : RoomModifierBase
{
	public Vector2Int dreamProps;

	public Vector2Int dreamStatues;

	public Material matDreamDust;

	private List<(int, int)> _indices;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (!base.isNewInstance)
		{
			return;
		}
		SingletonDewNetworkBehaviour<Room>.instance.rewards.DisableRegularRewards();
		_indices = new List<(int, int)>(SingletonDewNetworkBehaviour<Room>.instance.map.mapData.innerPropNodeIndices);
		int num = Random.Range(dreamProps.x, dreamProps.y + 1);
		for (int i = 0; i < num; i++)
		{
			Vector3 pos = CalSpawnPosition();
			SpawnEntity<PropEnt_Stone_DreamDust>(pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), DewPlayer.environment, 1);
		}
		ModifyEntities(delegate(Entity e)
		{
			if (e is Monster)
			{
				e.CreateStatusEffect<Se_PureDream>(e, new CastInfo(e));
			}
		}, delegate(Entity e)
		{
			if (e.Status.TryGetStatusEffect<Se_PureDream>(out var effect))
			{
				effect.Destroy();
			}
		});
		GameManager.CallOnReady(delegate
		{
			Routine();
		});
	}

	private Vector3 CalSpawnPosition()
	{
		if (_indices.Count == 0)
		{
			return SingletonDewNetworkBehaviour<Room>.instance.sections[Random.Range(0, SingletonDewNetworkBehaviour<Room>.instance.sections.Count)].GetAnyRandomNode();
		}
		int index = Random.Range(0, _indices.Count);
		Vector3 positionOnGround = Dew.GetPositionOnGround(SingletonDewNetworkBehaviour<Room>.instance.map.mapData.cells.GetWorldPos(_indices[index]).ToXZ() + Vector3.up * 100f, 200f);
		Vector3 positionOnGround2 = Dew.GetPositionOnGround(positionOnGround + Random.insideUnitSphere * 1f);
		positionOnGround2 = Dew.GetValidAgentDestination_LinearSweep(positionOnGround, positionOnGround2);
		_indices.RemoveAt(index);
		return positionOnGround2;
	}

	private void Routine()
	{
		List<MonsterPool.SpawnRuleEntry> filteredEntries = SingletonDewNetworkBehaviour<Room>.instance.monsters.defaultRule.pool.GetFilteredEntries();
		int count = filteredEntries.Count;
		List<MonsterPool.SpawnRuleEntry> list = new List<MonsterPool.SpawnRuleEntry>();
		for (int i = 0; i < count; i++)
		{
			if (!(filteredEntries[i].monster is Mon_Sky_StarSeed))
			{
				list.Add(filteredEntries[i]);
			}
		}
		filteredEntries.Clear();
		count = list.Count;
		int num = Random.Range(dreamStatues.x, dreamStatues.y + 1);
		for (int j = 0; j < num; j++)
		{
			Monster monster = list[Random.Range(0, count)].monster;
			Vector3 pos = CalSpawnPosition();
			Monster monster2 = SpawnEntity(monster, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), DewPlayer.environment, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Monster b)
			{
				b.Visual.skipSpawning = true;
				b.AI.disableAI = true;
				b.Sound.voiceStart = null;
				b.Sound.voiceIdle = null;
				b.Sound.voiceDeath = null;
				b.populationCost = 0f;
			});
			RpcModifiyStatueEntity(monster2);
			CreateStatusEffect<Se_PureDream_Statue>(monster2, new CastInfo(monster2));
		}
	}

	[ClientRpc]
	private void RpcModifiyStatueEntity(Entity b)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(b);
		SendRPCInternal("System.Void RoomMod_PureDream::RpcModifiyStatueEntity(Entity)", -812774626, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer)
		{
			_indices.Clear();
		}
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcModifiyStatueEntity__Entity(Entity b)
	{
		List<Renderer> list = new List<Renderer>();
		list.AddRange(b.GetComponentsInChildren<SkinnedMeshRenderer>());
		foreach (Renderer item in list)
		{
			Material[] sharedMaterials = item.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i] = matDreamDust;
			}
			item.sharedMaterials = sharedMaterials;
		}
		Animator componentInChildren = b.GetComponentInChildren<Animator>();
		if (componentInChildren != null)
		{
			componentInChildren.enabled = false;
		}
		b.Visual.deathBehavior = EntityVisual.EntityDeathBehavior.HideModel;
	}

	protected static void InvokeUserCode_RpcModifiyStatueEntity__Entity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcModifiyStatueEntity called on server.");
		}
		else
		{
			((RoomMod_PureDream)obj).UserCode_RpcModifiyStatueEntity__Entity(reader.ReadNetworkBehaviour<Entity>());
		}
	}

	static RoomMod_PureDream()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(RoomMod_PureDream), "System.Void RoomMod_PureDream::RpcModifiyStatueEntity(Entity)", InvokeUserCode_RpcModifiyStatueEntity__Entity);
	}
}
