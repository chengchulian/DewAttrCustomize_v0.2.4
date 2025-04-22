using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE _003C_003E4__this;

		private Dictionary<Entity, float> _003ClastHitTimes_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003COnCreateSequenced_003Ed__12(int _003C_003E1__state)
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
			Ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.isServer)
				{
					return false;
				}
				if (ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.playSizzleSound)
				{
					ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.FxPlayNetworked(ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.sizzleSound);
				}
				_003ClastHitTimes_003E5__2 = new Dictionary<Entity, float>();
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.tickCount)
			{
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.position, ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.radius, ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.tvDefaultHarmfulEffectTargets);
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					Entity entity = readOnlySpan[i];
					if (!_003ClastHitTimes_003E5__2.ContainsKey(entity) || !(Time.time - _003ClastHitTimes_003E5__2[entity] < ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.hitInterval))
					{
						_003ClastHitTimes_003E5__2[entity] = Time.time;
						ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.Damage(ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.tickDamage).Dispatch(entity);
						ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.FxPlayNewNetworked(ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.hitEffect, entity);
						if (ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.doSlow)
						{
							ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.CreateBasicEffect(entity, new SlowEffect
							{
								decay = ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.isSlowDecay,
								strength = ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.slowAmount
							}, ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.slowDuration, "artillery_slow", DuplicateEffectBehavior.UsePrevious);
						}
					}
				}
				handle.Return();
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.tickInterval);
				_003C_003E1__state = 1;
				return true;
			}
			ai_Mon_Despair_WretchedArtillery_BarrageAtk_AoE.Destroy();
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

	public float radius;

	public int tickCount;

	public float tickInterval;

	public ScalingValue tickDamage;

	public float hitInterval;

	public bool doSlow;

	public float slowAmount;

	public float slowDuration;

	public bool isSlowDecay;

	public GameObject hitEffect;

	public GameObject sizzleSound;

	[NonSerialized]
	public bool playSizzleSound;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__12))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
