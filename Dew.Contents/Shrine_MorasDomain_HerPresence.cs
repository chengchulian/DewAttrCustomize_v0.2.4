using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class Shrine_MorasDomain_HerPresence : Shrine, ICustomInteractable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass44_0
	{
		public UniTaskCompletionSource completionSource;

		internal void _003CWaveRoutine_003Eb__5(Actor _)
		{
			completionSource.TrySetResult();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaveRoutine_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public int waveIndex;

		public Shrine_MorasDomain_HerPresence _003C_003E4__this;

		private bool _003CisBossWave_003E5__2;

		private int _003CmonsterTypeCount_003E5__3;

		private float _003CpopMultiplier_003E5__4;

		private List<MonsterPool.SpawnRuleEntry> _003CbaseMonsterTypes_003E5__5;

		private int _003Ci_003E5__6;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaveRoutine_003Ed__44(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Shrine_MorasDomain_HerPresence CS_0024_003C_003E8__locals44 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CisBossWave_003E5__2 = waveIndex == CS_0024_003C_003E8__locals44.waveCount - 1;
				_003CmonsterTypeCount_003E5__3 = CS_0024_003C_003E8__locals44.monsterTypeCountsByWave[waveIndex];
				_003CpopMultiplier_003E5__4 = CS_0024_003C_003E8__locals44.populationMultipliersByWave[waveIndex];
				SingletonDewNetworkBehaviour<Room>.instance.monsters.FinishAllOngoingSpawns();
				if (CS_0024_003C_003E8__locals44._currentRule != null)
				{
					if (CS_0024_003C_003E8__locals44._currentRule.pool != null)
					{
						global::UnityEngine.Object.Destroy(CS_0024_003C_003E8__locals44._currentRule.pool);
					}
					global::UnityEngine.Object.Destroy(CS_0024_003C_003E8__locals44._currentRule);
				}
				CS_0024_003C_003E8__locals44.MakeUnavailable();
				if (_003CisBossWave_003E5__2)
				{
					CS_0024_003C_003E8__locals44.FxPlayNetworked(CS_0024_003C_003E8__locals44.fxSilence);
				}
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals44._currentRule = global::UnityEngine.Object.Instantiate(CS_0024_003C_003E8__locals44.singleWaveRule);
				CS_0024_003C_003E8__locals44._currentRule.pool = global::UnityEngine.Object.Instantiate(CS_0024_003C_003E8__locals44._allMonsters);
				_003CbaseMonsterTypes_003E5__5 = CS_0024_003C_003E8__locals44.GetRandomEntries(_003CmonsterTypeCount_003E5__3);
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__6 = 0;
				goto IL_02c8;
			case 3:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals44._currentRule.pool.entries = new List<MonsterPool.SpawnRuleEntry>(_003CbaseMonsterTypes_003E5__5.Take(_003Ci_003E5__6 + 1));
				_003C_003E2__current = SingletonDewNetworkBehaviour<Room>.instance.monsters.SpawnMonstersAsync(new SpawnMonsterSettings
				{
					rule = CS_0024_003C_003E8__locals44._currentRule,
					section = CS_0024_003C_003E8__locals44.mainSection,
					spawnPopulationMultiplier = _003CpopMultiplier_003E5__4 * CS_0024_003C_003E8__locals44.populationMultipliersBySubWave.GetClamped(_003Ci_003E5__6),
					ignoreTurnPopMultiplier = true,
					beforeSpawn = delegate(Entity e)
					{
						e.Visual.invisibleByDefault = true;
						e.Visual.skipSpawning = true;
					},
					spawnPosGetter = delegate
					{
						/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
					},
					afterSpawn = delegate
					{
					}
				}).ToCoroutine();
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(1.25f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003Ci_003E5__6++;
				goto IL_02c8;
			case 6:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals44.FxStopNetworked(CS_0024_003C_003E8__locals44.fxBossMusic);
				_003C_003E2__current = new WaitForSeconds(2f);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				goto IL_0430;
			case 8:
				{
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals44.nextWaveIndex == CS_0024_003C_003E8__locals44.waveCount)
					{
						CS_0024_003C_003E8__locals44.Break();
					}
					else
					{
						CS_0024_003C_003E8__locals44.MakeAvailable();
					}
					return false;
				}
				IL_02c8:
				if (_003Ci_003E5__6 < _003CmonsterTypeCount_003E5__3 && !(CS_0024_003C_003E8__locals44._currentRule == null))
				{
					if (_003Ci_003E5__6 == 0)
					{
						CS_0024_003C_003E8__locals44.FxPlayNetworked(CS_0024_003C_003E8__locals44.fxWaveStart);
					}
					else
					{
						CS_0024_003C_003E8__locals44.FxPlayNetworked(CS_0024_003C_003E8__locals44.fxSubWaveStart);
					}
					_003C_003E2__current = new WaitForSeconds(1.25f);
					_003C_003E1__state = 3;
					return true;
				}
				if (_003CisBossWave_003E5__2)
				{
					_003C_003Ec__DisplayClass44_0 CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass44_0();
					CS_0024_003C_003E8__locals44.FxPlayNetworked(CS_0024_003C_003E8__locals44.fxBossMusic);
					Monster monster = Dew.SpawnEntity(CS_0024_003C_003E8__locals44.availableBosses[(CS_0024_003C_003E8__locals44.bossIndexOverride < 0) ? global::UnityEngine.Random.Range(0, CS_0024_003C_003E8__locals44.availableBosses.Length) : CS_0024_003C_003E8__locals44.bossIndexOverride], CS_0024_003C_003E8__locals44.position, null, null, DewPlayer.creep, NetworkedManagerBase<GameManager>.instance.ambientLevel, delegate(Monster b)
					{
						if (b is BossMonster bossMonster)
						{
							bossMonster.skipBossSoulFlow = true;
						}
						b.Visual.skipSpawning = true;
						b.Visual.invisibleByDefault = true;
						b.Status.AddStatBonus(new StatBonus
						{
							maxHealthPercentage = -35f,
							attackDamagePercentage = 15f,
							abilityPowerPercentage = 15f,
							attackSpeedPercentage = 15f,
							abilityHastePercentage = 50f
						});
					});
					monster.Visual.EnableRenderers();
					CS_0024_003C_003E8__locals44.CreateStatusEffect<Se_MorasDomain_MorasCreation_Boss>(monster, default(CastInfo));
					CS_0024_003C_003E8__locals44.CreateStatusEffect(monster, default(CastInfo), delegate(Se_MorasDomain_AppearFromGround s)
					{
						s.NetworkskipDisappearAnim = true;
						s.onDisappearAnimFinish = (Action)Delegate.Combine(s.onDisappearAnimFinish, new Action(s.Appear));
					});
					CS_0024_003C_003E8__locals27.completionSource = new UniTaskCompletionSource();
					monster.ClientActorEvent_OnDestroyed += (Action<Actor>)delegate
					{
						CS_0024_003C_003E8__locals27.completionSource.TrySetResult();
					};
					_003C_003E2__current = CS_0024_003C_003E8__locals27.completionSource.Task.ToCoroutine();
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_0430;
				IL_0430:
				CS_0024_003C_003E8__locals44.SpawnRewards(waveIndex);
				_003C_003E2__current = new WaitForSeconds(2.5f);
				_003C_003E1__state = 8;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public GameObject fxBreakTap;

	public GameObject fxBreak;

	public GameObject fxSilence;

	public GameObject fxBossMusic;

	[SyncVar]
	private float _breakProgress;

	private float _lastBreakTapTime;

	private Coroutine _breakResetRoutine;

	public MonsterSpawnRule singleWaveRule;

	public RoomSection mainSection;

	public int waveCount;

	public int[] monsterTypeCountsByWave;

	public float[] populationMultipliersByWave;

	public Rarity[] rewardRarityByWave;

	public RoomRewardFlowItemType[] scrambledRewardPoolOnWaves;

	public float[] populationMultipliersBySubWave;

	public Monster[] availableBosses;

	public GameObject fxWaveStart;

	public GameObject fxSubWaveStart;

	public GameObject fxSpawnRewards;

	public int nextWaveIndex;

	public int bossIndexOverride;

	private MonsterPool _allMonsters;

	private MonsterSpawnRule _currentRule;

	private RoomRewardFlowItemType[] _rewardPoolByWave;

	public string nameRawText
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public string interactActionRawText
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public string interactAltActionRawText
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool canAltInteract
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public float? altInteractProgress
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public Cost cost
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public float Network_breakProgress
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
		[param: In]
		set
		{
		}
	}

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	public override void OnInteract(Entity entity, bool alt)
	{
	}

	private void DoWaveStart()
	{
	}

	[Server]
	private void DoBreakTap()
	{
	}

	[Server]
	private void Break()
	{
	}

	protected override bool OnUse(Entity entity)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private List<MonsterPool.SpawnRuleEntry> GetRandomEntries(int count)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[IteratorStateMachine(typeof(_003CWaveRoutine_003Ed__44))]
	private IEnumerator WaveRoutine(int waveIndex)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[Server]
	private void SpawnRewards(int waveIndex)
	{
	}

	private void OnCreate_Waves()
	{
	}

	private void OnDestroyActor_Waves()
	{
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
	}
}
