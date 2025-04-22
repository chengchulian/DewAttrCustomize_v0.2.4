using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_EgoSword_Atk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_EgoSword_Atk _003C_003E4__this;

		private Vector3 _003Cdest_003E5__2;

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
		public _003COnCreateSequenced_003Ed__17(int _003C_003E1__state)
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
			Ai_Mon_Ink_EgoSword_Atk ai_Mon_Ink_EgoSword_Atk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_EgoSword_Atk.isServer)
				{
					return false;
				}
				ai_Mon_Ink_EgoSword_Atk.DestroyOnDeath(ai_Mon_Ink_EgoSword_Atk.info.caster);
				ai_Mon_Ink_EgoSword_Atk.info.caster.Visual.DisableRenderers();
				ai_Mon_Ink_EgoSword_Atk.FxPlayNetworked(ai_Mon_Ink_EgoSword_Atk.telegraph, ai_Mon_Ink_EgoSword_Atk.info.point, Quaternion.identity);
				ai_Mon_Ink_EgoSword_Atk.CreateBasicEffect(ai_Mon_Ink_EgoSword_Atk.info.caster, new InvisibleEffect(), ai_Mon_Ink_EgoSword_Atk.spawnDuration + ai_Mon_Ink_EgoSword_Atk.spawnDelay + 0.1f);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_EgoSword_Atk.spawnDelay);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Cdest_003E5__2 = ai_Mon_Ink_EgoSword_Atk.info.point - ai_Mon_Ink_EgoSword_Atk.info.caster.agentPosition + ai_Mon_Ink_EgoSword_Atk.info.caster.agentPosition;
				ai_Mon_Ink_EgoSword_Atk.Teleport(ai_Mon_Ink_EgoSword_Atk.info.caster, _003Cdest_003E5__2);
				ai_Mon_Ink_EgoSword_Atk.range.transform.position = _003Cdest_003E5__2;
				ai_Mon_Ink_EgoSword_Atk.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_EgoSword_Atk.spawnAnimation);
				ai_Mon_Ink_EgoSword_Atk.info.caster.Visual.EnableRenderers();
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_EgoSword_Atk.spawnDuration);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				ai_Mon_Ink_EgoSword_Atk.FxPlayNewNetworked(ai_Mon_Ink_EgoSword_Atk.spawnEffect, Dew.GetPositionOnGround(_003Cdest_003E5__2), Quaternion.identity);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_EgoSword_Atk.range.GetEntities(out handle, ai_Mon_Ink_EgoSword_Atk.hittable, ai_Mon_Ink_EgoSword_Atk.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Ink_EgoSword_Atk.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_EgoSword_Atk.dmgFactor).Dispatch(entity);
					ai_Mon_Ink_EgoSword_Atk.FxPlayNewNetworked(ai_Mon_Ink_EgoSword_Atk.hitEffect, entity);
					ai_Mon_Ink_EgoSword_Atk.knockback.ApplyWithOrigin(ai_Mon_Ink_EgoSword_Atk.info.caster.agentPosition, entity);
				}
				handle.Return();
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_EgoSword_Atk.respawnDelay);
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				ai_Mon_Ink_EgoSword_Atk.FxPlayNetworked(ai_Mon_Ink_EgoSword_Atk.fxSecondAtkCharge, ai_Mon_Ink_EgoSword_Atk.info.caster);
				ai_Mon_Ink_EgoSword_Atk.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_EgoSword_Atk.animRespawnStart);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_EgoSword_Atk.fxDelay);
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				ai_Mon_Ink_EgoSword_Atk.FxPlayNewNetworked(ai_Mon_Ink_EgoSword_Atk.fxSecondAtkReady, ai_Mon_Ink_EgoSword_Atk.info.caster);
				ai_Mon_Ink_EgoSword_Atk.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_EgoSword_Atk.animRespawnEnd);
				Hero target = Dew.SelectRandomAliveHero();
				Vector3 end = AbilityTrigger.PredictPoint_Simple(NetworkedManagerBase<GameManager>.instance.GetPredictionStrength(), target, ai_Mon_Ink_EgoSword_Atk.spawnDelay);
				end = Dew.GetValidAgentDestination_LinearSweep(ai_Mon_Ink_EgoSword_Atk.info.caster.agentPosition, end);
				ai_Mon_Ink_EgoSword_Atk.info.caster.CreateAbilityInstance<Ai_Mon_Ink_EgoSword_Atk>(ai_Mon_Ink_EgoSword_Atk.info.caster.agentPosition, null, new CastInfo(ai_Mon_Ink_EgoSword_Atk.info.caster, end));
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_EgoSword_Atk.postDelay);
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				ai_Mon_Ink_EgoSword_Atk.Destroy();
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

	public float spawnDelay;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public ScalingValue dmgFactor;

	public Knockback knockback;

	public GameObject telegraph;

	public DewAnimationClip spawnAnimation;

	public DewAnimationClip animRespawnStart;

	public DewAnimationClip animRespawnEnd;

	public float spawnDuration;

	public GameObject spawnEffect;

	public GameObject hitEffect;

	public float respawnDelay;

	public float fxDelay;

	public float postDelay;

	public GameObject fxSecondAtkCharge;

	public GameObject fxSecondAtkReady;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__17))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}
}
