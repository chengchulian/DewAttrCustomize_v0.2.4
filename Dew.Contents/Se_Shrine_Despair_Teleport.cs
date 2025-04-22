using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

public class Se_Shrine_Despair_Teleport : StatusEffect
{
	[CompilerGenerated]
	private sealed class _003COnCreateSequenced_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Se_Shrine_Despair_Teleport _003C_003E4__this;

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
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Se_Shrine_Despair_Teleport se_Shrine_Despair_Teleport = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				se_Shrine_Despair_Teleport._entTransform = se_Shrine_Despair_Teleport.info.caster.Visual.GetNewTransformModifier();
				if (!se_Shrine_Despair_Teleport.isServer)
				{
					return false;
				}
				se_Shrine_Despair_Teleport.victim.Status.DisableSectionTriggering();
				se_Shrine_Despair_Teleport.DoUncollidable();
				se_Shrine_Despair_Teleport.DoInvulnerable();
				se_Shrine_Despair_Teleport.DoInvisible();
				se_Shrine_Despair_Teleport.DestroyOnDestroy(se_Shrine_Despair_Teleport.victim);
				se_Shrine_Despair_Teleport._displaceDuration = global::UnityEngine.Random.Range(se_Shrine_Despair_Teleport.displaceDuration.x, se_Shrine_Despair_Teleport.displaceDuration.y);
				se_Shrine_Despair_Teleport.victim.Control.StartDaze(se_Shrine_Despair_Teleport.delay + se_Shrine_Despair_Teleport._displaceDuration);
				_003C_003E2__current = new SI.WaitForSeconds(se_Shrine_Despair_Teleport.delay);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				se_Shrine_Despair_Teleport.FxPlayNetworked(se_Shrine_Despair_Teleport.displacingEffect, se_Shrine_Despair_Teleport.victim);
				Vector3 positionOnGround = Dew.GetPositionOnGround(se_Shrine_Despair_Teleport.info.point);
				se_Shrine_Despair_Teleport.RpcDisplacement(positionOnGround);
				se_Shrine_Despair_Teleport.victim.Control.StartDisplacement(new DispByDestination
				{
					affectedByMovementSpeed = false,
					canGoOverTerrain = true,
					destination = positionOnGround,
					duration = se_Shrine_Despair_Teleport._displaceDuration,
					ease = se_Shrine_Despair_Teleport.ease,
					isCanceledByCC = false,
					isFriendly = true,
					rotateForward = true,
					onCancel = se_Shrine_Despair_Teleport.DestroyIfActive,
					onFinish = se_Shrine_Despair_Teleport.DestroyIfActive
				});
				if (se_Shrine_Despair_Teleport.turnOffRenderer)
				{
					se_Shrine_Despair_Teleport.victim.Visual.DisableRenderers();
				}
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

	public float delay;

	public GameObject displacingEffect;

	public Vector2 displaceDuration;

	public DewEase ease;

	public bool turnOffRenderer;

	public float heightMultiplier;

	public float minHeight;

	private float _displaceDuration;

	private EntityTransformModifier _entTransform;

	[SerializeField]
	private AnimationCurve _heightCurve;

	[IteratorStateMachine(typeof(_003COnCreateSequenced_003Ed__10))]
	protected override IEnumerator OnCreateSequenced()
	{
		/*Error: Method body consists only of 'ret', but nothing is being returned. Decompiled assembly might be a reference assembly.*/;
	}

	[ClientRpc]
	private void RpcDisplacement(Vector3 dest)
	{
	}

	protected override void ActiveLogicUpdate(float dt)
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void MirrorProcessed()
	{
	}

	protected void UserCode_RpcDisplacement__Vector3(Vector3 dest)
	{
	}

	protected static void InvokeUserCode_RpcDisplacement__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
	}

	static Se_Shrine_Despair_Teleport()
	{
	}
}
