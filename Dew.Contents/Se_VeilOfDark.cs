using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;

public class Se_VeilOfDark : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_VeilOfDark _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__1(int _003C_003E1__state)
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
			Se_VeilOfDark se_VeilOfDark = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				se_VeilOfDark.PopUpBlindMessage();
			}
			else
			{
				_003C_003E1__state = -1;
				if (!se_VeilOfDark.isServer)
				{
					return false;
				}
				se_VeilOfDark.SetTimer(se_VeilOfDark.duration);
			}
			_003C_003E2__current = new SI.WaitForSeconds(0.35f);
			_003C_003E1__state = 1;
			return true;
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

	public float duration;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__1))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[ClientRpc]
	private void PopUpBlindMessage()
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_PopUpBlindMessage()
	{
	}

	protected static void InvokeUserCode_PopUpBlindMessage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static Se_VeilOfDark()
	{
	}
}
