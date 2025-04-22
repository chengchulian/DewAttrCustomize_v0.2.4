using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class Ai_Mon_LavaLand_BossInfernus_BreathFire : AbilityInstance
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Ai_Mon_LavaLand_BossInfernus_BreathFire _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003COnCreateSequenced_003Ed__7(int _003C_003E1__state)
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
			Ai_Mon_LavaLand_BossInfernus_BreathFire ai_Mon_LavaLand_BossInfernus_BreathFire = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ai_Mon_LavaLand_BossInfernus_BreathFire.isServer)
				{
					return false;
				}
				Channel channel = new Channel
				{
					duration = (float)ai_Mon_LavaLand_BossInfernus_BreathFire.spawns * ai_Mon_LavaLand_BossInfernus_BreathFire.spawnInterval,
					blockedActions = Channel.BlockedAction.Everything,
					onCancel = ai_Mon_LavaLand_BossInfernus_BreathFire.Destroy,
					onComplete = ai_Mon_LavaLand_BossInfernus_BreathFire.Destroy
				};
				ai_Mon_LavaLand_BossInfernus_BreathFire.info.caster.Control.StartChannel(channel);
				_003Ci_003E5__2 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < ai_Mon_LavaLand_BossInfernus_BreathFire.spawns)
			{
				ai_Mon_LavaLand_BossInfernus_BreathFire._normalizedElapsedTime = (float)_003Ci_003E5__2 / (float)ai_Mon_LavaLand_BossInfernus_BreathFire.spawns;
				ai_Mon_LavaLand_BossInfernus_BreathFire.CreateAbilityInstance<Ai_Mon_LavaLand_BossInfernus_BreathFire_Projectile>(ai_Mon_LavaLand_BossInfernus_BreathFire.info.caster.position, Quaternion.identity, new CastInfo(ai_Mon_LavaLand_BossInfernus_BreathFire.info.caster, CastInfo.GetAngle(ai_Mon_LavaLand_BossInfernus_BreathFire._syncedRotation)));
				_003C_003E2__current = new SI.WaitForSeconds(ai_Mon_LavaLand_BossInfernus_BreathFire.spawnInterval);
				_003C_003E1__state = 1;
				return true;
			}
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

	public int spawns;

	public float spawnInterval;

	public Vector2 rotateSpeedRange;

	public float endDaze;

	[SyncVar]
	private Quaternion _syncedRotation;

	private float _normalizedElapsedTime;

	public Quaternion Network_syncedRotation
	{
		get
		{
			/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
		}
		[param: In]
		set
		{
		}
	}

	protected override void OnCreate()
	{
	}

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__7))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	protected override void OnDestroyActor()
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void ActiveFrameUpdate()
	{
	}

	private void MirrorProcessed()
	{
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
	}
}
