using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Despair_Displacer_SpawnEgg : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Despair_Displacer_SpawnEgg _003C_003E4__this;

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
			Ai_Mon_Despair_Displacer_SpawnEgg ai_Mon_Despair_Displacer_SpawnEgg = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Despair_Displacer_SpawnEgg.isServer)
				{
					return false;
				}
				ai_Mon_Despair_Displacer_SpawnEgg.FxPlayNetworked(ai_Mon_Despair_Displacer_SpawnEgg.fxEgg);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Despair_Displacer_SpawnEgg.delay);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				ai_Mon_Despair_Displacer_SpawnEgg.FxPlayNetworked(ai_Mon_Despair_Displacer_SpawnEgg.fxTelegraph);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Despair_Displacer_SpawnEgg.duration);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				ai_Mon_Despair_Displacer_SpawnEgg.FxPlayNetworked(ai_Mon_Despair_Displacer_SpawnEgg.fxExplode);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Despair_Displacer_SpawnEgg.range.GetEntities(out handle, ai_Mon_Despair_Displacer_SpawnEgg.tvDefaultHarmfulEffectTargets);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Despair_Displacer_SpawnEgg.FxPlayNewNetworked(ai_Mon_Despair_Displacer_SpawnEgg.fxHit, entity);
					ai_Mon_Despair_Displacer_SpawnEgg.CreateDamage(DamageData.SourceType.Default, ai_Mon_Despair_Displacer_SpawnEgg.dmgFactor).SetOriginPosition(ai_Mon_Despair_Displacer_SpawnEgg.transform.position).Dispatch(entity);
					ai_Mon_Despair_Displacer_SpawnEgg.knockback.ApplyWithOrigin(ai_Mon_Despair_Displacer_SpawnEgg.transform.position, entity);
					entity.Visual.KnockUp(ai_Mon_Despair_Displacer_SpawnEgg.knockUpStrength, isFriendly: false);
				}
				handle.Return();
				ai_Mon_Despair_Displacer_SpawnEgg.Destroy();
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

	public float delay;

	public float duration;

	public DewCollider range;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public KnockUpStrength knockUpStrength;

	public GameObject fxEgg;

	public GameObject fxTelegraph;

	public GameObject fxHit;

	public GameObject fxExplode;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__10))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
