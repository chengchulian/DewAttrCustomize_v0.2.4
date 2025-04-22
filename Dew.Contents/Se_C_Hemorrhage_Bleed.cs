using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_C_Hemorrhage_Bleed : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_C_Hemorrhage_Bleed _003C_003E4__this;

		private int _003CticksValue_003E5__2;

		private int _003Ci_003E5__3;

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
			Se_C_Hemorrhage_Bleed se_C_Hemorrhage_Bleed = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!se_C_Hemorrhage_Bleed.isServer)
				{
					return false;
				}
				se_C_Hemorrhage_Bleed.DoSlow(se_C_Hemorrhage_Bleed.slowAmount);
				_003CticksValue_003E5__2 = Mathf.RoundToInt(se_C_Hemorrhage_Bleed.GetValue(se_C_Hemorrhage_Bleed.ticks));
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				se_C_Hemorrhage_Bleed.Damage(se_C_Hemorrhage_Bleed.totalDamage, (_003Ci_003E5__3 == 0) ? se_C_Hemorrhage_Bleed.firstProcCoefficient : se_C_Hemorrhage_Bleed.subsequentProcCoefficient).ApplyRawMultiplier(1f / (float)_003CticksValue_003E5__2).ApplyStrength(se_C_Hemorrhage_Bleed.strength)
					.Dispatch(se_C_Hemorrhage_Bleed.victim);
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003CticksValue_003E5__2)
			{
				_003C_003E2__current = new SI.WaitForSeconds(se_C_Hemorrhage_Bleed.ticksInterval);
				_003C_003E1__state = 1;
				return true;
			}
			se_C_Hemorrhage_Bleed.Destroy();
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

	public ScalingValue totalDamage;

	public ScalingValue ticks;

	public float ticksInterval;

	public float firstProcCoefficient;

	public float subsequentProcCoefficient;

	public float slowAmount;

	[NonSerialized]
	public float strength;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
