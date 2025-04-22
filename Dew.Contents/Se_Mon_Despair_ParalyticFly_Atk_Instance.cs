using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Se_Mon_Despair_ParalyticFly_Atk_Instance : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Mon_Despair_ParalyticFly_Atk_Instance _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__6(int _003C_003E1__state)
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
			Se_Mon_Despair_ParalyticFly_Atk_Instance se_Mon_Despair_ParalyticFly_Atk_Instance = _003C_003E4__this;
			if (num != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
			if (!se_Mon_Despair_ParalyticFly_Atk_Instance.isServer)
			{
				return false;
			}
			se_Mon_Despair_ParalyticFly_Atk_Instance.DoSlow(se_Mon_Despair_ParalyticFly_Atk_Instance.slowAmount).decay = se_Mon_Despair_ParalyticFly_Atk_Instance.isDecay;
			se_Mon_Despair_ParalyticFly_Atk_Instance.SetTimer(se_Mon_Despair_ParalyticFly_Atk_Instance.slowDuration);
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

	public float slowAmount;

	public float slowDuration;

	public bool isDecay;

	public ScalingValue tickDmgFactor;

	public float tickDuration;

	private float _time;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__6))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	private void MirrorProcessed()
	{
	}
}
