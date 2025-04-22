using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZoneManager : NetworkedManagerBase<ZoneManager>
{
	public SafeAction<EventInfoLoadRoom> ClientEvent_OnRoomLoaded;

	public SafeAction ClientEvent_OnLoopStarted;

	public SafeAction ClientEvent_OnZoneLoaded;

	public SafeAction ClientEvent_OnZoneLoadStarted;

	public SafeAction<EventInfoLoadRoom> ClientEvent_OnRoomLoadStarted;

	public SafeAction<bool> ClientEvent_OnIsInTransitionChanged;

	[CompilerGenerated]
	[SyncVar]
	private Zone _003CcurrentZone_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar]
	private Room _003CcurrentRoom_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar]
	private int _003CcurrentZoneIndex_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar(hook = "OnCurrentRoomIndexChanged")]
	private int _003CcurrentRoomIndex_003Ek__BackingField;

	public SafeAction ClientEvent_OnCurrentRoomIndexChanged;

	[CompilerGenerated]
	[SyncVar(hook = "OnClearedCombatRoomsChanged")]
	private int _003CclearedCombatRooms_003Ek__BackingField;

	public SafeAction ClientEvent_OnClearedCombatRoomsChanged;

	[CompilerGenerated]
	[SyncVar]
	private int _003CcurrentZoneClearedNodes_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar(hook = "OnIsInTransitionChanged")]
	private bool _003CisInRoomTransition_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar(hook = "OnCurrentHuntLevelChanged")]
	private int _003CcurrentHuntLevel_003Ek__BackingField;

	public SafeAction ClientEvent_OnCurrentHuntLevelChanged;

	internal readonly List<string> _bannedSidetracksForCurrentLoop = new List<string>();

	internal readonly List<string> _bannedRoomModifiersForCurrentLoop = new List<string>();

	private List<Func<EventInfoTravelToNodeInterrupt, bool>> _travelToNodeInterrupts = new List<Func<EventInfoTravelToNodeInterrupt, bool>>();

	[NonSerialized]
	public bool? forceVoteForDebug;

	public SafeAction<DewPlayer> ClientEvent_OnVoteStarted;

	public SafeAction<DewPlayer> ClientEvent_OnVoteCanceled;

	public SafeAction ClientEvent_OnVoteCompleted;

	[CompilerGenerated]
	[SyncVar]
	private bool _003CisVoting_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar]
	private int _003CvoteRemainingSeconds_003Ek__BackingField;

	[SyncVar]
	public VoteType voteType;

	[CompilerGenerated]
	[SyncVar]
	private int _003CvoteData_003Ek__BackingField;

	private Coroutine _voteRoutine;

	private Coroutine _voteCheckRoutine;

	private Coroutine _voteCompleteRoutine;

	public static bool StartOnRandomNode;

	public static float WorldHeight;

	public static float WorldWidth;

	public static float AdjacentThreshold;

	public static float MinDistanceBetweenNode;

	public static float MaxDistanceToNearestNode;

	public static float[] ScorePerNodeConnection;

	public static float ScorePerIsolatedNode;

	public static float ScorePerIsolatedImportantNode;

	public static float ScoreFuzziness;

	public static int WorldGenerationIterations;

	public static int NodePlacementTries;

	public static int HunterStartSkippedTurns;

	private const int NodeDistInfinity = 10000;

	[CompilerGenerated]
	[SyncVar]
	private int _003CcurrentNodeIndex_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar]
	private int _003CsidetrackReturnNodeIndex_003Ek__BackingField;

	public readonly SyncList<WorldNodeData> nodes = new SyncList<WorldNodeData>();

	public readonly SyncList<HunterStatus> hunterStatuses = new SyncList<HunterStatus>();

	public SafeAction ClientEvent_OnNodesChanged;

	public readonly SyncList<int> nodeDistanceMatrix = new SyncList<int>();

	public List<WorldNodeSaveData> visitedNodesSaveData;

	private List<string> _combatRoomPool = new List<string>();

	private List<string> _shopRoomPool = new List<string>();

	internal Dictionary<string, List<int>> _usedAngleIndexes = new Dictionary<string, List<int>>();

	[CompilerGenerated]
	[SyncVar]
	private bool _003CisHuntAdvanceDisabled_003Ek__BackingField;

	[CompilerGenerated]
	[SyncVar]
	private int _003ChunterStartNodeIndex_003Ek__BackingField;

	private bool _needToCallNodesChanged;

	private int _visitedCombatNodesWithoutMiniBoss;

	internal List<RoomRewardFlowItemType> _nextRewards;

	private float _hunterSpreadCredit;

	internal int _nextModifierId = 1;

	public Dictionary<int, Dictionary<string, object>> modifierServerData = new Dictionary<int, Dictionary<string, object>>();

	[CompilerGenerated]
	[SyncVar]
	private int _003CloopIndex_003Ek__BackingField;

	private int _currentTier = -1;

	private List<Zone> _remainingZonesOfCurrentTier = new List<Zone>();

	private Zone[] _zonesInGame;

	protected NetworkBehaviourSyncVar ____003CcurrentRoom_003Ek__BackingFieldNetId;

	public Zone currentZone
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentZone_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CcurrentZone_003Ek__BackingField = value;
		}
	}

	public Room currentRoom
	{
		[CompilerGenerated]
		get
		{
			return Network_003CcurrentRoom_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CcurrentRoom_003Ek__BackingField = value;
		}
	}

	public int currentZoneIndex
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentZoneIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CcurrentZoneIndex_003Ek__BackingField = value;
		}
	}

	public int currentRoomIndex
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentRoomIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CcurrentRoomIndex_003Ek__BackingField = value;
		}
	}

	public int clearedCombatRooms
	{
		[CompilerGenerated]
		get
		{
			return _003CclearedCombatRooms_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CclearedCombatRooms_003Ek__BackingField = value;
		}
	}

	public int currentZoneClearedNodes
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentZoneClearedNodes_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CcurrentZoneClearedNodes_003Ek__BackingField = value;
		}
	}

	public bool isInAnyTransition
	{
		get
		{
			if (!isInLocalTransition)
			{
				return isInRoomTransition;
			}
			return true;
		}
	}

	public bool isInLocalTransition { get; internal set; }

	public bool isInRoomTransition
	{
		[CompilerGenerated]
		get
		{
			return _003CisInRoomTransition_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CisInRoomTransition_003Ek__BackingField = value;
		}
	}

	public int currentHuntLevel
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentHuntLevel_003Ek__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			Network_003CcurrentHuntLevel_003Ek__BackingField = value;
		}
	}

	public bool isVoting
	{
		[CompilerGenerated]
		get
		{
			return _003CisVoting_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CisVoting_003Ek__BackingField = value;
		}
	}

	public int voteRemainingSeconds
	{
		[CompilerGenerated]
		get
		{
			return _003CvoteRemainingSeconds_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CvoteRemainingSeconds_003Ek__BackingField = value;
		}
	} = -1;

	public int voteData
	{
		[CompilerGenerated]
		get
		{
			return _003CvoteData_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CvoteData_003Ek__BackingField = value;
		}
	} = -1;

	public int currentNodeIndex
	{
		[CompilerGenerated]
		get
		{
			return _003CcurrentNodeIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CcurrentNodeIndex_003Ek__BackingField = value;
		}
	} = -1;

	public bool isSidetracking => sidetrackReturnNodeIndex >= 0;

	public int sidetrackReturnNodeIndex
	{
		[CompilerGenerated]
		get
		{
			return _003CsidetrackReturnNodeIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CsidetrackReturnNodeIndex_003Ek__BackingField = value;
		}
	} = -1;

	public WorldNodeData currentNode
	{
		get
		{
			return nodes[currentNodeIndex];
		}
		set
		{
			nodes[currentNodeIndex] = value;
		}
	}

	public HunterStatus currentNodeHunterStatus => hunterStatuses[currentNodeIndex];

	public bool isCurrentNodeHunted => hunterStatuses[currentNodeIndex] >= HunterStatus.Level1;

	public int currentTurnIndex { get; private set; }

	public bool isHuntAdvanceDisabled
	{
		[CompilerGenerated]
		get
		{
			return _003CisHuntAdvanceDisabled_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003CisHuntAdvanceDisabled_003Ek__BackingField = value;
		}
	}

	public int hunterStartNodeIndex
	{
		[CompilerGenerated]
		get
		{
			return _003ChunterStartNodeIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		set
		{
			Network_003ChunterStartNodeIndex_003Ek__BackingField = value;
		}
	}

	public int loopIndex
	{
		[CompilerGenerated]
		get
		{
			return _003CloopIndex_003Ek__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			Network_003CloopIndex_003Ek__BackingField = value;
		}
	}

	public Zone Network_003CcurrentZone_003Ek__BackingField
	{
		get
		{
			return currentZone;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentZone, 1uL, null);
		}
	}

	public Room Network_003CcurrentRoom_003Ek__BackingField
	{
		get
		{
			return GetSyncVarNetworkBehaviour(____003CcurrentRoom_003Ek__BackingFieldNetId, ref currentRoom);
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter_NetworkBehaviour(value, ref currentRoom, 2uL, null, ref ____003CcurrentRoom_003Ek__BackingFieldNetId);
		}
	}

	public int Network_003CcurrentZoneIndex_003Ek__BackingField
	{
		get
		{
			return currentZoneIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentZoneIndex, 4uL, null);
		}
	}

	public int Network_003CcurrentRoomIndex_003Ek__BackingField
	{
		get
		{
			return currentRoomIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentRoomIndex, 8uL, OnCurrentRoomIndexChanged);
		}
	}

	public int Network_003CclearedCombatRooms_003Ek__BackingField
	{
		get
		{
			return clearedCombatRooms;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref clearedCombatRooms, 16uL, OnClearedCombatRoomsChanged);
		}
	}

	public int Network_003CcurrentZoneClearedNodes_003Ek__BackingField
	{
		get
		{
			return currentZoneClearedNodes;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentZoneClearedNodes, 32uL, null);
		}
	}

	public bool Network_003CisInRoomTransition_003Ek__BackingField
	{
		get
		{
			return isInRoomTransition;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isInRoomTransition, 64uL, OnIsInTransitionChanged);
		}
	}

	public int Network_003CcurrentHuntLevel_003Ek__BackingField
	{
		get
		{
			return currentHuntLevel;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentHuntLevel, 128uL, OnCurrentHuntLevelChanged);
		}
	}

	public bool Network_003CisVoting_003Ek__BackingField
	{
		get
		{
			return isVoting;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isVoting, 256uL, null);
		}
	}

	public int Network_003CvoteRemainingSeconds_003Ek__BackingField
	{
		get
		{
			return voteRemainingSeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref voteRemainingSeconds, 512uL, null);
		}
	}

	public VoteType NetworkvoteType
	{
		get
		{
			return voteType;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref voteType, 1024uL, null);
		}
	}

	public int Network_003CvoteData_003Ek__BackingField
	{
		get
		{
			return voteData;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref voteData, 2048uL, null);
		}
	}

	public int Network_003CcurrentNodeIndex_003Ek__BackingField
	{
		get
		{
			return currentNodeIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref currentNodeIndex, 4096uL, null);
		}
	}

	public int Network_003CsidetrackReturnNodeIndex_003Ek__BackingField
	{
		get
		{
			return sidetrackReturnNodeIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref sidetrackReturnNodeIndex, 8192uL, null);
		}
	}

	public bool Network_003CisHuntAdvanceDisabled_003Ek__BackingField
	{
		get
		{
			return isHuntAdvanceDisabled;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref isHuntAdvanceDisabled, 16384uL, null);
		}
	}

	public int Network_003ChunterStartNodeIndex_003Ek__BackingField
	{
		get
		{
			return hunterStartNodeIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref hunterStartNodeIndex, 32768uL, null);
		}
	}

	public int Network_003CloopIndex_003Ek__BackingField
	{
		get
		{
			return loopIndex;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref loopIndex, 65536uL, null);
		}
	}

	private void OnCurrentRoomIndexChanged(int _, int __)
	{
		ClientEvent_OnCurrentRoomIndexChanged?.Invoke();
	}

	private void OnClearedCombatRoomsChanged(int _, int __)
	{
		ClientEvent_OnClearedCombatRoomsChanged?.Invoke();
	}

	private void OnIsInTransitionChanged(bool oldVal, bool newVal)
	{
		try
		{
			ClientEvent_OnIsInTransitionChanged?.Invoke(newVal);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception, this);
		}
	}

	private void OnCurrentHuntLevelChanged(int _, int __)
	{
		ClientEvent_OnCurrentHuntLevelChanged?.Invoke();
	}

	public override void OnStart()
	{
		base.OnStart();
		DewResources.AddPreloadRule(this, delegate(PreloadInterface preload)
		{
			foreach (WorldNodeData node in nodes)
			{
				foreach (ModifierData modifier in node.modifiers)
				{
					preload.AddType(modifier.type);
				}
			}
			if (currentZone != null && currentZone.defaultMonsters != null)
			{
				preload.AddFromMonsterPool(currentZone.defaultMonsters.pool);
			}
		});
	}

	private void OnLoopStart()
	{
		Debug.Log("New loop has started");
		_bannedSidetracksForCurrentLoop.Clear();
		_bannedRoomModifiersForCurrentLoop.Clear();
		InvokeOnLoopStarted();
	}

	public List<RoomModifierBase> LoadModifierPrefabsOfCurrentZone()
	{
		List<RoomModifierBase> list = new List<RoomModifierBase>();
		foreach (RoomModifierBase modPrefab in DewResources.FindAllByTypeSubstring<RoomModifierBase>("RoomMod_"))
		{
			if (Dew.IsRoomModifierIncludedInGame(modPrefab.GetType().Name) && (modPrefab.allowedZones == null || modPrefab.allowedZones.Length == 0 || modPrefab.allowedZones.Contains(currentZone.name)) && modPrefab.IsAvailableInGame() && currentZoneIndex >= modPrefab.zoneIndexRange.x && currentZoneIndex <= modPrefab.zoneIndexRange.y && !_bannedRoomModifiersForCurrentLoop.Contains(modPrefab.GetType().Name))
			{
				list.Add(modPrefab);
			}
		}
		return list;
	}

	[ClientRpc]
	private void InvokeOnRoomLoadStarted(EventInfoLoadRoom info)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_EventInfoLoadRoom(writer, info);
		SendRPCInternal("System.Void ZoneManager::InvokeOnRoomLoadStarted(EventInfoLoadRoom)", 514859420, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void InvokeOnZoneLoadStarted()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void ZoneManager::InvokeOnZoneLoadStarted()", 1722999772, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void InvokeOnRoomLoaded(EventInfoLoadRoom info)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_EventInfoLoadRoom(writer, info);
		SendRPCInternal("System.Void ZoneManager::InvokeOnRoomLoaded(EventInfoLoadRoom)", 2041766036, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void InvokeOnZoneLoaded()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void ZoneManager::InvokeOnZoneLoaded()", 1242595046, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void InvokeOnLoopStarted()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void ZoneManager::InvokeOnLoopStarted()", 786369066, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	public void AddTravelToNodeInterrupt(Func<EventInfoTravelToNodeInterrupt, bool> func)
	{
		_travelToNodeInterrupts.Add(func);
	}

	public void RemoveTravelToNodeInterrupt(Func<EventInfoTravelToNodeInterrupt, bool> func)
	{
		_travelToNodeInterrupts.Remove(func);
	}

	private bool CheckTravelToNodeInterrupts(EventInfoTravelToNodeInterrupt info)
	{
		Func<EventInfoTravelToNodeInterrupt, bool>[] array = _travelToNodeInterrupts.ToArray();
		foreach (Func<EventInfoTravelToNodeInterrupt, bool> i2 in array)
		{
			try
			{
				if (i2(info))
				{
					Debug.Log("Travel has been interrupt");
					return true;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		return false;
	}

	public void CallOnReadyAfterTransition(Action action)
	{
		bool wasCalledInTransition = isInAnyTransition;
		GameManager.CallOnReady(delegate
		{
			StartCoroutine(Routine());
		});
		IEnumerator Routine()
		{
			if (wasCalledInTransition)
			{
				yield return new WaitForSeconds(global::UnityEngine.Random.Range(0.85f, 1.3f));
			}
			try
			{
				action?.Invoke();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	[Server]
	public void LoadZone(Zone prefab, bool incrementParameters = true)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::LoadZone(Zone,System.Boolean)' called when server was not active");
			return;
		}
		LoadNode(new LoadNodeSettings
		{
			from = -1,
			to = 0,
			advanceTurn = true,
			dontIncrementNewZoneParameters = !incrementParameters,
			newZone = prefab
		});
	}

	[Server]
	public void ReturnFromSidetracking()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::ReturnFromSidetracking()' called when server was not active");
			return;
		}
		if (sidetrackReturnNodeIndex < 0)
		{
			throw new InvalidOperationException();
		}
		TravelToNode(sidetrackReturnNodeIndex, advanceTurn: false, isSidetrackTransition: true, ignoreInterrupts: true);
		Network_003CsidetrackReturnNodeIndex_003Ek__BackingField = -1;
	}

	[Server]
	public void LoadSidetrackRoom(string roomName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::LoadSidetrackRoom(System.String)' called when server was not active");
			return;
		}
		if (sidetrackReturnNodeIndex >= 0)
		{
			throw new InvalidOperationException();
		}
		int from = currentNodeIndex;
		int to = nodes.Count;
		AddSidetrackNode(roomName);
		Network_003CsidetrackReturnNodeIndex_003Ek__BackingField = from;
		TravelToNode(to, advanceTurn: false, isSidetrackTransition: true, ignoreInterrupts: true);
	}

	[Server]
	public void TravelToNode(int to, bool advanceTurn = true, bool isSidetrackTransition = false, bool ignoreInterrupts = false)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::TravelToNode(System.Int32,System.Boolean,System.Boolean,System.Boolean)' called when server was not active");
		}
		else if (ignoreInterrupts || !CheckTravelToNodeInterrupts(new EventInfoTravelToNodeInterrupt
		{
			from = currentNodeIndex,
			to = to,
			newZone = null
		}))
		{
			LoadNode(new LoadNodeSettings
			{
				from = currentNodeIndex,
				to = to,
				advanceTurn = advanceTurn,
				isSidetrackTransition = isSidetrackTransition
			});
		}
	}

	[Server]
	public void DoDeadEndTravel()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::DoDeadEndTravel()' called when server was not active");
		}
		else
		{
			StartCoroutine(Routine());
		}
		static IEnumerator Routine()
		{
			NetworkedManagerBase<GameManager>.instance.isGameTimePaused = true;
			for (int i = 0; i < NetworkedManagerBase<ActorManager>.instance.allHeroes.Count; i++)
			{
				Hero h = NetworkedManagerBase<ActorManager>.instance.allHeroes[i];
				if (!h.IsNullInactiveDeadOrKnockedOut())
				{
					yield return new WaitForSeconds((i == 0) ? 0.25f : 0.15f);
					if (!h.IsNullInactiveDeadOrKnockedOut())
					{
						h.CreateStatusEffect<Se_PortalTransition>(h, default(CastInfo));
					}
				}
			}
			yield return new WaitForSeconds(0.45f);
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Game_RiftTransition");
		}
	}

	[Server]
	public void LoadNode(LoadNodeSettings s)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::LoadNode(LoadNodeSettings)' called when server was not active");
		}
		else
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			if (isInRoomTransition)
			{
				throw new InvalidOperationException("Already in transition");
			}
			bool isFirstTime = Network_003CcurrentRoom_003Ek__BackingField == null;
			bool isSidetrackTransition = s.isSidetrackTransition;
			NetworkedManagerBase<GameManager>.instance.isGameTimePaused = true;
			Network_003CisInRoomTransition_003Ek__BackingField = true;
			if (s.advanceTurn)
			{
				int num = currentRoomIndex;
				Network_003CcurrentRoomIndex_003Ek__BackingField = num + 1;
			}
			if (s.from >= 0 && nodes[s.from].type == WorldNodeType.Combat)
			{
				int num = clearedCombatRooms;
				Network_003CclearedCombatRooms_003Ek__BackingField = num + 1;
			}
			Hero[] heroes = NetworkedManagerBase<ActorManager>.instance.allHeroes.ToArray();
			if (isFirstTime)
			{
				yield return new WaitForSecondsRealtime(0.5f);
				for (int i = 0; i < heroes.Length; i++)
				{
					Hero h = heroes[i];
					yield return new WaitForSeconds((i == 0) ? 0.75f : 0.3f);
					if (!h.IsNullOrInactive())
					{
						h.CreateStatusEffect(h, default(CastInfo), delegate(Se_PortalTransition p)
						{
							p.playDisappearEffect = false;
						});
					}
				}
				yield return new WaitForSecondsRealtime(1f);
			}
			else
			{
				if (!isSidetrackTransition)
				{
					for (int i = 0; i < heroes.Length; i++)
					{
						Hero h = heroes[i];
						if (!h.IsNullInactiveDeadOrKnockedOut())
						{
							yield return new WaitForSeconds((i == 0) ? 0.25f : 0.15f);
							if (!h.IsNullInactiveDeadOrKnockedOut())
							{
								Summon[] array = h.summons.ToArray();
								foreach (Summon sum in array)
								{
									if (!sum.IsNullInactiveDeadOrKnockedOut())
									{
										sum.CreateStatusEffect<Se_PortalTransition>(sum, default(CastInfo));
										yield return new WaitForSeconds(Mathf.Min(0.5f / (float)h.summons.Count, 0.3f));
									}
								}
								h.CreateStatusEffect<Se_PortalTransition>(h, default(CastInfo));
							}
						}
					}
					yield return new WaitForSeconds(0.45f);
				}
				ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Game_RiftTransition");
				DewNetworkManager.instance.SetLoadingStatus(isLoading: true);
				yield return new WaitForSecondsRealtime(ManagerBase<TransitionManager>.instance.fadeTime);
			}
			try
			{
				if (SingletonDewNetworkBehaviour<Room>.instance != null)
				{
					if (s.from >= 0 && visitedNodesSaveData[s.from] == null)
					{
						int num = currentZoneClearedNodes;
						Network_003CcurrentZoneClearedNodes_003Ek__BackingField = num + 1;
					}
					WorldNodeSaveData newSave = new WorldNodeSaveData();
					SingletonDewNetworkBehaviour<Room>.instance.StopRoom(newSave);
					foreach (DewPlayer h2 in DewPlayer.humanPlayers)
					{
						newSave.heroPositions.Add(h2.hero, (h2.hero.position, h2.hero.rotation));
					}
					if (s.from >= 0)
					{
						visitedNodesSaveData[s.from] = newSave;
					}
				}
				if (s.newZone != null)
				{
					InvokeOnZoneLoadStarted();
				}
				InvokeOnRoomLoadStarted(new EventInfoLoadRoom
				{
					fromIndex = s.from,
					toIndex = s.to,
					isSidetrackTransition = s.isSidetrackTransition
				});
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			yield return new WaitForSecondsRealtime(0.1f);
			if (s.newZone != null)
			{
				yield return new WaitForSecondsRealtime(0.2f);
			}
			try
			{
				Hero[] array2 = heroes;
				foreach (Hero h3 in array2)
				{
					if (!h3.IsNullOrInactive())
					{
						h3.Control.Teleport(new Vector3(-5000f, -5000f, 0f));
					}
				}
				foreach (Actor actor in new List<Actor>(NetworkedManagerBase<ActorManager>.instance.allActors))
				{
					if (actor.isDestroyedOnRoomChange && actor.isActive)
					{
						actor.Destroy();
					}
				}
			}
			catch (Exception exception2)
			{
				Debug.LogException(exception2);
			}
			try
			{
				if (s.newZone != null)
				{
					if (currentZone == null)
					{
						Network_003CcurrentRoomIndex_003Ek__BackingField = 0;
						Network_003CcurrentZoneIndex_003Ek__BackingField = -1;
						OnLoopStart();
					}
					Network_003CcurrentZoneClearedNodes_003Ek__BackingField = 0;
					_hunterSpreadCredit = 0f;
					Network_003CcurrentZone_003Ek__BackingField = s.newZone;
					if (!s.dontIncrementNewZoneParameters)
					{
						int num = currentZoneIndex;
						Network_003CcurrentZoneIndex_003Ek__BackingField = num + 1;
					}
					NetworkedManagerBase<GameManager>.instance.SetAmbientLevel(currentZoneIndex + 1);
					foreach (DewPlayer h4 in DewPlayer.humanPlayers)
					{
						if (h4.hero.isKnockedOut && h4.hero.Status.TryGetStatusEffect<Se_HeroKnockedOut>(out var eff))
						{
							eff.Destroy();
						}
					}
					GenerateWorld();
				}
			}
			catch (Exception exception3)
			{
				Debug.LogException(exception3);
			}
			try
			{
				if (nodes[s.to].room == null)
				{
					if (nodes[s.to].type == WorldNodeType.Merchant)
					{
						if (_shopRoomPool.Count == 0)
						{
							_shopRoomPool.AddRange(currentZone.shopRooms);
						}
						int i2 = global::UnityEngine.Random.Range(0, _shopRoomPool.Count);
						SetRoom(s.to, _shopRoomPool[i2]);
						_shopRoomPool.RemoveAt(i2);
					}
					else
					{
						if (_combatRoomPool.Count == 0)
						{
							_combatRoomPool.AddRange(currentZone.combatRooms);
						}
						int i3 = global::UnityEngine.Random.Range(0, _combatRoomPool.Count);
						SetRoom(s.to, _combatRoomPool[i3]);
						_combatRoomPool.RemoveAt(i3);
					}
				}
				if (nodes[s.to].type == WorldNodeType.Combat && !nodes[s.to].HasModifier<RoomMod_SpawnMiniBoss>())
				{
					float chance = NetworkedManagerBase<GameManager>.instance.gss.miniBossSpawnChanceByNonMiniBossCombatNodesVisited.Evaluate(_visitedCombatNodesWithoutMiniBoss);
					if (!nodes[s.to].HasMainModifier() && global::UnityEngine.Random.value < chance)
					{
						_visitedCombatNodesWithoutMiniBoss = 0;
						AddModifier<RoomMod_SpawnMiniBoss>(s.to);
					}
					else
					{
						_visitedCombatNodesWithoutMiniBoss++;
					}
				}
			}
			catch (Exception exception4)
			{
				Debug.LogException(exception4);
			}
			yield return null;
			try
			{
				if (s.advanceTurn)
				{
					int num = currentTurnIndex;
					currentTurnIndex = num + 1;
					AdvanceHunterTurn();
				}
				if (!isSidetrackTransition)
				{
					UpdateModifiersByHunterStatus(s.to);
				}
			}
			catch (Exception exception5)
			{
				Debug.LogException(exception5);
			}
			try
			{
				SetCurrentNodeIndexAndRevealAdjacent(s.to);
			}
			catch (Exception exception6)
			{
				Debug.LogException(exception6);
			}
			yield return DewNetworkManager.instance.LoadSceneAsync(nodes[s.to].room);
			Room newRoom = global::UnityEngine.Object.FindObjectOfType<Room>(includeInactive: true);
			if (newRoom == null)
			{
				throw new Exception("Could not find room in loaded scene: " + SceneManager.GetActiveScene().name);
			}
			Network_003CcurrentRoom_003Ek__BackingField = newRoom;
			if (isCurrentNodeHunted)
			{
				int num = currentHuntLevel;
				Network_003CcurrentHuntLevel_003Ek__BackingField = num + 1;
			}
			yield return new WaitUntil(() => NetworkClient.ready);
			Debug.Log("TravelToNode: Start waiting for clients");
			yield return Dew.WaitForClientsReadyRoutine();
			Debug.Log("TravelToNode: All clients are ready");
			try
			{
				WorldNodeSaveData saveData = ((s.to >= 0) ? visitedNodesSaveData[s.to] : null);
				SingletonDewNetworkBehaviour<Room>.instance.StartRoom(saveData);
				InvokeOnRoomLoaded(new EventInfoLoadRoom
				{
					fromIndex = s.from,
					toIndex = s.to,
					isSidetrackTransition = s.isSidetrackTransition
				});
				Transform riftTransform = ((Rift_RoomExit.instance != null) ? Rift_RoomExit.instance.transform : Rift.instance.transform);
				Vector3 visitedBasePos = Dew.GetValidAgentDestination_Closest(Dew.GetValidAgentPosition(riftTransform.position), riftTransform.position + riftTransform.forward * 3.5f);
				Hero[] array2 = heroes;
				foreach (Hero h5 in array2)
				{
					if (!h5.IsNullOrInactive() && !h5.isKnockedOut)
					{
						if (saveData != null)
						{
							Vector3 pos = Dew.GetValidAgentDestination_Closest(visitedBasePos, Dew.GetPositionOnGround(visitedBasePos + global::UnityEngine.Random.onUnitSphere.Flattened() * 3f));
							h5.Control.Teleport(pos);
						}
						else
						{
							h5.Control.Teleport(newRoom.GetHeroSpawnPosition());
						}
						foreach (Summon summon in h5.summons)
						{
							summon.Control.Teleport(Dew.GetValidAgentDestination_LinearSweep(h5.agentPosition, Dew.GetPositionOnGround(h5.agentPosition + global::UnityEngine.Random.onUnitSphere.Flattened() * 3f)));
						}
					}
				}
				SingletonDewNetworkBehaviour<Room>.instance.SyncCameraAngle();
			}
			catch (Exception exception7)
			{
				Debug.LogException(exception7);
			}
			yield return new WaitForSeconds(0.15f);
			DewNetworkManager.instance.SetLoadingStatus(isLoading: false);
			if (isFirstTime)
			{
				yield return new WaitForSeconds(0.45f);
			}
			yield return new WaitForSeconds(ManagerBase<TransitionManager>.instance.fadeTime - 0.15f);
			for (int i = 0; i < heroes.Length; i++)
			{
				Hero h = heroes[i];
				if (!h.IsNullOrInactive())
				{
					Summon[] array = h.summons.ToArray();
					foreach (Summon sum2 in array)
					{
						if (!sum2.IsNullInactiveDeadOrKnockedOut() && sum2.Status.TryGetStatusEffect<Se_PortalTransition>(out var transition0))
						{
							transition0.Destroy();
							h.CreateBasicEffect(h, new InvisibleEffect(), 1f, "SpawnInInvisibility");
							yield return new WaitForSeconds(Mathf.Min(0.4f / (float)h.summons.Count, 0.25f));
						}
					}
					if (h.Status.TryGetStatusEffect<Se_PortalTransition>(out var transition1))
					{
						transition1.Destroy();
						h.CreateBasicEffect(h, new InvisibleEffect(), 1f, "SpawnInInvisibility");
						if (i != heroes.Length - 1)
						{
							yield return new WaitForSeconds(0.15f);
						}
					}
				}
			}
			Network_003CisInRoomTransition_003Ek__BackingField = false;
			NetworkedManagerBase<GameManager>.instance.isGameTimePaused = false;
			NetworkedManagerBase<GameManager>.instance.spawnedPopulation = 0f;
			if (s.newZone != null)
			{
				InvokeOnZoneLoaded();
			}
		}
	}

	public bool ShouldVoteOnTravel()
	{
		if (forceVoteForDebug.HasValue)
		{
			return forceVoteForDebug.Value;
		}
		return DewPlayer.humanPlayers.Count((DewPlayer p) => !p.hero.IsNullInactiveDeadOrKnockedOut()) > 1;
	}

	[Server]
	public void StartVoteNextNode(DewPlayer player, int nextNodeIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::StartVoteNextNode(DewPlayer,System.Int32)' called when server was not active");
		}
		else
		{
			StartVote_Imp(player, VoteType.NextNode, nextNodeIndex);
		}
	}

	[Server]
	public void StartVoteNextZone(DewPlayer player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::StartVoteNextZone(DewPlayer)' called when server was not active");
		}
		else
		{
			StartVote_Imp(player, VoteType.NextZone, -1);
		}
	}

	[Server]
	public void StartVoteSidetrack(DewPlayer player, Rift_Sidetrack rift)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::StartVoteSidetrack(DewPlayer,Rift_Sidetrack)' called when server was not active");
		}
		else
		{
			StartVote_Imp(player, VoteType.Sidetrack, (int)rift.netId);
		}
	}

	private void StartVote_Imp(DewPlayer player, VoteType type, int data)
	{
		if (isVoting)
		{
			return;
		}
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			h.isReady = player == h;
		}
		Network_003CisVoting_003Ek__BackingField = true;
		RpcInvokeVoteStarted(player);
		RpcShowVoteChatMessage(isStart: true, player);
		Network_003CvoteRemainingSeconds_003Ek__BackingField = 6;
		Network_003CvoteData_003Ek__BackingField = data;
		NetworkvoteType = type;
		_voteRoutine = StartCoroutine(Routine());
		IEnumerator Routine()
		{
			while (voteRemainingSeconds > 0)
			{
				yield return new WaitForSeconds(1f);
				voteRemainingSeconds--;
				if (voteRemainingSeconds <= 0)
				{
					break;
				}
			}
			WaitAndCompleteVote();
			_voteRoutine = null;
		}
	}

	[Server]
	internal void UpdateVoteStatus()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::UpdateVoteStatus()' called when server was not active");
		}
		else if (isVoting)
		{
			if (_voteCheckRoutine != null)
			{
				StopCoroutine(_voteCheckRoutine);
				_voteCheckRoutine = null;
			}
			_voteCheckRoutine = StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.4f);
			if (isVoting && DewPlayer.humanPlayers.All((DewPlayer p) => p.isReady || p.hero.IsNullInactiveDeadOrKnockedOut()))
			{
				WaitAndCompleteVote();
			}
			_voteCheckRoutine = null;
		}
	}

	[Command(requiresAuthority = false)]
	public void CmdCancelVote(NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void ZoneManager::CmdCancelVote(Mirror.NetworkConnectionToClient)", 539255647, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	private void WaitAndCompleteVote()
	{
		if (isVoting)
		{
			if (_voteCompleteRoutine != null)
			{
				StopCoroutine(_voteCompleteRoutine);
			}
			if (_voteCheckRoutine != null)
			{
				StopCoroutine(_voteCheckRoutine);
				_voteCheckRoutine = null;
			}
			_voteCompleteRoutine = StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			float lastCannotTravelMessageShowTime = float.NegativeInfinity;
			while (GetCannotTravelReason() != null)
			{
				if (Time.time - lastCannotTravelMessageShowTime > 4f)
				{
					RpcShowCannotTravelMessage();
					lastCannotTravelMessageShowTime = Time.time;
				}
				yield return new WaitForSeconds(0.25f);
			}
			try
			{
				switch (voteType)
				{
				case VoteType.NextNode:
					TravelToNode(voteData);
					break;
				case VoteType.NextZone:
					NetworkedManagerBase<GameManager>.instance.LoadNextZone();
					break;
				case VoteType.Sidetrack:
				{
					Rift_Sidetrack rift = GetVoteSidetrackRift();
					if (rift != null)
					{
						rift.TravelImmediately();
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				case VoteType.None:
					break;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			ClearVoteState();
			RpcInvokeVoteCompleted();
		}
	}

	public Rift_Sidetrack GetVoteSidetrackRift()
	{
		if (voteData < 0)
		{
			return null;
		}
		if (!NetworkClient.spawned.TryGetValue((uint)voteData, out var netIdentity))
		{
			return null;
		}
		return netIdentity.GetComponent<Rift_Sidetrack>();
	}

	private void ClearVoteState()
	{
		Network_003CvoteData_003Ek__BackingField = -1;
		Network_003CisVoting_003Ek__BackingField = false;
		if (_voteRoutine != null)
		{
			StopCoroutine(_voteRoutine);
			_voteRoutine = null;
		}
		if (_voteCheckRoutine != null)
		{
			StopCoroutine(_voteCheckRoutine);
			_voteCheckRoutine = null;
		}
		if (_voteCompleteRoutine != null)
		{
			StopCoroutine(_voteCompleteRoutine);
			_voteCompleteRoutine = null;
		}
	}

	[ClientRpc]
	private void RpcShowVoteChatMessage(bool isStart, DewPlayer player)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(isStart);
		writer.WriteNetworkBehaviour(player);
		SendRPCInternal("System.Void ZoneManager::RpcShowVoteChatMessage(System.Boolean,DewPlayer)", 160450285, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcShowCannotTravelMessage()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void ZoneManager::RpcShowCannotTravelMessage()", 1307992230, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcInvokeVoteStarted(DewPlayer player)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(player);
		SendRPCInternal("System.Void ZoneManager::RpcInvokeVoteStarted(DewPlayer)", -2004448797, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcInvokeVoteCanceled(DewPlayer player)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteNetworkBehaviour(player);
		SendRPCInternal("System.Void ZoneManager::RpcInvokeVoteCanceled(DewPlayer)", -2054120585, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[ClientRpc]
	private void RpcInvokeVoteCompleted()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendRPCInternal("System.Void ZoneManager::RpcInvokeVoteCompleted()", 1605286274, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	private void Start()
	{
		nodes.Callback += delegate
		{
			_needToCallNodesChanged = true;
		};
	}

	private void LateUpdate()
	{
		if (_needToCallNodesChanged)
		{
			_needToCallNodesChanged = false;
			ClientEvent_OnNodesChanged?.Invoke();
		}
	}

	public float GetHunterProgress()
	{
		int total = 0;
		int corrupted = 0;
		for (int i = 0; i < nodes.Count; i++)
		{
			if (!nodes[i].IsSidetrackNode() && nodes[i].type != WorldNodeType.ExitBoss)
			{
				total++;
				if (hunterStatuses[i] != 0)
				{
					corrupted++;
				}
			}
		}
		return (float)corrupted / (float)total;
	}

	public void TravelWithValidationAndConfirmation(Action action, bool ignoreSidetrack = false)
	{
		if (DewPlayer.local == null || DewPlayer.local.hero.IsNullInactiveDeadOrKnockedOut())
		{
			return;
		}
		if (isVoting)
		{
			InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_AlreadyVotingForTravel");
			return;
		}
		bool hasRemainingReward = false;
		bool hasRemainingHeroSoul = false;
		bool hasOpenSidetrackRift = false;
		bool hasMemoryOnGround = false;
		bool hasEssenceOnGround = false;
		try
		{
			foreach (DewPlayer h in DewPlayer.humanPlayers)
			{
				if (h.isReadingArtifactStory && !h.hero.IsNullInactiveDeadOrKnockedOut())
				{
					string template = DewLocalization.GetUIValue("InGame_Message_ReadingArtifactStory");
					InGameUIManager.instance.ShowCenterMessageRaw(CenterMessageType.Error, string.Format(template, ChatManager.GetColoredDescribedPlayerName(h)));
					return;
				}
			}
			foreach (Actor a in NetworkedManagerBase<ActorManager>.instance.allActors)
			{
				if (a.IsNullOrInactive())
				{
					continue;
				}
				if (a is Shrine_Concept || a is Shrine_Memory || a is Shrine_Enlightenment || a is Shrine_Retrospection || a is Shrine_Chaos)
				{
					if (a is Shrine { isAvailable: false })
					{
						continue;
					}
					hasRemainingReward = true;
				}
				if (a is Shrine_Stardust stardust)
				{
					Vector3 riftPos = Vector3.zero;
					if (Rift_RoomExit.instance != null)
					{
						riftPos = Rift_RoomExit.instance.transform.position;
					}
					else if (Rift_Sidetrack.instance != null)
					{
						riftPos = Rift_Sidetrack.instance.transform.position;
					}
					if (riftPos != Vector3.zero && Vector3.Distance(riftPos, stardust.position) < 12f)
					{
						hasRemainingReward = true;
					}
				}
				if (a is SkillTrigger skill && skill.owner == null && skill.handOwner == null && Rift.instance != null && Dew.GetNavMeshPathStatus(Dew.GetValidAgentPosition(Dew.GetPositionOnGround(Rift.instance.transform.position)), Dew.GetValidAgentPosition(Dew.GetPositionOnGround(skill.position))) == NavMeshPathStatus.PathComplete)
				{
					hasMemoryOnGround = true;
					InGameUIManager.instance.ShowCenterMessageRaw(CenterMessageType.Error, GetCannotTravelReasonByDroppedSkill(skill));
					return;
				}
				if (a is Gem gem && gem.owner == null && gem.handOwner == null && Rift.instance != null && Dew.GetNavMeshPathStatus(Dew.GetValidAgentPosition(Dew.GetPositionOnGround(Rift.instance.transform.position)), Dew.GetValidAgentPosition(Dew.GetPositionOnGround(gem.position))) == NavMeshPathStatus.PathComplete)
				{
					hasEssenceOnGround = true;
					InGameUIManager.instance.ShowCenterMessageRaw(CenterMessageType.Error, GetCannotTravelReasonByDroppedGem(gem));
					return;
				}
				if (a is Shrine_HeroSoul { isAvailable: not false })
				{
					hasRemainingHeroSoul = true;
				}
			}
			if (!isSidetracking && Rift_Sidetrack.instance != null && Rift_Sidetrack.instance.isOpen)
			{
				hasOpenSidetrackRift = true;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		string rawContent = "";
		if (hasRemainingHeroSoul)
		{
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_DidntSaveLostSoul") + "\n";
		}
		if (hasRemainingReward)
		{
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_UnclaimedReward") + "\n";
		}
		if (hasMemoryOnGround)
		{
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_HasMemoryOnGround") + "\n";
		}
		if (hasEssenceOnGround)
		{
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_HasEssenceOnGround") + "\n";
		}
		if (hasOpenSidetrackRift && !ignoreSidetrack)
		{
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_HasOtherworldRiftOpen") + "\n";
		}
		if (rawContent == "")
		{
			action();
			return;
		}
		DewMessageSettings msg = new DewMessageSettings
		{
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
			defaultButton = DewMessageSettings.ButtonType.No,
			destructiveConfirm = true,
			onClose = delegate(DewMessageSettings.ButtonType b)
			{
				if (b == DewMessageSettings.ButtonType.Yes)
				{
					action();
				}
			},
			validator = () => InGameUIManager.ValidateInGameActionMessage(),
			rawContent = rawContent + DewLocalization.GetUIValue("InGame_Message_DoYouWishToContinue")
		};
		ManagerBase<MessageManager>.instance.ShowMessage(msg);
	}

	public void GenerateWorld()
	{
		int seed = new global::System.Random().Next();
		if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
		{
			seed = 16;
		}
		GenerateWorld(seed);
	}

	public void GenerateWorld(int seed)
	{
		DewRandomInstance random = new DewRandomInstance(seed);
		List<RoomModifierBase> mods = LoadModifierPrefabsOfCurrentZone();
		int numOfNodes = random.Range(currentZone.numOfNodes.x, currentZone.numOfNodes.y + 1) + DewBuildProfile.current.worldNodeCountOffset;
		List<Vector2> currentNodes = new List<Vector2>();
		List<bool> adjacentMatrix = new List<bool>();
		float bestScore = float.NegativeInfinity;
		List<Vector2> bestNodes = new List<Vector2>();
		bool[] isIsolated = new bool[numOfNodes];
		bool[] isExplored = new bool[numOfNodes];
		for (int iteration = 0; iteration < WorldGenerationIterations; iteration++)
		{
			float score = 0f;
			currentNodes.Clear();
			Vector2 range = new Vector2(40f, 70f);
			if (random.value > 0.5f)
			{
				currentNodes.Add(new Vector2(random.Range(range.x, range.y), random.Range(range.x, range.y)));
				currentNodes.Add(new Vector2(WorldWidth - random.Range(range.x, range.y), WorldHeight - random.Range(range.x, range.y)));
			}
			else
			{
				currentNodes.Add(new Vector2(WorldWidth - random.Range(range.x, range.y), random.Range(range.x, range.y)));
				currentNodes.Add(new Vector2(random.Range(range.x, range.y), WorldHeight - random.Range(range.x, range.y)));
			}
			if (random.value < 0.5f)
			{
				List<Vector2> list = currentNodes;
				List<Vector2> list2 = currentNodes;
				Vector2 vector = currentNodes[1];
				Vector2 vector2 = currentNodes[0];
				Vector2 vector4 = (list[0] = vector);
				vector4 = (list2[1] = vector2);
			}
			int addedNodes = currentNodes.Count;
			for (int j = 0; j < numOfNodes - addedNodes; j++)
			{
				GetRandomNodePosWithMinDistance(out var pos);
				currentNodes.Add(pos);
			}
			if (StartOnRandomNode)
			{
				int i2 = random.Range(0, currentNodes.Count);
				if (i2 != 0)
				{
					List<Vector2> list3 = currentNodes;
					List<Vector2> list2 = currentNodes;
					int index2 = i2;
					Vector2 vector2 = currentNodes[i2];
					Vector2 vector = currentNodes[0];
					Vector2 vector4 = (list3[0] = vector2);
					vector4 = (list2[index2] = vector);
				}
			}
			adjacentMatrix.Clear();
			for (int k = 0; k < currentNodes.Count; k++)
			{
				Vector2 from = currentNodes[k];
				for (int l = 0; l < currentNodes.Count; l++)
				{
					Vector2 to = currentNodes[l];
					if (k == l)
					{
						adjacentMatrix.Add(item: false);
					}
					else
					{
						adjacentMatrix.Add(Vector2.SqrMagnitude(from - to) < AdjacentThreshold * AdjacentThreshold);
					}
				}
			}
			for (int m = 0; m < currentNodes.Count; m++)
			{
				int numOfConnected = 0;
				for (int n = 0; n < currentNodes.Count; n++)
				{
					if (adjacentMatrix[currentNodes.Count * m + n])
					{
						numOfConnected++;
					}
				}
				score += ScorePerNodeConnection[Mathf.Clamp(numOfConnected, 0, ScorePerNodeConnection.Length - 1)];
			}
			for (int num = 0; num < currentNodes.Count; num++)
			{
				isIsolated[num] = true;
				isExplored[num] = false;
			}
			isIsolated[0] = false;
			ExploreIndex(0);
			for (int i3 = isIsolated.Length - 1; i3 >= 0; i3--)
			{
				if (isIsolated[i3])
				{
					currentNodes.RemoveAt(i3);
					score = ((i3 > 2) ? (score + ScorePerIsolatedNode) : (score + ScorePerIsolatedImportantNode));
				}
			}
			score *= 1f + random.Range(-1f, 1f) * ScoreFuzziness;
			if (score > bestScore)
			{
				bestScore = score;
				bestNodes.Clear();
				bestNodes.AddRange(currentNodes);
			}
		}
		Bounds bounds = default(Bounds);
		foreach (Vector2 node2 in bestNodes)
		{
			bounds.Encapsulate(new Vector2(node2.x - WorldWidth * 0.5f, node2.y - WorldHeight * 0.5f));
		}
		for (int num2 = 0; num2 < bestNodes.Count; num2++)
		{
			bestNodes[num2] -= (Vector2)bounds.center;
		}
		int n2 = bestNodes.Count;
		int[] d = new int[n2 * n2];
		CalculateDistance(-1);
		List<int> emptyNodes = new List<int>();
		WorldNodeData[] newNodes = new WorldNodeData[bestNodes.Count];
		for (int num3 = 0; num3 < bestNodes.Count; num3++)
		{
			newNodes[num3].position = bestNodes[num3];
			newNodes[num3].status = WorldNodeStatus.Unexplored;
			newNodes[num3].modifiers = new List<ModifierData>();
			newNodes[num3].roomRotIndex = -1;
			emptyNodes.Add(num3);
		}
		HunterStatus[] newHunterLevels = new HunterStatus[newNodes.Length];
		hunterStatuses.Clear();
		hunterStatuses.AddRange(newHunterLevels);
		currentTurnIndex = 0;
		emptyNodes.Remove(0);
		newNodes[0].type = WorldNodeType.Start;
		if (currentZone.startRooms == null || currentZone.startRooms.Count == 0)
		{
			Debug.LogWarning(currentZone.name + " does not have start rooms");
		}
		else
		{
			newNodes[0].room = currentZone.startRooms[random.Range(0, currentZone.startRooms.Count)];
		}
		newNodes[0].status = WorldNodeStatus.Unexplored;
		List<int> needToTraverse = new List<int>();
		Queue<int> floodFillQueue = new Queue<int>();
		int exitNode2 = Dew.SelectBestWithScore(emptyNodes, delegate(int i, int _)
		{
			float num4 = 0f;
			int num5 = nodeDistanceMatrix[i];
			if (num5 >= 4)
			{
				num4 += 20f;
			}
			else if (num5 < 3)
			{
				num4 -= 30f;
			}
			for (int num6 = 1; num6 < newNodes.Length; num6++)
			{
				if (num6 != i)
				{
					needToTraverse.Add(num6);
				}
			}
			floodFillQueue.Enqueue(0);
			int result;
			while (floodFillQueue.TryDequeue(out result))
			{
				for (int num7 = needToTraverse.Count - 1; num7 >= 0; num7--)
				{
					if (nodeDistanceMatrix[result * bestNodes.Count + needToTraverse[num7]] == 1)
					{
						floodFillQueue.Enqueue(needToTraverse[num7]);
						needToTraverse.RemoveAt(num7);
					}
				}
			}
			num4 -= (float)(needToTraverse.Count * 100);
			floodFillQueue.Clear();
			needToTraverse.Clear();
			num4 += (float)num5 * 1f;
			for (int num8 = 0; num8 < bestNodes.Count; num8++)
			{
				if (nodeDistanceMatrix[i * bestNodes.Count + num8] == 1)
				{
					num4 += 7.5f;
				}
			}
			return num4;
		}, 0.15f, random);
		emptyNodes.Remove(exitNode2);
		newNodes[exitNode2].type = WorldNodeType.ExitBoss;
		if (currentZone.bossRooms == null || currentZone.bossRooms.Count == 0)
		{
			Debug.LogWarning(currentZone.name + " does not have exit boss rooms");
		}
		else
		{
			newNodes[exitNode2].room = currentZone.bossRooms[random.Range(0, currentZone.bossRooms.Count)];
		}
		newNodes[exitNode2].status = WorldNodeStatus.Revealed;
		CalculateDistance(exitNode2);
		int furthestFromExit = Dew.SelectBestIndexWithScore((IList<WorldNodeData>)newNodes, (Func<WorldNodeData, int, float>)((WorldNodeData _, int index) => nodeDistanceMatrix[newNodes.Length * index + exitNode2]), 0.01f, random);
		Network_003ChunterStartNodeIndex_003Ek__BackingField = furthestFromExit;
		if (hunterStartNodeIndex != 0)
		{
			emptyNodes.Remove(hunterStartNodeIndex);
			newNodes[hunterStartNodeIndex].type = WorldNodeType.Combat;
		}
		int numOfShops = (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth) ? 1 : random.Range(currentZone.numOfMerchants.x, currentZone.numOfMerchants.y + 1));
		for (int num9 = 0; num9 < numOfShops; num9++)
		{
			int nodeIndex;
			if (currentZoneIndex <= 0)
			{
				int ii = Dew.SelectBestIndexWithScore(emptyNodes, (int node, int _) => Mathf.Min(nodeDistanceMatrix[node], 3), 0.4f, random);
				nodeIndex = emptyNodes[ii];
				emptyNodes.RemoveAt(ii);
			}
			else
			{
				int ii2 = random.Range(0, emptyNodes.Count);
				nodeIndex = emptyNodes[ii2];
				emptyNodes.RemoveAt(ii2);
			}
			newNodes[nodeIndex].type = WorldNodeType.Merchant;
		}
		int numOfChallenges = random.Range(currentZone.numOfEvents.x, currentZone.numOfEvents.y + 1);
		for (int num10 = 0; num10 < numOfChallenges; num10++)
		{
			int ii3 = random.Range(0, emptyNodes.Count);
			int nodeIndex2 = emptyNodes[ii3];
			emptyNodes.RemoveAt(ii3);
			newNodes[nodeIndex2].type = WorldNodeType.Event;
			if (currentZone.eventRooms == null || currentZone.eventRooms.Count == 0)
			{
				Debug.LogWarning(currentZone.name + " does not have challenge rooms");
			}
			else
			{
				newNodes[nodeIndex2].room = currentZone.eventRooms[random.Range(0, currentZone.eventRooms.Count)];
			}
		}
		foreach (int i4 in emptyNodes)
		{
			newNodes[i4].type = WorldNodeType.Combat;
		}
		emptyNodes.Clear();
		int combatNodeCount = 0;
		for (int num11 = 0; num11 < newNodes.Length; num11++)
		{
			if (num11 != hunterStartNodeIndex && newNodes[num11].type == WorldNodeType.Combat)
			{
				combatNodeCount++;
			}
		}
		List<int> combatNodeIndices = new List<int>();
		RoomModifierBase modifierToAdd;
		foreach (RoomModifierBase item2 in mods)
		{
			modifierToAdd = item2;
			float multiplier = 1f;
			if (DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth))
			{
				string modName = modifierToAdd.GetType().Name;
				multiplier = ((!(modName == "RoomMod_SpawnMiniBoss") && !(modName == "RoomMod_HarderFightBetterReward")) ? 2f : 1.5f);
			}
			if (modifierToAdd.spawnType == ModifierSpawnType.Chance)
			{
				int count2 = 0;
				for (int num12 = 0; num12 < combatNodeCount; num12++)
				{
					if (random.value < modifierToAdd.GetScaledChance() * multiplier)
					{
						count2++;
					}
				}
				SpawnWithCount(count2);
			}
			if (modifierToAdd.spawnType == ModifierSpawnType.Count)
			{
				SpawnWithCount(DewMath.RandomRoundToInt(modifierToAdd.GetScaledChance() * multiplier, random));
			}
			if (modifierToAdd.spawnType == ModifierSpawnType.Ratio)
			{
				SpawnWithCount(DewMath.RandomRoundToInt(modifierToAdd.GetScaledChance() * (float)combatNodeCount * multiplier, random));
			}
		}
		WorldNodeData[] array = newNodes;
		for (int index2 = 0; index2 < array.Length; index2++)
		{
			WorldNodeData node3 = array[index2];
			if (node3.modifiers == null)
			{
				continue;
			}
			foreach (ModifierData mod in node3.modifiers)
			{
				if (!modifierServerData.ContainsKey(mod.id))
				{
					modifierServerData[mod.id] = new Dictionary<string, object>();
				}
			}
		}
		nodes.Clear();
		nodes.AddRange(newNodes);
		Network_003CcurrentNodeIndex_003Ek__BackingField = -1;
		visitedNodesSaveData = new List<WorldNodeSaveData>();
		for (int num13 = 0; num13 < nodes.Count; num13++)
		{
			visitedNodesSaveData.Add(null);
		}
		_combatRoomPool.Clear();
		_shopRoomPool.Clear();
		_usedAngleIndexes.Clear();
		void CalculateDistance(int exitNode)
		{
			for (int num17 = 0; num17 < n2; num17++)
			{
				for (int num18 = 0; num18 < n2; num18++)
				{
					int index3 = num17 * n2 + num18;
					if (num17 == num18)
					{
						d[index3] = 0;
					}
					else if (num17 == exitNode)
					{
						d[index3] = 10000;
					}
					else if (Vector2.SqrMagnitude(bestNodes[num17] - bestNodes[num18]) < AdjacentThreshold * AdjacentThreshold)
					{
						d[index3] = 1;
					}
					else
					{
						d[index3] = 10000;
					}
				}
			}
			for (int num19 = 0; num19 < n2; num19++)
			{
				for (int s = 0; s < n2; s++)
				{
					for (int e = 0; e < n2; e++)
					{
						if (d[s * n2 + e] > d[s * n2 + num19] + d[num19 * n2 + e])
						{
							d[s * n2 + e] = d[s * n2 + num19] + d[num19 * n2 + e];
						}
					}
				}
			}
			nodeDistanceMatrix.Clear();
			nodeDistanceMatrix.AddRange(d);
		}
		void ExploreIndex(int index)
		{
			isExplored[index] = true;
			for (int num16 = 0; num16 < currentNodes.Count; num16++)
			{
				if (adjacentMatrix[currentNodes.Count * index + num16])
				{
					isIsolated[num16] = false;
					if (!isExplored[num16])
					{
						ExploreIndex(num16);
					}
				}
			}
		}
		bool GetRandomNodePosWithMinDistance(out Vector2 candidate)
		{
			candidate = default(Vector2);
			for (int num14 = 0; num14 < NodePlacementTries; num14++)
			{
				if (currentNodes.Count > 0)
				{
					Vector2 basis = currentNodes[random.Range(0, currentNodes.Count)];
					bool outOfBounds = true;
					for (int num15 = 0; num15 < 10; num15++)
					{
						candidate = basis + random.insideUnitCircle.normalized * random.Range(MinDistanceBetweenNode, MaxDistanceToNearestNode);
						if (candidate.x >= 0f && candidate.x <= WorldWidth && candidate.y >= 0f && candidate.y <= WorldHeight)
						{
							outOfBounds = false;
							break;
						}
					}
					if (outOfBounds)
					{
						continue;
					}
				}
				else
				{
					candidate = new Vector2(random.Range(0f, WorldWidth), random.Range(0f, WorldHeight));
				}
				bool tooClose = false;
				float sqrDistToNearest = 0f;
				foreach (Vector2 c in currentNodes)
				{
					float sqrDist = Vector2.SqrMagnitude(candidate - c);
					sqrDistToNearest = Mathf.Min(sqrDistToNearest, sqrDist);
					if (sqrDist < MinDistanceBetweenNode * MinDistanceBetweenNode)
					{
						tooClose = true;
					}
				}
				if (!tooClose && !(sqrDistToNearest > MaxDistanceToNearestNode * MaxDistanceToNearestNode))
				{
					return true;
				}
			}
			return false;
		}
		void SpawnWithCount(int count)
		{
			if (count > 0)
			{
				combatNodeIndices.Clear();
				for (int num20 = 0; num20 < newNodes.Length; num20++)
				{
					if (num20 != hunterStartNodeIndex && newNodes[num20].type == WorldNodeType.Combat)
					{
						combatNodeIndices.Add(num20);
					}
				}
				combatNodeIndices.Shuffle(random);
				for (int num21 = 0; num21 < count; num21++)
				{
					bool didAdd = false;
					for (int num22 = 0; num22 < combatNodeIndices.Count; num22++)
					{
						bool isValid = true;
						foreach (ModifierData modifier in newNodes[combatNodeIndices[num22]].modifiers)
						{
							RoomModifierBase existingModifier = DewResources.GetByShortTypeName<RoomModifierBase>(modifier.type);
							if (existingModifier.disallowOtherModifiers || modifierToAdd.disallowOtherModifiers)
							{
								isValid = false;
								break;
							}
							if (existingModifier.modifiesRewards && modifierToAdd.modifiesRewards)
							{
								isValid = false;
								break;
							}
							if (existingModifier.isMain && modifierToAdd.isMain)
							{
								isValid = false;
								break;
							}
						}
						if (isValid)
						{
							List<ModifierData> modifiers = newNodes[combatNodeIndices[num22]].modifiers;
							ModifierData item = default(ModifierData);
							ZoneManager zoneManager = this;
							int nextModifierId = _nextModifierId;
							zoneManager._nextModifierId = nextModifierId + 1;
							item.id = nextModifierId;
							item.type = modifierToAdd.name;
							item.clientData = "";
							modifiers.Add(item);
							combatNodeIndices.RemoveAt(num22);
							didAdd = true;
							break;
						}
					}
					if (!didAdd)
					{
						Debug.LogWarning("No valid node found to put " + modifierToAdd.name);
						break;
					}
				}
			}
		}
	}

	[Server]
	public void SetCurrentNodeIndexAndRevealAdjacent(int index)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::SetCurrentNodeIndexAndRevealAdjacent(System.Int32)' called when server was not active");
			return;
		}
		if (index < 0 || index >= nodes.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		Network_003CcurrentNodeIndex_003Ek__BackingField = index;
		if (currentNodeIndex < 0)
		{
			return;
		}
		WorldNodeData temp = nodes[currentNodeIndex];
		temp.status = WorldNodeStatus.HasVisited;
		nodes[currentNodeIndex] = temp;
		for (int i = 0; i < nodes.Count; i++)
		{
			if (IsNodeConnected(index, i))
			{
				WorldNodeData t = nodes[i];
				if (t.status == WorldNodeStatus.Unexplored)
				{
					t.status = WorldNodeStatus.Revealed;
					nodes[i] = t;
				}
			}
		}
	}

	[Server]
	public void AddSidetrackNode(string roomName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::AddSidetrackNode(System.String)' called when server was not active");
			return;
		}
		nodes.Add(new WorldNodeData
		{
			room = roomName,
			type = WorldNodeType.Special,
			position = Vector2.one * -100000f,
			modifiers = new List<ModifierData>()
		});
		hunterStatuses.Add(HunterStatus.None);
		visitedNodesSaveData.Add(null);
		int previousSize = nodes.Count - 1;
		for (int i = 0; i < previousSize + 1; i++)
		{
			nodeDistanceMatrix.Add((i != previousSize) ? 10000 : 0);
		}
		for (int index = previousSize * previousSize; index > 0; index -= previousSize)
		{
			nodeDistanceMatrix.Insert(index, 10000);
		}
	}

	public bool IsNodeConnected(int a, int b)
	{
		if (GetNodeDistance(a, b) != 1)
		{
			return GetNodeDistance(b, a) == 1;
		}
		return true;
	}

	public int GetNodeDistance(int a, int b)
	{
		return nodeDistanceMatrix[nodes.Count * a + b];
	}

	[Server]
	public void RevealWorld(bool fully = false)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::RevealWorld(System.Boolean)' called when server was not active");
			return;
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			WorldNodeData t = nodes[i];
			WorldNodeStatus target = ((!fully) ? WorldNodeStatus.Revealed : WorldNodeStatus.RevealedFull);
			if (t.status < target)
			{
				t.status = target;
				nodes[i] = t;
			}
		}
	}

	[Server]
	public void AdvanceHunterTurn(bool forceMove = false)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::AdvanceHunterTurn(System.Boolean)' called when server was not active");
		}
		else
		{
			if (isHuntAdvanceDisabled || (!forceMove && currentTurnIndex < HunterStartSkippedTurns - 1))
			{
				return;
			}
			if ((forceMove || currentTurnIndex >= HunterStartSkippedTurns - 1) && hunterStatuses[hunterStartNodeIndex] == HunterStatus.None)
			{
				hunterStatuses[hunterStartNodeIndex] = HunterStatus.AboutToBeTaken;
			}
			if (!forceMove && currentTurnIndex < HunterStartSkippedTurns)
			{
				return;
			}
			for (int i = 0; i < nodes.Count; i++)
			{
				if (hunterStatuses[i] == HunterStatus.AboutToBeTaken)
				{
					hunterStatuses[i] = HunterStatus.Level1;
				}
				else if (hunterStatuses[i] == HunterStatus.Level1)
				{
					hunterStatuses[i] = HunterStatus.Level2;
				}
				else if (hunterStatuses[i] == HunterStatus.Level2)
				{
					hunterStatuses[i] = HunterStatus.Level3;
				}
			}
			List<int> spreadIndices = new List<int>();
			for (int j = 0; j < nodes.Count; j++)
			{
				if (hunterStatuses[j] < HunterStatus.Level1)
				{
					continue;
				}
				for (int k = 0; k < nodes.Count; k++)
				{
					if (IsNodeConnected(j, k) && hunterStatuses[k] == HunterStatus.None && nodes[k].type != WorldNodeType.ExitBoss && !spreadIndices.Contains(k))
					{
						spreadIndices.Add(k);
					}
				}
			}
			float multiplier = (isCurrentNodeHunted ? 0.8f : 1f);
			_hunterSpreadCredit += NetworkedManagerBase<GameManager>.instance.difficulty.hunterSpreadChance * (float)spreadIndices.Count * multiplier;
			int bossNode = nodes.FindIndex((WorldNodeData w) => w.type == WorldNodeType.ExitBoss);
			while (_hunterSpreadCredit >= 1f && spreadIndices.Count > 0)
			{
				_hunterSpreadCredit -= 1f;
				int i2 = Dew.SelectBestIndexWithScore(spreadIndices, (int index, int _) => GetNodeDistance(index, bossNode), 0.1f);
				hunterStatuses[spreadIndices[i2]] = HunterStatus.AboutToBeTaken;
				spreadIndices.RemoveAt(i2);
			}
		}
	}

	[Server]
	public void UpdateModifiersByHunterStatus(int currentTravelDestinationNode = -1)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::UpdateModifiersByHunterStatus(System.Int32)' called when server was not active");
			return;
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			WorldNodeData n = nodes[i];
			if (n.HasModifier<RoomMod_Hunted>())
			{
				HunterStatus hunterStatus = hunterStatuses[i];
				if (hunterStatus == HunterStatus.None || hunterStatus == HunterStatus.AboutToBeTaken)
				{
					RemoveModifier<RoomMod_Hunted>(i);
				}
			}
			if (hunterStatuses[i] == HunterStatus.None || (hunterStatuses[i] == HunterStatus.AboutToBeTaken && i == currentTravelDestinationNode))
			{
				continue;
			}
			if (n.type == WorldNodeType.Merchant && !n.HasModifier<RoomMod_FledMerchant>())
			{
				AddModifier<RoomMod_FledMerchant>(i);
			}
			for (int j = n.modifiers.Count - 1; j >= 0; j--)
			{
				RoomModifierBase mod = DewResources.GetByShortTypeName<RoomModifierBase>(n.modifiers[j].type);
				if (mod != null && mod.isMain && !(mod is RoomMod_Hunted))
				{
					RemoveModifier(i, n.modifiers[j].id);
					break;
				}
			}
			if (!n.HasModifier<RoomMod_Hunted>())
			{
				AddModifier<RoomMod_Hunted>(i);
			}
		}
	}

	public string GetCannotTravelReason()
	{
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			if (!h.hero.isKnockedOut)
			{
				if (!h.hero.Skill.holdingObject.IsHoldableObjectNullOrInactive())
				{
					return string.Format(DewLocalization.GetUIValue("InGame_Message_RiftCantHoldOntoItem"), ChatManager.GetColoredDescribedPlayerName(h));
				}
				if (h.hero.Status.isInConversation)
				{
					return string.Format(DewLocalization.GetUIValue("InGame_Message_RiftInConversation"), ChatManager.GetColoredDescribedPlayerName(h));
				}
				if (h.isReadingArtifactStory && !h.hero.IsNullInactiveDeadOrKnockedOut())
				{
					return string.Format(DewLocalization.GetUIValue("InGame_Message_ReadingArtifactStory"), ChatManager.GetColoredDescribedPlayerName(h));
				}
			}
		}
		foreach (Actor a in NetworkedManagerBase<ActorManager>.instance.allActors)
		{
			if (a is SkillTrigger st && st.owner == null && st.handOwner == null && Rift.instance != null && Dew.GetNavMeshPathStatus(Dew.GetValidAgentPosition(Dew.GetPositionOnGround(Rift.instance.transform.position)), Dew.GetValidAgentPosition(Dew.GetPositionOnGround(st.position))) == NavMeshPathStatus.PathComplete)
			{
				return GetCannotTravelReasonByDroppedSkill(st);
			}
			if (a is Gem g && g.owner == null && g.handOwner == null && Rift.instance != null && Dew.GetNavMeshPathStatus(Dew.GetValidAgentPosition(Dew.GetPositionOnGround(Rift.instance.transform.position)), Dew.GetValidAgentPosition(Dew.GetPositionOnGround(g.position))) == NavMeshPathStatus.PathComplete)
			{
				return GetCannotTravelReasonByDroppedGem(g);
			}
		}
		return null;
	}

	public string GetCannotTravelReasonByDroppedSkill(SkillTrigger st)
	{
		string template = DewLocalization.GetUIValue("InGame_Message_RiftMemoryIsOnGround");
		if (st.tempOwner != null && !st.tempOwner.isOwned)
		{
			return string.Format(template, ChatManager.GetColoredDescribedPlayerName(st.tempOwner) + " - " + ChatManager.GetColoredSkillName(st.GetType().Name, st.level));
		}
		return string.Format(template, ChatManager.GetColoredSkillName(st.GetType().Name, st.level));
	}

	public string GetCannotTravelReasonByDroppedGem(Gem g)
	{
		string template = DewLocalization.GetUIValue("InGame_Message_RiftEssenceIsOnGround");
		if (g.tempOwner != null && !g.tempOwner.isOwned)
		{
			return string.Format(template, ChatManager.GetColoredDescribedPlayerName(g.tempOwner) + " - " + ChatManager.GetColoredGemName(g.GetType().Name, g.quality));
		}
		return string.Format(template, ChatManager.GetColoredGemName(g.GetType().Name, g.quality));
	}

	[Command(requiresAuthority = false)]
	public void CmdTravelToNode(int index, NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(index);
		SendCommandInternal("System.Void ZoneManager::CmdTravelToNode(System.Int32,Mirror.NetworkConnectionToClient)", -2012969603, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	[Command(requiresAuthority = false)]
	public void CmdTravelToNextZone(NetworkConnectionToClient sender = null)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendCommandInternal("System.Void ZoneManager::CmdTravelToNextZone(Mirror.NetworkConnectionToClient)", -277905233, writer, 0, requiresAuthority: false);
		NetworkWriterPool.Return(writer);
	}

	public bool TryGetNodeIndexForNextGoal(GetNodeIndexSettings s, out int nodeIndex)
	{
		int exitNodeIndex = 0;
		for (int j = 0; j < nodes.Count; j++)
		{
			if (nodes[j].type == WorldNodeType.ExitBoss)
			{
				exitNodeIndex = j;
				break;
			}
		}
		int currentDistToExit = GetNodeDistance(currentNodeIndex, exitNodeIndex);
		int index = (nodeIndex = Dew.SelectBestIndexWithScore(nodes, GetScore));
		return GetScore(nodes[index], index) > -5000f;
		float GetScore(WorldNodeData data, int i)
		{
			float score = 0f;
			if (data.IsSidetrackNode())
			{
				score -= 10000f;
			}
			if (!s.allowedTypes.Contains(data.type))
			{
				score -= 10000f;
			}
			if (i == currentNodeIndex)
			{
				score -= 10000f;
			}
			switch (data.status)
			{
			case WorldNodeStatus.Revealed:
			case WorldNodeStatus.RevealedFull:
				score -= 2.5f;
				break;
			case WorldNodeStatus.HasVisited:
				score -= 10000f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case WorldNodeStatus.Unexplored:
				break;
			}
			int nodeDist = GetNodeDistance(currentNodeIndex, i);
			if (nodeDist < s.desiredDistance.x)
			{
				score -= (float)(s.desiredDistance.x - nodeDist) * 1f;
			}
			score = ((nodeDist <= s.desiredDistance.y) ? (score + 5f) : (score - (float)(nodeDist - s.desiredDistance.y) * 1f));
			if (s.preferCloserToExit)
			{
				int delta = currentDistToExit - GetNodeDistance(i, exitNodeIndex);
				score = ((delta <= 0) ? (score + (float)delta * 3f) : (score + (float)delta * 0.75f));
			}
			if (s.preferNoMainModifier && nodes[i].HasMainModifier())
			{
				score -= 6f;
			}
			return score + global::UnityEngine.Random.Range(-1.5f, 1.5f);
		}
	}

	public int AddModifier<T>(int nodeIndex)
	{
		return AddModifier(nodeIndex, new ModifierData
		{
			id = _nextModifierId++,
			type = typeof(T).Name
		});
	}

	public int AddModifier<T>(int nodeIndex, string clientData)
	{
		return AddModifier(nodeIndex, new ModifierData
		{
			id = _nextModifierId++,
			type = typeof(T).Name,
			clientData = clientData
		});
	}

	public int AddModifier<T>(int nodeIndex, string clientData, Dictionary<string, object> serverData)
	{
		int id = _nextModifierId++;
		modifierServerData[id] = serverData;
		return AddModifier(nodeIndex, new ModifierData
		{
			id = id,
			type = typeof(T).Name,
			clientData = clientData
		});
	}

	public int AddModifier(int nodeIndex, ModifierData mod)
	{
		if (mod.id == 0)
		{
			mod.id = _nextModifierId++;
		}
		if (mod.clientData == null)
		{
			mod.clientData = "";
		}
		if (!modifierServerData.TryGetValue(mod.id, out var val) || val == null)
		{
			modifierServerData[mod.id] = new Dictionary<string, object>();
		}
		if (DewResources.GetByShortTypeName<RoomModifierBase>(mod.type).isMain)
		{
			List<ModifierData> prevMods = nodes[nodeIndex].modifiers;
			for (int modIndex = prevMods.Count - 1; modIndex >= 0; modIndex--)
			{
				if (DewResources.GetByShortTypeName<RoomModifierBase>(prevMods[modIndex].type).isMain)
				{
					RemoveModifier(nodeIndex, prevMods[modIndex].id);
					break;
				}
			}
		}
		WorldNodeData temp = nodes[nodeIndex];
		temp.modifiers = new List<ModifierData>(temp.modifiers);
		temp.modifiers.Add(mod);
		nodes[nodeIndex] = temp;
		if (nodeIndex == currentNodeIndex && !isInRoomTransition)
		{
			SingletonDewNetworkBehaviour<Room>.instance.modifiers.HandleRuntimeAddition(mod.id);
		}
		return mod.id;
	}

	public void SetRoomRotIndex(int nodeIndex, int roomRotIndex)
	{
		if (nodeIndex >= 0 && nodeIndex < nodes.Count)
		{
			WorldNodeData temp = nodes[nodeIndex];
			temp.roomRotIndex = roomRotIndex;
			nodes[nodeIndex] = temp;
		}
	}

	public bool RemoveModifier<T>(int nodeIndex)
	{
		if (nodeIndex < 0 || nodeIndex >= nodes.Count)
		{
			return false;
		}
		ModifierData mod = nodes[nodeIndex].modifiers.Find((ModifierData m) => m.type == typeof(T).Name);
		if (mod.id == 0)
		{
			return false;
		}
		RemoveModifier(nodeIndex, mod.id);
		return true;
	}

	public void RemoveModifier(int nodeIndex, int modifierId)
	{
		if (nodeIndex == currentNodeIndex)
		{
			SingletonDewNetworkBehaviour<Room>.instance.modifiers.HandleRuntimeRemoval(modifierId);
		}
		WorldNodeData temp = nodes[nodeIndex];
		temp.modifiers = new List<ModifierData>(temp.modifiers);
		int index = temp.modifiers.FindIndex((ModifierData m) => m.id == modifierId);
		if (index >= 0)
		{
			temp.modifiers.RemoveAt(index);
		}
		nodes[nodeIndex] = temp;
	}

	public void RemoveModifier(int modifierId)
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i].modifiers.FindIndex((ModifierData m) => m.id == modifierId) >= 0)
			{
				RemoveModifier(i, modifierId);
				break;
			}
		}
	}

	[Server]
	public void SyncNodeChanges(int nodeIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::SyncNodeChanges(System.Int32)' called when server was not active");
			return;
		}
		WorldNodeData temp = nodes[nodeIndex];
		temp.modifiers = new List<ModifierData>(temp.modifiers);
		nodes[nodeIndex] = temp;
	}

	public void SetRoom(int nodeIndex, string room)
	{
		WorldNodeData temp = nodes[nodeIndex];
		temp.room = room;
		nodes[nodeIndex] = temp;
	}

	[Server]
	public void LoadNextZoneByContentSettings()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ZoneManager::LoadNextZoneByContentSettings()' called when server was not active");
			return;
		}
		if (_zonesInGame == null)
		{
			Zone[] arr = DewResources.FindAllByNameSubstring<Zone>("Zone_").ToArray();
			_zonesInGame = DewBuildProfile.current.content.FilterZones(arr);
		}
		if (_remainingZonesOfCurrentTier.Count == 0)
		{
			DewGameContentSettings zc = DewBuildProfile.current.content;
			_currentTier++;
			if (_currentTier >= zc.zoneCountByTier.Count)
			{
				_currentTier = 0;
				loopIndex++;
				OnLoopStart();
			}
			Zone[] zonesInGame = _zonesInGame;
			foreach (Zone z in zonesInGame)
			{
				if (z.zoneTier == _currentTier)
				{
					_remainingZonesOfCurrentTier.Add(z);
				}
			}
			_remainingZonesOfCurrentTier.Shuffle();
			if (_remainingZonesOfCurrentTier.Count < zc.zoneCountByTier[_currentTier])
			{
				Debug.LogWarning($"Not enough zones to match current zone settings requirement: {_remainingZonesOfCurrentTier.Count}/{zc.zoneCountByTier[_currentTier]}");
			}
			while (_remainingZonesOfCurrentTier.Count > zc.zoneCountByTier[_currentTier])
			{
				_remainingZonesOfCurrentTier.RemoveAt(_remainingZonesOfCurrentTier.Count - 1);
			}
		}
		int index = global::UnityEngine.Random.Range(0, _remainingZonesOfCurrentTier.Count);
		Zone zone = _remainingZonesOfCurrentTier[index];
		_remainingZonesOfCurrentTier.RemoveAt(index);
		LoadZone(zone);
	}

	public ZoneManager()
	{
		InitSyncObject(nodes);
		InitSyncObject(hunterStatuses);
		InitSyncObject(nodeDistanceMatrix);
	}

	static ZoneManager()
	{
		StartOnRandomNode = false;
		WorldHeight = 120f;
		WorldWidth = 240f;
		AdjacentThreshold = 60f;
		MinDistanceBetweenNode = 25f;
		MaxDistanceToNearestNode = 50f;
		ScorePerNodeConnection = new float[8] { -800f, -800f, -800f, 400f, 800f, 800f, -400f, -800f };
		ScorePerIsolatedNode = -500f;
		ScorePerIsolatedImportantNode = -50000f;
		ScoreFuzziness = 0.1f;
		WorldGenerationIterations = 100;
		NodePlacementTries = 75;
		HunterStartSkippedTurns = 3;
		RemoteProcedureCalls.RegisterCommand(typeof(ZoneManager), "System.Void ZoneManager::CmdCancelVote(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdCancelVote__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterCommand(typeof(ZoneManager), "System.Void ZoneManager::CmdTravelToNode(System.Int32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdTravelToNode__Int32__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterCommand(typeof(ZoneManager), "System.Void ZoneManager::CmdTravelToNextZone(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdTravelToNextZone__NetworkConnectionToClient, requiresAuthority: false);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::InvokeOnRoomLoadStarted(EventInfoLoadRoom)", InvokeUserCode_InvokeOnRoomLoadStarted__EventInfoLoadRoom);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::InvokeOnZoneLoadStarted()", InvokeUserCode_InvokeOnZoneLoadStarted);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::InvokeOnRoomLoaded(EventInfoLoadRoom)", InvokeUserCode_InvokeOnRoomLoaded__EventInfoLoadRoom);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::InvokeOnZoneLoaded()", InvokeUserCode_InvokeOnZoneLoaded);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::InvokeOnLoopStarted()", InvokeUserCode_InvokeOnLoopStarted);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::RpcShowVoteChatMessage(System.Boolean,DewPlayer)", InvokeUserCode_RpcShowVoteChatMessage__Boolean__DewPlayer);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::RpcShowCannotTravelMessage()", InvokeUserCode_RpcShowCannotTravelMessage);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::RpcInvokeVoteStarted(DewPlayer)", InvokeUserCode_RpcInvokeVoteStarted__DewPlayer);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::RpcInvokeVoteCanceled(DewPlayer)", InvokeUserCode_RpcInvokeVoteCanceled__DewPlayer);
		RemoteProcedureCalls.RegisterRpc(typeof(ZoneManager), "System.Void ZoneManager::RpcInvokeVoteCompleted()", InvokeUserCode_RpcInvokeVoteCompleted);
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_InvokeOnRoomLoadStarted__EventInfoLoadRoom(EventInfoLoadRoom info)
	{
		ClientEvent_OnRoomLoadStarted?.Invoke(info);
	}

	protected static void InvokeUserCode_InvokeOnRoomLoadStarted__EventInfoLoadRoom(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC InvokeOnRoomLoadStarted called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_InvokeOnRoomLoadStarted__EventInfoLoadRoom(GeneratedNetworkCode._Read_EventInfoLoadRoom(reader));
		}
	}

	protected void UserCode_InvokeOnZoneLoadStarted()
	{
		ClientEvent_OnZoneLoadStarted?.Invoke();
	}

	protected static void InvokeUserCode_InvokeOnZoneLoadStarted(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC InvokeOnZoneLoadStarted called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_InvokeOnZoneLoadStarted();
		}
	}

	protected void UserCode_InvokeOnRoomLoaded__EventInfoLoadRoom(EventInfoLoadRoom info)
	{
		DewResources.UnloadUnused();
		ClientEvent_OnRoomLoaded?.Invoke(info);
	}

	protected static void InvokeUserCode_InvokeOnRoomLoaded__EventInfoLoadRoom(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC InvokeOnRoomLoaded called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_InvokeOnRoomLoaded__EventInfoLoadRoom(GeneratedNetworkCode._Read_EventInfoLoadRoom(reader));
		}
	}

	protected void UserCode_InvokeOnZoneLoaded()
	{
		ClientEvent_OnZoneLoaded?.Invoke();
	}

	protected static void InvokeUserCode_InvokeOnZoneLoaded(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC InvokeOnZoneLoaded called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_InvokeOnZoneLoaded();
		}
	}

	protected void UserCode_InvokeOnLoopStarted()
	{
		ClientEvent_OnLoopStarted?.Invoke();
	}

	protected static void InvokeUserCode_InvokeOnLoopStarted(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC InvokeOnLoopStarted called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_InvokeOnLoopStarted();
		}
	}

	protected void UserCode_CmdCancelVote__NetworkConnectionToClient(NetworkConnectionToClient sender)
	{
		if (isVoting)
		{
			DewPlayer player = sender.GetPlayer();
			if (!(player == null))
			{
				RpcShowVoteChatMessage(isStart: false, player);
				RpcInvokeVoteCanceled(player);
				ClearVoteState();
			}
		}
	}

	protected static void InvokeUserCode_CmdCancelVote__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCancelVote called on client.");
		}
		else
		{
			((ZoneManager)obj).UserCode_CmdCancelVote__NetworkConnectionToClient(senderConnection);
		}
	}

	protected void UserCode_RpcShowVoteChatMessage__Boolean__DewPlayer(bool isStart, DewPlayer player)
	{
		NetworkedManagerBase<ChatManager>.instance.ShowMessageLocally(new ChatManager.Message
		{
			type = ChatManager.MessageType.Notice,
			content = (isStart ? "InGame_Vote_StartedTravelVote" : "InGame_Vote_CanceledTravelVote"),
			args = new string[1] { ChatManager.GetDescribedPlayerName(player) }
		});
	}

	protected static void InvokeUserCode_RpcShowVoteChatMessage__Boolean__DewPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowVoteChatMessage called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_RpcShowVoteChatMessage__Boolean__DewPlayer(reader.ReadBool(), reader.ReadNetworkBehaviour<DewPlayer>());
		}
	}

	protected void UserCode_RpcShowCannotTravelMessage()
	{
		InGameUIManager.instance.ShowCenterMessageRaw(CenterMessageType.Error, GetCannotTravelReason());
	}

	protected static void InvokeUserCode_RpcShowCannotTravelMessage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowCannotTravelMessage called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_RpcShowCannotTravelMessage();
		}
	}

	protected void UserCode_RpcInvokeVoteStarted__DewPlayer(DewPlayer player)
	{
		try
		{
			ClientEvent_OnVoteStarted?.Invoke(player);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	protected static void InvokeUserCode_RpcInvokeVoteStarted__DewPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcInvokeVoteStarted called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_RpcInvokeVoteStarted__DewPlayer(reader.ReadNetworkBehaviour<DewPlayer>());
		}
	}

	protected void UserCode_RpcInvokeVoteCanceled__DewPlayer(DewPlayer player)
	{
		try
		{
			ClientEvent_OnVoteCanceled?.Invoke(player);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	protected static void InvokeUserCode_RpcInvokeVoteCanceled__DewPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcInvokeVoteCanceled called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_RpcInvokeVoteCanceled__DewPlayer(reader.ReadNetworkBehaviour<DewPlayer>());
		}
	}

	protected void UserCode_RpcInvokeVoteCompleted()
	{
		ClientEvent_OnVoteCompleted?.Invoke();
	}

	protected static void InvokeUserCode_RpcInvokeVoteCompleted(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcInvokeVoteCompleted called on server.");
		}
		else
		{
			((ZoneManager)obj).UserCode_RpcInvokeVoteCompleted();
		}
	}

	protected void UserCode_CmdTravelToNode__Int32__NetworkConnectionToClient(int index, NetworkConnectionToClient sender)
	{
		if (isInRoomTransition || index < 0 || index >= nodes.Count || !IsNodeConnected(currentNodeIndex, index) || isVoting)
		{
			return;
		}
		DewPlayer player = sender.GetPlayer();
		if (!(player == null))
		{
			if (ShouldVoteOnTravel())
			{
				StartVoteNextNode(player, index);
			}
			else
			{
				TravelToNode(index);
			}
		}
	}

	protected static void InvokeUserCode_CmdTravelToNode__Int32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTravelToNode called on client.");
		}
		else
		{
			((ZoneManager)obj).UserCode_CmdTravelToNode__Int32__NetworkConnectionToClient(reader.ReadInt(), senderConnection);
		}
	}

	protected void UserCode_CmdTravelToNextZone__NetworkConnectionToClient(NetworkConnectionToClient sender)
	{
		if (isInRoomTransition || currentNode.type != WorldNodeType.ExitBoss)
		{
			return;
		}
		DewPlayer player = sender.GetPlayer();
		if (!(player == null))
		{
			if (ShouldVoteOnTravel())
			{
				StartVoteNextZone(player);
			}
			else
			{
				NetworkedManagerBase<GameManager>.instance.LoadNextZone();
			}
		}
	}

	protected static void InvokeUserCode_CmdTravelToNextZone__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTravelToNextZone called on client.");
		}
		else
		{
			((ZoneManager)obj).UserCode_CmdTravelToNextZone__NetworkConnectionToClient(senderConnection);
		}
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteZone(currentZone);
			writer.WriteNetworkBehaviour(Network_003CcurrentRoom_003Ek__BackingField);
			writer.WriteInt(currentZoneIndex);
			writer.WriteInt(currentRoomIndex);
			writer.WriteInt(clearedCombatRooms);
			writer.WriteInt(currentZoneClearedNodes);
			writer.WriteBool(isInRoomTransition);
			writer.WriteInt(currentHuntLevel);
			writer.WriteBool(isVoting);
			writer.WriteInt(voteRemainingSeconds);
			GeneratedNetworkCode._Write_VoteType(writer, voteType);
			writer.WriteInt(voteData);
			writer.WriteInt(currentNodeIndex);
			writer.WriteInt(sidetrackReturnNodeIndex);
			writer.WriteBool(isHuntAdvanceDisabled);
			writer.WriteInt(hunterStartNodeIndex);
			writer.WriteInt(loopIndex);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteZone(currentZone);
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteNetworkBehaviour(Network_003CcurrentRoom_003Ek__BackingField);
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteInt(currentZoneIndex);
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteInt(currentRoomIndex);
		}
		if ((base.syncVarDirtyBits & 0x10L) != 0L)
		{
			writer.WriteInt(clearedCombatRooms);
		}
		if ((base.syncVarDirtyBits & 0x20L) != 0L)
		{
			writer.WriteInt(currentZoneClearedNodes);
		}
		if ((base.syncVarDirtyBits & 0x40L) != 0L)
		{
			writer.WriteBool(isInRoomTransition);
		}
		if ((base.syncVarDirtyBits & 0x80L) != 0L)
		{
			writer.WriteInt(currentHuntLevel);
		}
		if ((base.syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteBool(isVoting);
		}
		if ((base.syncVarDirtyBits & 0x200L) != 0L)
		{
			writer.WriteInt(voteRemainingSeconds);
		}
		if ((base.syncVarDirtyBits & 0x400L) != 0L)
		{
			GeneratedNetworkCode._Write_VoteType(writer, voteType);
		}
		if ((base.syncVarDirtyBits & 0x800L) != 0L)
		{
			writer.WriteInt(voteData);
		}
		if ((base.syncVarDirtyBits & 0x1000L) != 0L)
		{
			writer.WriteInt(currentNodeIndex);
		}
		if ((base.syncVarDirtyBits & 0x2000L) != 0L)
		{
			writer.WriteInt(sidetrackReturnNodeIndex);
		}
		if ((base.syncVarDirtyBits & 0x4000L) != 0L)
		{
			writer.WriteBool(isHuntAdvanceDisabled);
		}
		if ((base.syncVarDirtyBits & 0x8000L) != 0L)
		{
			writer.WriteInt(hunterStartNodeIndex);
		}
		if ((base.syncVarDirtyBits & 0x10000L) != 0L)
		{
			writer.WriteInt(loopIndex);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref currentZone, null, reader.ReadZone());
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref currentRoom, null, reader, ref ____003CcurrentRoom_003Ek__BackingFieldNetId);
			GeneratedSyncVarDeserialize(ref currentZoneIndex, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref currentRoomIndex, OnCurrentRoomIndexChanged, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref clearedCombatRooms, OnClearedCombatRoomsChanged, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref currentZoneClearedNodes, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref isInRoomTransition, OnIsInTransitionChanged, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref currentHuntLevel, OnCurrentHuntLevelChanged, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref isVoting, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref voteRemainingSeconds, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref voteType, null, GeneratedNetworkCode._Read_VoteType(reader));
			GeneratedSyncVarDeserialize(ref voteData, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref currentNodeIndex, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref sidetrackReturnNodeIndex, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref isHuntAdvanceDisabled, null, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref hunterStartNodeIndex, null, reader.ReadInt());
			GeneratedSyncVarDeserialize(ref loopIndex, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentZone, null, reader.ReadZone());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize_NetworkBehaviour(ref currentRoom, null, reader, ref ____003CcurrentRoom_003Ek__BackingFieldNetId);
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentZoneIndex, null, reader.ReadInt());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentRoomIndex, OnCurrentRoomIndexChanged, reader.ReadInt());
		}
		if ((num & 0x10L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref clearedCombatRooms, OnClearedCombatRoomsChanged, reader.ReadInt());
		}
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentZoneClearedNodes, null, reader.ReadInt());
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isInRoomTransition, OnIsInTransitionChanged, reader.ReadBool());
		}
		if ((num & 0x80L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentHuntLevel, OnCurrentHuntLevelChanged, reader.ReadInt());
		}
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isVoting, null, reader.ReadBool());
		}
		if ((num & 0x200L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref voteRemainingSeconds, null, reader.ReadInt());
		}
		if ((num & 0x400L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref voteType, null, GeneratedNetworkCode._Read_VoteType(reader));
		}
		if ((num & 0x800L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref voteData, null, reader.ReadInt());
		}
		if ((num & 0x1000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref currentNodeIndex, null, reader.ReadInt());
		}
		if ((num & 0x2000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref sidetrackReturnNodeIndex, null, reader.ReadInt());
		}
		if ((num & 0x4000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref isHuntAdvanceDisabled, null, reader.ReadBool());
		}
		if ((num & 0x8000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref hunterStartNodeIndex, null, reader.ReadInt());
		}
		if ((num & 0x10000L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref loopIndex, null, reader.ReadInt());
		}
	}
}
