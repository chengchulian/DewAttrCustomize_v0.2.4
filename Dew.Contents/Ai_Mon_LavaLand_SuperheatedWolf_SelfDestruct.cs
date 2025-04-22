using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct _003C_003E4__this;

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
		public _003COnCreateSequenced_003Ed__9(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct._003C_003En__0();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (!ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.isServer)
				{
					return false;
				}
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.CreateBasicEffect(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster, new UnstoppableEffect(), float.PositiveInfinity);
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.DestroyOnDeath(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster);
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.FxPlayNetworked(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.fxTelegraph, ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.explodeDelay - 0.2f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster.Control.StartDaze(10f);
				_003C_003E2__current = new SI.WaitForSeconds(0.2f);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster.Visual.DisableRenderers();
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.FxPlayNetworked(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.fxExplode);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.range.GetEntities(out handle, ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.tvDefaultHarmfulEffectTargets);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.CreateDamage(DamageData.SourceType.Default, ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.dmgFactor).SetElemental(ElementalType.Fire).Dispatch(entity);
					ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.knockback.ApplyWithOrigin(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster.position, entity);
					ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.FxPlayNewNetworked(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.fxHit, entity);
				}
				handle.Return();
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.FxPlayNetworked(ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.fxDeathOnCaster, ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster);
				ai_Mon_LavaLand_SuperheatedWolf_SelfDestruct.info.caster.Kill();
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

	public GameObject fxTelegraph;

	public GameObject fxHit;

	public GameObject fxExplode;

	public GameObject fxDeathOnCaster;

	public DewAnimationClip animStart;

	public DewCollider range;

	public Knockback knockback;

	public float explodeDelay;

	public ScalingValue dmgFactor;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__9))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void ActiveFrameUpdate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _003C_003En__0()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
