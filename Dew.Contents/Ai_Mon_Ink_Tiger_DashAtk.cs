using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_Tiger_DashAtk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_Tiger_DashAtk _003C_003E4__this;

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
			Ai_Mon_Ink_Tiger_DashAtk CS_0024_003C_003E8__locals27 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals27.isServer)
				{
					return false;
				}
				CS_0024_003C_003E8__locals27.FxPlayNetworked(CS_0024_003C_003E8__locals27.flyEffect, CS_0024_003C_003E8__locals27.info.caster);
				Vector3 end = CS_0024_003C_003E8__locals27.info.caster.agentPosition + CS_0024_003C_003E8__locals27.info.forward * CS_0024_003C_003E8__locals27.dashDis;
				end = Dew.GetValidAgentDestination_LinearSweep(CS_0024_003C_003E8__locals27.info.caster.agentPosition, end);
				CS_0024_003C_003E8__locals27.info.caster.Control.StartDaze(CS_0024_003C_003E8__locals27.dashDuration);
				CS_0024_003C_003E8__locals27.info.caster.Control.StartDisplacement(new DispByDestination
				{
					affectedByMovementSpeed = true,
					canGoOverTerrain = false,
					destination = end,
					duration = CS_0024_003C_003E8__locals27.dashDuration,
					ease = CS_0024_003C_003E8__locals27.ease,
					isCanceledByCC = false,
					isFriendly = true,
					rotateForward = true,
					onFinish = delegate
					{
					}
				});
				_003C_003E2__current = new SI.WaitForSeconds(CS_0024_003C_003E8__locals27.dashDuration);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals27.range.transform.position = CS_0024_003C_003E8__locals27.info.caster.agentPosition;
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> entities = CS_0024_003C_003E8__locals27.range.GetEntities(out handle, CS_0024_003C_003E8__locals27.hittable, CS_0024_003C_003E8__locals27.info.caster);
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					CS_0024_003C_003E8__locals27.FxPlayNewNetworked(CS_0024_003C_003E8__locals27.hitEffect, entity);
					CS_0024_003C_003E8__locals27.CreateBasicEffect(entity, new SlowEffect
					{
						strength = CS_0024_003C_003E8__locals27.slowStrength
					}, CS_0024_003C_003E8__locals27.slowDuration);
					CS_0024_003C_003E8__locals27.CreateDamage(DamageData.SourceType.Default, CS_0024_003C_003E8__locals27.dmgFactor).Dispatch(entity);
				}
				handle.Return();
				CS_0024_003C_003E8__locals27.Destroy();
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

	public float dashDis;

	public float dashDuration;

	public float landAfterDelay;

	public DewEase ease;

	public GameObject landEffect;

	public GameObject flyEffect;

	public DewCollider range;

	public AbilityTargetValidator hittable;

	public ScalingValue dmgFactor;

	public GameObject hitEffect;

	public float slowDuration;

	public float slowStrength;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__12))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
