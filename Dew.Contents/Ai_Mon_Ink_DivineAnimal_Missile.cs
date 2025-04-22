using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Ink_DivineAnimal_Missile : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Ink_DivineAnimal_Missile _003C_003E4__this;

		private int _003Ccount_003E5__2;

		private int _003CcalCount_003E5__3;

		private List<Vector3>.Enumerator _003C_003E7__wrap3;

		private Vector3 _003Cpos_003E5__5;

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
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Ai_Mon_Ink_DivineAnimal_Missile CS_0024_003C_003E8__locals24 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals24.isServer)
					{
						return false;
					}
					CS_0024_003C_003E8__locals24.DestroyOnDeath(CS_0024_003C_003E8__locals24.info.caster);
					CS_0024_003C_003E8__locals24._posList = new List<Vector3>();
					CS_0024_003C_003E8__locals24._previousPos = Vector3.zero;
					_003Ccount_003E5__2 = 0;
					_003CcalCount_003E5__3 = 0;
					_003C_003E2__current = new SI.WaitForSeconds(CS_0024_003C_003E8__locals24.startDelay);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					while (_003Ccount_003E5__2 < CS_0024_003C_003E8__locals24.missileCount)
					{
						if (_003CcalCount_003E5__3 > 2)
						{
							CS_0024_003C_003E8__locals24._posList.Add(CS_0024_003C_003E8__locals24._previousPos);
							_003Ccount_003E5__2++;
							_003CcalCount_003E5__3 = 0;
							continue;
						}
						Vector3 normalized = global::UnityEngine.Random.insideUnitSphere.Flattened().normalized;
						Vector3 end = CS_0024_003C_003E8__locals24.info.caster.agentPosition + normalized * global::UnityEngine.Random.Range(CS_0024_003C_003E8__locals24.minRange, CS_0024_003C_003E8__locals24.maxRange);
						end = Dew.GetValidAgentDestination_Closest(CS_0024_003C_003E8__locals24.info.caster.agentPosition, end);
						if (Vector3.Distance(end, CS_0024_003C_003E8__locals24._previousPos) < CS_0024_003C_003E8__locals24.gapForEachInstance && _003Ccount_003E5__2 != 0)
						{
							_003CcalCount_003E5__3++;
							continue;
						}
						CS_0024_003C_003E8__locals24._posList.Add(end);
						_003Ccount_003E5__2++;
						_003CcalCount_003E5__3 = 0;
						CS_0024_003C_003E8__locals24._previousPos = end;
					}
					_003C_003E7__wrap3 = CS_0024_003C_003E8__locals24._posList.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					CS_0024_003C_003E8__locals24.CreateAbilityInstance<Ai_Mon_Ink_DivineAnimal_Missile_Sub>(_003Cpos_003E5__5, null, CS_0024_003C_003E8__locals24.info, delegate
					{
					});
					_003Cpos_003E5__5 = default(Vector3);
					break;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					_003Cpos_003E5__5 = _003C_003E7__wrap3.Current;
					CS_0024_003C_003E8__locals24.FxPlayNewNetworked(CS_0024_003C_003E8__locals24.telegraph, _003Cpos_003E5__5, Quaternion.identity);
					_003C_003E2__current = new SI.WaitForSeconds(CS_0024_003C_003E8__locals24.delayForEachInstance);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = default(List<Vector3>.Enumerator);
				CS_0024_003C_003E8__locals24.Destroy();
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public int missileCount;

	public float startDelay;

	public float minRange;

	public float maxRange;

	public float gapForEachInstance;

	public float delayForEachInstance;

	public GameObject telegraph;

	public float delayForTelegraph;

	private Vector3 _previousPos;

	private List<Vector3> _posList;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__10))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
