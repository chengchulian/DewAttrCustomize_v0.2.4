using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Ai_Mon_Ink_GhostBlade_AtkSpawner : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public bool isSecondShot;

		internal void _003COnCreateSequenced_003Eb__0(Ai_Mon_Ink_GhostBlade_Atk b)
		{
			b.NetworkisFirst = !isSecondShot;
		}
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_GhostBlade_AtkSpawner _003C_003E4__this;

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
			Ai_Mon_Ink_GhostBlade_AtkSpawner ai_Mon_Ink_GhostBlade_AtkSpawner = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_GhostBlade_AtkSpawner.isServer)
				{
					return false;
				}
				ai_Mon_Ink_GhostBlade_AtkSpawner.DestroyOnDeath(ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostBlade_AtkSpawner.castDuration);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_0175;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0175;
			case 3:
				{
					_003C_003E1__state = -1;
					ai_Mon_Ink_GhostBlade_AtkSpawner.Destroy();
					return false;
				}
				IL_0175:
				if (_003Ci_003E5__2 < ai_Mon_Ink_GhostBlade_AtkSpawner.shootCount)
				{
					_003C_003Ec__DisplayClass4_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass4_0
					{
						isSecondShot = (_003Ci_003E5__2 % 2 != 0)
					};
					Ai_Mon_Ink_GhostBlade_Atk byType = DewResources.GetByType<Ai_Mon_Ink_GhostBlade_Atk>();
					float angle = AbilityTrigger.PredictAngle_SpeedAcceleration(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), ai_Mon_Ink_GhostBlade_AtkSpawner.info.target, ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster.position, 0f, byType.startInFrontDistance, byType.initialSpeed, byType.targetSpeed, byType.acceleration);
					ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster.Control.Rotate(angle, immediately: false);
					ai_Mon_Ink_GhostBlade_AtkSpawner.CreateAbilityInstance(ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster.position, null, new CastInfo(ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster, angle), delegate(Ai_Mon_Ink_GhostBlade_Atk b)
					{
						b.NetworkisFirst = !CS_0024_003C_003E8__locals1.isSecondShot;
					});
					_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostBlade_AtkSpawner.interval);
					_003C_003E1__state = 2;
					return true;
				}
				ai_Mon_Ink_GhostBlade_AtkSpawner.info.caster.Control.StartDaze(ai_Mon_Ink_GhostBlade_AtkSpawner.postDelay);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostBlade_AtkSpawner.postDelay);
				_003C_003E1__state = 3;
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

	public float castDuration;

	public float interval;

	public int shootCount;

	public float postDelay;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__4))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
