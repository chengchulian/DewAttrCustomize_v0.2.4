using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_Mon_DarkCave_BossSeeker_TunnelVision : StatusEffect
{
	[Serializable]
	public struct Pattern
	{
		public OffsetAndAngle[] items;

		public float interval;
	}

	[Serializable]
	public struct OffsetAndAngle
	{
		public Vector3 offset;

		public float angle;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Vector3 center;

		internal void _003COnCreateSequenced_003Eb__1(Se_Mon_DarkCave_BossSeeker_Blink blink)
		{
			blink.customDestination = center;
		}
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Mon_DarkCave_BossSeeker_TunnelVision _003C_003E4__this;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

		private Hero _003Ctarget_003E5__4;

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
		public _003COnCreateSequenced_003Ed__7(int _003C_003E1__state)
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
			Se_Mon_DarkCave_BossSeeker_TunnelVision se_Mon_DarkCave_BossSeeker_TunnelVision = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
				if (!se_Mon_DarkCave_BossSeeker_TunnelVision.isServer)
				{
					return false;
				}
				se_Mon_DarkCave_BossSeeker_TunnelVision.Disappear();
				se_Mon_DarkCave_BossSeeker_TunnelVision.victim.Control.IncrementBlockCounters(Channel.BlockedAction.Everything);
				foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
				{
					se_Mon_DarkCave_BossSeeker_TunnelVision.CreateStatusEffect<Se_Mon_DarkCave_BossSeeker_TunnelVision_PlayerLight>(humanPlayer.hero);
				}
				_003C_003E2__current = new SI.WaitForSeconds(se_Mon_DarkCave_BossSeeker_TunnelVision.initDelay);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				int num2 = DewPlayer.humanPlayers.Count((DewPlayer p) => !p.hero.IsNullInactiveDeadOrKnockedOut());
				_003Ccount_003E5__2 = se_Mon_DarkCave_BossSeeker_TunnelVision.baseCount + se_Mon_DarkCave_BossSeeker_TunnelVision.perPlayerCount * num2;
				_003Ci_003E5__3 = 0;
				goto IL_030b;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				se_Mon_DarkCave_BossSeeker_TunnelVision.Appear();
				Quaternion value = Quaternion.LookRotation(_003Ctarget_003E5__4.agentPosition - se_Mon_DarkCave_BossSeeker_TunnelVision.victim.agentPosition);
				se_Mon_DarkCave_BossSeeker_TunnelVision.victim.Control.RotateTowards(_003Ctarget_003E5__4.agentPosition, immediately: false);
				se_Mon_DarkCave_BossSeeker_TunnelVision.CreateAbilityInstance<Ai_Mon_DarkCave_BossSeeker_TunnelVision_Claw>(se_Mon_DarkCave_BossSeeker_TunnelVision.victim.position, value, new CastInfo(se_Mon_DarkCave_BossSeeker_TunnelVision.victim, value.eulerAngles.y));
				if (_003Ci_003E5__3 < _003Ccount_003E5__2 - 1)
				{
					_003C_003E2__current = new SI.WaitForSeconds(global::UnityEngine.Random.Range(0.7f, 1.25f));
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_031c;
			}
			case 3:
				_003C_003E1__state = -1;
				se_Mon_DarkCave_BossSeeker_TunnelVision.Disappear();
				_003C_003E2__current = new SI.WaitForSeconds(global::UnityEngine.Random.Range(0.35f, 2.5f));
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003Ctarget_003E5__4 = null;
				_003Ci_003E5__3++;
				goto IL_030b;
			case 5:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1.center = ((SingletonBehaviour<DarkCave_BossRoomCenter>.instance == null) ? se_Mon_DarkCave_BossSeeker_TunnelVision.victim.position : SingletonBehaviour<DarkCave_BossRoomCenter>.instance.transform.position);
					se_Mon_DarkCave_BossSeeker_TunnelVision.CreateStatusEffect(se_Mon_DarkCave_BossSeeker_TunnelVision.victim, delegate(Se_Mon_DarkCave_BossSeeker_Blink blink)
					{
						blink.customDestination = _003C_003E8__1.center;
					});
					se_Mon_DarkCave_BossSeeker_TunnelVision.Destroy();
					return false;
				}
				IL_030b:
				if (_003Ci_003E5__3 < _003Ccount_003E5__2)
				{
					DewPlayer dewPlayer = Dew.SelectBestWithScore(DewPlayer.humanPlayers, (DewPlayer p, int _) => p.hero.IsNullInactiveDeadOrKnockedOut() ? float.NegativeInfinity : (0f - p.hero.Status.normalizedHealth), 0.15f);
					if (!dewPlayer.hero.IsNullInactiveDeadOrKnockedOut())
					{
						_003Ctarget_003E5__4 = dewPlayer.hero;
						float y = global::UnityEngine.Random.Range(120f, 240f);
						Vector3 end = AbilityTrigger.PredictPoint_Simple(global::UnityEngine.Random.value, _003Ctarget_003E5__4, 1f) + Quaternion.Euler(0f, y, 0f) * _003Ctarget_003E5__4.transform.forward * 3f;
						end = Dew.GetValidAgentDestination_LinearSweep(_003Ctarget_003E5__4.agentPosition, end);
						se_Mon_DarkCave_BossSeeker_TunnelVision.victim.Control.Teleport(end);
						_003C_003E2__current = new SI.WaitForSeconds(0.15f);
						_003C_003E1__state = 2;
						return true;
					}
				}
				goto IL_031c;
				IL_031c:
				_003C_003E2__current = new SI.WaitForSeconds(se_Mon_DarkCave_BossSeeker_TunnelVision.endDelay);
				_003C_003E1__state = 5;
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

	public int baseCount;

	public int perPlayerCount;

	public float initDelay;

	public float endDelay;

	public float postDaze;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnDestroyActor()
	{
	}

	private void Disappear()
	{
	}

	private void Appear()
	{
	}

	private void MirrorProcessed()
	{
	}
}
