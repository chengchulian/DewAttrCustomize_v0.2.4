using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_DivineAnimal_Missile_Sub : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_DivineAnimal_Missile_Sub _003C_003E4__this;

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
			Ai_Mon_Ink_DivineAnimal_Missile_Sub ai_Mon_Ink_DivineAnimal_Missile_Sub = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_DivineAnimal_Missile_Sub.isServer)
				{
					return false;
				}
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_DivineAnimal_Missile_Sub.startDelay);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				ai_Mon_Ink_DivineAnimal_Missile_Sub.FxPlayNetworked(ai_Mon_Ink_DivineAnimal_Missile_Sub.skillEffect);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_DivineAnimal_Missile_Sub.range.GetEntities(out handle, ai_Mon_Ink_DivineAnimal_Missile_Sub.hittable, ai_Mon_Ink_DivineAnimal_Missile_Sub.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Ink_DivineAnimal_Missile_Sub.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_DivineAnimal_Missile_Sub.dmgFactor).Dispatch(entity);
					ai_Mon_Ink_DivineAnimal_Missile_Sub.FxPlayNetworked(ai_Mon_Ink_DivineAnimal_Missile_Sub.hitEffect, entity);
				}
				handle.Return();
				ai_Mon_Ink_DivineAnimal_Missile_Sub.Destroy();
				return false;
			}
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

	public GameObject skillEffect;

	public GameObject hitEffect;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public ScalingValue dmgFactor;

	public float startDelay;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__6))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
