using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Sky_StellaMatter_AtkSub : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Sky_StellaMatter_AtkSub _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__8(int _003C_003E1__state)
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
			Ai_Mon_Sky_StellaMatter_AtkSub ai_Mon_Sky_StellaMatter_AtkSub = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Sky_StellaMatter_AtkSub.isServer)
				{
					return false;
				}
				ai_Mon_Sky_StellaMatter_AtkSub.FxPlayNetworked(ai_Mon_Sky_StellaMatter_AtkSub.telegraph, ai_Mon_Sky_StellaMatter_AtkSub.info.point, null);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Sky_StellaMatter_AtkSub.atkDelay);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				ai_Mon_Sky_StellaMatter_AtkSub.FxPlayNetworked(ai_Mon_Sky_StellaMatter_AtkSub.fxProjectile, ai_Mon_Sky_StellaMatter_AtkSub.info.point, null);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Sky_StellaMatter_AtkSub.projectileDelay);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Sky_StellaMatter_AtkSub.range.GetEntities(out handle, ai_Mon_Sky_StellaMatter_AtkSub.tvDefaultHarmfulEffectTargets);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Sky_StellaMatter_AtkSub.CreateDamage(DamageData.SourceType.Default, ai_Mon_Sky_StellaMatter_AtkSub.dmgFactor).Dispatch(entity);
					ai_Mon_Sky_StellaMatter_AtkSub.knockback.ApplyWithOrigin(ai_Mon_Sky_StellaMatter_AtkSub.info.point, entity);
					ai_Mon_Sky_StellaMatter_AtkSub.FxPlayNewNetworked(ai_Mon_Sky_StellaMatter_AtkSub.fxHit, entity);
				}
				handle.Return();
				ai_Mon_Sky_StellaMatter_AtkSub.Destroy();
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

	public float atkDelay;

	public float projectileDelay;

	public GameObject telegraph;

	public DewCollider range;

	public ScalingValue dmgFactor;

	public GameObject fxProjectile;

	public GameObject fxHit;

	public Knockback knockback;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__8))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
