using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_GhostBlade_MeleeAtk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_GhostBlade_MeleeAtk _003C_003E4__this;

		private int _003Ccnt_003E5__2;

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
		public _003COnCreateSequenced_003Ed__10(int _003C_003E1__state)
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
			Ai_Mon_Ink_GhostBlade_MeleeAtk ai_Mon_Ink_GhostBlade_MeleeAtk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_GhostBlade_MeleeAtk.isServer)
				{
					return false;
				}
				ai_Mon_Ink_GhostBlade_MeleeAtk.DestroyOnDeath(ai_Mon_Ink_GhostBlade_MeleeAtk.info.caster);
				ai_Mon_Ink_GhostBlade_MeleeAtk._atkEffect = ai_Mon_Ink_GhostBlade_MeleeAtk.firstAtkEffect;
				if (ai_Mon_Ink_GhostBlade_MeleeAtk.damageDelay > 0f)
				{
					_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostBlade_MeleeAtk.damageDelay);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0084;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0084;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0084:
				_003Ccnt_003E5__2 = 0;
				break;
			}
			if (_003Ccnt_003E5__2 < ai_Mon_Ink_GhostBlade_MeleeAtk.atkCount)
			{
				_003Ccnt_003E5__2++;
				if (_003Ccnt_003E5__2 % 2 == 0)
				{
					ai_Mon_Ink_GhostBlade_MeleeAtk._atkEffect = ai_Mon_Ink_GhostBlade_MeleeAtk.secondAtkEffect;
				}
				ai_Mon_Ink_GhostBlade_MeleeAtk.FxPlayNewNetworked(ai_Mon_Ink_GhostBlade_MeleeAtk._atkEffect, ai_Mon_Ink_GhostBlade_MeleeAtk.info.caster);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_GhostBlade_MeleeAtk.range.GetEntities(out handle, ai_Mon_Ink_GhostBlade_MeleeAtk.hittable, ai_Mon_Ink_GhostBlade_MeleeAtk.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Ink_GhostBlade_MeleeAtk.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_GhostBlade_MeleeAtk.dmgFactor).Dispatch(entity);
					ai_Mon_Ink_GhostBlade_MeleeAtk.FxPlayNetworked(ai_Mon_Ink_GhostBlade_MeleeAtk.hitEffect, entity);
				}
				handle.Return();
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostBlade_MeleeAtk.atkInterval);
				_003C_003E1__state = 2;
				return true;
			}
			ai_Mon_Ink_GhostBlade_MeleeAtk.Destroy();
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

	public float atkInterval;

	public float damageDelay;

	public int atkCount;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public ScalingValue dmgFactor;

	public GameObject firstAtkEffect;

	public GameObject secondAtkEffect;

	public GameObject hitEffect;

	private GameObject _atkEffect;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__10))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
