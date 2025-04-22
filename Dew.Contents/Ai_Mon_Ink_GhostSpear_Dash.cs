using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_GhostSpear_Dash : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_GhostSpear_Dash _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__4(int _003C_003E1__state)
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
			Ai_Mon_Ink_GhostSpear_Dash CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			if (num != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
			if (!CS_0024_003C_003E8__locals12.isServer)
			{
				return false;
			}
			CS_0024_003C_003E8__locals12.DestroyOnDeath(CS_0024_003C_003E8__locals12.info.caster);
			Vector3 validAgentDestination_LinearSweep = Dew.GetValidAgentDestination_LinearSweep(CS_0024_003C_003E8__locals12.info.caster.agentPosition, CS_0024_003C_003E8__locals12.info.caster.agentPosition + CS_0024_003C_003E8__locals12.info.forward * CS_0024_003C_003E8__locals12.dashDistance);
			CS_0024_003C_003E8__locals12.info.caster.Control.StartDaze(CS_0024_003C_003E8__locals12.dashDuration);
			CS_0024_003C_003E8__locals12.info.caster.Control.StartDisplacement(new DispByDestination
			{
				affectedByMovementSpeed = false,
				canGoOverTerrain = false,
				destination = validAgentDestination_LinearSweep,
				duration = CS_0024_003C_003E8__locals12.dashDuration,
				ease = CS_0024_003C_003E8__locals12.ease,
				onCancel = delegate
				{
				},
				onFinish = delegate
				{
				},
				rotateForward = true
			});
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

	public float dashDuration;

	public float dashDistance;

	public DewEase ease;

	public GameObject finishEffect;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__4))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
