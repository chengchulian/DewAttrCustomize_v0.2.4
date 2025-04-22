using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomMod_VeilOfDark : RoomModifierBase
{
	[CompilerGenerated]
	private sealed class _003CCreateVeilRoutine_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RoomSection s;

		public RoomMod_VeilOfDark _003C_003E4__this;

		private int _003CspawnCount_003E5__2;

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
		public _003CCreateVeilRoutine_003Ed__8(int _003C_003E1__state)
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
			RoomMod_VeilOfDark CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_0029;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0029;
				}
				IL_0029:
				_003CspawnCount_003E5__2 = Mathf.RoundToInt(s.area / CS_0024_003C_003E8__locals8.spawnDensity);
				_003Ci_003E5__3 = 0;
				break;
			}
			if (_003Ci_003E5__3 < _003CspawnCount_003E5__2)
			{
				Vector3 position = Vector3.zero;
				List<Vector3> list = new List<Vector3>();
				int num2 = 0;
				while (num2 < CS_0024_003C_003E8__locals8._maxCalCount)
				{
					bool flag = false;
					Vector3 anyRandomNode = s.GetAnyRandomNode();
					foreach (Vector3 item in list)
					{
						if (Vector3.Distance(anyRandomNode, item) <= CS_0024_003C_003E8__locals8.preventSpawnDistance)
						{
							break;
						}
						flag = true;
					}
					num2++;
					if (flag || list.Count <= 0)
					{
						list.Add(anyRandomNode);
						position = anyRandomNode;
						break;
					}
				}
				CS_0024_003C_003E8__locals8.CreateAbilityInstance<Ai_VeilOfDark>(position, null, default(CastInfo), delegate
				{
				});
				_003C_003E2__current = new WaitForSeconds(global::UnityEngine.Random.Range(CS_0024_003C_003E8__locals8.interval.x, CS_0024_003C_003E8__locals8.interval.y));
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E2__current = new WaitForSeconds(global::UnityEngine.Random.Range(CS_0024_003C_003E8__locals8.delay.x, CS_0024_003C_003E8__locals8.delay.y));
			_003C_003E1__state = 2;
			return true;
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

	public float spawnDensity;

	public float preventSpawnDistance;

	public Vector2 interval;

	public Vector2 delay;

	public Vector2 range;

	private int _maxCalCount;

	public override void OnStartServer()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	[IteratorStateMachine(typeof(_003CCreateVeilRoutine_003Ed__8))]
	private IEnumerator CreateVeilRoutine(RoomSection s)
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	private void MirrorProcessed()
	{
	}
}
