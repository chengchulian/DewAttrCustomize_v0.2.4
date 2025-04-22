using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_DarkCave_NightOlm_Atk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_DarkCave_NightOlm_Atk _003C_003E4__this;

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
			Ai_Mon_DarkCave_NightOlm_Atk CS_0024_003C_003E8__locals20 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals20.isServer)
				{
					return false;
				}
				CS_0024_003C_003E8__locals20.DestroyOnDeath(CS_0024_003C_003E8__locals20.info.caster);
				float num2 = (float)CS_0024_003C_003E8__locals20.spawnCount * CS_0024_003C_003E8__locals20.spawnInterval;
				if (!CS_0024_003C_003E8__locals20.cancelable)
				{
					CS_0024_003C_003E8__locals20.CreateBasicEffect(CS_0024_003C_003E8__locals20.info.caster, new UnstoppableEffect(), num2 + CS_0024_003C_003E8__locals20.postDaze);
				}
				CS_0024_003C_003E8__locals20.info.caster.Control.StartChannel(new Channel
				{
					duration = num2 + CS_0024_003C_003E8__locals20.postDaze,
					blockedActions = Channel.BlockedAction.Everything,
					onCancel = delegate
					{
					},
					onComplete = delegate
					{
					}
				});
				_003Ci_003E5__2 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < CS_0024_003C_003E8__locals20.spawnCount)
			{
				float x = CS_0024_003C_003E8__locals20.angleRange.x;
				float y = CS_0024_003C_003E8__locals20.angleRange.y;
				float num3 = Mathf.Lerp(x, y, (Mathf.Sin(Time.time * CS_0024_003C_003E8__locals20.angleSpeed) + 1f) * 0.5f);
				CS_0024_003C_003E8__locals20.CreateAbilityInstance<Ai_Mon_DarkCave_NightOlm_Atk_Projectile>(CS_0024_003C_003E8__locals20.info.caster.position, Quaternion.identity, new CastInfo(CS_0024_003C_003E8__locals20.info.caster, num3 + CS_0024_003C_003E8__locals20.info.angle));
				_003C_003E2__current = new SI.WaitForSeconds(CS_0024_003C_003E8__locals20.spawnInterval);
				_003C_003E1__state = 1;
				return true;
			}
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

	public int spawnCount;

	public float spawnInterval;

	public float postDaze;

	public bool cancelable;

	public Vector2 angleRange;

	public float angleSpeed;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__6))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
