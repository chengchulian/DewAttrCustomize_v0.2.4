using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RoomComponentStartDependency(typeof(RoomModifiers))]
public class RoomMonsters : RoomComponent
{
	private struct ActorSaveData
	{
		public Type type;

		public ulong sceneId;

		public Dictionary<string, object> data;
	}

	private class RoomMonstersSaveData
	{
		public List<ActorSaveData> actors = new List<ActorSaveData>();
	}

	public class MonsterSpawnData
	{
		public Entity lastKiller;

		public Vector3 lastDeathPosition;

		public float remainingPopulation;
	}

	private class Camp
	{
		public Vector3 position;

		public bool wasInRange;

		public bool isSpawned;

		public List<SpawnEntry> entries = new List<SpawnEntry>();

		public float nextRegenerateChance = 1f;

		public float nextRegenerateTime = float.PositiveInfinity;

		public Camp(Vector3 position)
		{
			this.position = position;
		}

		public void DoCampTick()
		{
			bool isInRange = Dew.GetClosestHeroDistance(position) < 40f;
			if (!isInRange && isSpawned && AreAllEntitiesSleeping())
			{
				Despawn();
			}
			if (!wasInRange && isInRange)
			{
				if (entries.Count == 0)
				{
					RegenerateEntries();
				}
				else if (Time.time > nextRegenerateTime)
				{
					if (global::UnityEngine.Random.value < nextRegenerateChance)
					{
						RegenerateEntries();
					}
					else
					{
						nextRegenerateTime = Time.time + float.PositiveInfinity;
					}
				}
				if (!isSpawned && HasAnyAliveEntries())
				{
					Spawn();
				}
			}
			wasInRange = isInRange;
		}

		private bool AreAllEntitiesSleeping()
		{
			foreach (SpawnEntry e in entries)
			{
				if (!e.instance.IsNullOrInactive() && !e.instance.isSleeping)
				{
					return false;
				}
			}
			return true;
		}

		private bool HasAnyAliveEntries()
		{
			foreach (SpawnEntry entry in entries)
			{
				if (!entry.isKilled)
				{
					return true;
				}
			}
			return false;
		}

		public void RegenerateEntries()
		{
			if (isSpawned)
			{
				throw new InvalidOperationException();
			}
			RoomMonsters mon = SingletonDewNetworkBehaviour<Room>.instance.monsters;
			float totalPopToSpawn = global::UnityEngine.Random.Range(mon.campPopulation.x, mon.campPopulation.y);
			totalPopToSpawn = NetworkedManagerBase<GameManager>.instance.GetAdjustedMonsterSpawnPopulation(totalPopToSpawn) * SingletonDewNetworkBehaviour<Room>.instance.monsters.spawnedPopMultiplier;
			IEnumerator<Monster> enumerator = ((mon.envSpawnPoolOverride != null) ? mon.envSpawnPoolOverride : mon.defaultRule.pool).GetMonsters(int.MaxValue);
			enumerator.MoveNext();
			float spawnedPop = 0f;
			while (true)
			{
				float popCost = enumerator.Current.populationCost;
				entries.Add(new SpawnEntry
				{
					prefab = enumerator.Current
				});
				spawnedPop += popCost;
				if (spawnedPop > totalPopToSpawn)
				{
					break;
				}
				enumerator.MoveNext();
			}
			nextRegenerateChance *= 0.65f;
			nextRegenerateTime = float.PositiveInfinity;
		}

		public void Spawn()
		{
			for (int i = 0; i < entries.Count; i++)
			{
				SpawnEntry entry = entries[i];
				if (entry.isKilled)
				{
					continue;
				}
				int index = i;
				Vector3 pos = Dew.GetPositionOnGround(position + global::UnityEngine.Random.insideUnitSphere * 6f);
				pos = Dew.GetValidAgentDestination_LinearSweep(position, pos);
				entry.instance = Dew.SpawnEntity(entry.prefab, pos, Quaternion.Euler(0f, global::UnityEngine.Random.Range(0f, 360f), 0f), NetworkedManagerBase<ActorManager>.instance.serverActor, DewPlayer.creep, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Entity entity)
				{
					entity._accumulatedSleepTime = float.PositiveInfinity;
					entity.Visual.NetworkskipSpawning = true;
					if (entity is Monster monster)
					{
						monster.campPosition = pos;
					}
					entity.EntityEvent_OnDeath += (Action<EventInfoKill>)delegate
					{
						SpawnEntry value = entries[index];
						value.isKilled = true;
						entries[index] = value;
						nextRegenerateTime = Time.time + float.PositiveInfinity;
						if (isSpawned)
						{
							foreach (SpawnEntry entry2 in entries)
							{
								if (!entry2.isKilled)
								{
									return;
								}
							}
							isSpawned = false;
						}
					};
					SingletonDewNetworkBehaviour<Room>.instance.monsters.onBeforeSpawn?.Invoke(entity);
				});
				SingletonDewNetworkBehaviour<Room>.instance.monsters.onAfterSpawn?.Invoke(entry.instance);
				entries[i] = entry;
			}
			isSpawned = true;
		}

