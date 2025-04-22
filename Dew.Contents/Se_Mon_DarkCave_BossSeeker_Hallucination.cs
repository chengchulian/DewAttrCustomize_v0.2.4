using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_Mon_DarkCave_BossSeeker_Hallucination : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public Vector3 center;

		internal void _003COnCreateSequenced_003Eb__1(Se_Mon_DarkCave_BossSeeker_Blink blink)
		{
			blink.customDestination = center;
		}
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Mon_DarkCave_BossSeeker_Hallucination _003C_003E4__this;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private Se_Mon_DarkCave_BossSeeker_Hallucination_Disappear _003Cdisappear_003E5__2;

		private float _003Cangle_003E5__3;

		private int _003CrealIndex_003E5__4;

		private Entity[] _003Cclones_003E5__5;

		private float _003CstartRealHealth_003E5__6;

		private bool _003CshouldStagger_003E5__7;

		private int _003Ci_003E5__8;

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
		public _003COnCreateSequenced_003Ed__16(int _003C_003E1__state)
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
			Se_Mon_DarkCave_BossSeeker_Hallucination se_Mon_DarkCave_BossSeeker_Hallucination = _003C_003E4__this;
			Entity[] array;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0();
				if (!se_Mon_DarkCave_BossSeeker_Hallucination.isServer)
				{
					return false;
				}
				se_Mon_DarkCave_BossSeeker_Hallucination.DoUnstoppable();
				se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.StartDaze(se_Mon_DarkCave_BossSeeker_Hallucination.disappearDuration + se_Mon_DarkCave_BossSeeker_Hallucination.dispDuration);
				_003Cdisappear_003E5__2 = se_Mon_DarkCave_BossSeeker_Hallucination.CreateStatusEffect<Se_Mon_DarkCave_BossSeeker_Hallucination_Disappear>(se_Mon_DarkCave_BossSeeker_Hallucination.victim);
				_003C_003E2__current = new SI.WaitForSeconds(se_Mon_DarkCave_BossSeeker_Hallucination.disappearDuration - 0.25f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.center = ((SingletonBehaviour<DarkCave_BossRoomCenter>.instance == null) ? se_Mon_DarkCave_BossSeeker_Hallucination.victim.position : SingletonBehaviour<DarkCave_BossRoomCenter>.instance.transform.position);
				_003Cangle_003E5__3 = global::UnityEngine.Random.Range(0f, 360f);
				se_Mon_DarkCave_BossSeeker_Hallucination.Teleport(se_Mon_DarkCave_BossSeeker_Hallucination.victim, _003C_003E8__1.center + Quaternion.Euler(0f, _003Cangle_003E5__3, 0f) * Vector3.forward * se_Mon_DarkCave_BossSeeker_Hallucination.radiusFromCenter);
				se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.RotateTowards(_003C_003E8__1.center, immediately: true, 1f);
				_003C_003E2__current = new SI.WaitForSeconds(0.25f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CrealIndex_003E5__4 = global::UnityEngine.Random.Range(0, se_Mon_DarkCave_BossSeeker_Hallucination.clonesCount + 1);
				_003Cclones_003E5__5 = new Entity[se_Mon_DarkCave_BossSeeker_Hallucination.clonesCount];
				_003Ci_003E5__8 = 0;
				goto IL_04e6;
			case 3:
				_003C_003E1__state = -1;
				goto IL_04d4;
			case 4:
				_003C_003E1__state = -1;
				goto IL_04d4;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0511;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_04d4:
				_003Ci_003E5__8++;
				goto IL_04e6;
				IL_04e6:
				if (_003Ci_003E5__8 < se_Mon_DarkCave_BossSeeker_Hallucination.clonesCount + 1)
				{
					if (_003Ci_003E5__8 == _003CrealIndex_003E5__4)
					{
						_003Cdisappear_003E5__2.Destroy();
						se_Mon_DarkCave_BossSeeker_Hallucination.FxPlayNetworked(se_Mon_DarkCave_BossSeeker_Hallucination.fxAppearReal, se_Mon_DarkCave_BossSeeker_Hallucination.victim);
						se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.StartDaze(se_Mon_DarkCave_BossSeeker_Hallucination.dispDuration + 1f);
						se_Mon_DarkCave_BossSeeker_Hallucination.victim.Animation.PlayAbilityAnimation(se_Mon_DarkCave_BossSeeker_Hallucination.animAppear);
						se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.StartDisplacement(new DispByDestination
						{
							affectedByMovementSpeed = false,
							canGoOverTerrain = true,
							destination = Dew.GetValidAgentPosition(se_Mon_DarkCave_BossSeeker_Hallucination.victim.position + (se_Mon_DarkCave_BossSeeker_Hallucination.victim.position - _003C_003E8__1.center).Flattened().normalized * se_Mon_DarkCave_BossSeeker_Hallucination.dispAwayDistance),
							duration = se_Mon_DarkCave_BossSeeker_Hallucination.dispDuration,
							ease = se_Mon_DarkCave_BossSeeker_Hallucination.dispEase,
							isCanceledByCC = false,
							isFriendly = true,
							rotateForward = false
						});
						_003C_003E2__current = new SI.WaitForSeconds(0.15f);
						_003C_003E1__state = 3;
						return true;
					}
					int num2 = _003Ci_003E5__8;
					if (_003Ci_003E5__8 > _003CrealIndex_003E5__4)
					{
						num2--;
					}
					_003Cangle_003E5__3 += 360f / (float)(se_Mon_DarkCave_BossSeeker_Hallucination.clonesCount + 1);
					Vector3 vector = _003C_003E8__1.center + Quaternion.Euler(0f, _003Cangle_003E5__3, 0f) * Vector3.forward * se_Mon_DarkCave_BossSeeker_Hallucination.radiusFromCenter;
					Mon_DarkCave_SeekerHallucination mon_DarkCave_SeekerHallucination = Dew.SpawnEntity<Mon_DarkCave_SeekerHallucination>(vector, Quaternion.LookRotation(_003C_003E8__1.center - vector).Flattened(), se_Mon_DarkCave_BossSeeker_Hallucination, DewPlayer.creep, se_Mon_DarkCave_BossSeeker_Hallucination.info.caster.level);
					mon_DarkCave_SeekerHallucination.Status.SetHealth(se_Mon_DarkCave_BossSeeker_Hallucination.victim.normalizedHealth * mon_DarkCave_SeekerHallucination.maxHealth);
					mon_DarkCave_SeekerHallucination.destroyHpThreshold = mon_DarkCave_SeekerHallucination.normalizedHealth - se_Mon_DarkCave_BossSeeker_Hallucination.cloneDestroyDamageThreshold;
					mon_DarkCave_SeekerHallucination.Control.StartDaze(se_Mon_DarkCave_BossSeeker_Hallucination.dispDuration + 1f);
					mon_DarkCave_SeekerHallucination.Control.StartDisplacement(new DispByDestination
					{
						affectedByMovementSpeed = false,
						canGoOverTerrain = true,
						destination = Dew.GetValidAgentPosition(mon_DarkCave_SeekerHallucination.position + (mon_DarkCave_SeekerHallucination.position - _003C_003E8__1.center).Flattened().normalized * se_Mon_DarkCave_BossSeeker_Hallucination.dispAwayDistance),
						duration = se_Mon_DarkCave_BossSeeker_Hallucination.dispDuration,
						ease = se_Mon_DarkCave_BossSeeker_Hallucination.dispEase,
						isCanceledByCC = false,
						isFriendly = true,
						rotateForward = false
					});
					se_Mon_DarkCave_BossSeeker_Hallucination.FxPlayNewNetworked(se_Mon_DarkCave_BossSeeker_Hallucination.fxAppearClone, mon_DarkCave_SeekerHallucination);
					mon_DarkCave_SeekerHallucination.Animation.PlayAbilityAnimation(se_Mon_DarkCave_BossSeeker_Hallucination.animAppear);
					_003Cclones_003E5__5[num2] = mon_DarkCave_SeekerHallucination;
					_003C_003E2__current = new SI.WaitForSeconds(0.15f);
					_003C_003E1__state = 4;
					return true;
				}
				_003CstartRealHealth_003E5__6 = se_Mon_DarkCave_BossSeeker_Hallucination.victim.normalizedHealth;
				_003CshouldStagger_003E5__7 = false;
				goto IL_0511;
				IL_0511:
				if (!_003Cclones_003E5__5.All((Entity c) => c.IsNullOrInactive()))
				{
					if (!(_003CstartRealHealth_003E5__6 - se_Mon_DarkCave_BossSeeker_Hallucination.victim.normalizedHealth > se_Mon_DarkCave_BossSeeker_Hallucination.realDamageThreshold))
					{
						_003C_003E2__current = new SI.WaitForSeconds(0.25f);
						_003C_003E1__state = 5;
						return true;
					}
					_003CshouldStagger_003E5__7 = true;
				}
				array = _003Cclones_003E5__5;
				foreach (Entity entity in array)
				{
					if (!entity.IsNullInactiveDeadOrKnockedOut())
					{
						entity.Kill();
					}
				}
				se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.CancelOngoingChannels();
				se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.Stop();
				if (_003CshouldStagger_003E5__7)
				{
					se_Mon_DarkCave_BossSeeker_Hallucination.victim.Animation.PlayAbilityAnimation(se_Mon_DarkCave_BossSeeker_Hallucination.animStagger);
					se_Mon_DarkCave_BossSeeker_Hallucination.victim.Control.StartDaze(se_Mon_DarkCave_BossSeeker_Hallucination.staggerDaze);
					se_Mon_DarkCave_BossSeeker_Hallucination.FxPlayNetworked(se_Mon_DarkCave_BossSeeker_Hallucination.fxStagger, se_Mon_DarkCave_BossSeeker_Hallucination.victim);
					_003C_003E2__current = new SI.WaitForSeconds(se_Mon_DarkCave_BossSeeker_Hallucination.staggerDaze);
					_003C_003E1__state = 6;
					return true;
				}
				break;
			}
			se_Mon_DarkCave_BossSeeker_Hallucination.CreateStatusEffect(se_Mon_DarkCave_BossSeeker_Hallucination.victim, delegate(Se_Mon_DarkCave_BossSeeker_Blink blink)
			{
				blink.customDestination = _003C_003E8__1.center;
			});
			se_Mon_DarkCave_BossSeeker_Hallucination.Destroy();
			return false;
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

	public float disappearDuration;

	public int clonesCount;

	public float radiusFromCenter;

	public GameObject fxAppearReal;

	public GameObject fxAppearClone;

	public DewAnimationClip animAppear;

	public float cloneDestroyDamageThreshold;

	public float realDamageThreshold;

	public float dispDuration;

	public float dispDurationDiff;

	public float dispAwayDistance;

	public DewEase dispEase;

	public DewAnimationClip animStagger;

	public GameObject fxStagger;

	public float staggerDaze;

	private bool _isSpinning;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__16))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
