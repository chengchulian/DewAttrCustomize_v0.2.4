using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_Curse_FaintMemory : CurseStatusEffect
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public AbilityLockHandle handle;

		public Se_Curse_FaintMemory _003C_003E4__this;
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Curse_FaintMemory _003C_003E4__this;

		private Hero _003Chero_003E5__2;

		private List<HeroSkillLocation> _003Cpool_003E5__3;

		private HeroSkillLocation _003ClastLocked_003E5__4;

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
		public _003COnCreateSequenced_003Ed__5(int _003C_003E1__state)
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
			Se_Curse_FaintMemory se_Curse_FaintMemory = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
				if (!se_Curse_FaintMemory.isServer)
				{
					return false;
				}
				_003Chero_003E5__2 = (Hero)se_Curse_FaintMemory.victim;
				_003Cpool_003E5__3 = new List<HeroSkillLocation>();
				_003ClastLocked_003E5__4 = HeroSkillLocation.Identity;
			}
			_003Cpool_003E5__3.Clear();
			if (_003Chero_003E5__2.Skill.Q != null)
			{
				_003Cpool_003E5__3.Add(HeroSkillLocation.Q);
			}
			if (_003Chero_003E5__2.Skill.W != null)
			{
				_003Cpool_003E5__3.Add(HeroSkillLocation.W);
			}
			if (_003Chero_003E5__2.Skill.E != null)
			{
				_003Cpool_003E5__3.Add(HeroSkillLocation.E);
			}
			if (_003Chero_003E5__2.Skill.R != null)
			{
				_003Cpool_003E5__3.Add(HeroSkillLocation.R);
			}
			_003Cpool_003E5__3.Remove(_003ClastLocked_003E5__4);
			_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals11;
			if (_003Cpool_003E5__3.Count > 0)
			{
				CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass5_0
				{
					_003C_003E4__this = se_Curse_FaintMemory,
					handle = _003Chero_003E5__2.Ability.GetNewAbilityLockHandle(shouldShowLockIcon: true)
				};
				se_Curse_FaintMemory._handles.Add(CS_0024_003C_003E8__locals11.handle);
				_003ClastLocked_003E5__4 = _003Cpool_003E5__3[global::UnityEngine.Random.Range(0, _003Cpool_003E5__3.Count)];
				CS_0024_003C_003E8__locals11.handle.LockAbilityCast((int)_003ClastLocked_003E5__4);
				CS_0024_003C_003E8__locals11.handle.LockAbilityEdit((int)_003ClastLocked_003E5__4);
				se_Curse_FaintMemory.FxPlayNewNetworked(se_Curse_FaintMemory.fxNewLock, se_Curse_FaintMemory.victim);
				se_Curse_FaintMemory.StartCoroutine(Routine());
			}
			else
			{
				_003ClastLocked_003E5__4 = HeroSkillLocation.Identity;
			}
			_003C_003E2__current = new SI.WaitForSeconds(se_Curse_FaintMemory.refreshTime);
			_003C_003E1__state = 1;
			return true;
			IEnumerator Routine()
			{
				yield return new WaitForSeconds(CS_0024_003C_003E8__locals11._003C_003E4__this.GetValue(CS_0024_003C_003E8__locals11._003C_003E4__this.sealDuration));
				CS_0024_003C_003E8__locals11.handle.Stop();
				CS_0024_003C_003E8__locals11._003C_003E4__this._handles.Remove(CS_0024_003C_003E8__locals11.handle);
				CS_0024_003C_003E8__locals11._003C_003E4__this.FxPlayNewNetworked(CS_0024_003C_003E8__locals11._003C_003E4__this.fxLockRemoved, CS_0024_003C_003E8__locals11._003C_003E4__this.victim);
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

	public GameObject fxNewLock;

	public GameObject fxLockRemoved;

	public float refreshTime;

	public float[] sealDuration;

	private List<AbilityLockHandle> _handles;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__5))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