		public void Despawn()
		{
			for (int i = 0; i < entries.Count; i++)
			{
				SpawnEntry e = entries[i];
				if (!e.instance.IsNullOrInactive())
				{
					e.instance.Destroy();
					e.instance = null;
					entries[i] = e;
				}
			}
			isSpawned = false;
		}
	}

	private struct SpawnEntry
	{
		public Entity prefab;

		public Entity instance;

		public bool isKilled;
	}

	public const float CombatAreaScoreRandomness = 0.3f;

	public Action<Entity> onBeforeSpawn;

	public Action<Entity> onAfterSpawn;

	public int insertedCombatAreas;

	public float spawnedPopMultiplier = 1f;

	public float maxPopulationMultiplier = 1f;

	public float addedMirageChance;

	public float addedHunterChance;

	public bool clearRoomOnClearAllCombatAreas = true;

	public Dictionary<SpawnMonsterSettings, Coroutine> ongoingSpawns = new Dictionary<SpawnMonsterSettings, Coroutine>();

	public SafeAction onWelcomingSpawnStart;

	public SafeAction onWelcomingSpawnEnd;

	private static List<MiniBossEffect> _miniBossEffects;

	public const float CampWanderRange = 6f;

	private const float RespawnChanceMultiplier = 0.65f;

	private const float CampRespawnTimeAfterBeingSeen = float.PositiveInfinity;

	private const float CampScoreFuzziness = 0.15f;

	private const float BanCampDistanceFromHeroSpawn = 25f;

	private const float BanCampDistanceFromBanPosition = 3f;

	private const float CampActivateDistanceFromHero = 40f;

	private const float EnvSpawnTickInterval = 1f;

	[Space(16f)]
	public bool disableEnvSpawn;

	public MonsterPool envSpawnPoolOverride;

	public float campDensity;

	public Vector2 campPopulation = new Vector2(0.5f, 3f);

	private List<Camp> _camps;

	private float _nextEnvSpawnTickTime;

	public const float OverpopulationStallDelayMin = 0.5f;

	public const float OverpopulationStallDelayMax = 1.5f;

	public const float SpawnDelayMin = 0.1f;

	public const float SpawnDelayMax = 0.5f;

	public MonsterSpawnRule defaultRule;

	private static NavMeshPath _tempPath;

	public bool didSetupCombatAreas { get; private set; }

	public bool isDoingHunterWelcomingSpawn { get; private set; }

	public override void OnRoomStart(bool isRevisit)
	{
		base.OnRoomStart(isRevisit);
		DewResources.AddPreloadRule(this, delegate(PreloadInterface preload)
		{
			if (defaultRule != null && defaultRule.pool != null)
			{
				preload.AddFromMonsterPool(defaultRule.pool);
			}
			foreach (RoomSection current in base.room.sections)
			{
				if (!(current == null) && !(current.monsters.ruleOverride == null) && !(current.monsters.ruleOverride.pool == null))
				{
					preload.AddFromMonsterPool(current.monsters.ruleOverride.pool);
				}
			}
		});
	}

	public override void OnRoomStartServer(WorldNodeSaveData save)
	{
		base.OnRoomStartServer(save);
		if (defaultRule == null)
		{
			defaultRule = NetworkedManagerBase<ZoneManager>.instance.currentZone.defaultMonsters;
		}
		if (save == null)
		{
			SetupCombatAreas();
			DoEnvSpawnRoomStart();
			if (NetworkedManagerBase<ZoneManager>.instance.currentNode.HasMainModifier())
			{
				RoomSection section = base.room.GetSectionFromWorldPos(base.room.heroSpawnPos);
				if (section != null)
				{
					section.monsters.addedInitDelay += Vector2.one * 1.5f;
				}
			}
			if (!clearRoomOnClearAllCombatAreas)
			{
				return;
			}
			RefValue<int> remaining = new RefValue<int>(0);
			foreach (RoomSection s in base.room.sections)
			{
				if (!s.monsters.isMarkedAsCombatArea)
				{
					continue;
				}
				remaining.value++;
				s.monsters.onClearCombatArea.AddListener(delegate
				{
					remaining.value--;
					if (remaining.value <= 0)
					{
						s.monsters.StartCoroutine(Routine());
					}
				});
			}
		}
		else
		{
			foreach (ActorSaveData actorSave in save.Get<RoomMonstersSaveData>().actors)
			{
				Actor targetActor;
				if (actorSave.sceneId == 0L)
				{
					targetActor = DewResources.GetByType<Actor>(actorSave.type).OnLoadCreateActor(actorSave.data, null);
				}
				else
				{
					targetActor = null;
					foreach (Actor a in NetworkedManagerBase<ActorManager>.instance.allActors)
					{
						if (a.netIdentity.sceneId == actorSave.sceneId)
						{
							targetActor = a;
							break;
						}
					}
					if (targetActor == null)
					{
						Debug.LogWarning($"Could not find scene actor to load in: {actorSave.type.Name} ({actorSave.sceneId})");
						continue;
					}
				}
				targetActor.OnLoadActor(actorSave.data);
			}
			Room_Barrier[] array = global::UnityEngine.Object.FindObjectsOfType<Room_Barrier>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Open();
			}
		}
		Rift[] rifts;
		if (NetworkedManagerBase<ZoneManager>.instance.isCurrentNodeHunted)
		{
			isDoingHunterWelcomingSpawn = true;
			onWelcomingSpawnStart?.Invoke();
			rifts = global::UnityEngine.Object.FindObjectsOfType<Rift>();
			Rift[] array2 = rifts;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].isLocked = true;
			}
			StartCoroutine(Routine());
		}
		static IEnumerator Routine()
		{
			yield return Dew.WaitForAggroedEnemiesRoutine();
			if (!SingletonDewNetworkBehaviour<Room>.instance.didClearRoom)
			{
				SingletonDewNetworkBehaviour<Room>.instance.ClearRoom();
			}
		}
		IEnumerator Routine()
		{
			yield return new WaitWhile(() => NetworkedManagerBase<ZoneManager>.instance.isInRoomTransition);
			yield return new WaitForSeconds(global::UnityEngine.Random.Range(0.5f, 1f));
			if (NetworkedManagerBase<ZoneManager>.instance.currentNode.HasMainModifier() && save == null)
			{
				yield return new WaitForSeconds(1.25f);
			}
			SpawnMonsters(new SpawnMonsterSettings
			{
				rule = defaultRule,
				initDelayMultiplier = 0.1f,
				spawnPopulationMultiplier = NetworkedManagerBase<GameManager>.instance.gss.welcomingSpawnPopMultiplierByArea.Evaluate(base.room.map.mapData.area),
				onFinish = delegate
				{
					Rift[] array3 = rifts;
					for (int j = 0; j < array3.Length; j++)
					{
						array3[j].isLocked = false;
					}
					isDoingHunterWelcomingSpawn = false;
					onWelcomingSpawnEnd?.Invoke();
				}
			});
		}
	}

	public override void OnRoomStopServer(WorldNodeSaveData save)
	{
		base.OnRoomStopServer(save);
		DoEnvSpawnRoomStop();
		foreach (KeyValuePair<SpawnMonsterSettings, Coroutine> p in ongoingSpawns)
		{
			StopCoroutine(p.Value);
			p.Key.onFinish?.Invoke();
		}
		ongoingSpawns.Clear();
		StopAllCoroutines();
		RoomMonstersSaveData data = new RoomMonstersSaveData();
		foreach (Actor e in NetworkedManagerBase<ActorManager>.instance.allActors)
		{
			if (e.isActive && e.isDestroyedOnRoomChange && e.ShouldBeSaved() && !e.IsExcludedFromRoomSave())
			{
				ActorSaveData actorSaveData = default(ActorSaveData);
				actorSaveData.type = e.GetType();
				actorSaveData.sceneId = e.netIdentity.sceneId;
				actorSaveData.data = new Dictionary<string, object>();
				ActorSaveData actorSave = actorSaveData;
				try
				{
					e.OnSaveActor(actorSave.data);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				data.actors.Add(actorSave);
			}
		}
		save.Set(data);
	}

	public override void OnRoomStop()
	{
		base.OnRoomStop();
		foreach (Actor allActor in NetworkedManagerBase<ActorManager>.instance.allActors)
		{
			allActor.netIdentity.sceneId = 0uL;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		DoEnvSpawnLogicUpdate(dt);
	}

	private void SetupCombatAreas()
	{
		didSetupCombatAreas = true;
		int requiredRiftAreas = 0;
		int numOfCurrentRiftAreas = 0;
		List<RoomSection> combatAreas = new List<RoomSection>();
		int inserted = 0;
		foreach (RoomSection s2 in base.room.sections)
		{
			if (s2.monsters.combatAreaSettings == SectionCombatAreaType.Yes)
			{
				inserted--;
				InsertCombatArea(s2);
			}
		}
		if (insertedCombatAreas <= 0)
		{
			return;
		}
		if (combatAreas.Count == 0)
		{
			List<RoomSection> can = new List<RoomSection>();
			foreach (RoomSection s3 in base.room.sections)
			{
				if (s3.monsters.combatAreaSettings == SectionCombatAreaType.Random)
				{
					can.Add(s3);
				}
			}
			if (can.Count == 0)
			{
				LogNotEnoughCandidates();
				return;
			}
			RoomSection selected = can[global::UnityEngine.Random.Range(0, can.Count)];
			InsertCombatArea(selected);
		}
		Dictionary<RoomSection, float> scores = new Dictionary<RoomSection, float>();
		while (true)
		{
			scores.Clear();
			foreach (RoomSection s4 in base.room.sections)
			{
				if (combatAreas.Contains(s4) || s4.monsters.combatAreaSettings != 0)
				{
					continue;
				}
				float distToNearestCombatArea = float.PositiveInfinity;
				foreach (RoomSection c in combatAreas)
				{
					float dist = s4.GetNavDistanceTo(c);
					if (dist < distToNearestCombatArea)
					{
						distToNearestCombatArea = dist;
					}
				}
				float score = distToNearestCombatArea * (1f + global::UnityEngine.Random.Range(-0.3f, 0.3f));
				if (numOfCurrentRiftAreas < requiredRiftAreas && s4.GetComponentInChildren<Room_RiftPos>() != null)
				{
					score += 100f;
				}
				scores.Add(s4, score);
			}
			if (scores.Count == 0)
			{
				LogNotEnoughCandidates();
				break;
			}
			float bestScore = float.NegativeInfinity;
			RoomSection bestSection = null;
			foreach (KeyValuePair<RoomSection, float> p in scores)
			{
				if (!(p.Value <= bestScore))
				{
					bestScore = p.Value;
					bestSection = p.Key;
				}
			}
			InsertCombatArea(bestSection);
			inserted++;
			if (inserted >= insertedCombatAreas)
			{
				if (numOfCurrentRiftAreas >= requiredRiftAreas)
				{
					break;
				}
				Debug.Log($"Inserted {inserted} combat areas but did not meet minimum rift area requirement ({numOfCurrentRiftAreas}/{requiredRiftAreas}), continuing...");
			}
		}
		void InsertCombatArea(RoomSection s)
		{
			combatAreas.Add(s);
			s.monsters.isMarkedAsCombatArea = true;
			if (s.GetComponentInChildren<Room_RiftPos>() != null)
			{
				numOfCurrentRiftAreas++;
			}
		}
		void LogNotEnoughCandidates()
		{
			Debug.LogWarning("Room " + SceneManager.GetActiveScene().name + " does not have enough combat area candidates");
			Debug.LogWarning($"Inserted: {inserted}/{insertedCombatAreas}, Rift: {numOfCurrentRiftAreas}/{requiredRiftAreas}");
		}
	}

	[Server]
	public void OverrideMonsterType(Monster m)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RoomMonsters::OverrideMonsterType(Monster)' called when server was not active");
			return;
		}
		if (defaultRule == null)
		{
			defaultRule = NetworkedManagerBase<ZoneManager>.instance.currentZone.defaultMonsters;
		}
		defaultRule = global::UnityEngine.Object.Instantiate(defaultRule);
		defaultRule.pool = global::UnityEngine.Object.Instantiate(defaultRule.pool);
		defaultRule.pool.entries.Clear();
		defaultRule.pool.entries.Add(new MonsterPool.SpawnRuleEntry
		{
			chance = 1f,
			monsterRef = m.ToAssetRef(),
			minCount = 1,
			maxCount = 1
		});
		foreach (RoomSection section in base.room.sections)
		{
			section.monsters.ruleOverride = defaultRule;
		}
	}

	[Server]
	public void FinishAllOngoingSpawns()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RoomMonsters::FinishAllOngoingSpawns()' called when server was not active");
			return;
		}
		foreach (KeyValuePair<SpawnMonsterSettings, Coroutine> os in ongoingSpawns)
		{
			os.Key.onFinish?.Invoke();
			StopCoroutine(os.Value);
		}
		SingletonDewNetworkBehaviour<Room>.instance.monsters.ongoingSpawns.Clear();
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Init()
	{
		_miniBossEffects = null;
	}

	public void SpawnMiniBoss(SpawnMonsterSettings settings, Entity ent = null, StatusEffect se = null)
	{
		SelectMiniBoss(out var entity, out var effect);
		if (ent == null)
		{
			ent = entity;
		}
		if (se == null)
		{
			se = effect;
		}
		if (!(ent is ISpawnableAsMiniBoss))
		{
			Debug.LogWarning("Invalid entity for mini boss spawn provided: " + ent.name);
			return;
		}
		settings.rule = ScriptableObject.CreateInstance<MonsterSpawnRule>();
		settings.rule.isBossSpawn = true;
		settings.rule.pool = ScriptableObject.CreateInstance<MonsterPool>();
		settings.rule.pool.entries = new List<MonsterPool.SpawnRuleEntry>();
		settings.rule.pool.entries.Add(new MonsterPool.SpawnRuleEntry
		{
			monsterRef = ent.ToAssetRef(),
			chance = 1f,
			minCount = 1,
			maxCount = 1
		});
		settings.rule.initialDelay = Vector2.one * 0.5f;
		settings.rule.wavesMax = 1;
		settings.rule.wavesMin = 1;
		settings.rule.spawnMinDistance = 6f;
		settings.rule.spawnMaxDistance = 9f;
		settings.rule.onOverPopulation = OverpopulationBehavior.Ignore;
		settings.rule.populationPerWave = ((Monster)ent).populationCost * 0.55f * Vector2.one;
		settings.rule.waveTimeoutMin = float.PositiveInfinity;
		settings.rule.waveTimeoutMax = float.PositiveInfinity;
		if (settings.spawnPosGetter == null && settings.section != null)
		{
			settings.spawnPosGetter = () => settings.section.pathablePivot;
		}
		SpawnMonsterSettings spawnMonsterSettings = settings;
		spawnMonsterSettings.beforeSpawn = (Action<Entity>)Delegate.Combine(spawnMonsterSettings.beforeSpawn, (Action<Entity>)delegate(Entity e)
		{
			((Monster)e).Networktype = Monster.MonsterType.MiniBoss;
			((ISpawnableAsMiniBoss)e).OnBeforeSpawnAsMiniBoss();
		});
		SpawnMonsterSettings spawnMonsterSettings2 = settings;
		spawnMonsterSettings2.afterSpawn = (Action<Entity>)Delegate.Combine(spawnMonsterSettings2.afterSpawn, (Action<Entity>)delegate(Entity e)
		{
			CallOnCreateAsMiniBoss(e);
			e.CreateStatusEffect(se, e, new CastInfo(e));
			e.EntityEvent_OnDeath += (Action<EventInfoKill>)delegate
			{
				StartCoroutine(Routine());
			};
			IEnumerator Routine()
			{
				Vector3 pos = e.agentPosition;
				yield return new WaitForSeconds(1.5f);
				SingletonDewNetworkBehaviour<Room>.instance.rewards.DropChaosReward(pos, isHighQuality: false);
			}
		});
		SpawnMonsters(settings);
	}

	[ClientRpc]
	private void CallOnCreateAsMiniBoss(Entity e)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(e);
		SendRPCInternal("System.Void RoomMonsters::CallOnCreateAsMiniBoss(Entity)", 1965624741, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	public void SelectMiniBoss(out Entity entity, out MiniBossEffect effect)
	{
		if (_miniBossEffects == null)
		{
			_miniBossEffects = new List<MiniBossEffect>();
			foreach (MiniBossEffect e in DewResources.FindAllByType<MiniBossEffect>())
			{
				_miniBossEffects.Add(e);
			}
		}
		List<Entity> candidates = new List<Entity>();
		foreach (MonsterPool.SpawnRuleEntry e2 in defaultRule.pool.GetFilteredEntries())
		{
			if (e2.monster is ISpawnableAsMiniBoss && !candidates.Contains(e2.monster))
			{
				candidates.Add(e2.monster);
			}
		}
		entity = candidates[global::UnityEngine.Random.Range(0, candidates.Count)];
		effect = _miniBossEffects[global::UnityEngine.Random.Range(0, _miniBossEffects.Count)];
	}

	private void DoEnvSpawnRoomStart()
	{
		if (disableEnvSpawn)
		{
			return;
		}
		_camps = new List<Camp>();
		int campCount = Mathf.RoundToInt(campDensity * base.room.map.mapData.area);
		IBanCampsNearby[] bans = Dew.FindInterfacesOfType<IBanCampsNearby>(includeInactive: false);
		Vector2[] bannedPositions = new Vector2[bans.Length];
		for (int i = 0; i < bans.Length; i++)
		{
			IBanCampsNearby b = bans[i];
			bannedPositions[i] = ((Component)b).transform.position.ToXY();
		}
		for (int j = 0; j < campCount; j++)
		{
			AddCampPos(bannedPositions);
		}
		foreach (Hero allHero in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			allHero.Control.ClientEvent_OnTeleport += new Action<Vector3, Vector3>(ClientEventOnTeleport);
		}
	}

	private void DoEnvSpawnRoomStop()
	{
		if (NetworkedManagerBase<ActorManager>.instance == null)
		{
			return;
		}
		foreach (Hero allHero in NetworkedManagerBase<ActorManager>.instance.allHeroes)
		{
			allHero.Control.ClientEvent_OnTeleport -= new Action<Vector3, Vector3>(ClientEventOnTeleport);
		}
	}

	private void DoEnvSpawnLogicUpdate(float dt)
	{
		if (base.isServer && _camps != null && !(Time.time < _nextEnvSpawnTickTime))
		{
			_nextEnvSpawnTickTime = Time.time + 1f;
			DoCampTickImmediately();
		}
	}

	private void DoCampTickImmediately()
	{
		if (_camps != null)
		{
			for (int i = 0; i < _camps.Count; i++)
			{
				_camps[i].DoCampTick();
			}
		}
	}

	private void ClientEventOnTeleport(Vector3 arg1, Vector3 arg2)
	{
		if (Vector2.Distance(arg1.ToXY(), arg2.ToXY()) > 5f)
		{
			DoCampTickImmediately();
		}
	}

	private void AddCampPos(Vector2[] bannedPositions)
	{
		bool hasMainMod = NetworkedManagerBase<ZoneManager>.instance.currentNode.HasMainModifier();
		List<Vector2> candidates = new List<Vector2>();
		IReadOnlyList<(int, int)> indices = base.room.map.mapData.innerPropNodeIndices;
		for (int i = 0; i < 30; i++)
		{
			Vector2 pos = base.room.map.mapData.cells.GetWorldPos(indices[global::UnityEngine.Random.Range(0, indices.Count)]);
			candidates.Add(pos);
		}
		Vector3 heroSpawnPos = base.room.heroSpawnPos;
		Vector3 finalPos = Dew.SelectBestWithScore(candidates, delegate(Vector2 v, int _)
		{
			if (Vector2.Distance(heroSpawnPos.ToXY(), v) < 25f)
			{
				return float.NegativeInfinity;
			}
			Vector2[] array = bannedPositions;
			for (int j = 0; j < array.Length; j++)
			{
				if (Vector2.Distance(array[j], v) < 3f)
				{
					return float.NegativeInfinity;
				}
			}
			float num = float.PositiveInfinity;
			foreach (Camp current in _camps)
			{
				num = Mathf.Min(num, Vector2.Distance(current.position.ToXY(), v));
			}
			return num;
		}, 0.15f).ToXZ();
		finalPos = Dew.GetPositionOnGround(finalPos);
		finalPos = Dew.GetValidAgentPosition(finalPos);
		if (!hasMainMod || !(Vector2.Distance(finalPos.ToXY(), heroSpawnPos.ToXY()) < 25f))
		{
			_camps.Add(new Camp(finalPos));
		}
	}

	[Server]
	public void RemoveAllCamps()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void RoomMonsters::RemoveAllCamps()' called when server was not active");
		}
		else
		{
			if (_camps == null)
			{
				return;
			}
			foreach (Camp camp in _camps)
			{
				foreach (SpawnEntry e in camp.entries)
				{
					if (!e.instance.IsNullInactiveDeadOrKnockedOut())
					{
						e.instance.Destroy();
					}
				}
			}
			_camps.Clear();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (_camps == null)
		{
			return;
		}
		foreach (Camp camp in _camps)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(camp.position, 1.5f);
		}
	}

	private IEnumerator WaitForPopulationRoutine(MonsterSpawnRule rule, RoomSection section, float requiredPopulation, RefValue<bool> didFail)
	{
		if (requiredPopulation <= 0f || !IsOverPop())
		{
			didFail.value = false;
			yield break;
		}
		switch (rule.onOverPopulation)
		{
		case OverpopulationBehavior.Stall:
		{
			float stallStart = Time.time;
			do
			{
				yield return new WaitForSeconds(global::UnityEngine.Random.Range(0.5f, 1.5f));
				if (Time.time - stallStart > rule.stallCancelTimeout)
				{
					didFail.value = true;
					yield break;
				}
			}
			while (IsOverPop());
			break;
		}
		case OverpopulationBehavior.Cancel:
			didFail.value = true;
			yield break;
		default:
			throw new ArgumentOutOfRangeException();
		case OverpopulationBehavior.Ignore:
			break;
		}
		didFail.value = false;
		bool IsOverPop()
		{
			if (!NetworkedManagerBase<GameManager>.instance.isSpawnOverPopulation)
			{
				if (section != null)
				{
					return section.monsters.isOverPopulation;
				}
				return false;
			}
			return true;
		}
	}

	public void SpawnMonsters(SpawnMonsterSettings settings)
	{
		ongoingSpawns.Add(settings, StartCoroutine(SpawnMonstersRoutine(settings)));
	}

	public UniTask SpawnMonstersAsync(SpawnMonsterSettings settings)
	{
		UniTaskCompletionSource completionSource = new UniTaskCompletionSource();
		settings.onFinish = (Action)Delegate.Combine(settings.onFinish, (Action)delegate
		{
			completionSource.TrySetResult();
		});
		SpawnMonsters(settings);
		return completionSource.Task;
	}

	private IEnumerator SpawnMonstersRoutine(SpawnMonsterSettings s)
	{
		if (ongoingSpawns.ContainsKey(s))
		{
			throw new InvalidOperationException();
		}
		yield return null;
		while (NetworkedManagerBase<ZoneManager>.instance.isInRoomTransition)
		{
			yield return null;
		}
		MonsterSpawnData monsterSpawnData = s.monsterSpawnData;
		MonsterSpawnRule rule = s.rule;
		float waitStartTime = Time.time;
		float timeToWait = global::UnityEngine.Random.Range(rule.initialDelay.x, rule.initialDelay.y) * s.initDelayMultiplier + s.initDelayFlat;
		yield return new WaitWhile(() => Time.time - waitStartTime < timeToWait && !s.isCutsceneSkipped);
		int waves = global::UnityEngine.Random.Range(rule.wavesMin, rule.wavesMax + 1);
		waves = NetworkedManagerBase<GameManager>.instance.GetAdjustedMonsterWaves(waves);
		float hunterChance = addedHunterChance;
		float[,] matrix = NetworkedManagerBase<GameManager>.instance.gss.mirageSkinChanceByZoneIndexAndPlayerCount;
		float mirageChance = matrix[Mathf.Clamp(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex, 0, matrix.GetLength(0) - 1), Mathf.Clamp(Dew.GetAliveHeroCount() - 1, 0, matrix.GetLength(1) - 1)];
		if (mirageChance > 0.0001f)
		{
			mirageChance += addedMirageChance;
		}
		if (rule.isBossSpawn)
		{
			waves = 1;
			hunterChance = 0f;
			mirageChance = 0f;
		}
		int waveIndex = 0;
		while (true)
		{
			float waveStartTime;
			float waveTimeout;
			float nextWaveThreshold;
			if (waveIndex < waves)
			{
				waveStartTime = Time.time;
				waveTimeout = global::UnityEngine.Random.Range(rule.waveTimeoutMin, rule.waveTimeoutMax);
				float population = global::UnityEngine.Random.Range(rule.populationPerWave.x, rule.populationPerWave.y);
				population = NetworkedManagerBase<GameManager>.instance.GetAdjustedMonsterSpawnPopulation(population, s.ignoreTurnPopMultiplier, s.ignoreCoopPopMultiplier) * s.spawnPopulationMultiplier * spawnedPopMultiplier;
				nextWaveThreshold = global::UnityEngine.Random.Range(rule.nextWavePopulationThreshold.x, rule.nextWavePopulationThreshold.y);
				nextWaveThreshold = NetworkedManagerBase<GameManager>.instance.GetAdjustedMonsterSpawnPopulation(nextWaveThreshold, s.ignoreTurnPopMultiplier, s.ignoreCoopPopMultiplier);
				nextWaveThreshold = Mathf.Clamp(nextWaveThreshold, 0.0001f, population - 0.1f);
				IEnumerator<Monster> enumerator = rule.pool.GetMonsters(int.MaxValue);
				enumerator.MoveNext();
				float spawnedPop = 0f;
				bool isFirstSpawn = true;
				while (true)
				{
					if (isFirstSpawn)
					{
						isFirstSpawn = false;
					}
					else
					{
						yield return new WaitForSeconds(global::UnityEngine.Random.Range(0.1f, 0.5f));
					}
					float popCost = enumerator.Current.populationCost;
					if (!rule.isBossSpawn)
					{
						RefValue<bool> didFail = new RefValue<bool>(v: false);
						yield return WaitForPopulationRoutine(rule, s.section, popCost, didFail);
						if ((bool)didFail)
						{
							break;
						}
					}
					if (s.earlyFinishCondition != null && s.earlyFinishCondition())
					{
						goto end_IL_0622;
					}
					Entity spawned = SpawnMonsterImp(s, monsterSpawnData, enumerator.Current, popCost);
					if (spawned != null)
					{
						spawnedPop += popCost;
						if (global::UnityEngine.Random.value < hunterChance && !spawned.Status.HasStatusEffect<Se_HunterBuff>())
						{
							spawned.CreateStatusEffect<Se_HunterBuff>(spawned, new CastInfo(spawned));
						}
						if (global::UnityEngine.Random.value < mirageChance && spawned is Monster { type: not Monster.MonsterType.Lesser } && !spawned.Status.HasStatusEffect<Se_MirageSkin_Delusion>())
						{
							spawned.CreateStatusEffect<Se_MirageSkin_Delusion>(spawned, new CastInfo(spawned));
						}
					}
					if (!rule.isBossSpawn && !(spawnedPop > population))
					{
						enumerator.MoveNext();
						continue;
					}
					goto IL_05e2;
				}
				if (rule.onOverPopulation == OverpopulationBehavior.Stall)
				{
					Debug.Log(rule.name + " timed out due to overpopulation");
				}
				else
				{
					Debug.Log(rule.name + " canceled due to overpopulation");
				}
			}
			while (monsterSpawnData.remainingPopulation > 0.05f)
			{
				yield return new WaitForSeconds(0.25f);
			}
			break;
			IL_05e2:
			while (monsterSpawnData.remainingPopulation > nextWaveThreshold && Time.time - waveStartTime < waveTimeout)
			{
				yield return new WaitForSeconds(0.25f);
			}
			waveIndex++;
			continue;
			end_IL_0622:
			break;
		}
		s.onFinish?.Invoke();
		ongoingSpawns.Remove(s);
	}

	internal (Vector3, Quaternion) GetSpawnMonsterPosRot(SpawnMonsterSettings s, Entity monster)
	{
		Vector3 spawnPos = ((!(monster is Monster { spawnPosOverride: not null, spawnPosOverride: var spawnPosOverride })) ? ((s.spawnPosGetter != null) ? s.spawnPosGetter() : ((!(s.section != null)) ? GetSpawnPositionNearPlayer(s.rule.spawnMinDistance, s.rule.spawnMaxDistance, s.hero) : s.section.monsters.GetSpawnPositionInSection(s.rule.spawnMinDistance, s.rule.spawnMaxDistance))) : spawnPosOverride.Value);
		Quaternion spawnRot;
		if (monster is Monster { spawnRotOverride: not null, spawnRotOverride: var spawnRotOverride })
		{
			spawnRot = spawnRotOverride.Value;
		}
		else if (s.spawnRotGetter != null)
		{
			spawnRot = s.spawnRotGetter();
		}
		else
		{
			Vector3 closestPosition = NetworkedManagerBase<ActorManager>.instance.allHeroes[0].position;
			float minDistance = Vector3.Distance(spawnPos, NetworkedManagerBase<ActorManager>.instance.allHeroes[0].position);
			for (int i = 1; i < NetworkedManagerBase<ActorManager>.instance.allHeroes.Count; i++)
			{
				Vector3 p = NetworkedManagerBase<ActorManager>.instance.allHeroes[i].position;
				float d = Vector3.Distance(spawnPos, p);
				if (!(d >= minDistance))
				{
					minDistance = d;
					closestPosition = p;
				}
			}
			spawnRot = Quaternion.LookRotation(closestPosition - spawnPos).Flattened();
		}
		return (spawnPos, spawnRot);
	}

	private Entity SpawnMonsterImp(SpawnMonsterSettings s, MonsterSpawnData monsterSpawnData, Entity monster, float popCost)
	{
		try
		{
			(Vector3, Quaternion) spawnPosRot = GetSpawnMonsterPosRot(s, monster);
			Entity ent = Dew.SpawnEntity(monster, spawnPosRot.Item1, spawnPosRot.Item2, NetworkedManagerBase<ActorManager>.instance.serverActor, DewPlayer.creep, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Entity e)
			{
				if (s.isCutsceneSkipped)
				{
					e.Visual.NetworkskipSpawning = true;
				}
				onBeforeSpawn?.Invoke(e);
				s.beforeSpawn?.Invoke(e);
				monsterSpawnData.remainingPopulation += popCost;
				e.EntityEvent_OnDeath += (Action<EventInfoKill>)delegate(EventInfoKill kill)
				{
					monsterSpawnData.remainingPopulation -= popCost;
					monsterSpawnData.lastKiller = kill.actor.firstEntity;
					monsterSpawnData.lastDeathPosition = kill.victim.agentPosition;
				};
			});
			onAfterSpawn?.Invoke(ent);
			s.afterSpawn?.Invoke(ent);
			Hero closest = null;
			float closestSqr = float.PositiveInfinity;
			foreach (DewPlayer p in DewPlayer.humanPlayers)
			{
				if (!(p.hero == null) && !p.hero.isKnockedOut)
				{
					float sqr = Vector2.SqrMagnitude(p.hero.position.ToXY() - spawnPosRot.Item1.ToXY());
					if (sqr < closestSqr)
					{
						closestSqr = sqr;
						closest = p.hero;
					}
				}
			}
			if (closest != null)
			{
				ent.Control.MoveToDestination(closest.position, immediately: false);
			}
			return ent;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return null;
		}
	}

	private Vector3 GetSpawnPositionNearPlayer(float minDist, float maxDist, Hero target = null)
	{
		if (target == null)
		{
			target = Dew.SelectRandomAliveHero();
		}
		for (int tries = 0; tries < 30; tries++)
		{
			Vector3 worldPos = target.agentPosition + global::UnityEngine.Random.insideUnitSphere.Flattened().normalized * global::UnityEngine.Random.Range(minDist, maxDist);
			if (tries < 29)
			{
				if (tries < 10)
				{
					bool tooClose = false;
					bool tooFar = true;
					foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
					{
						float dist = Vector2.Distance(humanPlayer.hero.agentPosition.ToXY(), worldPos.ToXY());
						if (dist < minDist)
						{
							tooClose = true;
							break;
						}
						if (dist < maxDist)
						{
							tooFar = false;
						}
					}
					if (tooClose || tooFar)
					{
						continue;
					}
				}
				if (FilterSpawnPosition(worldPos, out var filtered, target.agentPosition))
				{
					return filtered;
				}
				continue;
			}
			Debug.LogWarning("Using fallback spawn position for room '" + SceneManager.GetActiveScene().name + "'");
			FilterSpawnPosition(worldPos, out var fallback, target.agentPosition);
			return fallback;
		}
		throw new InvalidOperationException("");
	}

	internal static bool FilterSpawnPosition(Vector3 pos, out Vector3 filteredPos, Vector3? pathPivot)
	{
		if (_tempPath == null)
		{
			_tempPath = new NavMeshPath();
		}
		if (!Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out var rayHit, 100f, LayerMasks.Ground))
		{
			filteredPos = pos;
			return false;
		}
		if (!NavMesh.SamplePosition(rayHit.point, out var navHit, 5f, -1))
		{
			filteredPos = rayHit.point;
			return false;
		}
		if ((pathPivot.HasValue && !NavMesh.CalculatePath(pathPivot.Value, navHit.position, -1, _tempPath)) || _tempPath.status != 0)
		{
			filteredPos = navHit.position;
			return false;
		}
		filteredPos = navHit.position;
		return true;
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_CallOnCreateAsMiniBoss__Entity(Entity e)
	{
		((ISpawnableAsMiniBoss)e).OnCreateAsMiniBoss();
	}

	protected static void InvokeUserCode_CallOnCreateAsMiniBoss__Entity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC CallOnCreateAsMiniBoss called on server.");
		}
		else
		{
			((RoomMonsters)obj).UserCode_CallOnCreateAsMiniBoss__Entity(reader.ReadNetworkBehaviour<Entity>());
		}
	}

	static RoomMonsters()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(RoomMonsters), "System.Void RoomMonsters::CallOnCreateAsMiniBoss(Entity)", InvokeUserCode_CallOnCreateAsMiniBoss__Entity);
	}
}
