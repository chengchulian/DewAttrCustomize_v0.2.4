using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomMod_DistantMemories : RoomModifierBase
{
	public float spawnDensity;

	public float delay;

	public GameObject fxEnd;

	private PropEnt_Stone_Nightmare _prop;

	private Rift_RoomExit _portal;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (!base.isNewInstance)
		{
			return;
		}
		SingletonDewNetworkBehaviour<Room>.instance.rewards.DisableRegularRewards();
		ManagerBase<MusicManager>.instance.Stop();
		SingletonDewNetworkBehaviour<Room>.instance.openRoomExitOnClear = false;
		_portal = Rift_RoomExit.instance;
		foreach (RoomSection section in SingletonDewNetworkBehaviour<Room>.instance.sections)
		{
			section.monsters.combatAreaSettings = SectionCombatAreaType.No;
		}
	}

	public override void OnLateStartServer()
	{
		base.OnLateStartServer();
		Room_Barrier[] array = global::UnityEngine.Object.FindObjectsOfType<Room_Barrier>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Open();
		}
		SingletonDewNetworkBehaviour<Room>.instance.monsters.RemoveAllCamps();
		Entity[] array2 = NetworkedManagerBase<ActorManager>.instance.allEntities.ToArray();
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i] is Monster monster && monster.GetTeamRelation(DewPlayer.local) == TeamRelation.Enemy)
			{
				monster.Destroy();
			}
		}
		Vector3 goodWanderPosition = SingletonDewNetworkBehaviour<Room>.instance.GetFinalSection().GetGoodWanderPosition(_portal.transform.position);
		_prop = SpawnEntity<PropEnt_Stone_Nightmare>(goodWanderPosition, null, DewPlayer.environment, 1);
		_prop.EntityEvent_OnDeath += new Action<EventInfoKill>(OnPropKilled);
		ModifyEntities(delegate(Entity e)
		{
			if (e is Monster)
			{
				e.CreateStatusEffect<Se_DistantMemories>(e, new CastInfo(e));
			}
		}, delegate(Entity e)
		{
			if (e.Status.TryGetStatusEffect<Se_DistantMemories>(out var effect))
			{
				effect.Destroy();
			}
		});
		List<MonsterPool.SpawnRuleEntry> filteredEntries = SingletonDewNetworkBehaviour<Room>.instance.monsters.defaultRule.pool.GetFilteredEntries();
		int count = filteredEntries.Count;
		foreach (RoomSection section in SingletonDewNetworkBehaviour<Room>.instance.sections)
		{
			int num = Mathf.RoundToInt(section.area / spawnDensity);
			for (int j = 0; j < num; j++)
			{
				Monster monster2 = filteredEntries[global::UnityEngine.Random.Range(0, count)].monster;
				Vector3 anyRandomNode = section.GetAnyRandomNode();
				Monster monster3 = SpawnEntity(monster2, anyRandomNode, null, DewPlayer.environment, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Monster b)
				{
					b.Visual.skipSpawning = true;
				});
				monster3.Sound.voiceStart = null;
				monster3.Sound.voiceIdle = null;
			}
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(_prop == null))
		{
			_prop.EntityEvent_OnDeath -= new Action<EventInfoKill>(OnPropKilled);
		}
	}

	private void OnPropKilled(EventInfoKill obj)
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			FxPlayNetworked(fxEnd, _prop.transform.position, null);
			yield return new WaitForSeconds(1f);
			Entity[] array = NetworkedManagerBase<ActorManager>.instance.allEntities.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is Monster monster && !monster.IsNullInactiveDeadOrKnockedOut())
				{
					monster.Destroy();
				}
			}
			SingletonDewNetworkBehaviour<Room>.instance.ClearRoom();
			RemoveModifier();
			ManagerBase<MusicManager>.instance.Play(SingletonDewNetworkBehaviour<Room>.instance.music);
			yield return new WaitForSeconds(delay);
			_portal.Open();
			CreateActor(obj.victim.agentPosition, null, delegate(Shrine_Chaos b)
			{
				b.SetRandomRarity(isHighQuality: false);
			});
		}
	}

	private void MirrorProcessed()
	{
	}
}
