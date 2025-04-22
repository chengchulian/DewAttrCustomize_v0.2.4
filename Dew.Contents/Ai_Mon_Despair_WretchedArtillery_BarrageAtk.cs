using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ai_Mon_Despair_WretchedArtillery_BarrageAtk : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public Vector3 pos;

		public bool first;

		public Ai_Mon_Despair_WretchedArtillery_BarrageAtk _003C_003E4__this;

		internal void _003COnCreateSequenced_003Eb__0(Ai_Mon_Despair_WretchedArtillery_BarrageAtk_Missile a)
		{
			a.initialSpeed = Vector3.Distance(_003C_003E4__this.info.caster.position, pos) / global::UnityEngine.Random.Range(_003C_003E4__this.landTime.x, _003C_003E4__this.landTime.y);
			a.targetSpeed = a.initialSpeed;
			a.acceleration = 0f;
			a.isFirstMissile = first;
		}
	}

	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_Despair_WretchedArtillery_BarrageAtk _003C_003E4__this;

		private Vector3 _003Ccenter_003E5__2;

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
			Ai_Mon_Despair_WretchedArtillery_BarrageAtk ai_Mon_Despair_WretchedArtillery_BarrageAtk = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ai_Mon_Despair_WretchedArtillery_BarrageAtk.isServer)
				{
					return false;
				}
				ai_Mon_Despair_WretchedArtillery_BarrageAtk.DestroyOnDeath(ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster);
				_003Ccenter_003E5__2 = ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.point;
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < ai_Mon_Despair_WretchedArtillery_BarrageAtk.waves)
			{
				ai_Mon_Despair_WretchedArtillery_BarrageAtk.FxPlayNewNetworked(ai_Mon_Despair_WretchedArtillery_BarrageAtk.perWaveEffect, ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster);
				Vector3 vector = ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster.position - _003Ccenter_003E5__2;
				if (vector.sqrMagnitude < ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeOfCenterFromSelf * ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeOfCenterFromSelf)
				{
					vector = vector.normalized * ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeOfCenterFromSelf;
					_003Ccenter_003E5__2 = ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster.position + vector;
				}
				for (int i = 0; i < ai_Mon_Despair_WretchedArtillery_BarrageAtk.shootCountPerWave; i++)
				{
					_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass8_0
					{
						_003C_003E4__this = ai_Mon_Despair_WretchedArtillery_BarrageAtk,
						pos = ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.point + global::UnityEngine.Random.onUnitSphere.Flattened().normalized * (global::UnityEngine.Random.value * ai_Mon_Despair_WretchedArtillery_BarrageAtk.radius)
					};
					Vector3 vector2 = (CS_0024_003C_003E8__locals11.pos - ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster.position).Flattened();
					if (vector2.sqrMagnitude < ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeFromSelf * ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeFromSelf)
					{
						vector2 = vector2.normalized * ai_Mon_Despair_WretchedArtillery_BarrageAtk.minRangeFromSelf;
						CS_0024_003C_003E8__locals11.pos = ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster.position + vector2;
					}
					CS_0024_003C_003E8__locals11.pos = Dew.GetPositionOnGround(CS_0024_003C_003E8__locals11.pos);
					CS_0024_003C_003E8__locals11.first = _003Ci_003E5__3 == 0 && i == 0;
					ai_Mon_Despair_WretchedArtillery_BarrageAtk.CreateAbilityInstance(ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster.position, null, new CastInfo(ai_Mon_Despair_WretchedArtillery_BarrageAtk.info.caster, CS_0024_003C_003E8__locals11.pos), delegate(Ai_Mon_Despair_WretchedArtillery_BarrageAtk_Missile a)
					{
						a.initialSpeed = Vector3.Distance(CS_0024_003C_003E8__locals11._003C_003E4__this.info.caster.position, CS_0024_003C_003E8__locals11.pos) / global::UnityEngine.Random.Range(CS_0024_003C_003E8__locals11._003C_003E4__this.landTime.x, CS_0024_003C_003E8__locals11._003C_003E4__this.landTime.y);
						a.targetSpeed = a.initialSpeed;
						a.acceleration = 0f;
						a.isFirstMissile = CS_0024_003C_003E8__locals11.first;
					});
				}
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_Despair_WretchedArtillery_BarrageAtk.waveInterval);
				_003C_003E1__state = 1;
				return true;
			}
			ai_Mon_Despair_WretchedArtillery_BarrageAtk.Destroy();
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

	public GameObject perWaveEffect;

	public int waves;

	public int shootCountPerWave;

	public float waveInterval;

	public float radius;

	public Vector2 landTime;

	public float minRangeFromSelf;

	public float minRangeOfCenterFromSelf;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__8))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void ActiveFrameUpdate()
	{
	}

	private void MirrorProcessed()
	{
	}
}
