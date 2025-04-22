using System;
using System.Linq;
using Mirror;
using UnityEngine;

public class RoomMod_Ambush : RoomModifierBase
{
	private Mon_Special_AmbushSpawner _prop;

	private RoomSection _final;

	public override void OnLateStartServer()
	{
		base.OnLateStartServer();
		if (!base.isNewInstance)
		{
			return;
		}
		Room_Barrier[] array = global::UnityEngine.Object.FindObjectsOfType<Room_Barrier>();
		for (int i = 0; i < array.Length; i++)
		{
			NetworkServer.Destroy(array[i].gameObject);
		}
		Room instance = SingletonDewNetworkBehaviour<Room>.instance;
		instance.rewards.DisableRegularRewards();
		instance.openRoomExitOnClear = false;
		instance.monsters.RemoveAllCamps();
		_final = instance.GetFinalSection();
		if (!_final.TryGetGoodNodePosition(out var anyRandomNode))
		{
			anyRandomNode = _final.GetAnyRandomNode();
		}
		_prop = Dew.SpawnEntity<Mon_Special_AmbushSpawner>(anyRandomNode, null, null, DewPlayer.creep, NetworkedManagerBase<GameManager>.instance.ambientLevel);
		_prop.ClientActorEvent_OnDestroyed += new Action<Actor>(OnPropDestroyed);
		RoomMod_Hunted hunted = DewResources.GetByType<RoomMod_Hunted>();
		ModifyEntities(delegate(Entity e)
		{
			if (e is Monster && !(e is Mon_Special_AmbushSpawner))
			{
				CreateStatusEffect<Se_HunterBuff>(e, new CastInfo(e));
				hunted.ApplyHunterStatBonusAndAIPrediction(e, NetworkedManagerBase<ZoneManager>.instance.currentHuntLevel);
			}
		}, delegate(Entity e)
		{
			if (e.Status.TryGetStatusEffect<Se_HunterBuff>(out var effect))
			{
				effect.Destroy();
			}
		});
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _prop != null)
		{
			_prop.ClientActorEvent_OnDestroyed -= new Action<Actor>(OnPropDestroyed);
		}
	}

	private void OnPropDestroyed(Actor obj)
	{
		Entity[] array = NetworkedManagerBase<ActorManager>.instance.allEntities.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is Monster monster && !monster.IsNullInactiveDeadOrKnockedOut())
			{
				monster.Destroy();
			}
		}
		SingletonDewNetworkBehaviour<Room>.instance.monsters.FinishAllOngoingSpawns();
		SingletonDewNetworkBehaviour<Room>.instance.monsters.RemoveAllCamps();
		SingletonDewNetworkBehaviour<Room>.instance.ClearRoom();
		RemoveModifier();
		Rift_RoomExit.instance.Open();
	}

	private void MirrorProcessed()
	{
	}
}
