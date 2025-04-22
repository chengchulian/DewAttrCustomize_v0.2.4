using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Se_R_LightningDance : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_R_LightningDance _003C_003E4__this;

		private Entity _003Ctarget_003E5__2;

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
		public _003COnCreateSequenced_003Ed__5(int _003C_003E1__state)
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
			Se_R_LightningDance se_R_LightningDance = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!se_R_LightningDance.isServer)
				{
					return false;
				}
				se_R_LightningDance.DoInvisible();
				se_R_LightningDance.DoInvulnerable();
				se_R_LightningDance.DoUncollidable();
				se_R_LightningDance.victim.Visual.DisableRenderers();
				_003C_003E2__current = new SI.WaitForSeconds(se_R_LightningDance.bounceInterval * 0.7f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Ctarget_003E5__2 = se_R_LightningDance.info.target;
				_003Ci_003E5__3 = 0;
				break;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < se_R_LightningDance.bounceCount)
			{
				se_R_LightningDance.victim.Control.StartDisplacement(new DispByDestination
				{
					destination = _003Ctarget_003E5__2.position + (_003Ctarget_003E5__2.position - se_R_LightningDance.victim.position).normalized * 0.4f,
					affectedByMovementSpeed = false,
					duration = se_R_LightningDance.bounceInterval,
					isFriendly = true,
					rotateForward = true,
					isCanceledByCC = false,
					canGoOverTerrain = true,
					ease = DewEase.EaseOutQuart
				});
				se_R_LightningDance.FxPlayNewNetworked(se_R_LightningDance.fxHit, _003Ctarget_003E5__2);
				se_R_LightningDance.Damage(se_R_LightningDance.damage).SetElemental(ElementalType.Light).Dispatch(_003Ctarget_003E5__2);
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, _003Ctarget_003E5__2.position, se_R_LightningDance.targetRadius, se_R_LightningDance.tvDefaultHarmfulEffectTargets);
				if (readOnlySpan.Length == 0)
				{
					ArrayReturnHandle<Entity> handle2;
					ReadOnlySpan<Entity> readOnlySpan2 = DewPhysics.OverlapCircleAllEntities(out handle2, _003Ctarget_003E5__2.position, se_R_LightningDance.targetRadius * 2f, se_R_LightningDance.tvDefaultHarmfulEffectTargets);
					if (readOnlySpan2.Length == 0)
					{
						handle.Return();
						handle2.Return();
						se_R_LightningDance.Destroy();
						return false;
					}
					_003Ctarget_003E5__2 = readOnlySpan2[global::UnityEngine.Random.Range(0, readOnlySpan2.Length)];
					handle2.Return();
				}
				else
				{
					_003Ctarget_003E5__2 = readOnlySpan[global::UnityEngine.Random.Range(0, readOnlySpan.Length)];
				}
				handle.Return();
				_003C_003E2__current = new SI.WaitForSeconds(se_R_LightningDance.bounceInterval);
				_003C_003E1__state = 2;
				return true;
			}
			se_R_LightningDance.Destroy();
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

	public ScalingValue damage;

	public GameObject fxHit;

	public int bounceCount;

	public float bounceInterval;

	public float targetRadius;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__5))]
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
