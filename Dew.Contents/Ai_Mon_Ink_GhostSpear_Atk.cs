using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_GhostSpear_Atk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_GhostSpear_Atk _003C_003E4__this;

		private int _003Cindex_003E5__2;

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
		public _003COnCreateSequenced_003Ed__20(int _003C_003E1__state)
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
			Ai_Mon_Ink_GhostSpear_Atk ai_Mon_Ink_GhostSpear_Atk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Ink_GhostSpear_Atk.isServer)
				{
					return false;
				}
				ai_Mon_Ink_GhostSpear_Atk.DestroyOnDeath(ai_Mon_Ink_GhostSpear_Atk.info.caster);
				_003Cindex_003E5__2 = ((!(ai_Mon_Ink_GhostSpear_Atk.swingAtkChance > global::UnityEngine.Random.value)) ? 1 : 0);
				if (_003Cindex_003E5__2 == 0)
				{
					ai_Mon_Ink_GhostSpear_Atk._range = ai_Mon_Ink_GhostSpear_Atk.swingRange;
					ai_Mon_Ink_GhostSpear_Atk._startEffect = ai_Mon_Ink_GhostSpear_Atk.fxSwingStart;
					ai_Mon_Ink_GhostSpear_Atk._chargeEffect = ai_Mon_Ink_GhostSpear_Atk.fxChargeSwing;
					ai_Mon_Ink_GhostSpear_Atk._chargeDuration = ai_Mon_Ink_GhostSpear_Atk.swingChargeTime;
				}
				if (_003Cindex_003E5__2 == 1)
				{
					ai_Mon_Ink_GhostSpear_Atk._range = ai_Mon_Ink_GhostSpear_Atk.stingRange;
					ai_Mon_Ink_GhostSpear_Atk._startEffect = ai_Mon_Ink_GhostSpear_Atk.fxStingStart;
					ai_Mon_Ink_GhostSpear_Atk._chargeEffect = ai_Mon_Ink_GhostSpear_Atk.fxChargeSting;
					ai_Mon_Ink_GhostSpear_Atk._chargeDuration = ai_Mon_Ink_GhostSpear_Atk.stingChargeTime;
				}
				ai_Mon_Ink_GhostSpear_Atk.info.caster.Control.Rotate(ai_Mon_Ink_GhostSpear_Atk.info.forward, immediately: false);
				ai_Mon_Ink_GhostSpear_Atk.info.caster.Control.StartDaze(ai_Mon_Ink_GhostSpear_Atk._chargeDuration);
				ai_Mon_Ink_GhostSpear_Atk.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_GhostSpear_Atk.startAnim, 1f, (float)_003Cindex_003E5__2 * 0.5f);
				ai_Mon_Ink_GhostSpear_Atk.FxPlayNetworked(ai_Mon_Ink_GhostSpear_Atk._chargeEffect, ai_Mon_Ink_GhostSpear_Atk.info.caster);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostSpear_Atk._chargeDuration);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				ai_Mon_Ink_GhostSpear_Atk.info.caster.Animation.PlayAbilityAnimation(ai_Mon_Ink_GhostSpear_Atk.endAnim, 1f, (float)_003Cindex_003E5__2 * 0.5f);
				ai_Mon_Ink_GhostSpear_Atk.FxPlayNetworked(ai_Mon_Ink_GhostSpear_Atk._startEffect);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = ai_Mon_Ink_GhostSpear_Atk._range.GetEntities(out handle, ai_Mon_Ink_GhostSpear_Atk.hittable, ai_Mon_Ink_GhostSpear_Atk.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					ai_Mon_Ink_GhostSpear_Atk.FxPlayNewNetworked(ai_Mon_Ink_GhostSpear_Atk.hitEffect, entity);
					ai_Mon_Ink_GhostSpear_Atk.CreateDamage(DamageData.SourceType.Default, ai_Mon_Ink_GhostSpear_Atk.dmgFactor).Dispatch(entity);
				}
				handle.Return();
				ai_Mon_Ink_GhostSpear_Atk.info.caster.Control.StartDaze(ai_Mon_Ink_GhostSpear_Atk.postDelay);
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Ink_GhostSpear_Atk.postDelay);
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				ai_Mon_Ink_GhostSpear_Atk.Destroy();
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

	public DewCollider swingRange;

	public DewCollider stingRange;

	public ScalingValue dmgFactor;

	public AbilityTargetValidator hittable;

	public float delay;

	public float postDelay;

	public float stingChargeTime;

	public float swingChargeTime;

	public float swingAtkChance;

	public DewAnimationClip startAnim;

	public DewAnimationClip endAnim;

	public GameObject fxSwingStart;

	public GameObject fxStingStart;

	public GameObject fxChargeSwing;

	public GameObject fxChargeSting;

	public GameObject hitEffect;

	private DewCollider _range;

	private GameObject _chargeEffect;

	private GameObject _startEffect;

	private float _chargeDuration;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__20))]
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
