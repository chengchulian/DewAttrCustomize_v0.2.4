using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_QR_Innocence_Spawner : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_QR_Innocence_Spawner _003C_003E4__this;

		private Hero_Bismuth _003Chero_003E5__2;

		private int _003Ccount_003E5__3;

		private List<Entity> _003Ctargets_003E5__4;

		private ListReturnHandle<Entity> _003Chandle_003E5__5;

		private Vector3 _003CtargetPoint_003E5__6;

		private int _003Ci_003E5__7;

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
		public _003COnCreateSequenced_003Ed__4(int _003C_003E1__state)
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
			Ai_QR_Innocence_Spawner ai_QR_Innocence_Spawner = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_QR_Innocence_Spawner.isServer)
				{
					return false;
				}
				ai_QR_Innocence_Spawner.DestroyOnDeath(ai_QR_Innocence_Spawner.info.caster);
				_003Chero_003E5__2 = ai_QR_Innocence_Spawner.FindFirstAncestorOfType<Hero_Bismuth>();
				_003Ccount_003E5__3 = ai_QR_Innocence_Spawner.clampedCount;
				_003Ctargets_003E5__4 = Hero_Bismuth.GetTargetEntities(out _003Chandle_003E5__5, ai_QR_Innocence_Spawner.info.caster, canBeNeutral: true, ai_QR_Innocence_Spawner.range.radius);
				_003CtargetPoint_003E5__6 = Dew.GetValidAgentDestination_LinearSweep(ai_QR_Innocence_Spawner.info.caster.agentPosition, ai_QR_Innocence_Spawner.info.caster.owner.cursorWorldPos);
				_003Ci_003E5__7 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__7++;
				break;
			}
			if (_003Ci_003E5__7 < _003Ccount_003E5__3)
			{
				if (_003Ctargets_003E5__4.Count > 0)
				{
					Entity entity = _003Ctargets_003E5__4[_003Ci_003E5__7 % _003Ctargets_003E5__4.Count];
					_003CtargetPoint_003E5__6 = entity.agentPosition;
				}
				if (_003Chero_003E5__2 != null)
				{
					_003Chero_003E5__2.book.RpcBookCast(Quaternion.LookRotation(_003CtargetPoint_003E5__6 - _003Chero_003E5__2.book.bookTransform.position) * Quaternion.Euler(-40f, 0f, 0f));
					_003Chero_003E5__2.SpendAttack();
				}
				Vector3 positionOnGround = Dew.GetPositionOnGround(_003CtargetPoint_003E5__6 + global::UnityEngine.Random.insideUnitSphere * 2f);
				ai_QR_Innocence_Spawner.CreateAbilityInstance<Ai_QR_Innocence_Projectile>(ai_QR_Innocence_Spawner.info.caster.agentPosition, null, new CastInfo(ai_QR_Innocence_Spawner.info.caster, positionOnGround));
				_003C_003E2__current = new SI.WaitForSeconds(0.4f / (float)_003Ccount_003E5__3);
				_003C_003E1__state = 1;
				return true;
			}
			_003Chandle_003E5__5.Return();
			ai_QR_Innocence_Spawner.Destroy();
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

	public DewCollider range;

	public ScalingValue countRaw;

	public int clampedCount
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__4))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
