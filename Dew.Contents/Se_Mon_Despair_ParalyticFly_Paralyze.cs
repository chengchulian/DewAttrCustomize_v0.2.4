using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_Mon_Despair_ParalyticFly_Paralyze : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Mon_Despair_ParalyticFly_Paralyze _003C_003E4__this;

		private int _003Ci_003E5__2;

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
			Se_Mon_Despair_ParalyticFly_Paralyze se_Mon_Despair_ParalyticFly_Paralyze = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!se_Mon_Despair_ParalyticFly_Paralyze.isServer)
				{
					return false;
				}
				se_Mon_Despair_ParalyticFly_Paralyze.DoSlow(se_Mon_Despair_ParalyticFly_Paralyze.slowAmount);
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < se_Mon_Despair_ParalyticFly_Paralyze.tickCount)
			{
				se_Mon_Despair_ParalyticFly_Paralyze.DefaultDamage(se_Mon_Despair_ParalyticFly_Paralyze.tickDamage).Dispatch(se_Mon_Despair_ParalyticFly_Paralyze.victim);
				se_Mon_Despair_ParalyticFly_Paralyze.FxPlayNewNetworked(se_Mon_Despair_ParalyticFly_Paralyze.tickHitEffect, se_Mon_Despair_ParalyticFly_Paralyze.victim);
				_003C_003E2__current = new SI.WaitForSeconds(se_Mon_Despair_ParalyticFly_Paralyze.tickInterval);
				_003C_003E1__state = 1;
				return true;
			}
			se_Mon_Despair_ParalyticFly_Paralyze.DefaultDamage(se_Mon_Despair_ParalyticFly_Paralyze.endDamage).Dispatch(se_Mon_Despair_ParalyticFly_Paralyze.victim);
			se_Mon_Despair_ParalyticFly_Paralyze.CreateBasicEffect(se_Mon_Despair_ParalyticFly_Paralyze.victim, new StunEffect(), se_Mon_Despair_ParalyticFly_Paralyze.stunDuration, "paralyticfly_stun");
			se_Mon_Despair_ParalyticFly_Paralyze.Destroy();
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

	public int tickCount;

	public float slowAmount;

	public float tickInterval;

	public ScalingValue tickDamage;

	public ScalingValue endDamage;

	public GameObject tickHitEffect;

	public float stunDuration;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
