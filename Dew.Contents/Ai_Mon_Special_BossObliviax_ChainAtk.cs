using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Ai_Mon_Special_BossObliviax_ChainAtk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Special_BossObliviax_ChainAtk _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__2(int _003C_003E1__state)
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
			Ai_Mon_Special_BossObliviax_ChainAtk ai_Mon_Special_BossObliviax_ChainAtk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_Special_BossObliviax_ChainAtk.isServer)
				{
					return false;
				}
				ai_Mon_Special_BossObliviax_ChainAtk.DestroyOnDeath(ai_Mon_Special_BossObliviax_ChainAtk.info.caster);
				Hero closestAliveHero = Dew.GetClosestAliveHero(ai_Mon_Special_BossObliviax_ChainAtk.info.caster.position);
				ai_Mon_Special_BossObliviax_ChainAtk.CreateAbilityInstance<Ai_Mon_Special_BossObliviax_ShadowWalk>(ai_Mon_Special_BossObliviax_ChainAtk.info.caster.position, null, new CastInfo(ai_Mon_Special_BossObliviax_ChainAtk.info.caster, closestAliveHero));
				ai_Mon_Special_BossObliviax_ChainAtk.info.caster.Ability.attackAbility.ResetCooldown();
				ai_Mon_Special_BossObliviax_ChainAtk.info.caster.Control.StartDaze(ai_Mon_Special_BossObliviax_ChainAtk.postDelay);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Special_BossObliviax_ChainAtk.postDelay);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				ai_Mon_Special_BossObliviax_ChainAtk.Destroy();
				return false;
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

	public float postDelay;

	public float castDuration;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__2))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
