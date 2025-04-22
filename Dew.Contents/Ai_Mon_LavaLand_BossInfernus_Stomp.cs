using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_Stomp : InstantDamageInstance
{
	[Serializable]
	public struct Pattern
	{
		public float[] eruptAngles;

		public bool useLocalRotation;
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_BossInfernus_Stomp _003C_003E4__this;

		private float[] _003CeruptAngles_003E5__2;

		private bool[] _003CisFinished_003E5__3;

		private float _003CstartAngle_003E5__4;

		private Vector3 _003Ccenter_003E5__5;

		private int _003Cwave_003E5__6;

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
		public _003COnCreateSequenced_003Ed__8(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_BossInfernus_Stomp ai_Mon_LavaLand_BossInfernus_Stomp = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ai_Mon_LavaLand_BossInfernus_Stomp._003C_003En__0();
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_LavaLand_BossInfernus_Stomp.isServer)
				{
					return false;
				}
				int num2 = global::UnityEngine.Random.Range(0, ai_Mon_LavaLand_BossInfernus_Stomp.patterns.Length);
				_003CeruptAngles_003E5__2 = ai_Mon_LavaLand_BossInfernus_Stomp.patterns[num2].eruptAngles;
				_003CisFinished_003E5__3 = new bool[_003CeruptAngles_003E5__2.Length];
				_003CstartAngle_003E5__4 = (ai_Mon_LavaLand_BossInfernus_Stomp.patterns[num2].useLocalRotation ? ai_Mon_LavaLand_BossInfernus_Stomp.info.caster.rotation.eulerAngles.y : 0f);
				_003Ccenter_003E5__5 = Dew.GetPositionOnGround(ai_Mon_LavaLand_BossInfernus_Stomp.info.caster.agentPosition);
				_003Cwave_003E5__6 = 0;
				break;
			}
			case 2:
				_003C_003E1__state = -1;
				_003Cwave_003E5__6++;
				break;
			}
			if (_003Cwave_003E5__6 < ai_Mon_LavaLand_BossInfernus_Stomp.maxSpawnCount)
			{
				for (int i = 0; i < _003CeruptAngles_003E5__2.Length; i++)
				{
					if (!_003CisFinished_003E5__3[i])
					{
						float num3 = ai_Mon_LavaLand_BossInfernus_Stomp.startDistance + ai_Mon_LavaLand_BossInfernus_Stomp.stepDistance * (float)_003Cwave_003E5__6;
						Vector3 position = _003Ccenter_003E5__5 + Quaternion.Euler(0f, _003CstartAngle_003E5__4 + _003CeruptAngles_003E5__2[i], 0f) * Vector3.forward * num3;
						position = Dew.GetPositionOnGround(position);
						if (Mathf.Abs(position.y - _003Ccenter_003E5__5.y) > 2f)
						{
							_003CisFinished_003E5__3[i] = true;
						}
						else
						{
							ai_Mon_LavaLand_BossInfernus_Stomp.CreateAbilityInstance<Ai_Mon_LavaLand_BossInfernus_Stomp_Eruption>(position, Quaternion.LookRotation(position - ai_Mon_LavaLand_BossInfernus_Stomp.info.caster.agentPosition), new CastInfo(ai_Mon_LavaLand_BossInfernus_Stomp.info.caster));
						}
					}
				}
				if (!_003CisFinished_003E5__3.All((bool b) => b))
				{
					ai_Mon_LavaLand_BossInfernus_Stomp.FxPlayNewNetworked(ai_Mon_LavaLand_BossInfernus_Stomp.fxPerWave);
					_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_BossInfernus_Stomp.waveInterval);
					_003C_003E1__state = 2;
					return true;
				}
			}
			ai_Mon_LavaLand_BossInfernus_Stomp.Destroy();
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

	public Pattern[] patterns;

	public float startDistance;

	public int maxSpawnCount;

	public float stepDistance;

	public GameObject fxPerWave;

	public float waveInterval;

	public float stunDuration;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__8))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnHit(Entity entity)
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
